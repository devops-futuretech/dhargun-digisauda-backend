using Adani.Solution.DTO;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.MVC.ServiceClient;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Models;
using Newtonsoft.Json.Converters;
using GMCore.Logger;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using GMCore.Helper;
using OfficeOpenXml.Drawing;
using System.Drawing;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class ImportController : BaseController
    {
        private readonly ImportClient _importClient;
        private const string ServiceName = "Import Controller";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        public string[] fileNames = { "profitmargin", "salesorganization","distributionchannel","division","city","broker","cushionmargin","customer","customergroupfive","depotcost","detentioncost","freightroute","honeycombcost"
                ,"loadcapacity","materialcost","oiltype","packingcost","primaryfreight","ramargin","schemecost","secondaryfreight","material","user"
                ,"usercustomermapping","usercustomersalestarget","usercustomersaudatarget","userdepotmapping","freightzone","depot","plant"
                ,"plantdepotmapping","district","state","territory","territorydistrictmapping", "ingredient", "ingredientcost","retailer","geography"
                ,"skuingredient","pendingsauda","secondaryfreightmaster","usercustomertarget","customermaster","rake","tradeticket","shiptoparty","vehicleloadability","saudaconversionunitandbaseratedifference","customergroupone","customergrouptwo",
                "cmsuser" , "dealersaudavalidity" , "ramaterialcost" ,"dealersaudalimit","dealercallrecordingdetails","brokercallrecordingdetails","materialtype","volumeloadability","userdivisionmapping" ,"userdiscount","geographydiscount","line","qpsdiscount","skubpcpmapping",
                "gamificationdashboard","quantitylimit","saudaconditionalbookingconfiguration"};

        private readonly JsonSerializerSettings _dateAndNullSettings;
        private readonly JsonSerializerSettings _dateTimeAndNullSettings;
        static string connectionString = ConfigHelper.SPConnectionString;

        public ImportController()
        {
            _importClient = new ImportClient { ControllerDelegate = this };

            _dateAndNullSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Populate,
                Converters = new List<JsonConverter>() { new IsoDateTimeConverter() { DateTimeFormat = Settings.DateFormatForImportData } }
            };

            _dateTimeAndNullSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Populate,
                Converters = new List<JsonConverter>() { new IsoDateTimeConverter() { DateTimeFormat = Settings.DateTimeFormatForImportData } }
            };
        }

        [AuthorizeClaims(Claims.ImportData)]
        public ActionResult DataUpload()
        {
            if (TempData["isPageLoaded"] == null)
            {
                Session["UploadedFiles"] = null;
            }
            return View();
        }

        public JsonResult GetEnityList([DataSourceRequest] DataSourceRequest request)
        {
            var approveList = ((Entity[])Enum.GetValues(typeof(Entity))).Select(c => new EnumModel() { EntityTypeId = (int)c, Name = c.Description().ToString() }).OrderBy(_ => _.Name).ToList();
            return Json(approveList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            _methodName = "Products_Read";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SkuUploadDto> result = new List<SkuUploadDto>();
            try
            {
                if (Session["UploadedFiles"] != null)
                {
                    var x = Session["FileName"].ToString();
                    if (fileNames.Contains(Session["FileName"].ToString()))
                    {
                        var dt = new DataTable();
                        if (Session["FileName"].ToString() == "userdiscount" || Session["FileName"].ToString() == "geographydiscount" || Session["FileName"].ToString() == "ingredientcost" || Session["FileName"].ToString() == "ramaterialcost" || Session["FileName"].ToString() == "skubpcpmapping")
                        {
                            dt = ReadExcelFileForDateTime(Session["UploadedFiles"].ToString());
                        }
                        else if (Session["FileName"].ToString() == "qpsdiscount")
                        {
                            dt = ReadHeaderExcelFile(Session["UploadedFiles"].ToString());
                        }
                        else if (Session["FileName"].ToString() == "quantitylimit")
                        {
                            dt = ReadExcelFileForDateTime(Session["UploadedFiles"].ToString());
                        }
                        else
                        {
                            dt = ReadExcelFile(Session["UploadedFiles"].ToString());
                        }
                        //var data=
                        dt.TableName = Session["FileName"].ToString();
                        var fileName = Session["UploadedFiles"].ToString();

                        if (fileName != null || fileName != string.Empty)
                        {
                            if ((System.IO.File.Exists(fileName)))
                            {
                                System.IO.File.Delete(fileName);
                            }

                        }
                        var resultList = dt.ToDataSourceResult(request);
                        return new JsonResult
                        {
                            Data = resultList,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            MaxJsonLength = int.MaxValue
                        };
                    }
                    else
                    {
                        ModelState.AddModelError("FileNameNotExist", "Please upload file using excel template. Uploaded file and Template mismatch error");
                        return Json(result.AsQueryable().ToDataSourceResult(request, ModelState));
                    }
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return Json("");
        }

        public async Task<ActionResult> Products_Update([DataSourceRequest] DataSourceRequest request, string items, List<dynamic> dynamicList)
        {
            ActionResult result = null;

            try
            {
                var entity = Session["FileName"].ToString().ToLower();

                switch (entity)
                {
                    case "state":
                        result = InsertStates(request, items);
                        break;
                    case "salesorganization":
                        result = InsertSalesOrganization(request, items);
                        break;
                    case "distributionchannel":
                        result = InsertDistributionChannel(request, items);
                        break;
                    case "division":
                        result = InsertDivision(request, items);
                        break;
                    case "customergroupfive":
                        result = InsertCustomerGroupFive(request, items);
                        break;
                    case "territory":
                        result = InsertTerritories(request, items);
                        break;
                    case "district":
                        result = InsertDistricts(request, items);
                        break;
                    case "territorydistrictmapping":
                        result = InsertTerritoryDistrictMapping(request, items);
                        break;
                    case "materialcost":
                        result = InsertMaterialCost(request, items);
                        break;
                    case "city":
                        result = InsertCity(request, items);
                        break;
                    case "material":
                        result = InsertSku(request, items);
                        break;
                    case "packingcost":
                        result = InsertPackingCost(request, items);
                        break;
                    case "primaryfreight":
                        result = InsertPrimaryFreight(request, items);
                        break;
                    //case "secondaryfreight":
                    //    result = InsertSecondaryFreight(request, items);
                    //    break;
                    case "depotcost":
                        result = InsertDepotCost(request, items);
                        break;
                    case "detentioncost":
                        result = InsertDetentionCost(request, items);
                        break;
                    case "honeycombcost":
                        result = InsertHoneyCombCost(request, items);
                        break;
                    case "broker":
                        result = InsertBroker(request, items);
                        break;
                    case "usercustomersalestarget":
                        result = InsertUserCustomerSalesTarget(request, items);
                        break;
                    case "usercustomersaudatarget":
                        result = InsertUserCustomerSaudaTarget(request, items);
                        break;
                    case "profitmargin":
                        result = InsertProfitMargin(request, items);
                        break;
                    case "cushionmargin":
                        result = InsertCushionMargin(request, items);
                        break;
                    case "ramargin":
                        result = InsertRAMargin(request, items);
                        break;
                    case "schemecost":
                        result = InsertSchemeCost(request, items);
                        break;
                    case "loadcapacity":
                        result = InsertLoadCapacity(request, items);
                        break;
                    case "plant":
                        result = InsertPlants(request, items);
                        break;
                    case "depot":
                        result = InsertDepots(request, items);
                        break;
                    case "plantdepotmapping":
                        result = InsertPlantDepotMapping(request, items);
                        break;
                    case "freightzone":
                        result = InsertFreightZone(request, items);
                        break;
                    case "freightroute":
                        result = InsertFreightRoute(request, items);
                        break;
                    case "userdepotmapping":
                        result = InsertUserDepotMapping(request, items);
                        break;
                    case "usercustomermapping":
                        result = InsertUserCustomerMapping(request, items);
                        break;
                    case "oiltype":
                        result = InsertOilType(request, items);
                        break;
                    case "ingredient":
                        result = InsertIngredients(request, items);
                        break;
                    case "ingredientcost":
                        result = InsertIngredientsCost(request, items);
                        break;
                    case "retailer":
                        result = InsertRetailer(request, items);
                        break;
                    case "user":
                        result = InsertUser(request, items);
                        break;
                    case "geography":
                        result = InsertGeography(request, items);
                        break;
                    case "skuingredient":
                        result = InsertSkuIngredient(request, items);
                        break;
                    case "pendingsauda":
                        result = InsertPendingSauda(request, items);
                        break;
                    case "secondaryfreightmaster":
                        result = InsertSecondaryFreightWithZoneAndRoute(request, items);
                        break;
                    case "customermaster":
                        result = InsertCustomerMaster(request, items);
                        break;
                    case "usercustomertarget":
                        result = InsertUserCustomerTarget(request, items);
                        break;
                    case "rake":
                        result = InsertRake(request, items);
                        break;
                    //case "usersalessaudatarget":
                    //    result = InsertUserSalesSaudaTarget(request, items);
                    //    break;
                    //case "useroiltypetarget":
                    //    result = InsertUserOilTypeTarget(request, items);
                    //    break;
                    case "tradeticket":
                        result = InsertTradeTicket(request, items);
                        break;
                    case "shiptoparty":
                        result = InsertShipToParty(request, items);
                        break;
                    case "customergroup":
                        result = InsertCustomerGroup(request, items);
                        break;
                    case "percentilenumbers":
                        result = InsertPercentileNumbers(request, items);
                        break;
                    case "gst":
                        result = InsertGST(request, items);
                        break;
                    //case "vehicleloadability":
                    //    result = InsertVehicleLoadabilities(request,items);
                    //    break;
                    case "saudaconversionunitandbaseratedifference":
                        result = InsertSaudaConversionUnitAndBaseRateDifference(request, items);
                        break;
                    //case "customergroupone":
                    //    result = InsertCustomerGroupOne(request, items);
                    //    break;
                    //case "customergrouptwo":
                    //    result = InsertCustomerGroupTwo(request, items);
                    //    break;
                    case "cmsuser":
                        result = InsertCMSUsers(request, items);
                        break;
                    case "dealersaudavalidity":
                        result = UpdateDealerSaudaValidity(request, items);
                        break;
                    case "ramaterialcost":
                        result = InsertRAMaterialCost(request, items);
                        break;
                    case "dealersaudalimit":
                        result = UpdateDealerSaudaLimit(request, items);
                        break;
                    case "dealercallrecordingdetails":
                        result = await UploadDealerCallRecordingDetails(request, items);
                        break;
                    case "brokercallrecordingdetails":
                        result = UploadBrokerCallRecordingDetails(request, items);
                        break;
                    case "materialtype":
                        result = InsertMaterialType(request, items);
                        break;
                    case "volumeloadability":
                        result = InsertVolumeLoadability(request, items);
                        break;
                    case "userdivisionmapping":
                        result = InsertUserDivisionMapping(request, items);
                        break;
                    case "userdiscount":
                        result = UserDiscount(request, items);
                        break;
                    case "geographydiscount":
                        result = GeographyDiscount(request, items).Result;
                        break;
                    case "line":
                        result = InsertLine(request, items);
                        break;
                    case "qpsdiscount":
                        result = InsertQpsDiscount(request, items);
                        break;
                    case "skubpcpmapping":
                        result = InsertPackGroupMapping(request, items);
                        break;
                    case "gamificationdashboard":
                        result = InsertGamificationDashboard(request, items);
                        break;
                    case "quantitylimit":
                        result = InsertQuantityLimit(request, items);
                        break;
                    case "saudaconditionalbookingconfiguration":
                        result = InsertAndUpdateSaudaConditionalBookingConfiguration(request, items);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public ActionResult Submit(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                string filePath = GetFileInfo(files);
                if (!string.IsNullOrEmpty(filePath))
                {

                    //Session["UploadedFiles"] = GetFileInfo(files);
                    Session["UploadedFiles"] = filePath;
                    TempData["isPageLoaded"] = "fileUploaded";
                }
                else
                {
                    Session["UploadedFiles"] = null;
                }
            }
            else
            {
                Session["UploadedFiles"] = null;
            }

            return RedirectToAction("DataUpload");
        }

        #region Import Masters

        public ActionResult InsertStates(DataSourceRequest request, string items)
        {
            _methodName = "InsertStates";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<StateUploadDto> result = new List<StateUploadDto>();
            try
            {
                var stateList = Settings.DeserializeObject<StateUploadDto>(items, _dateAndNullSettings);
                foreach (var state in stateList)
                {
                    result.Add(_importClient.InsertState(state.CountryName, state.StateName, state.IsActive));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertTerritoryDistrictMapping(DataSourceRequest request, string items)
        {
            _methodName = "InsertTerritoryDistrictMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DistrictUploadDto> result = new List<DistrictUploadDto>();
            try
            {
                var districtList = Settings.DeserializeObject<DistrictUploadDto>(items, _dateAndNullSettings);
                foreach (var district in districtList)
                {
                    result.Add(_importClient.InsertTerritoryDistrictMapping(district.StateName, district.TerritoryName, district.DistrictName, district.IsActive));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertTerritories(DataSourceRequest request, string items)
        {
            _methodName = "InsertTerritories";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<TerritoryUploadDto> result = new List<TerritoryUploadDto>();
            try
            {
                var territoryList = Settings.DeserializeObject<TerritoryUploadDto>(items, _dateAndNullSettings);
                foreach (var territory in territoryList)
                {
                    result.Add(_importClient.InsertTerritory(territory.TerritoryName, territory.StateName, territory.IsActive));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertCity(DataSourceRequest request, string items)
        {
            _methodName = "InsertCity";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CityUploadDto> result = new List<CityUploadDto>();
            try
            {
                var cityList = Settings.DeserializeObject<CityUploadDto>(items, _dateAndNullSettings);
                foreach (var city in cityList)
                {
                    result.Add(_importClient.InsertCity(city.CityName, city.DistrictName, city.StateName, city.IsActive));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertDistricts(DataSourceRequest request, string items)
        {
            _methodName = "InsertDistricts";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DistrictUploadDto> result = new List<DistrictUploadDto>();
            try
            {
                var districtList = Settings.DeserializeObject<DistrictUploadDto>(items, _dateAndNullSettings);
                foreach (var district in districtList)
                {
                    result.Add(_importClient.InsertDistrict(district.DistrictName, district.StateName, district.IsActive));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertFreightZone(DataSourceRequest request, string items)
        {
            _methodName = "InsertFreightZone";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<FreightZoneUploadDto> result = new List<FreightZoneUploadDto>();
            try
            {
                var freightZones = Settings.DeserializeObject<FreightZoneUploadDto>(items, _dateAndNullSettings);
                foreach (var freightZone in freightZones)
                {
                    freightZone.CreatedBy = UserId;
                    result.Add(_importClient.InsertFreightZone(freightZone));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertFreightRoute(DataSourceRequest request, string items)
        {
            _methodName = "InsertFreightRoute";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<FreightRouteUploadDto> result = new List<FreightRouteUploadDto>();
            try
            {
                var freightRoutes = Settings.DeserializeObject<FreightRouteUploadDto>(items, _dateAndNullSettings);
                foreach (var freightRoute in freightRoutes)
                {
                    freightRoute.CreatedBy = UserId;
                    result.Add(_importClient.InsertFreightRoute(freightRoute));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertOilType(DataSourceRequest request, string items)
        {
            _methodName = "InsertOilType";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<OilTypeUploadDto> result = new List<OilTypeUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<OilTypeUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertOilType(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        public ActionResult InsertVehicleLoadabilities(DataSourceRequest request, string items)
        {
            _methodName = "InsertVehicleLoadabilities";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<VehicleLoadabilitiesDto> result = new List<VehicleLoadabilitiesDto>();
            try
            {
                var vehicleLoadabilitiesList = Settings.DeserializeObject<VehicleLoadabilitiesDto>(items, _dateAndNullSettings);
                foreach (var vehicleLoadabilities in vehicleLoadabilitiesList)
                {
                    vehicleLoadabilities.CreatedBy = UserId;
                    result.Add(_importClient.InsertVehicleLoadabilities(vehicleLoadabilities));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        #endregion

        #region Import Pricing

        public ActionResult InsertMaterialCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertMaterialCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<MaterialCostUploadDto> result = new List<MaterialCostUploadDto>();
            try
            {
                var materialCostList = Settings.DeserializeObject<MaterialCostUploadDto>(items, _dateTimeAndNullSettings);
                foreach (var input in materialCostList)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertMaterialCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertRAMaterialCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertRAMaterialCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<RAMaterialCostUploadDto> result = new List<RAMaterialCostUploadDto>();
            try
            {
                var materialCostList = Settings.DeserializeObject<RAMaterialCostUploadDto>(items, _dateTimeAndNullSettings);
                foreach (var input in materialCostList)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertRAMaterialCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertPackingCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertPackingCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<PackingCostUploadDto> result = new List<PackingCostUploadDto>();
            try
            {
                var packingResult = Settings.DeserializeObject<PackingCostUploadDto>(items, _dateTimeAndNullSettings);
                foreach (var input in packingResult)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertPackingCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertPrimaryFreight(DataSourceRequest request, string items)
        {
            _methodName = "InsertPrimaryFreight";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<PrimaryFreightUploadDto> result = new List<PrimaryFreightUploadDto>();
            try
            {
                var primaryFreight = Settings.DeserializeObject<PrimaryFreightUploadDto>(items, _dateAndNullSettings);
                foreach (var input in primaryFreight)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertPrimaryFreight(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertDepotCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertDepotCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DepotCostUploadDto> result = new List<DepotCostUploadDto>();
            try
            {
                var depotCost = Settings.DeserializeObject<DepotCostUploadDto>(items, _dateAndNullSettings);
                foreach (var input in depotCost)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertDepotCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertDetentionCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertDetentionCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DetentionCostUploadDto> result = new List<DetentionCostUploadDto>();
            try
            {
                var detentionCost = Settings.DeserializeObject<DetentionCostUploadDto>(items, _dateAndNullSettings);
                foreach (var input in detentionCost)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertDetentionCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertHoneyCombCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertHoneyCombCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<HoneyCombCostUploadDto> result = new List<HoneyCombCostUploadDto>();
            try
            {
                var honeyCombCost = Settings.DeserializeObject<HoneyCombCostUploadDto>(items, _dateAndNullSettings);
                foreach (var input in honeyCombCost)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertHoneyCombCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private ActionResult InsertProfitMargin(DataSourceRequest request, string items)
        {
            _methodName = "InsertProfitMargin";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<ProfitMarginUploadDto> result = new List<ProfitMarginUploadDto>();
            try
            {
                var profitMargin = Settings.DeserializeObject<ProfitMarginUploadDto>(items, _dateAndNullSettings);
                foreach (var input in profitMargin)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertProfitMargin(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private ActionResult InsertCushionMargin(DataSourceRequest request, string items)
        {
            _methodName = "InsertCushionMargin";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CushionMarginUploadDto> result = new List<CushionMarginUploadDto>();
            try
            {
                var cushionMargin = Settings.DeserializeObject<CushionMarginUploadDto>(items, _dateAndNullSettings);
                foreach (var input in cushionMargin)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertCushionMargin(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private ActionResult InsertRAMargin(DataSourceRequest request, string items)
        {
            _methodName = "InsertRAMargin";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<RAMarginUploadDto> result = new List<RAMarginUploadDto>();
            try
            {
                var rAMargin = Settings.DeserializeObject<RAMarginUploadDto>(items, _dateAndNullSettings);
                foreach (var input in rAMargin)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertRAMargin(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private ActionResult InsertSchemeCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertSchemeCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SchemeCostUploadDto> result = new List<SchemeCostUploadDto>();
            try
            {
                var schemeCost = Settings.DeserializeObject<SchemeCostUploadDto>(items, _dateAndNullSettings);
                foreach (var input in schemeCost)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertSchemeCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertLoadCapacity(DataSourceRequest request, string items)
        {
            _methodName = "InsertLoadCapacity";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<LoadCapacityConversionUploadDto> result = new List<LoadCapacityConversionUploadDto>();
            try
            {
                var loadCapacity = Settings.DeserializeObject<LoadCapacityConversionUploadDto>(items, _dateAndNullSettings);
                foreach (var input in loadCapacity)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertLoadCapacity(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertIngredients(DataSourceRequest request, string items)
        {
            _methodName = "InsertIngredients";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<IngredientsUploadDto> result = new List<IngredientsUploadDto>();
            try
            {
                var ingredients = Settings.DeserializeObject<IngredientsUploadDto>(items, _dateAndNullSettings);
                foreach (var ingredient in ingredients)
                {
                    result.Add(_importClient.InsertIngredients(ingredient));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertIngredientsCost(DataSourceRequest request, string items)
        {
            _methodName = "InsertIngredientsCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<IngredientCostUploadDto> result = new List<IngredientCostUploadDto>();
            try
            {
                var ingredientCostList = Settings.DeserializeObject<IngredientCostUploadDto>(items, _dateTimeAndNullSettings);
                foreach (var input in ingredientCostList)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertIngredientsCost(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertSkuIngredient(DataSourceRequest request, string items)
        {
            _methodName = "InsertSkuIngredient";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SkuIngredientUploadDto> result = new List<SkuIngredientUploadDto>();
            try
            {
                var ingredients = Settings.DeserializeObject<SkuIngredientUploadDto>(items, _dateAndNullSettings);
                foreach (var ingredient in ingredients)
                {
                    ingredient.CreatedBy = UserId;
                    result.Add(_importClient.InsertSkuIngredient(ingredient));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Sku

        public ActionResult InsertSku(DataSourceRequest request, string items)
        {
            _methodName = "InsertSku";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SkuUploadDto> result = new List<SkuUploadDto>();
            try
            {
                var skuResult = Settings.DeserializeObject<SkuUploadDto>(items, _dateAndNullSettings);
                foreach (var input in skuResult)
                {
                    input.CreatedBy = UserId;
                    input.SapStatusId = 0;
                    result.Add(_importClient.InsertSku(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Broker

        public ActionResult InsertBroker(DataSourceRequest request, string items)
        {
            _methodName = "InsertBroker";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<BrokerUploadDto> result = new List<BrokerUploadDto>();
            try
            {
                var brokerUpload = Settings.DeserializeObject<BrokerUploadDto>(items, _dateAndNullSettings);
                foreach (var input in brokerUpload)
                {
                    input.EncryptedPassword = string.IsNullOrEmpty(input.Password) ? input.Password : UtilityHelper.ConvertToMd5(input.Password, SecurityConstants.EncryptionKey);
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertBroker(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Targets

        public ActionResult InsertUserCustomerTarget(DataSourceRequest request, string items)
        {
            _methodName = "InsertUserCustomerSalesTargets";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<UserCustomerSalesTargetUploadDto> result = new List<UserCustomerSalesTargetUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<UserCustomerSalesTargetUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.UserId = UserId;
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertUserCustomerTarget(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertUserCustomerSalesTarget(DataSourceRequest request, string items)
        {
            _methodName = "InsertUserCustomerSalesTargets";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<UserCustomerSalesTargetUploadDto> result = new List<UserCustomerSalesTargetUploadDto>();
            try
            {
                var userCustomer = Settings.DeserializeObject<UserCustomerSalesTargetUploadDto>(items, _dateAndNullSettings);
                foreach (var input in userCustomer)
                {
                    input.UserId = UserId;
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertUserCustomerSalesTarget(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertUserCustomerSaudaTarget(DataSourceRequest request, string items)
        {
            _methodName = "InsertUserCustomerSaudaTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<UserCustomerSaudaTargetUploadDto> result = new List<UserCustomerSaudaTargetUploadDto>();
            try
            {
                var userCustomerSaudaTarget = Settings.DeserializeObject<UserCustomerSaudaTargetUploadDto>(items, _dateAndNullSettings);
                foreach (var input in userCustomerSaudaTarget)
                {
                    input.UserId = UserId;
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertUserCustomerSaudaTarget(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //public ActionResult InsertUserSalesSaudaTarget(DataSourceRequest request, string items)
        //{
        //    _methodName = "InsertUserSalesSaudaTarget";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    List<UserSalesSaudaTargetUploadDto> result = new List<UserSalesSaudaTargetUploadDto>();
        //    try
        //    {
        //        var base64EncodedBytes = System.Convert.FromBase64String(items);
        //        var json = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        //        var inputs = JsonConvert.DeserializeObject<List<UserSalesSaudaTargetUploadDto>>(json);
        //        long createdBy = UserId;
        //        foreach (var input in inputs)
        //        {
        //            input.UserId = UserId;
        //            input.CreatedBy = UserId;
        //            result.Add(_importClient.InsertUserSalesSaudaTarget(input));
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //    }
        //    return Json(result.ToDataSourceResult(request));
        //}

        //public ActionResult InsertUserOilTypeTarget(DataSourceRequest request, string items)
        //{
        //    _methodName = "InsertUserOilTypeTarget";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    List<UserOilTypeTargetUploadDto> result = new List<UserOilTypeTargetUploadDto>();
        //    try
        //    {
        //        var base64EncodedBytes = System.Convert.FromBase64String(items);
        //        var json = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        //        var inputs = JsonConvert.DeserializeObject<List<UserOilTypeTargetUploadDto>>(json);
        //        long createdBy = UserId;
        //        foreach (var input in inputs)
        //        {
        //            input.UserId = UserId;
        //            input.CreatedBy = UserId;
        //            result.Add(_importClient.InsertUserOilTypeTarget(input));
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //    }
        //    return Json(result.ToDataSourceResult(request));
        //}

        #endregion

        #region Import Plant & Depot

        public ActionResult InsertPlants(DataSourceRequest request, string items)
        {
            _methodName = "InsertPlants";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<PlantUploadDto>();
            try
            {
                var plantUpload = Settings.DeserializeObject<PlantUploadDto>(items, _dateAndNullSettings);
                foreach (var plant in plantUpload)
                {
                    plant.CreatedBy = UserId;
                    plant.StorageTypeId = (int)StorageType.Plant;
                    result.Add(_importClient.InsertPlants(plant));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertDepots(DataSourceRequest request, string items)
        {
            _methodName = "InsertDepots";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<PlantUploadDto>();
            try
            {
                var depotUpload = Settings.DeserializeObject<PlantUploadDto>(items, _dateAndNullSettings);
                foreach (var depot in depotUpload)
                {
                    depot.CreatedBy = UserId;
                    depot.StorageTypeId = (int)StorageType.Depot;
                    result.Add(_importClient.InsertDepots(depot));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertRake(DataSourceRequest request, string items)
        {
            _methodName = "InsertRaks";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<RakeUploadDto>();
            try
            {
                var rakeUpload = Settings.DeserializeObject<RakeUploadDto>(items, _dateAndNullSettings);
                foreach (var rake in rakeUpload)
                {
                    //rake.MappedStateNames = new List<string>() { "Uttaranchal", "Goa" };
                    rake.CreatedBy = UserId;
                    rake.StorageTypeId = (int)StorageType.Rake;
                    result.Add(_importClient.InsertRake(rake));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertPlantDepotMapping(DataSourceRequest request, string items)
        {
            _methodName = "InsertDepots";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<PlantDepotMappingUploadDto>();
            try
            {
                var plantDepotMappingUpload = Settings.DeserializeObject<PlantDepotMappingUploadDto>(items, _dateAndNullSettings);
                foreach (var item in plantDepotMappingUpload)
                {
                    result.Add(_importClient.InsertPlantDepotMapping(item.PlantCode, item.DepotCode, UserId));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertUserDepotMapping(DataSourceRequest request, string items)
        {
            _methodName = "InsertUserDepotMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<UserDepotMappingUploadDto>();
            try
            {
                var userDepotMapping = Settings.DeserializeObject<UserDepotMappingUploadDto>(items, _dateAndNullSettings);
                foreach (var item in userDepotMapping)
                {
                    result.Add(_importClient.InsertUserDepotMapping(item));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult InsertUserCustomerMapping(DataSourceRequest request, string items)
        {
            _methodName = "InsertUserCustomerMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<UserCustomerMappingUploadDto>();
            try
            {
                var userCustomerMappings = Settings.DeserializeObject<UserCustomerMappingUploadDto>(items, _dateAndNullSettings);
                foreach (var item in userCustomerMappings)
                {
                    item.CreatedBy = UserId;
                    result.Add(_importClient.InsertUserCustomerMapping(item));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Retailer

        public ActionResult InsertRetailer(DataSourceRequest request, string items)
        {
            _methodName = "InsertRetailer";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<RetailerUploadDto> result = new List<RetailerUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<RetailerUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertRetailer(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import User     

        public ActionResult InsertUser(DataSourceRequest request, string items)
        {
            _methodName = "InsertUser";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<UserUploadDto> result = new List<UserUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<UserUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.EncryptedPassword = string.IsNullOrEmpty(input.Password) ? input.Password : UtilityHelper.ConvertToMd5(input.Password, SecurityConstants.EncryptionKey);
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertUser(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Geography - State Territory District City

        public ActionResult InsertGeography(DataSourceRequest request, string items)
        {
            _methodName = "InsertGeography";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<GeographyUploadDto> result = new List<GeographyUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<GeographyUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CountryName = "India";
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertGeography(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Pending Sauda      

        private ActionResult InsertPendingSauda(DataSourceRequest request, string items)
        {
            _methodName = "InsertPendingSauda";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<PendingSaudaUploadDto> result = new List<PendingSaudaUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<PendingSaudaUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertPendingSauda(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Secondary Freight With FreightZone And FreightRoute

        public ActionResult InsertSecondaryFreightWithZoneAndRoute(DataSourceRequest request, string items)
        {
            _methodName = "InsertSecondaryFreightWithZoneAndRoute";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SecondaryFreightUploadDto> result = new List<SecondaryFreightUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<SecondaryFreightUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertSecondaryFreightMaster(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Customer Master With Depot and Customer Mapping

        public ActionResult InsertCustomerMaster(DataSourceRequest request, string items)
        {
            _methodName = "InsertCustomerMaster";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DealerUploadDto> result = new List<DealerUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<DealerUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.EncryptedPassword = string.IsNullOrEmpty(input.Password) ? input.Password : UtilityHelper.ConvertToMd5(input.Password, SecurityConstants.EncryptionKey);
                    input.CreatedBy = UserId;
                    input.RoleId = (int)DTO.Enums.Role.Dealer;
                    result.Add(_importClient.InsertCustomerMaster(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Private Methods

        private string GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            var filePath = string.Empty;
            _methodName = "GetFileInfo";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var entity = files.FirstOrDefault().FileName.ToLower();
                var fileInfo = new FileInfo(entity);
                Session["FileName"] = entity.Replace(fileInfo.Extension, "");
                filePath = Server.MapPath("/App_Data/") + Guid.NewGuid() + ".xlsx";
                files.SingleOrDefault().SaveAs(filePath);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return filePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        public DataTable ReadExcelFile(string path, bool hasHeader = true)
        {
            DataTable tbl = new DataTable();
            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }

                    List<char> dateColumns = new List<char>();
                    List<char> imageColumns = new List<char>();
                    var ws = pck.Workbook.Worksheets.First();


                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        if (CheckDate(firstRowCell.Text))
                        {
                            dateColumns.Add(firstRowCell.ToString().ToCharArray()[0]);
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        else
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                    }

                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            if (dateColumns.Any(dc => dc.Equals(cell.ToString().ToCharArray()[0])))
                            {
                                cell.Style.Numberformat.Format = Settings.DateFormatForImportData;
                            }

                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    tbl = tbl.Rows
                                .Cast<DataRow>()
                                .Where(row => !row.ItemArray.All(field => field is DBNull ||
                                         string.IsNullOrWhiteSpace(field as string)))
                                .CopyToDataTable();
                }
            }
            catch (Exception exception)
            {
                DataTable emptyTable = new DataTable();

                emptyTable.Columns.Add(string.Format("{0}", "Message"));
                DataRow row = emptyTable.NewRow(); //emptyTable.Rows.Add(); //emptyTable.Rows.Add("Invalid file format");
                row[0] = "Invalid file format";
                emptyTable.Rows.Add(row);

                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return emptyTable;
            }
            return tbl;
        }

        public DataTable ReadExcelFileForDateTime(string path, bool hasHeader = true)
        {
            DataTable tbl = new DataTable();
            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }

                    List<char> dateColumns = new List<char>();
                    var ws = pck.Workbook.Worksheets.First();

                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        if (CheckDate(firstRowCell.Text))
                        {
                            dateColumns.Add(firstRowCell.ToString().ToCharArray()[0]);
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        else
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            if (dateColumns.Any(dc => dc.Equals(cell.ToString().ToCharArray()[0])))
                            {
                                cell.Style.Numberformat.Format = Settings.DateTimeFormatForImportData;
                            }
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    tbl = tbl.Rows
                                .Cast<DataRow>()
                                .Where(row => !row.ItemArray.All(field => field is DBNull ||
                                         string.IsNullOrWhiteSpace(field as string)))
                                .CopyToDataTable();

                }
            }
            catch (Exception exception)
            {
                DataTable emptyTable = new DataTable();

                emptyTable.Columns.Add(string.Format("{0}", "Message"));
                DataRow row = emptyTable.NewRow(); //emptyTable.Rows.Add(); //emptyTable.Rows.Add("Invalid file format");
                row[0] = "Invalid file format";
                emptyTable.Rows.Add(row);

                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return emptyTable;
            }
            return tbl;
        }

        public DataTable ReadHeaderExcelFile(string path, bool hasHeader = true)
        {
            DataTable tbl = new DataTable();
            var qpsDiscount = new List<QPSDiscountImportDto>();
            var act_qpsDiscount = new List<QPSDiscountImportDto>();

            try
            {
                FileInfo File = new FileInfo(path);

                using (var pck = new OfficeOpenXml.ExcelPackage(File))
                {
                    var ws = pck.Workbook.Worksheets.First();
                    int totalRows = ws.Dimension.End.Row;
                    int totalColumns = ws.Dimension.End.Column;

                    var StartDate = "";
                    var EndDate = "";
                    var SalesOrgId = "";
                    var DistributionChannelId = "";
                    var DivisionId = "";
                    var OilTypeName = "";
                    //var SkuName = "";
                    var SkuCode = "";
                    var ZoneName = "";
                    var StateName = "";
                    var SlabCount = "";
                    var FromRange = "";
                    var ToRange = "";
                    var Discount = "";
                    int Count = 0;
                    int iterator = 0;
                    int Parentindex = 1;
                    int QpsRowId = 1;


                    //for (int row = 1; row <= totalRows; row++)
                    for (var rowIterator = 2; rowIterator <= totalRows; rowIterator = rowIterator + 0)
                    {
                        if (ws.Cells[rowIterator, 1].Value == null && ws.Cells[rowIterator, 2].Value == null && ws.Cells[rowIterator, 3].Value == null && ws.Cells[rowIterator, 4].Value == null && ws.Cells[rowIterator, 5].Value == null && ws.Cells[rowIterator, 6].Value == null && ws.Cells[rowIterator, 8].Value == null && ws.Cells[rowIterator, 9].Value == null && ws.Cells[rowIterator, 10].Value == null)
                        {
                            rowIterator++;
                            continue;
                        }
                        StartDate = ws.Cells[rowIterator, 1].Value != null ? ws.Cells[rowIterator, 1].Value.ToString() : string.Empty;
                        EndDate = ws.Cells[rowIterator, 2].Value != null ? ws.Cells[rowIterator, 2].Value.ToString() : string.Empty;
                        SalesOrgId = ws.Cells[rowIterator, 3].Value != null ? ws.Cells[rowIterator, 3].Value.ToString() : string.Empty;
                        DistributionChannelId = ws.Cells[rowIterator, 4].Value != null ? ws.Cells[rowIterator, 4].Value.ToString() : string.Empty;
                        DivisionId = ws.Cells[rowIterator, 5].Value != null ? ws.Cells[rowIterator, 5].Value.ToString() : string.Empty;
                        OilTypeName = ws.Cells[rowIterator, 6].Value != null ? ws.Cells[rowIterator, 6].Value.ToString() : string.Empty;
                        //SkuName = ws.Cells[rowIterator, 7].Value != null ? ws.Cells[rowIterator, 7].Value.ToString() : string.Empty;
                        SkuCode = ws.Cells[rowIterator, 7].Value != null ? ws.Cells[rowIterator, 7].Value.ToString() : string.Empty;
                        ZoneName = ws.Cells[rowIterator, 8].Value != null ? ws.Cells[rowIterator, 8].Value.ToString() : string.Empty;
                        StateName = ws.Cells[rowIterator, 9].Value != null ? ws.Cells[rowIterator, 9].Value.ToString() : string.Empty;
                        SlabCount = ws.Cells[rowIterator, 10].Value != null ? ws.Cells[rowIterator, 10].Value.ToString() : string.Empty;
                        //FromRange = ws.Cells[rowIterator, 11].Value != null ? ws.Cells[rowIterator, 11].Value.ToString() : string.Empty;
                        //ToRange = ws.Cells[rowIterator, 12].Value != null ? ws.Cells[rowIterator, 12].Value.ToString() : string.Empty;
                        //Discount = ws.Cells[rowIterator, 13].Value != null ? ws.Cells[rowIterator, 13].Value.ToString() : string.Empty;
                        Count = Convert.ToInt32(SlabCount);
                        iterator = rowIterator + Count;
                        for (rowIterator = rowIterator; rowIterator <= iterator - 1; rowIterator++)
                        {
                            var stock = new QPSDiscountImportDto
                            {
                                QpsParentId = Parentindex,
                                StartDate = StartDate,
                                EndDate = EndDate,
                                SalesOrgCode = Convert.ToInt64(SalesOrgId),
                                DistributionChannelCode = Convert.ToInt64(DistributionChannelId),
                                DivisionCode = Convert.ToInt64(DivisionId),
                                OilTypeName = OilTypeName,
                                //SkuName = SkuName,
                                SkuCode = SkuCode,
                                ZoneName = ZoneName,
                                StateName = StateName,
                                SlabCount = Convert.ToInt64(SlabCount),
                                FromRange = Convert.ToInt64(ws.Cells[rowIterator, 11].Value != null ? ws.Cells[rowIterator, 11].Value.ToString() : string.Empty),
                                ToRange = Convert.ToInt64(ws.Cells[rowIterator, 12].Value != null ? ws.Cells[rowIterator, 12].Value.ToString() : string.Empty),
                                Discount = Convert.ToDecimal(ws.Cells[rowIterator, 13].Value != null ? ws.Cells[rowIterator, 13].Value.ToString() : string.Empty)
                                //QpsRowId = QpsRowId
                            };
                            qpsDiscount.Add(stock);
                        }
                        Parentindex++;
                    }
                    foreach (var item in qpsDiscount)
                    {
                        List<string> oilTypeNames = item.OilTypeName.Split(',').ToList();
                        //List<string> skuNames = item.SkuName.Split(',').ToList();
                        List<string> skuCodes = item.SkuCode.Split(',').ToList();
                        List<string> zoneNames = item.ZoneName.Split(',').ToList();
                        List<string> stateNames = item.StateName.Split(',').ToList();
                        List<QPSOilTypeListDto> oilTypeCombinations = new List<QPSOilTypeListDto>();

                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            var parameters = new
                            {
                                OilTypeNames = oilTypeNames,
                                //SkuNames = skuNames,
                                SkuCodes = skuCodes,
                                //SkuNames = skuNames != null ? string.Join(",", skuNames) : null,
                                StateNames = stateNames,
                                SalesOrgCode = item.SalesOrgCode,
                                DistributionChannelCode = item.DistributionChannelCode,
                                DivisionCode = item.DivisionCode,
                                ZoneName = zoneNames
                            };
                            string validationQuery = @"SELECT COUNT(*)
                                                        FROM OilTypes o
                                                        JOIN SalesOrganizations sa ON sa.Id = o.SalesOrganizationId
                                                        JOIN DistributionChannels db ON db.Id = o.DistributionChannelId
                                                        JOIN Divisions ds ON ds.Id = o.DivisionId
                                                        WHERE o.Name IN @OilTypeNames 
                                                          AND sa.Code = @SalesOrgCode 
                                                          AND db.Code = @DistributionChannelCode 
                                                          AND ds.Code = @DivisionCode
                                                          AND o.IsActive = 1";

                            int matchCount = connection.QuerySingle<int>(validationQuery, parameters);

                            if (matchCount == 0)
                            {
                                DataTable emptyTable = new DataTable();

                                emptyTable.Columns.Add(string.Format("{0}", "Message"));
                                DataRow row = emptyTable.NewRow();
                                row[0] = "Invalid Combination";
                                emptyTable.Rows.Add(row);
                                return emptyTable;
                            }

                            string validationQuery1 = @"SELECT COUNT(*)
                                                        FROM ZoneStateMappings zs
                                                        JOIN Zones z ON z.Id = zs.ZoneId
                                                        Join States s ON zs.StateId = s.Id
                                                        WHERE z.Name IN @ZoneName 
                                                          AND s.StateName = @StateNames";
                            int Zonestate = connection.QuerySingle<int>(validationQuery1, parameters);

                            if (Zonestate == 0)
                            {
                                DataTable emptyTable = new DataTable();

                                emptyTable.Columns.Add(string.Format("{0}", "Message"));
                                DataRow row = emptyTable.NewRow();
                                row[0] = "Invalid Zone Or State";
                                emptyTable.Rows.Add(row);
                                return emptyTable;
                            }
                            //string query = @"SELECT sa.Id AS SalesOrgId,db.Id AS DistributionChannelId,ds.Id AS DivisionId, o.Id AS OilTypeId,o.Name AS OilTypeName, s.Id AS SkuId,s.SkuName AS SkuName,st.Id AS StateId,st.StateName AS StateName,zs.ZoneId AS ZoneId,z.Name AS ZoneName
                            //               FROM OilTypes o
                            //               Join SalesOrganizations sa ON sa.Id = o.SalesOrganizationId
                            //      Join DistributionChannels db ON db.Id = o.DistributionChannelId
                            //      Join Divisions ds ON ds.Id = o.DivisionId
                            //               LEFT JOIN Skus s ON o.Id = s.OilTypeId AND s.SkuName IN @SkuNames AND s.IsActive = 1
                            //               INNER JOIN States st ON st.StateName IN @StateNames AND St.IsActive = 1
                            //               JOIN ZoneStateMappings zs ON st.Id = zs.StateId 
                            //               JOIN Zones z ON z.Id = zs.ZoneId
                            //               WHERE o.Name IN @OilTypeNames AND o.IsActive = 1 
                            //               AND sa.Code = @SalesOrgCode AND db.Code = @DistributionChannelCode
                            //               AND ds.Code = @DivisionCode";


                            string query = @"SELECT sa.Id AS SalesOrgId, db.Id AS DistributionChannelId, ds.Id AS DivisionId, 
                                                   o.Id AS OilTypeId, o.Name AS OilTypeName, s.Id AS SkuId, s.SkuCode AS SkuCode, 
                                                   st.Id AS StateId, st.StateName AS StateName, zs.ZoneId AS ZoneId, z.Name AS ZoneName
                                            FROM OilTypes o
                                            JOIN SalesOrganizations sa ON sa.Id = o.SalesOrganizationId
                                            JOIN DistributionChannels db ON db.Id = o.DistributionChannelId
                                            JOIN Divisions ds ON ds.Id = o.DivisionId
                                            LEFT JOIN Skus s ON o.Id = s.OilTypeId AND s.SkuCode IN @skuCodes AND s.IsActive = 1
                                            INNER JOIN States st ON st.StateName IN @StateNames AND st.IsActive = 1
                                            JOIN ZoneStateMappings zs ON st.Id = zs.StateId 
                                            JOIN Zones z ON z.Id = zs.ZoneId
                                            WHERE o.Name IN @OilTypeNames AND o.IsActive = 1 
                                            AND sa.Code = @SalesOrgCode AND db.Code = @DistributionChannelCode
                                            AND ds.Code = @DivisionCode";

                            oilTypeCombinations = connection.QueryAsync<QPSOilTypeListDto>(query, parameters).Result.ToList();
                        }
                        //var combinations = GetCombinations(skuNames, zoneNames, stateNames);
                        foreach (var combitem in oilTypeCombinations)
                        {
                            var stock = new QPSDiscountImportDto
                            {
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                SalesOrgCode = item.SalesOrgCode,
                                DistributionChannelCode = item.DistributionChannelCode,
                                DivisionCode = item.DivisionCode,
                                OilTypeId = combitem.OilTypeId,
                                OilTypeName = combitem.OilTypeName,
                                SkuId = combitem.SkuId,
                                //SkuName =combitem.SkuName, 
                                SkuCode = combitem.SkuCode,
                                StateName = combitem.StateName,
                                ZoneName = combitem.ZoneName,
                                ZoneId = combitem.ZoneId,
                                StateId = combitem.StateId,
                                SlabCount = item.SlabCount,
                                FromRange = item.FromRange,
                                ToRange = item.ToRange,
                                Discount = item.Discount,
                                QpsParentId = item.QpsParentId,
                                QpsRowId = QpsRowId
                            };
                            act_qpsDiscount.Add(stock);
                            QpsRowId++;

                        }
                    }

                    tbl = ToDataTable(act_qpsDiscount);
                }
            }
            catch (Exception exception)
            {
                DataTable emptyTable = new DataTable();

                emptyTable.Columns.Add(string.Format("{0}", "Message"));
                DataRow row = emptyTable.NewRow();
                row[0] = "Enter all fields before importing";
                emptyTable.Rows.Add(row);

                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return emptyTable;
            }
            return tbl;
        }

        static List<QPSDiscountImportDto> GetCombinations(List<string> oilTypeNames, List<string> skuNames, List<string> stateNames)
        {
            var combinations = new List<QPSDiscountImportDto>();

            if (skuNames == null || !skuNames.Any())
            {
                skuNames = new List<string> { "" };
            }
            foreach (var oilTypeName in oilTypeNames)
            {
                foreach (var skuName in skuNames)
                {
                    foreach (var stateName in stateNames)
                    {
                        combinations.Add(new QPSDiscountImportDto
                        {
                            OilTypeName = oilTypeName,
                            //SkuName = skuName,
                            StateName = stateName
                        });
                    }
                }
            }

            return combinations;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected static bool CheckDate(String date)
        {
            string[] columnNames = Settings.DateColumns;
            return columnNames.Any(c => c.ToLower().Trim().Equals(date.ToLower().Trim()));
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #endregion

        #region Import TradeTicket


        public ActionResult InsertTradeTicket(DataSourceRequest request, string items)
        {
            _methodName = "InsertTradeTicket";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<TradeTicketUploadDto> result = new List<TradeTicketUploadDto>();
            try
            {
                var tradeTickets = Settings.DeserializeObject<TradeTicketUploadDto>(items, _dateAndNullSettings);
                foreach (var tradeTicket in tradeTickets)
                {
                    tradeTicket.CreatedBy = UserId;
                    result.Add(_importClient.InsertTradeTicket(tradeTicket));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import ShipToParty

        public ActionResult InsertShipToParty(DataSourceRequest request, string items)
        {
            _methodName = "InsertShipToParty";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<ShipToPartyUploadDto> result = new List<ShipToPartyUploadDto>();
            try
            {
                var dealerUpload = Settings.DeserializeObject<ShipToPartyUploadDto>(items, _dateAndNullSettings);
                foreach (var input in dealerUpload)
                {
                    if (string.IsNullOrEmpty(input.PlantTruckCapacity))
                        input.PlantTruckCapacity = "0";
                    if (string.IsNullOrEmpty(input.DepotTruckCapacity))
                        input.DepotTruckCapacity = "0";
                    if (string.IsNullOrEmpty(input.IsActive))
                        input.IsActive = "0";
                    input.EncryptedPassword = string.IsNullOrEmpty(input.Password) ? input.Password : UtilityHelper.ConvertToMd5(input.Password, SecurityConstants.EncryptionKey);
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertShipToParty(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import CustomerGroup

        public ActionResult InsertCustomerGroup(DataSourceRequest request, string items)
        {
            _methodName = "InsertCustomerGroup";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CustomerGroupUploadDto> result = new List<CustomerGroupUploadDto>();
            try
            {
                var dataUpload = Settings.DeserializeObject<CustomerGroupUploadDto>(items, _dateAndNullSettings);
                foreach (var input in dataUpload)
                {
                    if (string.IsNullOrEmpty(input.IsActive))
                        input.IsActive = "0";
                    if (string.IsNullOrEmpty(input.IsBaseGroup))
                        input.IsBaseGroup = "0";
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertCustomerGroup(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Percentile Numbers
        public ActionResult InsertPercentileNumbers(DataSourceRequest request, string items)
        {
            _methodName = "InsertPercentileNumbers";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<PercentileNumbersUploadDto>();
            try
            {
                var depotUpload = Settings.DeserializeObject<PercentileNumbersUploadDto>(items, _dateAndNullSettings);
                foreach (var depot in depotUpload)
                {
                    depot.CreatedBy = UserId;
                    //result.Add(_importClient.InsertDepots(depot));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Import GST

        public ActionResult InsertGST(DataSourceRequest request, string items)
        {
            _methodName = "InsertGST";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<GSTUploadDto>();
            try
            {
                var gstUpload = Settings.DeserializeObject<GSTUploadDto>(items, _dateAndNullSettings);
                result = _importClient.InsertGSTNew(gstUpload);
                //foreach (var gst in gstUpload)
                //{
                //    gst.CreatedBy = UserId;
                //    result.Add(_importClient.InsertGST(gst));
                //}
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import CustomerGroupOne
        public ActionResult InsertCustomerGroupOne(DataSourceRequest request, string items)
        {
            _methodName = "InsertCustomerGroupOne";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CustomerGroupOneAndTwoUploadDto> result = new List<CustomerGroupOneAndTwoUploadDto>();
            try
            {
                var customerGroupOneList = Settings.DeserializeObject<CustomerGroupOneAndTwoUploadDto>(items, _dateAndNullSettings);
                foreach (var data in customerGroupOneList)
                {
                    data.LoginUserId = UserId;
                    result.Add(_importClient.InsertCustomerGroupOne(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Import CustomerGroupTwo
        public ActionResult InsertCustomerGroupTwo(DataSourceRequest request, string items)
        {
            _methodName = "InsertCustomerGroupTwo";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CustomerGroupOneAndTwoUploadDto> result = new List<CustomerGroupOneAndTwoUploadDto>();
            try
            {
                var customerGroupTwoList = Settings.DeserializeObject<CustomerGroupOneAndTwoUploadDto>(items, _dateAndNullSettings);
                foreach (var data in customerGroupTwoList)
                {
                    data.LoginUserId = UserId;
                    result.Add(_importClient.InsertCustomerGroupTwo(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import CustomerGroupFive
        public ActionResult InsertCustomerGroupFive(DataSourceRequest request, string items)
        {
            _methodName = "InsertCustomerGroupFive";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CustomerGroupFiveUploadDto> result = new List<CustomerGroupFiveUploadDto>();
            try
            {
                var customerGroupTwoList = Settings.DeserializeObject<CustomerGroupFiveUploadDto>(items, _dateAndNullSettings);
                foreach (var data in customerGroupTwoList)
                {
                    data.LoginUserId = UserId;
                    result.Add(_importClient.InsertCustomerGroupFive(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion


        #region Import SalesOrganization
        public ActionResult InsertSalesOrganization(DataSourceRequest request, string items)
        {
            _methodName = "InsertSalesOrganization";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SalesOrganizationUploadDto> result = new List<SalesOrganizationUploadDto>();
            try
            {
                var customerGroupTwoList = Settings.DeserializeObject<SalesOrganizationUploadDto>(items, _dateAndNullSettings);
                foreach (var data in customerGroupTwoList)
                {
                    data.LoginUserId = UserId;
                    result.Add(_importClient.InsertSalesOrganization(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import DistributionChannel
        public ActionResult InsertDistributionChannel(DataSourceRequest request, string items)
        {
            _methodName = "InsertDistributionChannel";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DistributionChannelUploadDto> result = new List<DistributionChannelUploadDto>();
            try
            {
                var customerGroupTwoList = Settings.DeserializeObject<DistributionChannelUploadDto>(items, _dateAndNullSettings);
                foreach (var data in customerGroupTwoList)
                {
                    data.LoginUserId = UserId;
                    result.Add(_importClient.InsertDistributionChannel(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import Division
        public ActionResult InsertDivision(DataSourceRequest request, string items)
        {
            _methodName = "InsertDivision";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DivisionUploadDto> result = new List<DivisionUploadDto>();
            try
            {
                var customerGroupTwoList = Settings.DeserializeObject<DivisionUploadDto>(items, _dateAndNullSettings);
                foreach (var data in customerGroupTwoList)
                {
                    data.LoginUserId = UserId;
                    result.Add(_importClient.InsertDivision(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion


        #region Import SaudaConversionUnitAndBaseRateDifference

        public ActionResult InsertSaudaConversionUnitAndBaseRateDifference(DataSourceRequest request, string items)
        {
            _methodName = "InsertSaudaConversionUnitAndBaseRateDifference";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SaudaConversionUnitAndDiffRateUploadDto> result = new List<SaudaConversionUnitAndDiffRateUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<SaudaConversionUnitAndDiffRateUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertSaudaConversionUnitAndBaseRateDifference(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Insert CMSUsers

        public ActionResult InsertCMSUsers(DataSourceRequest request, string items)
        {
            _methodName = "InsertCMSUsers";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<UserUploadDto> result = new List<UserUploadDto>();
            try
            {
                var cmsusers = Settings.DeserializeObject<UserUploadDto>(items, _dateAndNullSettings);
                foreach (var data in cmsusers)
                {
                    data.CreatedBy = UserId;
                    data.EncryptedPassword = string.IsNullOrEmpty(data.Password) ? data.Password : UtilityHelper.ConvertToMd5(data.Password, SecurityConstants.EncryptionKey);
                    result.Add(_importClient.InsertCMSUser(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region UpdateDealerSaudaValidityAndSaudaLimit

        public ActionResult UpdateDealerSaudaValidity(DataSourceRequest request, string items)
        {
            _methodName = "UpdateDealerSaudaValidity";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DealerSaudaValidityUpdateDto> result = new List<DealerSaudaValidityUpdateDto>();
            try
            {
                var dealersaudavalidity = Settings.DeserializeObject<DealerSaudaValidityUpdateDto>(items, _dateAndNullSettings);
                foreach (var data in dealersaudavalidity)
                {
                    data.ModifiedBy = UserId;
                    result.Add(_importClient.UpdateDealerSaudaValidity(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult UpdateDealerSaudaLimit(DataSourceRequest request, string items)
        {
            _methodName = "UpdateDealerSaudaLimit";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DealerSaudaValidityUpdateDto> result = new List<DealerSaudaValidityUpdateDto>();
            try
            {
                var dealersaudavalidity = Settings.DeserializeObject<DealerSaudaValidityUpdateDto>(items, _dateAndNullSettings);
                foreach (var data in dealersaudavalidity)
                {
                    data.ModifiedBy = UserId;
                    result.Add(_importClient.UpdateDealerSaudaLimit(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        public async Task<ActionResult> UploadDealerCallRecordingDetails(DataSourceRequest request, string items)
        {
            _methodName = "UploadDealerCallRecordingDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            IList<DealerConsentImageUploadDto> result = new List<DealerConsentImageUploadDto>();
            try
            {
                var dealerdetails = Settings.DeserializeObject<DealerConsentImageUploadDto>(items, _dateAndNullSettings);
                foreach (var data in dealerdetails)
                {
                    data.ModifiedBy = UserId;
                }
                result = await _importClient.UploadDealerConsentImage(dealerdetails);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        public ActionResult UploadBrokerCallRecordingDetails(DataSourceRequest request, string items)
        {
            _methodName = "UploadBrokerCallRecordingDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            IList<DealerConsentImageUploadDto> result = new List<DealerConsentImageUploadDto>();
            try
            {
                var brokerdetails = Settings.DeserializeObject<DealerConsentImageUploadDto>(items, _dateAndNullSettings);
                foreach (var data in brokerdetails)
                {
                    data.ModifiedBy = UserId;
                    result.Add(_importClient.UploadBrokerCallRecordingDetails(data));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import MaterialType
        public ActionResult InsertMaterialType(DataSourceRequest request, string items)
        {
            _methodName = "InsertMaterialType";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<MaterialTypeUploadDto> result = new List<MaterialTypeUploadDto>();
            try
            {
                var materialtypeResult = Settings.DeserializeObject<MaterialTypeUploadDto>(items, _dateAndNullSettings);
                foreach (var input in materialtypeResult)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertMaterialType(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Volume Loadability
        public ActionResult InsertVolumeLoadability(DataSourceRequest request, string items)
        {
            _methodName = "InsertVolumeLoadability";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<VolumeLoadabilityUploadDto> result = new List<VolumeLoadabilityUploadDto>();
            try
            {
                var volumeLoadabilities = Settings.DeserializeObject<VolumeLoadabilityUploadDto>(items, _dateAndNullSettings);
                foreach (var input in volumeLoadabilities)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertVolumeLoadability(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region UserDivision Mapping
        public ActionResult InsertUserDivisionMapping(DataSourceRequest request, string items)
        {
            _methodName = "InsertUserDivisionMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<UserDivisionUploadDto> result = new List<UserDivisionUploadDto>();
            try
            {
                var volumeLoadabilities = Settings.DeserializeObject<UserDivisionUploadDto>(items, _dateAndNullSettings);
                foreach (var input in volumeLoadabilities)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertUserDivisionMapping(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import UserDiscount     

        public ActionResult UserDiscount(DataSourceRequest request, string items)
        {
            _methodName = "UserDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<UserDiscount> result = new List<UserDiscount>();
            try
            {
                var inputs = Settings.DeserializeObject<UserDiscount>(items, _dateTimeAndNullSettings);
                foreach (var input in inputs)
                {
                    input.LoginUserId = UserId;
                    result.Add(_importClient.UserDiscount(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import GeographyDiscount     
        public async Task<ActionResult> GeographyDiscount(DataSourceRequest request, string items)
        {
            _methodName = "GeographyDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<GeographyDiscount> result = new List<GeographyDiscount>();

            try
            {
                var importGeographyDiscountInputs = Settings.DeserializeObject<GeographyDiscount>(items, _dateTimeAndNullSettings);
                var mappedGeographyDiscountInputs = importGeographyDiscountInputs.Select(_ => MapToImportStatus(_, UserId)).ToList();
                result = await _importClient.ImportGeographyDiscount(mappedGeographyDiscountInputs);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private GeographyDiscountImportStatus MapToImportStatus(GeographyDiscount input, long userId)
        {
            return new GeographyDiscountImportStatus
            {
                SalesOrganization = input.SalesOrganization,
                DistributionChannel = input.DistributionChannel,
                Division = input.Division,
                MaterialCode = input.MaterialCode,
                DiscountReason = input.DiscountReason,
                Discount = input.Discount,
                ValidFrom = input.ValidFrom,
                ValidTo = input.ValidTo,
                LoginUserId = userId,
                Zone = input.Zone,
                State = input.State,
                District = input.District,
                City = input.City,
                Message = input.Message,
                OilType = input.OilType,
                PackGroup = input.PackGroup,
                PackType = input.PackType,
                IsActive = input.IsActive == Settings.Active
            };
        }


        #endregion

        #region Import Line

        public ActionResult InsertLine(DataSourceRequest request, string items)
        {
            _methodName = "InsertLine";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<LineUploadDto> result = new List<LineUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<LineUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertLine(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Import InsertQpsDiscount

        public ActionResult InsertQpsDiscount(DataSourceRequest request, string items)
        {
            _methodName = "InsertQpsDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<QpsDiscountUploadDto> result = new List<QpsDiscountUploadDto>();
            try
            {
                var inputs = Settings.DeserializeObject<QpsDiscountUploadDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                }
                var importDatatable = ToDataTable(inputs);
                result = _importClient.InsertQpsDiscount(importDatatable);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region
        public ActionResult InsertPackGroupMapping(DataSourceRequest request, string items)
        {
            _methodName = "InsertPackGroupMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<PackGroupTypeMapping> result = new List<PackGroupTypeMapping>();
            try
            {
                var inputs = Settings.DeserializeObject<PackGroupTypeMapping>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.CreatedBy = UserId;
                    result.Add(_importClient.InsertPackGroupMapping(input));
                }
                //var importDatatable = ToDataTable(inputs);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region  GamificationDashboard

        public ActionResult InsertGamificationDashboard(DataSourceRequest request, string items)
        {
            _methodName = "InsertGamificationDashboard";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<GamificationDashboardImportDto> result = new List<GamificationDashboardImportDto>();
            try
            {
                var inputs = Settings.DeserializeObject<GamificationDashboardImportDto>(items, _dateAndNullSettings);
                foreach (var input in inputs)
                {
                    input.LoginUserId = UserId;
                    result.Add(_importClient.InsertGamificationDashboard(input));
                }
                //var importDatatable = ToDataTable(inputs);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region MyRegion
        public async Task<JsonResult> GetGeographyDiscountStatus()
        {
            var data = await _importClient.GetGeographyDiscountStatus();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Quantity Limit

        public ActionResult InsertQuantityLimit(DataSourceRequest request, string items)
        {
            _methodName = "InsertQuantityLimit";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<QuantityLimitDTO> result = new List<QuantityLimitDTO>();
            try
            {
                //var skuResult = Settings.DeserializeObject<QuantityLimitDTO>(items, _dateAndNullSettings);
                var skuResult = Settings.DeserializeObject<QuantityLimitDTO>(items, _dateTimeAndNullSettings);
                foreach (var input in skuResult)
                {
                    input.LoginUserId = UserId;
                    result.Add(_importClient.InsertQuantityLimit(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region crossAndUpsell Configuration
        public ActionResult InsertAndUpdateSaudaConditionalBookingConfiguration(DataSourceRequest request, string items)
        {
            _methodName = "InsertAndUpdateSaudaConditionalBookingConfiguration";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SaudaConditionalBookingConfigurationImportDto> result = new List<SaudaConditionalBookingConfigurationImportDto>();
            try
            {
                var saudaConditionalConfig = Settings.DeserializeObject<SaudaConditionalBookingConfigurationImportDto>(items, _dateTimeAndNullSettings);
                foreach (var input in saudaConditionalConfig)
                {
                    input.LoginUserId = UserId;
                    result.Add(_importClient.InsertAndUpdateSaudaConditionalBookingConfiguration(input));
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var resultList = result.ToDataSourceResult(request);
            var jsonResult = Json(resultList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
    }
}