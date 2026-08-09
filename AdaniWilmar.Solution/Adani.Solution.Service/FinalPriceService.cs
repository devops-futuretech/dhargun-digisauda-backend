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
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using Adani.Solution.DTO.Common;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using Adani.Solution.DTO.Enums;
using System.Data.SqlClient;
using Dapper;
using OfficeOpenXml;
using System.Configuration;
using System.IO;

namespace Adani.Solution.Service
{
    public interface IFinalPriceService
    {
        //Unused method
        ResultDto SkuFinalpriceListForAdmin(SkuFinalpriceListInputDto inputDto);
        ResultDto SkuFinalpriceListForAdminUpdated(SkuFinalpriceListInputDto inputDto);
        void SkuFinalpriceListForAdminUpdatedNew(SkuFinalpriceListInputDto inputDto);

        //Old final price screen
        ResultDto SkuFinalpriceListForAdminNew(SkuFinalpriceListInputDto inputDto);
        ResultDto SaveTraditionalProcessFinalPrice(SaveFinalPricngInputDto inputDto);
        ResultDto SaveReverseAuctionFinalPrice(SaveFinalPricngInputDto inputDto);

        //New final price screen
        ResultDto SaveFinalPrice(SkuFinalpriceListInputDto inputDto);
        ResultDto PublishFinalPrice(FinalPricePublishDto inputDto);
        ResultDto GetPublishedPriceDetails(PricePublishInputDto inputDto);
        ResultDto GetSkuFinalPriceList(FinalPricePublishDto inputDto);
        ResultDto GetPublishedPriceErrorDetails(PricePublishInputDto inputDto);

        ResultDto PricingDataBackup(LoginUserIdDto inputDto);

        //New Final Price
        //ResultDto SavePriceGenerate(FinalPriceGenerateInputDto inputDto);
        ResultDto GetPriceGenerates(PricePublishInputDto inputDto);
        //ResultDto GetPriceGenerateList(PricePublistInputDataDto inputDto);
        ResultDto GetPriceGenerateDetails(PricePublishInputDto inputDto);
        ResultDto StateBasePublishFinalPrice(FinalPricePublishDto inputDto);
        ResultDto GetStateBaseFinalPriceList(FinalPricePublishDto inputDto);
        ResultDto GetStateBasePublishedPriceErrorDetails(PricePublishInputDto inputDto);
        ResultDto FinalPriceBulkPublish(FinalPricePublishDto inputDto);

        //RA2.0 Final Price
        ResultDto RaFinalPricePriceGenerate(RaFinalPriceGenerateInputDto inputDto);
        ResultDto RaGetFinalPriceGenerates(RaPricePublishInputDto inputDto);
        ResultDto RaGetFinalPriceGenerateDetails(RaPricePublishInputDto inputDto);

        ResultDto ZoneBasedFinalPriceDownload(PriceDownloadInputDto inputDto);
        ResultDto DownloadPriceGenerateSuccessList(PriceDownloadInputDto inputDto);
        ResultDto DownloadPriceGenerateErrorList(PriceDownloadInputDto inputDto);

    }

    public class FinalPriceService : IFinalPriceService
    {

        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Final Price Service");
        private const string ServiceName = "Final Price Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public FinalPriceService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
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

        #region SkuFinalprice

        /// Method to calculate final price
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto SkuFinalpriceListForAdmin(SkuFinalpriceListInputDto inputDto)
        {
            _methodName = "SkuFinalpriceListForAdmin";
            var resultDto = new ResultDto();
            var outputDto = new List<SkuFinalpriceListOutputDto>();
            var errorList = new List<string>();
            bool isError = false;
            try
            {
                var skuList = new List<Sku>();


                skuList = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId && _.PackGroupId == inputDto.OilPackingTypeId &&
                                   _.IsActive).ToList();


                if (skuList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                foreach (var sku in skuList)
                {
                    var validSku = true;
                    var errorMessage = sku.SkuName + " : " + Constants.MissingSkuRequiredField;

                    if (sku.Quantity <= 0)
                    {
                        errorMessage = Constants.BindErrorMessage(Constants.MissingSkuQuantityField, errorMessage);
                        validSku = false;
                    }

                    if (sku.UomId == null || sku.UomId <= 0)
                    {
                        errorMessage = Constants.BindErrorMessage(Constants.MissingSkuPackSizeQuantityField, errorMessage);
                        validSku = false;
                    }

                    //var rasoiOilTypeId = _emamiContext.Configurations.FirstOrDefault(f => f.Key == Constants.RasoiOilTypeId)?.Value;
                    //var rasoiOilTypeIds = string.IsNullOrEmpty(rasoiOilTypeId) ? new List<long>() : rasoiOilTypeId.Split(',').Select(Int64.Parse).ToList();
                    //var rasoiOilTypeIds = _emamiContext.OilTypes.Where(w => w.IsRasoi).Select(s => s.Id).ToList();

                    //if (sku.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat || (sku.VerticalId == (int)DTO.Enums.Vertical.Hbc && (rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(sku.OilTypeId.GetValueOrDefault()))))
                    {
                        if (sku.ProcessCost <= 0)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.MissingSkuProcessCostField, errorMessage);
                            validSku = false;
                        }

                        //if (!_emamiContext.SkuIngrediant.AsNoTracking().Any(_ => _.SkuId == sku.Id))
                        //{
                        //    errorMessage = Constants.BindErrorMessage(Constants.SkuIngredientNotAdded, errorMessage);
                        //    validSku = false;
                        //}
                    }

                    if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                    {
                        errorMessage = Constants.BindErrorMessage(Constants.MissingSkuUom2Field, errorMessage);
                        validSku = false;
                    }

                    if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                    {
                        errorMessage = Constants.BindErrorMessage(Constants.MissingSkuUom3Field, errorMessage);
                        validSku = false;
                    }

                    if (validSku)
                    {
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        //var isExists = _emamiContext.Pricing.AsNoTracking().Any(s => s.SaudaBookingTypeId == inputDto.SaudaBookingTypeId && s.SkuId == sku.Id && DbFunctions.TruncateTime(s.BiddingDate) == currentDate.Date);
                        //if (!isExists)
                        //{
                        inputDto.SkuId = sku.Id;
                        var finalPriceResultList = SkuFinalPriceCalculation(inputDto);
                        foreach (var finalPriceResult in finalPriceResultList)
                        {
                            if (finalPriceResult.IsSuccess)
                            {
                                outputDto.Add((SkuFinalpriceListOutputDto)finalPriceResult.SuccessDto.Response);
                            }
                            else
                            {
                                isError = true;
                                errorList.Add(finalPriceResult.ErrorDto.Message + "<br>");

                                //Restrict to add in Grid if any of the Cost Price is Zero
                                //outputDto.Add((SkuFinalpriceListOutputDto)finalPriceResult.SuccessDto.Response);
                            }
                        }
                        //}
                    }
                    else
                    {
                        isError = true;
                        errorList.Add(errorMessage + "<br>");
                    }


                }
                if (isError)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = errorList;
                    resultDto.SuccessDto.Response = outputDto;
                }
                else
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = outputDto;
                }
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// Method to calculate final price
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public List<ResultDto> SkuFinalPriceCalculation(SkuFinalpriceListInputDto inputDto)
        {
            _methodName = "SkuFinalPriceCalculation";
            var resultDtoList = new List<ResultDto>();


            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var skuId = inputDto.SkuId;
                //var userId = inputDto.DealerId;
                var incoTermsId = 0L;
                var plantId = inputDto.PlantId;
                var depotId = inputDto.DepotId;
                var verticalId = 0L;
                var oilTypeId = 0L;
                var transportModeId = 0L;
                var oilPackingTypeId = 0L;
                var cityId = inputDto.CityId;
                var stateId = inputDto.StateId;
                var uomId = 0L;
                var freightRouteId = inputDto.FreightRouteId;

                var litreConversion = (decimal)0;
                var quantity = (decimal)0;
                var materialCost = (decimal)0;
                var packingCost = (decimal)0;
                var primaryFrieght = (decimal)0;
                var secondaryFrieght = (decimal)0;
                var depoCost = (decimal)0;
                var detentionCost = (decimal)0;
                var honeycombCost = (decimal)0;
                var marginCost = (decimal)0;
                var cushionMarginCost = (decimal)0;
                var schemeCostRecovery = (decimal)0;
                var raMarginCost = (decimal)0;
                var discount = (decimal)0;
                var premium = (decimal)0;
                var secondaryFrieghtForPlant = (decimal)0;

                var exPlantPrice = (decimal)0;
                var forPlantPrice = (decimal)0;
                var exDepotPrice = (decimal)0;
                var forDepotPrice = (decimal)0;
                var exRakePrice = (decimal)0;
                var forRakePrice = (decimal)0;

                var finalPrice = (decimal)0;
                bool isError = false;


                var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                //if (skuContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}

                oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                uomId = Convert.ToInt64(skuContext.UomId);
                quantity = skuContext.Quantity;

                var errorMessage = skuContext.SkuName + " : " + Constants.DataMissingToCalculate;

                var noofPiecesperCase = (decimal)0; ;
                var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                if (skuUomContext != null)
                {
                   // noofPiecesperCase = skuUomContext.ConversionFactor;
                }

                var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                //if (oilTypeContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
                verticalId = oilTypeContext.DivisionId;
               // litreConversion = oilTypeContext.LitreConversion;




                //Material Cost                
                //var rasoiOilTypeIds = _emamiContext.OilTypes.Where(w => w.IsRasoi).Select(s => s.Id).ToList();

                //if (verticalId == (int)DTO.Enums.Vertical.Hbc && ((rasoiOilTypeIds == null || !rasoiOilTypeIds.Any()) || !rasoiOilTypeIds.Contains(oilTypeId)))
                {
                    var materialCostContext = _emamiContext.MaterialCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                    if (materialCostContext != null)
                    {
                        materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                        materialCost = noofPiecesperCase * materialCost;
                    }
                    else
                    {
                        isError = true;
                        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost + " - ", errorMessage);
                    }
                }

                //Packing Cost
                var packingCostContext = _emamiContext.PackingCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.SkuId == skuId);
                if (packingCostContext != null)
                {
                    packingCost = packingCostContext.SalesPackingCost;
                    /*var noofPiecesperMt = (decimal)0; ;
                    var skuUomMtContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                    if (skuUomMtContext != null)
                    {
                        noofPiecesperMt = skuUomMtContext.ConversionFactor;
                    }

                    packingCost = (packingCostContext.SalesPackingCost / noofPiecesperMt) * noofPiecesperCase;
                    */
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToPackingCost + " - ", errorMessage);
                }



                //Depo Cost
                var depoCostContext = _emamiContext.DepotCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                if (depoCostContext != null)
                {
                    depoCost = _resultService.GetSkuQuanityRate(uomId, quantity, depoCostContext.RatePerMt, litreConversion);
                    depoCost = noofPiecesperCase * depoCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToDepoCost + " - ", errorMessage);
                }

                //Detention Cost
                var detentionCostContext = _emamiContext.DetentionCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                if (detentionCostContext != null)
                {
                    detentionCost = _resultService.GetSkuQuanityRate(uomId, quantity, detentionCostContext.RatePerMt, litreConversion);
                    detentionCost = noofPiecesperCase * detentionCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToDetentionCost + " - ", errorMessage);
                }



                //Margin Cost
                var marginCostContext = _emamiContext.ProfitMargins.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                _.OilPackingTypeId == oilPackingTypeId);
                if (marginCostContext != null)
                {
                    marginCost = _resultService.GetSkuQuanityRate(uomId, quantity, marginCostContext.RatePerMt, litreConversion);
                    marginCost = noofPiecesperCase * marginCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToMarginCost + " - ", errorMessage);
                }

                //Cushion Margin Cost
                var cushionMarginCostContext = _emamiContext.CushionMargins.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                _.OilPackingTypeId == oilPackingTypeId);
                if (cushionMarginCostContext != null)
                {
                    cushionMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                    cushionMarginCost = noofPiecesperCase * cushionMarginCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToCushionMarginCost + " - ", errorMessage);
                }

                //Scheme Cost Recovery
                var schemeCostContext = _emamiContext.SchemeCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PackGroupId == inputDto.OilPackingTypeId && _.CityId == cityId);
                if (schemeCostContext != null)
                {
                    schemeCostRecovery = _resultService.GetSkuQuanityRate(uomId, quantity, schemeCostContext.RatePerMt, litreConversion);
                    schemeCostRecovery = noofPiecesperCase * schemeCostRecovery;
                }
                //else
                //{
                //    isError = true;
                //    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSchemeCost + " - ", errorMessage);
                //}

                var formulationCost = (decimal)0;
                if (verticalId == (int)DTO.Enums.Division.SpecialityFat || (verticalId == (int)DTO.Enums.Division.Hbc)) /*rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(oilTypeId))*/
                {

                    //var skuIngredientList = _emamiContext.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId).ToList();
                    //foreach (var skuIngredient in skuIngredientList)
                    //{
                    //    var ingredientCost = _emamiContext.IngredientCost.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                    //    if (ingredientCost != null)
                    //    {

                    //        //var percentageValue = (skuIngredient.Percentage / 100);
                    //        var oneKgIngredientCost = (ingredientCost.LooseOilRate * skuIngredient.Percentage) / 100;
                    //        formulationCost = formulationCost + oneKgIngredientCost;
                    //    }
                    //    else
                    //    {
                    //        isError = true;
                    //        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost + " - ", errorMessage);
                    //    }
                    //}

                    var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;
                    materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, specialityFatMaterialCost, litreConversion);
                    materialCost = noofPiecesperCase * materialCost;

                    formulationCost = _resultService.GetSkuQuanityRate(uomId, quantity, formulationCost, litreConversion);
                    formulationCost = noofPiecesperCase * formulationCost;
                    //outputDto.IngredientCost = formulationCost;

                    finalPrice = materialCost + ((packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                     honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                }
                else
                {

                    finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                         honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                }

                foreach (var transportId in inputDto.TransportModeId)
                {
                    var transportMode = string.Empty;
                    var loadCapacityContextList = _emamiContext.LoadCapacityConversion.AsNoTracking()
                        .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.SkuId == skuId && _.TransportModeId == transportId &&
                   _.DivisionId == verticalId).ToList();


                    if (loadCapacityContextList != null && loadCapacityContextList.Any())
                    {
                        foreach (var loadCapacityItem in loadCapacityContextList)
                        {
                            var transportModeContext = _emamiContext.TransportModes.AsNoTracking().FirstOrDefault(_ => _.Id == loadCapacityItem.TransportModeId);

                            if (transportModeContext != null)
                                transportMode = transportModeContext.Name;

                            var resultDto = new ResultDto();
                            var outputDto = new SkuFinalpriceListOutputDto();
                            outputDto.IngredientCost = formulationCost;
                            outputDto.SkuId = skuContext.Id;
                            outputDto.SkuName = skuContext.SkuName;
                            outputDto.TransportModeId = transportId;

                            var loadCapacity = loadCapacityItem.LoadCapacity;
                            var loadQuantityCase = loadCapacityItem.LoadQuantity;

                            //var loadCapacity = (decimal)0;
                            //var loadQuantityCase = (decimal)0;
                            ////Load Capacity
                            //var loadCapacityContext = _emamiContext.LoadCapacityConversion.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.TransportModeId == transportId &&
                            //_.VerticalId == verticalId);
                            //if (loadCapacityContext != null)
                            //{
                            //    loadCapacity = loadCapacityContext.LoadCapacity;
                            //    loadQuantityCase = loadCapacityContext.LoadQuantity;
                            //}
                            //else
                            //{
                            //    isError = true;
                            //    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToLoadCapacity + " - ", errorMessage);
                            //}

                            outputDto.LoadQuantity = loadCapacity;

                            //Primary Frieght
                            var primaryFrieghtContext = _emamiContext.PrimaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                            _.VerticalId == verticalId && _.TransportModeId == transportId);
                            if (primaryFrieghtContext != null)
                            {
                                primaryFrieght = primaryFrieghtContext.SalesFreight;
                                primaryFrieght = (primaryFrieght / loadQuantityCase) * 1;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToPrimaryFrieght + " for " + transportMode + " " + loadCapacity + "MT" + " - ", errorMessage);
                            }


                            //Secondary Frieght
                            var secondaryFrieghtContext = _emamiContext.SecondaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == depotId
                        && _.FreightRouteId == freightRouteId
                        && _.VerticalId == verticalId && _.TransportModeId == transportId && _.Capacity == loadCapacity);
                            if (secondaryFrieghtContext != null)
                            {
                                secondaryFrieght = secondaryFrieghtContext.SalesFreight;
                                secondaryFrieght = (secondaryFrieght / loadQuantityCase) * 1;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSecondaryFrieght + " for " + transportMode + " " + loadCapacity + "MT" + " - ", errorMessage);
                            }

                            //Secondary Frieght for plant
                            var secondaryFrieghtContextForPlant = _emamiContext.SecondaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == plantId
                        && _.FreightRouteId == freightRouteId
                        && _.VerticalId == verticalId && _.TransportModeId == transportId && _.Capacity == loadCapacity);
                            if (secondaryFrieghtContextForPlant != null)
                            {
                                secondaryFrieghtForPlant = secondaryFrieghtContextForPlant.SalesFreight;
                                secondaryFrieghtForPlant = (secondaryFrieghtForPlant / loadQuantityCase) * 1;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSecondaryFrieghtForPlant + " for " + transportMode + " " + loadCapacity + "MT" + " - ", errorMessage);
                            }


                            /*
                            //Primary Frieght
                            var primaryFrieghtContext = _emamiContext.PrimaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                            _.VerticalId == verticalId && _.TransportModeId == transportId);
                            if (primaryFrieghtContext != null)
                            {
                                primaryFrieght = _resultService.GetSkuQuanityRate(uomId, quantity, primaryFrieghtContext.SalesFreight, litreConversion);
                                primaryFrieght = noofPiecesperCase * primaryFrieght;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToPrimaryFrieght + " - ", errorMessage);
                            }

                            //Secondary Frieght
                            var secondaryFrieghtContext = _emamiContext.SecondaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == depotId
                        && _.FreightRouteId == freightRouteId
                        && _.VerticalId == verticalId && _.TransportModeId == transportId);
                            if (secondaryFrieghtContext != null)
                            {
                                secondaryFrieght = _resultService.GetSkuQuanityRate(uomId, quantity, secondaryFrieghtContext.SalesFreight, litreConversion);
                                secondaryFrieght = noofPiecesperCase * secondaryFrieght;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSecondaryFrieght + " - ", errorMessage);
                            }

                            //Secondary Frieght for plant
                            var secondaryFrieghtContextForPlant = _emamiContext.SecondaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == plantId
                        && _.FreightRouteId == freightRouteId
                        && _.VerticalId == verticalId && _.TransportModeId == transportId);
                            if (secondaryFrieghtContextForPlant != null)
                            {
                                secondaryFrieghtForPlant = _resultService.GetSkuQuanityRate(uomId, quantity, secondaryFrieghtContextForPlant.SalesFreight, litreConversion);
                                secondaryFrieghtForPlant = noofPiecesperCase * secondaryFrieghtForPlant;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSecondaryFrieghtForPlant + " - ", errorMessage);
                            }
                            */

                            //Honeycomb Cost
                            var honeycombCostContext = _emamiContext.HoneycombCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.StateId == stateId &&
                        _.SkuId == skuId && _.TransportModeId == transportId);
                            if (honeycombCostContext != null)
                            {
                                honeycombCost = _resultService.GetSkuQuanityRate(uomId, quantity, honeycombCostContext.RatePerMt, litreConversion);
                                honeycombCost = noofPiecesperCase * honeycombCost;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToHoneyCombCost + " - ", errorMessage);
                            }

                            finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                          honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;


                            exDepotPrice = ((materialCost + packingCost + primaryFrieght + depoCost + detentionCost +
                                                     marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                            exPlantPrice = ((materialCost + packingCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                            forPlantPrice = ((materialCost + packingCost + secondaryFrieghtForPlant +
                                              honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                            exRakePrice = ((materialCost + packingCost + primaryFrieght +
                                             honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;



                            //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                            //{
                            //    //RA Margin Cost
                            //    var raMarginCostContext = _emamiContext.RaMargin.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                            //    _.OilPackingTypeId == oilPackingTypeId);
                            //    if (raMarginCostContext != null)
                            //    {
                            //        raMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, raMarginCostContext.RatePerMt, litreConversion);
                            //        raMarginCost = noofPiecesperCase * raMarginCost;
                            //    }
                            //    else
                            //    {
                            //        isError = true;
                            //        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost + " - ", errorMessage);
                            //    }

                            //    exDepotPrice = exDepotPrice + raMarginCost;
                            //    exPlantPrice = exPlantPrice + raMarginCost;
                            //    forPlantPrice = forPlantPrice + raMarginCost;
                            //    exRakePrice = exRakePrice + raMarginCost;
                            //    outputDto.ForDepotPrice = finalPrice + raMarginCost;
                            //    outputDto.ForRakePrice = finalPrice + raMarginCost;

                            //    outputDto.TpPrice = finalPrice;
                            //    finalPrice = finalPrice + raMarginCost;
                            //    outputDto.ClearanceRate = finalPrice * inputDto.CounterBidLimit;
                            //    outputDto.CounterbidOffer = finalPrice + inputDto.BpCpJump;
                            //    outputDto.BaseRate = finalPrice;
                            //    outputDto.XMarginCost = inputDto.XMargin;
                            //    outputDto.FinalPrice = finalPrice + inputDto.XMargin;
                            //}
                            //else
                            //{
                            outputDto.ForDepotPrice = finalPrice;
                            outputDto.ForRakePrice = finalPrice;
                            outputDto.FinalPrice = finalPrice;
                            //}
                            outputDto.TransportMode = primaryFrieghtContext != null && primaryFrieghtContext.TransportMode != null ? primaryFrieghtContext.TransportMode.Name : string.Empty;
                            outputDto.MaterialCost = materialCost;
                            outputDto.PackingCost = packingCost;
                            outputDto.Premium = premium;
                            outputDto.Discount = discount;
                            outputDto.PrimaryFrieght = primaryFrieght;
                            outputDto.SecondaryFrieght = secondaryFrieght;
                            outputDto.SecondaryFrieghtForPlant = secondaryFrieghtForPlant;
                            outputDto.DepoCost = depoCost;
                            outputDto.DetentionCost = detentionCost;
                            outputDto.HoneycombCost = honeycombCost;
                            outputDto.MarginCost = marginCost;
                            outputDto.CushionMarginCost = cushionMarginCost;
                            outputDto.SchemeCost = schemeCostRecovery;
                            outputDto.RaMarginCost = raMarginCost;

                            outputDto.ExPlantPrice = exPlantPrice;
                            outputDto.ExDepotPrice = exDepotPrice;
                            outputDto.ForPlantPrice = forPlantPrice;
                            outputDto.ExRakePrice = exRakePrice;


                            //var currentDate = DateTime.UtcNow;
                            //outputDto.FinalPrice = inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess ? finalPrice : 0;
                            //outputDto.IsAddedForPricing = _emamiContext.Pricing.Any(s => s.SaudaBookingTypeId == inputDto.SaudaBookingTypeId && s.SkuId == inputDto.SkuId && DbFunctions.TruncateTime(s.BiddingDate) == currentDate.Date);

                            if (isError)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = errorMessage;
                                resultDto.SuccessDto.Response = outputDto;
                            }
                            else
                            {
                                resultDto.IsSuccess = true;
                                resultDto.SuccessDto.Response = outputDto;
                            }
                            resultDtoList.Add(resultDto);
                        }
                    }
                    else
                    {
                        var resultDto = new ResultDto();
                        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToLoadCapacity + " - ", errorMessage);
                        resultDto.ErrorDto.Message = errorMessage;
                        resultDtoList.Add(resultDto);
                    }
                }
                return resultDtoList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                //return _resultService.ErrorMessage(Constants.Exception);
                return new List<ResultDto>();
            }
        }

        /// <summary>
        /// TP and RA publish validation
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto PublishPriceValidate(SaveFinalPricngInputDto inputDto)
        {
            var resultDto = new ResultDto();
            var depotName = "";
            var plantName = "";
            var oilName = "";
            var errorMsg = "";
            long plantId = 0;
            var input = inputDto.inputDto;
            List<string> errorMessage = new List<string>();
            List<SkuFinalpriceListOutputDto> skuOutputDto = new List<SkuFinalpriceListOutputDto>();
            inputDto.BiddingDate = DateHelper.UtcToIndia(inputDto.BiddingDate);
            if (inputDto.inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
            {
                foreach (var item in inputDto.outputDto)
                {
                    var existsSkuData = _emamiContext.Pricing.AsNoTracking()
                    .Where(w => //w.OilTypeId == input.OilTypeId
                    w.PlantId == input.PlantId
                    //&& w.DepotId == input.DepotId
                    //&& input.FreightRouteIds.Contains(w.FrieghtRouteId)
                    //&& input.CityIds.Contains(w.CityId)
                    //&& DbFunctions.TruncateTime(w.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate)
                    && w.SkuId == item.SkuId
                    //&& w.TransportModeId == item.TransportModeId
                    //&& w.LoadQuantity == item.LoadQuantity
                    //&& w.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                    ).ToList();

                    if (existsSkuData != null && existsSkuData.Any())
                    {
                        foreach (var pricing in existsSkuData)
                        {
                            //var oiltyName = pricing.OilType.Name;
                            var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(f => f.Id == pricing.SkuId)?.SkuName;
                            var plant = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == pricing.PlantId)?.Name;
                            //var depot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == pricing.DepotId)?.Name;
                            //var cityName = pricing.City.CityName;
                            //var cityName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.CityId)?.CityName;
                            //var freightRouteName = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == pricing.FrieghtRouteId)?.Name;
                            //var transportModeName = pricing.TransportMode.Name;
                            //var loadQuantity = pricing.LoadQuantity;
                            var msg1 = //oiltyName + " - " + 
                                skuName + " - " + plant //+ " - " + depot
                                                        //+ " - " + cityName + " - "  + transportModeName + "  " + loadQuantity 
                                + "MT" + "<br>";
                            errorMessage.Add(msg1);
                        }
                    }
                    else
                    {
                        skuOutputDto.Add(item);
                    }
                }
                if (errorMessage != null && errorMessage.Any())
                {
                    errorMessage.Insert(0, "Already Published" + "<br><br>");
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = string.Join("", errorMessage);
                    return resultDto;
                }

                var existsData = _emamiContext.Pricing.AsNoTracking()
                    //.Where(w =>//w.OilTypeId == input.OilTypeId 
                    // && DbFunctions.TruncateTime(w.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate)
                    //)
                    ;

                if (existsData != null && existsData.Any())
                {
                    //var publishedData = existsData.Where(w => w.DepotId == input.DepotId);
                    var publishedData = existsData;
                    if (publishedData != null && publishedData.Any())
                    {
                        var isExistsPlant = publishedData.Any(w => w.PlantId == input.PlantId);
                        if (isExistsPlant)
                        {
                            resultDto.IsSuccess = true;
                        }
                        else
                        {
                            plantId = publishedData.FirstOrDefault().PlantId;
                            depotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == input.DepotId).Name;
                            plantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == plantId).Name;
                            oilName = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(f => f.Id == input.OilTypeId).Name;
                            errorMsg = "OilType: " + "<b>" + oilName + "</b>" + "," + "Depot: " + "<b>" + depotName + "," + "</b>" + "Plant: " + "<b>" + plantName + "</b>" + " already published. You will select the plant: " + "<b>" + plantName + "/<b>";
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = errorMsg;
                        }
                    }
                    else { resultDto.IsSuccess = true; }
                }
                else { resultDto.IsSuccess = true; }
            }
            //else if (input.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
            //{
            //    foreach (var item in inputDto.outputDto)
            //    {
            //        var existsSkuData = _emamiContext.Pricing.AsNoTracking()
            //        .Where(w => w.OilTypeId == input.OilTypeId
            //        && w.PlantId == input.PlantId
            //        && w.DepotId == input.DepotId
            //        && input.FreightRouteIds.Contains(w.FrieghtRouteId)
            //        && input.CityIds.Contains(w.CityId)
            //        && DbFunctions.TruncateTime(w.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate)
            //        && w.SkuId == item.SkuId
            //        && w.TransportModeId == item.TransportModeId
            //        && w.LoadQuantity == item.LoadQuantity
            //        && w.BiddingWindowId == inputDto.BiddingWindowId
            //        && w.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction).ToList();

            //        if (existsSkuData != null && existsSkuData.Any())
            //        {
            //            foreach (var pricing in existsSkuData)
            //            {
            //                var oiltyName = pricing.OilType.Name;
            //                var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(f => f.Id == pricing.SkuId)?.SkuName;
            //                var plant = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == pricing.PlantId)?.Name;
            //                var depot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == pricing.DepotId)?.Name;
            //                //var cityName = pricing.City.CityName;
            //                var cityName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.CityId)?.CityName;
            //                //var freightRouteName = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == pricing.FrieghtRouteId)?.Name;
            //                var transportModeName = pricing.TransportMode.Name;
            //                var loadQuantity = pricing.LoadQuantity;
            //                var bidWindow = _emamiContext.BiddingWindowTiming.AsNoTracking().FirstOrDefault(f => f.Id == pricing.BiddingWindowId);
            //                var msg1 = oiltyName + " - " + skuName + " - " + plant + " - " + depot + " - " + cityName + " - " + transportModeName + " " + loadQuantity + "MT" + " - " + "(" + bidWindow.FromHours + "-" + bidWindow.ToHours + ")" + "<br>";
            //                errorMessage.Add(msg1);
            //            }
            //        }
            //        else
            //        {
            //            skuOutputDto.Add(item);
            //        }
            //    }
            //    if (errorMessage != null && errorMessage.Any())
            //    {
            //        errorMessage.Insert(0, "Already Published" + "<br><br>");
            //        resultDto.IsSuccess = false;
            //        resultDto.ErrorDto.Message = string.Join("", errorMessage);
            //        return resultDto;
            //    }


            //    var existsData = _emamiContext.Pricing.AsNoTracking().Where(w => w.OilTypeId == input.OilTypeId
            //    && w.BiddingWindowId == inputDto.BiddingWindowId
            //    && DbFunctions.TruncateTime(w.BiddingDate) == inputDto.BiddingDate.Date);

            //    if (existsData != null && existsData.Any())
            //    {
            //        var publishedData = existsData.Where(w => w.DepotId == input.DepotId);
            //        if (publishedData != null && publishedData.Any())
            //        {
            //            var isExistsPlant = publishedData.Any(w => w.PlantId == input.PlantId);

            //            if (isExistsPlant)
            //            {
            //                resultDto.IsSuccess = true;
            //            }
            //            else
            //            {
            //                plantId = publishedData.FirstOrDefault().PlantId;
            //                depotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == input.DepotId).Name;
            //                plantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == plantId).Name;
            //                oilName = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(f => f.Id == input.OilTypeId).Name;
            //                errorMsg = "OilType: " + "<b>" + oilName + "</b>" + "," + "Depot: " + "<b>" + depotName + "," + "</b>" + "Plant: " + "<b>" + plantName + "</b>" + "" + " already published. You will select the plant: " + "<b>" + plantName + "<b>";
            //                resultDto.IsSuccess = false;
            //                resultDto.ErrorDto.Message = errorMsg;
            //            }
            //        }
            //        else { resultDto.IsSuccess = true; }
            //    }
            //    else { resultDto.IsSuccess = true; }
            //}
            else
            {
                resultDto.IsSuccess = false;
            }
            resultDto.SuccessDto.Response = skuOutputDto;
            return resultDto;
        }

        public ResultDto SaveTraditionalProcessFinalPrice(SaveFinalPricngInputDto inputDto)
        {
            _methodName = "SaveTraditionalProcessFinalPrice";
            var resultDto = new ResultDto();
            var inputs = inputDto.inputDto;
            var outputDto = new SaveFinalPricngInputDto();
            if (inputs == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }


            var result = PublishPriceValidate(inputDto);
            if (!(result.IsSuccess))
            {
                return result;
            }

            try
            {
                Pricing entity = null;
                if (inputDto.outputDto != null && inputDto.outputDto.Any())
                {
                    foreach (var TraditionalProcessFinalPrice in inputDto.outputDto)
                    {
                        entity = new Pricing()
                        {
                            SkuId = TraditionalProcessFinalPrice.SkuId,
                            OilTypeId = inputs.OilTypeId,
                            //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                            OilPackingTypeId = inputs.OilPackingTypeId,
                            //StateId = (int)TraditionalProcessFinalPrice.StateId,
                            //CityId = (int)TraditionalProcessFinalPrice.CityId,
                            PlantId = inputs.PlantId,
                            //DepotId = inputs.DepotId,
                            //Price = pricingLiveContext.Price,
                            //SalesOrganizationId = salesOrganizationId,
                            //DistributionChannelId = distributionChannelId,
                            //DivisionId = divisionId,
                            //ValidFrom = pricingLiveContext.ValidFrom,
                            //ValidTo = pricingLiveContext.ValidTo,
                            //FrieghtRouteId = TraditionalProcessFinalPrice.FreightRouteId,
                            //FrieghtZoneId = TraditionalProcessFinalPrice.FreightZoneId,
                            //TransportModeId = inputs.TransportModeId,
                            //TransportModeId = TraditionalProcessFinalPrice.TransportModeId,
                            //LoadQuantity = TraditionalProcessFinalPrice.LoadQuantity,
                            //BiddingDate = DateHelper.UtcToIndia(inputDto.BiddingDate),
                            //MaterialCost = TraditionalProcessFinalPrice.MaterialCost,
                            //PackingCost = TraditionalProcessFinalPrice.PackingCost,
                            //PrimaryFrieght = TraditionalProcessFinalPrice.PrimaryFrieght,
                            //SecondaryFrieght = TraditionalProcessFinalPrice.SecondaryFrieght,
                            //PlantSecondaryFrieght = TraditionalProcessFinalPrice.SecondaryFrieghtForPlant,
                            //DepotCost = TraditionalProcessFinalPrice.DepoCost,
                            //DetentionCost = TraditionalProcessFinalPrice.DetentionCost,
                            //HoneycombCost = TraditionalProcessFinalPrice.HoneycombCost,
                            //Margin = TraditionalProcessFinalPrice.MarginCost,
                            //CushionMargin = TraditionalProcessFinalPrice.CushionMarginCost,
                            //SchemeCostRecovery = TraditionalProcessFinalPrice.SchemeCost,
                            //ProcessCost = TraditionalProcessFinalPrice.SchemeCost,
                            //TpPrice = TraditionalProcessFinalPrice.FinalPrice,
                            //FinalRate = TraditionalProcessFinalPrice.FinalPrice,
                            //ExDepotPrice = TraditionalProcessFinalPrice.ExDepotPrice,
                            //ExPlantPrice = TraditionalProcessFinalPrice.ExPlantPrice,
                            //ForDepotPrice = TraditionalProcessFinalPrice.ForDepotPrice,
                            //ForPlantPrice = TraditionalProcessFinalPrice.ForPlantPrice,
                            //ExRakePrice = TraditionalProcessFinalPrice.ExRakePrice,
                            //ForRakePrice = TraditionalProcessFinalPrice.ForRakePrice,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            //IsActive = true,
                        };
                        _emamiContext.Pricing.Add(entity);
                    }
                    _emamiContext.SaveChanges();

                    var publishCityIds = inputDto.outputDto.Select(_ => _.CityId);
                    if (publishCityIds != null && publishCityIds.Any())
                    {
                        try
                        {
                            var usersRoleIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader || _.RoleId == (int)DTO.Enums.Role.Dealer || _.RoleId == (int)DTO.Enums.Role.Broker).Select(_ => _.UserId);
                            if (usersRoleIds != null && usersRoleIds.Any())
                            {
                                var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => usersRoleIds.Contains(_.Id) && _.IsActive && publishCityIds.Contains(_.CityId)
                                    && _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess).ToList();
                                List<string> toUsers = new List<string>();
                                if (usersContext != null && usersContext.Any())
                                {
                                    toUsers = usersContext.Where(_ => _.Email != null && _.Email != "").Select(_ => _.Email).ToList();
                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                    if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var emailSubject = Constants.FinalPricePublishSubject;
                                        var plainText = string.Empty;
                                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalPricePublishNotificationEmail);
                                        if (emailTemplate != null)
                                        {
                                            var plainTemplate = emailTemplate.PlainTemplate;
                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                        }
                                    }
                                    var smsMessage = string.Empty;
                                    if (_resultService.IsSMS())
                                    {
                                        toUsers = usersContext.Where(_ => _.MobileNumber != null && _.MobileNumber != "").Select(_ => _.MobileNumber).ToList();
                                        if (toUsers != null && toUsers.Any())
                                        {
                                            var smsPlainTemplate = string.Empty;
                                            EmailTemplate smsTemplate = new EmailTemplate();
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalPricePublishNotificationSMS);
                                            if (smsTemplate != null)
                                            {
                                                smsPlainTemplate = smsTemplate.PlainTemplate;
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                                foreach (var mobileNo in toUsers)
                                                {
                                                    amazonNotificationService.SendMessage(smsMessage, mobileNo, smsTemplate.SMSTemplateID);
                                                }
                                            }
                                        }
                                    }
                                    if (_resultService.IsPushNotification())
                                    {
                                        foreach (var userContext in usersContext)
                                        {
                                            if (userContext != null && userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
                                            {
                                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                                {
                                                    PushTokenKey = userContext.PushTokenKey,
                                                    RegistrationTypeId = (int)userContext.RegistrationTypeId,
                                                    Title = Constants.FinalPricePublishSubject,
                                                    Message = smsMessage,
                                                    Id = "00"
                                                };
                                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }




                //Notification

                /*
                foreach (var item in inputDto.outputDto)
                {
                    var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId);
                    if (skuContext != null && !string.IsNullOrEmpty(skuContext.SkuName) && entity.FinalRate != 0)
                    {
                        var priceContent = string.Empty;

                        //Final price notification
                        List<PriceNotifyConfiguration> notifyConfigContextList = _emamiContext.PriceNotifyConfiguration.AsNoTracking().
                            Where(_ => DbFunctions.TruncateTime(_.NotificationDate) == DbFunctions.TruncateTime(DateTime.UtcNow)).ToList();
                        if (notifyConfigContextList != null && notifyConfigContextList.Any())
                        {
                            List<int> iCityIds = new List<int>();
                            PriceNotifyConfiguration notifyConfigContext = new PriceNotifyConfiguration();
                            foreach (var notifyContext in notifyConfigContextList)
                            {
                                List<int> currentNotifyCityIds = new List<int>();
                                currentNotifyCityIds = UtilityHelper.ConvertStringToIntList(notifyContext.CityId);
                                if (currentNotifyCityIds != null && currentNotifyCityIds.Any())
                                {
                                    iCityIds.AddRange(currentNotifyCityIds);
                                    if (currentNotifyCityIds.Contains(inputs.CityId))
                                    {
                                        notifyConfigContext = notifyContext;
                                    }
                                }
                            }

                            if (notifyConfigContext != null)
                            {
                                var incoterms = !string.IsNullOrEmpty(notifyConfigContext.IncoTermId) ? notifyConfigContext.IncoTermId.Split(',').Select(x => long.Parse(x)) : new List<long>();
                                if (incoterms.Any())
                                {
                                    foreach (var incoterm in incoterms)
                                    {
                                        if (incoterm == (int)DTO.Enums.IncoTerms.ExDepot)
                                        {
                                            priceContent = priceContent + " " + "ExDepot Price: " + item.ExDepotPrice;
                                        }
                                        if (incoterm == (int)DTO.Enums.IncoTerms.ExPlant)
                                        {
                                            priceContent = priceContent + " " + "ExPlant Price: " + item.ExPlantPrice;
                                        }
                                        if (incoterm == (int)DTO.Enums.IncoTerms.ExRake)
                                        {
                                            priceContent = priceContent + " " + "ExRake Price: " + item.ExRakePrice;
                                        }
                                        if (incoterm == (int)DTO.Enums.IncoTerms.ForDepot)
                                        {
                                            priceContent = priceContent + " " + "ForDepot Price: " + item.ForDepotPrice;
                                        }
                                        if (incoterm == (int)DTO.Enums.IncoTerms.ForPlant)
                                        {
                                            priceContent = priceContent + " " + "ForPlant Price: " + item.ForPlantPrice;
                                        }
                                        if (incoterm == (int)DTO.Enums.IncoTerms.ForRake)
                                        {
                                            priceContent = priceContent + " " + "ForRake Price: " + item.ForRakePrice;
                                        }
                                    }
                                }

                                List<User> userContextList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserDepotMapping.AsNoTracking(), u => u.Id, ud => ud.UserId, (u, ud) => new { u = u, ud = ud })
                                    .Join(_emamiContext.UserRoles.AsNoTracking(), uud => uud.u.Id, ur => ur.UserId, (uud, ur) => new { uud, ur })
                                    .Where(_ => _.uud != null && _.uud.u != null && _.uud.ud != null && _.ur != null && _.uud.u.CityId == inputs.CityId && _.uud.u.FreightRouteId == inputs.FreightRouteId
                                    && _.uud.u.FreightZoneId == inputs.FreightZoneId && _.uud.u.TransportModeId == inputs.TransportModeId
                                    && _.uud.ud.DepotId == inputs.DepotId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker))
                                    .Select(_ => _.uud.u).Distinct().ToList();
                                if (userContextList != null && userContextList.Any())
                                {
                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                    if (_resultService.IsEmail())
                                    {
                                        if (notifyConfigContext.IsEmail == true)
                                        {
                                            var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalRateNotificationEmail);
                                            List<string> toUser = new List<string>();
                                            toUser.AddRange(userContextList.Select(_ => _.Email));
                                            var emailSubject = Constants.SkuFinalRateSubject;
                                            var fromEmail = Constants.FromEmail;
                                            var plainText = string.Empty;
                                            if (emailTemplate != null)
                                            {
                                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, skuContext.SkuName)
                                                    .Replace(Constants.SkuPricing, priceContent);
                                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                                            }
                                        }
                                    }
                                    var smsPlainTemplate = string.Empty;
                                    if (notifyConfigContext.IsSMS == true)
                                    {
                                        var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalRateNotificationSMS);
                                        if (smsTemplate != null)
                                        {
                                            //var plainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.FinalRate, entity.FinalRate.ToString());
                                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.SkuPricing, priceContent);

                                            if (_resultService.IsSMS())
                                            {
                                                var smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                                foreach (var userContext in userContextList)
                                                {
                                                    if (!string.IsNullOrEmpty(userContext.MobileNumber))
                                                    {
                                                        amazonNotificationService.SendMessage(smsMessage, userContext.MobileNumber);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (_resultService.IsPushNotification())
                                    {
                                        foreach (var userContext in userContextList)
                                        {
                                            if (userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
                                            {
                                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                                {
                                                    PushTokenKey = userContext.PushTokenKey,
                                                    RegistrationTypeId = (int)userContext.RegistrationTypeId,
                                                    Title = Constants.SkuFinalRateSubject,
                                                    Message = smsPlainTemplate
                                                };
                                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                */

                return _resultService.SuccessMessage(Constants.PriceDetailsSavedSuccessfully);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto SaveReverseAuctionFinalPrice(SaveFinalPricngInputDto inputDto)
        {
            _methodName = "SaveReverseAuctionFinalPrice";
            var resultDto = new ResultDto();
            var inputs = inputDto.inputDto;
            if (inputs == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            var result = PublishPriceValidate(inputDto);
            if (!(result.IsSuccess))
            {
                return result;
            }

            //try
            //{
            //    Pricing entity = new Pricing();
            //    foreach (var ReverseAuactionPriceing in inputDto.outputDto)
            //    {
            //        entity = new Pricing()
            //        {
            //            SkuId = ReverseAuactionPriceing.SkuId,
            //            //OilTypeId = inputs.OilTypeId,
            //            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
            //            OilPackingTypeId = inputs.OilPackingTypeId,
            //            StateId = (int)ReverseAuactionPriceing.StateId,
            //            CityId = (int)ReverseAuactionPriceing.CityId,
            //            PlantId = inputs.PlantId,
            //            DepotId = inputs.DepotId,
            //            FrieghtRouteId = ReverseAuactionPriceing.FreightRouteId,
            //            FrieghtZoneId = ReverseAuactionPriceing.FreightZoneId,
            //            //TransportModeId = inputs.TransportModeId,
            //            TransportModeId = ReverseAuactionPriceing.TransportModeId,
            //            LoadQuantity = ReverseAuactionPriceing.LoadQuantity,
            //            MaterialCost = ReverseAuactionPriceing.MaterialCost,
            //            PackingCost = ReverseAuactionPriceing.PackingCost,
            //            PrimaryFrieght = ReverseAuactionPriceing.PrimaryFrieght,
            //            SecondaryFrieght = ReverseAuactionPriceing.SecondaryFrieght,
            //            PlantSecondaryFrieght = ReverseAuactionPriceing.SecondaryFrieghtForPlant,
            //            DepotCost = ReverseAuactionPriceing.DepoCost,
            //            DetentionCost = ReverseAuactionPriceing.DetentionCost,
            //            HoneycombCost = ReverseAuactionPriceing.HoneycombCost,
            //            Margin = ReverseAuactionPriceing.MarginCost,
            //            CushionMargin = ReverseAuactionPriceing.CushionMarginCost,
            //            SchemeCostRecovery = ReverseAuactionPriceing.SchemeCost,
            //            ProcessCost = ReverseAuactionPriceing.SchemeCost,
            //            SumOfIngredientCost = ReverseAuactionPriceing.IngredientCost,
            //            TpPrice = ReverseAuactionPriceing.TpPrice,
            //            RaMargin = ReverseAuactionPriceing.RaMarginCost,
            //            BaseRate = ReverseAuactionPriceing.BaseRate,
            //            XMargin = ReverseAuactionPriceing.XMarginCost,
            //            FinalRate = Math.Round(ReverseAuactionPriceing.FinalPrice),
            //            ExDepotPrice = ReverseAuactionPriceing.ExDepotPrice,
            //            ExPlantPrice = ReverseAuactionPriceing.ExPlantPrice,
            //            ForDepotPrice = ReverseAuactionPriceing.ForDepotPrice,
            //            ForPlantPrice = ReverseAuactionPriceing.ForPlantPrice,
            //            ExRakePrice = ReverseAuactionPriceing.ExRakePrice,
            //            ForRakePrice = ReverseAuactionPriceing.ForRakePrice,
            //            ClearanceRate = ReverseAuactionPriceing.ClearanceRate,
            //            CounterBidOffer = ReverseAuactionPriceing.CounterbidOffer,
            //            CounterBidLimit = inputs.CounterBidLimit,
            //            BpCpJumb = inputs.BpCpJump,
            //            CreatedBy = inputDto.LoginUserId,
            //            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
            //            IsActive = true,
            //            BiddingDate = DateHelper.UtcToIndia(inputDto.BiddingDate),
            //            BiddingWindowId = inputDto.BiddingWindowId
            //        };
            //        _emamiContext.Pricing.Add(entity);
            //    }
            //    _emamiContext.SaveChanges();

            //    Thread workingThread = new Thread(new ParameterizedThreadStart(RAFinalPriceNotification))
            //    { IsBackground = true };
            //    workingThread.Start(inputDto);

            //    //try
            //    //{
            //    //    var biddingWindowContext = _emamiContext.BiddingWindowTiming.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
            //    //    var publishCityIds = inputDto.outputDto.Select(_ => _.CityId).Distinct();
            //    //    if (biddingWindowContext != null && publishCityIds != null && publishCityIds.Any())
            //    //    {
            //    //        var date = biddingWindowContext.BiddingDate.ToString("dddd, dd MMMM yyyy");
            //    //        DateTime dtFromTime = DateTime.MinValue + biddingWindowContext.FromHours;
            //    //        var fromTime = dtFromTime.ToString("hh:mm tt");
            //    //        DateTime dtToTime = DateTime.MinValue + biddingWindowContext.ToHours;
            //    //        var toTime = dtToTime.ToString("hh:mm tt");

            //    //        var priceNotifyConfig = Config.PriceNotifyConfigurationFlag;
            //    //        if (!string.IsNullOrEmpty(priceNotifyConfig) && priceNotifyConfig.ToLower().Equals("true"))
            //    //        {
            //    //            DateTime currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            //    //            List<long> publishSkulist = inputDto.outputDto.Select(s => s.SkuId).ToList();
            //    //            var priceNotifyConfigListContext = _emamiContext.PriceNotifyConfiguration.AsNoTracking().Where(_ => _.NotificationDate == DbFunctions.TruncateTime(currentDate)).ToList()
            //    //                .Where(_ => _.CityId.Split(',').ToList().Intersect(publishCityIds.Select(s => s.ToString())).Any()).ToList();
            //    //            List<long> availableSkus = new List<long>();
            //    //            List<long> unAvailableSkus = null;
            //    //            foreach (var priceNotifyConfigContext in priceNotifyConfigListContext)
            //    //            {
            //    //                if (priceNotifyConfigContext != null && (priceNotifyConfigContext.IsSMS || priceNotifyConfigContext.IsEmail || priceNotifyConfigContext.IsPushNotification))
            //    //                {
            //    //                    List<long> incoTermsIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.IncoTermId);
            //    //                    List<long> skuIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.SkuId);
            //    //                    availableSkus.AddRange(skuIds);
            //    //                    List<long> configCityIds = null;
            //    //                    if (publishCityIds != null && publishCityIds.Any())
            //    //                    {
            //    //                        configCityIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.CityId);
            //    //                        if (configCityIds != null && configCityIds.Any())
            //    //                        {
            //    //                            configCityIds = publishCityIds.Intersect(configCityIds).ToList();
            //    //                        }
            //    //                    }
            //    //                    foreach (var incoTermId in incoTermsIds)
            //    //                    {
            //    //                        var incoTermsName = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == incoTermId).Name;
            //    //                        foreach (long cityId in configCityIds)
            //    //                        {
            //    //                            var usersContext = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
            //    //                                .Where(_ => _.u.IsActive && _.u.CityId == cityId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker || _.ur.RoleId == (int)DTO.Enums.Role.StateTrader)
            //    //                                && _.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction).Select(_ => new
            //    //                                {
            //    //                                    Email = _.u.Email,
            //    //                                    MobileNumber = _.u.MobileNumber
            //    //                                ,
            //    //                                    RegistrationTypeId = _.u.RegistrationTypeId,
            //    //                                    PushTokenKey = _.u.PushTokenKey
            //    //                                });
            //    //                            if (usersContext != null && usersContext.Any())
            //    //                            {
            //    //                                List<string> toUsers = usersContext.Select(_ => _.Email).ToList();
            //    //                                string emailSkusPricing = string.Empty;
            //    //                                string mobileSkusPricing = string.Empty;
            //    //                                var skuList = inputDto.outputDto.Where(_ => skuIds.Contains(_.SkuId) && _.CityId == cityId);
            //    //                                foreach (var sku in skuList)
            //    //                                {
            //    //                                    var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == sku.SkuId)?.SkuName;

            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExDepot && sku.ExDepotPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForDepot && sku.ForDepotPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExPlant && sku.ExPlantPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForPlant && sku.ForPlantPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExRake && sku.ExRakePrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForRake && sku.ForRakePrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }

            //    //                                }
            //    //                                if (!string.IsNullOrEmpty(mobileSkusPricing))
            //    //                                {
            //    //                                    mobileSkusPricing = mobileSkusPricing.Substring(0, mobileSkusPricing.Length - 2);
            //    //                                }
            //    //                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //    //                                if (priceNotifyConfigContext.IsEmail && toUsers != null && toUsers.Any())
            //    //                                {
            //    //                                    var fromEmail = Constants.FromEmail;
            //    //                                    var emailSubject = Constants.FinalPricePublishSubject;
            //    //                                    var plainText = string.Empty;
            //    //                                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceConfigFinalPricePublishEmail);
            //    //                                    if (emailTemplate != null && !string.IsNullOrEmpty(emailSkusPricing))
            //    //                                    {
            //    //                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //    //                                            .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, emailSkusPricing);
            //    //                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
            //    //                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //    //                                    }
            //    //                                }
            //    //                                var smsMessage = string.Empty;
            //    //                                if (priceNotifyConfigContext.IsSMS && !string.IsNullOrEmpty(mobileSkusPricing))
            //    //                                {
            //    //                                    var smsPlainTemplate = string.Empty;
            //    //                                    EmailTemplate smsTemplate = new EmailTemplate();
            //    //                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceConfigFinalPricePublishSMS);
            //    //                                    if (smsTemplate != null)
            //    //                                    {
            //    //                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //    //                                            .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, mobileSkusPricing);
            //    //                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //    //                                        try
            //    //                                        {
            //    //                                            foreach (var mobileNo in usersContext.Select(_ => _.MobileNumber))
            //    //                                            {
            //    //                                                amazonNotificationService.SendMessage(smsMessage, mobileNo);
            //    //                                            }
            //    //                                        }
            //    //                                        catch (Exception ex)
            //    //                                        {

            //    //                                        }
            //    //                                    }
            //    //                                }
            //    //                                if (priceNotifyConfigContext.IsPushNotification && !string.IsNullOrEmpty(mobileSkusPricing))
            //    //                                {
            //    //                                    foreach (var userContext in usersContext)
            //    //                                    {
            //    //                                        if (userContext != null && userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
            //    //                                        {
            //    //                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //    //                                            {
            //    //                                                PushTokenKey = userContext.PushTokenKey,
            //    //                                                RegistrationTypeId = (int)userContext.RegistrationTypeId,
            //    //                                                Title = Constants.FinalPricePublishSubject,
            //    //                                                Message = smsMessage,
            //    //                                                Id = "00"
            //    //                                            };
            //    //                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //    //                                        }
            //    //                                    }
            //    //                                }
            //    //                            }
            //    //                        }
            //    //                    }
            //    //                }
            //    //            }
            //    //            unAvailableSkus = availableSkus != null && availableSkus.Any() ? publishSkulist.Except(availableSkus).ToList() : publishSkulist.ToList();
            //    //            if ((unAvailableSkus != null && unAvailableSkus.Any()) && (publishCityIds != null && publishCityIds.Any()))
            //    //            {
            //    //                foreach (var cityId in publishCityIds)
            //    //                {
            //    //                    List<SkuFinalpriceListOutputDto> skuList = new List<SkuFinalpriceListOutputDto>();
            //    //                    if (unAvailableSkus != null && unAvailableSkus.Any())
            //    //                    {
            //    //                        skuList = inputDto.outputDto.Where(_ => unAvailableSkus.Contains(_.SkuId) && _.CityId == cityId).ToList();
            //    //                    }
            //    //                    else
            //    //                    {
            //    //                        skuList = inputDto.outputDto.Where(_ => _.CityId == cityId).ToList();
            //    //                    }
            //    //                    if (skuList != null && skuList.Any())
            //    //                    {
            //    //                        var usersContext = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
            //    //                               .Where(_ => _.u.IsActive && _.u.CityId == cityId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker || _.ur.RoleId == (int)DTO.Enums.Role.StateTrader)
            //    //                               && _.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction).Select(_ => new
            //    //                               {
            //    //                                   Id = _.u.Id,
            //    //                                   Email = _.u.Email,
            //    //                                   MobileNumber = _.u.MobileNumber
            //    //                               ,
            //    //                                   RegistrationTypeId = _.u.RegistrationTypeId,
            //    //                                   PushTokenKey = _.u.PushTokenKey
            //    //                               }).ToList();

            //    //                        foreach (var userContextItem in usersContext)
            //    //                        {
            //    //                            List<string> toUsers = new List<string>();
            //    //                            toUsers.Add(userContextItem.Email);
            //    //                            var incotermsList = _emamiContext.UserIncoTerms.AsNoTracking().Where(_ => _.UserId == userContextItem.Id).ToList();
            //    //                            foreach (var incoTermItem in incotermsList)
            //    //                            {
            //    //                                string emailSkusPricing = string.Empty;
            //    //                                string mobileSkusPricing = string.Empty;
            //    //                                var incoTermId = incoTermItem.IncoTermsId;
            //    //                                var incoTermsName = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == incoTermId)?.Name;
            //    //                                foreach (var sku in skuList)
            //    //                                {
            //    //                                    var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == sku.SkuId)?.SkuName;
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExDepot && sku.ExDepotPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForDepot && sku.ForDepotPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExPlant && sku.ExPlantPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForPlant && sku.ForPlantPrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExRake && sku.ExRakePrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForRake && sku.ForRakePrice != 0)
            //    //                                    {
            //    //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString()));
            //    //                                        mobileSkusPricing += skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //    //                                    }
            //    //                                }
            //    //                                if (!string.IsNullOrEmpty(mobileSkusPricing))
            //    //                                {
            //    //                                    mobileSkusPricing = mobileSkusPricing.Substring(0, mobileSkusPricing.Length - 2);
            //    //                                }
            //    //                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //    //                                var fromEmail = Constants.FromEmail;
            //    //                                var emailSubject = Constants.FinalPricePublishSubject;
            //    //                                var plainText = string.Empty;
            //    //                                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceConfigFinalPricePublishEmail);
            //    //                                if (emailTemplate != null && !string.IsNullOrEmpty(emailSkusPricing))
            //    //                                {
            //    //                                    var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //    //                                        .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, emailSkusPricing);
            //    //                                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
            //    //                                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //    //                                }
            //    //                                var smsMessage = string.Empty;
            //    //                                var smsPlainTemplate = string.Empty;
            //    //                                EmailTemplate smsTemplate = new EmailTemplate();
            //    //                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceConfigFinalPricePublishSMS);
            //    //                                if (smsTemplate != null && !string.IsNullOrEmpty(mobileSkusPricing))
            //    //                                {
            //    //                                    smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //    //                                        .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, mobileSkusPricing);
            //    //                                    smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //    //                                    try
            //    //                                    {
            //    //                                        amazonNotificationService.SendMessage(smsMessage, userContextItem.MobileNumber);
            //    //                                    }
            //    //                                    catch (Exception ex)
            //    //                                    {

            //    //                                    }
            //    //                                }
            //    //                                if (userContextItem != null && userContextItem.RegistrationTypeId != null && userContextItem.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContextItem.PushTokenKey) && !string.IsNullOrEmpty(mobileSkusPricing))
            //    //                                {
            //    //                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //    //                                    {
            //    //                                        PushTokenKey = userContextItem.PushTokenKey,
            //    //                                        RegistrationTypeId = (int)userContextItem.RegistrationTypeId,
            //    //                                        Title = Constants.FinalPricePublishSubject,
            //    //                                        Message = smsMessage,
            //    //                                        Id = "00"
            //    //                                    };
            //    //                                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //    //                                }
            //    //                            }
            //    //                        }
            //    //                    }
            //    //                }
            //    //            }
            //    //        }
            //    //        else if (publishCityIds != null && publishCityIds.Any())
            //    //        {
            //    //            var usersRoleIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader || _.RoleId == (int)DTO.Enums.Role.Dealer || _.RoleId == (int)DTO.Enums.Role.Broker).Select(_ => _.UserId);
            //    //            if (usersRoleIds != null && usersRoleIds.Any())
            //    //            {
            //    //                var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => usersRoleIds.Contains(_.Id) && _.IsActive && publishCityIds.Contains(_.CityId)
            //    //                    && _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction).ToList();
            //    //                List<string> toUsers = new List<string>();
            //    //                if (usersContext != null && usersContext.Any() && biddingWindowContext != null)
            //    //                {
            //    //                    toUsers = usersContext.Where(_ => _.Email != null && _.Email != "").Select(_ => _.Email).ToList();

            //    //                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //    //                    if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
            //    //                    {
            //    //                        var fromEmail = Constants.FromEmail;
            //    //                        var emailSubject = Constants.FinalPricePublishSubject;
            //    //                        var plainText = string.Empty;
            //    //                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.RAFinalPricePublishNotificationEmail);
            //    //                        if (emailTemplate != null)
            //    //                        {
            //    //                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime);
            //    //                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
            //    //                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //    //                        }

            //    //                    }
            //    //                    var smsMessage = string.Empty;
            //    //                    if (_resultService.IsSMS())
            //    //                    {
            //    //                        toUsers = usersContext.Where(_ => _.MobileNumber != null && _.MobileNumber != "").Select(_ => _.MobileNumber).ToList();
            //    //                        if (toUsers != null && toUsers.Any())
            //    //                        {
            //    //                            var smsPlainTemplate = string.Empty;
            //    //                            EmailTemplate smsTemplate = new EmailTemplate();
            //    //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.RAFinalPricePublishNotificationSMS);
            //    //                            if (smsTemplate != null)
            //    //                            {
            //    //                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime);
            //    //                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //    //                                foreach (var mobileNo in toUsers)
            //    //                                {
            //    //                                    amazonNotificationService.SendMessage(smsMessage, mobileNo);
            //    //                                }
            //    //                            }
            //    //                        }
            //    //                    }
            //    //                    if (_resultService.IsPushNotification())
            //    //                    {
            //    //                        foreach (var userContext in usersContext)
            //    //                        {
            //    //                            if (userContext != null && userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
            //    //                            {
            //    //                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //    //                                {
            //    //                                    PushTokenKey = userContext.PushTokenKey,
            //    //                                    RegistrationTypeId = (int)userContext.RegistrationTypeId,
            //    //                                    Title = Constants.FinalPricePublishSubject,
            //    //                                    Message = smsMessage,
            //    //                                    Id = "00",
            //    //                                };
            //    //                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //    //                            }
            //    //                        }
            //    //                    }
            //    //                }
            //    //            }
            //    //        }
            //    //    }
            //    //}
            //    //catch (Exception ex)
            //    //{

            //    //}


            //    //Notification
            //    /*
            //    foreach (var item in inputDto.outputDto)
            //    {
            //        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId);
            //        if (skuContext != null && !string.IsNullOrEmpty(skuContext.SkuName) && entity.FinalRate != 0)
            //        {
            //            var priceContent = string.Empty;

            //            //Final price notification
            //            List<PriceNotifyConfiguration> notifyConfigContextList = _emamiContext.PriceNotifyConfiguration.AsNoTracking().
            //                Where(_ => DbFunctions.TruncateTime(_.NotificationDate) == DbFunctions.TruncateTime(DateTime.UtcNow)).ToList();
            //            if (notifyConfigContextList != null && notifyConfigContextList.Any())
            //            {
            //                List<int> iCityIds = new List<int>();
            //                PriceNotifyConfiguration notifyConfigContext = new PriceNotifyConfiguration();
            //                foreach (var notifyContext in notifyConfigContextList)
            //                {
            //                    List<int> currentNotifyCityIds = new List<int>();
            //                    currentNotifyCityIds = UtilityHelper.ConvertStringToIntList(notifyContext.CityId);
            //                    if (currentNotifyCityIds != null && currentNotifyCityIds.Any())
            //                    {
            //                        iCityIds.AddRange(currentNotifyCityIds);
            //                        if (currentNotifyCityIds.Contains(inputs.CityId))
            //                        {
            //                            notifyConfigContext = notifyContext;
            //                        }
            //                    }
            //                }

            //                if (notifyConfigContext != null)
            //                {
            //                    var incoterms = !string.IsNullOrEmpty(notifyConfigContext.IncoTermId) ? notifyConfigContext.IncoTermId.Split(',').Select(x => long.Parse(x)) : new List<long>();
            //                    if (incoterms.Any())
            //                    {
            //                        foreach (var incoterm in incoterms)
            //                        {
            //                            if (incoterm == (int)DTO.Enums.IncoTerms.ExDepot)
            //                            {
            //                                priceContent = priceContent + " " + "ExDepot Price: " + item.ExDepotPrice;
            //                            }
            //                            if (incoterm == (int)DTO.Enums.IncoTerms.ExPlant)
            //                            {
            //                                priceContent = priceContent + " " + "ExPlant Price: " + item.ExPlantPrice;
            //                            }
            //                            if (incoterm == (int)DTO.Enums.IncoTerms.ExRake)
            //                            {
            //                                priceContent = priceContent + " " + "ExRake Price: " + item.ExRakePrice;
            //                            }
            //                            if (incoterm == (int)DTO.Enums.IncoTerms.ForDepot)
            //                            {
            //                                priceContent = priceContent + " " + "ForDepot Price: " + item.ForDepotPrice;
            //                            }
            //                            if (incoterm == (int)DTO.Enums.IncoTerms.ForPlant)
            //                            {
            //                                priceContent = priceContent + " " + "ForPlant Price: " + item.ForPlantPrice;
            //                            }
            //                            if (incoterm == (int)DTO.Enums.IncoTerms.ForRake)
            //                            {
            //                                priceContent = priceContent + " " + "ForRake Price: " + item.ForRakePrice;
            //                            }
            //                        }
            //                    }

            //                    List<User> userContextList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserDepotMapping.AsNoTracking(), u => u.Id, ud => ud.UserId, (u, ud) => new { u = u, ud = ud })
            //                        .Join(_emamiContext.UserRoles.AsNoTracking(), uud => uud.u.Id, ur => ur.UserId, (uud, ur) => new { uud, ur })
            //                        .Where(_ => _.uud != null && _.uud.u != null && _.uud.ud != null && _.ur != null && _.uud.u.CityId == inputs.CityId && _.uud.u.FreightRouteId == inputs.FreightRouteId
            //                        && _.uud.u.FreightZoneId == inputs.FreightZoneId && _.uud.u.TransportModeId == inputs.TransportModeId
            //                        && _.uud.ud.DepotId == inputs.DepotId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker))
            //                        .Select(_ => _.uud.u).Distinct().ToList();
            //                    if (userContextList != null && userContextList.Any())
            //                    {
            //                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //                        if (_resultService.IsEmail())
            //                        {
            //                            if (notifyConfigContext.IsEmail == true)
            //                            {
            //                                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalRateNotificationEmail);
            //                                List<string> toUser = new List<string>();
            //                                toUser.AddRange(userContextList.Select(_ => _.Email));
            //                                var emailSubject = Constants.SkuFinalRateSubject;
            //                                var fromEmail = Constants.FromEmail;
            //                                var plainText = string.Empty;
            //                                if (emailTemplate != null)
            //                                {
            //                                    var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, skuContext.SkuName)
            //                                        .Replace(Constants.SkuPricing, priceContent);
            //                                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
            //                                    amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
            //                                }
            //                            }
            //                        }
            //                        var smsPlainTemplate = string.Empty;
            //                        if (notifyConfigContext.IsSMS == true)
            //                        {
            //                            var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalRateNotificationSMS);
            //                            if (smsTemplate != null)
            //                            {
            //                                //var plainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.FinalRate, entity.FinalRate.ToString());
            //                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.SkuPricing, priceContent);
            //                                if (_resultService.IsSMS())
            //                                {
            //                                    var smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //                                    foreach (var userContext in userContextList)
            //                                    {
            //                                        if (!string.IsNullOrEmpty(userContext.MobileNumber))
            //                                        {
            //                                            amazonNotificationService.SendMessage(smsMessage, userContext.MobileNumber);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        if (_resultService.IsPushNotification())
            //                        {
            //                            foreach (var userContext in userContextList)
            //                            {
            //                                if (userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
            //                                {
            //                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //                                    {
            //                                        PushTokenKey = userContext.PushTokenKey,
            //                                        RegistrationTypeId = (int)userContext.RegistrationTypeId,
            //                                        Title = Constants.SkuFinalRateSubject,
            //                                        Message = smsPlainTemplate
            //                                    };
            //                                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    */
            return _resultService.SuccessMessage(Constants.PriceDetailsSavedSuccessfully);
            //}
            //catch (Exception exception)
            //{
            //    var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
            //    _logger.Error(message);
            //    return _resultService.ErrorMessage(Constants.Exception);
            //}
        }

        /// <summary>
        /// TP final price calculation new
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto SkuFinalpriceListForAdminNew(SkuFinalpriceListInputDto inputDto)
        {
            _logger.Info("Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));
            _methodName = "SkuFinalpriceListForAdminNew";
            var resultDtoMain = new ResultDto();
            var outputDtoMain = new List<SkuFinalpriceListOutputDto>();
            var errorListMain = new List<string>();
            bool isErrorMain = false;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

            try
            {
                //Get Common data
                var MaterialCostData = _emamiContext.MaterialCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var PackingCostData = _emamiContext.PackingCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var DepotCostData = _emamiContext.DepotCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var DetentionCostData = _emamiContext.DetentionCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var ProfitMarginsData = _emamiContext.ProfitMargins.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var CushionMarginData = _emamiContext.CushionMargins.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var SchemeCostData = _emamiContext.SchemeCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var PrimaryFreightData = _emamiContext.PrimaryFreights.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var SecondaryFreightData = _emamiContext.SecondaryFreights.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var HoneycombCostData = _emamiContext.HoneycombCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var RaMarginData = _emamiContext.RaMargin.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));

                var LoadCapacityConversionData = _emamiContext.LoadCapacityConversion.AsNoTracking()
                   .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));


                _logger.Info("Pricing Cost :" + DateHelper.UtcToIndia(DateTime.UtcNow));

                //Get Transport Modes
                var transportModeData = _emamiContext.TransportModes.AsNoTracking().Where(_ => _.IsActive).Select(s => s).ToList();
                var transportModes = transportModeData.Select(s => s.Id).ToList();

                //Get SKU details
                var skuList = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId && _.PackGroupId == inputDto.OilPackingTypeId &&
                                   _.IsActive).ToList();

                //Get Rasoi oiltypes
                //var rasoiOilTypeIds = _emamiContext.OilTypes.AsNoTracking().Where(w => w.IsRasoi).Select(s => s.Id).ToList();

                _logger.Info("Before Calculation Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));

                //Process the SKU's
                long unickId = 0;
                if (skuList != null && skuList.Any())
                {
                    foreach (var sku in skuList)
                    {

                        var isValidSku = true;
                        var errorMessage1 = sku.SkuName + " : " + Constants.MissingSkuRequiredField;

                        if (sku.Quantity <= 0)
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuQuantityField, errorMessage1);
                            isValidSku = false;
                        }

                        if (sku.UomId == null || sku.UomId <= 0)
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuPackSizeQuantityField, errorMessage1);
                            isValidSku = false;
                        }

                        //If Vertical type is (SpecialityFat or HBC) and it contains rasoi oiltypes condition true
                        if (sku.DivisionId == (int)DTO.Enums.Division.SpecialityFat || (sku.DivisionId == (int)DTO.Enums.Division.Hbc))/*rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(sku.OilTypeId.GetValueOrDefault())*/
                        {
                            //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), ProcessCost is required
                            //if (sku.ProcessCost <= 0)
                            //{
                            //    errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuProcessCostField, errorMessage1);
                            //    isValidSku = false;
                            //}

                            //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), SKU ingredients is required
                            //if (!_emamiContext.SkuIngrediant.AsNoTracking().Any(_ => _.SkuId == sku.Id))
                            //{
                            //    errorMessage1 = Constants.BindErrorMessage(Constants.SkuIngredientNotAdded, errorMessage1);
                            //    isValidSku = false;
                            //}
                        }

                        if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuUom2Field, errorMessage1);
                            isValidSku = false;
                        }

                        if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuUom3Field, errorMessage1);
                            isValidSku = false;
                        }

                        if (isValidSku)
                        {
                            _logger.Info("Valid  SKU Calculation Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                            #region Price Calculation

                            var resultDtoList = new List<ResultDto>();
                            inputDto.SkuId = sku.Id;
                            var verticalId = 0L;
                            var oilTypeId = 0L;
                            var oilPackingTypeId = 0L;
                            var uomId = 0L;
                            decimal litreConversion = 0;
                            decimal quantity = 0;
                            decimal materialCost = 0;
                            decimal packingCost = 0;
                            decimal primaryFrieght = 0;
                            decimal secondaryFrieght = 0;
                            decimal depoCost = 0;
                            decimal detentionCost = 0;
                            decimal honeycombCost = 0;
                            decimal marginCost = 0;
                            decimal cushionMarginCost = 0;
                            decimal schemeCostRecovery = 0;
                            decimal raMarginCost = 0;
                            decimal discount = 0;
                            decimal premium = 0;
                            decimal secondaryFrieghtForPlant = 0;
                            decimal exPlantPrice = 0;
                            decimal forPlantPrice = 0;
                            decimal exDepotPrice = 0;
                            decimal exRakePrice = 0;
                            decimal finalPrice = 0;
                            decimal noofPiecesperCase = 0;
                            bool isError = false;

                            var skuId = inputDto.SkuId;
                            var plantId = inputDto.PlantId;
                            var depotId = inputDto.DepotId;
                            var stateId = inputDto.StateId;
                            //var cityId = inputDto.CityId;
                            //var freightRouteId = inputDto.FreightRouteId;


                            //Get SKU details
                            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                            oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                            oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                            uomId = Convert.ToInt64(skuContext.UomId);
                            quantity = skuContext.Quantity;
                            var errorMessage = skuContext.SkuName + " : " + Constants.DataMissingToCalculate;


                            //Get OilType details
                            var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                            verticalId = oilTypeContext.DivisionId;
                           // litreConversion = oilTypeContext.LitreConversion;


                            //Get SkuUomMapping details
                            var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                            if (skuUomContext != null)
                            {
                                noofPiecesperCase = skuUomContext.ConversionFactor;
                            }


                            //Material Cost calculations
                            if (verticalId == (int)DTO.Enums.Division.Hbc)/*rasoiOilTypeIds == null || !rasoiOilTypeIds.Any()) || !rasoiOilTypeIds.Contains(oilTypeId))*/
                            {
                                var materialCostContext = MaterialCostData.FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                                if (materialCostContext != null)
                                {
                                    materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                                    materialCost = noofPiecesperCase * materialCost;
                                }
                                else
                                {
                                    isError = true;
                                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost + " - ", errorMessage);
                                }
                            }

                            //Packing Cost calculations
                            var packingCostContext = PackingCostData.FirstOrDefault(_ => _.PlantId == plantId && _.SkuId == skuId);
                            if (packingCostContext != null)
                            {
                                packingCost = packingCostContext.SalesPackingCost;
                                //var noofPiecesperMt = (decimal)0;
                                //var skuUomMtContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                //if (skuUomMtContext != null)
                                //{
                                //    noofPiecesperMt = skuUomMtContext.ConversionFactor;
                                //}

                                //packingCost = (packingCostContext.SalesPackingCost / noofPiecesperMt) * noofPiecesperCase;
                                //packingCost = _resultService.GetSkuQuanityRate(uomId, quantity, packingCostContext.SalesPackingCost, litreConversion);
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToPackingCost + " - ", errorMessage);
                            }

                            //Depot Cost calculations
                            var depoCostContext = DepotCostData.FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                            if (depoCostContext != null)
                            {
                                depoCost = _resultService.GetSkuQuanityRate(uomId, quantity, depoCostContext.RatePerMt, litreConversion);
                                depoCost = noofPiecesperCase * depoCost;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToDepoCost + " - ", errorMessage);
                            }

                            //Detention Cost calculations
                            var detentionCostContext = DetentionCostData.FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                            if (detentionCostContext != null)
                            {
                                detentionCost = _resultService.GetSkuQuanityRate(uomId, quantity, detentionCostContext.RatePerMt, litreConversion);
                                detentionCost = noofPiecesperCase * detentionCost;
                            }
                            else
                            {
                                isError = true;
                                errorMessage = Constants.BindErrorMessage(Constants.DataMissingToDetentionCost + " - ", errorMessage);
                            }

                            //Process the Cities
                            foreach (var cityId in inputDto.CityIds)
                            {
                                schemeCostRecovery = 0;

                                //Margin Cost calculations
                                var marginCostContext = ProfitMarginsData.FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                                _.OilPackingTypeId == oilPackingTypeId);
                                if (marginCostContext != null)
                                {
                                    marginCost = _resultService.GetSkuQuanityRate(uomId, quantity, marginCostContext.RatePerMt, litreConversion);
                                    marginCost = noofPiecesperCase * marginCost;
                                }
                                else
                                {
                                    isError = true;
                                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToMarginCost + " - ", errorMessage);
                                }

                                //Cushion Margin Cost calculations
                                var cushionMarginCostContext = CushionMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                                _.OilPackingTypeId == oilPackingTypeId);
                                if (cushionMarginCostContext != null)
                                {
                                    cushionMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                                    cushionMarginCost = noofPiecesperCase * cushionMarginCost;
                                }
                                else
                                {
                                    isError = true;
                                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToCushionMarginCost + " - ", errorMessage);
                                }

                                //RA Margin Cost
                                //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                //{
                                //    var raMarginCostContext = RaMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                                //    _.OilPackingTypeId == oilPackingTypeId);
                                //    if (raMarginCostContext != null)
                                //    {
                                //        raMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, raMarginCostContext.RatePerMt, litreConversion);
                                //        raMarginCost = noofPiecesperCase * raMarginCost;
                                //    }
                                //    else
                                //    {
                                //        isError = true;
                                //        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost + " - ", errorMessage);
                                //    }
                                //}

                                //Scheme Cost Recovery calculations
                                var schemeCostContext = SchemeCostData.FirstOrDefault(_ => _.PackGroupId == inputDto.OilPackingTypeId && _.OilTypeId == oilTypeId && _.CityId == cityId);
                                if (schemeCostContext != null)
                                {
                                    schemeCostRecovery = _resultService.GetSkuQuanityRate(uomId, quantity, schemeCostContext.RatePerMt, litreConversion);
                                    schemeCostRecovery = noofPiecesperCase * schemeCostRecovery;
                                }

                                decimal formulationCost = 0;

                                //If Vertical type is (SpecialityFat or HBC) and it contains rasoi oiltypes condition true
                                //Vertical type SpecialityFat or (HBC - Rasoi) cost calculation
                                if (verticalId == (int)DTO.Enums.Division.SpecialityFat || (verticalId == (int)DTO.Enums.Division.Hbc /*rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(oilTypeId))*/))
                                {

                                    //var skuIngredientList = _emamiContext.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId && _.OilTypeId == oilTypeId).ToList();
                                    //foreach (var skuIngredient in skuIngredientList)
                                    //{
                                    //    var ingredientCost = _emamiContext.IngredientCost.AsNoTracking()
                                    //        .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                    //    if (ingredientCost != null)
                                    //    {
                                    //        var oneKgIngredientCost = (ingredientCost.LooseOilRate * skuIngredient.Percentage) / 100;
                                    //        formulationCost = formulationCost + oneKgIngredientCost;

                                    //        //var qtySplitup = (skuIngredient.Percentage / 100) * quantity;
                                    //        //var oneKgIngredientCost = (ingredientCost.LooseOilRate / 1000) * qtySplitup;
                                    //        //formulationCost = formulationCost + oneKgIngredientCost;
                                    //    }
                                    //    else
                                    //    {
                                    //        isError = true;
                                    //        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost + " - ", errorMessage);
                                    //    }
                                    //}

                                    var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;
                                    materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, specialityFatMaterialCost, litreConversion);
                                    materialCost = noofPiecesperCase * materialCost;

                                    formulationCost = _resultService.GetSkuQuanityRate(uomId, quantity, formulationCost, litreConversion);
                                    formulationCost = noofPiecesperCase * formulationCost;

                                    //var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;
                                    //materialCost = noofPiecesperCase * specialityFatMaterialCost;
                                    //formulationCost = noofPiecesperCase * formulationCost;

                                    //finalPrice = materialCost + ((packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                    // honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                }
                                else
                                {
                                    //HBC cost calculation
                                    //finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                    //                     honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                }


                                if (inputDto.FreightRouteIds != null && inputDto.FreightRouteIds.Any())
                                {
                                    foreach (var freightRouteId in inputDto.FreightRouteIds)
                                    {
                                        if (transportModes != null && transportModes.Any())
                                        {
                                            foreach (var transportId in transportModes)
                                            {
                                                var transportMode = string.Empty;

                                                var loadCapacityContextList = LoadCapacityConversionData.Where(_ =>
                                                     _.SkuId == skuId && _.TransportModeId == transportId
                                                    && _.DivisionId == verticalId).ToList();

                                                //Honeycomb Cost calculations
                                                var honeycombCostContext = HoneycombCostData.FirstOrDefault(_ => _.PlantId == plantId && _.StateId == stateId &&
                                            _.SkuId == skuId && _.TransportModeId == transportId);
                                                if (honeycombCostContext != null)
                                                {
                                                    honeycombCost = _resultService.GetSkuQuanityRate(uomId, quantity, honeycombCostContext.RatePerMt, litreConversion);
                                                    honeycombCost = noofPiecesperCase * honeycombCost;
                                                }
                                                else
                                                {
                                                    isError = true;
                                                    errorMessage = Constants.BindErrorMessage(transportModeData.FirstOrDefault(f => f.Id == transportId).Name + "-" + Constants.DataMissingToHoneyCombCost + " - ", errorMessage);
                                                }

                                                var resultDtoSub = new ResultDto();
                                                var outputDto1 = new SkuFinalpriceListOutputDto();

                                                if (!isError && loadCapacityContextList != null && loadCapacityContextList.Any())
                                                {
                                                    foreach (var loadCapacityItem in loadCapacityContextList)
                                                    {

                                                        var loadCapacity = loadCapacityItem.LoadCapacity;
                                                        var loadQuantityCase = loadCapacityItem.LoadQuantity;
                                                        primaryFrieght = 0;
                                                        secondaryFrieght = 0;
                                                        honeycombCost = 0;
                                                        raMarginCost = 0;
                                                        discount = 0;
                                                        premium = 0;
                                                        secondaryFrieghtForPlant = 0;
                                                        exPlantPrice = 0;
                                                        forPlantPrice = 0;
                                                        exDepotPrice = 0;
                                                        exRakePrice = 0;
                                                        finalPrice = 0;

                                                        var transportModeContext = transportModeData.FirstOrDefault(_ => _.Id == loadCapacityItem.TransportModeId);
                                                        if (transportModeContext != null)
                                                            transportMode = transportModeContext.Name;

                                                        resultDtoSub = new ResultDto();
                                                        outputDto1 = new SkuFinalpriceListOutputDto()
                                                        {
                                                            IngredientCost = formulationCost,
                                                            SkuId = skuContext.Id,
                                                            SkuName = skuContext.SkuName,
                                                            TransportModeId = transportId
                                                        };

                                                        outputDto1.LoadQuantity = loadCapacity;

                                                        var primaryFrieghtContext = PrimaryFreightData.FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                                                        _.VerticalId == verticalId && _.TransportModeId == transportId && _.LoadCapacity == Constants.DefaultLoadQuantity);
                                                        if (primaryFrieghtContext != null)
                                                        {
                                                            var loadCapacity16Mt = loadCapacityContextList.FirstOrDefault(_ => _.LoadCapacity == Constants.DefaultLoadQuantity);
                                                            if (loadCapacity16Mt != null)
                                                            {
                                                                primaryFrieght = primaryFrieghtContext.SalesFreight;
                                                                primaryFrieght = (primaryFrieght / loadCapacity16Mt.LoadQuantity) * 1;
                                                            }
                                                            //else
                                                            //{
                                                            //    isError = true;
                                                            //    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToLoadCapacity + " for " + transportMode + " " + "16MT" + " - ", errorMessage);
                                                            //}
                                                        }
                                                        //else
                                                        //{
                                                        //    isError = true;
                                                        //    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToPrimaryFrieght + " for " + transportMode + " " + "16MT" + " - ", errorMessage);
                                                        //}


                                                        //Secondary Frieght
                                                        var secondaryFrieghtContext = SecondaryFreightData.FirstOrDefault(_ => _.DepotId == depotId
                                                    && _.FreightRouteId == freightRouteId
                                                    && _.VerticalId == verticalId && _.TransportModeId == transportId && _.Capacity == loadCapacity);
                                                        if (secondaryFrieghtContext != null)
                                                        {
                                                            secondaryFrieght = secondaryFrieghtContext.SalesFreight;
                                                            secondaryFrieght = (secondaryFrieght / loadQuantityCase) * 1;
                                                        }
                                                        //else
                                                        //{
                                                        //    isError = true;
                                                        //    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSecondaryFrieght + " for " + transportMode + " " + loadCapacity + "MT" + " - ", errorMessage);
                                                        //}

                                                        //Secondary Frieght for plant                                                        
                                                        var secondaryFrieghtContextForPlant = SecondaryFreightData.FirstOrDefault(_ => _.DepotId == plantId
                                                    && _.FreightRouteId == freightRouteId
                                                    && _.VerticalId == verticalId && _.TransportModeId == transportId && _.Capacity == loadCapacity);
                                                        if (secondaryFrieghtContextForPlant != null)
                                                        {
                                                            secondaryFrieghtForPlant = secondaryFrieghtContextForPlant.SalesFreight;
                                                            secondaryFrieghtForPlant = (secondaryFrieghtForPlant / loadQuantityCase) * 1;
                                                        }
                                                        //else
                                                        //{
                                                        //    isError = true;
                                                        //    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSecondaryFrieghtForPlant + " for " + transportMode + " " + loadCapacity + "MT" + " - ", errorMessage);
                                                        //}

                                                        var cityData = _emamiContext.City.FirstOrDefault(f => f.Id == cityId);
                                                        stateId = cityData.District.StateId;

                                                        if (primaryFrieght > 0 && secondaryFrieght > 0)
                                                        {
                                                            finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                                                      honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                        }

                                                        if (primaryFrieght > 0)
                                                        {
                                                            exDepotPrice = ((materialCost + packingCost + primaryFrieght + depoCost + detentionCost +
                                                                                     marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                        }

                                                        exPlantPrice = ((materialCost + packingCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                                                        if (secondaryFrieghtForPlant > 0)
                                                        {
                                                            forPlantPrice = ((materialCost + packingCost + secondaryFrieghtForPlant +
                                                                      honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                        }

                                                        if (primaryFrieght > 0)
                                                        {
                                                            exRakePrice = ((materialCost + packingCost + primaryFrieght +
                                                                         honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                        }

                                                        //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                        //{

                                                        //    exDepotPrice = exDepotPrice > 0 ? (exDepotPrice + raMarginCost) : 0;
                                                        //    exPlantPrice = exPlantPrice + raMarginCost;
                                                        //    forPlantPrice = forPlantPrice > 0 ? (forPlantPrice + raMarginCost) : forPlantPrice;
                                                        //    exRakePrice = exRakePrice > 0 ? (exRakePrice + raMarginCost) : exRakePrice;
                                                        //    outputDto1.ForDepotPrice = finalPrice > 0 ? (finalPrice + raMarginCost) : finalPrice;
                                                        //    outputDto1.ForRakePrice = finalPrice > 0 ? (finalPrice + raMarginCost) : finalPrice;

                                                        //    outputDto1.TpPrice = finalPrice;
                                                        //    finalPrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;
                                                        //    outputDto1.ClearanceRate = finalPrice > 0 ? (finalPrice * inputDto.CounterBidLimit) : 0;
                                                        //    outputDto1.CounterbidOffer = finalPrice > 0 ? (finalPrice + inputDto.BpCpJump) : 0;
                                                        //    outputDto1.BaseRate = finalPrice;
                                                        //    outputDto1.XMarginCost = inputDto.XMargin;
                                                        //    outputDto1.FinalPrice = finalPrice > 0 ? (finalPrice + inputDto.XMargin) : 0;
                                                        //}
                                                        //else
                                                        //{
                                                        outputDto1.ForDepotPrice = finalPrice;
                                                        outputDto1.ForRakePrice = finalPrice;
                                                        outputDto1.FinalPrice = finalPrice;
                                                        //}
                                                        outputDto1.TransportMode = _emamiContext.TransportModes.AsNoTracking().FirstOrDefault(_ => _.Id == transportId)?.Name;
                                                        outputDto1.MaterialCost = materialCost;
                                                        outputDto1.PackingCost = packingCost;
                                                        outputDto1.Premium = premium;
                                                        outputDto1.Discount = discount;
                                                        outputDto1.PrimaryFrieght = primaryFrieght;
                                                        outputDto1.SecondaryFrieght = secondaryFrieght;
                                                        outputDto1.SecondaryFrieghtForPlant = secondaryFrieghtForPlant;
                                                        outputDto1.DepoCost = depoCost;
                                                        outputDto1.DetentionCost = detentionCost;
                                                        outputDto1.HoneycombCost = honeycombCost;
                                                        outputDto1.MarginCost = marginCost;
                                                        outputDto1.CushionMarginCost = cushionMarginCost;
                                                        outputDto1.SchemeCost = schemeCostRecovery;
                                                        outputDto1.RaMarginCost = raMarginCost;

                                                        outputDto1.ExPlantPrice = exPlantPrice;
                                                        outputDto1.ExDepotPrice = exDepotPrice;
                                                        outputDto1.ForPlantPrice = forPlantPrice;
                                                        outputDto1.ExRakePrice = exRakePrice;

                                                        outputDto1.CityName = cityData.CityName;
                                                        outputDto1.CityId = cityId;
                                                        outputDto1.StateId = stateId;
                                                        unickId++;
                                                        outputDto1.Id = unickId;

                                                        //var freightRoutes = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == freightRouteId);

                                                        //if (freightRoutes != null)
                                                        //{
                                                        //    outputDto1.FreightZoneId = freightRoutes.FreightZoneId;
                                                        //    outputDto1.FreightRouteName = freightRoutes.Name;
                                                        //}
                                                        //outputDto1.FreightRouteId = freightRouteId;

                                                        //if (isError)
                                                        //{
                                                        //    resultDtoSub.IsSuccess = false;
                                                        //    resultDtoSub.ErrorDto.Message = errorMessage;
                                                        //    resultDtoSub.SuccessDto.Response = outputDto1;
                                                        //    if (!string.IsNullOrEmpty(errorMessage))
                                                        //        resultDtoList.Add(resultDtoSub);
                                                        //    errorMessage = string.Empty;
                                                        //}
                                                        //else
                                                        //{
                                                        //    resultDtoSub.IsSuccess = true;
                                                        //    resultDtoSub.SuccessDto.Response = outputDto1;
                                                        //    resultDtoList.Add(resultDtoSub);
                                                        //}

                                                        resultDtoSub.IsSuccess = true;
                                                        resultDtoSub.SuccessDto.Response = outputDto1;
                                                        resultDtoList.Add(resultDtoSub);

                                                    }
                                                }
                                                else
                                                {
                                                    isError = true;
                                                    errorMessage = Constants.BindErrorMessage(transportModeData.FirstOrDefault(f => f.Id == transportId).Name + "-" + Constants.DataMissingToLoadCapacity + " - ", errorMessage);
                                                }

                                                if (isError)
                                                {
                                                    resultDtoSub.IsSuccess = false;
                                                    resultDtoSub.ErrorDto.Message = errorMessage;
                                                    resultDtoSub.SuccessDto.Response = outputDto1;
                                                    if (!string.IsNullOrEmpty(errorMessage))
                                                        resultDtoList.Add(resultDtoSub);
                                                    //errorMessage = string.Empty;
                                                }
                                                //else
                                                //{
                                                //    resultDtoSub.IsSuccess = true;
                                                //    resultDtoSub.SuccessDto.Response = outputDto1;
                                                //    resultDtoList.Add(resultDtoSub);
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            return _resultService.ErrorMessage(Constants.TransportModeNotFount);
                                        }
                                    }
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.FreightRouteNotFount);
                                }
                            }

                            foreach (var finalPriceResult in resultDtoList)
                            {
                                if (finalPriceResult.IsSuccess)
                                {
                                    outputDtoMain.Add((SkuFinalpriceListOutputDto)finalPriceResult.SuccessDto.Response);
                                }
                                else
                                {
                                    isErrorMain = true;
                                    errorListMain.Add(finalPriceResult.ErrorDto.Message + "<br>");
                                }
                            }
                            _logger.Info("Completed :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                            #endregion

                        }
                        else
                        {
                            isErrorMain = true;
                            errorListMain.Add(errorMessage1 + "<br>");
                        }
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                if (isErrorMain)
                {
                    resultDtoMain.IsSuccess = false;
                    resultDtoMain.ErrorDto.Response = errorListMain;
                    resultDtoMain.SuccessDto.Response = outputDtoMain;
                }
                else
                {
                    resultDtoMain.IsSuccess = true;
                    resultDtoMain.SuccessDto.Response = outputDtoMain;
                }
                return _resultService.SuccessObject(resultDtoMain);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// TP final price calculation new
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto SkuFinalpriceListForAdminUpdated(SkuFinalpriceListInputDto inputDto)
        {
            _logger.Info("Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));
            _methodName = "SkuFinalpriceListForAdminNew";
            var resultDtoMain = new ResultDto();
            var outputDtoMain = new List<SkuFinalpriceListOutputDto>();
            var errorListMain = new List<string>();
            bool isErrorMain = false;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            long unickId = 0;

            try
            {
                //Get Common data
                var MaterialCostData = _emamiContext.MaterialCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var PackingCostData = _emamiContext.PackingCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var DepotCostData = _emamiContext.DepotCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var DetentionCostData = _emamiContext.DetentionCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var ProfitMarginsData = _emamiContext.ProfitMargins.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var CushionMarginData = _emamiContext.CushionMargins.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var SchemeCostData = _emamiContext.SchemeCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var PrimaryFreightData = _emamiContext.PrimaryFreights.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var SecondaryFreightData = _emamiContext.SecondaryFreights.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var HoneycombCostData = _emamiContext.HoneycombCosts.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                var RaMarginData = _emamiContext.RaMargin.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));

                var LoadCapacityConversionData = _emamiContext.LoadCapacityConversion.AsNoTracking()
                   .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));


                _logger.Info("Pricing Cost :" + DateHelper.UtcToIndia(DateTime.UtcNow));

                //Get Transport Modes
                var transportModeData = _emamiContext.TransportModes.AsNoTracking().Select(s => s).ToList();
                var transportModes = transportModeData.Select(s => s.Id).ToList();

                //Get SKU details
                var skuList = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId && _.PackGroupId == inputDto.OilPackingTypeId &&
                                   _.IsActive).ToList();

                //Get Rasoi oiltypes
                //var rasoiOilTypeIds = _emamiContext.OilTypes.AsNoTracking().Where(w => w.IsRasoi).Select(s => s.Id).ToList();

                _logger.Info("Before Calculation Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));

                //Process the SKU's
                if (skuList != null && skuList.Any())
                {
                    foreach (var sku in skuList)
                    {
                        var isValidSku = true;
                        var errorMessage1 = sku.SkuName + " : " + Constants.MissingSkuRequiredField;

                        if (sku.Quantity <= 0)
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuQuantityField, errorMessage1);
                            isValidSku = false;
                        }

                        if (sku.UomId == null || sku.UomId <= 0)
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuPackSizeQuantityField, errorMessage1);
                            isValidSku = false;
                        }

                        //If Vertical type is (SpecialityFat or HBC) and it contains rasoi oiltypes condition true
                        if (sku.DivisionId == (int)DTO.Enums.Division.SpecialityFat || (sku.DivisionId == (int)DTO.Enums.Division.Hbc))/*rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(sku.OilTypeId.GetValueOrDefault())*/
                        {
                            //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), ProcessCost is required
                            if (sku.ProcessCost <= 0)
                            {
                                errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuProcessCostField, errorMessage1);
                                isValidSku = false;
                            }

                            //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), SKU ingredients is required
                            //if (!_emamiContext.SkuIngrediant.AsNoTracking().Any(_ => _.SkuId == sku.Id))
                            //{
                            //    errorMessage1 = Constants.BindErrorMessage(Constants.SkuIngredientNotAdded, errorMessage1);
                            //    isValidSku = false;
                            //}
                        }

                        if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuUom2Field, errorMessage1);
                            isValidSku = false;
                        }

                        if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                        {
                            errorMessage1 = Constants.BindErrorMessage(Constants.MissingSkuUom3Field, errorMessage1);
                            isValidSku = false;
                        }

                        if (isValidSku)
                        {
                            _logger.Info("Valid  SKU Calculation Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                            #region Price Calculation

                            var resultDtoList = new List<ResultDto>();
                            inputDto.SkuId = sku.Id;
                            var verticalId = 0L;
                            var oilTypeId = 0L;
                            var oilPackingTypeId = 0L;
                            var uomId = 0L;
                            decimal litreConversion = 0;
                            decimal quantity = 0;
                            decimal materialCost = 0;
                            decimal packingCost = 0;
                            decimal primaryFrieght = 0;
                            decimal secondaryFrieght = 0;
                            decimal depoCost = 0;
                            decimal detentionCost = 0;
                            decimal honeycombCost = 0;
                            decimal marginCost = 0;
                            decimal cushionMarginCost = 0;
                            decimal schemeCostRecovery = 0;
                            decimal raMarginCost = 0;
                            decimal discount = 0;
                            decimal premium = 0;
                            decimal secondaryFrieghtForPlant = 0;
                            decimal exPlantPrice = 0;
                            decimal forPlantPrice = 0;
                            decimal exDepotPrice = 0;
                            decimal exRakePrice = 0;
                            decimal finalPrice = 0;
                            decimal noofPiecesperCase = 0;
                            bool isError = false;

                            var skuId = inputDto.SkuId;
                            var plantId = inputDto.PlantId;
                            var depotId = inputDto.DepotId;
                            var stateId = inputDto.StateId;

                            //Get SKU details
                            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                            oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                            oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                            uomId = Convert.ToInt64(skuContext.UomId);
                            quantity = skuContext.Quantity;
                            var errorMessage = skuContext.SkuName + " : " + Constants.DataMissingToCalculate;


                            //Get OilType details
                            var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                            verticalId = oilTypeContext.DivisionId;
                           // litreConversion = oilTypeContext.LitreConversion;


                            //Get SkuUomMapping details
                            var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                            if (skuUomContext != null)
                            {
                                noofPiecesperCase = skuUomContext.ConversionFactor;
                            }


                            //Material Cost calculations
                            decimal formulationCost = 0;
                            if (verticalId == (int)DTO.Enums.Division.Hbc)/*rasoiOilTypeIds == null || !rasoiOilTypeIds.Any()) || !rasoiOilTypeIds.Contains(oilTypeId)*/
                            {
                                var materialCostContext = MaterialCostData.FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                                if (materialCostContext != null)
                                {
                                    materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                                    materialCost = noofPiecesperCase * materialCost;
                                }
                                else
                                {
                                    isError = true;
                                }
                            }
                            else if (verticalId == (int)DTO.Enums.Division.SpecialityFat || (verticalId == (int)DTO.Enums.Division.Hbc)) /*rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(oilTypeId)*/
                            {

                                //var skuIngredientList = _emamiContext.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId).ToList();
                                //foreach (var skuIngredient in skuIngredientList)
                                //{
                                //    var ingredientCost = _emamiContext.IngredientCost.AsNoTracking()
                                //        .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                //    if (ingredientCost != null)
                                //    {

                                //        var qtySplitup = (skuIngredient.Percentage / 100) * quantity;
                                //        var oneKgIngredientCost = (ingredientCost.LooseOilRate / 1000) * qtySplitup;
                                //        formulationCost = formulationCost + oneKgIngredientCost;
                                //    }
                                //    else
                                //    {
                                //        isError = true;
                                //    }
                                //}

                                var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;
                                materialCost = noofPiecesperCase * specialityFatMaterialCost;

                                formulationCost = noofPiecesperCase * formulationCost;
                            }

                            //Packing Cost calculations
                            var packingCostContext = PackingCostData.FirstOrDefault(_ => _.PlantId == plantId && _.SkuId == skuId);
                            if (packingCostContext != null)
                            {
                                packingCost = packingCostContext.SalesPackingCost;
                                //var noofPiecesperMt = (decimal)0;
                                //var skuUomMtContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                //if (skuUomMtContext != null)
                                //{
                                //    noofPiecesperMt = skuUomMtContext.ConversionFactor;
                                //}

                                //packingCost = (packingCostContext.SalesPackingCost / noofPiecesperMt) * noofPiecesperCase;
                            }
                            else
                            {
                                isError = true;
                            }

                            //Depot Cost calculations
                            var depoCostContext = DepotCostData.FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                            if (depoCostContext != null)
                            {
                                depoCost = _resultService.GetSkuQuanityRate(uomId, quantity, depoCostContext.RatePerMt, litreConversion);
                                depoCost = noofPiecesperCase * depoCost;
                            }
                            else
                            {
                                isError = true;
                            }

                            //Detention Cost calculations
                            var detentionCostContext = DetentionCostData.FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                            if (detentionCostContext != null)
                            {
                                detentionCost = _resultService.GetSkuQuanityRate(uomId, quantity, detentionCostContext.RatePerMt, litreConversion);
                                detentionCost = noofPiecesperCase * detentionCost;
                            }
                            else
                            {
                                isError = true;
                            }

                            if (!isError)
                            {
                                var profitMarginDataContext = ProfitMarginsData.ToList().Where(_ => _.SkuId == skuId && inputDto.CityIds.Contains(Convert.ToInt64(_.CityId)) &&
                                    _.OilPackingTypeId == oilPackingTypeId);
                                var cushionMarginDataContext = CushionMarginData.ToList().Where(_ => _.SkuId == skuId && profitMarginDataContext != null && profitMarginDataContext.Any(a => a.CityId == _.CityId) &&
                                     _.OilPackingTypeId == oilPackingTypeId);
                                var schemeCostDataContext = SchemeCostData.ToList().Where(_ => _.PackGroupId == inputDto.OilPackingTypeId && cushionMarginDataContext != null && cushionMarginDataContext.Any(a => a.CityId == _.CityId));

                                //Process the Cities
                                if (cushionMarginDataContext != null && cushionMarginDataContext.Any())
                                {
                                    var cityIds = cushionMarginDataContext.Select(_ => Convert.ToInt64(_.CityId)).ToList();
                                    foreach (var cityId in cityIds)
                                    {
                                        //Margin Cost calculations
                                        var marginCostContext = profitMarginDataContext.FirstOrDefault(_ => _.CityId == cityId);
                                        if (marginCostContext != null)
                                        {
                                            marginCost = _resultService.GetSkuQuanityRate(uomId, quantity, marginCostContext.RatePerMt, litreConversion);
                                            marginCost = noofPiecesperCase * marginCost;
                                        }
                                        else
                                        {
                                            isError = true;
                                        }

                                        //Cushion Margin Cost calculations
                                        var cushionMarginCostContext = cushionMarginDataContext.FirstOrDefault(_ => _.CityId == cityId);
                                        if (cushionMarginCostContext != null)
                                        {
                                            cushionMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                                            cushionMarginCost = noofPiecesperCase * cushionMarginCost;
                                        }
                                        else
                                        {
                                            isError = true;
                                        }

                                        //Scheme Cost Recovery calculations
                                        var schemeCostContext = schemeCostDataContext.FirstOrDefault(_ => _.CityId == cityId);
                                        if (schemeCostContext != null)
                                        {
                                            schemeCostRecovery = _resultService.GetSkuQuanityRate(uomId, quantity, schemeCostContext.RatePerMt, litreConversion);
                                            schemeCostRecovery = noofPiecesperCase * schemeCostRecovery;
                                        }

                                        //finalPrice = materialCost + ((packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                        // honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                                        if (inputDto.FreightRouteIds != null && inputDto.FreightRouteIds.Any() && !isError)
                                        {
                                            //var secondaryFrieghtDataContext = SecondaryFreightData.ToList().Where(_ => _.DepotId == depotId
                                            //                && inputDto.FreightRouteIds.Contains(_.FreightRouteId ?? 0)
                                            //                && _.VerticalId == verticalId && transportModes.Contains(_.TransportModeId));
                                            //var secondaryFrieghtDataContextForPlant = SecondaryFreightData.ToList().Where(_ => _.DepotId == plantId
                                            //                && secondaryFrieghtDataContext.Any(a => a.FreightRouteId == _.FreightRouteId)
                                            //                && _.VerticalId == verticalId && secondaryFrieghtDataContext.Any(a => a.TransportModeId == _.TransportModeId));
                                            //var freightRouteIds = secondaryFrieghtDataContextForPlant.Select(_ => _.FreightRouteId).ToList();
                                            foreach (var freightRouteId in inputDto.FreightRouteIds)
                                            {
                                                if (transportModes != null && transportModes.Any())
                                                {
                                                    var loadCapacityDataContextList = LoadCapacityConversionData.Where(_ => _.SkuId == skuId && transportModes.Contains(_.TransportModeId)
                                                                                            && _.DivisionId == verticalId).ToList();
                                                    var transportModeIds = loadCapacityDataContextList.Select(_ => _.TransportModeId).Distinct().ToList();
                                                    foreach (var transportId in transportModeIds)
                                                    {
                                                        var transportMode = string.Empty;

                                                        var loadCapacityContextList = loadCapacityDataContextList.Where(_ => _.TransportModeId == transportId).ToList();
                                                        if (loadCapacityContextList != null && loadCapacityContextList.Any())
                                                        {
                                                            foreach (var loadCapacityItem in loadCapacityContextList)
                                                            {
                                                                var transportModeContext = transportModeData.FirstOrDefault(_ => _.Id == loadCapacityItem.TransportModeId);
                                                                if (transportModeContext != null)
                                                                    transportMode = transportModeContext.Name;

                                                                var resultDtoSub = new ResultDto();
                                                                var outputDto1 = new SkuFinalpriceListOutputDto()
                                                                {
                                                                    IngredientCost = formulationCost,
                                                                    SkuId = skuContext.Id,
                                                                    SkuName = skuContext.SkuName,
                                                                    SkuCode = skuContext.SkuCode,
                                                                    TransportModeId = transportId
                                                                };

                                                                var loadCapacity = loadCapacityItem.LoadCapacity;
                                                                var loadQuantityCase = loadCapacityItem.LoadQuantity;

                                                                outputDto1.LoadQuantity = loadCapacity;

                                                                var primaryFrieghtContext = PrimaryFreightData.FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                                                                _.VerticalId == verticalId && _.TransportModeId == transportId);
                                                                if (primaryFrieghtContext != null)
                                                                {
                                                                    primaryFrieght = primaryFrieghtContext.SalesFreight;
                                                                    primaryFrieght = (primaryFrieght / loadQuantityCase) * 1;
                                                                }
                                                                else
                                                                {
                                                                    isError = true;
                                                                }

                                                                var secondaryFrieghtContext = SecondaryFreightData.FirstOrDefault(_ => _.FreightRouteId == freightRouteId
                                                                            && _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == depotId && _.VerticalId == verticalId);
                                                                if (secondaryFrieghtContext != null)
                                                                {
                                                                    secondaryFrieght = secondaryFrieghtContext.SalesFreight;
                                                                    secondaryFrieght = (secondaryFrieght / loadQuantityCase) * 1;
                                                                }
                                                                else
                                                                {
                                                                    isError = true;
                                                                }

                                                                var secondaryFrieghtContextForPlant = SecondaryFreightData.FirstOrDefault(_ => _.FreightRouteId == freightRouteId
                                                                            && _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == plantId && _.VerticalId == verticalId);
                                                                if (secondaryFrieghtContextForPlant != null)
                                                                {
                                                                    secondaryFrieghtForPlant = secondaryFrieghtContextForPlant.SalesFreight;
                                                                    secondaryFrieghtForPlant = (secondaryFrieghtForPlant / loadQuantityCase) * 1;
                                                                }
                                                                else
                                                                {
                                                                    isError = true;
                                                                }

                                                                var cityData = _emamiContext.City.FirstOrDefault(f => f.Id == cityId);
                                                                stateId = cityData.District.StateId;

                                                                //Honeycomb Cost calculations
                                                                var honeycombCostContext = HoneycombCostData.FirstOrDefault(_ => _.PlantId == plantId && _.StateId == stateId &&
                                                            _.SkuId == skuId && _.TransportModeId == transportId);
                                                                if (honeycombCostContext != null)
                                                                {
                                                                    honeycombCost = _resultService.GetSkuQuanityRate(uomId, quantity, honeycombCostContext.RatePerMt, litreConversion);
                                                                    honeycombCost = noofPiecesperCase * honeycombCost;
                                                                }
                                                                else
                                                                {
                                                                    isError = true;
                                                                }

                                                                //if (primaryFrieght > 0 && secondaryFrieght>0)
                                                                //{
                                                                finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                                                              honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                //}
                                                                //if (primaryFrieght > 0)
                                                                //{
                                                                exDepotPrice = ((materialCost + packingCost + primaryFrieght + depoCost + detentionCost +
                                                                                         marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                //}
                                                                exPlantPrice = ((materialCost + packingCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                //if (secondaryFrieghtForPlant > 0)
                                                                //{
                                                                forPlantPrice = ((materialCost + packingCost + secondaryFrieghtForPlant +
                                                                                  honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                //}
                                                                //if (primaryFrieght > 0)
                                                                //{
                                                                exRakePrice = ((materialCost + packingCost + primaryFrieght +
                                                                                 honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                //}
                                                                //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                                //{
                                                                //    //RA Margin Cost
                                                                //    var raMarginCostContext = RaMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                                                                //    _.OilPackingTypeId == oilPackingTypeId);
                                                                //    if (raMarginCostContext != null)
                                                                //    {
                                                                //        raMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, raMarginCostContext.RatePerMt, litreConversion);
                                                                //        raMarginCost = noofPiecesperCase * raMarginCost;
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        isError = true;
                                                                //        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost + " - ", errorMessage);
                                                                //    }

                                                                //    exDepotPrice = exDepotPrice > 0 ? (exDepotPrice + raMarginCost) : 0;
                                                                //    exPlantPrice = exPlantPrice + raMarginCost;
                                                                //    forPlantPrice = forPlantPrice > 0 ? (forPlantPrice + raMarginCost) : 0;
                                                                //    exRakePrice = exRakePrice > 0 ? (exRakePrice + raMarginCost) : 0;
                                                                //    outputDto1.ForDepotPrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;
                                                                //    outputDto1.ForRakePrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;

                                                                //    outputDto1.TpPrice = exPlantPrice;
                                                                //    finalPrice = exPlantPrice > 0 ? (exPlantPrice + raMarginCost) : 0;
                                                                //    outputDto1.ClearanceRate = finalPrice > 0 ? (finalPrice * inputDto.CounterBidLimit) : 0;
                                                                //    outputDto1.CounterbidOffer = finalPrice > 0 ? (finalPrice + inputDto.BpCpJump) : 0;
                                                                //    outputDto1.BaseRate = finalPrice;
                                                                //    outputDto1.XMarginCost = inputDto.XMargin;
                                                                //    outputDto1.FinalPrice = finalPrice > 0 ? (finalPrice + inputDto.XMargin) : 0;
                                                                //}
                                                                //else
                                                                //{
                                                                outputDto1.ForDepotPrice = finalPrice;
                                                                outputDto1.ForRakePrice = finalPrice;
                                                                outputDto1.FinalPrice = finalPrice;
                                                                //}
                                                                outputDto1.TransportMode = primaryFrieghtContext != null && primaryFrieghtContext.TransportMode != null ? primaryFrieghtContext.TransportMode.Name : string.Empty;
                                                                outputDto1.MaterialCost = materialCost;
                                                                outputDto1.PackingCost = packingCost;
                                                                outputDto1.Premium = premium;
                                                                outputDto1.Discount = discount;
                                                                outputDto1.PrimaryFrieght = primaryFrieght;
                                                                outputDto1.SecondaryFrieght = secondaryFrieght;
                                                                outputDto1.SecondaryFrieghtForPlant = secondaryFrieghtForPlant;
                                                                outputDto1.DepoCost = depoCost;
                                                                outputDto1.DetentionCost = detentionCost;
                                                                outputDto1.HoneycombCost = honeycombCost;
                                                                outputDto1.MarginCost = marginCost;
                                                                outputDto1.CushionMarginCost = cushionMarginCost;
                                                                outputDto1.SchemeCost = schemeCostRecovery;
                                                                outputDto1.RaMarginCost = raMarginCost;

                                                                outputDto1.ExPlantPrice = exPlantPrice;
                                                                outputDto1.ExDepotPrice = exDepotPrice;
                                                                outputDto1.ForPlantPrice = forPlantPrice;
                                                                outputDto1.ExRakePrice = exRakePrice;

                                                                outputDto1.CityName = cityData.CityName;
                                                                outputDto1.CityId = cityId;
                                                                outputDto1.StateId = stateId;
                                                                unickId++;
                                                                outputDto1.Id = unickId;

                                                                //var freightRoutes = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == freightRouteId);

                                                                //if (freightRoutes != null)
                                                                //{
                                                                //    outputDto1.FreightZoneId = freightRoutes.FreightZoneId;
                                                                //    outputDto1.FreightRouteName = freightRoutes.Name;
                                                                //}
                                                                //outputDto1.FreightRouteId = freightRouteId;

                                                                if (!isError)
                                                                {
                                                                    outputDtoMain.Add(outputDto1);
                                                                }
                                                                isError = false;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    return _resultService.ErrorMessage(Constants.TransportModeNotFount);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return _resultService.ErrorMessage(Constants.FreightRouteNotFount);
                                        }
                                    }
                                }
                            }

                            _logger.Info("Completed :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                            #endregion

                        }
                        else
                        {
                            isErrorMain = true;
                            errorListMain.Add(errorMessage1 + "<br>");
                        }
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                if (isErrorMain)
                {
                    resultDtoMain.IsSuccess = false;
                    resultDtoMain.ErrorDto.Response = errorListMain;
                    resultDtoMain.SuccessDto.Response = outputDtoMain;
                }
                else
                {
                    resultDtoMain.IsSuccess = true;
                    resultDtoMain.SuccessDto.Response = outputDtoMain;
                }
                return _resultService.SuccessObject(resultDtoMain);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// Final price calculation city, district, territory, freight route, freight zone removed, state multiple, depot multiple
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public void SkuFinalpriceListForAdminUpdatedNew(SkuFinalpriceListInputDto inputDto)
        {
            _logger.Info("Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));
            _methodName = "SkuFinalpriceListForAdminUpdatedNew";
            _logger.Info("Pricing Cost :" + DateHelper.UtcToIndia(DateTime.UtcNow));
            var resultDtoMain = new ResultDto();
            var outputDtoMain = new List<SkuFinalpriceListOutputDto>();
            var errorListMain = new List<string>();
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            bool isAvailable = false;
            string smsContent = string.Empty;
            string mainErrorMessage = string.Empty;
            List<string> errorMessageList = new List<string>();
            int count = 0;
            PricePublish pricePublishContext = new PricePublish();
            List<string> mobileNoList = UtilityHelper.ConvertStringToStringArray(inputDto.MobileNoList).ToList();
            List<Pricing> pricings = new List<Pricing>();


            try
            {

                var skuList1 = _emamiContext.Skus.AsNoTracking().Where(_ => inputDto.OilTypeIds.Contains(_.OilTypeId ?? 0) && inputDto.OilPackingTypeIds.Contains(_.PackGroupId ?? 0) &&
                                   _.IsActive).ToList();
                var skuList = _emamiContext.Skus.AsNoTracking()
                    .Where(_ => inputDto.OilTypeIds.Contains(_.OilTypeId ?? 0) && inputDto.OilPackingTypeIds.Contains(_.PackGroupId ?? 0) && _.IsActive)
                                   .Select(s => new Sku { Id = s.Id, SkuName = s.SkuName, SkuCode = s.SkuCode, Quantity = s.Quantity, UomId = s.UomId, DivisionId = s.DivisionId, OilTypeId = s.OilTypeId, PackGroupId = s.PackGroupId }).ToList();
                //Process the SKU's0
                if (skuList != null && skuList.Any())
                {
                    //Get Depots
                    inputDto.DepotIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(_ => _.PlantId == inputDto.PlantId).Select(_ => _.DepotId).ToList();
                    if (inputDto.DepotIds != null && inputDto.DepotIds.Any())
                    {
                        //Get Freight Route details
                        //var freightRouteIds = _emamiContext.FreightRoutes.AsNoTracking().Where(_ => _.IsActive).Select(_ => _.Id).ToList();
                        //if (freightRouteIds != null && freightRouteIds.Any())
                        //{
                        //Get Transport Modes
                        var transportModeData = _emamiContext.TransportModes.AsNoTracking().Where(w => w.IsActive).Select(s => s);
                        var transportModes = transportModeData.Select(s => s.Id).ToList();
                        if (transportModes != null && transportModes.Any())
                        {
                            _logger.Info("Before Calculation Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));

                            pricePublishContext = _emamiContext.PricePublish.AsNoTracking().FirstOrDefault(_ => _.StatusId == (int)DTO.Enums.PublishStatus.Started && DbFunctions.TruncateTime(_.StartDate) == DbFunctions.TruncateTime(currentDate));
                            if (pricePublishContext == null || pricePublishContext.StatusId != (int)DTO.Enums.PublishStatus.Started)
                            {
                                pricePublishContext = new PricePublish()
                                {
                                    StatusId = (long)DTO.Enums.PublishStatus.Started,
                                    StartDate = currentDate,
                                    IsPublish = false,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = currentDate,
                                };
                                //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                //{
                                //    pricePublishContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction;
                                //}
                                //else
                                //{
                                pricePublishContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                                //}
                                _emamiContext.PricePublish.Add(pricePublishContext);
                                _emamiContext.SaveChanges();

                                //Get Common data
                                var MaterialCostData = _emamiContext.MaterialCosts.AsNoTracking()
                                    .Where(_ => currentDate >= _.ValidFrom && currentDate <= _.ValidTo && _.IsActive);
                                var PackingCostData = _emamiContext.PackingCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var DepotCostData = _emamiContext.DepotCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var DetentionCostData = _emamiContext.DetentionCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var ProfitMarginsData = _emamiContext.ProfitMargins.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var CushionMarginData = _emamiContext.CushionMargins.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var SchemeCostData = _emamiContext.SchemeCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var PrimaryFreightData = _emamiContext.PrimaryFreights.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var SecondaryFreightData = _emamiContext.SecondaryFreights.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var HoneycombCostData = _emamiContext.HoneycombCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                var RaMarginData = _emamiContext.RaMargin.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);

                                var LoadCapacityConversionData = _emamiContext.LoadCapacityConversion.AsNoTracking()
                                   .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);

                                //States
                                List<long> stateIds = inputDto.StateIds;

                                //Get Rasoi oiltypes
                                //var rasoiOilTypeIds = _emamiContext.OilTypes.AsNoTracking().Where(w => w.IsRasoi).Select(s => s.Id).ToList();

                                foreach (var sku in skuList)
                                {
                                    var isValidSku = true;
                                    var valErrorMessage = sku.SkuName + " ~ " + sku.SkuCode + " ~ ~ ~ ~ ~ ~ " + Constants.MissingSkuRequiredField;

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

                                    //If Vertical type is (SpecialityFat or HBC) and it contains rasoi oiltypes condition true
                                    if (sku.DivisionId == (int)DTO.Enums.Division.SpecialityFat || (sku.DivisionId == (int)DTO.Enums.Division.Hbc)) /*rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(sku.OilTypeId.GetValueOrDefault()*/
                                    {
                                        //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), ProcessCost is required
                                        if (sku.ProcessCost <= 0)
                                        {
                                            valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuProcessCostField, valErrorMessage);
                                            isValidSku = false;
                                        }

                                        //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), SKU ingredients is required
                                        //if (!_emamiContext.SkuIngrediant.AsNoTracking().Any(_ => _.SkuId == sku.Id))
                                        //{
                                        //    valErrorMessage = Constants.BindErrorMessage(Constants.SkuIngredientNotAdded, valErrorMessage);
                                        //    isValidSku = false;
                                        //}
                                    }

                                    if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                                    {
                                        valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuUom2Field, valErrorMessage);
                                        isValidSku = false;
                                    }

                                    if (!_emamiContext.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                                    {
                                        valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuUom3Field, valErrorMessage);
                                        isValidSku = false;
                                    }
                                    if (isValidSku)
                                    {
                                        _logger.Info("Valid  SKU Calculation Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                                        #region Price Calculation

                                        var resultDtoList = new List<ResultDto>();
                                        inputDto.SkuId = sku.Id;
                                        var verticalId = 0L;
                                        var oilTypeId = 0L;
                                        var oilPackingTypeId = 0L;
                                        var uomId = 0L;
                                        decimal litreConversion = 0;
                                        decimal quantity = 0;
                                        decimal materialCost = 0;
                                        decimal packingCost = 0;
                                        decimal noofPiecesperCase = 0;
                                        long materialCostId = 0;
                                        List<long> ingredientCostId = new List<long>();
                                        long packingCostId = 0;
                                        bool isError = false;

                                        var skuId = inputDto.SkuId;
                                        var plantId = inputDto.PlantId;

                                        //Get SKU details
                                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                                        oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                                        oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                                        uomId = Convert.ToInt64(skuContext.UomId);
                                        quantity = skuContext.Quantity;
                                        var dataMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ ~ ~ ~ ~ ~ " + Constants.DataMissingToCalculate;


                                        //Get OilType details
                                        var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                                        verticalId = oilTypeContext.DivisionId;
                                      //  litreConversion = oilTypeContext.LitreConversion;


                                        //Get SkuUomMapping details
                                        var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                        if (skuUomContext != null)
                                        {
                                            noofPiecesperCase = skuUomContext.ConversionFactor;
                                        }


                                        //Material Cost calculations
                                        decimal formulationCost = 0;
                                        if (verticalId == (int)DTO.Enums.Division.Hbc)/*rasoiOilTypeIds == null || !rasoiOilTypeIds.Any()) || !rasoiOilTypeIds.Contains(oilTypeId)*/
                                        {
                                            var materialCostContext = MaterialCostData.FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                                            if (materialCostContext != null)
                                            {
                                                materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                                                materialCost = noofPiecesperCase * materialCost;
                                                materialCostId = materialCostContext.Id;
                                            }
                                            else
                                            {
                                                dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost, dataMissingErrorMessage);
                                                isError = true;
                                            }
                                        }
                                        else if (verticalId == (int)DTO.Enums.Division.SpecialityFat || (verticalId == (int)DTO.Enums.Division.Hbc)) //rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(oilTypeId)//
                                        {

                                            //var skuIngredientList = _emamiContext.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId && _.OilTypeId == oilTypeId).ToList();
                                            //foreach (var skuIngredient in skuIngredientList)
                                            //{
                                            //    var ingredientCost = _emamiContext.IngredientCost.AsNoTracking()
                                            //        .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                            //    if (ingredientCost != null)
                                            //    {

                                            //        var oneKgIngredientCost = (ingredientCost.LooseOilRate * skuIngredient.Percentage) / 100;
                                            //        formulationCost = formulationCost + oneKgIngredientCost;

                                            //        ingredientCostId.Add(ingredientCost.Id);
                                            //    }
                                            //    else
                                            //    {
                                            //        dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost, dataMissingErrorMessage);
                                            //        isError = true;
                                            //    }
                                            //}

                                            var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;
                                            materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, specialityFatMaterialCost, litreConversion);
                                            materialCost = noofPiecesperCase * materialCost;

                                            formulationCost = _resultService.GetSkuQuanityRate(uomId, quantity, formulationCost, litreConversion);
                                            formulationCost = noofPiecesperCase * formulationCost;

                                            //var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;
                                            //materialCost = noofPiecesperCase * specialityFatMaterialCost;
                                            //formulationCost = noofPiecesperCase * formulationCost;
                                        }

                                        //Packing Cost calculations
                                        var packingCostContext = PackingCostData.FirstOrDefault(_ => _.PlantId == plantId && _.SkuId == skuId);
                                        if (packingCostContext != null)
                                        {
                                            packingCost = packingCostContext.SalesPackingCost;
                                            packingCostId = packingCostContext.Id;
                                        }
                                        else
                                        {
                                            dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToPackingCost, dataMissingErrorMessage);
                                            isError = true;
                                        }

                                        if (!isError)
                                        {
                                            foreach (long depotId in inputDto.DepotIds)
                                            {
                                                isError = false;
                                                decimal depoCost = 0;
                                                decimal detentionCost = 0;
                                                long depotCostId = 0;
                                                long detentionCostId = 0;
                                                var depotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == depotId)?.Name;
                                                dataMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName + " ~ ~ ~ ~ ~ " + Constants.DataMissingToCalculate;

                                                //Depot Cost calculations
                                                var depoCostContext = DepotCostData.FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                                                if (depoCostContext != null)
                                                {
                                                    depoCost = _resultService.GetSkuQuanityRate(uomId, quantity, depoCostContext.RatePerMt, litreConversion);
                                                    depoCost = noofPiecesperCase * depoCost;
                                                    depotCostId = depoCostContext.Id;
                                                }
                                                else
                                                {
                                                    dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToDepoCost, dataMissingErrorMessage);
                                                    isError = true;
                                                }

                                                //Detention Cost calculations
                                                var detentionCostContext = DetentionCostData.FirstOrDefault(_ => _.DepotId == depotId && _.DivisionId == verticalId);
                                                if (detentionCostContext != null)
                                                {
                                                    detentionCost = _resultService.GetSkuQuanityRate(uomId, quantity, detentionCostContext.RatePerMt, litreConversion);
                                                    detentionCost = noofPiecesperCase * detentionCost;
                                                    detentionCostId = detentionCostContext.Id;
                                                }
                                                else
                                                {
                                                    dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToDetentionCost, dataMissingErrorMessage);
                                                    isError = true;
                                                }

                                                if (!isError)
                                                {
                                                    //Process the Cities
                                                    foreach (var stateId in stateIds)
                                                    {
                                                        isError = false;
                                                        decimal marginCost = 0;
                                                        decimal cushionMarginCost = 0;
                                                        decimal schemeCostRecovery = 0;
                                                        long marginCostId = 0;
                                                        long cushionMarginCostId = 0;
                                                        long schemeCostRecoveryId = 0;
                                                        var stateName = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == stateId)?.StateName;
                                                        dataMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName + " ~ " + stateName + " ~ ~ ~ ~ " + Constants.DataMissingToCalculate;
                                                        if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                                        {
                                                            //Margin Cost calculations
                                                            var marginCostContext = ProfitMarginsData.FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId);
                                                            if (marginCostContext != null)
                                                            {
                                                                marginCost = _resultService.GetSkuQuanityRate(uomId, quantity, marginCostContext.RatePerMt, litreConversion);
                                                                marginCost = noofPiecesperCase * marginCost;
                                                                marginCostId = marginCostContext.Id;
                                                            }
                                                            else
                                                            {
                                                                dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToMarginCost, dataMissingErrorMessage);
                                                                isError = true;
                                                            }

                                                            //Cushion Margin Cost calculations
                                                            var cushionMarginCostContext = CushionMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId);
                                                            if (cushionMarginCostContext != null)
                                                            {
                                                                cushionMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                                                                cushionMarginCost = noofPiecesperCase * cushionMarginCost;
                                                                cushionMarginCostId = cushionMarginCostContext.Id;
                                                            }
                                                            else
                                                            {
                                                                dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToCushionMarginCost, dataMissingErrorMessage);
                                                                isError = true;
                                                            }
                                                        }

                                                        //Scheme Cost Recovery calculations
                                                        var schemeCostContext = SchemeCostData.FirstOrDefault(_ => _.PackGroupId == sku.PackGroupId && _.OilTypeId == sku.OilTypeId && _.StateId == stateId);
                                                        if (schemeCostContext != null)
                                                        {
                                                            schemeCostRecovery = _resultService.GetSkuQuanityRate(uomId, quantity, schemeCostContext.RatePerMt, litreConversion);
                                                            schemeCostRecovery = noofPiecesperCase * schemeCostRecovery;
                                                            schemeCostRecoveryId = schemeCostContext.Id;
                                                        }

                                                        //finalPrice = materialCost + ((packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                                        // honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                        if (!isError)
                                                        {

                                                            //foreach (var freightRouteId in freightRouteIds)
                                                            //{
                                                            //if (transportModes != null && transportModes.Any())
                                                            //{
                                                            isError = false;
                                                            //var freightRouteName = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == freightRouteId)?.Name;

                                                            foreach (var transportId in transportModes)
                                                            {
                                                                var transportMode = string.Empty;
                                                                var transportModeName = transportModeData.FirstOrDefault(_ => _.Id == transportId)?.Name;

                                                                dataMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName
                                                                    + " ~ " + stateName + " ~ " + transportModeName + " ~ ~ " + Constants.LoadCapacityMissing;

                                                                var loadCapacityContextList = LoadCapacityConversionData.Where(_ => _.SkuId == skuId
                                                                                                      && _.DivisionId == verticalId && _.TransportModeId == transportId).ToList();

                                                                if (loadCapacityContextList != null && loadCapacityContextList.Any())
                                                                {
                                                                    foreach (var loadCapacityItem in loadCapacityContextList)
                                                                    {
                                                                        isError = false;
                                                                        decimal primaryFrieght = 0;
                                                                        decimal secondaryFrieght = 0;
                                                                        decimal honeycombCost = 0;
                                                                        decimal raMarginCost = 0;
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
                                                                        long honeycombCostId = 0;
                                                                        long raMarginCostId = 0;
                                                                        var resultDtoSub = new ResultDto();
                                                                        var loadCapacity = loadCapacityItem.LoadCapacity;
                                                                        var loadQuantityCase = loadCapacityItem.LoadQuantity;

                                                                        dataMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName
                                                                            + " ~ " + stateName + " ~ " + transportModeName + " ~ " +
                                                                            loadCapacity + " ~ " + Constants.DataMissingToCalculate;

                                                                        var primaryFrieghtContext = PrimaryFreightData.FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                                                                        _.VerticalId == verticalId && _.TransportModeId == transportId && _.LoadCapacity == Constants.DefaultLoadQuantity);
                                                                        if (primaryFrieghtContext != null)
                                                                        {
                                                                            var defaultLoadCapacity16MT = loadCapacityContextList.FirstOrDefault(_ => _.LoadCapacity == Constants.DefaultLoadQuantity);
                                                                            if (defaultLoadCapacity16MT != null)
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

                                                                        var secondaryFrieghtContext = SecondaryFreightData.FirstOrDefault(_ => _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == depotId && _.VerticalId == verticalId);
                                                                        if (secondaryFrieghtContext != null)
                                                                        {
                                                                            secondaryFrieght = secondaryFrieghtContext.SalesFreight;
                                                                            secondaryFrieght = (secondaryFrieght / loadQuantityCase) * 1;
                                                                            secondaryFrieghtId = secondaryFrieghtContext.Id;
                                                                        }
                                                                        //else
                                                                        //{
                                                                        //    isError = true;
                                                                        //}

                                                                        var secondaryFrieghtContextForPlant = SecondaryFreightData.FirstOrDefault(_ => _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == plantId && _.VerticalId == verticalId);
                                                                        if (secondaryFrieghtContextForPlant != null)
                                                                        {
                                                                            secondaryFrieghtForPlant = secondaryFrieghtContextForPlant.SalesFreight;
                                                                            secondaryFrieghtForPlant = (secondaryFrieghtForPlant / loadQuantityCase) * 1;
                                                                            secondaryFrieghtForPlantId = secondaryFrieghtContextForPlant.Id;
                                                                        }
                                                                        //else
                                                                        //{
                                                                        //    isError = true;
                                                                        //}                                                                                

                                                                        //Honeycomb Cost calculations
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
                                                                            dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToHoneyCombCost, dataMissingErrorMessage);
                                                                            isError = true;
                                                                        }

                                                                        if (!isError)
                                                                        {
                                                                            if (primaryFrieght > 0 && secondaryFrieght > 0)
                                                                            {
                                                                                finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                                                                              honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            }
                                                                            if (primaryFrieght > 0)
                                                                            {
                                                                                exDepotPrice = ((materialCost + packingCost + primaryFrieght + depoCost + detentionCost +
                                                                                                         marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            }
                                                                            exPlantPrice = ((materialCost + packingCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            if (secondaryFrieghtForPlant > 0)
                                                                            {
                                                                                forPlantPrice = ((materialCost + packingCost + secondaryFrieghtForPlant +
                                                                                                  honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            }
                                                                            if (primaryFrieght > 0)
                                                                            {
                                                                                exRakePrice = ((materialCost + packingCost + primaryFrieght +
                                                                                                 honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            }

                                                                            var pricingContext = new Pricing()
                                                                            {
                                                                                SkuId = skuId,
                                                                                OilTypeId = oilTypeId,
                                                                                OilPackingTypeId = oilPackingTypeId,
                                                                                PlantId = plantId,
                                                                                // DepotId = depotId,
                                                                                //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                                                                //StateId = (int)TraditionalProcessFinalPrice.StateId,
                                                                                //CityId = (int)TraditionalProcessFinalPrice.CityId,
                                                                                // Price = pricingLiveContext.Price,
                                                                                //SalesOrganizationId = salesOrganizationId,
                                                                                //DistributionChannelId = distributionChannelId,
                                                                                //DivisionId = divisionId,
                                                                                //ValidFrom = pricingLiveContext.ValidFrom,
                                                                                //ValidTo = pricingLiveContext.ValidTo,//StateId = (int)stateId,
                                                                                //FrieghtRouteId = freightRouteId,
                                                                                //FrieghtZoneId = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == freightRouteId)?.FreightZoneId ?? 0,
                                                                                //TransportModeId = transportId,
                                                                                //LoadQuantity = loadCapacity,
                                                                                //SumOfIngredientCost = formulationCost,
                                                                                CreatedBy = inputDto.LoginUserId,
                                                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                                                //IsActive = true,
                                                                            };

                                                                            //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                                            //{
                                                                            //    var raMarginCostContext = RaMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId &&
                                                                            //    _.OilPackingTypeId == oilPackingTypeId);
                                                                            //    if (raMarginCostContext != null)
                                                                            //    {
                                                                            //        raMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, raMarginCostContext.RatePerMt, litreConversion);
                                                                            //        raMarginCost = noofPiecesperCase * raMarginCost;
                                                                            //        raMarginCostId = raMarginCostContext.Id;
                                                                            //    }
                                                                            //    else
                                                                            //    {
                                                                            //        isError = true;
                                                                            //        dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost + " - ", dataMissingErrorMessage);
                                                                            //    }

                                                                            //    exDepotPrice = exDepotPrice > 0 ? (exDepotPrice + raMarginCost) : 0;
                                                                            //    exPlantPrice = exPlantPrice + raMarginCost;
                                                                            //    forPlantPrice = forPlantPrice > 0 ? (forPlantPrice + raMarginCost) : 0;
                                                                            //    exRakePrice = exRakePrice > 0 ? (exRakePrice + raMarginCost) : 0;
                                                                            //    //pricingContext.ForDepotPrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;
                                                                            //    //pricingContext.ForRakePrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;

                                                                            //    //pricingContext.TpPrice = exPlantPrice;
                                                                            //    finalPrice = exPlantPrice > 0 ? (exPlantPrice + raMarginCost) : 0;
                                                                            //    //pricingContext.ClearanceRate = finalPrice > 0 ? (finalPrice * inputDto.CounterBidLimit) : 0;
                                                                            //    //pricingContext.CounterBidOffer = finalPrice > 0 ? (finalPrice + inputDto.BpCpJump) : 0;
                                                                            //    //pricingContext.BaseRate = finalPrice;
                                                                            //    //pricingContext.XMargin = inputDto.XMargin;
                                                                            //    //pricingContext.FinalRate = finalPrice > 0 ? (finalPrice + inputDto.XMargin) : 0;
                                                                            //    //pricingContext.CounterBidLimit = inputDto.CounterBidLimit;
                                                                            //    //pricingContext.BpCpJumb = inputDto.BpCpJump;
                                                                            //    pricingContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction;
                                                                            //}
                                                                            //else
                                                                            //{
                                                                            //pricingContext.ForDepotPrice = finalPrice;
                                                                            //pricingContext.ForRakePrice = finalPrice;
                                                                            //pricingContext.FinalRate = finalPrice;
                                                                            //pricingContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                                                                            //pricingContext.TpPrice = finalPrice;
                                                                            //}
                                                                            //pricingContext.MaterialCost = materialCost;
                                                                            //pricingContext.PackingCost = packingCost;
                                                                            //pricingContext.Premium = premium;
                                                                            //pricingContext.Discount = discount;
                                                                            //pricingContext.PrimaryFrieght = primaryFrieght;
                                                                            //pricingContext.SecondaryFrieght = secondaryFrieght;
                                                                            //pricingContext.PlantSecondaryFrieght = secondaryFrieghtForPlant;
                                                                            //pricingContext.DepotCost = depoCost;
                                                                            //pricingContext.DetentionCost = detentionCost;
                                                                            //pricingContext.HoneycombCost = honeycombCost;
                                                                            //pricingContext.Margin = marginCost;
                                                                            //pricingContext.CushionMargin = cushionMarginCost;
                                                                            //pricingContext.SchemeCostRecovery = schemeCostRecovery;
                                                                            //pricingContext.ProcessCost = schemeCostRecovery;
                                                                            //pricingContext.RaMargin = raMarginCost;

                                                                            //pricingContext.ExPlantPrice = exPlantPrice;
                                                                            //pricingContext.ExDepotPrice = exDepotPrice;
                                                                            //pricingContext.ForPlantPrice = forPlantPrice;
                                                                            //pricingContext.ExRakePrice = exRakePrice;
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
                                                                            //outputDto1.CityName = cityData.CityName;
                                                                            //outputDto1.CityId = cityId;
                                                                            //outputDto1.StateName = stateName;

                                                                            //unickId++;
                                                                            //outputDto1.Id = unickId;

                                                                            //pricingContext.FrieghtZoneId = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == freightRouteId)?.FreightZoneId ?? 0;
                                                                            //var freightRoutes = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == freightRouteId);

                                                                            //if (freightRoutes != null)
                                                                            //{
                                                                            //    pricingContext.FrieghtZoneId = freightRoutes.FreightZoneId;
                                                                            //    //outputDto1.FreightRouteName = freightRoutes.Name;
                                                                            //}
                                                                            //pricingContext.FrieghtRouteId = freightRouteId;

                                                                            if (!isError)
                                                                            {
                                                                                //outputDtoMain.Add(outputDto1);
                                                                                //pricingContext.IsPublish = false;
                                                                                //pricingContext.PublishId = pricePublishContext.Id;
                                                                                pricings.Add(pricingContext);
                                                                                count++;
                                                                                isAvailable = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                dataMissingErrorMessage = dataMissingErrorMessage + "|";
                                                                                errorMessageList.Add(dataMissingErrorMessage);
                                                                            }
                                                                            isError = false;
                                                                        }
                                                                        else
                                                                        {
                                                                            dataMissingErrorMessage = dataMissingErrorMessage + "|";
                                                                            errorMessageList.Add(dataMissingErrorMessage);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.LoadCapacityMissing + " ~ ", dataMissingErrorMessage);
                                                                    isError = true;
                                                                    dataMissingErrorMessage = dataMissingErrorMessage + "|";
                                                                    errorMessageList.Add(dataMissingErrorMessage);
                                                                }
                                                            }
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            dataMissingErrorMessage = dataMissingErrorMessage + "|";
                                                            errorMessageList.Add(dataMissingErrorMessage);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    dataMissingErrorMessage = dataMissingErrorMessage + "|";
                                                    errorMessageList.Add(dataMissingErrorMessage);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dataMissingErrorMessage = dataMissingErrorMessage + "|";
                                            errorMessageList.Add(dataMissingErrorMessage);
                                        }

                                        _logger.Info("Completed :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                                        #endregion

                                    }
                                    else
                                    {
                                        valErrorMessage = valErrorMessage + "|";
                                        errorMessageList.Add(valErrorMessage);
                                    }
                                }
                                if (isAvailable)
                                {
                                    _emamiContext.BulkInsertProxy(pricings);
                                    _emamiContext.SaveChanges();
                                    pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Completed;
                                }
                                else
                                {
                                    pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Failed;
                                }
                                if (errorMessageList != null && errorMessageList.Any())
                                {
                                    pricePublishContext.ErrorMessage = string.Join("", errorMessageList);

                                }
                                pricePublishContext.EndDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SaveChanges();
                                if (isAvailable)
                                {
                                    smsContent = Constants.PriceCalculationCompleted.Replace(Constants.Count, count.ToString()).Replace(Constants.StartTime, pricePublishContext.StartDate.ToString("hh:mm tt"))
                                        .Replace(Constants.EndTime, pricePublishContext.EndDate.ToString("hh:mm tt"));
                                }
                                else
                                {
                                    smsContent = Constants.PriceCalculationFailed;
                                }
                            }
                            //else
                            //{
                            //    errorMessageList.Add(Constants.PriceCalculationInprocess);
                            //}
                        }
                        //else
                        //{
                        //    errorMessageList.Add(Constants.TransportModeNotAvailable);
                        //}
                        //}
                        //else
                        //{
                        //    errorMessageList.Add(Constants.FreightRouteNotAvailable);
                        //}
                    }
                    //else
                    //{
                    //    errorMessageList.Add(Constants.DepotsNotAvailable);
                    //}
                }
                //else
                //{
                //    errorMessageList.Add(Constants.SKUsNotAvailable);
                //}
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
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                if (pricePublishContext != null && (pricePublishContext.StatusId == (int)DTO.Enums.PublishStatus.Started || pricePublishContext.StatusId == (int)DTO.Enums.PublishStatus.Failed))
                {
                    pricePublishContext.StatusId = (int)DTO.Enums.PublishStatus.Failed;
                    _emamiContext.SaveChanges();
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
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        /*
        /// <summary>
        /// Method to calculate final price
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto SkuFinalPriceCalculationOld(SkuFinalpriceListInputDto inputDto)
        {
            _methodName = "SkuFinalPriceCalculation";
            var resultDto = new ResultDto();
            var outputDto = new SkuFinalpriceListOutputDto();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var skuId = inputDto.SkuId;
                var incoTermsId = 0L;
                var plantId = inputDto.PlantId;
                var depotId = inputDto.DepotId;
                var verticalId = 0L;
                var oilTypeId = 0L;
                var transportModeId = inputDto.TransportModeId;
                var oilPackingTypeId = 0L;
                var cityId = inputDto.CityId;
                var stateId = inputDto.StateId;
                var uomId = 0L;
                var freightRouteId = inputDto.FreightRouteId;

                var litreConversion = (decimal)0;
                var quantity = (decimal)0;
                var materialCost = (decimal)0;
                var packingCost = (decimal)0;
                var primaryFrieght = (decimal)0;
                var secondaryFrieght = (decimal)0;
                var depoCost = (decimal)0;
                var detentionCost = (decimal)0;
                var honeycombCost = (decimal)0;
                var marginCost = (decimal)0;
                var cushionMarginCost = (decimal)0;
                var schemeCostRecovery = (decimal)0;
                var raMarginCost = (decimal)0;
                var discount = (decimal)0;
                var premium = (decimal)0;

                var exPlantPrice = (decimal)0;
                var forPlantPrice = (decimal)0;
                var exDepotPrice = (decimal)0;
                var forDepotPrice = (decimal)0;
                var exRakePrice = (decimal)0;
                var forRakePrice = (decimal)0;

                var finalPrice = (decimal)0;
                bool isError = false;


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
                var errorMessage = skuContext.SkuName + " : " + Constants.DataMissingToCalculate;

                var noofPiecesperCase = (decimal)0; ;
                var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                if (skuUomContext != null)
                {
                    noofPiecesperCase = skuUomContext.ConversionFactor;
                }

                var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                if (oilTypeContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                verticalId = oilTypeContext.VerticalId;
                litreConversion = oilTypeContext.LitreConversion;




                //Material Cost
                if (verticalId == (int)DTO.Enums.Vertical.Hbc)
                {
                    var materialCostContext = _emamiContext.MaterialCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                    if (materialCostContext != null)
                    {
                        materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                        materialCost = noofPiecesperCase * materialCost;
                    }
                    else
                    {
                        isError = true;
                        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost + " - ", errorMessage);
                    }
                }

                //Packing Cost
                var packingCostContext = _emamiContext.PackingCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.SkuId == skuId);
                if (packingCostContext != null)
                {
                    var noofPiecesperMt = (decimal)0; ;
                    var skuUomMtContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                    if (skuUomMtContext != null)
                    {
                        noofPiecesperMt = skuUomContext.ConversionFactor;
                    }

                    packingCost = (packingCostContext.SalesPackingCost / noofPiecesperMt) * noofPiecesperCase;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToPackingCost + " - ", errorMessage);
                }

                //Primary Frieght
                var primaryFrieghtContext = _emamiContext.PrimaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                _.VerticalId == verticalId && _.TransportModeId == transportModeId);
                if (primaryFrieghtContext != null)
                {
                    primaryFrieght = _resultService.GetSkuQuanityRate(uomId, quantity, primaryFrieghtContext.SalesFreight, litreConversion);
                    primaryFrieght = noofPiecesperCase * primaryFrieght;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToPrimaryFrieght + " - ", errorMessage);
                }

                //Secondary Frieght
                var secondaryFrieghtContext = _emamiContext.SecondaryFreights.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == depotId
            && _.FreightRouteId == freightRouteId
            && _.VerticalId == verticalId && _.TransportModeId == transportModeId);
                if (secondaryFrieghtContext != null)
                {
                    secondaryFrieght = _resultService.GetSkuQuanityRate(uomId, quantity, secondaryFrieghtContext.SalesFreight, litreConversion);
                    secondaryFrieght = noofPiecesperCase * secondaryFrieght;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSecondaryFrieght + " - ", errorMessage);
                }

                //Depo Cost
                var depoCostContext = _emamiContext.DepotCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == depotId && _.VerticalId == verticalId);
                if (depoCostContext != null)
                {
                    depoCost = _resultService.GetSkuQuanityRate(uomId, quantity, depoCostContext.RatePerMt, litreConversion);
                    depoCost = noofPiecesperCase * depoCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToDepoCost + " - ", errorMessage);
                }

                //Detention Cost
                var detentionCostContext = _emamiContext.DetentionCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.DepotId == depotId && _.VerticalId == verticalId);
                if (detentionCostContext != null)
                {
                    detentionCost = _resultService.GetSkuQuanityRate(uomId, quantity, detentionCostContext.RatePerMt, litreConversion);
                    detentionCost = noofPiecesperCase * detentionCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToDetentionCost + " - ", errorMessage);
                }

                //Honeycomb Cost
                var honeycombCostContext = _emamiContext.HoneycombCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.PlantId == plantId && _.StateId == stateId &&
            _.SkuId == skuId && _.TransportModeId == transportModeId);
                if (honeycombCostContext != null)
                {
                    honeycombCost = _resultService.GetSkuQuanityRate(uomId, quantity, honeycombCostContext.RatePerMt, litreConversion);
                    honeycombCost = noofPiecesperCase * honeycombCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToHoneyCombCost + " - ", errorMessage);
                }

                //Margin Cost
                var marginCostContext = _emamiContext.ProfitMargins.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId &&
                _.OilPackingTypeId == oilPackingTypeId);
                if (marginCostContext != null)
                {
                    marginCost = _resultService.GetSkuQuanityRate(uomId, quantity, marginCostContext.RatePerMt, litreConversion);
                    marginCost = noofPiecesperCase * marginCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToMarginCost + " - ", errorMessage);
                }

                //Cushion Margin Cost
                var cushionMarginCostContext = _emamiContext.CushionMargins.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                _.OilPackingTypeId == oilPackingTypeId);
                if (cushionMarginCostContext != null)
                {
                    cushionMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                    cushionMarginCost = noofPiecesperCase * cushionMarginCost;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToCushionMarginCost + " - ", errorMessage);
                }

                //Scheme Cost Recovery
                var schemeCostContext = _emamiContext.SchemeCosts.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId);
                if (schemeCostContext != null)
                {
                    schemeCostRecovery = _resultService.GetSkuQuanityRate(uomId, quantity, schemeCostContext.RatePerMt, litreConversion);
                    schemeCostRecovery = noofPiecesperCase * schemeCostRecovery;
                }
                else
                {
                    isError = true;
                    errorMessage = Constants.BindErrorMessage(Constants.DataMissingToSchemeCost + " - ", errorMessage);
                }


                if (verticalId != (int)DTO.Enums.Vertical.Hbc)
                {
                    var formulationCost = (decimal)0;
                    var skuIngredientList = _emamiContext.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId).ToList();
                    foreach (var skuIngredient in skuIngredientList)
                    {
                        var ingredientCost = _emamiContext.IngredientCost.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                        if (ingredientCost != null)
                        {

                            var qtySplitup = (skuIngredient.Percentage / 100) * quantity;
                            var oneKgIngredientCost = (ingredientCost.LooseOilRate / 1000) * qtySplitup;
                            formulationCost = formulationCost + oneKgIngredientCost;
                        }
                        else
                        {
                            isError = true;
                            errorMessage = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost + " - ", errorMessage);
                        }
                    }

                    var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;
                    materialCost = noofPiecesperCase * specialityFatMaterialCost;

                    formulationCost = noofPiecesperCase * formulationCost;
                    outputDto.IngredientCost = formulationCost;

                    finalPrice = materialCost + ((packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                     honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                }
                else
                {

                    finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                         honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                }

                exDepotPrice = ((materialCost + packingCost + primaryFrieght + depoCost + detentionCost +
                                         marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                exPlantPrice = ((materialCost + packingCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                forPlantPrice = ((materialCost + packingCost + secondaryFrieght +
                                  honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;

                exRakePrice = ((materialCost + packingCost + primaryFrieght +
                                 honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;



                if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                {
                    //RA Margin Cost
                    var raMarginCostContext = _emamiContext.RaMargin.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.SkuId == skuId && _.CityId == cityId &&
                    _.OilPackingTypeId == oilPackingTypeId);
                    if (raMarginCostContext != null)
                    {
                        raMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, raMarginCostContext.RatePerMt, litreConversion);
                        raMarginCost = noofPiecesperCase * raMarginCost;
                    }
                    else
                    {
                        isError = true;
                        errorMessage = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost + " - ", errorMessage);
                    }

                    exDepotPrice = exDepotPrice + raMarginCost;
                    exPlantPrice = exPlantPrice + raMarginCost;
                    forPlantPrice = forPlantPrice + raMarginCost;
                    exRakePrice = exRakePrice + raMarginCost;
                    outputDto.ForDepotPrice = finalPrice + raMarginCost;
                    outputDto.ForRakePrice = finalPrice + raMarginCost;

                    outputDto.TpPrice = finalPrice;
                    finalPrice = finalPrice + raMarginCost;
                    outputDto.ClearanceRate = finalPrice * inputDto.CounterBidLimit;
                    outputDto.CounterbidOffer = finalPrice + inputDto.BpCpJump;
                    outputDto.BaseRate = finalPrice;
                    outputDto.XMarginCost = inputDto.XMargin;
                    outputDto.FinalPrice = finalPrice + inputDto.XMargin;
                }
                else
                {
                    outputDto.ForDepotPrice = finalPrice;
                    outputDto.ForRakePrice = finalPrice;
                    outputDto.FinalPrice = finalPrice;
                }

                outputDto.MaterialCost = materialCost;
                outputDto.PackingCost = packingCost;
                outputDto.Premium = premium;
                outputDto.Discount = discount;
                outputDto.PrimaryFrieght = primaryFrieght;
                outputDto.SecondaryFrieght = secondaryFrieght;
                outputDto.DepoCost = depoCost;
                outputDto.DetentionCost = detentionCost;
                outputDto.HoneycombCost = honeycombCost;
                outputDto.MarginCost = marginCost;
                outputDto.CushionMarginCost = cushionMarginCost;
                outputDto.SchemeCost = schemeCostRecovery;
                outputDto.RaMarginCost = raMarginCost;

                outputDto.ExPlantPrice = exPlantPrice;
                outputDto.ExDepotPrice = exDepotPrice;
                outputDto.ForPlantPrice = forPlantPrice;
                outputDto.ExRakePrice = exRakePrice;


                //var currentDate = DateTime.UtcNow;
                //outputDto.FinalPrice = inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess ? finalPrice : 0;
                //outputDto.IsAddedForPricing = _emamiContext.Pricing.Any(s => s.SaudaBookingTypeId == inputDto.SaudaBookingTypeId && s.SkuId == inputDto.SkuId && DbFunctions.TruncateTime(s.BiddingDate) == currentDate.Date);

                if (isError)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = errorMessage;
                    resultDto.SuccessDto.Response = outputDto;
                }
                else
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = outputDto;
                }
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        */

        public void RAFinalPriceNotificationUpdated(object notifyinputDto)
        {
            ResultDto result = new ResultDto();
            //try
            //{
            //    string connetionString = Config.DBConnectionString;
            //    SqlConnection cnn;
            //    cnn = new SqlConnection(connetionString);
            //    cnn.Open();
            //    FinalPricePublishDto inputDto = (FinalPricePublishDto)notifyinputDto;
            //    SqlCommand publishStatescmd = new SqlCommand("select *from Pricings where PublishId=@publishId", cnn);
            //    publishStatescmd.Parameters.AddWithValue("@publishId", inputDto.PublishId);
            //    SqlDataAdapter da = new SqlDataAdapter(publishStatescmd);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    IList<Pricing> pricingsContext = dt.AsEnumerable().Select(row =>
            //        new Pricing
            //        {
            //            ForPlantPrice = row.Field<decimal>("ForPlantPrice"),
            //            ExPlantPrice = row.Field<decimal>("ExPlantPrice"),
            //            ForDepotPrice = row.Field<decimal>("ForDepotPrice"),
            //            ExDepotPrice = row.Field<decimal>("ExDepotPrice"),
            //            ForRakePrice = row.Field<decimal>("ForRakePrice"),
            //            ExRakePrice = row.Field<decimal>("ExRakePrice"),
            //            StateId = row.Field<int>("StateId"),
            //            TransportModeId = row.Field<long>("TransportModeId"),
            //            PlantId = row.Field<long>("PlantId"),
            //            DepotId = row.Field<long>("DepotId"),
            //            FrieghtRouteId = row.Field<long>("FrieghtRouteId"),
            //            LoadQuantity = row.Field<decimal>("LoadQuantity"),
            //            SkuId = row.Field<long>("SkuId"),
            //            XMargin = row.Field<decimal>("XMargin"),
            //            IsPublish = false,
            //        }).ToList();
            //    //List<Pricing> publishedPricings = new List<Pricing>();
            //    List<int> allStateIds = new List<int>();
            //    if (pricingsContext != null && pricingsContext.Any())
            //    {
            //        allStateIds = pricingsContext.Select(_ => _.StateId).Distinct().ToList();
            //    }
            //    SqlCommand biddingWindowCmd = new SqlCommand("Select * from BiddingWindowTimings where id=@biddingWindowId", cnn);
            //    biddingWindowCmd.Parameters.AddWithValue("@biddingWindowId", inputDto.BiddingWindowId);
            //    string date = string.Empty;
            //    string fromTime = string.Empty;
            //    string toTime = string.Empty;
            //    using (SqlDataReader biddingWindowReader = biddingWindowCmd.ExecuteReader())
            //    {
            //        if (biddingWindowReader.Read())
            //        {
            //            date = ((DateTime)biddingWindowReader["BiddingDate"]).ToString("dddd, dd MMMM yyyy");
            //            DateTime dtFromTime = DateTime.MinValue + (TimeSpan)biddingWindowReader["FromHours"];
            //            fromTime = dtFromTime.ToString("hh:mm tt");
            //            DateTime dtToTime = DateTime.MinValue + (TimeSpan)biddingWindowReader["ToHours"];
            //            toTime = dtToTime.ToString("hh:mm tt");
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(date) && pricingsContext != null && pricingsContext.Any())
            //    {
            //        var priceNotifyConfig = Config.PriceNotifyConfigurationFlag;
            //        DateTime currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            //        //List<long> publishSkulist = inputDto.outputDto.Select(s => s.SkuId).ToList();
            //        if (!string.IsNullOrEmpty(priceNotifyConfig) && priceNotifyConfig.ToLower().Equals("true"))
            //        {
            //            SqlCommand priceNotifycmd = new SqlCommand("select *from PriceNotifyConfigurations where NotificationDate=Convert(date,@currentDate)", cnn);
            //            priceNotifycmd.Parameters.AddWithValue("@currentDate", currentDate);
            //            da = new SqlDataAdapter(priceNotifycmd);
            //            dt = new DataTable();
            //            da.Fill(dt);
            //            IList<PriceNotifyConfiguration> priceNotifyConfigListContext = dt.AsEnumerable().Select(row =>
            //                new PriceNotifyConfiguration
            //                {
            //                    IsSMS = row.Field<bool>("IsSMS"),
            //                    IsEmail = row.Field<bool>("IsEmail"),
            //                    IsPushNotification = row.Field<bool>("IsPushNotification"),
            //                    //CityId = row.Field<string>("CityId"),
            //                    StateId = row.Field<string>("StateId"),
            //                    SkuId = row.Field<string>("SkuId"),
            //                    IncoTermId = row.Field<string>("IncoTermId"),
            //                }).ToList().Where(_ => _.StateId.Split(',').ToList().Intersect(pricingsContext.Select(s => s.StateId.ToString())).Any()).ToList();

            //            foreach (var priceNotifyConfigContext in priceNotifyConfigListContext)
            //            {
            //                if (priceNotifyConfigContext != null && (priceNotifyConfigContext.IsSMS || priceNotifyConfigContext.IsEmail || priceNotifyConfigContext.IsPushNotification))
            //                {
            //                    List<long> incoTermsIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.IncoTermId);
            //                    List<long> skuIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.SkuId);
            //                    List<long> configStateIds = null;

            //                    configStateIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.StateId);
            //                    if (configStateIds != null && configStateIds.Any())
            //                    {
            //                        configStateIds = pricingsContext.Select(_ => (long)_.StateId).ToList().Intersect(configStateIds).ToList();
            //                    }

            //                    foreach (var incoTermId in incoTermsIds)
            //                    {
            //                        SqlCommand incoTermCmd = new SqlCommand("select Name from IncoTerms where Id=@incoTermId", cnn);
            //                        incoTermCmd.Parameters.AddWithValue("@incoTermId", incoTermId);
            //                        string incoTermsName = (string)incoTermCmd.ExecuteScalar();
            //                        if (!string.IsNullOrEmpty(incoTermsName))
            //                        {
            //                            foreach (long stateId in configStateIds)
            //                            {
            //                                SqlCommand usercmd = new SqlCommand("select * from Users as u " +
            //                                    "INNER JOIN UserRoles as ur on u.Id= ur.UserId where u.IsActive=1 and u.StateId = @stateId and u.SaudaBookingTypeId = @bookingTypeId and " +
            //                                    "RoleId in (@DealerRoleId,@BrokerRoleId,@BDORoleId)", cnn);
            //                                usercmd.Parameters.AddWithValue("@stateId", stateId);
            //                                usercmd.Parameters.AddWithValue("@bookingTypeId", (int)DTO.Enums.SaudaBookingTypes.ReverseAuction);
            //                                usercmd.Parameters.AddWithValue("@DealerRoleId", (int)DTO.Enums.Role.Dealer);
            //                                usercmd.Parameters.AddWithValue("@BrokerRoleId", (int)DTO.Enums.Role.Broker);
            //                                usercmd.Parameters.AddWithValue("@BDORoleId", (int)DTO.Enums.Role.StateTrader);
            //                                da = new SqlDataAdapter(usercmd);
            //                                dt = new DataTable();
            //                                da.Fill(dt);
            //                                IList<User> usersContext = dt.AsEnumerable().Select(row =>
            //                                    new User
            //                                    {
            //                                        Email = row.Field<string>("Email"),
            //                                        MobileNumber = row.Field<string>("MobileNumber"),
            //                                        RegistrationTypeId = row.Field<int?>("RegistrationTypeId"),
            //                                        PushTokenKey = row.Field<string>("PushTokenKey"),
            //                                        TransportModeId = row.Field<long?>("TransportModeId"),
            //                                        FreightRouteId = row.Field<long?>("FreightRouteId"),
            //                                        Loadability = row.Field<decimal>("Loadability"),
            //                                    }).ToList();
            //                                foreach (var userContext in usersContext)
            //                                {
            //                                    List<string> toUsers = new List<string>();
            //                                    toUsers.Add(userContext.Email);
            //                                    string emailSkusPricing = string.Empty;
            //                                    string mobileSkusPricing = string.Empty;
            //                                    //var skuList = inputDto.outputDto.Where(_ => skuIds.Contains(_.SkuId) && _.CityId == cityId);
            //                                    //availableSkuList.AddRange(skuList);
            //                                    var availablePricings = pricingsContext.Where(_ => _.StateId == stateId && _.TransportModeId == userContext.TransportModeId
            //                                    //&&
            //                                     //_.FrieghtRouteId == userContext.FreightRouteId 
            //                                     && _.LoadQuantity == userContext.Loadability && skuIds.Contains(_.SkuId));
            //                                    pricingsContext.Where(_ => _.StateId == stateId && _.TransportModeId == userContext.TransportModeId &&
            //                                     _.FrieghtRouteId == userContext.FreightRouteId && _.LoadQuantity == userContext.Loadability && skuIds.Contains(_.SkuId)).ToList().ForEach(_ => _.IsPublish = true);
            //                                    //publishedPricings.AddRange(availablePricings);
            //                                    //publishedPricings.Distinct();
            //                                    foreach (var pricing in availablePricings)
            //                                    {
            //                                        SqlCommand skuCmd = new SqlCommand("select SkuName from Skus where Id=@skuId", cnn);
            //                                        skuCmd.Parameters.AddWithValue("@skuId", pricing.SkuId);
            //                                        string skuName = (string)skuCmd.ExecuteScalar();
            //                                        SqlCommand plantCmd = new SqlCommand("select Name from Depots where Id=@plantId", cnn);
            //                                        plantCmd.Parameters.AddWithValue("@plantId", pricing.PlantId);
            //                                        string plantName = (string)plantCmd.ExecuteScalar();
            //                                        SqlCommand depotCmd = new SqlCommand("select Name from Depots where Id=@depotId", cnn);
            //                                        depotCmd.Parameters.AddWithValue("@depotId", pricing.DepotId);
            //                                        string depotName = (string)depotCmd.ExecuteScalar();
            //                                        if (!string.IsNullOrEmpty(skuName) && !string.IsNullOrEmpty(plantName) && !string.IsNullOrEmpty(depotName))
            //                                        {
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ExDepot && pricing.ExDepotPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExDepotPrice + pricing.XMargin), 2).ToString()));
            //                                                mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExDepotPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ForDepot && pricing.ForDepotPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForDepotPrice + pricing.XMargin), 2).ToString()));
            //                                                mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForDepotPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ExPlant && pricing.ExPlantPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExPlantPrice + pricing.XMargin), 2).ToString()));
            //                                                mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExPlantPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ForPlant && pricing.ForPlantPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForPlantPrice + pricing.XMargin), 2).ToString()));
            //                                                mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForPlantPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ExRake && pricing.ExRakePrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExRakePrice + pricing.XMargin), 2).ToString()));
            //                                                mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExRakePrice + pricing.XMargin), 2).ToString() + ", "));
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ForRake && pricing.ForRakePrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForRakePrice + pricing.XMargin), 2).ToString()));
            //                                                mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForRakePrice + pricing.XMargin), 2).ToString() + ", "));
            //                                            }
            //                                        }
            //                                    }
            //                                    if (!string.IsNullOrEmpty(mobileSkusPricing))
            //                                    {
            //                                        mobileSkusPricing = mobileSkusPricing.Substring(0, mobileSkusPricing.Length - 2);
            //                                    }
            //                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //                                    if (priceNotifyConfigContext.IsEmail && toUsers != null && toUsers.Any() && !string.IsNullOrEmpty(emailSkusPricing))
            //                                    {
            //                                        var fromEmail = Constants.FromEmail;
            //                                        var emailSubject = Constants.FinalPricePublishSubject;
            //                                        var plainText = string.Empty;
            //                                        SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishEmail", cnn);
            //                                        emailTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishEmail", Constants.PriceConfigFinalPricePublishEmail);
            //                                        using (SqlDataReader emailTemplateReader = emailTemplatecmd.ExecuteReader())
            //                                        {
            //                                            if (emailTemplateReader.Read())
            //                                            {
            //                                                var plainTemplate = emailTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                                    .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, emailSkusPricing);
            //                                                var htmlTemplate = emailTemplateReader["Template"].ToString().Replace(Constants.ReplaceMainContent, plainTemplate);
            //                                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //                                            }
            //                                        }
            //                                    }
            //                                    var smsMessage = string.Empty;
            //                                    if (priceNotifyConfigContext.IsSMS && !string.IsNullOrEmpty(mobileSkusPricing))
            //                                    {
            //                                        var smsPlainTemplate = string.Empty;
            //                                        EmailTemplate smsTemplate = new EmailTemplate();
            //                                        SqlCommand smsTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishSMS", cnn);
            //                                        smsTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishSMS", Constants.PriceConfigFinalPricePublishSMS);
            //                                        using (SqlDataReader smsTemplateReader = smsTemplatecmd.ExecuteReader())
            //                                        {
            //                                            if (smsTemplateReader.Read())
            //                                            {
            //                                                smsPlainTemplate = smsTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                                    .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, mobileSkusPricing);
            //                                                smsMessage = smsTemplateReader["Template"].ToString().Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //                                                try
            //                                                {
            //                                                    //foreach (var mobileNo in usersContext.Select(_ => _.MobileNumber))
            //                                                    //{
            //                                                    amazonNotificationService.SendMessage(smsMessage, userContext.MobileNumber);
            //                                                    //}
            //                                                }
            //                                                catch (Exception ex)
            //                                                {

            //                                                }
            //                                            }
            //                                        }
            //                                    }
            //                                    if (priceNotifyConfigContext.IsPushNotification && !string.IsNullOrEmpty(mobileSkusPricing))
            //                                    {
            //                                        //foreach (var userContext in usersContext)
            //                                        //{
            //                                        if (userContext != null && userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
            //                                        {
            //                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //                                            {
            //                                                PushTokenKey = userContext.PushTokenKey,
            //                                                RegistrationTypeId = (int)userContext.RegistrationTypeId,
            //                                                Title = Constants.FinalPricePublishSubject,
            //                                                Message = smsMessage,
            //                                                Id = "00"
            //                                            };
            //                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //                                        }
            //                                        //}
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //        }

            //        //var unavailablePricings = publishedPricings != null && publishedPricings.Any() ? pricingsContext.ToList().Except(publishedPricings).ToList() : pricingsContext.ToList();
            //        var unavailablePricings = pricingsContext.Where(_ => _.IsPublish != true).ToList();
            //        if (unavailablePricings != null && unavailablePricings.Any())
            //        {
            //            var stateIds = unavailablePricings.Select(_ => _.StateId).Distinct().ToList();
            //            foreach (var stateId in stateIds)
            //            {
            //                //List<SkuFinalpriceListOutputDto> skuList = new List<SkuFinalpriceListOutputDto>();
            //                //if (unAvailableSkus != null && unAvailableSkus.Any())
            //                //{
            //                //    skuList = unAvailableSkus.Where(_ => _.CityId == cityId).ToList();
            //                //}
            //                //else
            //                //{
            //                //    skuList = inputDto.outputDto.Where(_ => _.CityId == cityId).ToList();
            //                //}
            //                //if (skuList != null && skuList.Any())
            //                //{
            //                SqlCommand usercmd = new SqlCommand("select * from Users as u " +
            //                                "INNER JOIN UserRoles as ur on u.Id= ur.UserId where u.IsActive=1 and u.StateId = @stateId and u.SaudaBookingTypeId = @bookingTypeId and " +
            //                                "RoleId in (@DealerRoleId,@BrokerRoleId,@BDORoleId)", cnn);
            //                usercmd.Parameters.AddWithValue("@stateId", stateId);
            //                usercmd.Parameters.AddWithValue("@bookingTypeId", (int)DTO.Enums.SaudaBookingTypes.ReverseAuction);
            //                usercmd.Parameters.AddWithValue("@DealerRoleId", (int)DTO.Enums.Role.Dealer);
            //                usercmd.Parameters.AddWithValue("@BrokerRoleId", (int)DTO.Enums.Role.Broker);
            //                usercmd.Parameters.AddWithValue("@BDORoleId", (int)DTO.Enums.Role.StateTrader);
            //                da = new SqlDataAdapter(usercmd);
            //                dt = new DataTable();
            //                da.Fill(dt);
            //                IList<User> usersContext = dt.AsEnumerable().Select(row =>
            //                    new User
            //                    {
            //                        Id = row.Field<long>("Id"),
            //                        Email = row.Field<string>("Email"),
            //                        MobileNumber = row.Field<string>("MobileNumber"),
            //                        RegistrationTypeId = row.Field<int?>("RegistrationTypeId"),
            //                        PushTokenKey = row.Field<string>("PushTokenKey"),
            //                        TransportModeId = row.Field<long?>("TransportModeId"),
            //                        FreightRouteId = row.Field<long?>("FreightRouteId"),
            //                        Loadability = row.Field<decimal>("Loadability"),
            //                    }).ToList();
            //                foreach (var userContextItem in usersContext)
            //                {
            //                    List<string> toUsers = new List<string>();
            //                    toUsers.Add(userContextItem.Email);
            //                    var availablePricings = unavailablePricings.Where(_ => _.StateId == stateId && _.TransportModeId == userContextItem.TransportModeId &&
            //                                 _.FrieghtRouteId == userContextItem.FreightRouteId && _.LoadQuantity == userContextItem.Loadability);
            //                    SqlCommand incoTermCmd = new SqlCommand("select IncoTermsId from UserIncoTerms where UserId=@userId", cnn);
            //                    incoTermCmd.Parameters.AddWithValue("@userId", userContextItem.Id);
            //                    da = new SqlDataAdapter(incoTermCmd);
            //                    dt = new DataTable();
            //                    da.Fill(dt);
            //                    IList<UserIncoTerms> incotermsList = dt.AsEnumerable().Select(row =>
            //                        new UserIncoTerms
            //                        {
            //                            IncoTermsId = row.Field<long>("IncoTermsId"),
            //                        }).ToList();
            //                    foreach (var incoTermItem in incotermsList)
            //                    {
            //                        string emailSkusPricing = string.Empty;
            //                        string mobileSkusPricing = string.Empty;
            //                        var incoTermId = incoTermItem.IncoTermsId;
            //                        SqlCommand incoTermNameCmd = new SqlCommand("select Name from IncoTerms where Id=@incoTermId", cnn);
            //                        incoTermNameCmd.Parameters.AddWithValue("@incoTermId", incoTermItem.IncoTermsId);
            //                        string incoTermsName = (string)incoTermNameCmd.ExecuteScalar();
            //                        if (!string.IsNullOrEmpty(incoTermsName))
            //                        {
            //                            foreach (var pricing in availablePricings)
            //                            {
            //                                SqlCommand skuCmd = new SqlCommand("select SkuName from Skus where Id=@skuId", cnn);
            //                                skuCmd.Parameters.AddWithValue("@skuId", pricing.SkuId);
            //                                string skuName = (string)skuCmd.ExecuteScalar();
            //                                SqlCommand plantCmd = new SqlCommand("select Name from Depots where Id=@plantId", cnn);
            //                                plantCmd.Parameters.AddWithValue("@plantId", pricing.PlantId);
            //                                string plantName = (string)plantCmd.ExecuteScalar();
            //                                SqlCommand depotCmd = new SqlCommand("select Name from Depots where Id=@depotId", cnn);
            //                                depotCmd.Parameters.AddWithValue("@depotId", pricing.DepotId);
            //                                string depotName = (string)depotCmd.ExecuteScalar();
            //                                if (!string.IsNullOrEmpty(skuName) && !string.IsNullOrEmpty(plantName) && !string.IsNullOrEmpty(depotName))
            //                                {
            //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExDepot && pricing.ExDepotPrice != 0)
            //                                    {
            //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExDepotPrice + pricing.XMargin), 2).ToString()));
            //                                        mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExDepotPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                    }
            //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForDepot && pricing.ForDepotPrice != 0)
            //                                    {
            //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForDepotPrice + pricing.XMargin), 2).ToString()));
            //                                        mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForDepotPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                    }
            //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExPlant && pricing.ExPlantPrice != 0)
            //                                    {
            //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExPlantPrice + pricing.XMargin), 2).ToString()));
            //                                        mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExPlantPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                    }
            //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForPlant && pricing.ForPlantPrice != 0)
            //                                    {
            //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForPlantPrice + pricing.XMargin), 2).ToString()));
            //                                        mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForPlantPrice + pricing.XMargin), 2).ToString() + ", "));
            //                                    }
            //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ExRake && pricing.ExRakePrice != 0)
            //                                    {
            //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExRakePrice + pricing.XMargin), 2).ToString()));
            //                                        mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ExRakePrice + pricing.XMargin), 2).ToString() + ", "));
            //                                    }
            //                                    if (incoTermId == (int)DTO.Enums.IncoTerms.ForRake && pricing.ForRakePrice != 0)
            //                                    {
            //                                        emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForRakePrice + pricing.XMargin), 2).ToString()));
            //                                        mobileSkusPricing += Constants.SkuPriceFormatMobile.Replace(Constants.PlantName, plantName).Replace(Constants.DepotName, depotName).Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((pricing.ForRakePrice + pricing.XMargin), 2).ToString() + ", "));
            //                                    }
            //                                }
            //                            }
            //                            if (!string.IsNullOrEmpty(mobileSkusPricing))
            //                            {
            //                                mobileSkusPricing = mobileSkusPricing.Substring(0, mobileSkusPricing.Length - 2);
            //                            }
            //                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //                            var fromEmail = Constants.FromEmail;
            //                            var emailSubject = Constants.FinalPricePublishSubject;
            //                            var plainText = string.Empty;
            //                            SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishEmail", cnn);
            //                            emailTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishEmail", Constants.PriceConfigFinalPricePublishEmail);
            //                            using (SqlDataReader emailTemplateReader = emailTemplatecmd.ExecuteReader())
            //                            {
            //                                if (emailTemplateReader.Read() && !string.IsNullOrEmpty(emailSkusPricing))
            //                                {
            //                                    var plainTemplate = emailTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                        .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, emailSkusPricing);
            //                                    var htmlTemplate = emailTemplateReader["Template"].ToString().Replace(Constants.ReplaceMainContent, plainTemplate);
            //                                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //                                }
            //                            }

            //                            var smsMessage = string.Empty;
            //                            var smsPlainTemplate = string.Empty;
            //                            EmailTemplate smsTemplate = new EmailTemplate();
            //                            SqlCommand smsTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishSMS", cnn);
            //                            smsTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishSMS", Constants.PriceConfigFinalPricePublishSMS);
            //                            using (SqlDataReader smsTemplateReader = smsTemplatecmd.ExecuteReader())
            //                            {
            //                                if (smsTemplateReader.Read() && !string.IsNullOrEmpty(mobileSkusPricing))
            //                                {
            //                                    smsPlainTemplate = smsTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                        .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, mobileSkusPricing);
            //                                    smsMessage = smsTemplateReader["Template"].ToString().Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //                                    try
            //                                    {
            //                                        amazonNotificationService.SendMessage(smsMessage, userContextItem.MobileNumber);
            //                                    }
            //                                    catch (Exception ex)
            //                                    {

            //                                    }
            //                                }
            //                            }

            //                            if (!string.IsNullOrEmpty(mobileSkusPricing) && userContextItem != null && userContextItem.RegistrationTypeId != null && userContextItem.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContextItem.PushTokenKey) && !string.IsNullOrEmpty(mobileSkusPricing))
            //                            {
            //                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //                                {
            //                                    PushTokenKey = userContextItem.PushTokenKey,
            //                                    RegistrationTypeId = (int)userContextItem.RegistrationTypeId,
            //                                    Title = Constants.FinalPricePublishSubject,
            //                                    Message = smsMessage,
            //                                    Id = "00"
            //                                };
            //                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //        }
            //    }
            //    else if (pricingsContext != null && pricingsContext.Any())
            //    {
            //        SqlCommand usercmd = new SqlCommand("select u.Id, u.Email,u.MobileNumber, u.StateId, u.RegistrationTypeId,u.PushTokenKey from Users as u " +
            //                                    "INNER JOIN UserRoles as ur on u.Id= ur.UserId where u.IsActive=1 and u.SaudaBookingTypeId = @bookingTypeId and " +
            //                                    "RoleId in (@DealerRoleId,@BrokerRoleId,@BDORoleId)", cnn);
            //        usercmd.Parameters.AddWithValue("@bookingTypeId", (int)DTO.Enums.SaudaBookingTypes.ReverseAuction);
            //        usercmd.Parameters.AddWithValue("@DealerRoleId", (int)DTO.Enums.Role.Dealer);
            //        usercmd.Parameters.AddWithValue("@BrokerRoleId", (int)DTO.Enums.Role.Broker);
            //        usercmd.Parameters.AddWithValue("@BDORoleId", (int)DTO.Enums.Role.StateTrader);
            //        da = new SqlDataAdapter(usercmd);
            //        dt = new DataTable();
            //        da.Fill(dt);
            //        IList<User> usersContext = dt.AsEnumerable().Select(row =>
            //            new User
            //            {
            //                Id = row.Field<long>("Id"),
            //                Email = row.Field<string>("Email"),
            //                MobileNumber = row.Field<string>("MobileNumber"),
            //                RegistrationTypeId = row.Field<int?>("RegistrationTypeId"),
            //                PushTokenKey = row.Field<string>("PushTokenKey"),
            //                StateId = row.Field<int>("StateId"),
            //            }).ToList().Where(_ => pricingsContext.Any(a => a.StateId == _.StateId)).ToList();
            //        List<string> toUsers = new List<string>();
            //        if (usersContext != null && usersContext.Any() && !string.IsNullOrEmpty(date))
            //        {
            //            toUsers = usersContext.Where(_ => _.Email != null && _.Email != "").Select(_ => _.Email).ToList();
            //            bool isSMS = false;
            //            bool isEmail = false;
            //            bool isPush = false;
            //            SqlCommand isEmailCmd = new SqlCommand("select Value from Configurations where Id=@isEmailId", cnn);
            //            isEmailCmd.Parameters.AddWithValue("@isEmailId", (int)DTO.Enums.Configuration.IsEMAIL);
            //            string sIsEmail = (string)isEmailCmd.ExecuteScalar();
            //            if (sIsEmail.Equals("1") || sIsEmail.Equals("True"))
            //            {
            //                isEmail = true;
            //            }
            //            SqlCommand isSMSCmd = new SqlCommand("select Value from Configurations where Id=@isSMSId", cnn);
            //            isSMSCmd.Parameters.AddWithValue("@isSMSId", (int)DTO.Enums.Configuration.IsSMS);
            //            string sIsSMS = (string)isSMSCmd.ExecuteScalar();
            //            if (sIsSMS.Equals("1") || sIsSMS.Equals("True"))
            //            {
            //                isSMS = true;
            //            }
            //            SqlCommand isPushCmd = new SqlCommand("select Value from Configurations where Id=@isPushCmdId", cnn);
            //            isPushCmd.Parameters.AddWithValue("@isPushCmdId", (int)DTO.Enums.Configuration.IsPushNotification);
            //            string sIsPush = (string)isPushCmd.ExecuteScalar();
            //            if (sIsPush.Equals("1") || sIsPush.Equals("True"))
            //            {
            //                isPush = true;
            //            }
            //            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //            if (isEmail && toUsers != null && toUsers.Any())
            //            {
            //                var fromEmail = Constants.FromEmail;
            //                var emailSubject = Constants.FinalPricePublishSubject;
            //                var plainText = string.Empty;
            //                SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@RAFinalPricePublishNotificationEmail", cnn);
            //                emailTemplatecmd.Parameters.AddWithValue("@RAFinalPricePublishNotificationEmail", Constants.RAFinalPricePublishNotificationEmail);
            //                using (SqlDataReader emailTemplateReader = emailTemplatecmd.ExecuteReader())
            //                {
            //                    var plainTemplate = emailTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime);
            //                    var htmlTemplate = emailTemplateReader["Template"].ToString().Replace(Constants.ReplaceMainContent, plainTemplate);
            //                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //                }
            //            }
            //            var smsMessage = string.Empty;
            //            if (isSMS)
            //            {
            //                toUsers = usersContext.Where(_ => _.MobileNumber != null && _.MobileNumber != "").Select(_ => _.MobileNumber).ToList();
            //                if (toUsers != null && toUsers.Any())
            //                {
            //                    var smsPlainTemplate = string.Empty;
            //                    EmailTemplate smsTemplate = new EmailTemplate();
            //                    SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@RAFinalPricePublishNotificationSMS", cnn);
            //                    emailTemplatecmd.Parameters.AddWithValue("@RAFinalPricePublishNotificationSMS", Constants.RAFinalPricePublishNotificationSMS);
            //                    using (SqlDataReader smsTemplateReader = emailTemplatecmd.ExecuteReader())
            //                    {
            //                        smsPlainTemplate = smsTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime);
            //                        smsMessage = smsTemplateReader["Template"].ToString().Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //                        foreach (var mobileNo in toUsers)
            //                        {
            //                            amazonNotificationService.SendMessage(smsMessage, mobileNo);
            //                        }
            //                    }
            //                }
            //            }
            //            if (isPush)
            //            {
            //                foreach (var userContext in usersContext)
            //                {
            //                    if (userContext != null && userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
            //                    {
            //                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //                        {
            //                            PushTokenKey = userContext.PushTokenKey,
            //                            RegistrationTypeId = (int)userContext.RegistrationTypeId,
            //                            Title = Constants.FinalPricePublishSubject,
            //                            Message = smsMessage,
            //                            Id = "00",
            //                        };
            //                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    cnn.Close();

            //}
            //catch (Exception ex)
            //{

            //}
            //return result;
        }

        public ResultDto PublishFinalPrice(FinalPricePublishDto inputDto)
        {
            _methodName = "PublishFinalPrice";
            var resultDto = new ResultDto();
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

                if (!(inputDto.PublishId > 0))
                {
                    return _resultService.ErrorMessage(Constants.PublishIdMissing);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                //PricePublish pricePublishContext = _emamiContext.PricePublish.FirstOrDefault(_ => _.Id == inputDto.PublishId);
                //if (pricePublishContext != null)
                //{

                //var pricingsContext = _emamiContext.Pricing.AsNoTracking()
                //    .Where(_ => _.PublishId == inputDto.PublishId).
                //    Select(s => s);

                var pricingsContext = _emamiContext.Pricing.AsNoTracking()
                    //.Where(_ => _.PublishId == inputDto.PublishId)
                    .
                    Select(s => new
                    {
                        //MaterialCostId = s.MaterialCostId,
                        //PackingCostId = s.PackingCostId,
                        //DepotCostId = s.DepotCostId,
                        //DetentionCostId = s.DetentionCostId,
                        //ProfitMarginId = s.ProfitMarginId,
                        //CushionMarginId = s.CushionMarginId,
                        //SchemeCostId = s.SchemeCostId,
                        //PrimaryFrieghtId = s.PrimaryFrieghtId,
                        //SecondaryFrieghtForPlantId = s.SecondaryFrieghtForPlantId,
                        //HoneycombCostId = s.HoneycombCostId,
                        //RaMarginId = s.RaMarginId,
                        //LoadCapacityId = s.LoadCapacityId,
                        //IngredientCostId = s.IngredientCostId,
                        //SkuIngrediantPlantId = s.SkuIngrediantPlantId,
                        //StateId = s.StateId,
                        //SecondaryFrieghtId = s.SecondaryFrieghtId
                    }).ToList();

                var pricingIds = new List<long>();
                var ingredientIds = new List<string>();
                List<string> updateQuery = new List<string>();
                string bookingType = inputDto.BookingTypeId == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess ? DTO.Enums.SaudaBookingTypes.TraditionalProcess.ToString()
                    //: (inputDto.BookingTypeId == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction ? DTO.Enums.SaudaBookingTypes.ReverseAuction.ToString() 
                    : "";

                if (pricingsContext != null && pricingsContext.Any())
                {
                    //pricingIds = pricingsContext.Where(w => w.MaterialCostId != 0).Select(s => s.MaterialCostId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update MaterialCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.PackingCostId != 0).Select(s => s.PackingCostId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update PackingCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.DepotCostId != 0).Select(s => s.DepotCostId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update DepotCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.DetentionCostId != 0).Select(s => s.DetentionCostId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update DetentionCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.ProfitMarginId != 0).Select(s => s.ProfitMarginId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update ProfitMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.CushionMarginId != 0).Select(s => s.CushionMarginId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update CushionMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.SchemeCostId != 0).Select(s => s.SchemeCostId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update SchemeCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.PrimaryFrieghtId != 0).Select(s => s.PrimaryFrieghtId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update PrimaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.SecondaryFrieghtId != 0).Select(s => s.SecondaryFrieghtId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update SecondaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.SecondaryFrieghtForPlantId != 0).Select(s => s.SecondaryFrieghtForPlantId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update SecondaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.HoneycombCostId != 0).Select(s => s.HoneycombCostId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update HoneycombCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.RaMarginId != 0).Select(s => s.RaMarginId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update RaMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.LoadCapacityId != 0).Select(s => s.LoadCapacityId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update LoadCapacityConversions Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    //ingredientIds = pricingsContext.Where(w => !string.IsNullOrEmpty(w.IngredientCostId)).Select(s => s.IngredientCostId).Distinct().ToList();
                    //if (ingredientIds != null && ingredientIds.Any())
                    //{
                    //    var ingredientId = string.Join(",", ingredientIds).Split(',').Distinct().ToList();
                    //    updateQuery.Add("Update IngredientCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", ingredientId) + ");");
                    //}

                    //pricingIds = pricingsContext.Where(w => w.SkuIngrediantPlantId != 0).Select(s => s.SkuIngrediantPlantId).Distinct().ToList();
                    //if (pricingIds != null && pricingIds.Any())
                    //{
                    //    updateQuery.Add("Update SkuIngrediantPlants Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                    //}

                    using (SqlConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        conn.Open();
                        SqlCommand command;
                        SqlTransaction sqlTransaction = conn.BeginTransaction();
                        var startedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        try
                        {
                            string updatePricings = "Update Pricings set IsPublish = @isPublish, BiddingDate = @biddingDate, ModifiedBy = @modifiedBy, ModifiedDate = @modifiedDate Where PublishId = @publishIds";
                            string tblQuery = string.Join("", updateQuery ?? new List<string>());

                            command = new SqlCommand(updatePricings, conn);
                            command.Parameters.AddWithValue("@isPublish", true);
                            command.Parameters.AddWithValue("@biddingDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                            command.Parameters.AddWithValue("@modifiedBy", inputDto.LoginUserId);
                            command.Parameters.AddWithValue("@modifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                            command.Parameters.AddWithValue("@publishIds", inputDto.PublishId);
                            command.Transaction = sqlTransaction;
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                            _logger.Info($"{bookingType} Pricings Table Updated - Date Time : {startedDate}");

                            string updatePricePublishes = "Update PricePublishes set IsPublish = @isPublish, ModifiedBy = @modifiedBy, ModifiedDate = @modifiedDate Where Id = @publishIds";
                            command = conn.CreateCommand();
                            command.CommandText = updatePricePublishes;
                            command.Parameters.AddWithValue("@isPublish", true);
                            command.Parameters.AddWithValue("@modifiedBy", inputDto.LoginUserId);
                            command.Parameters.AddWithValue("@modifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                            command.Parameters.AddWithValue("@publishIds", inputDto.PublishId);
                            command.Transaction = sqlTransaction;
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                            _logger.Info($"{bookingType} - PricePublishes Table Updated - Date Time : {startedDate}");

                            if (!string.IsNullOrEmpty(tblQuery))
                            {
                                command = conn.CreateCommand();
                                command.CommandText = tblQuery;
                                command.Parameters.AddWithValue("@isPublish", true);
                                command.Transaction = sqlTransaction;
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                                _logger.Info($"{bookingType} - All Pricings Table Updated - Date Time : {startedDate}");
                            }
                            sqlTransaction.Commit();
                        }
                        catch (Exception exception)
                        {
                            sqlTransaction.Rollback();
                            _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {exception}");
                            return _resultService.ErrorMessage(Constants.Exception);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }

                    //var publishStateIds = pricingsContext.Select(_ => _.StateId).Distinct().ToList();
                    // System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => FinalPricePublishBackgroundQueue(publishStateIds,));


                    //if (pricePublishContext.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                    //{
                    //    var publishStateIds = pricingsContext.Select(_ => _.StateId).Distinct().ToList();
                    //    System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => FinalPricePublishBackgroundQueue(publishStateIds));
                    //}
                    //else
                    //{
                    //    Thread workingThread = new Thread(new ParameterizedThreadStart(RAFinalPriceNotificationUpdated))
                    //    { IsBackground = true };
                    //    workingThread.Start(inputDto);
                    //}
                }

                return _resultService.SuccessMessage(Constants.PriceDetailsPublishedSuccessfully);
                //}
                //else
                //{
                //    return _resultService.SuccessMessage(Constants.RecordNotFound);
                //}
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public void RAFinalPriceNotification(object notifyinputDto)
        {
            ResultDto result = new ResultDto();
            //try
            //{
            //    string connetionString = Config.DBConnectionString;
            //    SqlConnection cnn;
            //    cnn = new SqlConnection(connetionString);
            //    cnn.Open();
            //    SaveFinalPricngInputDto inputDto = (SaveFinalPricngInputDto)notifyinputDto;
            //    var publishCityIds = inputDto.outputDto.Select(_ => _.CityId).Distinct();
            //    SqlCommand biddingWindowCmd = new SqlCommand("Select * from BiddingWindowTimings where id=@biddingWindowId", cnn);
            //    biddingWindowCmd.Parameters.AddWithValue("@biddingWindowId", inputDto.BiddingWindowId);
            //    string date = string.Empty;
            //    string fromTime = string.Empty;
            //    string toTime = string.Empty;
            //    using (SqlDataReader biddingWindowReader = biddingWindowCmd.ExecuteReader())
            //    {
            //        if (biddingWindowReader.Read())
            //        {
            //            date = ((DateTime)biddingWindowReader["BiddingDate"]).ToString("dddd, dd MMMM yyyy");
            //            DateTime dtFromTime = DateTime.MinValue + (TimeSpan)biddingWindowReader["FromHours"];
            //            fromTime = dtFromTime.ToString("hh:mm tt");
            //            DateTime dtToTime = DateTime.MinValue + (TimeSpan)biddingWindowReader["ToHours"];
            //            toTime = dtToTime.ToString("hh:mm tt");
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(date) && publishCityIds != null && publishCityIds.Any())
            //    {
            //        var priceNotifyConfig = Config.PriceNotifyConfigurationFlag;
            //        List<long> availableSkus = new List<long>();
            //        List<SkuFinalpriceListOutputDto> availableSkuList = new List<SkuFinalpriceListOutputDto>();
            //        List<SkuFinalpriceListOutputDto> unAvailableSkus = null;
            //        DateTime currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            //        //List<long> publishSkulist = inputDto.outputDto.Select(s => s.SkuId).ToList();
            //        if (!string.IsNullOrEmpty(priceNotifyConfig) && priceNotifyConfig.ToLower().Equals("true"))
            //        {
            //            SqlCommand priceNotifycmd = new SqlCommand("select *from PriceNotifyConfigurations where NotificationDate=Convert(date,@currentDate)", cnn);
            //            priceNotifycmd.Parameters.AddWithValue("@currentDate", currentDate);
            //            SqlDataAdapter da = new SqlDataAdapter(priceNotifycmd);
            //            DataTable dt = new DataTable();
            //            da.Fill(dt);
            //            IList<PriceNotifyConfiguration> priceNotifyConfigListContext = dt.AsEnumerable().Select(row =>
            //                new PriceNotifyConfiguration
            //                {
            //                    IsSMS = row.Field<bool>("IsSMS"),
            //                    IsEmail = row.Field<bool>("IsEmail"),
            //                    IsPushNotification = row.Field<bool>("IsPushNotification"),
            //                    CityId = row.Field<string>("CityId"),
            //                    SkuId = row.Field<string>("SkuId"),
            //                    IncoTermId = row.Field<string>("IncoTermId"),
            //                }).ToList().Where(_ => _.CityId.Split(',').ToList().Intersect(publishCityIds.Select(s => s.ToString())).Any()).ToList();

            //            foreach (var priceNotifyConfigContext in priceNotifyConfigListContext)
            //            {
            //                if (priceNotifyConfigContext != null && (priceNotifyConfigContext.IsSMS || priceNotifyConfigContext.IsEmail || priceNotifyConfigContext.IsPushNotification))
            //                {
            //                    List<long> incoTermsIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.IncoTermId);
            //                    List<long> skuIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.SkuId);
            //                    availableSkus.AddRange(skuIds);
            //                    List<long> configCityIds = null;
            //                    if (publishCityIds != null && publishCityIds.Any())
            //                    {
            //                        configCityIds = UtilityHelper.ConvertStringToLongList(priceNotifyConfigContext.CityId);
            //                        if (configCityIds != null && configCityIds.Any())
            //                        {
            //                            configCityIds = publishCityIds.Intersect(configCityIds).ToList();
            //                        }
            //                    }
            //                    foreach (var incoTermId in incoTermsIds)
            //                    {
            //                        SqlCommand incoTermCmd = new SqlCommand("select Name from IncoTerms where Id=@incoTermId", cnn);
            //                        incoTermCmd.Parameters.AddWithValue("@incoTermId", incoTermId);
            //                        string incoTermsName = (string)incoTermCmd.ExecuteScalar();
            //                        if (!string.IsNullOrEmpty(incoTermsName))
            //                        {
            //                            foreach (long cityId in configCityIds)
            //                            {
            //                                SqlCommand usercmd = new SqlCommand("select u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey from Users as u " +
            //                                    "INNER JOIN UserRoles as ur on u.Id= ur.UserId where u.IsActive=1 and u.CityId = @cityId and u.SaudaBookingTypeId = @bookingTypeId and " +
            //                                    "RoleId in (@DealerRoleId,@BrokerRoleId,@BDORoleId)", cnn);
            //                                usercmd.Parameters.AddWithValue("@cityId", cityId);
            //                                usercmd.Parameters.AddWithValue("@bookingTypeId", (int)DTO.Enums.SaudaBookingTypes.ReverseAuction);
            //                                usercmd.Parameters.AddWithValue("@DealerRoleId", (int)DTO.Enums.Role.Dealer);
            //                                usercmd.Parameters.AddWithValue("@BrokerRoleId", (int)DTO.Enums.Role.Broker);
            //                                usercmd.Parameters.AddWithValue("@BDORoleId", (int)DTO.Enums.Role.StateTrader);
            //                                da = new SqlDataAdapter(usercmd);
            //                                dt = new DataTable();
            //                                da.Fill(dt);
            //                                IList<User> usersContext = dt.AsEnumerable().Select(row =>
            //                                    new User
            //                                    {
            //                                        Email = row.Field<string>("Email"),
            //                                        MobileNumber = row.Field<string>("MobileNumber"),
            //                                        RegistrationTypeId = row.Field<int?>("RegistrationTypeId"),
            //                                        PushTokenKey = row.Field<string>("PushTokenKey"),
            //                                    }).ToList();
            //                                if (usersContext != null && usersContext.Any())
            //                                {
            //                                    List<string> toUsers = usersContext.Select(_ => _.Email).ToList();
            //                                    string emailSkusPricing = string.Empty;
            //                                    string mobileSkusPricing = string.Empty;
            //                                    var skuList = inputDto.outputDto.Where(_ => skuIds.Contains(_.SkuId) && _.CityId == cityId);
            //                                    availableSkuList.AddRange(skuList);
            //                                    foreach (var sku in skuList)
            //                                    {
            //                                        SqlCommand skuCmd = new SqlCommand("select SkuName from Skus where Id=@skuId", cnn);
            //                                        skuCmd.Parameters.AddWithValue("@skuId", sku.SkuId);
            //                                        string skuName = (string)skuCmd.ExecuteScalar();
            //                                        if (!string.IsNullOrEmpty(skuName))
            //                                        {
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ExDepot && sku.ExDepotPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString()));
            //                                                mobileSkusPricing += skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ForDepot && sku.ForDepotPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString()));
            //                                                mobileSkusPricing += skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ExPlant && sku.ExPlantPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString()));
            //                                                mobileSkusPricing += skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ForPlant && sku.ForPlantPrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString()));
            //                                                mobileSkusPricing += skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ExRake && sku.ExRakePrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString()));
            //                                                mobileSkusPricing += skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                            }
            //                                            if (incoTermId == (int)DTO.Enums.IncoTerms.ForRake && sku.ForRakePrice != 0)
            //                                            {
            //                                                emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString()));
            //                                                mobileSkusPricing += skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                            }
            //                                        }
            //                                    }
            //                                    if (!string.IsNullOrEmpty(mobileSkusPricing))
            //                                    {
            //                                        mobileSkusPricing = mobileSkusPricing.Substring(0, mobileSkusPricing.Length - 2);
            //                                    }
            //                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //                                    if (priceNotifyConfigContext.IsEmail && toUsers != null && toUsers.Any() && !string.IsNullOrEmpty(emailSkusPricing))
            //                                    {
            //                                        var fromEmail = Constants.FromEmail;
            //                                        var emailSubject = Constants.FinalPricePublishSubject;
            //                                        var plainText = string.Empty;
            //                                        SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishEmail", cnn);
            //                                        emailTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishEmail", Constants.PriceConfigFinalPricePublishEmail);
            //                                        using (SqlDataReader emailTemplateReader = emailTemplatecmd.ExecuteReader())
            //                                        {
            //                                            if (emailTemplateReader.Read())
            //                                            {
            //                                                var plainTemplate = emailTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                                    .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, emailSkusPricing);
            //                                                var htmlTemplate = emailTemplateReader["Template"].ToString().Replace(Constants.ReplaceMainContent, plainTemplate);
            //                                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //                                            }
            //                                        }
            //                                    }
            //                                    var smsMessage = string.Empty;
            //                                    if (priceNotifyConfigContext.IsSMS && !string.IsNullOrEmpty(mobileSkusPricing))
            //                                    {
            //                                        var smsPlainTemplate = string.Empty;
            //                                        EmailTemplate smsTemplate = new EmailTemplate();
            //                                        SqlCommand smsTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishSMS", cnn);
            //                                        smsTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishSMS", Constants.PriceConfigFinalPricePublishSMS);
            //                                        using (SqlDataReader smsTemplateReader = smsTemplatecmd.ExecuteReader())
            //                                        {
            //                                            if (smsTemplateReader.Read())
            //                                            {
            //                                                smsPlainTemplate = smsTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                                    .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, mobileSkusPricing);
            //                                                smsMessage = smsTemplateReader["Template"].ToString().Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //                                                try
            //                                                {
            //                                                    foreach (var mobileNo in usersContext.Select(_ => _.MobileNumber))
            //                                                    {
            //                                                        amazonNotificationService.SendMessage(smsMessage, mobileNo);
            //                                                    }
            //                                                }
            //                                                catch (Exception ex)
            //                                                {

            //                                                }
            //                                            }
            //                                        }
            //                                    }
            //                                    if (priceNotifyConfigContext.IsPushNotification && !string.IsNullOrEmpty(mobileSkusPricing))
            //                                    {
            //                                        foreach (var userContext in usersContext)
            //                                        {
            //                                            if (userContext != null && userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
            //                                            {
            //                                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //                                                {
            //                                                    PushTokenKey = userContext.PushTokenKey,
            //                                                    RegistrationTypeId = (int)userContext.RegistrationTypeId,
            //                                                    Title = Constants.FinalPricePublishSubject,
            //                                                    Message = smsMessage,
            //                                                    Id = "00"
            //                                                };
            //                                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //        }
            //        unAvailableSkus = availableSkuList != null && availableSkuList.Any() ? inputDto.outputDto.ToList().Except(availableSkuList).ToList() : inputDto.outputDto.ToList();
            //        if ((unAvailableSkus != null && unAvailableSkus.Any()) && (publishCityIds != null && publishCityIds.Any()))
            //        {
            //            foreach (var cityId in publishCityIds)
            //            {
            //                List<SkuFinalpriceListOutputDto> skuList = new List<SkuFinalpriceListOutputDto>();
            //                if (unAvailableSkus != null && unAvailableSkus.Any())
            //                {
            //                    skuList = unAvailableSkus.Where(_ => _.CityId == cityId).ToList();
            //                }
            //                else
            //                {
            //                    skuList = inputDto.outputDto.Where(_ => _.CityId == cityId).ToList();
            //                }
            //                if (skuList != null && skuList.Any())
            //                {
            //                    SqlCommand usercmd = new SqlCommand("select u.Id, u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey from Users as u " +
            //                                    "INNER JOIN UserRoles as ur on u.Id= ur.UserId where u.IsActive=1 and u.CityId = @cityId and u.SaudaBookingTypeId = @bookingTypeId and " +
            //                                    "RoleId in (@DealerRoleId,@BrokerRoleId,@BDORoleId)", cnn);
            //                    usercmd.Parameters.AddWithValue("@cityId", cityId);
            //                    usercmd.Parameters.AddWithValue("@bookingTypeId", (int)DTO.Enums.SaudaBookingTypes.ReverseAuction);
            //                    usercmd.Parameters.AddWithValue("@DealerRoleId", (int)DTO.Enums.Role.Dealer);
            //                    usercmd.Parameters.AddWithValue("@BrokerRoleId", (int)DTO.Enums.Role.Broker);
            //                    usercmd.Parameters.AddWithValue("@BDORoleId", (int)DTO.Enums.Role.StateTrader);
            //                    SqlDataAdapter da = new SqlDataAdapter(usercmd);
            //                    DataTable dt = new DataTable();
            //                    da.Fill(dt);
            //                    IList<User> usersContext = dt.AsEnumerable().Select(row =>
            //                        new User
            //                        {
            //                            Id = row.Field<long>("Id"),
            //                            Email = row.Field<string>("Email"),
            //                            MobileNumber = row.Field<string>("MobileNumber"),
            //                            RegistrationTypeId = row.Field<int?>("RegistrationTypeId"),
            //                            PushTokenKey = row.Field<string>("PushTokenKey"),
            //                        }).ToList();
            //                    foreach (var userContextItem in usersContext)
            //                    {
            //                        List<string> toUsers = new List<string>();
            //                        toUsers.Add(userContextItem.Email);
            //                        SqlCommand incoTermCmd = new SqlCommand("select IncoTermsId from UserIncoTerms where UserId=@userId", cnn);
            //                        incoTermCmd.Parameters.AddWithValue("@userId", userContextItem.Id);
            //                        da = new SqlDataAdapter(incoTermCmd);
            //                        dt = new DataTable();
            //                        da.Fill(dt);
            //                        IList<UserIncoTerms> incotermsList = dt.AsEnumerable().Select(row =>
            //                            new UserIncoTerms
            //                            {
            //                                IncoTermsId = row.Field<long>("IncoTermsId"),
            //                            }).ToList();
            //                        foreach (var incoTermItem in incotermsList)
            //                        {
            //                            string emailSkusPricing = string.Empty;
            //                            string mobileSkusPricing = string.Empty;
            //                            var incoTermId = incoTermItem.IncoTermsId;
            //                            SqlCommand incoTermNameCmd = new SqlCommand("select Name from IncoTerms where Id=@incoTermId", cnn);
            //                            incoTermNameCmd.Parameters.AddWithValue("@incoTermId", incoTermItem.IncoTermsId);
            //                            string incoTermsName = (string)incoTermNameCmd.ExecuteScalar();
            //                            if (!string.IsNullOrEmpty(incoTermsName))
            //                            {
            //                                foreach (var sku in skuList)
            //                                {
            //                                    SqlCommand skuCmd = new SqlCommand("select SkuName from Skus where Id=@skuId", cnn);
            //                                    skuCmd.Parameters.AddWithValue("@skuId", sku.SkuId);
            //                                    string skuName = (string)skuCmd.ExecuteScalar();
            //                                    if (!string.IsNullOrEmpty(skuName))
            //                                    {
            //                                        if (incoTermId == (int)DTO.Enums.IncoTerms.ExDepot && sku.ExDepotPrice != 0)
            //                                        {
            //                                            emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString()));
            //                                            mobileSkusPricing += skuName + " - " + Math.Round((sku.ExDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                        }
            //                                        if (incoTermId == (int)DTO.Enums.IncoTerms.ForDepot && sku.ForDepotPrice != 0)
            //                                        {
            //                                            emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString()));
            //                                            mobileSkusPricing += skuName + " - " + Math.Round((sku.ForDepotPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                        }
            //                                        if (incoTermId == (int)DTO.Enums.IncoTerms.ExPlant && sku.ExPlantPrice != 0)
            //                                        {
            //                                            emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString()));
            //                                            mobileSkusPricing += skuName + " - " + Math.Round((sku.ExPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                        }
            //                                        if (incoTermId == (int)DTO.Enums.IncoTerms.ForPlant && sku.ForPlantPrice != 0)
            //                                        {
            //                                            emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString()));
            //                                            mobileSkusPricing += skuName + " - " + Math.Round((sku.ForPlantPrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                        }
            //                                        if (incoTermId == (int)DTO.Enums.IncoTerms.ExRake && sku.ExRakePrice != 0)
            //                                        {
            //                                            emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString()));
            //                                            mobileSkusPricing += skuName + " - " + Math.Round((sku.ExRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                        }
            //                                        if (incoTermId == (int)DTO.Enums.IncoTerms.ForRake && sku.ForRakePrice != 0)
            //                                        {
            //                                            emailSkusPricing += Constants.SkuPriceFormat.Replace(Constants.SkuPrice, (skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString()));
            //                                            mobileSkusPricing += skuName + " - " + Math.Round((sku.ForRakePrice + sku.XMarginCost), 2).ToString() + ", ";
            //                                        }
            //                                    }
            //                                }
            //                                if (!string.IsNullOrEmpty(mobileSkusPricing))
            //                                {
            //                                    mobileSkusPricing = mobileSkusPricing.Substring(0, mobileSkusPricing.Length - 2);
            //                                }
            //                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //                                var fromEmail = Constants.FromEmail;
            //                                var emailSubject = Constants.FinalPricePublishSubject;
            //                                var plainText = string.Empty;
            //                                SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishEmail", cnn);
            //                                emailTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishEmail", Constants.PriceConfigFinalPricePublishEmail);
            //                                using (SqlDataReader emailTemplateReader = emailTemplatecmd.ExecuteReader())
            //                                {
            //                                    if (emailTemplateReader.Read() && !string.IsNullOrEmpty(emailSkusPricing))
            //                                    {
            //                                        var plainTemplate = emailTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                            .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, emailSkusPricing);
            //                                        var htmlTemplate = emailTemplateReader["Template"].ToString().Replace(Constants.ReplaceMainContent, plainTemplate);
            //                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //                                    }
            //                                }

            //                                var smsMessage = string.Empty;
            //                                var smsPlainTemplate = string.Empty;
            //                                EmailTemplate smsTemplate = new EmailTemplate();
            //                                SqlCommand smsTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@priceConfigFinalPricePublishSMS", cnn);
            //                                smsTemplatecmd.Parameters.AddWithValue("@priceConfigFinalPricePublishSMS", Constants.PriceConfigFinalPricePublishSMS);
            //                                using (SqlDataReader smsTemplateReader = smsTemplatecmd.ExecuteReader())
            //                                {
            //                                    if (smsTemplateReader.Read() && !string.IsNullOrEmpty(mobileSkusPricing))
            //                                    {
            //                                        smsPlainTemplate = smsTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime)
            //                                            .Replace(Constants.IncoTerms, incoTermsName).Replace(Constants.SkuPricings, mobileSkusPricing);
            //                                        smsMessage = smsTemplateReader["Template"].ToString().Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //                                        try
            //                                        {
            //                                            amazonNotificationService.SendMessage(smsMessage, userContextItem.MobileNumber);
            //                                        }
            //                                        catch (Exception ex)
            //                                        {

            //                                        }
            //                                    }
            //                                }

            //                                if (!string.IsNullOrEmpty(mobileSkusPricing) && userContextItem != null && userContextItem.RegistrationTypeId != null && userContextItem.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContextItem.PushTokenKey) && !string.IsNullOrEmpty(mobileSkusPricing))
            //                                {
            //                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //                                    {
            //                                        PushTokenKey = userContextItem.PushTokenKey,
            //                                        RegistrationTypeId = (int)userContextItem.RegistrationTypeId,
            //                                        Title = Constants.FinalPricePublishSubject,
            //                                        Message = smsMessage,
            //                                        Id = "00"
            //                                    };
            //                                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else if (publishCityIds != null && publishCityIds.Any())
            //    {
            //        SqlCommand usercmd = new SqlCommand("select u.Id, u.Email,u.MobileNumber, u.CityId, u.RegistrationTypeId,u.PushTokenKey from Users as u " +
            //                                    "INNER JOIN UserRoles as ur on u.Id= ur.UserId where u.IsActive=1 and u.SaudaBookingTypeId = @bookingTypeId and " +
            //                                    "RoleId in (@DealerRoleId,@BrokerRoleId,@BDORoleId)", cnn);
            //        usercmd.Parameters.AddWithValue("@bookingTypeId", (int)DTO.Enums.SaudaBookingTypes.ReverseAuction);
            //        usercmd.Parameters.AddWithValue("@DealerRoleId", (int)DTO.Enums.Role.Dealer);
            //        usercmd.Parameters.AddWithValue("@BrokerRoleId", (int)DTO.Enums.Role.Broker);
            //        usercmd.Parameters.AddWithValue("@BDORoleId", (int)DTO.Enums.Role.StateTrader);
            //        SqlDataAdapter da = new SqlDataAdapter(usercmd);
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);
            //        IList<User> usersContext = dt.AsEnumerable().Select(row =>
            //            new User
            //            {
            //                Id = row.Field<long>("Id"),
            //                Email = row.Field<string>("Email"),
            //                MobileNumber = row.Field<string>("MobileNumber"),
            //                RegistrationTypeId = row.Field<int?>("RegistrationTypeId"),
            //                PushTokenKey = row.Field<string>("PushTokenKey"),
            //                CityId = row.Field<int>("CityId"),
            //            }).ToList().Where(_ => publishCityIds.Contains(_.CityId)).ToList();
            //        List<string> toUsers = new List<string>();
            //        if (usersContext != null && usersContext.Any() && !string.IsNullOrEmpty(date))
            //        {
            //            toUsers = usersContext.Where(_ => _.Email != null && _.Email != "").Select(_ => _.Email).ToList();
            //            bool isSMS = false;
            //            bool isEmail = false;
            //            bool isPush = false;
            //            SqlCommand isEmailCmd = new SqlCommand("select Value from Configurations where Id=@isEmailId", cnn);
            //            isEmailCmd.Parameters.AddWithValue("@isEmailId", (int)DTO.Enums.Configuration.IsEMAIL);
            //            string sIsEmail = (string)isEmailCmd.ExecuteScalar();
            //            if (sIsEmail.Equals("1") || sIsEmail.Equals("True"))
            //            {
            //                isEmail = true;
            //            }
            //            SqlCommand isSMSCmd = new SqlCommand("select Value from Configurations where Id=@isSMSId", cnn);
            //            isSMSCmd.Parameters.AddWithValue("@isSMSId", (int)DTO.Enums.Configuration.IsSMS);
            //            string sIsSMS = (string)isSMSCmd.ExecuteScalar();
            //            if (sIsSMS.Equals("1") || sIsSMS.Equals("True"))
            //            {
            //                isSMS = true;
            //            }
            //            SqlCommand isPushCmd = new SqlCommand("select Value from Configurations where Id=@isPushCmdId", cnn);
            //            isPushCmd.Parameters.AddWithValue("@isPushCmdId", (int)DTO.Enums.Configuration.IsPushNotification);
            //            string sIsPush = (string)isPushCmd.ExecuteScalar();
            //            if (sIsPush.Equals("1") || sIsPush.Equals("True"))
            //            {
            //                isPush = true;
            //            }
            //            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            //            if (isEmail && toUsers != null && toUsers.Any())
            //            {
            //                var fromEmail = Constants.FromEmail;
            //                var emailSubject = Constants.FinalPricePublishSubject;
            //                var plainText = string.Empty;
            //                SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@RAFinalPricePublishNotificationEmail", cnn);
            //                emailTemplatecmd.Parameters.AddWithValue("@RAFinalPricePublishNotificationEmail", Constants.RAFinalPricePublishNotificationEmail);
            //                using (SqlDataReader emailTemplateReader = emailTemplatecmd.ExecuteReader())
            //                {
            //                    var plainTemplate = emailTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime);
            //                    var htmlTemplate = emailTemplateReader["Template"].ToString().Replace(Constants.ReplaceMainContent, plainTemplate);
            //                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
            //                }
            //            }
            //            var smsMessage = string.Empty;
            //            if (isSMS)
            //            {
            //                toUsers = usersContext.Where(_ => _.MobileNumber != null && _.MobileNumber != "").Select(_ => _.MobileNumber).ToList();
            //                if (toUsers != null && toUsers.Any())
            //                {
            //                    var smsPlainTemplate = string.Empty;
            //                    EmailTemplate smsTemplate = new EmailTemplate();
            //                    SqlCommand emailTemplatecmd = new SqlCommand("select *from EmailTemplates where Name=@RAFinalPricePublishNotificationSMS", cnn);
            //                    emailTemplatecmd.Parameters.AddWithValue("@RAFinalPricePublishNotificationSMS", Constants.RAFinalPricePublishNotificationSMS);
            //                    using (SqlDataReader smsTemplateReader = emailTemplatecmd.ExecuteReader())
            //                    {
            //                        smsPlainTemplate = smsTemplateReader["PlainTemplate"].ToString().Replace(Constants.Date, date).Replace(Constants.FROM_TIME, fromTime).Replace(Constants.TO_TIME, toTime);
            //                        smsMessage = smsTemplateReader["Template"].ToString().Replace(Constants.ReplaceValueContent, smsPlainTemplate);
            //                        foreach (var mobileNo in toUsers)
            //                        {
            //                            amazonNotificationService.SendMessage(smsMessage, mobileNo);
            //                        }
            //                    }
            //                }
            //            }
            //            if (isPush)
            //            {
            //                foreach (var userContext in usersContext)
            //                {
            //                    if (userContext != null && userContext.RegistrationTypeId != null && userContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(userContext.PushTokenKey))
            //                    {
            //                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
            //                        {
            //                            PushTokenKey = userContext.PushTokenKey,
            //                            RegistrationTypeId = (int)userContext.RegistrationTypeId,
            //                            Title = Constants.FinalPricePublishSubject,
            //                            Message = smsMessage,
            //                            Id = "00",
            //                        };
            //                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    cnn.Close();

            //}
            //catch (Exception ex)
            //{

            //}
            //return result;
        }

        public ResultDto GetSkuFinalPriceList(FinalPricePublishDto inputDto)
        {
            _methodName = "GetSaudaShortViewList";
            var pricingListDto = new List<PricingDto>();
            try
            {
                using (var _context = new AdaniContext())
                {
                    _context.Database.CommandTimeout = 240;
                    if (inputDto == null)
                    {
                        return _resultService.ErrorMessage(Constants.InvalidRequest);
                    }
                    if (inputDto.LoginUserId == 0)
                    {
                        return _resultService.ErrorMessage(Constants.UserIdMissing);
                    }
                    var userContext = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                    if (userContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.UserNotFound);
                    }
                    int SkipCount = inputDto.SkipCount;
                    var pricingContext = _context.Pricing.AsNoTracking()
                        //.Where(_ => _.PublishId == inputDto.PublishId)
                        //.Join(_context.State.AsNoTracking(), p => p.StateId, s => s.Id, (p, s) => new { p, StateName = s.StateName })
                        .Join(_context.Depots.AsNoTracking().Where(_ => _.IsPlant == true), x => x.PlantId, pnt => pnt.Id, (x, pnt) => new { p = x, /*x.StateName,*/ PlantName = pnt.Name })
                        //.Join(_context.Depots.AsNoTracking().Where(_ => _.IsPlant == false), x => x.x.DepotId, dpt => dpt.Id, (x, dpt) => new {p= x.x, /*x.StateName,*/ x.PlantName, DepotName = dpt.Name })
                        //.Join(_context.FreightZones.AsNoTracking(), x => x.p.FrieghtZoneId, fz => fz.Id, (x, fz) => new { x.p, x.StateName, x.PlantName, x.DepotName, FreightZoneName = fz.Name })
                        //.Join(_context.FreightRoutes.AsNoTracking(), x => x.p.FrieghtRouteId, fr => fr.Id, (x, fr) => new { x.p, x.StateName, x.PlantName, x.DepotName, x.FreightZoneName, FreightRouteName = fr.Name })
                        //.Join(_context.PricePublish.AsNoTracking(), x => x.PublishId, pp => pp.Id, (x, pp) => new { x.p, x.StateName, x.PlantName, x.DepotName, PublishPrice = pp }
                        //)
                        .OrderBy(_ => _.p.Id).Skip(SkipCount).Take(50000);
                    if (pricingContext != null && pricingContext.Any())
                    {
                        pricingListDto = pricingContext.Select(_ => new PricingDto()
                        {
                            Id = _.p.Id,
                            SkuName = _.p.Sku.SkuName,
                            //OilTypeName = _.p.OilType.Name,
                            //SaudaBookingTypeId = _.p.SaudaBookingTypeId,
                            //SaudaBookingType = _.p.SaudaBookingType.Name,
                            //OilPackingType = _.p.OilPackingType.Name,
                            //State = _.StateName,
                            //TransportMode = _.p.TransportMode.Name,
                            Plant = _.PlantName,
                            //Price = _.Price,
                            //Depot = _.DepotName,
                            //FrieghtZone = _.FreightZoneName,
                            //FrieghtRoute = _.FreightRouteName,
                            //BiddingDate = _.p.BiddingDate,
                            //MaterialCost = _.p.MaterialCost,
                            //PackingCost = _.p.PackingCost,
                            //PrimaryFrieght = _.p.PrimaryFrieght,
                            //SecondaryFrieght = _.p.SecondaryFrieght,
                            //PlantSecondaryFrieght = _.p.PlantSecondaryFrieght,
                            //DepotCost = _.p.DepotCost,
                            //DetentionCost = _.p.DetentionCost,
                            //HoneycombCost = _.p.HoneycombCost,
                            //Margin = _.p.Margin,
                            //CushionMargin = _.p.CushionMargin,
                            //SchemeCostRecovery = _.p.SchemeCostRecovery,
                            //Discount = _.p.Discount,
                            //Premium = _.p.Premium,
                            //ProcessCost = _.p.ProcessCost,
                            //SumOfIngredientCost = _.p.SumOfIngredientCost,
                            //TpPrice = _.p.TpPrice,
                            //RaMargin = _.p.RaMargin,
                            //BaseRate = _.p.BaseRate,
                            //XMargin = _.p.XMargin,
                            //FinalRate = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.FinalRate > 0 ? _.p.FinalRate + _.p.XMargin : 0) : _.p.FinalRate,
                            //ExPlantPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ExPlantPrice > 0 ? _.p.ExPlantPrice + _.p.XMargin : 0) : _.p.ExPlantPrice,
                            //ForDepotPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ForDepotPrice > 0 ? _.p.ForDepotPrice + _.p.XMargin : 0) : _.p.ForDepotPrice,
                            //ForPlantPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ForPlantPrice > 0 ? _.p.ForPlantPrice + _.p.XMargin : 0) : _.p.ForPlantPrice,
                            //ExDepotPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ExDepotPrice > 0 ? _.p.ExDepotPrice + _.p.XMargin : 0) : _.p.ExDepotPrice,
                            //ClearanceRate = _.p.ClearanceRate,
                            //CounterBidOffer = _.p.CounterBidOffer,
                            //CounterBidLimit = _.p.CounterBidLimit,
                            //BpCpJumb = _.p.BpCpJumb,
                            //ExRakePrice = _.p.ExRakePrice,
                            //ForRakePrice = _.p.ForRakePrice,
                            //Loadability = _.p.LoadQuantity,
                            //StartDate = _.PublishPrice.StartDate,
                            //EndDate = _.PublishPrice.EndDate,
                            //Status = _.PublishPrice.StatusId == 1 ? DTO.Enums.PublishStatus.Started.ToString() : _.PublishPrice.StatusId == 2 ? DTO.Enums.PublishStatus.Completed.ToString() : _.PublishPrice.StatusId == 3 ? DTO.Enums.PublishStatus.Failed.ToString() : "",
                            //BiddingWindowId = _.p.BiddingWindowId
                        }).ToList();
                    }

                    if (pricingListDto != null && pricingListDto.Any())
                    {
                        pricingListDto.ForEach(f =>
                        {
                            if (f.BiddingWindowId > 0)
                            {
                                var biddingWindow = _context.BiddingWindowTiming.AsNoTracking().FirstOrDefault(w => w.Id == f.BiddingWindowId);
                                f.BiddingWindowTiming = biddingWindow.FromHours.ToString() + " - " + biddingWindow.ToHours.ToString();
                            }
                        });

                        return _resultService.SuccessObject(pricingListDto);
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }

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

        #region Published Price

        public ResultDto GetPublishedPriceDetails(PricePublishInputDto inputDto)
        {
            _methodName = "GetPublishedPriceDetails Testing";
            IList<PricePublishesDto> pricePublishedList = new List<PricePublishesDto>();
            IList<BiddingWindowTiming> biddingWindowData = new List<BiddingWindowTiming>();

            try
            {

                //var pricePublishedData = _emamiContext.PricePublish.AsNoTracking()
                // .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId).ToList()
                // .Select(s => s).ToList();

                var pricePublishedData = _emamiContext.PricePublish.AsNoTracking()
                .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                .Select(s => new
                {
                    Id = s.Id,
                    CreatedDate = s.CreatedDate,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    OilTypeId = s.OilTypeId,
                    StatusId = s.StatusId,
                    IsPublish = s.IsPublish,
                    PlantName = s.Plant.Name,
                    ModifiedDate = s.ModifiedDate
                }).ToList();

                if (pricePublishedData != null && pricePublishedData.Any())
                {
                    var publishIds = new List<long?>();
                    pricePublishedData.Select(s => s.Id).ToList().ForEach(f =>
                    {
                        publishIds.Add(f);
                    });

                    //var pricingData = _emamiContext.Pricing.AsNoTracking().Where(f => publishIds.Contains(f.PublishId)).Select(s => new { BiddingWindowId = s.BiddingWindowId, PublishDate = s.ModifiedDate, PublishId = s.PublishId });
                    //if (pricingData != null && pricingData.Any())
                    //{
                    // biddingWindowData = _emamiContext.BiddingWindowTiming.AsNoTracking().ToList().Where(w => pricingData.Select(s => s.BiddingWindowId).ToList().Contains(w.Id)).Select(s => new BiddingWindowTiming() { Id = s.Id, FromHours = s.FromHours, ToHours = s.ToHours }).ToList();
                    //}

                    foreach (var s in pricePublishedData)
                    {
                        //var pricing = pricingData != null && pricingData.Any() ? pricingData.FirstOrDefault(f => f.PublishId == s.Id) : null;
                        //var biddingWindow = (pricing != null && biddingWindowData != null && biddingWindowData.Any()) ? biddingWindowData.FirstOrDefault(f => f.Id == pricing.BiddingWindowId) : null;

                        var finalPriceRecordCount = _emamiContext.Pricing.AsNoTracking().Count();
                        //var finalPriceRecordCount = _emamiContext.Pricing.AsNoTracking().Count(c => c.PublishId == s.Id);
                        //var pricing = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(f => f.PublishId == s.Id);

                        //var biddingWindow = pricing != null ? _emamiContext.BiddingWindowTiming.AsNoTracking().FirstOrDefault(f => f.Id == pricing.BiddingWindowId) : null;

                        string oiltype = "";
                        if (!string.IsNullOrEmpty(s.OilTypeId))
                        {
                            var oilTypeIds = s.OilTypeId.Split(',').Select(Int64.Parse).ToList();
                            oiltype = oilTypeIds != null && oilTypeIds.Any() ? string.Join(",", _emamiContext.OilTypes.AsNoTracking().Where(w => oilTypeIds.Contains(w.Id)).Select(oil => oil.Name)) : "";
                        }

                        pricePublishedList.Add(new PricePublishesDto()
                        {
                            Id = s.Id,
                            CreatedDate = s.CreatedDate,
                            StartDate = s.StartDate,
                            EndDate = s.EndDate,
                            StatusId = s.StatusId,
                            Status = s.StatusId == 1 ? DTO.Enums.PublishStatus.Started.ToString() : s.StatusId == 2 ? DTO.Enums.PublishStatus.Completed.ToString() : s.StatusId == 3 ? DTO.Enums.PublishStatus.Failed.ToString() : "",
                            IsPublish = s.IsPublish,
                            SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
                            //ErrorMessage = s.ErrorMessage,
                            //BiddingWindowTiming = biddingWindow?.FromHours.ToString() + " - " + biddingWindow?.ToHours.ToString(),
                            PublishDate = s.ModifiedDate,
                            //FinalPriceRecordCount = pricingData.Count(c => c.PublishId == s.Id),
                            FinalPriceRecordCount = finalPriceRecordCount,
                            //Plant = s.Plant.Name,
                            Plant = s.PlantName,
                            OilType = oiltype
                        });
                    }
                }

                return _resultService.SuccessMessageWitObject(pricePublishedList.OrderByDescending(o => o.Id), "Success");

                //var pricePublishedList = _emamiContext.Pricing.AsNoTracking()
                // .Join(_emamiContext.PricePublish.AsNoTracking(), p => p.PublishId, pp => pp.Id, (p, pp) => new { Pricing = p, PricePublish = pp })
                // .Where(w => DbFunctions.TruncateTime(w.PricePublish.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.PricePublish.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                // .GroupBy(g => new
                // {
                // g.Pricing.PublishId,
                // g.PricePublish.Id,
                // g.PricePublish.CreatedDate,
                // g.PricePublish.StartDate,
                // g.PricePublish.EndDate,
                // g.PricePublish.StatusId,
                // g.PricePublish.IsPublish,
                // g.PricePublish.ErrorMessage,
                // g.PricePublish.SaudaBookingTypeId,
                // g.Pricing.BiddingWindowId,
                // g.Pricing.ModifiedDate
                // }).Select(s => new PricePublishesDto
                // {
                // Id = s.Key.Id,
                // CreatedDate = s.Key.CreatedDate,
                // StartDate = s.Key.StartDate,
                // EndDate = s.Key.EndDate,
                // StatusId = s.Key.StatusId,
                // Status = s.Key.StatusId == 1 ? DTO.Enums.PublishStatus.Started.ToString() : s.Key.StatusId == 2 ? DTO.Enums.PublishStatus.Completed.ToString() : s.Key.StatusId == 3 ? DTO.Enums.PublishStatus.Failed.ToString() : "",
                // IsPublish = s.Key.IsPublish,
                // SaudaBookingTypeId = s.Key.SaudaBookingTypeId,
                // BiddingWindowId = s.Key.BiddingWindowId,
                // PublishDate = s.Key.ModifiedDate,
                // ErrorMessage = s.Key.ErrorMessage,
                // FinalPriceRecordCount = s.Select(c => c).Count()
                // }).ToList(); 

                //if (pricePublishedList != null && pricePublishedList.Any())
                //{
                // pricePublishedList.ForEach(f =>
                // {
                // if (f.BiddingWindowId > 0)
                // {
                // var biddingWindow = _emamiContext.BiddingWindowTiming.AsNoTracking().FirstOrDefault(w => w.Id == f.BiddingWindowId);
                // f.BiddingWindowTiming = biddingWindow.FromHours.ToString() + " - " + biddingWindow.ToHours.ToString();
                // }
                // });
                //} 
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }


        public ResultDto SaveFinalPrice(SkuFinalpriceListInputDto inputDto)
        {
            _methodName = "SaveFinalPrice";
            var resultDto = new ResultDto();
            try
            {
                System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => SkuFinalpriceListForAdminUpdatedQueue(inputDto, cancellationToken));
                inputDto.PostStatus = true;
                resultDto = _resultService.SuccessMessageWitObject(inputDto, "success");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public void SkuFinalpriceListForAdminUpdatedQueue(SkuFinalpriceListInputDto inputDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            using (var _context = new AdaniContext())
            {
                _context.Database.CommandTimeout = 0;
                string message = "";
                string saudaBookingType = "";

                saudaBookingType = //inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                                   // ? DTO.Enums.SaudaBookingTypes.ReverseAuction.ToString()
                                   // : 
                    (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                    ? DTO.Enums.SaudaBookingTypes.TraditionalProcess.ToString() : string.Empty);
                message = $"---------------------------------------- {saudaBookingType} Final Price Generate Started ----------------------------------------";
                _logger.Info(message);
                _methodName = "SkuFinalpriceListForAdminUpdatedQueue";
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
                PricePublish pricePublishContext = new PricePublish();
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

                            //var freightRoutes = _context.Users.AsNoTracking().Join(_context.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u = u, ur = ur })
                            //     .Where(w => w.ur.RoleId == (long)DTO.Enums.Role.Dealer).Select(s => s.u.FreightRouteId).Distinct().ToList();

                            ////Get Freight Route details-----------------------------
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
                            //var freightRoutesDatas = _context.FreightRoutes.AsNoTracking().Where(w => fzIds.Contains(w.FreightZoneId) && w.IsActive)
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
                                //_logger.Info("Price Generate Started : " + DateHelper.UtcToIndia(DateTime.UtcNow));
                                #region PricePublish Table Insert - ProcessStart
                                pricePublishContext = new PricePublish()
                                {
                                    StatusId = (long)DTO.Enums.PublishStatus.Started,
                                    StartDate = currentDate,
                                    IsPublish = false,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = currentDate,
                                    OilTypeId = (inputDto.OilTypeIds != null && inputDto.OilTypeIds.Any() ? string.Join(",", inputDto.OilTypeIds) : ""),
                                    PlantId = inputDto.PlantId
                                };
                                //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                //{
                                //    pricePublishContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction;
                                //}
                                //else
                                //{
                                pricePublishContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                                //}
                                _context.PricePublish.Add(pricePublishContext);
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
                                                   .Where(w =>// DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                                                    w.PlantId == inputDto.PlantId
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
                                //&& freightRouteIds.Contains(w.FreightRouteId.Value)
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

                                    //If Vertical type is (SpecialityFat or HBC) and it contains rasoi oiltypes condition true
                                    if (sku.VerticalId == (int)DTO.Enums.Division.SpecialityFat || (sku.VerticalId == (int)DTO.Enums.Division.Hbc))/*rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(sku.OilTypeId.GetValueOrDefault()*/
                                    {
                                        //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), SKU ingredients is required
                                        //if (!_context.SkuIngrediant.AsNoTracking().Any(_ => _.SkuId == sku.Id))
                                        //if (!SkuIngrediantData.Any(_ => _.SkuId == sku.Id))
                                        //{
                                        //    valErrorMessage = Constants.BindErrorMessage(Constants.SkuIngredientNotAdded, valErrorMessage);
                                        //    isValidSku = false;
                                        //}
                                    }

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
                                            noofPiecesperCase = skuUomContext.ConversionFactor;
                                        }

                                        decimal formulationCost = 0;
                                        if (verticalId == (int)DTO.Enums.Division.Hbc)//rasoiOilTypeIds == null || !rasoiOilTypeIds.Any()) || !rasoiOilTypeIds.Contains(oilTypeId)//
                                        {
                                            #region Material Cost calculations
                                            var materialCostContext = MaterialCostData.FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                                            if (materialCostContext != null)
                                            {
                                                materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                                                materialCost = noofPiecesperCase * materialCost;
                                                materialCostId = materialCostContext.Id;
                                            }
                                            else
                                            {
                                                isError = true;
                                                skuLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost, skuLoopErrorMsg);
                                                //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost, dataMissingErrorMessage);
                                            }
                                            #endregion
                                        }
                                        else if (verticalId == (int)DTO.Enums.Division.SpecialityFat || (verticalId == (int)DTO.Enums.Division.Hbc))//rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(oilTypeId)//
                                        {

                                            //#region SKU Ingredients calculations
                                            ////var skuIngredientList = _context.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId && _.OilTypeId == oilTypeId).ToList();
                                            //var skuIngredientList = SkuIngrediantData.Where(_ => _.SkuId == skuId && _.OilTypeId == oilTypeId).ToList();
                                            //skuIngrediantPlantId = SkuIngrediantData.FirstOrDefault().SkuIngrediantPlantId;

                                            //foreach (var skuIngredient in skuIngredientList)
                                            //{
                                            //    //var ingredientCost = _context.IngredientCost.AsNoTracking()
                                            //    //    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                            //    //var ingredientCost = IngredientCostData.Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                            //    //       .FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                            //    var ingredientCost = IngredientCostData
                                            //          .FirstOrDefault(_ =>
                                            //           _.IsActive && _.IngredientId == skuIngredient.IngredientId);
                                            //    if (ingredientCost != null)
                                            //    {
                                            //        var oneKgIngredientCost = (ingredientCost.LooseOilRate * skuIngredient.Percentage) / 100;
                                            //        formulationCost = formulationCost + oneKgIngredientCost;
                                            //        ingredientCostId.Add(ingredientCost.Id);
                                            //    }
                                            //    else
                                            //    {
                                            //        isError = true;
                                            //        skuLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost, skuLoopErrorMsg);
                                            //        //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost, dataMissingErrorMessage);
                                            //    }
                                            //}

                                            //var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;

                                            //if (verticalId == (int)DTO.Enums.Vertical.SpecialityFat)
                                            //{
                                            //    var noofPiecesperCaseConstant = quantity * Constants.SFNoOfPiiceConstant;
                                            //    var kgToLtrConstant = 1000 * Constants.SFKgtoLtrConstant;
                                            //    var kp = kgToLtrConstant / DecimalFormat2(noofPiecesperCaseConstant);
                                            //    materialCost = specialityFatMaterialCost / kp;
                                            //    formulationCost = formulationCost / kp;
                                            //}
                                            //else
                                            //{
                                            //    materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, specialityFatMaterialCost, litreConversion);
                                            //    materialCost = noofPiecesperCase * materialCost;
                                            //    formulationCost = _resultService.GetSkuQuanityRate(uomId, quantity, formulationCost, litreConversion);
                                            //    formulationCost = noofPiecesperCase * formulationCost;
                                            //}
                                            //#endregion

                                        }

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
                                                //CommentedForAdani        
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
                                                                               + " ~ " + stateName + " ~ " + transportModeName + " ~ ~ ";

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
                                                                                       + " ~ " + stateName + " ~ " + transportModeName + " ~ " +
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
                                                                    OilTypeId = oilTypeId,
                                                                    OilPackingTypeId = oilPackingTypeId,
                                                                    PlantId = plantId,
                                                                    //DepotId = depotId,
                                                                    //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                                                    // Price = pricingLiveContext.Price,
                                                                    // SalesOrganizationId = salesOrganizationId,
                                                                    // DistributionChannelId = distributionChannelId,
                                                                    // DivisionId = divisionId,
                                                                    // ValidFrom = pricingLiveContext.ValidFrom,
                                                                    // ValidTo = pricingLiveContext.ValidTo,//StateId = (int)stateId,

                                                                    // StateId = (int)TraditionalProcessFinalPrice.StateId,
                                                                    //CityId = (int)TraditionalProcessFinalPrice.CityId,
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

                                                                //    //For Depot Price
                                                                //    //if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot)
                                                                //    //    pricingContext.ForDepotPrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;

                                                                //    ////For Rake Price
                                                                //    //if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake)
                                                                //    //    pricingContext.ForRakePrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;


                                                                //    //pricingContext.TpPrice = DecimalFormat2(exPlantPrice);
                                                                //    //finalPrice = exPlantPrice > 0 ? DecimalFormat2((exPlantPrice + raMarginCost)) : 0;
                                                                //    //pricingContext.ClearanceRate = finalPrice > 0 ? DecimalFormat2((finalPrice * inputDto.CounterBidLimit)) : 0;
                                                                //    //pricingContext.CounterBidOffer = finalPrice > 0 ? DecimalFormat2((finalPrice + inputDto.BpCpJump)) : 0;
                                                                //    //pricingContext.BaseRate = DecimalFormat2(finalPrice);
                                                                //    //pricingContext.XMargin = DecimalFormat2(inputDto.XMargin);
                                                                //    //pricingContext.FinalRate = finalPrice > 0 ? DecimalFormat2((finalPrice + inputDto.XMargin)) : 0;
                                                                //    //pricingContext.CounterBidLimit = DecimalFormat2(inputDto.CounterBidLimit);
                                                                //    //pricingContext.BpCpJumb = DecimalFormat2(inputDto.BpCpJump);
                                                                //    pricingContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction;
                                                                //    //pricingContext.BiddingWindowId = inputDto.BiddingWindowId;
                                                                //}
                                                                //else
                                                                //{
                                                                //For Depot Price
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
                                                                        //           //&& w.OilTypeId == pricingContext.OilTypeId
                                                                        //           && w.SaudaBookingTypeId == pricingContext.SaudaBookingTypeId
                                                                        //           //&& w.OilPackingTypeId == pricingContext.OilPackingTypeId
                                                                        //           //&& w.StateId == pricingContext.StateId
                                                                        //           //&& w.CityId == pricingContext.CityId
                                                                        //           //&& w.TransportModeId == pricingContext.TransportModeId
                                                                        //           && w.PlantId == pricingContext.PlantId
                                                                        //           && w.DepotId == pricingContext.DepotId
                                                                        //           //&& w.FrieghtZoneId == pricingContext.FrieghtZoneId
                                                                        //           //&& w.FrieghtRouteId == pricingContext.FrieghtRouteId
                                                                        //           //&& w.BiddingWindowId == pricingContext.BiddingWindowId
                                                                        //           //&& w.MaterialCost == pricingContext.MaterialCost
                                                                        //           //&& w.PackingCost == pricingContext.PackingCost
                                                                        //           //&& w.PrimaryFrieght == pricingContext.PrimaryFrieght
                                                                        //           //&& w.SecondaryFrieght == pricingContext.SecondaryFrieght
                                                                        //           //&& w.DepotCost == pricingContext.DepotCost
                                                                        //           //&& w.DetentionCost == pricingContext.DetentionCost
                                                                        //           //&& w.HoneycombCost == pricingContext.HoneycombCost
                                                                        //           //&& w.Margin == pricingContext.Margin
                                                                        //           //&& w.CushionMargin == pricingContext.CushionMargin
                                                                        //           //&& w.SchemeCostRecovery == pricingContext.SchemeCostRecovery
                                                                        //           //&& w.Discount == pricingContext.Discount
                                                                        //           //&& w.Premium == pricingContext.Premium
                                                                        //           //&& w.ProcessCost == pricingContext.ProcessCost
                                                                        //           //&& w.SumOfIngredientCost == pricingContext.SumOfIngredientCost
                                                                        //           //&& w.TpPrice == pricingContext.TpPrice
                                                                        //           //&& w.RaMargin == pricingContext.RaMargin
                                                                        //           //&& w.BaseRate == pricingContext.BaseRate
                                                                        //           //&& w.XMargin == pricingContext.XMargin
                                                                        //           //&& w.FinalRate == pricingContext.FinalRate
                                                                        //           //&& w.ExPlantPrice == pricingContext.ExPlantPrice
                                                                        //           //&& w.ForDepotPrice == pricingContext.ForDepotPrice
                                                                        //           //&& w.ForPlantPrice == pricingContext.ForPlantPrice
                                                                        //           //&& w.ExDepotPrice == pricingContext.ExDepotPrice
                                                                        //           //&& w.ExRakePrice == pricingContext.ExRakePrice
                                                                        //           //&& w.ForRakePrice == pricingContext.ForRakePrice
                                                                        //           //&& w.ClearanceRate == pricingContext.ClearanceRate
                                                                        //           //&& w.CounterBidOffer == pricingContext.CounterBidOffer
                                                                        //           //&& w.CounterBidLimit == pricingContext.CounterBidLimit
                                                                        //           //&& w.BpCpJumb == pricingContext.BpCpJumb
                                                                        //           );
                                                                        //}

                                                                        if (isValidPrice)
                                                                        {
                                                                            var finalDataTitleMissingErrorMessage = dataTitleMissingErrorMessage + " Price Already Generated " + "|";
                                                                            errorMessageList.Add(finalDataTitleMissingErrorMessage);
                                                                        }
                                                                        else
                                                                        {
                                                                            //pricingContext.IsPublish = false;
                                                                            //pricingContext.PublishId = pricePublishContext.Id;
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
                                                                        //pricingContext.PublishId = pricePublishContext.Id;
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

                                //if (isAvailable)
                                //{
                                //    _context.BulkInsertProxy(pricings);
                                //    _context.SaveChanges();
                                //    pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Completed;
                                //}
                                //else
                                //{
                                //    pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Failed;
                                //}
                                //if (errorMessageList != null && errorMessageList.Any())
                                //{
                                //    pricePublishContext.ErrorMessage = string.Join("", errorMessageList);
                                //}
                                //pricePublishContext.EndDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                //_context.SaveChanges();
                                //if (isAvailable)
                                //{
                                //    smsContent = Constants.PriceCalculationCompleted.Replace(Constants.Count, count.ToString()).Replace(Constants.StartTime, pricePublishContext.StartDate.ToString("hh:mm tt"))
                                //        .Replace(Constants.EndTime, pricePublishContext.EndDate.ToString("hh:mm tt"));
                                //}
                                //else
                                //{
                                //    smsContent = Constants.PriceCalculationFailed;
                                //}

                            }
                            else { _logger.Error("TransportModes is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                            //    }
                            //    else { _logger.Error("FreightRouteIds is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                            //}
                            //else { _logger.Error("FreightZoneId is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                        }
                        else { _logger.Error("DepotIds is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                    }
                    else { _logger.Error("SKU is empty : " + DateHelper.UtcToIndia(DateTime.UtcNow)); }

                    pricePublishContext.EndDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    if (isAvailable)
                    {
                        _context.BulkInsertProxy(pricings);
                        pricePublishContext.StatusId = (int)DTO.Enums.PublishStatus.Completed;
                        _context.SaveChanges();
                    }
                    else
                    {
                        pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Failed;
                    }
                    if (errorMessageList != null && errorMessageList.Any())
                    {
                        pricePublishContext.ErrorMessage = string.Join("", errorMessageList);
                    }
                    _context.SaveChanges();

                    if (isAvailable)
                    {
                        smsContent = Constants.PriceCalculationCompleted.Replace(Constants.Count, count.ToString()).Replace(Constants.StartTime, pricePublishContext.StartDate.ToString("hh:mm tt"))
                            .Replace(Constants.EndTime, pricePublishContext.EndDate.ToString("hh:mm tt"));
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
                    if (pricePublishContext != null && (pricePublishContext.StatusId == (int)DTO.Enums.PublishStatus.Started || pricePublishContext.StatusId == (int)DTO.Enums.PublishStatus.Failed))
                    {
                        pricePublishContext.StatusId = (int)DTO.Enums.PublishStatus.Failed;
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

        public long TpFinalPricingValidation(Pricing pricing)
        {
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

            long count = 0;
            using (SqlConnection conn = new SqlConnection(Config.DBConnectionString))
            {
                conn.Open();
                SqlDataReader rdr = null;
                SqlCommand command = new SqlCommand("Select Count(Id) From Pricings Where CreatedDate=@CreatedDate and SkuId=@SkuId and OilTypeId=@OilTypeId and SaudaBookingTypeId=@SaudaBookingTypeId and OilPackingTypeId=@OilPackingTypeId and StateId=@StateId and CityId=@CityId and TransportModeId=@TransportModeId and PlantId=@PlantId and DepotId=@DepotId and BiddingWindowId=@BiddingWindowId and MaterialCost=@MaterialCost and PackingCost=@PackingCost and PrimaryFrieght=@PrimaryFrieght and SecondaryFrieght=@SecondaryFrieght and DepotCost=@DepotCost and DetentionCost=@DetentionCost and HoneycombCost=@HoneycombCost and Margin=@Margin and CushionMargin=@CushionMargin and SchemeCostRecovery=@SchemeCostRecovery and Discount=@Discount and Premium=@Premium and ProcessCost=@ProcessCost and SumOfIngredientCost=@SumOfIngredientCost and TpPrice=@TpPrice and RaMargin=@RaMargin and BaseRate=@BaseRate and XMargin=@XMargin and FinalRate=@FinalRate and ExPlantPrice=@ExPlantPrice and ForDepotPrice=@ForDepotPrice and ForPlantPrice=@ForPlantPrice and ExDepotPrice=@ExDepotPrice and ExRakePrice=@ExRakePrice and ForRakePrice=@ForRakePrice and ClearanceRate=@ClearanceRate and CounterBidOffer=@CounterBidOffer and CounterBidLimit=@CounterBidLimit and BpCpJumb=@BpCpJumb", conn);
                //Add params values
                command.Parameters.AddWithValue("@CreatedDate", currentDate);
                command.Parameters.AddWithValue("@SkuId", pricing.SkuId);
                //command.Parameters.AddWithValue("@OilTypeId", pricing.OilTypeId);
                //command.Parameters.AddWithValue("@SaudaBookingTypeId", pricing.SaudaBookingTypeId);
                //command.Parameters.AddWithValue("@OilPackingTypeId", pricing.OilPackingTypeId);
                //command.Parameters.AddWithValue("@StateId", pricing.StateId);
                //command.Parameters.AddWithValue("@CityId", pricing.CityId);
                //command.Parameters.AddWithValue("@TransportModeId", pricing.TransportModeId);
                command.Parameters.AddWithValue("@PlantId", pricing.PlantId);
                //command.Parameters.AddWithValue("@DepotId", pricing.DepotId);
                //command.Parameters.AddWithValue("@FrieghtZoneId", pricing.FrieghtZoneId);
                //command.Parameters.AddWithValue("@FrieghtRouteId", pricing.FrieghtRouteId);
                //command.Parameters.AddWithValue("@BiddingWindowId", pricing.BiddingWindowId);
                //command.Parameters.AddWithValue("@MaterialCost", pricing.MaterialCost);
                //command.Parameters.AddWithValue("@PackingCost", pricing.PackingCost);
                //command.Parameters.AddWithValue("@PrimaryFrieght", pricing.PrimaryFrieght);
                //command.Parameters.AddWithValue("@SecondaryFrieght", pricing.SecondaryFrieght);
                //command.Parameters.AddWithValue("@DepotCost", pricing.DepotCost);
                //command.Parameters.AddWithValue("@DetentionCost", pricing.DetentionCost);
                //command.Parameters.AddWithValue("@HoneycombCost", pricing.HoneycombCost);
                //command.Parameters.AddWithValue("@Margin", pricing.Margin);
                //command.Parameters.AddWithValue("@CushionMargin", pricing.CushionMargin);
                //command.Parameters.AddWithValue("@SchemeCostRecovery", pricing.SchemeCostRecovery);
                //command.Parameters.AddWithValue("@Discount", pricing.Discount);
                //command.Parameters.AddWithValue("@Premium", pricing.Premium);
                //command.Parameters.AddWithValue("@ProcessCost", pricing.ProcessCost);
                //command.Parameters.AddWithValue("@SumOfIngredientCost", pricing.SumOfIngredientCost);
                //command.Parameters.AddWithValue("@TpPrice", pricing.TpPrice);
                //command.Parameters.AddWithValue("@RaMargin", pricing.RaMargin);
                //command.Parameters.AddWithValue("@BaseRate", pricing.BaseRate);
                //command.Parameters.AddWithValue("@XMargin", pricing.XMargin);
                //command.Parameters.AddWithValue("@FinalRate", pricing.FinalRate);
                //command.Parameters.AddWithValue("@ExPlantPrice", pricing.ExPlantPrice);
                //command.Parameters.AddWithValue("@ForDepotPrice", pricing.ForDepotPrice);
                //command.Parameters.AddWithValue("@ForPlantPrice", pricing.ForPlantPrice);
                //command.Parameters.AddWithValue("@ExDepotPrice", pricing.ForDepotPrice);
                //command.Parameters.AddWithValue("@ExRakePrice", pricing.ExRakePrice);
                //command.Parameters.AddWithValue("@ForRakePrice", pricing.ForRakePrice);
                //command.Parameters.AddWithValue("@ClearanceRate", pricing.ClearanceRate);
                //command.Parameters.AddWithValue("@CounterBidOffer", pricing.CounterBidOffer);
                //command.Parameters.AddWithValue("@CounterBidLimit", pricing.CounterBidLimit);
                //command.Parameters.AddWithValue("@BpCpJumb", pricing.BpCpJumb);
                rdr = command.ExecuteReader();

                //Read result from datareader
                while (rdr.Read())
                {
                    count = Convert.ToInt64(rdr[0]);
                }
            }
            return count;
        }

        public long RaFinalPricingValidation(Pricing pricing)
        {
            long count = 0;
            using (SqlConnection conn = new SqlConnection(Config.DBConnectionString))
            {
                conn.Open();
                SqlDataReader rdr = null;
                SqlCommand command = new SqlCommand("Select Count(Id) From Pricings Where CreatedDate=@CreatedDate and BiddingWindowId=@BiddingWindowId,SkuId=@SkuId and OilTypeId=@OilTypeId and SaudaBookingTypeId=@SaudaBookingTypeId and OilPackingTypeId=@OilPackingTypeId and StateId=@StateId and CityId=@CityId and TransportModeId=@TransportModeId and PlantId=@PlantId and DepotId=@DepotId   and BiddingWindowId=@BiddingWindowId and MaterialCost=@MaterialCost and PackingCost=@PackingCost and PrimaryFrieght=@PrimaryFrieght and SecondaryFrieght=@SecondaryFrieght and DepotCost=@DepotCost and DetentionCost=@DetentionCost and HoneycombCost=@HoneycombCost and Margin=@Margin and CushionMargin=@CushionMargin and SchemeCostRecovery=@SchemeCostRecovery and Discount=@Discount and Premium=@Premium and ProcessCost=@ProcessCost and SumOfIngredientCost=@SumOfIngredientCost and TpPrice=@TpPrice and RaMargin=@RaMargin and BaseRate=@BaseRate and XMargin=@XMargin and FinalRate=@FinalRate and ExPlantPrice=@ExPlantPrice and ForDepotPrice=@ForDepotPrice and ForPlantPrice=@ForPlantPrice and ExDepotPrice=@ExDepotPrice and ExRakePrice=@ExRakePrice and ForRakePrice=@ForRakePrice and ClearanceRate=@ClearanceRate and CounterBidOffer=@CounterBidOffer and CounterBidLimit=@CounterBidLimit and BpCpJumb=@BpCpJumb", conn);
                //Add params values
                //command.Parameters.AddWithValue("@CreatedDate", pricing.BiddingWindowId);
                //command.Parameters.AddWithValue("@BiddingWindowId", pricing.BiddingWindowId);
                command.Parameters.AddWithValue("@SkuId", pricing.SkuId);
                //command.Parameters.AddWithValue("@OilTypeId", pricing.OilTypeId);
                //command.Parameters.AddWithValue("@SaudaBookingTypeId", pricing.SaudaBookingTypeId);
                //command.Parameters.AddWithValue("@OilPackingTypeId", pricing.OilPackingTypeId);
                //command.Parameters.AddWithValue("@StateId", pricing.StateId);
                //command.Parameters.AddWithValue("@CityId", pricing.CityId);
                //command.Parameters.AddWithValue("@TransportModeId", pricing.TransportModeId);
                command.Parameters.AddWithValue("@PlantId", pricing.PlantId);
                //command.Parameters.AddWithValue("@DepotId", pricing.DepotId);
                //command.Parameters.AddWithValue("@FrieghtZoneId", pricing.FrieghtZoneId);
                //command.Parameters.AddWithValue("@FrieghtRouteId", pricing.FrieghtRouteId);
                //command.Parameters.AddWithValue("@BiddingWindowId", pricing.BiddingWindowId);
                //command.Parameters.AddWithValue("@MaterialCost", pricing.MaterialCost);
                //command.Parameters.AddWithValue("@PackingCost", pricing.PackingCost);
                //command.Parameters.AddWithValue("@PrimaryFrieght", pricing.PrimaryFrieght);
                //command.Parameters.AddWithValue("@SecondaryFrieght", pricing.SecondaryFrieght);
                //command.Parameters.AddWithValue("@DepotCost", pricing.DepotCost);
                //command.Parameters.AddWithValue("@DetentionCost", pricing.DetentionCost);
                //command.Parameters.AddWithValue("@HoneycombCost", pricing.HoneycombCost);
                //command.Parameters.AddWithValue("@Margin", pricing.Margin);
                //command.Parameters.AddWithValue("@CushionMargin", pricing.CushionMargin);
                //command.Parameters.AddWithValue("@SchemeCostRecovery", pricing.SchemeCostRecovery);
                //command.Parameters.AddWithValue("@Discount", pricing.Discount);
                //command.Parameters.AddWithValue("@Premium", pricing.Premium);
                //command.Parameters.AddWithValue("@ProcessCost", pricing.ProcessCost);
                //command.Parameters.AddWithValue("@SumOfIngredientCost", pricing.SumOfIngredientCost);
                //command.Parameters.AddWithValue("@TpPrice", pricing.TpPrice);
                //command.Parameters.AddWithValue("@RaMargin", pricing.RaMargin);
                //command.Parameters.AddWithValue("@BaseRate", pricing.BaseRate);
                //command.Parameters.AddWithValue("@XMargin", pricing.XMargin);
                //command.Parameters.AddWithValue("@FinalRate", pricing.FinalRate);
                //command.Parameters.AddWithValue("@ExPlantPrice", pricing.ExPlantPrice);
                //command.Parameters.AddWithValue("@ForDepotPrice", pricing.ForDepotPrice);
                //command.Parameters.AddWithValue("@ForPlantPrice", pricing.ForPlantPrice);
                //command.Parameters.AddWithValue("@ExDepotPrice", pricing.ForDepotPrice);
                //command.Parameters.AddWithValue("@ExRakePrice", pricing.ExRakePrice);
                //command.Parameters.AddWithValue("@ForRakePrice", pricing.ForRakePrice);
                //command.Parameters.AddWithValue("@ClearanceRate", pricing.ClearanceRate);
                //command.Parameters.AddWithValue("@CounterBidOffer", pricing.CounterBidOffer);
                //command.Parameters.AddWithValue("@CounterBidLimit", pricing.CounterBidLimit);
                //command.Parameters.AddWithValue("@BpCpJumb", pricing.BpCpJumb);
                rdr = command.ExecuteReader();

                //Read result from datareader
                while (rdr.Read())
                {
                    count = Convert.ToInt64(rdr[0]);
                }
            }
            return count;
        }

        /*
        public void BeforePerformanceImprovementSkuFinalpriceListForAdminUpdatedQueue(SkuFinalpriceListInputDto inputDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            using (var _context = new EmamiContext())
            {
                string message = "";
                string saudaBookingType = "";

                saudaBookingType = inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                    ? DTO.Enums.SaudaBookingTypes.ReverseAuction.ToString()
                    : (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                    ? DTO.Enums.SaudaBookingTypes.TraditionalProcess.ToString() : string.Empty);
                message = $"---------------------------------------- {saudaBookingType} Final Price Generate Started ----------------------------------------";
                _logger.Info(message);
                _methodName = "SkuFinalpriceListForAdminUpdatedQueue";
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
                PricePublish pricePublishContext = new PricePublish();
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
                                       .Select(s => new { Id = s.Id, SkuName = s.SkuName, SkuCode = s.SkuCode, Quantity = s.Quantity, UomId = s.UomId, VerticalId = s.VerticalId, OilTypeId = s.OilTypeId, PackGroupId = s.PackGroupId, ProcessCost = s.ProcessCost }).ToList();
                    //Process the SKU's0
                    if (skuList != null && skuList.Any())
                    {
                        //Get Depots
                        inputDto.DepotIds = _context.PlantDepotMapping.AsNoTracking().Where(_ => _.PlantId == inputDto.PlantId).Select(_ => _.DepotId).ToList();
                        List<long> stateIds = inputDto.StateIds;
                        //Get Rasoi oiltypes
                        var rasoiOilTypeIds = _context.OilTypes.AsNoTracking().Where(w => w.IsRasoi).Select(s => s.Id).ToList();


                        if (inputDto.DepotIds != null && inputDto.DepotIds.Any())
                        {
                            depotPlantIds.Add(inputDto.PlantId);
                            depotPlantIds.AddRange(inputDto.DepotIds);

                            //Get Freight Route details

                            var freightZoneId = _context.FreightZones.AsNoTracking().ToList().Where(w => inputDto.StateIds.Contains(Convert.ToInt32(w.StateId)) && w.IsActive).Select(s => s.Id).ToList();
                            if (freightZoneId != null && freightZoneId.Any())
                            {
                                var freightRouteIds = _context.FreightRoutes.AsNoTracking().Where(_ => _.IsActive && freightZoneId.Contains(_.FreightZoneId)).Select(_ => _.Id).ToList();
                                if (freightRouteIds != null && freightRouteIds.Any())
                                {
                                    //Get Transport Modes
                                    var transportModeData = _context.TransportModes.AsNoTracking().Where(w => w.IsActive).Select(s => s);
                                    var transportModes = transportModeData.Select(s => s.Id).ToList();
                                    if (transportModes != null && transportModes.Any())
                                    {
                                        //_logger.Info("Price Generate Started : " + DateHelper.UtcToIndia(DateTime.UtcNow));
                                        #region PricePublish Table Insert - ProcessStart
                                        pricePublishContext = new PricePublish()
                                        {
                                            StatusId = (long)DTO.Enums.PublishStatus.Started,
                                            StartDate = currentDate,
                                            IsPublish = false,
                                            CreatedBy = inputDto.LoginUserId,
                                            CreatedDate = currentDate,
                                            OilTypeId = (inputDto.OilTypeIds != null && inputDto.OilTypeIds.Any() ? string.Join(",", inputDto.OilTypeIds) : ""),
                                            PlantId = inputDto.PlantId
                                        };
                                        if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                        {
                                            pricePublishContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction;
                                        }
                                        else
                                        {
                                            pricePublishContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                                        }
                                        _context.PricePublish.Add(pricePublishContext);
                                        _context.SaveChanges();
                                        #endregion

                                        #region Get Common data
                                        var MaterialCostData = _context.MaterialCosts.AsNoTracking()
                                                                        .Where(_ => currentDate >= _.ValidFrom && currentDate <= _.ValidTo && _.IsActive);
                                        var PackingCostData = _context.PackingCosts.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var DepotCostData = _context.DepotCosts.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var DetentionCostData = _context.DetentionCosts.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var ProfitMarginsData = _context.ProfitMargins.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var CushionMarginData = _context.CushionMargins.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var SchemeCostData = _context.SchemeCosts.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var PrimaryFreightData = _context.PrimaryFreights.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var SecondaryFreightData = _context.SecondaryFreights.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var HoneycombCostData = _context.HoneycombCosts.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var RaMarginData = _context.RaMargin.AsNoTracking()
                                            .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var LoadCapacityConversionData = _context.LoadCapacityConversion.AsNoTracking()
                                           .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive);
                                        var PricingData = _context.Pricing.AsNoTracking().Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(currentDate));
                                        #endregion

                                        #region Load Capacity filter based on PrimaryFreight and SecondaryFreight
                                        var primaryFreightLoadCapacity = PrimaryFreightData.Where(w => w.VerticalId == inputDto.VerticalId && w.PlantId == inputDto.PlantId && inputDto.DepotIds.Contains(w.DepotId)
                                                                    && transportModes.Contains(w.TransportModeId)).Select(s => s.LoadCapacity).ToList();
                                        var secondaryFreightLoadCapacity = SecondaryFreightData.Where(w => w.VerticalId == inputDto.VerticalId && depotPlantIds.Contains(w.DepotId)
                                        && freightRouteIds.Contains(w.FreightRouteId.Value) && transportModes.Contains(w.TransportModeId)).Select(s => s.Capacity).ToList();
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

                                            //If Vertical type is (SpecialityFat or HBC) and it contains rasoi oiltypes condition true
                                            if (sku.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat || (sku.VerticalId == (int)DTO.Enums.Vertical.Hbc && (rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(sku.OilTypeId.GetValueOrDefault()))))
                                            {
                                                //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), SKU ingredients is required
                                                if (!_context.SkuIngrediant.AsNoTracking().Any(_ => _.SkuId == sku.Id))
                                                {
                                                    valErrorMessage = Constants.BindErrorMessage(Constants.SkuIngredientNotAdded, valErrorMessage);
                                                    isValidSku = false;
                                                }
                                            }

                                            if (!_context.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                                            {
                                                valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuUom2Field, valErrorMessage);
                                                isValidSku = false;
                                            }

                                            if (!_context.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
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
                                                var skuContext = _context.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                                                oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                                                oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                                                uomId = Convert.ToInt64(skuContext.UomId);
                                                quantity = skuContext.Quantity;
                                                var dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ ~ ~ ~ ~ ~ ";

                                                //Get OilType details
                                                var oilTypeContext = _context.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                                                verticalId = oilTypeContext.VerticalId;
                                                litreConversion = oilTypeContext.LitreConversion;

                                                var skuUomContext = _context.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                                if (skuUomContext != null)
                                                {
                                                    noofPiecesperCase = skuUomContext.ConversionFactor;
                                                }

                                                decimal formulationCost = 0;
                                                if (verticalId == (int)DTO.Enums.Vertical.Hbc && ((rasoiOilTypeIds == null || !rasoiOilTypeIds.Any()) || !rasoiOilTypeIds.Contains(oilTypeId)))
                                                {
                                                    #region Material Cost calculations
                                                    var materialCostContext = MaterialCostData.FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                                                    if (materialCostContext != null)
                                                    {
                                                        materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                                                        materialCost = noofPiecesperCase * materialCost;
                                                        materialCostId = materialCostContext.Id;
                                                    }
                                                    else
                                                    {
                                                        isError = true;
                                                        skuLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost, skuLoopErrorMsg);
                                                        //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost, dataMissingErrorMessage);
                                                    }
                                                    #endregion
                                                }
                                                else if (verticalId == (int)DTO.Enums.Vertical.SpecialityFat || (verticalId == (int)DTO.Enums.Vertical.Hbc && (rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(oilTypeId))))
                                                {

                                                    #region SKU Ingredients calculations
                                                    var skuIngredientList = _context.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId && _.OilTypeId == oilTypeId).ToList();
                                                    foreach (var skuIngredient in skuIngredientList)
                                                    {
                                                        var ingredientCost = _context.IngredientCost.AsNoTracking()
                                                            .Where(_ => currentDate >= _.ValidFrom && currentDate <= _.ValidTo && _.IsActive).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                                        if (ingredientCost != null)
                                                        {
                                                            var oneKgIngredientCost = (ingredientCost.LooseOilRate * skuIngredient.Percentage) / 100;
                                                            formulationCost = formulationCost + oneKgIngredientCost;
                                                            ingredientCostId.Add(ingredientCost.Id);
                                                        }
                                                        else
                                                        {
                                                            isError = true;
                                                            skuLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost, skuLoopErrorMsg);
                                                            //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost, dataMissingErrorMessage);
                                                        }
                                                    }

                                                    var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;

                                                    if (verticalId == (int)DTO.Enums.Vertical.SpecialityFat)
                                                    {
                                                        var noofPiecesperCaseConstant = quantity * Constants.SFNoOfPiiceConstant;
                                                        var kgToLtrConstant = 1000 * Constants.SFKgtoLtrConstant;
                                                        var kp = kgToLtrConstant / DecimalFormat2(noofPiecesperCaseConstant);
                                                        materialCost = specialityFatMaterialCost / kp;
                                                        formulationCost = formulationCost / kp;
                                                    }
                                                    else
                                                    {
                                                        materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, specialityFatMaterialCost, litreConversion);
                                                        materialCost = noofPiecesperCase * materialCost;
                                                        formulationCost = _resultService.GetSkuQuanityRate(uomId, quantity, formulationCost, litreConversion);
                                                        formulationCost = noofPiecesperCase * formulationCost;
                                                    }
                                                    #endregion

                                                }

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
                                                    var depotName = _context.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == depotId)?.Name;
                                                    dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName + " ~ ~ ~ ~ ~ ";

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

                                                        var stateName = _context.State.AsNoTracking().FirstOrDefault(_ => _.Id == stateId)?.StateName;
                                                        dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName + " ~ " + stateName + " ~ ~ ~ ~ ";

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
                                                        if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                        {

                                                            #region RaMargin Cost calculations
                                                            var raMarginCostContext = RaMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId &&
                                                                                                            _.OilPackingTypeId == oilPackingTypeId);
                                                            if (raMarginCostContext != null)
                                                            {
                                                                raMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, raMarginCostContext.RatePerMt, litreConversion);
                                                                raMarginCost = noofPiecesperCase * raMarginCost;
                                                                raMarginCostId = raMarginCostContext.Id;
                                                            }
                                                            else
                                                            {
                                                                isStateError = true;
                                                                stateLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost, stateLoopErrorMsg);
                                                                //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost + " - ", dataMissingErrorMessage);
                                                            }
                                                            #endregion

                                                        }

                                                        #region Scheme Cost Recovery calculations
                                                        var schemeCostContext = SchemeCostData.FirstOrDefault(_ => _.PackGroupId == sku.PackGroupId && _.OilTypeId == sku.OilTypeId && _.StateId == stateId);
                                                        if (schemeCostContext != null)
                                                        {
                                                            schemeCostRecovery = _resultService.GetSkuQuanityRate(uomId, quantity, schemeCostContext.RatePerMt, litreConversion);
                                                            schemeCostRecovery = noofPiecesperCase * schemeCostRecovery;
                                                            schemeCostRecoveryId = schemeCostContext.Id;
                                                        }
                                                        #endregion

                                                        foreach (var freightRouteId in freightRouteIds)
                                                        {
                                                            var freightRouteName = _context.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == freightRouteId)?.Name;

                                                            foreach (var transportId in transportModes)
                                                            {
                                                                freightRouteLoopErrorMsg = "";
                                                                var isFrieghtRouteError = false;
                                                                var transportMode = string.Empty;
                                                                decimal honeycombCost = 0;
                                                                long honeycombCostId = 0;

                                                                var transportModeName = transportModeData.FirstOrDefault(_ => _.Id == transportId)?.Name;

                                                                dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName
                                                                                    + " ~ " + stateName + " ~ "  + transportModeName + " ~ ~ ";

                                                                var loadCapacityContextList = LoadCapacityConversionData.Where(_ => _.SkuId == skuId
                                                                                                      && _.VerticalId == verticalId && _.TransportModeId == transportId && loadCapacities.Contains(_.LoadCapacity)).ToList();



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

                                                                        dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depotName
                                                                                            + " ~ " + stateName + " ~ "  + transportModeName + " ~ " +
                                                                                            loadCapacity + " ~ ";


                                                                        #region Primary Freight Calculation
                                                                        var primaryFrieghtContext = PrimaryFreightData.FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                                                                                                                                    _.VerticalId == verticalId && _.TransportModeId == transportId && _.LoadCapacity == Constants.DefaultLoadQuantity);
                                                                        if (primaryFrieghtContext != null)
                                                                        {
                                                                            //var defaultLoadCapacity16MT = loadCapacityContextList.FirstOrDefault(_ => _.LoadCapacity == Constants.DefaultLoadQuantity);
                                                                            var defaultLoadCapacity16MT = LoadCapacityConversionData.FirstOrDefault(_ => _.OilTypeId == oilTypeId
                                                                            && _.VerticalId == verticalId && _.TransportModeId == transportId && _.LoadCapacity == Constants.DefaultLoadQuantity
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
                                                                        var secondaryFrieghtContext = SecondaryFreightData.FirstOrDefault(_ => _.FreightRouteId == freightRouteId
                                                                                                                                                && _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == depotId && _.VerticalId == verticalId);
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
                                                                        var secondaryFrieghtContextForPlant = SecondaryFreightData.FirstOrDefault(_ => _.FreightRouteId == freightRouteId
                                                                                                                                                && _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == plantId && _.VerticalId == verticalId);
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
                                                                            if (primaryFrieght > 0)
                                                                            {
                                                                                exDepotPrice = ((materialCost + packingCost + primaryFrieght + depoCost + detentionCost +
                                                                                                         marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            }
                                                                            exPlantPrice = ((materialCost + packingCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            if (secondaryFrieghtForPlant > 0)
                                                                            {
                                                                                forPlantPrice = ((materialCost + packingCost + secondaryFrieghtForPlant +
                                                                                                  honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            }
                                                                            if (primaryFrieght > 0)
                                                                            {
                                                                                exRakePrice = ((materialCost + packingCost + primaryFrieght +
                                                                                                 honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                            }

                                                                            var pricingContext = new Pricing()
                                                                            {
                                                                                SkuId = skuId,
                                                                                OilTypeId = oilTypeId,
                                                                                OilPackingTypeId = oilPackingTypeId,
                                                                                PlantId = plantId,
                                                                                DepotId = depotId,
                                                                                StateId = (int)stateId,
                                                                                FrieghtRouteId = freightRouteId,
                                                                                FrieghtZoneId = _context.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == freightRouteId)?.FreightZoneId ?? 0,
                                                                                TransportModeId = transportId,
                                                                                LoadQuantity = loadCapacity,
                                                                                SumOfIngredientCost = formulationCost,
                                                                                CreatedBy = inputDto.LoginUserId,
                                                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                                                IsActive = true,
                                                                            };

                                                                            if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                                            {

                                                                                exDepotPrice = exDepotPrice > 0 ? (exDepotPrice + raMarginCost) : 0;
                                                                                exPlantPrice = exPlantPrice + raMarginCost;
                                                                                forPlantPrice = forPlantPrice > 0 ? (forPlantPrice + raMarginCost) : 0;
                                                                                exRakePrice = exRakePrice > 0 ? (exRakePrice + raMarginCost) : 0;
                                                                                pricingContext.ForDepotPrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;
                                                                                pricingContext.ForRakePrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;

                                                                                pricingContext.TpPrice = DecimalFormat2(exPlantPrice);
                                                                                finalPrice = exPlantPrice > 0 ? DecimalFormat2((exPlantPrice + raMarginCost)) : 0;
                                                                                pricingContext.ClearanceRate = finalPrice > 0 ? DecimalFormat2((finalPrice * inputDto.CounterBidLimit)) : 0;
                                                                                pricingContext.CounterBidOffer = finalPrice > 0 ? DecimalFormat2((finalPrice + inputDto.BpCpJump)) : 0;
                                                                                pricingContext.BaseRate = DecimalFormat2(finalPrice);
                                                                                pricingContext.XMargin = DecimalFormat2(inputDto.XMargin);
                                                                                pricingContext.FinalRate = finalPrice > 0 ? DecimalFormat2((finalPrice + inputDto.XMargin)) : 0;
                                                                                pricingContext.CounterBidLimit = DecimalFormat2(inputDto.CounterBidLimit);
                                                                                pricingContext.BpCpJumb = DecimalFormat2(inputDto.BpCpJump);
                                                                                pricingContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction;
                                                                                pricingContext.BiddingWindowId = inputDto.BiddingWindowId;
                                                                            }
                                                                            else
                                                                            {
                                                                                pricingContext.ForDepotPrice = DecimalFormat2(finalPrice);
                                                                                pricingContext.ForRakePrice = DecimalFormat2(finalPrice);
                                                                                pricingContext.FinalRate = DecimalFormat2(finalPrice);
                                                                                pricingContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                                                                                pricingContext.TpPrice = DecimalFormat2(finalPrice);
                                                                            }
                                                                            pricingContext.MaterialCost = DecimalFormat2(materialCost);
                                                                            pricingContext.PackingCost = DecimalFormat2(packingCost);
                                                                            pricingContext.Premium = DecimalFormat2(premium);
                                                                            pricingContext.Discount = DecimalFormat2(discount);
                                                                            pricingContext.PrimaryFrieght = DecimalFormat2(primaryFrieght);
                                                                            pricingContext.SecondaryFrieght = DecimalFormat2(secondaryFrieght);
                                                                            pricingContext.PlantSecondaryFrieght = DecimalFormat2(secondaryFrieghtForPlant);
                                                                            pricingContext.DepotCost = DecimalFormat2(depoCost);
                                                                            pricingContext.DetentionCost = DecimalFormat2(detentionCost);
                                                                            pricingContext.HoneycombCost = DecimalFormat2(honeycombCost);
                                                                            pricingContext.Margin = DecimalFormat2(marginCost);
                                                                            pricingContext.CushionMargin = DecimalFormat2(cushionMarginCost);
                                                                            pricingContext.SchemeCostRecovery = DecimalFormat2(schemeCostRecovery);
                                                                            pricingContext.ProcessCost = DecimalFormat2(schemeCostRecovery);
                                                                            pricingContext.RaMargin = DecimalFormat2(raMarginCost);

                                                                            pricingContext.ExPlantPrice = DecimalFormat2(exPlantPrice);
                                                                            pricingContext.ExDepotPrice = DecimalFormat2(exDepotPrice);
                                                                            pricingContext.ForPlantPrice = DecimalFormat2(forPlantPrice);
                                                                            pricingContext.ExRakePrice = DecimalFormat2(exRakePrice);
                                                                            pricingContext.MaterialCostId = materialCostId;
                                                                            pricingContext.IngredientCostId = (ingredientCostId != null && ingredientCostId.Any()) ? string.Join(",", ingredientCostId) : "";
                                                                            pricingContext.PackingCostId = packingCostId;
                                                                            pricingContext.DepotCostId = depotCostId;
                                                                            pricingContext.DetentionCostId = detentionCostId;
                                                                            pricingContext.ProfitMarginId = marginCostId;
                                                                            pricingContext.CushionMarginId = cushionMarginCostId;
                                                                            pricingContext.SchemeCostId = schemeCostRecoveryId;
                                                                            pricingContext.PrimaryFrieghtId = primaryFrieghtId;
                                                                            pricingContext.SecondaryFrieghtId = secondaryFrieghtId;
                                                                            pricingContext.SecondaryFrieghtForPlantId = secondaryFrieghtForPlantId;
                                                                            pricingContext.HoneycombCostId = honeycombCostId;
                                                                            pricingContext.RaMarginId = raMarginCostId;
                                                                            pricingContext.LoadCapacityId = loadCapacityItem.Id;

                                                                            if (!isError)
                                                                            {
                                                                                if (PricingData != null && PricingData.Any())
                                                                                {
                                                                                    List<Pricing> publishedPrice = new List<Pricing>();
                                                                                    if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                                                                    {
                                                                                        publishedPrice = PricingData.Where(w => w.SkuId == pricingContext.SkuId
                                                                                        && w.OilTypeId == pricingContext.OilTypeId
                                                                                        && w.SaudaBookingTypeId == pricingContext.SaudaBookingTypeId
                                                                                        && w.OilPackingTypeId == pricingContext.OilPackingTypeId
                                                                                        && w.StateId == pricingContext.StateId
                                                                                        && w.CityId == pricingContext.CityId
                                                                                        && w.TransportModeId == pricingContext.TransportModeId
                                                                                        && w.PlantId == pricingContext.PlantId
                                                                                        && w.DepotId == pricingContext.DepotId
                                                                                        && w.FrieghtZoneId == pricingContext.FrieghtZoneId
                                                                                        && w.FrieghtRouteId == pricingContext.FrieghtRouteId
                                                                                        && w.BiddingWindowId == pricingContext.BiddingWindowId
                                                                                        && w.MaterialCost == pricingContext.MaterialCost
                                                                                        && w.PackingCost == pricingContext.PackingCost
                                                                                        && w.PrimaryFrieght == pricingContext.PrimaryFrieght
                                                                                        && w.SecondaryFrieght == pricingContext.SecondaryFrieght
                                                                                        && w.DepotCost == pricingContext.DepotCost
                                                                                        && w.DetentionCost == pricingContext.DetentionCost
                                                                                        && w.HoneycombCost == pricingContext.HoneycombCost
                                                                                        && w.Margin == pricingContext.Margin
                                                                                        && w.CushionMargin == pricingContext.CushionMargin
                                                                                        && w.SchemeCostRecovery == pricingContext.SchemeCostRecovery
                                                                                        && w.Discount == pricingContext.Discount
                                                                                        && w.Premium == pricingContext.Premium
                                                                                        && w.ProcessCost == pricingContext.ProcessCost
                                                                                        && w.SumOfIngredientCost == pricingContext.SumOfIngredientCost
                                                                                        && w.TpPrice == pricingContext.TpPrice
                                                                                        && w.RaMargin == pricingContext.RaMargin
                                                                                        && w.BaseRate == pricingContext.BaseRate
                                                                                        && w.XMargin == pricingContext.XMargin
                                                                                        && w.FinalRate == pricingContext.FinalRate
                                                                                        && w.ExPlantPrice == pricingContext.ExPlantPrice
                                                                                        && w.ForDepotPrice == pricingContext.ForDepotPrice
                                                                                        && w.ForPlantPrice == pricingContext.ForPlantPrice
                                                                                        && w.ExDepotPrice == pricingContext.ExDepotPrice
                                                                                        && w.ExRakePrice == pricingContext.ExRakePrice
                                                                                        && w.ForRakePrice == pricingContext.ForRakePrice
                                                                                        && w.ClearanceRate == pricingContext.ClearanceRate
                                                                                        && w.CounterBidOffer == pricingContext.CounterBidOffer
                                                                                        && w.CounterBidLimit == pricingContext.CounterBidLimit
                                                                                        && w.BpCpJumb == pricingContext.BpCpJumb).ToList();
                                                                                    }
                                                                                    else if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                                                    {
                                                                                        publishedPrice = PricingData.Where(w => w.BiddingWindowId == inputDto.BiddingWindowId
                                                                                        && w.SkuId == pricingContext.SkuId
                                                                                        && w.OilTypeId == pricingContext.OilTypeId
                                                                                        && w.SaudaBookingTypeId == pricingContext.SaudaBookingTypeId
                                                                                        && w.OilPackingTypeId == pricingContext.OilPackingTypeId
                                                                                        && w.StateId == pricingContext.StateId
                                                                                        && w.CityId == pricingContext.CityId
                                                                                        && w.TransportModeId == pricingContext.TransportModeId
                                                                                        && w.PlantId == pricingContext.PlantId
                                                                                        && w.DepotId == pricingContext.DepotId
                                                                                        && w.FrieghtZoneId == pricingContext.FrieghtZoneId
                                                                                        && w.FrieghtRouteId == pricingContext.FrieghtRouteId
                                                                                        && w.BiddingWindowId == pricingContext.BiddingWindowId
                                                                                        && w.MaterialCost == pricingContext.MaterialCost
                                                                                        && w.PackingCost == pricingContext.PackingCost
                                                                                        && w.PrimaryFrieght == pricingContext.PrimaryFrieght
                                                                                        && w.SecondaryFrieght == pricingContext.SecondaryFrieght
                                                                                        && w.DepotCost == pricingContext.DepotCost
                                                                                        && w.DetentionCost == pricingContext.DetentionCost
                                                                                        && w.HoneycombCost == pricingContext.HoneycombCost
                                                                                        && w.Margin == pricingContext.Margin
                                                                                        && w.CushionMargin == pricingContext.CushionMargin
                                                                                        && w.SchemeCostRecovery == pricingContext.SchemeCostRecovery
                                                                                        && w.Discount == pricingContext.Discount
                                                                                        && w.Premium == pricingContext.Premium
                                                                                        && w.ProcessCost == pricingContext.ProcessCost
                                                                                        && w.SumOfIngredientCost == pricingContext.SumOfIngredientCost
                                                                                        && w.TpPrice == pricingContext.TpPrice
                                                                                        && w.RaMargin == pricingContext.RaMargin
                                                                                        && w.BaseRate == pricingContext.BaseRate
                                                                                        && w.XMargin == pricingContext.XMargin
                                                                                        && w.FinalRate == pricingContext.FinalRate
                                                                                        && w.ExPlantPrice == pricingContext.ExPlantPrice
                                                                                        && w.ForDepotPrice == pricingContext.ForDepotPrice
                                                                                        && w.ForPlantPrice == pricingContext.ForPlantPrice
                                                                                        && w.ExDepotPrice == pricingContext.ExDepotPrice
                                                                                        && w.ExRakePrice == pricingContext.ExRakePrice
                                                                                        && w.ForRakePrice == pricingContext.ForRakePrice
                                                                                        && w.ClearanceRate == pricingContext.ClearanceRate
                                                                                        && w.CounterBidOffer == pricingContext.CounterBidOffer
                                                                                        && w.CounterBidLimit == pricingContext.CounterBidLimit
                                                                                        && w.BpCpJumb == pricingContext.BpCpJumb).ToList();
                                                                                    }

                                                                                    if (publishedPrice != null && publishedPrice.Any())
                                                                                    {
                                                                                        var finalDataTitleMissingErrorMessage = dataTitleMissingErrorMessage + " Price Already Generated " + "|";
                                                                                        errorMessageList.Add(finalDataTitleMissingErrorMessage);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        pricingContext.IsPublish = false;
                                                                                        pricingContext.PublishId = pricePublishContext.Id;
                                                                                        pricings.Add(pricingContext);
                                                                                        count++;
                                                                                        isAvailable = true;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    pricingContext.IsPublish = false;
                                                                                    pricingContext.PublishId = pricePublishContext.Id;
                                                                                    pricings.Add(pricingContext);
                                                                                    count++;
                                                                                    isAvailable = true;
                                                                                }
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
                                                        }
                                                    }
                                                }
                                                //_logger.Info("Completed :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                                            }
                                            else
                                            {
                                                valErrorMessage = valErrorMessage + "|";
                                                errorMessageList.Add(valErrorMessage.TrimAndReduce());
                                            }
                                        }

                                        //if (isAvailable)
                                        //{
                                        //    _context.BulkInsertProxy(pricings);
                                        //    _context.SaveChanges();
                                        //    pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Completed;
                                        //}
                                        //else
                                        //{
                                        //    pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Failed;
                                        //}
                                        //if (errorMessageList != null && errorMessageList.Any())
                                        //{
                                        //    pricePublishContext.ErrorMessage = string.Join("", errorMessageList);
                                        //}
                                        //pricePublishContext.EndDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        //_context.SaveChanges();
                                        //if (isAvailable)
                                        //{
                                        //    smsContent = Constants.PriceCalculationCompleted.Replace(Constants.Count, count.ToString()).Replace(Constants.StartTime, pricePublishContext.StartDate.ToString("hh:mm tt"))
                                        //        .Replace(Constants.EndTime, pricePublishContext.EndDate.ToString("hh:mm tt"));
                                        //}
                                        //else
                                        //{
                                        //    smsContent = Constants.PriceCalculationFailed;
                                        //}

                                    }
                                    else { _logger.Error("TransportModes is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                                }
                                else { _logger.Error("FreightRouteIds is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                            }
                            else { _logger.Error("FreightZoneId is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                        }
                        else { _logger.Error("DepotIds is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                    }
                    else { _logger.Error("SKU is empty : " + DateHelper.UtcToIndia(DateTime.UtcNow)); }

                    if (isAvailable)
                    {
                        _context.BulkInsertProxy(pricings);
                        _context.SaveChanges();
                        pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Completed;
                    }
                    else
                    {
                        pricePublishContext.StatusId = (long)DTO.Enums.PublishStatus.Failed;
                    }
                    if (errorMessageList != null && errorMessageList.Any())
                    {
                        pricePublishContext.ErrorMessage = string.Join("", errorMessageList);
                    }
                    pricePublishContext.EndDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _context.SaveChanges();

                    if (isAvailable)
                    {
                        smsContent = Constants.PriceCalculationCompleted.Replace(Constants.Count, count.ToString()).Replace(Constants.StartTime, pricePublishContext.StartDate.ToString("hh:mm tt"))
                            .Replace(Constants.EndTime, pricePublishContext.EndDate.ToString("hh:mm tt"));
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
                    if (pricePublishContext != null && (pricePublishContext.StatusId == (int)DTO.Enums.PublishStatus.Started || pricePublishContext.StatusId == (int)DTO.Enums.PublishStatus.Failed))
                    {
                        pricePublishContext.StatusId = (int)DTO.Enums.PublishStatus.Failed;
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
        */
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

        #endregion

        #region FinalPrice Publish Background

        public void FinalPricePublishBackgroundQueue(List<int> publishStateIds, long verticalId)
        {
            _methodName = "FinalPricePublishBackgroundQueue";
            if (publishStateIds != null && publishStateIds.Any())
            {
                using (var _context = new AdaniContext())
                {
                    try
                    {

                        var usersRoleIds = _context.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader || _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.UserId);
                        if (usersRoleIds != null && usersRoleIds.Any())
                        {
                            var usersContext = _context.Users.AsNoTracking().Where(_ => usersRoleIds.Contains(_.Id) && _.IsActive && publishStateIds.Contains(_.StateId)
                                //&& _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                                //&& _.DivisionId == verticalId
                                ).ToList();

                            var bdoContext = _context.Users.AsNoTracking()
                                .Join(_context.UserRoles.AsNoTracking(), U => U.Id, UR => UR.UserId, (U, UR) => new { U, UR })
                                .Where(_ => usersRoleIds.Contains(_.U.Id) && _.U.IsActive && publishStateIds.Contains(_.U.StateId)
                                //&& _.U.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                                && _.UR.RoleId == (int)DTO.Enums.Role.StateTrader
                                //&& _.U.DivisionId == verticalId
                                ).ToList();

                            List<string> toUsers = new List<string>();
                            List<long> toUsersIds = new List<long>();
                            bool isSms = true;
                            bool isEmail = true;
                            var DealerNotificationContext = _context.TPNotification.AsNoTracking()
                                                       .Join(_context.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                       .Where(_ => toUsersIds.Contains(_.TPND.DealerId) && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.PriceRelease && _.TPND.IsActive)
                                                       .ToList();
                            if (usersContext != null && usersContext.Any())
                            {
                                toUsersIds = usersContext.Select(_ => _.Id).ToList();
                                toUsers = DealerNotificationContext.Join(usersContext, TPN => TPN.TPND.DealerId, U => U.Id, (TPN, U) => new { TPN, U }).Where(_ => _.TPN.TPN.Email).Select(_ => _.U.Email).ToList();
                            }
                            if (bdoContext != null && bdoContext.Any())
                            {
                                var bdosemail = bdoContext.Where(_ => _.U.Email != null && _.U.Email != "").Select(_ => _.U.Email).ToList();
                                toUsers.AddRange(bdosemail);
                            }
                            if (usersContext != null && usersContext.Any())
                            {
                                //toUsers = usersContext.Where(_ => _.Email != null && _.Email != "").Select(_ => _.Email).ToList();

                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var emailSubject = Constants.FinalPricePublishSubject;
                                    var plainText = string.Empty;
                                    var emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalPricePublishNotificationEmail);
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate;
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                                var smsMessage = string.Empty;
                                if (isSms)
                                {
                                    //toUsers = usersContext.Where(_ => _.MobileNumber != null && _.MobileNumber != "").Select(_ => _.MobileNumber).ToList();
                                    toUsers = DealerNotificationContext.Join(usersContext, TPN => TPN.TPND.DealerId, U => U.Id, (TPN, U) => new { TPN, U }).Where(_ => _.TPN.TPN.SMS).Select(_ => _.U.MobileNumber).ToList();
                                    var bdoMobileNumber = bdoContext.Where(_ => _.U.MobileNumber != null && _.U.MobileNumber != "").Select(_ => _.U.MobileNumber).ToList();
                                    toUsers.AddRange(bdoMobileNumber);
                                    if (toUsers != null && toUsers.Any())
                                    {
                                        var smsPlainTemplate = string.Empty;
                                        EmailTemplate smsTemplate = new EmailTemplate();
                                        smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.FinalPricePublishNotificationSMS);
                                        if (smsTemplate != null)
                                        {

                                            smsPlainTemplate = smsTemplate.PlainTemplate;
                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            foreach (var mobileNo in toUsers)
                                            {

                                                amazonNotificationService.SendMessage(smsMessage, mobileNo);
                                            }
                                        }
                                    }
                                }
                                bool isPushNotification = false;
                                var IsPushNotificationContext = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsPushNotification).Select(_ => _.Value).Single();
                                if (IsPushNotificationContext.Equals("1") || IsPushNotificationContext.Equals("True"))
                                    isPushNotification = true;
                                if (isPushNotification)
                                {
                                    var pushNotificationUsers = DealerNotificationContext.Join(usersContext, TPN => TPN.TPND.DealerId, U => U.Id, (TPN, U) => new { TPN, U }).Where(_ => _.TPN.TPN.InAppNotification).Select(_ => _.U).ToList();
                                    var bdoPushNotification = bdoContext.Select(_ => _.U).ToList();
                                    pushNotificationUsers.AddRange(bdoPushNotification);
                                    foreach (var user in pushNotificationUsers)
                                    {
                                        if (user != null && user.RegistrationTypeId != null && user.RegistrationTypeId > 0 && !string.IsNullOrEmpty(user.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = user.PushTokenKey,
                                                RegistrationTypeId = (int)user.RegistrationTypeId,
                                                Title = Constants.FinalPricePublishSubject,
                                                Message = smsMessage,
                                                Id = "00"
                                            };
                                            SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                        _logger.Error(message);
                    }
                    ResultDto SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
                    {
                        _methodName = "SendPushNotificationThroughFirebase";
                        var resultDto = new ResultDto();
                        try
                        {
                            _logger.Info($"{ServiceName} Service-Method {_methodName}");
                            if (pushNotificationInputDto == null)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                                return resultDto;
                            }
                            if (string.IsNullOrEmpty(pushNotificationInputDto.Title))
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = Constants.PushNotificationTitleMissing;
                                return resultDto;
                            }
                            if (string.IsNullOrEmpty(pushNotificationInputDto.Message))
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = Constants.PushNotifcationMessageMissing;
                                return resultDto;
                            }
                            if (string.IsNullOrEmpty(pushNotificationInputDto.PushTokenKey))
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = Constants.PushTokenEmpty;
                                return resultDto;
                            }
                            if (pushNotificationInputDto.RegistrationTypeId == 0)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                                return resultDto;
                            }

                            var firebaseSenderId = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                            var pushNotifyServerkey = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                            var pushNotifyUrl = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

                            if (string.IsNullOrEmpty(pushNotifyServerkey) || string.IsNullOrEmpty(firebaseSenderId) || string.IsNullOrEmpty(pushNotifyUrl))
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                                return resultDto;
                            }

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
                                        IsLogOut = pushNotificationInputDto.IsLogOut
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
                                        IsLogOut = pushNotificationInputDto.IsLogOut
                                    },
                                    notification = new
                                    {
                                        title = pushNotificationInputDto.Title,
                                        body = pushNotificationInputDto.Message,
                                        id = pushNotificationInputDto.Id,
                                        sound = "default",
                                        IsLogOut = pushNotificationInputDto.IsLogOut
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
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.ErrorCode = Constants.Exception;
                            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                            _logger.Error(message);

                        }
                        return resultDto;
                    }
                }
            }
        }

        #endregion

        #region Publish Backup

        public ResultDto GetPublishedPriceErrorDetails(PricePublishInputDto inputDto)
        {
            _methodName = "GetPublishedPriceErrorDetails";
            try
            {
                var pricePublishedList = new List<PricePublishesDto>();

                var pricePublishedData = _emamiContext.PricePublish.AsNoTracking()
                    .FirstOrDefault(w => w.Id == inputDto.Id)?.ErrorMessage;

                if (pricePublishedData != null && pricePublishedData.Any())
                {
                    pricePublishedList.Add(new PricePublishesDto() { ErrorMessage = pricePublishedData });
                }
                return _resultService.SuccessMessageWitObject(pricePublishedList, "Success");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto PricingDataBackup(LoginUserIdDto inputDto)
        {
            var resultDto = new ResultDto();
            try
            {
                //Task.Run(() => PricingBackup());
                System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => PricingBackup(cancellationToken));
                resultDto = _resultService.SuccessMessage("success");
            }
            catch (Exception)
            {
                resultDto = _resultService.ErrorMessage("fail");
            }
            return resultDto;
        }

        public void PricingBackup(CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {

            var message = $"---------------------------------------- Price Data Backup Started ----------------------------------------";
            _logger.Info(message);
            _methodName = "PricingBackup";
            message = $"{ServiceName} Service-Method {_methodName} Process Start Date Time : " + DateHelper.UtcToIndia(DateTime.UtcNow);
            _logger.Info(message);

            using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
            {
                connection.Open();
                SqlTransaction sqlTransaction = connection.BeginTransaction();
                try
                {
                    string SP_Name = "SetPricingsDataBackup";
                    SqlCommand cmd = new SqlCommand(SP_Name, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Transaction = sqlTransaction;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    message = $"{ServiceName} Service-Method {_methodName} Process End Date Time : " + DateHelper.UtcToIndia(DateTime.UtcNow);
                    _logger.Info(message);
                    message = $"---------------------------------------- Price Data Backup Completed ----------------------------------------";
                    _logger.Info(message);
                }
                catch (Exception exception)
                {
                    sqlTransaction.Rollback();
                    message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
        }

        #endregion

        #region New FinalPrice - State Based


        public ResultDto GetPriceGenerates(PricePublishInputDto inputDto)
        {
            _methodName = "GetPriceGenerates";
            var priceGenerateList = new List<FinalPriceGenerateOutputDto>();
            try
            {
                if (inputDto.RoleId == (long)DTO.Enums.Role.Admin)
                {
                    priceGenerateList = _emamiContext.PriceGenerate.AsNoTracking()
                    .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                    .Select(s => new FinalPriceGenerateOutputDto
                    {
                        Id = s.Id,
                        PricingDate = s.CreatedDate,
                        SaudaBookingTypeId = s.SaudaBookingTypeId,
                        SaudaBookingType = s.SaudaBookingType.Name,
                        Vertical = s.Vertical.Name,
                        TotalState = s.PriceGenerateDetail.Count
                    }).ToList();
                }
                else if (inputDto.RoleId == (long)DTO.Enums.Role.BusinessFinanceAdmin)
                {
                    priceGenerateList = _emamiContext.PriceGenerate.AsNoTracking()
                    .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId && w.VerticalId == inputDto.VerticalId)
                    .Select(s => new FinalPriceGenerateOutputDto
                    {
                        Id = s.Id,
                        PricingDate = s.CreatedDate,
                        SaudaBookingTypeId = s.SaudaBookingTypeId,
                        SaudaBookingType = s.SaudaBookingType.Name,
                        Vertical = s.Vertical.Name,
                        TotalState = s.PriceGenerateDetail.Count
                    }).ToList();
                }
                else
                {
                    priceGenerateList = _emamiContext.PriceGenerate.AsNoTracking()
                    .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId && w.CreatedBy == inputDto.LoginUserId)
                    .Select(s => new FinalPriceGenerateOutputDto
                    {
                        Id = s.Id,
                        PricingDate = s.CreatedDate,
                        SaudaBookingTypeId = s.SaudaBookingTypeId,
                        SaudaBookingType = s.SaudaBookingType.Name,
                        Vertical = s.Vertical.Name,
                        TotalState = s.PriceGenerateDetail.Count
                    }).ToList();
                }

                if (priceGenerateList != null && priceGenerateList.Any())
                {
                    priceGenerateList.ForEach(f =>
                    {

                        var priceGenerateDetail = _emamiContext.PriceGenerateDetail.AsNoTracking()
                        .Where(w => w.PriceGenerateId == f.Id).Select(s => new { IsPublish = s.IsPublish, StatusId = s.StatusId, TaskStatusId = s.TaskStatusId });

                        if (priceGenerateDetail != null && priceGenerateDetail.Any())
                        {
                            var isProcessing = priceGenerateDetail.Any(a => a.TaskStatusId == (int)DTO.Enums.FinalPriceTaskStatus.Created);
                            if (isProcessing)
                            {
                                f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.PriceGenerating;
                            }
                            else
                            {
                                if (priceGenerateDetail.Any(a => a.IsPublish == true))
                                {
                                    f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.Published;
                                }
                                else if (priceGenerateDetail.All(a => a.StatusId == (int)DTO.Enums.PricePublishStatus.Failed))
                                {
                                    f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.PriceGenerateFailed;
                                }
                                else if (priceGenerateDetail.Any(a => a.StatusId == (int)DTO.Enums.PricePublishStatus.Completed
                                 || a.StatusId == (int)DTO.Enums.PricePublishStatus.CompletedWithError))
                                {
                                    f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.Publish;
                                }
                            }
                        }
                        else
                        {
                            f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.PriceGenerateFailed;
                        }

                    });
                }

                return _resultService.SuccessMessageWitObject(priceGenerateList, Constants.SuccessMessage);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        

        public ResultDto GetPriceGenerateDetails(PricePublishInputDto inputDto)
        {
            _methodName = "GetPriceGenerateDetails";
            var pricingList = new List<FinalPriceGenerateDetailOutputDto>();
            var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            try
            {
                var priceGenerateList = _emamiContext.PriceGenerateDetail.AsNoTracking()
                    .Where(w => w.PriceGenerateId == inputDto.Id)
                    .Select(s => new
                    {
                        Id = s.Id,
                        OilTypeId = s.OilTypeId,
                        PackGroupId = s.PackGroupId,
                        PlantName = s.Plant.Name,
                        ZoneId = s.ZoneId,
                        StateId = s.StateId,
                        StatusId = s.StatusId,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        PublishDate = s.ModifiedDate,
                        IsPublish = s.IsPublish,
                        Message = s.ErrorMessage,
                        ErrorMessageCount = s.ErrorMessageCount,
                        TaskStatusId = s.TaskStatusId
                    }).ToList();

                if (priceGenerateList != null && priceGenerateList.Any())
                {
                    var oilTypeIds = priceGenerateList.FirstOrDefault().OilTypeId.ToString().Split(',').Select(s => Int64.Parse(s)).ToList();
                    var oilPackingTypeIds = priceGenerateList.FirstOrDefault().PackGroupId.ToString().Split(',').Select(s => Int64.Parse(s)).ToList();
                    var stateIds = priceGenerateList.Select(s => s.StateId).ToList();

                    #region Price Generate Parameters
                    var oilTypeName = _emamiContext.OilTypes.AsNoTracking().Where(w => oilTypeIds.Contains(w.Id))
                        .Select(s => new { OilName = s.Name }).ToList();
                    var oilPackType = _emamiContext.OilPackingTypes.AsNoTracking().Where(w => oilPackingTypeIds.Contains(w.Id))
                        .Select(s => new { OilPackName = s.Name }).ToList();
                    var zoneStateData = _emamiContext.ZoneStateMappings.AsNoTracking().Where(w => stateIds.Contains(w.StateId)).Select(s => new { StateId = s.StateId, ZoneName = s.Zone.Name, StateName = s.State.StateName }).ToList();
                    #endregion


                    if (inputDto.SearchDate.Date == todayDate.Date)
                    {
                        foreach (var price in priceGenerateList)
                        {
                            int recordCount = _emamiContext.TodayPricing.AsNoTracking()
                                //.Count(c => c.PublishId == price.Id);
                                .Count();
                            var state = zoneStateData.FirstOrDefault(f => f.StateId == price.StateId);
                            var status = Utility.GetEnumFromString<DTO.Enums.PricePublishStatus>(price.StatusId);
                            pricingList.Add(new FinalPriceGenerateDetailOutputDto()
                            {
                                Id = price.Id,
                                OilType = string.Join(",", oilTypeName.Select(s => s.OilName)),
                                PackGroup = string.Join(",", oilPackType.Select(s => s.OilPackName)),
                                PlantName = price.PlantName,
                                ZoneName = state.ZoneName,
                                StateName = state.StateName,
                                StatusId = price.StatusId,
                                Status = status,
                                // TotalPriceCount = _emamiContext.TodayPricing.AsNoTracking().Count(c => c.PublishId == price.Id),
                                StartDate = price.StartDate,
                                EndDate = price.EndDate,
                                PublishDate = price.PublishDate,
                                IsPublish = price.IsPublish,
                                ErrorMessageCount = price.ErrorMessageCount,
                                TaskStatusId = price.TaskStatusId,
                                TaskStatus = Utility.GetEnumFromString<DTO.Enums.FinalPriceTaskStatus>(price.TaskStatusId)
                            });
                        }
                    }
                    else if (inputDto.SearchDate.Date < todayDate.Date)
                    {
                        foreach (var price in priceGenerateList)
                        {
                            int recordCount = _emamiContext.PricingBackup.AsNoTracking().Count(c => c.PublishId == price.Id);
                            var state = zoneStateData.FirstOrDefault(f => f.StateId == price.StateId);
                            var status = Utility.GetEnumFromString<DTO.Enums.PricePublishStatus>(price.StatusId);
                            pricingList.Add(new FinalPriceGenerateDetailOutputDto()
                            {
                                Id = price.Id,
                                OilType = string.Join(",", oilTypeName.Select(s => s.OilName)),
                                PackGroup = string.Join(",", oilPackType.Select(s => s.OilPackName)),
                                PlantName = price.PlantName,
                                ZoneName = state.ZoneName,
                                StateName = state.StateName,
                                StatusId = price.StatusId,
                                Status = status,
                                TotalPriceCount = _emamiContext.PricingBackup.AsNoTracking().Count(c => c.PublishId == price.Id),
                                StartDate = price.StartDate,
                                EndDate = price.EndDate,
                                PublishDate = price.PublishDate,
                                IsPublish = price.IsPublish,
                                ErrorMessageCount = price.ErrorMessageCount,
                                TaskStatusId = price.TaskStatusId,
                                TaskStatus = Utility.GetEnumFromString<DTO.Enums.FinalPriceTaskStatus>(price.TaskStatusId)
                            });
                        }
                    }

                }

                return _resultService.SuccessMessageWitObject(pricingList, Constants.SuccessMessage);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto StateBasePublishFinalPrice(FinalPricePublishDto inputDto)
        {
            _methodName = "StateBasePublishFinalPrice";
            var resultDto = new ResultDto();
            try
            {
                using (var _context = new AdaniContext())
                {
                    _context.Database.CommandTimeout = 0;
                    if (inputDto == null)
                    {
                        return _resultService.ErrorMessage(Constants.InvalidRequest);
                    }

                    if (inputDto.LoginUserId == 0)
                    {
                        return _resultService.ErrorMessage(Constants.UserIdMissing);
                    }

                    if (!(inputDto.PublishId > 0))
                    {
                        return _resultService.ErrorMessage(Constants.PublishIdMissing);
                    }

                    var userContext = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                    if (userContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.UserNotFound);
                    }

                    //PricePublish pricePublishContext = _emamiContext.PricePublish.FirstOrDefault(_ => _.Id == inputDto.PublishId);
                    //if (pricePublishContext != null)
                    //{

                    //var pricingsContext = _emamiContext.Pricing.AsNoTracking()
                    //    .Where(_ => _.PublishId == inputDto.PublishId).
                    //    Select(s => s);

                    var pricingsContext = _context.Pricing.AsNoTracking()
                        //.Where(_ => _.PublishId == inputDto.PublishId).
                        .Select(s => new
                        {
                            //MaterialCostId = s.MaterialCostId,
                            //PackingCostId = s.PackingCostId,
                            //DepotCostId = s.DepotCostId,
                            //DetentionCostId = s.DetentionCostId,
                            //ProfitMarginId = s.ProfitMarginId,
                            //CushionMarginId = s.CushionMarginId,
                            //SchemeCostId = s.SchemeCostId,
                            //PrimaryFrieghtId = s.PrimaryFrieghtId,
                            //SecondaryFrieghtForPlantId = s.SecondaryFrieghtForPlantId,
                            //HoneycombCostId = s.HoneycombCostId,
                            //RaMarginId = s.RaMarginId,
                            //LoadCapacityId = s.LoadCapacityId,
                            //IngredientCostId = s.IngredientCostId,
                            //SkuIngrediantPlantId = s.SkuIngrediantPlantId,
                            //StateId = s.StateId,
                            //SecondaryFrieghtId = s.SecondaryFrieghtId
                        }).ToList();

                    var pricingIds = new List<long>();
                    var ingredientIds = new List<string>();
                    List<string> updateQuery = new List<string>();
                    string bookingType = inputDto.BookingTypeId == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess ? DTO.Enums.SaudaBookingTypes.TraditionalProcess.ToString()
                        //: (inputDto.BookingTypeId == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction ? DTO.Enums.SaudaBookingTypes.ReverseAuction.ToString() 
                        : "";

                    if (pricingsContext != null && pricingsContext.Any())
                    {
                        //pricingIds = pricingsContext.Where(w => w.MaterialCostId != 0).Select(s => s.MaterialCostId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update MaterialCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.PackingCostId != 0).Select(s => s.PackingCostId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update PackingCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.DepotCostId != 0).Select(s => s.DepotCostId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update DepotCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.DetentionCostId != 0).Select(s => s.DetentionCostId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update DetentionCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.ProfitMarginId != 0).Select(s => s.ProfitMarginId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update ProfitMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.CushionMarginId != 0).Select(s => s.CushionMarginId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update CushionMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.SchemeCostId != 0).Select(s => s.SchemeCostId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update SchemeCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.PrimaryFrieghtId != 0).Select(s => s.PrimaryFrieghtId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update PrimaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.SecondaryFrieghtId != 0).Select(s => s.SecondaryFrieghtId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update SecondaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.SecondaryFrieghtForPlantId != 0).Select(s => s.SecondaryFrieghtForPlantId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update SecondaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.HoneycombCostId != 0).Select(s => s.HoneycombCostId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update HoneycombCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.RaMarginId != 0).Select(s => s.RaMarginId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update RaMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.LoadCapacityId != 0).Select(s => s.LoadCapacityId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update LoadCapacityConversions Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //ingredientIds = pricingsContext.Where(w => !string.IsNullOrEmpty(w.IngredientCostId)).Select(s => s.IngredientCostId).Distinct().ToList();
                        //if (ingredientIds != null && ingredientIds.Any())
                        //{
                        //    var ingredientId = string.Join(",", ingredientIds).Split(',').Distinct().ToList();
                        //    updateQuery.Add("Update IngredientCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", ingredientId) + ");");
                        //}

                        //pricingIds = pricingsContext.Where(w => w.SkuIngrediantPlantId != 0).Select(s => s.SkuIngrediantPlantId).Distinct().ToList();
                        //if (pricingIds != null && pricingIds.Any())
                        //{
                        //    updateQuery.Add("Update SkuIngrediantPlants Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        //}

                        //using (SqlConnection conn = new SqlConnection(Config.DBConnectionString))
                        //{
                        //    conn.Open();
                        //    SqlCommand command;
                        //    SqlTransaction sqlTransaction = conn.BeginTransaction();
                        //    var startedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        //    try
                        //    {
                        //        string updatePricings = "Update Pricings set IsPublish = @isPublish, BiddingDate = @biddingDate, ModifiedBy = @modifiedBy, ModifiedDate = @modifiedDate Where PublishId = @publishIds";
                        //        string tblQuery = string.Join("", updateQuery ?? new List<string>());

                        //        command = new SqlCommand(updatePricings, conn);
                        //        command.Parameters.AddWithValue("@isPublish", true);
                        //        command.Parameters.AddWithValue("@biddingDate", inputDto.BiddingDate);
                        //        command.Parameters.AddWithValue("@modifiedBy", inputDto.LoginUserId);
                        //        command.Parameters.AddWithValue("@modifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                        //        command.Parameters.AddWithValue("@publishIds", inputDto.PublishId);
                        //        command.Transaction = sqlTransaction;
                        //        command.ExecuteNonQuery();
                        //        command.Parameters.Clear();
                        //        _logger.Info($"{bookingType} Pricings Table Updated - Date Time : {startedDate}");

                        //        string updatePricePublishes = "Update PriceGenerateDetails set IsPublish = @isPublish, ModifiedBy = @modifiedBy, ModifiedDate = @modifiedDate Where Id = @publishIds";
                        //        command = conn.CreateCommand();
                        //        command.CommandText = updatePricePublishes;
                        //        command.Parameters.AddWithValue("@isPublish", true);
                        //        command.Parameters.AddWithValue("@modifiedBy", inputDto.LoginUserId);
                        //        command.Parameters.AddWithValue("@modifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                        //        command.Parameters.AddWithValue("@publishIds", inputDto.PublishId);
                        //        command.Transaction = sqlTransaction;
                        //        command.ExecuteNonQuery();
                        //        command.Parameters.Clear();
                        //        _logger.Info($"{bookingType} - PricePublishes Table Updated - Date Time : {startedDate}");

                        //        if (!string.IsNullOrEmpty(tblQuery))
                        //        {
                        //            command = conn.CreateCommand();
                        //            command.CommandText = tblQuery;
                        //            command.Parameters.AddWithValue("@isPublish", true);
                        //            command.Transaction = sqlTransaction;
                        //            command.ExecuteNonQuery();
                        //            command.Parameters.Clear();
                        //            _logger.Info($"{bookingType} - All Pricings Table Updated - Date Time : {startedDate}");
                        //        }
                        //        sqlTransaction.Commit();
                        //    }
                        //    catch (Exception exception)
                        //    {
                        //        sqlTransaction.Rollback();
                        //        _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {exception}");
                        //        return _resultService.ErrorMessage(Constants.Exception);
                        //    }
                        //    finally
                        //    {
                        //        conn.Close();
                        //    }
                        //}
                        //var publishStateIds = pricingsContext.Select(_ => _.StateId).Distinct().ToList();
                    }
                }


                //HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => _notificationService.PricePublishNotificationAsync(pricingMailDto, cancellationToken));

                return _resultService.SuccessMessage(Constants.PriceDetailsPublishedSuccessfully);
                //}
                //else
                //{
                //    return _resultService.SuccessMessage(Constants.RecordNotFound);
                //}
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetStateBaseFinalPriceList(FinalPricePublishDto inputDto)
        {
            _methodName = "GetStateBaseFinalPriceList";
            var pricingListDto = new List<PricingDto>();
            try
            {
                using (var _context = new AdaniContext())
                {
                    _context.Database.CommandTimeout = 240;
                    if (inputDto == null)
                    {
                        return _resultService.ErrorMessage(Constants.InvalidRequest);
                    }
                    if (inputDto.LoginUserId == 0)
                    {
                        return _resultService.ErrorMessage(Constants.UserIdMissing);
                    }
                    var userContext = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                    if (userContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.UserNotFound);
                    }
                    int SkipCount = inputDto.SkipCount;
                    var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                    //for current date data is taken from TodayPrcings table
                    if (inputDto.SearchDate.Date == todayDate.Date)
                    {
                        var pricingContext = _context.TodayPricing.AsNoTracking()
                        .Join(_context.Depots.AsNoTracking().Where(_ => _.IsPlant == true), x => x.PlantId, pnt => pnt.Id, (x, pnt) => new { x, /*x.StateName,*/ PlantName = pnt.Name });

                        var skuDatas = _emamiContext.Skus.AsNoTracking().Select(s => new { Id = s.Id, Name = s.SkuName }).ToList();
                        var oilTypeDatas = _emamiContext.OilTypes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();
                        var oilPackingDatas = _emamiContext.OilPackingTypes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();
                        var saudaBookingTypeDatas = _emamiContext.SaudaBookingTypes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();
                        var transportModesDatas = _emamiContext.TransportModes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();

                        if (pricingContext != null && pricingContext.Any())
                        {
                            pricingListDto = pricingContext.Select(_ => new PricingDto()
                            {
                                Id = _.x.Id,
                                SkuName = skuDatas.FirstOrDefault(f => f.Id == _.x.SkuId).Name,
                                SkuId = _.x.SkuId,
                                Plant = _.PlantName,
                            }).ToList();
                        }

                        if (pricingListDto != null && pricingListDto.Any())
                        {
                            pricingListDto.ForEach(f =>
                            {
                                f.SkuName = skuDatas.FirstOrDefault(fd => fd.Id == f.SkuId).Name;
                                f.OilTypeName = oilTypeDatas.FirstOrDefault(fd => fd.Id == f.OilTypeId).Name;
                                f.OilPackingType = oilPackingDatas.FirstOrDefault(fd => fd.Id == f.OilPackTypeId).Name;

                                if (f.BiddingWindowId > 0)
                                {
                                    var biddingWindow = _context.BiddingWindowTiming.AsNoTracking().FirstOrDefault(w => w.Id == f.BiddingWindowId);
                                    f.BiddingWindowTiming = biddingWindow.FromHours.ToString() + " - " + biddingWindow.ToHours.ToString();
                                }
                            });

                            return _resultService.SuccessObject(pricingListDto);
                        }
                        else
                        {
                            return _resultService.ErrorMessage(Constants.RecordNotFound);
                        }
                    }
                    //for previous date records taken from Pricing Backup
                    else if (inputDto.SearchDate.Date < todayDate.Date)
                    {
                        var pricingContext = _context.PricingBackup.AsNoTracking()
                        .Where(_ => _.PublishId == inputDto.PublishId)
                        //.Join(_context.State.AsNoTracking(), p => p.StateId, s => s.Id, (p, s) => new { p, StateName = s.StateName })
                        .Join(_context.Depots.AsNoTracking().Where(_ => _.IsPlant == true), x => x.PlantId, pnt => pnt.Id, (x, pnt) => new { x, /* x.StateName,*/ PlantName = pnt.Name })
                        .Join(_context.Depots.AsNoTracking().Where(_ => _.IsPlant == false), x => x.x.DepotId, dpt => dpt.Id, (x, dpt) => new { p = x.x, /*x.p, x.StateName,*/ x.PlantName, DepotName = dpt.Name })
                        //.Join(_context.FreightZones.AsNoTracking(), x => x.p.FrieghtZoneId, fz => fz.Id, (x, fz) => new { x.p, x.StateName, x.PlantName, x.DepotName, FreightZoneName = fz.Name })
                        //.Join(_context.FreightRoutes.AsNoTracking(), x => x.p.FrieghtRouteId, fr => fr.Id, (x, fr) => new { x.p, x.StateName, x.PlantName, x.DepotName, x.FreightZoneName, FreightRouteName = fr.Name })
                        //.Join(_context.PriceGenerateDetail.AsNoTracking(), x => x.x, pp => pp.Id, (x, pp) => new { x, /*x.p, x.StateName,*/ x.PlantName, x.DepotName, PublishPrice = pp }
                        //);
                        .OrderBy(_ => _.p.Id).Skip(SkipCount).Take(50000);

                        var skuDatas = _emamiContext.Skus.AsNoTracking().Select(s => new { Id = s.Id, Name = s.SkuName }).ToList();
                        var oilTypeDatas = _emamiContext.OilTypes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();
                        var oilPackingDatas = _emamiContext.OilPackingTypes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();
                        var saudaBookingTypeDatas = _emamiContext.SaudaBookingTypes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();
                        var transportModesDatas = _emamiContext.TransportModes.AsNoTracking().Select(s => new { Id = s.Id, Name = s.Name }).ToList();

                        if (pricingContext != null && pricingContext.Any())
                        {
                            pricingListDto = pricingContext.Select(_ => new PricingDto()
                            {
                                Id = _.p.Id,
                                //SkuName = skuDatas.FirstOrDefault(f => f.Id == _.p.SkuId).Name,
                                //OilTypeName = oilTypeDatas.FirstOrDefault(f => f.Id == _.p.OilTypeId).Name,
                                //SaudaBookingType = saudaBookingTypeDatas.FirstOrDefault(f => f.Id == _.p.SaudaBookingTypeId).Name,// _.p.SaudaBookingType.Name,
                                //TransportMode = transportModesDatas.FirstOrDefault(f => f.Id == _.p.TransportModeId).Name,// _.p.TransportMode.Name,
                                //OilPackingType = oilPackingDatas.FirstOrDefault(f => f.Id == _.p.OilPackingTypeId).Name,
                                SkuId = _.p.SkuId,
                                //OilTypeId = _.p.OilTypeId,
                                //OilPackTypeId = _.p.OilPackingTypeId,
                                //TransPortModeId = _.p.TransportModeId,
                                //SaudaBookingTypeId = _.p.SaudaBookingTypeId,
                                //State = _.StateName,
                                Plant = _.PlantName,
                                //Depot = _.DepotName,
                                //FrieghtZone = _.FreightZoneName,
                                //FrieghtRoute = _.FreightRouteName,
                                //BiddingDate = _.p.BiddingDate,
                                //MaterialCost = _.p.MaterialCost,
                                //PackingCost = _.p.PackingCost,
                                //PrimaryFrieght = _.p.PrimaryFrieght,
                                //SecondaryFrieght = _.p.SecondaryFrieght,
                                //PlantSecondaryFrieght = _.p.PlantSecondaryFrieght,
                                //DepotCost = _.p.DepotCost,
                                //DetentionCost = _.p.DetentionCost,
                                //HoneycombCost = _.p.HoneycombCost,
                                //Margin = _.p.Margin,
                                //CushionMargin = _.p.CushionMargin,
                                //SchemeCostRecovery = _.p.SchemeCostRecovery,
                                //Discount = _.p.Discount,
                                //Premium = _.p.Premium,
                                //ProcessCost = _.p.ProcessCost,
                                //SumOfIngredientCost = _.p.SumOfIngredientCost,
                                //TpPrice = _.p.TpPrice,
                                //RaMargin = _.p.RaMargin,
                                //BaseRate = _.p.BaseRate,
                                //XMargin = _.p.XMargin,
                                //FinalRate = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.FinalRate > 0 ? _.p.FinalRate + _.p.XMargin : 0) : _.p.FinalRate,
                                //ExPlantPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ExPlantPrice > 0 ? _.p.ExPlantPrice + _.p.XMargin : 0) : _.p.ExPlantPrice,
                                //ForDepotPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ForDepotPrice > 0 ? _.p.ForDepotPrice + _.p.XMargin : 0) : _.p.ForDepotPrice,
                                //ForPlantPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ForPlantPrice > 0 ? _.p.ForPlantPrice + _.p.XMargin : 0) : _.p.ForPlantPrice,
                                //ExDepotPrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ExDepotPrice > 0 ? _.p.ExDepotPrice + _.p.XMargin : 0) : _.p.ExDepotPrice,
                                //ClearanceRate = _.p.ClearanceRate,
                                //CounterBidOffer = _.p.CounterBidOffer,
                                //CounterBidLimit = _.p.CounterBidLimit,
                                //BpCpJumb = _.p.BpCpJumb,
                                //ExRakePrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ExRakePrice > 0 ? _.p.ExRakePrice + _.p.XMargin : 0) : _.p.ExRakePrice,
                                //ForRakePrice = _.p.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ? (_.p.ForRakePrice > 0 ? _.p.ForRakePrice + _.p.XMargin : 0) : _.p.ForRakePrice,
                                //Loadability = _.p.LoadQuantity,
                                //StartDate = _.PublishPrice.StartDate,
                                //EndDate = _.PublishPrice.EndDate,
                                //StatusId = _.PublishPrice.StatusId,
                                //BiddingWindowId = _.p.BiddingWindowId,
                                //AdditionalCost = _.p.AdditionalCost,
                                //OilTransferCost = _.p.OilTransferCostForPlant
                            }).ToList();
                        }

                        if (pricingListDto != null && pricingListDto.Any())
                        {
                            pricingListDto.ForEach(f =>
                            {
                                f.SkuName = skuDatas.FirstOrDefault(fd => fd.Id == f.SkuId).Name;
                                f.OilTypeName = oilTypeDatas.FirstOrDefault(fd => fd.Id == f.OilTypeId).Name;
                                //f.SaudaBookingType = saudaBookingTypeDatas.FirstOrDefault(fd => fd.Id == f.SaudaBookingTypeId).Name;// _.p.SaudaBookingType.Name,
                                //f.TransportMode = transportModesDatas.FirstOrDefault(fd => fd.Id == f.TransPortModeId).Name;// _.p.TransportMode.Name,
                                f.OilPackingType = oilPackingDatas.FirstOrDefault(fd => fd.Id == f.OilPackTypeId).Name;

                                if (f.BiddingWindowId > 0)
                                {
                                    var biddingWindow = _context.BiddingWindowTiming.AsNoTracking().FirstOrDefault(w => w.Id == f.BiddingWindowId);
                                    f.BiddingWindowTiming = biddingWindow.FromHours.ToString() + " - " + biddingWindow.ToHours.ToString();
                                }
                            });

                            return _resultService.SuccessObject(pricingListDto);
                        }
                        else
                        {
                            return _resultService.ErrorMessage(Constants.RecordNotFound);
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetStateBasePublishedPriceErrorDetails(PricePublishInputDto inputDto)
        {
            _methodName = "GetStateBasePublishedPriceErrorDetails";
            try
            {
                var pricePublishedList = new List<PricePublishesDto>();

                var pricePublishedData = _emamiContext.PriceGenerateDetail.AsNoTracking()
                    .FirstOrDefault(w => w.Id == inputDto.Id);

                if (pricePublishedData != null)
                {
                    pricePublishedList.Add(new PricePublishesDto() { ErrorMessage = pricePublishedData.ErrorMessage, StartDate = pricePublishedData.StartDate, EndDate = pricePublishedData.EndDate, Status = Utility.GetEnumFromString<DTO.Enums.PricePublishStatus>(pricePublishedData.StatusId), StatusId = pricePublishedData.StatusId });
                }
                return _resultService.SuccessMessageWitObject(pricePublishedList, "Success");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto FinalPriceBulkPublish(FinalPricePublishDto inputDto)
        {
            _methodName = "FinalPriceBulkPublish";
            try
            {
                var publishIds = new List<long?>();
                using (var _context = new AdaniContext())
                {
                    _context.Database.CommandTimeout = 0;

                    var priceGenerate = _context.PriceGenerate.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.PublishId);
                    if (priceGenerate == null)
                    {
                        return _resultService.ErrorMessage(Constants.PublishIdMissing);
                    }

                    if (inputDto == null)
                    {
                        return _resultService.ErrorMessage(Constants.InvalidRequest);
                    }

                    if (inputDto.LoginUserId == 0)
                    {
                        return _resultService.ErrorMessage(Constants.UserIdMissing);
                    }

                    if (!(inputDto.PublishId > 0))
                    {
                        return _resultService.ErrorMessage(Constants.PublishIdMissing);
                    }

                    var userContext = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                    if (userContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.UserNotFound);
                    }

                    var priceGenerateDetailIds = _emamiContext.PriceGenerateDetail.AsNoTracking()
                         .Where(w => w.PriceGenerateId == priceGenerate.Id && ((w.StatusId == (int)DTO.Enums.PricePublishStatus.Completed)
                         || w.StatusId == (int)DTO.Enums.PricePublishStatus.CompletedWithError))
                         .Select(s => new { Id = s.Id, CustomerGroupId = s.CustomerGroupId, BiddingWindowId = s.BiddingWindowId }).ToList();

                    if (priceGenerateDetailIds.IsNotAny())
                    {
                        return _resultService.ErrorMessage(Constants.PublishIdMissing);
                    }

                    priceGenerateDetailIds.ForEach(f => publishIds.Add(f.Id));

                    var pricingsContext = _context.TodayPricing.AsNoTracking()
                        //.Where(_ => publishIds.Contains(_.PublishId))
                        .
                        Select(s => new
                        {
                            //MaterialCostId = s.MaterialCostId,
                            //PackingCostId = s.PackingCostId,
                            //DepotCostId = s.DepotCostId,
                            //DetentionCostId = s.DetentionCostId,
                            //ProfitMarginId = s.ProfitMarginId,
                            //CushionMarginId = s.CushionMarginId,
                            //SchemeCostId = s.SchemeCostId,
                            //PrimaryFrieghtId = s.PrimaryFrieghtId,
                            //SecondaryFrieghtForPlantId = s.SecondaryFrieghtForPlantId,
                            //HoneycombCostId = s.HoneycombCostId,
                            //RaMarginId = s.RaMarginId,
                            //LoadCapacityId = s.LoadCapacityId,
                            //IngredientCostId = s.IngredientCostId,
                            //SkuIngrediantPlantId = s.SkuIngrediantPlantId,
                            //StateId = s.StateId,
                            //SecondaryFrieghtId = s.SecondaryFrieghtId,
                            //AdditionalCostId = s.AdditionalCostId,
                            //OilTransferCostId = s.OilTransferCosForPlantId
                        }).ToList();

                    var pricingIds = new List<long>();
                    var ingredientIds = new List<string>();
                    List<string> updateQuery = new List<string>();
                    string bookingType = //inputDto.BookingTypeId == //(long)DTO.Enums.SaudaBookingTypes.TraditionalProcess ?
                        DTO.Enums.SaudaBookingTypes.TraditionalProcess.ToString();
                    //: (inputDto.BookingTypeId == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction ? DTO.Enums.SaudaBookingTypes.ReverseAuction.ToString() : "");

                    if (pricingsContext != null && pricingsContext.Any())
                    {
                        //pricingIds = pricingsContext.Where(w => w.MaterialCostId != 0).Select(s => s.MaterialCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            if (inputDto.BookingTypeId == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                            {
                                updateQuery.Add("Update MaterialCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                            }
                            else
                            {
                                updateQuery.Add("Update RAMaterialCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                            }
                        }

                        //pricingIds = pricingsContext.Where(w => w.PackingCostId != 0).Select(s => s.PackingCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update PackingCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.DepotCostId != 0).Select(s => s.DepotCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update DepotCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.DetentionCostId != 0).Select(s => s.DetentionCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update DetentionCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.ProfitMarginId != 0).Select(s => s.ProfitMarginId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update ProfitMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.CushionMarginId != 0).Select(s => s.CushionMarginId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update CushionMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.SchemeCostId != 0).Select(s => s.SchemeCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update SchemeCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.PrimaryFrieghtId != 0).Select(s => s.PrimaryFrieghtId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update PrimaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.SecondaryFrieghtId != 0).Select(s => s.SecondaryFrieghtId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update SecondaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.SecondaryFrieghtForPlantId != 0).Select(s => s.SecondaryFrieghtForPlantId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update SecondaryFreights Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.HoneycombCostId != 0).Select(s => s.HoneycombCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update HoneycombCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.RaMarginId != 0).Select(s => s.RaMarginId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update RaMargins Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.LoadCapacityId != 0).Select(s => s.LoadCapacityId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update LoadCapacityConversions Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //ingredientIds = pricingsContext.Where(w => !string.IsNullOrEmpty(w.IngredientCostId)).Select(s => s.IngredientCostId).Distinct().ToList();
                        if (ingredientIds != null && ingredientIds.Any())
                        {
                            var ingredientId = string.Join(",", ingredientIds).Split(',').Distinct().ToList();
                            updateQuery.Add("Update IngredientCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", ingredientId) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.SkuIngrediantPlantId != 0).Select(s => s.SkuIngrediantPlantId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update SkuIngrediantPlants Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.AdditionalCostId != 0).Select(s => s.AdditionalCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update AdditionalCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        //pricingIds = pricingsContext.Where(w => w.OilTransferCostId != 0).Select(s => s.OilTransferCostId).Distinct().ToList();
                        if (pricingIds != null && pricingIds.Any())
                        {
                            updateQuery.Add("Update OilTransferCosts Set IsPublished = @isPublish Where Id in (" + string.Join(",", pricingIds) + ");");
                        }

                        using (SqlConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            conn.Open();
                            SqlCommand command;
                            SqlTransaction sqlTransaction = conn.BeginTransaction();
                            var startedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            try
                            {
                                #region Existing Price IsActive Status Update
                                //var currentDate = DateTime.Now;
                                //var priceGenerateIds = _context.PriceGenerate.AsNoTracking()
                                //    .Where(f => DbFunctions.TruncateTime(f.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                                //&& f.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess).Select(s => s.Id).ToList();

                                //var stateIds = finapPriceGenerateDetails.Select(s => s.StateId).ToList();

                                //var existPriceGenerateDetail = _context.PriceGenerateDetail.AsNoTracking()
                                //    .Where(w => priceGenerateIds.Contains(w.PriceGenerateId) && w.IsPublish && stateIds.Contains(w.StateId))
                                //    .Select(s => new
                                //    {
                                //        Id = s.Id,
                                //        PriceGenerateId = s.PriceGenerateId,
                                //        OilTypeId = s.OilTypeId,
                                //        PackGroupId = s.PackGroupId,
                                //        PlantId = s.PlantId,
                                //        ZoneId = s.ZoneId,
                                //        StateId = s.StateId,
                                //        StatusId = s.StatusId
                                //    }).ToList();

                                //var priceGenerateDetails = _context.PriceGenerateDetail.AsNoTracking()
                                //     .Where(w => w.PriceGenerateId == priceGenerate.Id
                                //     && ((w.StatusId == (int)DTO.Enums.PricePublishStatus.Completed)
                                //     || w.StatusId == (int)DTO.Enums.PricePublishStatus.CompletedWithError))
                                //     .Select(s => new
                                //     {
                                //         Id = s.Id,
                                //         PriceGenerateId = s.PriceGenerateId,
                                //         OilTypeId = s.OilTypeId,
                                //         PackGroupId = s.PackGroupId,
                                //         PlantId = s.PlantId,
                                //         ZoneId = s.ZoneId,
                                //         StateId = s.StateId,
                                //         StatusId = s.StatusId
                                //     }).ToList();


                                //if (priceGenerateDetails != null && priceGenerateDetails.Any()
                                //    && existPriceGenerateDetail != null && existPriceGenerateDetail.Any())
                                //{
                                //    foreach (var existingData in existPriceGenerateDetail)
                                //    {
                                //        bool isExistPrice = _context.Pricing.AsNoTracking().Any(a => a.PublishId == existingData.Id && a.IsActive);
                                //        if (isExistPrice)
                                //        {
                                //            foreach (var priceGenerat in priceGenerateDetails)
                                //            {
                                //                //var existingData = existPriceGenerateDetail.FirstOrDefault(f => f.PlantId == priceGenerat.PlantId && f.ZoneId == priceGenerat.ZoneId && f.StateId == priceGenerat.StateId);
                                //                if (existingData != null)
                                //                {
                                //                    var publishedOilTypes = existingData.OilTypeId.Split(',');
                                //                    var publishedOilTypesPackGroups = existingData.PackGroupId.Split(',');

                                //                    var publishOilTypes = priceGenerat.OilTypeId.Split(',');
                                //                    var publishOilTypesPackGroups = priceGenerat.PackGroupId.Split(',');

                                //                    var statusUpdateOilTypes = publishOilTypes.Where(a => publishedOilTypes.Contains(a)).Select(Int64.Parse).ToList();
                                //                    var statusUpdatePackGroup = publishOilTypesPackGroups.Where(a => publishedOilTypesPackGroups.Contains(a)).Select(Int64.Parse).ToList();
                                //                    if (statusUpdatePackGroup != null && statusUpdatePackGroup.Any())
                                //                    {
                                //                        string statusFalsePricing = "Update Pricings set IsActive = @IsActive Where PublishId = @PublishId and PlantId = @PlantId and StateId = @StateId and PublishId = @PublishId and OilTypeId in (" + string.Join(",", statusUpdateOilTypes) + ") and OilPackingTypeId in (" + string.Join(",", statusUpdatePackGroup) + ")";
                                //                        command = new SqlCommand(statusFalsePricing, conn);
                                //                        command.Parameters.AddWithValue("@IsActive", false);
                                //                        command.Parameters.AddWithValue("@PublishId", existingData.Id);
                                //                        command.Parameters.AddWithValue("@PlantId", priceGenerat.PlantId);
                                //                        command.Parameters.AddWithValue("@StateId", priceGenerat.StateId);
                                //                        command.Transaction = sqlTransaction;
                                //                        command.ExecuteNonQuery();
                                //                        command.Parameters.Clear();
                                //                        _logger.Info($"{bookingType} Pricings Table Status IsActive Updated - Date Time : {startedDate} PublishId = {existingData.Id} OilTypeId = {string.Join(",", statusUpdateOilTypes)} OilPackingTypeId = {string.Join(",", statusUpdatePackGroup)} PlantId = {priceGenerat.PlantId} StateId = {priceGenerat.StateId}");
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                                #endregion

                                string updatePricings = "Update TodayPricings set IsPublish = @isPublish, BiddingDate = @biddingDate, ModifiedBy = @modifiedBy, ModifiedDate = @modifiedDate Where PublishId in (" + string.Join(",", publishIds) + ")";
                                string tblQuery = string.Join("", updateQuery ?? new List<string>());

                                command = new SqlCommand(updatePricings, conn);
                                command.Parameters.AddWithValue("@isPublish", true);
                                command.Parameters.AddWithValue("@biddingDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                                command.Parameters.AddWithValue("@modifiedBy", inputDto.LoginUserId);
                                command.Parameters.AddWithValue("@modifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                                //command.Parameters.AddWithValue("@publishIds", inputDto.PublishId);
                                command.Transaction = sqlTransaction;
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                                _logger.Info($"{bookingType} TodayPricing Table Updated - Date Time : {startedDate}");

                                string updatePricePublishes = "Update PriceGenerateDetails set IsPublish = @isPublish, ModifiedBy = @modifiedBy, ModifiedDate = @modifiedDate Where Id in (" + string.Join(",", publishIds) + ");";
                                command = conn.CreateCommand();
                                command.CommandText = updatePricePublishes;
                                command.Parameters.AddWithValue("@isPublish", true);
                                command.Parameters.AddWithValue("@modifiedBy", inputDto.LoginUserId);
                                command.Parameters.AddWithValue("@modifiedDate", DateHelper.UtcToIndia(DateTime.UtcNow));
                                //command.Parameters.AddWithValue("@publishIds", inputDto.PublishId);
                                command.Transaction = sqlTransaction;
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                                _logger.Info($"{bookingType} - PricePublishes Table Updated - Date Time : {startedDate}");

                                if (!string.IsNullOrEmpty(tblQuery))
                                {
                                    command = conn.CreateCommand();
                                    command.CommandText = tblQuery;
                                    command.Parameters.AddWithValue("@isPublish", true);
                                    command.Transaction = sqlTransaction;
                                    command.ExecuteNonQuery();
                                    command.Parameters.Clear();
                                    _logger.Info($"{bookingType} - All TodayPricings Table Updated - Date Time : {startedDate}");
                                }
                                sqlTransaction.Commit();
                            }
                            catch (Exception exception)
                            {
                                sqlTransaction.Rollback();
                                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {exception}");
                                return _resultService.ErrorMessage(Constants.Exception);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                        if (inputDto.BookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                        {
                            //var publishStateIds = pricingsContext.Select(_ => _.StateId).Distinct().ToList();
                            //System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => FinalPricePublishBackgroundQueue(publishStateIds, priceGenerate.VerticalId));
                            priceGenerateDetailIds.Select(s => s.CustomerGroupId).ToList();
                        }
                        //else if (inputDto.BookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                        //{
                        //    var pricingMailDto = new PricingMailDto
                        //    {
                        //        CustomerGroupIds = priceGenerateDetailIds.Select(s => s.CustomerGroupId).Distinct().ToList(),
                        //        BiddingWindowId = priceGenerateDetailIds.Select(s => s.BiddingWindowId).FirstOrDefault(),
                        //        NotificationActionId = (int)DTO.Enums.NotificationActions.WindowPricePublish,
                        //    };
                        //    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => _notificationService.ReverseAuctionWindowNotificationAsync(pricingMailDto, cancellationToken));
                        //}
                    }

                    //var pricePublishedList = new List<PricePublishesDto>();

                    //var pricePublishedData = _emamiContext.PriceGenerateDetail.AsNoTracking()
                    //    .FirstOrDefault(w => w.Id == inputDto.Id);

                    //if (pricePublishedData != null)
                    //{
                    //    pricePublishedList.Add(new PricePublishesDto() { ErrorMessage = pricePublishedData.ErrorMessage, StartDate = pricePublishedData.StartDate, EndDate = pricePublishedData.EndDate });
                    //}
                    //var pricingMailDto = new PricingMailDto
                    //{
                    //    CustomerGroupIds = priceGenerateDetailIds.Select(s => s.CustomerGroupId).ToList(),
                    //    BiddingWindowId = inputDto.BiddingWindowId,
                    //    NotificationActionId = (int)DTO.Enums.NotificationActions.WindowPricePublish
                    //};

                    //HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => _notificationService.PricePublishNotificationAsync(pricingMailDto, cancellationToken));
                    return _resultService.SuccessMessageWitObject(publishIds, "Success");
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

        #region RA2.0 Final Price

        public ResultDto RaFinalPricePriceGenerate(RaFinalPriceGenerateInputDto inputDto)
        {
            _methodName = "RaFinalPricePriceGenerate";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.CustomerGroupIds == null || !inputDto.CustomerGroupIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.CustomerGroupIsEmpty);
                }

                var checkingBiddingWindowStartTime = _emamiContext.BiddingWindow.FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId).StartTime;
                if (checkingBiddingWindowStartTime.AddSeconds(-DateTime.Now.Second) <= DateTime.Now.AddSeconds(-DateTime.Now.Second))
                {
                    return _resultService.ErrorMessage(Constants.BiddingWindowMovedToInprogressState);
                }

                var priceGenerate = new PriceGenerate()
                {
                    SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
                    VerticalId = inputDto.VerticalId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    ExeStatusId = (int)ExeStatus.Pending
                };
                _emamiContext.PriceGenerate.Add(priceGenerate);
                _emamiContext.SaveChanges();

                foreach (var customerId in inputDto.CustomerGroupIds)
                {
                    //var customerGroup = _emamiContext.BiddingWindowCustomerGroups.FirstOrDefault(f => f.CustomerGroupId == customerId);
                    _emamiContext.PriceGenerateDetail.Add(new PriceGenerateDetail()
                    {
                        PriceGenerateId = priceGenerate.Id,
                        OilTypeId = string.Join(",", inputDto.OilTypeIds),
                        PackGroupId = string.Join(",", inputDto.OilPackingTypeIds),
                        PlantId = inputDto.PlantId,
                        StatusId = (int)DTO.Enums.PricePublishStatus.Pending,
                        BiddingWindowId = inputDto.BiddingWindowId,
                        CustomerGroupId = customerId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    });
                }
                _emamiContext.SaveChanges();

                if (Config.IsFinalPriceGenerateOld)
                {
                    ProcessFileTrigger processFileTrigger = new ProcessFileTrigger();
                    processFileTrigger.FinalPriceExeInvoke(inputDto.SaudaBookingTypeId);
                }
                //HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => _notificationService.PricePublishNotificationAsync(inputDto.CustomerGroupIds, BW,1, cancellationToken));

                return _resultService.SuccessMessage(Constants.SuccessMessage);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto RaGetFinalPriceGenerates(RaPricePublishInputDto inputDto)
        {
            _methodName = "RaGetFinalPriceGenerates";
            var priceGenerateList = new List<RaFinalPriceGenerateOutputDto>();
            try
            {
                if (inputDto.RoleId == (long)DTO.Enums.Role.Admin)
                {
                    priceGenerateList = _emamiContext.PriceGenerate.AsNoTracking()
                    .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                    .Select(s => new RaFinalPriceGenerateOutputDto
                    {
                        Id = s.Id,
                        PricingDate = s.CreatedDate,
                        SaudaBookingTypeId = s.SaudaBookingTypeId,
                        SaudaBookingType = s.SaudaBookingType.Name,
                        Vertical = s.Vertical.Name,
                        TotalCustomerGroup = s.PriceGenerateDetail.Count
                    }).ToList();
                }
                else
                {
                    priceGenerateList = _emamiContext.PriceGenerate.AsNoTracking()
                    .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.SearchDate) && w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId && w.CreatedBy == inputDto.LoginUserId)
                    .Select(s => new RaFinalPriceGenerateOutputDto
                    {
                        Id = s.Id,
                        PricingDate = s.CreatedDate,
                        SaudaBookingTypeId = s.SaudaBookingTypeId,
                        SaudaBookingType = s.SaudaBookingType.Name,
                        Vertical = s.Vertical.Name,
                        TotalCustomerGroup = s.PriceGenerateDetail.Count
                    }).ToList();
                }

                if (priceGenerateList != null && priceGenerateList.Any())
                {
                    //priceGenerateList.ForEach(f =>
                    //{
                    foreach (var f in priceGenerateList)
                    {
                        var priceGenerateDetail = _emamiContext.PriceGenerateDetail.AsNoTracking()
                        .Where(w => w.PriceGenerateId == f.Id).Select(s => new { IsPublish = s.IsPublish, StatusId = s.StatusId, TaskStatusId = s.TaskStatusId, BiddWindowId = s.BiddingWindowId });

                        if (priceGenerateDetail != null && priceGenerateDetail.Any())
                        {
                            long windowId = priceGenerateDetail.FirstOrDefault().BiddWindowId;
                            var biddingWindow = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(w => w.Id == windowId);

                            f.BiddingWindowName = biddingWindow.Name;
                            f.StartTime = biddingWindow.StartTime;
                            f.EndTime = biddingWindow.EndTime;
                            f.WindowStatus = Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingWindow.StatusId);

                            var isProcessing = priceGenerateDetail.Any(a => a.TaskStatusId == (int)DTO.Enums.FinalPriceTaskStatus.Created);
                            if (isProcessing)
                            {
                                f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.PriceGenerating;
                            }
                            else
                            {
                                if (priceGenerateDetail.Any(a => a.IsPublish == true))
                                {
                                    f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.Published;
                                }
                                else if (priceGenerateDetail.All(a => a.StatusId == (int)DTO.Enums.PricePublishStatus.Failed))
                                {
                                    f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.PriceGenerateFailed;
                                }
                                else if (priceGenerateDetail.Any(a => a.StatusId == (int)DTO.Enums.PricePublishStatus.Completed
                                 || a.StatusId == (int)DTO.Enums.PricePublishStatus.CompletedWithError))
                                {
                                    f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.Publish;
                                }
                            }
                        }
                        else
                        {
                            f.PublishButtonStatus = (int)DTO.Enums.PublishButtonStatus.PriceGenerateFailed;
                        }
                    }
                }

                return _resultService.SuccessMessageWitObject(priceGenerateList, Constants.SuccessMessage);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto RaGetFinalPriceGenerateDetails(RaPricePublishInputDto inputDto)
        {
            _methodName = "RaGetFinalPriceGenerateDetails";
            var pricingList = new List<RaFinalPriceGenerateDetailOutputDto>();
            var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            try
            {
                var priceGenerateList = _emamiContext.PriceGenerateDetail.AsNoTracking()
                    .Where(w => w.PriceGenerateId == inputDto.Id)
                    .Select(s => new
                    {
                        Id = s.Id,
                        OilTypeId = s.OilTypeId,
                        PackGroupId = s.PackGroupId,
                        PlantName = s.Plant.Name,
                        CustomerGroupId = s.CustomerGroupId,
                        StatusId = s.StatusId,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        PublishDate = s.ModifiedDate,
                        IsPublish = s.IsPublish,
                        Message = s.ErrorMessage,
                        ErrorMessageCount = s.ErrorMessageCount,
                        TaskStatusId = s.TaskStatusId,
                        BiddingWindowId = s.BiddingWindowId
                    }).ToList();

                if (priceGenerateList != null && priceGenerateList.Any())
                {
                    var oilTypeIds = priceGenerateList.FirstOrDefault().OilTypeId.ToString().Split(',').Select(s => Int64.Parse(s)).ToList();
                    var oilPackingTypeIds = priceGenerateList.FirstOrDefault().PackGroupId.ToString().Split(',').Select(s => Int64.Parse(s)).ToList();

                    #region Price Generate Parameters
                    var oilTypeName = _emamiContext.OilTypes.AsNoTracking().Where(w => oilTypeIds.Contains(w.Id))
                        .Select(s => new { OilName = s.Name }).ToList();
                    var oilPackType = _emamiContext.OilPackingTypes.AsNoTracking().Where(w => oilPackingTypeIds.Contains(w.Id))
                        .Select(s => new { OilPackName = s.Name }).ToList();
                    #endregion

                    if (inputDto.SearchDate.Date == todayDate.Date)
                    {
                        foreach (var price in priceGenerateList)
                        {
                            //int recordCount = _emamiContext.TodayPricing.AsNoTracking().Count(c => c.PublishId == price.Id);
                            var status = Utility.GetEnumFromString<DTO.Enums.PricePublishStatus>(price.StatusId);
                            var customerGroupName = _emamiContext.CustomerGroups.FirstOrDefault(f => f.Id == price.CustomerGroupId).Name;
                            pricingList.Add(new RaFinalPriceGenerateDetailOutputDto()
                            {
                                Id = price.Id,
                                OilType = string.Join(",", oilTypeName.Select(s => s.OilName)),
                                PackGroup = string.Join(",", oilPackType.Select(s => s.OilPackName)),
                                PlantName = price.PlantName,
                                CustomerGroupName = customerGroupName,
                                StatusId = price.StatusId,
                                Status = status,
                                //TotalPriceCount = _emamiContext.TodayPricing.AsNoTracking().Count(c => c.PublishId == price.Id),
                                TotalPriceCount = _emamiContext.TodayPricing.AsNoTracking().Count(),
                                StartDate = price.StartDate,
                                EndDate = price.EndDate,
                                PublishDate = price.PublishDate,
                                IsPublish = price.IsPublish,
                                ErrorMessageCount = price.ErrorMessageCount,
                                TaskStatusId = price.TaskStatusId,
                                TaskStatus = Utility.GetEnumFromString<DTO.Enums.FinalPriceTaskStatus>(price.TaskStatusId),
                                CustomerGroupId = price.CustomerGroupId,
                                BiddingWindowId = price.BiddingWindowId
                            });
                        }
                    }
                    else if (inputDto.SearchDate.Date < todayDate.Date)
                    {
                        foreach (var price in priceGenerateList)
                        {
                            int recordCount = _emamiContext.PricingBackup.AsNoTracking().Count(c => c.PublishId == price.Id);
                            var status = Utility.GetEnumFromString<DTO.Enums.PricePublishStatus>(price.StatusId);
                            var customerGroupName = _emamiContext.CustomerGroups.FirstOrDefault(f => f.Id == price.CustomerGroupId).Name;
                            pricingList.Add(new RaFinalPriceGenerateDetailOutputDto()
                            {
                                Id = price.Id,
                                OilType = string.Join(",", oilTypeName.Select(s => s.OilName)),
                                PackGroup = string.Join(",", oilPackType.Select(s => s.OilPackName)),
                                PlantName = price.PlantName,
                                CustomerGroupName = customerGroupName,
                                StatusId = price.StatusId,
                                Status = status,
                                TotalPriceCount = _emamiContext.PricingBackup.AsNoTracking().Count(),
                                //TotalPriceCount = _emamiContext.PricingBackup.AsNoTracking().Count(c => c.PublishId == price.Id),
                                StartDate = price.StartDate,
                                EndDate = price.EndDate,
                                PublishDate = price.PublishDate,
                                IsPublish = price.IsPublish,
                                ErrorMessageCount = price.ErrorMessageCount,
                                TaskStatusId = price.TaskStatusId,
                                TaskStatus = Utility.GetEnumFromString<DTO.Enums.FinalPriceTaskStatus>(price.TaskStatusId),
                                CustomerGroupId = price.CustomerGroupId,
                                BiddingWindowId = price.BiddingWindowId
                            });
                        }
                    }

                }

                return _resultService.SuccessMessageWitObject(pricingList, Constants.SuccessMessage);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Final Price Download

        public ResultDto ZoneBasedFinalPriceDownload(PriceDownloadInputDto inputDto)
        {
            var resultDto = new ResultDto();
            var outputDto = new FileOutputDto();
            int skipCount = 0;
            int takeCount = Config.RecordCountForExcelSheet;
            decimal divider = Convert.ToDecimal(string.Format("{0:0.0}", takeCount));
            decimal count = 0;

            try
            {
                var totalDataCount = GetTotalPricingCountForZoneBased(inputDto.SearchDate, inputDto.PriceGenerateId);
                if (totalDataCount < takeCount)
                    count = 1;
                else
                    count = Math.Round(totalDataCount / divider) + 2;

                if (count > 0)
                {
                    string folderName = DTO.Enums.PageType.FinalPriceDownload.ToString();
                    var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string templatePath = Path.Combine(directory + "FinalPriceTemplateForAllRecords.xlsx");
                    string savePath = "";
                    string guidFileName = "";
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        if (ep.Workbook.Worksheets.Any())
                        {
                            ep.Workbook.Worksheets.Delete("Sheet1");
                        }
                        for (int i = 0; i < count; i++)
                        {
                            var ws = ep.Workbook.Worksheets.Add("Sheet" + (i + 1).ToString());
                        }
                        guidFileName = Guid.NewGuid().ToString() + ".xlsx";
                        savePath = Path.Combine(directory, guidFileName);
                        if (System.IO.File.Exists(savePath))
                        {
                            System.IO.File.Delete(savePath);
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                        else
                        {
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                    }
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(savePath)))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            using (SqlConnection conn = new SqlConnection(Config.DBConnectionString))
                            {
                                conn.Open();
                                string SP_Name = "GetFinalPriceDataExport";
                                SqlCommand cmd = new SqlCommand(SP_Name, conn);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                //cmd.Parameters.AddWithValue("@Skip", skipCount);
                                //cmd.Parameters.AddWithValue("@Take", takeCount);
                                cmd.Parameters.AddWithValue("@PriceId", inputDto.PriceGenerateId);
                                //cmd.Parameters.AddWithValue("@SearchDate", inputDto.SearchDate);
                                cmd.CommandTimeout = 0;
                                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                                var ws = ep.Workbook.Worksheets["Sheet" + (i + 1).ToString()];
                                ws.Cells["A1:BZ1"].Style.Font.Bold = true;
                                ws.Cells["A1:BZ1"].Style.Font.Size = 12;
                                ws.Cells["A1"].LoadFromDataReader(sqlDataReader, true);
                                ep.Save();
                            }
                            skipCount += takeCount;
                        }
                        //jsonResult = Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet); ;
                    }

                    var filename = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName, guidFileName);
                    outputDto = new FileOutputDto
                    {
                        FilePath = filename,
                        FileName = guidFileName
                    };
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return resultDto;
        }

        public ResultDto DownloadPriceGenerateSuccessList(PriceDownloadInputDto inputDto)
        {
            var resultDto = new ResultDto();
            var outputDto = new FileOutputDto();
            int skipCount = 0;
            int takeCount = Config.RecordCountForExcelSheet;
            decimal divider = Convert.ToDecimal(string.Format("{0:0.0}", takeCount));
            decimal count = 0;
            try
            {
                var totalDataCount = GetTotalPricingCountForStateBased(inputDto.SearchDate, inputDto.PriceGenerateDetailId);

                if (totalDataCount < takeCount)
                    count = 1;
                else
                    count = Math.Round(totalDataCount / divider) + 2;

                if (count > 0)
                {
                    string folderName = DTO.Enums.PageType.FinalPriceDownload.ToString();
                    var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string templatePath = Path.Combine(directory + "FinalPriceTemplateForAllRecords.xlsx");
                    string savePath = "";
                    string guidFileName = "";
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        if (ep.Workbook.Worksheets.Any())
                        {
                            ep.Workbook.Worksheets.Delete("Sheet1");
                        }
                        for (int i = 0; i < count; i++)
                        {
                            var ws = ep.Workbook.Worksheets.Add("Sheet" + (i + 1).ToString());
                        }
                        guidFileName = Guid.NewGuid().ToString() + ".xlsx";
                        savePath = Path.Combine(directory, guidFileName);
                        if (System.IO.File.Exists(savePath))
                        {
                            System.IO.File.Delete(savePath);
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                        else
                        {
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                    }
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(savePath)))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            using (SqlConnection conn = new SqlConnection(Config.DBConnectionString))
                            {
                                conn.Open();
                                string SP_Name = "SP_TPStateBasedDownload";
                                SqlCommand cmd = new SqlCommand(SP_Name, conn);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Skip", skipCount);
                                cmd.Parameters.AddWithValue("@Take", takeCount);
                                cmd.Parameters.AddWithValue("@publishId", inputDto.PriceGenerateDetailId);
                                cmd.Parameters.AddWithValue("@SearchDate", inputDto.SearchDate);
                                cmd.CommandTimeout = 0;
                                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                                var ws = ep.Workbook.Worksheets["Sheet" + (i + 1).ToString()];
                                ws.Cells["A1:BZ1"].Style.Font.Bold = true;
                                ws.Cells["A1:BZ1"].Style.Font.Size = 12;
                                ws.Cells["A1"].LoadFromDataReader(sqlDataReader, true);
                                //ws.Cells.AutoFitColumns();
                                ep.Save();
                            }
                            skipCount += takeCount;
                        }

                    }
                    var filename = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName, guidFileName);
                    outputDto = new FileOutputDto
                    {
                        FilePath = filename,
                        FileName = guidFileName
                    };
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return resultDto;
        }

        public ResultDto DownloadPriceGenerateErrorList(PriceDownloadInputDto inputDto)
        {
            var resultDto = new ResultDto();
            var outputDto = new FileOutputDto();
            try
            {
                string bookingTypeName = "";
                string fileGuid = "";
                string savePath = "";
                string guidFileName = "";
                //PriceErrorMessageDto inputDtos = new PriceErrorMessageDto() { Id = inputDto.PricingId };
                var pricingInputDto = new PricePublishInputDto { Id = inputDto.PriceGenerateId };

                var pricingErrorData = GetStateBasePublishedPriceErrorDetail(pricingInputDto);
                var priceData = pricingErrorData != null && pricingErrorData.Any() ? pricingErrorData.FirstOrDefault() : new PricePublishesDto();

                if (!string.IsNullOrEmpty(priceData.ErrorMessage))
                {
                    string folderName = DTO.Enums.PageType.FinalPriceDownload.ToString();
                    var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string templatePath = Path.Combine(directory + "FinalPriceTemplate.xlsx");

                    using (var ep = new ExcelPackage(new FileInfo(templatePath)))
                    {
                        var ws = ep.Workbook.Worksheets[1];
                        ws.Name = Utility.GetEnumFromString<SaudaBookingTypes>(inputDto.SaudaBookingTypeId);

                        #region Header
                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = "AWL Agri business.";
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.Font.Size = 16;

                        ws.Cells["A2"].Value = "Report Name";
                        ws.Cells["A3"].Value = "Process Start Date Time";
                        ws.Cells["A4"].Value = "Process End Date Time";
                        ws.Cells["A5"].Value = "Status";
                        if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                        {
                            bookingTypeName = SaudaBookingTypes.TraditionalProcess.ToString();
                            ws.Cells["A6"].Value = "Total Record Count";
                        }
                        //else if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                        //{
                        //    bookingTypeName = SaudaBookingTypes.ReverseAuction.ToString();
                        //}

                        ws.Cells["B2"].Value = bookingTypeName;
                        ws.Cells["B3"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", priceData.StartDate);
                        ws.Cells["B4"].Value = string.Format("{0:dd-MMM-yyyy hh:mm tt}", priceData.EndDate);
                        ws.Cells["B5"].Value = Utility.GetEnumFromString<PricePublishStatus>(priceData.StatusId);
                        ws.Cells["B6"].Value = Utility.CalculateErrorMessageCount(priceData.ErrorMessage);
                        if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                        {
                            //ws.Cells["B6"].Value = 0;
                        }
                        //else if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                        //{
                        //    ws.Cells["B6"].Value = priceData.BiddingWindowTiming;
                        //    //ws.Cells["B7"].Value = 0;
                        //}

                        for (int i = 2; i <= 7; i++)
                        {
                            ws.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            ws.Cells["A" + i].Style.Font.Bold = true;
                            ws.Cells["A" + i].Style.Font.Size = 12;

                            ws.Cells["B" + i + ":" + "F" + i].Merge = true;
                            ws.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        #region Message                    

                        int headerIndex = 8;
                        int index = 0;

                        ws.Cells["A" + headerIndex].Value = "Sku Name";
                        ws.Cells["B" + headerIndex].Value = "Sku Code";
                        ws.Cells["C" + headerIndex].Value = "Depot Name";
                        ws.Cells["D" + headerIndex].Value = "State Name";
                        ws.Cells["E" + headerIndex].Value = "Freight Route Name";
                        ws.Cells["F" + headerIndex].Value = "Transport Mode Name";
                        ws.Cells["G" + headerIndex].Value = "Load Capacity";
                        ws.Cells["H" + headerIndex].Value = "Missing Data";

                        ExcelRange range = ws.Cells["A8:H8"];
                        range.AutoFitColumns();
                        range.Style.Font.Size = 12;
                        range.Style.Font.Bold = true;

                        if (priceData.ErrorMessage.Contains("|"))
                        {
                            var errorList = priceData.ErrorMessage.Split('|');
                            foreach (var error in errorList)
                            {
                                var errorResult = error.Split('~');
                                if (errorResult != null && errorResult.Any())
                                {
                                    headerIndex++;
                                    ws.Cells["A" + headerIndex].Value = errorResult.Length > 0 ? errorResult[index].ToString() : "";
                                    ws.Cells["B" + headerIndex].Value = errorResult.Length > 1 ? errorResult[index + 1].ToString() : "";
                                    ws.Cells["C" + headerIndex].Value = errorResult.Length > 2 ? errorResult[index + 2].ToString() : "";
                                    ws.Cells["D" + headerIndex].Value = errorResult.Length > 3 ? errorResult[index + 3].ToString() : "";
                                    ws.Cells["E" + headerIndex].Value = errorResult.Length > 4 ? errorResult[index + 4].ToString() : "";
                                    ws.Cells["F" + headerIndex].Value = errorResult.Length > 5 ? errorResult[index + 5].ToString() : "";
                                    ws.Cells["G" + headerIndex].Value = errorResult.Length > 6 ? errorResult[index + 6].ToString() : "";
                                    ws.Cells["H" + headerIndex].Value = errorResult.Length > 7 ? errorResult[index + 7].ToString() : "";
                                }
                            }
                        }
                        else
                        {
                            headerIndex++;
                            ws.Cells["H" + headerIndex].Value = priceData.ErrorMessage;
                        }

                        #endregion

                        ws.Cells.AutoFitColumns();

                        guidFileName = fileGuid + ".xlsx";
                        savePath = Path.Combine(directory, guidFileName);

                        if (System.IO.File.Exists(savePath))
                        {
                            System.IO.File.Delete(savePath);
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                        else
                        {
                            using (Stream stream = System.IO.File.Create(savePath))
                            {
                                ep.SaveAs(stream);
                            }
                        }
                    }
                    var filename = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName, guidFileName);
                    outputDto = new FileOutputDto
                    {
                        FilePath = filename,
                        FileName = guidFileName
                    };
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return resultDto;
        }

        public long GetTotalPricingCountForZoneBased(DateTime SearchDate, long PriceId)
        {
            var result = 0;
            try
            {
                var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (SearchDate.Date == todayDate.Date)
                {
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        result = conn.Execute(@"select Count(Id) from TodayPricings as p
                        where p.PublishId in
                        (Select pgd.Id From PriceGenerates pg Join PriceGenerateDetails pgd on pg.Id = pgd.PriceGenerateId Where pg.Id = @PriceId)
                        ", new
                        {
                            PriceId = PriceId
                        });
                    }
                }
                else if (SearchDate.Date < todayDate.Date)
                {
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        result = conn.Execute(@"select Count(Id) from PricingBackups as p
                        where p.PublishId in
                        (Select pgd.Id From PriceGenerates pg Join PriceGenerateDetails pgd on pg.Id = pgd.PriceGenerateId Where pg.Id = @PriceId)", new
                        {
                            PriceId = PriceId
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return result;

        }

        public long GetTotalPricingCountForStateBased(DateTime SearchDate, long PublishId)
        {
            var result = 0;
            try
            {
                var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (SearchDate.Date == todayDate.Date)
                {
                    using (SqlConnection query = new SqlConnection(Config.DBConnectionString))
                    {
                        result = query.QueryFirstOrDefault<int>("Select Count(Id) as CountOfRecords From TodayPricings Where PublishId = @PublishId ", new
                        {
                            PublishId = PublishId
                        });
                    }
                }
                else if (SearchDate.Date < todayDate.Date)
                {
                    using (SqlConnection query = new SqlConnection(Config.DBConnectionString))
                    {
                        result = query.QueryFirstOrDefault<int>("Select Count(Id) as CountOfRecords From PricingBackups Where PublishId = @PublishId ", new
                        {
                            PublishId = PublishId
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return result;

        }

        public List<PricePublishesDto> GetStateBasePublishedPriceErrorDetail(PricePublishInputDto inputDto)
        {
            _methodName = "GetStateBasePublishedPriceErrorDetail";
            var pricePublishedList = new List<PricePublishesDto>();
            try
            {

                var pricePublishedData = _emamiContext.PriceGenerateDetail.AsNoTracking()
                    .FirstOrDefault(w => w.Id == inputDto.Id);

                if (pricePublishedData != null)
                {
                    pricePublishedList.Add(new PricePublishesDto() { ErrorMessage = pricePublishedData.ErrorMessage, StartDate = pricePublishedData.StartDate, EndDate = pricePublishedData.EndDate, Status = Utility.GetEnumFromString<DTO.Enums.PricePublishStatus>(pricePublishedData.StatusId), StatusId = pricePublishedData.StatusId });
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return pricePublishedList;
        }

        #endregion

    }
}
