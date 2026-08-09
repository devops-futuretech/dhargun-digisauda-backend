using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Adani.Solution.DTO.Enums;
using GMCore.Helper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using System.Data.SqlClient;
using Dapper;
using Adani.Solution.MVC.Common;
using System.Data;
using Dapper;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.Data.DatabaseContextMigrations;
using Adani.Solution.Data;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Web.Script.Serialization;

namespace Adani.Solution.Service
{
    public interface IMasterService
    {
        //Delivery
        ResultDto GetDeliveryDetails(DeliveryTypeInputDto deliveryTypeDto);
        ResultDto InsertDeliveryDetails(DeliveryTypeDto deliveryTypeDto);
        ResultDto UpdateDeliveryDetails(DeliveryTypeDto deliveryTypeDto);

        //Contract
        ResultDto GetContractDetails(ContractTypeInputDto contractTypeDto);
        ResultDto AddOrUpdateContract(ContractTypeDto contractTypeDto);

        //Vertical
        ResultDto GetVerticals(LoginUserIdDto loginUserIdDto);
        ResultDto AddOrUpdateVerticals(VerticalDto verticalDto);
        ResultDto ExportVertical(LoginUserIdDto inputDto);
        ResultDto GetVerticalListWithPagination(KendoGridResult inputDto);

        //OilType
        ResultDto GetOilType(LoginUserIdDto loginUserIdDto);
        ResultDto GetOilTypeListBasedOnLogin(LoginUserIdDto loginUserIdDto);
        ResultDto AddOrUpdateOiltype(OilTypeDto oilTypeDto);
        ResultDto ExportOilType(LoginUserIdDto loginUserIdDto);
        ResultDto GetOilTypeListWithPagination(KendoGridResult inputDto);

        ResultDto GetServerDateTime();

        //Plant
        ResultDto GetPlantMaster(LoginUserIdDto loginUserIdDto);
        ResultDto AddPlants(DepotDto plantDto);
        ResultDto UpdatePlants(DepotDto plantDto);
        ResultDto GetPlantMasterById(DepotDto plantDto);
        ResultDto GetPlantMasterddl();
        ResultDto GetPlantMasterddlbased(PlantDDLDto plant);
        ResultDto ExportPlantMaster(LoginUserIdDto loginUserIdDto);
        ResultDto GetPlantListWithPagination(KendoGridResult inputDto);

        //Depot
        ResultDto GetDepotMaster(LoginUserIdDto loginUserIdDto);
        ResultDto AddDepots(DepotDto depotDto);
        ResultDto UpdateDepots(DepotDto depotDto);
        ResultDto GetDepotMasterById(DepotDto depotDto);
        ResultDto GetDepotAndPlantList(LoginUserIdDto loginUserIdDto);
        ResultDto ExportDepot(LoginUserIdDto loginUserIdDto);
        ResultDto GetDepotListWithPagination(KendoGridResult inputDto);

        //Zone Mapping     
        ResultDto GetZoneList();
        ResultDto GetRolesList();
        ResultDto GetZoneListForDropdown();
        ResultDto GetStateListByZoneIdForDropdown(long zoneId);
        ResultDto GetZoneStateList(long zoneId);
        ResultDto NewZone();
        ResultDto EditZone(string id);
        ResultDto AddZone(AddorUpdateZoneDto dto);
        ResultDto UpdateZone(AddorUpdateZoneDto dto);
        ResultDto ExportZone(LoginUserIdDto loginUserIdDto);

        //Sku Master
        ResultDto SaveSku(SkuDto inputDto);
        ResultDto GetSkuList(LoginUserIdDto inputDto);
        ResultDto GetSkuDetailsById(string skuId);
        ResultDto UpdateSku(SkuDto inputDto);
        ResultDto GetSkuListWithPagination(KendoGridResult inputDto);

        //Sauda Booking Types
        ResultDto GetBookingTypes();
        ResultDto GetMaterialTypes();
        ResultDto GetOilTypes();
        ResultDto GetProfileImageUrl(UserProfileDto inputDto);
        ResultDto GetSalesDocumentTypes();

        ////Ingredient
        //ResultDto AddIngredients(IngredientDto ingredientDto);
        //ResultDto UpdateIngredients(IngredientDto ingredientDto);
        //ResultDto GetIngredients(KendoGridResult inputDto);

        //State
        ResultDto GetStates();
        ResultDto AddStates(AddStateDto addStateDto);
        ResultDto UpdateStates(UpdateStateDto updateStateDto);
        ResultDto ViewState(UpdateStateDto updateStateDto);
        ResultDto ExportStates(LoginUserIdDto loginUserIdDto);
        ResultDto GetStateListWithPagination(KendoGridResult inputDto);

        //District
        ResultDto GetDistricts();
        ResultDto AddDistrict(AddDistrictDto addDistrictDto);
        ResultDto UpdateDistrict(UpdateDistrictDto updateDistrictDto);
        ResultDto ViewDistrict(UpdateDistrictDto updateDistrictDto);
        ResultDto ExportDistrict(LoginUserIdDto loginUserIdDto);

        //City
        ResultDto GetCities();
        ResultDto AddCity(AddCityDto addCityDto);
        ResultDto UpdateCity(UpdateCityDto updateCityDto);
        ResultDto ViewCity(UpdateCityDto updateCityDto);
        ResultDto ExportCity(LoginUserIdDto loginUserIdDto);

        ////FreightZone Master
        //ResultDto SaveFreightZone(FreightZoneDto inputDto);
        //ResultDto GetFreightZoneList(LoginUserIdDto inputDto);
        //ResultDto GetFreightZoneDetailsById(long freightZoneId);
        //ResultDto UpdateFreightZone(FreightZoneDto inputDto);
        ////ResultDto GetFreightZoneListByDepot(IdInputDto inputDto);
        //ResultDto GetFreightZoneListddl();
        //ResultDto GetFreightZoneListddlByStateZone(FreightZoneInputDto inputDto);
        //ResultDto ExportFreightZone(LoginUserIdDto inputDto);


        //ResultDto GetFreightZoneListByDepotIds(List<long> depotIds);

        ////FreightRoute Master
        //ResultDto SaveFreightRoute(FreightRouteDto inputDto);
        //ResultDto GetFreightRouteList(LoginUserIdDto inputDto);
        //ResultDto GetFreightRouteBasedOnStateList(LoginUserIdDto inputDto);

        //ResultDto GetFreightRouteDetailsById(long freightRouteId);
        //ResultDto UpdateFreightRoute(FreightRouteDto inputDto);
        //ResultDto GetFreightRouteListByZone(IdInputDto inputDto);

        //Territories
        ResultDto AddTerritory(TerritoryDto territoryDto);
        ResultDto UpdateTerritory(TerritoryDto territoryDto);
        ResultDto GerTerritoryById(int id);
        ResultDto GerTerritoryList(LoginUserIdDto inputDto);
        ResultDto GerTerritoryListByStateForDropdown(int stateId);
        ResultDto GetDistrictListBaseTerritoryForDropdown(int territoryId);
        ResultDto GerTerritoryMappedDistrict(TerritoryDistrictParam inputDto);
        ResultDto ExportTerritory(LoginUserIdDto inputDto);

        ResultDto GetIncoTermsList();
        ResultDto GetIncotermListBasedOnUser(LoginUserIdDto inputDto);
        ResultDto GetPlantDepotList();
        ResultDto GetPlantDepotListBasedOnUser(LoginUserIdDto inputDto);
        ResultDto GetStatesBasedOnZone(List<int> zoneId);
        ResultDto GetTerritoryListByStateIdsForDropdown(List<int> stateIds);
        ResultDto GetDepotsByPlantId(IdInputDto inputDto);

        ResultDto GetZonalHeadBasedonZoneState(ZonalHeadMappingDto inputDto);
        ResultDto GetOilTypeBasedonVerticals(OilTypeMappingDto inputDto);

        //ResultDto GetFrieghtRouteList();
        //ResultDto ExportFreightRoute(LoginUserIdDto inputDto);

        ResultDto GetGeographySchemeBasedOnState(List<int> stateId);


        // Get Current FinancialYear
        ResultDto GetCurrentFinancialYear();
        ResultDto GetNotification(LoginUserIdDto loginUserIdDto);
        ResultDto GetRequestNotification(LoginUserIdDto inputDto);
        ResultDto GetDepotsByPlantIds(DepotDropDownParam inputDto);

        #region SubCategory

        ResultDto SaveSubCategory(SubCategoryDto inputDto);
        ResultDto GetSubCategoryList(KendoGridResult inputDto);
        ResultDto GetSubCategoryDetailsById(long subCategoryId);
        ResultDto UpdateSubCategory(SubCategoryDto inputDto);
        ResultDto ExportSubCategory(LoginUserIdDto inputDto);

        #endregion

        //District,Territory and FreightRoute multiselect
        ResultDto GetDistrictListByTerritoryIdsForDropdown(List<int> territoryIds);
        ResultDto GetCityListByDistrictIdsForDropdown(List<int> districtIds);
        //ResultDto GetFreightRouteByZone(List<long> districtIds);

        ////Rake
        //ResultDto GetRakeList(LoginUserIdDto loginUserIdDto);
        //ResultDto AddRake(RakeDto inputDto);
        //ResultDto UpdateRake(RakeDto inputDto);
        //ResultDto GetRakeById(IdInputDto inputDto);
        //ResultDto ExportRake(LoginUserIdDto loginUserIdDto);
        //ResultDto GetRakeListWithPagination(KendoGridResult inputDto);

        //ResultDto GetDepotRakeddList(IdInputDto inputDto);
        //ResultDto GetDepotRakeByPlantId(IdInputDto inputDto);
        //ResultDto GetDepotRakePlantddList(IdInputDto inputDto);
        ResultDto GetDepotPlantddList(IdInputDto inputDto);
        ResultDto GetDepotList(LoginUserIdDto loginUserIdDto);

        ResultDto GetTransportModeBasedonDepotRake(IdInputDto inputDto);

        ResultDto AddOrUpdateVehicleLoadabilities(VehicleLoadabilitiesDto inputDto);

        ResultDto GetVehicleLoadabilitiesList();
        ResultDto GetVehicleLoadabilitiesById(VehicleLoadabilitiesDto inputDto);
        ResultDto ExportVehicleLoadabiliities(LoginUserIdDto loginUserIdDto);

        ResultDto GetStateListByZoneIds(List<long> zoneIds);
        //ResultDto GetFreightZoneListddlByStateZoneIds(FreightZoneInputDto inputDto);
        ResultDto AddorUpdateCustomerGroupFive(CustomerGroupFiveDto inputDto);
        ResultDto GetCustomerGroupFiveList();
        ResultDto GetCustomerGroupFiveDetailsById(string customerGroupId);

        ResultDto AddorUpdateSalesOrganization(SalesOrganizationDto inputDto);
        ResultDto GetSalesOrganizationList();
        ResultDto GetSalesOrganizationDetailsById(string EncryptedId);

        //ResultDto AddorUpdateCustomerGroupOne(CustomerGroupOneDto inputDto);
        //ResultDto GetCustomerGroupOneList();
        //ResultDto GetCustomerGroupOneDetailsById(long customerGroupId);
        //ResultDto AddorUpdateCustomerGroupTwo(CustomerGroupOneDto inputDto);
        //ResultDto GetCustomerGroupTwoList();
        //ResultDto GetCustomerGroupTwoDetailsById(long customerGroupId);
        //ResultDto AddOrUpdateMaterialType(MaterialTypeDto inputDto);
        //ResultDto GetMaterialTypeList();
        //ResultDto GetMaterialTypeById(MaterialTypeDto inputDto);
        //ResultDto ExportMaterialType(LoginUserIdDto loginUserIdDto);

        //Volume loadability
        ResultDto AddOrUpdateVolumeLoadability(DTO.VolumeLoadability inputDto);
        ResultDto GetVolumeLoadabilityList();
        ResultDto GetVolumeLoadabilityById(DTO.VolumeLoadability inputDto);
        ResultDto ExportVolumeLoadability(LoginUserIdDto loginUserIdDto);


        ResultDto AddorUpdateDistributionChannel(DistributionChannelDto inputDto);
        ResultDto GetDistributionChannelList();
        ResultDto GetDistributionChannelDetailsById(string distributionChannelId);
        ResultDto AddLineDetails(AddAndUpdateLineDto InputDto);
        ResultDto UpdateLineDetails(AddAndUpdateLineDto InputDto);
        ResultDto GetLineListForddl();
        ResultDto GetLineListForGrid();
        ResultDto GetLineDetailsById(string LineId);
        ResultDto ExportLine(LoginUserIdDto loginUserIdDto);
        ResultDto GetDONumberListByDistributorId(List<string> selectedIdsList);

        // GamificationDashboard
        ResultDto GetGamificationDashboard(GamificationDashboardDto inputDto);
        ResultDto GCPApidata();

        //TANNumber Mobile API
        ResultDto GetTANNumber(DealerTANDto dealerTANDto);
        ResultDto UpdateTANNumber(DealerTANDto dealerTANDto);
        ResultDto ValidateCalendar();
        ResultDto AccountStatementCount(CustomerAccountStatementDto inputDto);
        ResultDto UpdateAccountStatementStatus(CustomerAccountStatementDto customerAccountStatementDto);
        Task<ResultDto> AddAndUpdateSAPEmailStatement(SAPEmailStatementInputDto inputDto);
        Task<ResultDto> UpdateEmailStatementSAPStatus(SAPEmailStatementDStatusDto inputDto);
        Task<ResultDto> ImportGeographyDiscount(List<GeographyDiscountImportStatus> inputDto);
    }

    public class MasterService : IMasterService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("MasterService");
        private const string ServiceName = "Master Service";
        private string _methodName;
        private readonly IResultService _resultService;

        static string connectionString = ConfigHelper.SPConnectionString;

        public MasterService(IAdaniContext emamiContext, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Master Service", exception);
            }
        }

        #region Masters

        public ResultDto GetDeliveryDetails(DeliveryTypeInputDto deliveryTypeDto)
        {
            _methodName = "GetDelivertDetails";
            var resultDto = new ResultDto();
            try
            {
                switch (deliveryTypeDto.SelectedTypeId)
                {
                    case (int)MasterDataTypes.SaudaStatus:
                        resultDto = GetSaudaStatus(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.SaudaBookingType:
                        resultDto = GetSaudaBookingType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.OilPackingType:
                        resultDto = GetOilPackingType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.PackType:
                        resultDto = GetPackType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.TransaportMode:
                        resultDto = GetTransaportMode(deliveryTypeDto);
                        break;
                    default:
                        resultDto.IsSuccess = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetSaudaStatus(DeliveryTypeInputDto deliveryTypeDto)
        {
            _methodName = "GetSaudaStatus";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.SaudaStatus> entity;
                if (deliveryTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.SaudaStatus.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.SaudaStatus.AsNoTracking().Where(w => w.IsActive);
                }
                var saudaList = entity
                    .Select(s => new DeliveryTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = saudaList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetSaudaBookingType(DeliveryTypeInputDto deliveryTypeDto)
        {
            _methodName = "GetSaudaBookingType";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<SaudaBookingType> entity;
                if (deliveryTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.SaudaBookingTypes.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.SaudaBookingTypes.AsNoTracking().Where(w => w.IsActive);
                }
                var saudaList = entity
                    .Select(s => new DeliveryTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = saudaList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetOilPackingType(DeliveryTypeInputDto deliveryTypeDto)
        {
            _methodName = "GetOilPackingType";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<PackGroup> entity;
                if (deliveryTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.OilPackingTypes.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.OilPackingTypes.AsNoTracking().Where(w => w.IsActive);
                }
                var saudaList = entity
                    .Select(s => new DeliveryTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsActive = s.IsActive
                    });

                resultDto.SuccessDto.Response = saudaList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetPackType(DeliveryTypeInputDto deliveryTypeDto)
        {
            _methodName = "GetPackType";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<PackType> entity;
                if (deliveryTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.PackTypes.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.PackTypes.AsNoTracking().Where(w => w.IsActive);
                }
                var saudaList = entity
                    .Select(s => new DeliveryTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = saudaList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetTransaportMode(DeliveryTypeInputDto deliveryTypeDto)
        {
            _methodName = "GetTransaportMode";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.TransportMode> entity;
                if (deliveryTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.TransportModes.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.TransportModes.AsNoTracking().Where(w => w.IsActive);
                }
                var saudaList = entity
                    .Select(s => new DeliveryTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = saudaList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertDeliveryDetails(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "InsertDelivertDetails";
            var resultDto = new ResultDto();
            try
            {
                switch (deliveryTypeDto.SelectedTypeId)
                {
                    case (int)MasterDataTypes.SaudaStatus:
                        resultDto = InsertSaudaStatus(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.SaudaBookingType:
                        resultDto = InsertSaudaBookingType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.OilPackingType:
                        resultDto = InsertOilPackingType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.PackType:
                        resultDto = InsertPackType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.TransaportMode:
                        resultDto = InsertTransaportMode(deliveryTypeDto);
                        break;
                    default:
                        resultDto.IsSuccess = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertSaudaStatus(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "InsertSaudaStatus";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaData = new Data.Entities.SaudaStatus();
                saudaData.Name = deliveryTypeDto.Name;
                saudaData.IsActive = deliveryTypeDto.IsActive;
                saudaData.CreatedBy = deliveryTypeDto.LoginUserId;
                saudaData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaudaStatus.Add(saudaData);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertSaudaBookingType(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "InsertSaudaBookingType";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaBookData = new SaudaBookingType();
                saudaBookData.Name = deliveryTypeDto.Name;
                saudaBookData.IsActive = deliveryTypeDto.IsActive;
                saudaBookData.CreatedBy = deliveryTypeDto.LoginUserId;
                saudaBookData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaudaBookingTypes.Add(saudaBookData);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertOilPackingType(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "InsertOilPackingType";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var oilPackingData = new PackGroup();
                oilPackingData.Name = deliveryTypeDto.Name;
                oilPackingData.IsActive = deliveryTypeDto.IsActive;
                oilPackingData.CreatedBy = deliveryTypeDto.LoginUserId;
                oilPackingData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.OilPackingTypes.Add(oilPackingData);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertPackType(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "InsertPackType";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var packData = new PackType();
                packData.Name = deliveryTypeDto.Name;
                packData.IsActive = deliveryTypeDto.IsActive;
                packData.CreatedBy = deliveryTypeDto.LoginUserId;
                packData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.PackTypes.Add(packData);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertTransaportMode(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "InsertTransaportMode";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var transportData = new Data.Entities.TransportMode();
                transportData.Name = deliveryTypeDto.Name;
                transportData.IsActive = deliveryTypeDto.IsActive;
                transportData.CreatedBy = deliveryTypeDto.LoginUserId;
                transportData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.TransportModes.Add(transportData);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateDeliveryDetails(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "UpdateDelivertDetails";
            var resultDto = new ResultDto();
            try
            {
                switch (deliveryTypeDto.SelectedTypeId)
                {
                    case (int)MasterDataTypes.SaudaStatus:
                        resultDto = UpdateSaudaStatus(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.SaudaBookingType:
                        resultDto = UpdateSaudaBookingType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.OilPackingType:
                        resultDto = UpdateOilPackingType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.PackType:
                        resultDto = UpdatePackType(deliveryTypeDto);
                        break;
                    case (int)MasterDataTypes.TransaportMode:
                        resultDto = UpdateTransaportMode(deliveryTypeDto);
                        break;
                    default:
                        resultDto.IsSuccess = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateSaudaStatus(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "UpdateSaudaStatus";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaData = new Data.Entities.SaudaStatus();
                saudaData = _emamiContext.SaudaStatus.FirstOrDefault(f => f.Id == deliveryTypeDto.Id);
                saudaData.Name = deliveryTypeDto.Name;
                saudaData.IsActive = deliveryTypeDto.IsActive;
                saudaData.ModifiedBy = deliveryTypeDto.LoginUserId;
                saudaData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateSaudaBookingType(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "UpdateSaudaBookingType";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaBookData = new SaudaBookingType();
                saudaBookData = _emamiContext.SaudaBookingTypes.FirstOrDefault(f => f.Id == deliveryTypeDto.Id);
                saudaBookData.Name = deliveryTypeDto.Name;
                saudaBookData.IsActive = deliveryTypeDto.IsActive;
                saudaBookData.ModifiedBy = deliveryTypeDto.LoginUserId;
                saudaBookData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateOilPackingType(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "UpdateOilPackingType";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var oilPackingData = new PackGroup();
                oilPackingData = _emamiContext.OilPackingTypes.FirstOrDefault(f => f.Id == deliveryTypeDto.Id);
                oilPackingData.Name = deliveryTypeDto.Name;
                oilPackingData.IsActive = deliveryTypeDto.IsActive;
                oilPackingData.ModifiedBy = deliveryTypeDto.LoginUserId;
                oilPackingData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdatePackType(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "UpdatePackType";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var packData = new PackType();
                packData = _emamiContext.PackTypes.FirstOrDefault(f => f.Id == deliveryTypeDto.Id);
                packData.Name = deliveryTypeDto.Name;
                packData.IsActive = deliveryTypeDto.IsActive;
                packData.ModifiedBy = deliveryTypeDto.LoginUserId;
                packData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateTransaportMode(DeliveryTypeDto deliveryTypeDto)
        {
            _methodName = "UpdateTransaportMode";
            var resultDto = new ResultDto();
            try
            {
                if (deliveryTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var transportData = new Data.Entities.TransportMode();
                transportData = _emamiContext.TransportModes.FirstOrDefault(f => f.Id == deliveryTypeDto.Id);
                transportData.Name = deliveryTypeDto.Name;
                transportData.IsActive = deliveryTypeDto.IsActive;
                transportData.ModifiedBy = deliveryTypeDto.LoginUserId;
                transportData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetContractDetails(ContractTypeInputDto contractTypeDto)
        {
            _methodName = "GetContractDetails";
            var resultDto = new ResultDto();
            try
            {
                switch (contractTypeDto.SelectedTypeId)
                {
                    case (int)SeederDataType.ContractType:
                        resultDto = GetContractType(contractTypeDto);
                        break;
                    case (int)SeederDataType.IncoTerms:
                        resultDto = GetIncoTerms(contractTypeDto);
                        break;
                    case (int)SeederDataType.DeliveryPriority:
                        resultDto = GetDeliveryPriority(contractTypeDto);
                        break;
                    case (int)SeederDataType.PickingPoint:
                        resultDto = GetPickingPoint(contractTypeDto);
                        break;
                    default:
                        resultDto.IsSuccess = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetContractType(ContractTypeInputDto contractTypeDto)
        {
            _methodName = "GetContractType";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<ContractType> entity;
                if (contractTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.ContractTypes.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.ContractTypes.AsNoTracking().Where(w => w.IsActive);
                }
                var contractList = entity
                    .Select(s => new ContractTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = contractList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetIncoTerms(ContractTypeInputDto contractTypeDto)
        {
            _methodName = "GetIncoTerms";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.IncoTerms> entity;
                if (contractTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.IncoTerms.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.IncoTerms.AsNoTracking().Where(w => w.IsActive);
                }

                var incoTermsList = entity
                    .Select(s => new ContractTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsActive = s.IsActive
                    });

                resultDto.SuccessDto.Response = incoTermsList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetDeliveryPriority(ContractTypeInputDto contractTypeDto)
        {
            _methodName = "GetDeliveryPriority";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<DeliveryPriority> entity;
                if (contractTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.DeliveryPriorities.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.DeliveryPriorities.AsNoTracking().Where(w => w.IsActive);
                }
                var deliveryList = entity
                    .Select(s => new ContractTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = deliveryList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetPickingPoint(ContractTypeInputDto contractTypeDto)
        {
            _methodName = "GetPickingPoint";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<PickingPoint> entity;
                if (contractTypeDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.PickingPoints.AsNoTracking();
                }
                else
                {
                    entity = _emamiContext.PickingPoints.AsNoTracking().Where(w => w.IsActive);
                }
                var pickingList = entity
                    .Select(s => new ContractTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = pickingList.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetServerDateTime()
        {
            _methodName = "GetServerDateTime";
            var resultDto = new ResultDto();
            try
            {
                resultDto.SuccessDto.Response = DateHelper.UtcToIndia(DateTime.UtcNow);
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetBookingTypes()
        {
            _methodName = "GetBookingTypes";
            var resultDto = new ResultDto();
            try
            {
                var bookingTypes = _emamiContext.BookingTypes.Where(s => s.IsActive).Select(s => new BookingTypeDto()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                resultDto.SuccessDto.Response = bookingTypes;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetMaterialTypes()
        {
            _methodName = "GetMeterialTypes";
            var resultDto = new ResultDto();
            try
            {
                //var bookingTypes = _emamiContext.MaterialTypes.Where(s => s.IsActive).Select(s => new MaterialTypesDto()
                //{
                //    Id = s.Id,
                //    Name = s.Name
                //}).ToList();
                //resultDto.SuccessDto.Response = bookingTypes;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetOilTypes()
        {
            _methodName = "GetOilTypes";
            var resultDto = new ResultDto();
            try
            {
                var bookingTypes = _emamiContext.OilTypes.Where(s => s.IsActive).Select(s => new OilTypesDto()
                {
                    OilTypeId = s.Id,

                    OilName = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code,
                    VerticleId = s.DivisionId
                }).ToList();
                resultDto.SuccessDto.Response = bookingTypes;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }
        public ResultDto GetSalesDocumentTypes()
        {
            _methodName = "GetSalesDocumentTypes";
            var resultDto = new ResultDto();
            try
            {
                //var salesdocumentTypes = _emamiContext.SalesDocumentType.Where(s => s.IsActive).Select(s => new SalesDocumentTypeddlDto()
                //{
                //    Id = s.Id,
                //    Name = s.Name,
                //}).ToList();
                //resultDto.SuccessDto.Response = salesdocumentTypes;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Contract Types

        public ResultDto AddOrUpdateContract(ContractTypeDto contractTypeDto)
        {
            _methodName = "InsertContractDetails";
            var resultDto = new ResultDto();
            try
            {
                switch (contractTypeDto.SelectedTypeId)
                {
                    case (int)SeederDataType.ContractType:
                        resultDto = InsertContractType(contractTypeDto);
                        break;
                    case (int)SeederDataType.IncoTerms:
                        resultDto = InsertIncoTerms(contractTypeDto);
                        break;
                    case (int)SeederDataType.DeliveryPriority:
                        resultDto = InsertDeliveryPriority(contractTypeDto);
                        break;
                    case (int)SeederDataType.PickingPoint:
                        resultDto = InsertPickingPoint(contractTypeDto);
                        break;
                    default:
                        resultDto.IsSuccess = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertContractType(ContractTypeDto contractTypeDto)
        {
            _methodName = "InsertContractType";
            var resultDto = new ResultDto();
            try
            {
                if (contractTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var contractData = new ContractType();
                if (!(contractTypeDto.Id > 0))
                {
                    contractData.Name = contractTypeDto.Name;
                    contractData.Code = contractTypeDto.Code;
                    contractData.IsActive = contractTypeDto.IsActive;
                    contractData.CreatedBy = contractTypeDto.LoginUserId;
                    contractData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.ContractTypes.Add(contractData);
                }
                else
                {
                    contractData = _emamiContext.ContractTypes.FirstOrDefault(f => f.Id == contractTypeDto.Id);
                    contractData.Name = contractTypeDto.Name;
                    contractData.Code = contractTypeDto.Code;
                    contractData.IsActive = contractTypeDto.IsActive;
                    contractData.ModifiedBy = contractTypeDto.LoginUserId;
                    contractData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertIncoTerms(ContractTypeDto contractTypeDto)
        {
            _methodName = "InsertIncoTerms";
            var resultDto = new ResultDto();
            try
            {
                if (contractTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var incoTermsData = new Data.Entities.IncoTerms();
                if (!(contractTypeDto.Id > 0))
                {
                    incoTermsData.Name = contractTypeDto.Name;
                    incoTermsData.Code = contractTypeDto.Code;
                    incoTermsData.IsActive = contractTypeDto.IsActive;
                    incoTermsData.CreatedBy = contractTypeDto.LoginUserId;
                    incoTermsData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.IncoTerms.Add(incoTermsData);
                }
                else
                {
                    incoTermsData = _emamiContext.IncoTerms.FirstOrDefault(f => f.Id == contractTypeDto.Id);
                    incoTermsData.Name = contractTypeDto.Name;
                    incoTermsData.Code = contractTypeDto.Code;
                    incoTermsData.IsActive = contractTypeDto.IsActive;
                    incoTermsData.ModifiedBy = contractTypeDto.LoginUserId;
                    incoTermsData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertDeliveryPriority(ContractTypeDto contractTypeDto)
        {
            _methodName = "InsertDeliveryPriority";
            var resultDto = new ResultDto();
            try
            {
                if (contractTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var deliveryData = new DeliveryPriority();
                if (!(contractTypeDto.Id > 0))
                {
                    deliveryData.Name = contractTypeDto.Name;
                    deliveryData.Code = contractTypeDto.Code;
                    deliveryData.IsActive = contractTypeDto.IsActive;
                    deliveryData.CreatedBy = contractTypeDto.LoginUserId;
                    deliveryData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.DeliveryPriorities.Add(deliveryData);
                }
                else
                {
                    deliveryData = _emamiContext.DeliveryPriorities.FirstOrDefault(f => f.Id == contractTypeDto.Id);
                    deliveryData.Name = contractTypeDto.Name;
                    deliveryData.Code = contractTypeDto.Code;
                    deliveryData.IsActive = contractTypeDto.IsActive;
                    deliveryData.ModifiedBy = contractTypeDto.LoginUserId;
                    deliveryData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto InsertPickingPoint(ContractTypeDto contractTypeDto)
        {
            _methodName = "InsertPickingPoint";
            var resultDto = new ResultDto();
            try
            {
                if (contractTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var pickingData = new PickingPoint();
                if (!(contractTypeDto.Id > 0))
                {
                    pickingData.Name = contractTypeDto.Name;
                    pickingData.Code = contractTypeDto.Code;
                    pickingData.IsActive = contractTypeDto.IsActive;
                    pickingData.CreatedBy = contractTypeDto.LoginUserId;
                    pickingData.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.PickingPoints.Add(pickingData);
                }
                else
                {
                    pickingData = _emamiContext.PickingPoints.FirstOrDefault(f => f.Id == contractTypeDto.Id);
                    pickingData.Name = contractTypeDto.Name;
                    pickingData.Code = contractTypeDto.Code;
                    pickingData.IsActive = contractTypeDto.IsActive;
                    pickingData.ModifiedBy = contractTypeDto.LoginUserId;
                    pickingData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Vertical

        public ResultDto GetVerticals(LoginUserIdDto inputDto)
        {
            _methodName = "GetVerticals";
            var resultDto = new ResultDto();
            try
            {
                List<Data.Entities.Division> verticals;
                if (inputDto.IsToReturnInactiveData)
                {
                    verticals = _emamiContext.Divisions.AsNoTracking().ToList();
                }
                else
                {
                    verticals = _emamiContext.Divisions.AsNoTracking()
                  .Where(w => w.IsActive && w.DistributionChannelId == inputDto.DistributionId).ToList();
                }

                var verticalList = verticals.AsEnumerable().Where(w => (inputDto.VerticalId > 0 ? w.Id == inputDto.VerticalId : w.Id > 0))
                    .Select(s => new VerticalDto()
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsActive = s.IsActive,
                        ZPR4 = s.ZPR4,
                        SalesDocumentType = s.SalesDocumentType,
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        SalesOrderDocumentType = s.SalesOrderDocumentType == null ? string.Empty : s.SalesOrderDocumentType
                    }).ToList();

                resultDto.SuccessDto.Response = verticalList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        /// <summary>
        /// Add or Update Vertical Details
        /// </summary>
        /// <param name="verticalDto"></param>
        /// <returns></returns>
        public ResultDto AddOrUpdateVerticals(VerticalDto verticalDto)
        {
            _methodName = "AddOrUpdateVerticals";
            var resultDto = new ResultDto();
            var vertical = new Data.Entities.Division();
            bool isNameExist = false;
            bool isCodeExist = false;

            try
            {
                if (verticalDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(verticalDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(verticalDto.EncryptedId, SecurityConstants.EncryptionKey);

                    verticalDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }
                if (!(verticalDto.Id > 0))
                {
                    isNameExist = _emamiContext.Divisions.AsNoTracking().Any(a => a.Name == verticalDto.Name
                    && a.DistributionChannelId == verticalDto.DistributionChannelId
                    && a.SalesOrganizationId == verticalDto.SalesOrganizationId);
                    if (isNameExist)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.VerticaleNameExists;
                        return resultDto;
                    }

                    isCodeExist = _emamiContext.Divisions.AsNoTracking().Any(a => a.Code == verticalDto.Code
                    && a.DistributionChannelId == verticalDto.DistributionChannelId
                    && a.SalesOrganizationId == verticalDto.SalesOrganizationId);
                    if (isCodeExist)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.CodeExist;
                        return resultDto;
                    }

                    vertical.Name = verticalDto.Name;
                    vertical.Code = verticalDto.Code;
                    vertical.IsActive = verticalDto.IsActive;
                    vertical.ZPR4 = verticalDto.ZPR4;
                    vertical.CreatedBy = verticalDto.UserId;
                    vertical.SalesDocumentType = verticalDto.SalesDocumentType;
                    vertical.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    vertical.DistributionChannelId = verticalDto.DistributionChannelId;
                    vertical.SalesOrganizationId = verticalDto.SalesOrganizationId;
                    vertical.SalesOrderDocumentType = verticalDto.SalesOrderDocumentType;
                    //_emamiContext.Verticals.Add(vertical);
                    _emamiContext.Divisions.Add(vertical);
                }
                else
                {
                    isNameExist = _emamiContext.Divisions.AsNoTracking().Any(a => a.Name == verticalDto.Name && a.DistributionChannelId == verticalDto.DistributionChannelId
                   && a.SalesOrganizationId == verticalDto.SalesOrganizationId && a.Id != verticalDto.Id);
                    if (isNameExist)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.VerticaleNameExists;
                        return resultDto;
                    }
                    isCodeExist = _emamiContext.Divisions.AsNoTracking().Any(a => a.Code == verticalDto.Code && a.DistributionChannelId == verticalDto.DistributionChannelId
                   && a.SalesOrganizationId == verticalDto.SalesOrganizationId && a.Id != verticalDto.Id);
                    if (isCodeExist)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.CodeExist;
                        return resultDto;
                    }

                    vertical = _emamiContext.Divisions.FirstOrDefault(f => f.Id == verticalDto.Id);
                    vertical.Name = verticalDto.Name;
                    vertical.Code = verticalDto.Code;
                    vertical.SalesDocumentType = verticalDto.SalesDocumentType;
                    vertical.IsActive = verticalDto.IsActive;
                    vertical.ZPR4 = verticalDto.ZPR4;
                    vertical.ModifiedBy = verticalDto.UserId;
                    vertical.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    vertical.DistributionChannelId = verticalDto.DistributionChannelId;
                    vertical.SalesOrganizationId = verticalDto.SalesOrganizationId;
                    vertical.SalesOrderDocumentType = verticalDto.SalesOrderDocumentType;
                }
                _emamiContext.SaveChanges();

                //var verticalDetailExists = _emamiContext.DivisionDetails.Where(_ => _.DivisionId == vertical.Id).ToList();

                //if (verticalDetailExists.IsAny())
                //{
                //    verticalDetailExists.ForEach(s => _emamiContext.DivisionDetails.Remove(s));
                //    _emamiContext.SaveChanges();
                //}
                //var CCAreaList = verticalDto.CCArea.Split(',').ToList();
                //foreach (var data in CCAreaList)
                //{
                //    var verticalDetail = new DivisionDetail()
                //    {
                //        DivisionId = (int)vertical.Id,
                //        CCArea = data,
                //        ModifiedBy = verticalDto.UserId,
                //        ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                //    };
                //    _emamiContext.DivisionDetails.Add(verticalDetail);
                //}
                //_emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto ExportVertical(LoginUserIdDto inputDto)
        {
            _methodName = "ExportVertical";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.Division> verticals;
                if (inputDto.IsToReturnInactiveData)
                {
                    verticals = _emamiContext.Divisions.AsNoTracking();
                }
                else
                {
                    verticals = _emamiContext.Divisions.AsNoTracking()
                  .Where(w => w.IsActive);
                }

                var verticalList = verticals.Where(w => (inputDto.VerticalId > 0 ? w.Id == inputDto.VerticalId : w.Id > 0)).Join(_emamiContext.DistributionChannel.AsNoTracking(), v => v.DistributionChannelId, Dc => Dc.Id, (v, Dc) => new { v, Dc })
                    .Select(s => new VerticalDto()
                    {
                        //Id = s.v.Id,
                        Name = s.v.Name,
                        Code = s.v.Code,
                        IsActive = s.v.IsActive,
                        ZPR4 = s.v.ZPR4,
                        SalesOrganizationName = s.v.SalesOrganization.Name,
                        DistributionChannelName = s.Dc.Name,
                        SalesDocumentType = s.v.SalesDocumentType,
                        SalesOrderDocumentType = s.v.SalesOrderDocumentType == null ? string.Empty : s.v.SalesOrderDocumentType
                    }).ToList();


                resultDto.SuccessDto.Response = verticalList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetVerticalListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetVerticalListWithPagination";
            var resultDto = new ResultDto();
            try
            {
                List<Data.Entities.Division> verticals;
                if (inputDto.IsToReturnInactiveData)
                {
                    verticals = _emamiContext.Divisions.AsEnumerable().ToList();
                }
                else
                {
                    verticals = _emamiContext.Divisions.AsEnumerable()
                  .Where(w => w.IsActive).ToList();
                }

                //var verticalList = verticals.Where(w => (inputDto.VerticalId > 0 ? w.Id == inputDto.VerticalId : w.Id > 0)).Join(_emamiContext.DistributionChannel.AsNoTracking(), v => v.DistributionChannelId, Dc => Dc.Id, (v, Dc) => new { v, Dc })
                var verticalList = verticals.AsEnumerable().Select(s => new VerticalDto()
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code,
                    IsActive = s.IsActive,
                    ZPR4 = s.ZPR4,
                    SalesDocumentType = s.SalesDocumentType,
                    SalesOrganizationName = s.SalesOrganization.Name,
                    DistributionChannelName = s.DistributionChannel.Name,
                    SalesOrganizationId = s.SalesOrganizationId,
                    DistributionChannelId = s.DistributionChannelId,
                    SalesOrderDocumentType = s.SalesOrderDocumentType == null ? string.Empty : s.SalesOrderDocumentType
                    //Id = s.v.Id,
                    //Name = s.v.Name,
                    //Code = s.v.Code,
                    //IsActive = s.v.IsActive,
                    //SalesOrganizationName=s.SalesOrganization.Name,
                    //DistributionChannelName = s.Dc.Name
                }).ToList();

                //foreach (var vertical in verticalList)
                //{
                //    var CCAreaList = _emamiContext.DivisionDetails.AsNoTracking().Where(_ => _.DivisionId == vertical.Id).Select(s => s.CCArea).ToList();
                //    vertical.CCArea = string.Join(",", CCAreaList);
                //}

                resultDto.SuccessDto.Response = verticalList.ToDataSourceResult(inputDto.DataSourceRequest);
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region OilType

        public ResultDto GetOilType(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetOilType";
            var resultDto = new ResultDto();
            var oilTypeList = new List<OilTypeDto>();
            try
            {
                var currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var userDivisionContext = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.DealerId).ToList();
                if (userDivisionContext != null && userDivisionContext.Any())
                {
                    foreach (var userDivision in userDivisionContext)
                    {
                        var oiltypeContext = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == userDivision.DivisionId
                        && _.SalesOrganizationId == userDivision.SalesOrganizationId && _.DistributionChannelId == userDivision.DistributionChannelId && _.IsActive).Select(s => new OilTypeDto()
                        {
                            Id = s.Id,
                            Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code,
                            //Code = s.SAPCode,
                            VerticalId = s.DivisionId,
                            VerticalName = s.Division.Name,
                            // LitreConversion = s.LitreConversion,
                            IsActive = s.IsActive,
                            //IsRasoi = s.IsRasoi
                        }).ToList();

                        if (oiltypeContext != null && oiltypeContext.Any())
                        {
                            oilTypeList.AddRange(oiltypeContext);
                        }
                    }
                }
                else
                {
                    IQueryable<OilType> oiltype;
                    if (loginUserIdDto.IsToReturnInactiveData)
                    {
                        if (loginUserIdDto.DivisionId == 0 && loginUserIdDto.SalesOrganizationId == 0 && loginUserIdDto.DistributionChannelId == 0)
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking();
                        }
                        else
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == loginUserIdDto.DivisionId
                            && _.SalesOrganizationId == loginUserIdDto.SalesOrganizationId && _.DistributionChannelId == loginUserIdDto.DistributionChannelId);
                        }
                    }
                    else
                    {
                        if (loginUserIdDto.DivisionId == 0 && loginUserIdDto.SalesOrganizationId == 0 && loginUserIdDto.DistributionChannelId == 0)
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.IsActive);
                        }
                        else
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == loginUserIdDto.DivisionId
                            && _.SalesOrganizationId == loginUserIdDto.SalesOrganizationId && _.DistributionChannelId == loginUserIdDto.DistributionChannelId && _.IsActive);
                        }
                    }
                    oilTypeList = oiltype
                    .Select(s => new OilTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code,
                        //Code = s.SAPCode,
                        VerticalId = s.DivisionId,
                        VerticalName = s.Division.Name,
                        // LitreConversion = s.LitreConversion,
                        IsActive = s.IsActive,
                        //IsRasoi = s.IsRasoi
                    }).ToList();

                }

                // Removing restricted oiltypes in configuration
                if (loginUserIdDto.IsSaudaConfig)
                {
                    var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(s => s.UserId == loginUserIdDto.LoginUserId).RoleId;
                    var sudaconfiguredOilTypes = _emamiContext.SaudaBookingConfiguration.AsNoTracking().FirstOrDefault(config => config.RoleId == userrole && config.IsActive && config.StartDate >= currentdate);
                    if (sudaconfiguredOilTypes != null)
                    {
                        var oilTypes = sudaconfiguredOilTypes.OilTypeIds.IsAny() ? sudaconfiguredOilTypes.OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();
                        oilTypeList = oilTypeList.Where(oiltype => !oilTypes.Contains(oiltype.Id)).ToList();
                    }
                }
                resultDto.SuccessDto.Response = oilTypeList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }
        public ResultDto GetOilTypeListBasedOnLogin(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetOilTypeListBasedOnLogin";
            var resultDto = new ResultDto();
            var oilTypeList = new List<OilTypeDto>();
            try
            {
                if (loginUserIdDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var userDivisionContext = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId).ToList();
                if (userDivisionContext != null && userDivisionContext.Any())
                {
                    foreach (var userDivision in userDivisionContext)
                    {
                        var oiltypeContext = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == userDivision.DivisionId
                        && _.SalesOrganizationId == userDivision.SalesOrganizationId && _.DistributionChannelId == userDivision.DistributionChannelId && _.IsActive).Select(s => new OilTypeDto()
                        {
                            Id = s.Id,
                            Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code,
                            VerticalId = s.DivisionId,
                            VerticalName = s.Division.Name,
                            IsActive = s.IsActive,
                        }).ToList();

                        if (oiltypeContext != null && oiltypeContext.Any())
                        {
                            oilTypeList.AddRange(oiltypeContext);
                        }
                    }
                }

                resultDto.SuccessDto.Response = oilTypeList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }
        /// <summary>
        /// Add or Update Oil Type
        /// </summary>
        /// <param name="oilTypeDto"></param>
        /// <returns></returns>
        public ResultDto AddOrUpdateOiltype(OilTypeDto oilTypeDto)
        {
            _methodName = "AddOrUpdateOiltype";
            var resultDto = new ResultDto();
            var oiltype = new OilType();
            bool isNameExist = false;
            bool isCodeExist = false;

            try
            {
                if (oilTypeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(oilTypeDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(oilTypeDto.EncryptedId, SecurityConstants.EncryptionKey);

                    oilTypeDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }
                if (!(oilTypeDto.Id > 0))
                {
                    var isExists = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Name == oilTypeDto.Name && _.SalesOrganizationId == oilTypeDto.SalesOrganizationId && _.DistributionChannelId == oilTypeDto.DistributionChannelId && _.DivisionId == oilTypeDto.VerticalId).FirstOrDefault();
                    //isNameExist = _emamiContext.OilTypes.AsNoTracking().Any(a =>  a.Name == oilTypeDto.Name);
                    //if (isNameExist)
                    //{
                    //    resultDto.IsSuccess = false;
                    //    resultDto.ErrorDto.Message = Constants.OilTypeNameExists;
                    //    return resultDto;
                    //}

                    if (isExists != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.OilTypeExists;
                        return resultDto;
                    }
                    //isCodeExist = _emamiContext.OilTypes.AsNoTracking().Any(a => a.SAPCode == oilTypeDto.Code);
                    //if (isCodeExist)
                    //{
                    //    resultDto.IsSuccess = false;
                    //    resultDto.ErrorDto.Message = Constants.OilTypeCodeExists;
                    //    return resultDto;
                    //}


                    oiltype.Name = oilTypeDto.Name;
                    oiltype.DivisionId = oilTypeDto.VerticalId;
                    //oiltype.SAPCode = oilTypeDto.Code;
                    oiltype.DistributionChannelId = oilTypeDto.DistributionChannelId;
                    oiltype.SalesOrganizationId = oilTypeDto.SalesOrganizationId;
                    oiltype.IsActive = oilTypeDto.IsActive;
                    // oiltype.LitreConversion = oilTypeDto.LitreConversion;
                    //oiltype.IsRasoi = oilTypeDto.IsRasoi;
                    oiltype.CreatedBy = oilTypeDto.LoginUserId;
                    oiltype.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.OilTypes.Add(oiltype);
                    resultDto.SuccessDto.Message = "Add Successfully";
                }
                else
                {

                    var isExists = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Name == oilTypeDto.Name && _.Id != oilTypeDto.Id && _.SalesOrganizationId == oilTypeDto.SalesOrganizationId && _.DistributionChannelId == oilTypeDto.DistributionChannelId && _.DivisionId == oilTypeDto.VerticalId).FirstOrDefault();

                    //isNameExist = _emamiContext.OilTypes.AsNoTracking().Any(a =>  a.Name == oilTypeDto.Name && a.Id != oilTypeDto.Id);
                    //if (isNameExist)
                    //{
                    //    resultDto.IsSuccess = false;
                    //    resultDto.ErrorDto.Message = Constants.OilTypeNameExists;
                    //    return resultDto;
                    //}
                    //isCodeExist = _emamiContext.OilTypes.AsNoTracking().Any(a =>  a.SAPCode == oilTypeDto.Code && a.Id != oilTypeDto.Id);
                    //if (isCodeExist)
                    //{
                    //    resultDto.IsSuccess = false;
                    //    resultDto.ErrorDto.Message = Constants.OilTypeCodeExists;
                    //    return resultDto;
                    //}
                    if (isExists != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.OilTypeExists;
                        return resultDto;
                    }


                    oiltype = _emamiContext.OilTypes.FirstOrDefault(f => f.Id == oilTypeDto.Id);
                    oiltype.Name = oilTypeDto.Name;
                    oiltype.SalesOrganizationId = oilTypeDto.SalesOrganizationId;
                    oiltype.DistributionChannelId = oilTypeDto.DistributionChannelId;
                    oiltype.DivisionId = oilTypeDto.VerticalId;
                    //oiltype.SAPCode = oilTypeDto.Code;
                    // oiltype.LitreConversion = oilTypeDto.LitreConversion;
                    oiltype.IsActive = oilTypeDto.IsActive;
                    //oiltype.IsRasoi = oilTypeDto.IsRasoi;
                    oiltype.ModifiedBy = oilTypeDto.LoginUserId;
                    oiltype.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    resultDto.SuccessDto.Message = "Update Successfully";
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto ExportOilType(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportOilType";
            var resultDto = new ResultDto();
            try
            {
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                IQueryable<OilType> oiltype;
                var userDivisionContext = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId).ToList(); ;
                var userDivisionIds = userDivisionContext.Select(_ => _.DivisionId).ToList();
                var userSalesOrgIds = userDivisionContext.Select(_ => _.SalesOrganizationId).ToList();
                var userDistChanIds = userDivisionContext.Select(_ => _.DistributionChannelId).ToList();
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    if (userDivisionContext == null || !userDivisionContext.Any())
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking();
                    }
                    else
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => userDivisionIds.Contains(_.DivisionId)
                        && userSalesOrgIds.Contains(_.SalesOrganizationId) && userDistChanIds.Contains(_.DistributionChannelId));
                    }
                }
                else
                {
                    if (userDivisionContext == null || !userDivisionContext.Any())
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking();
                    }
                    else
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => userDivisionIds.Contains(_.DivisionId)
                        && userSalesOrgIds.Contains(_.SalesOrganizationId) && userDistChanIds.Contains(_.DistributionChannelId));
                    }
                }

                var oiltypeList = oiltype
                    .Select(s => new OilTypeDto()
                    {
                        //Id = s.Id,
                        Code = s.SAPCode,
                        Name = s.Name,
                        VerticalId = s.DivisionId,
                        DistributionChannelName = s.DistributionChannel.Name,
                        SalesOrganizationName = s.SalesOrganization.Name,
                        VerticalName = s.Division.Name,
                        //  LitreConversion = s.LitreConversion,
                        IsActive = s.IsActive,
                        DistributionChannelId = s.DistributionChannelId,
                        SalesOrganizationId = s.SalesOrganizationId
                        //IsRasoi = s.IsRasoi
                    }).ToList();

                resultDto.SuccessDto.Response = oiltypeList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetOilTypeListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetOilTypeListWithPagination";
            var resultDto = new ResultDto();
            try
            {
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                List<OilType> oiltype;



                var userRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);

                if (userRole.RoleId == (int)DTO.Enums.Role.Admin)
                {
                    oiltype = _emamiContext.OilTypes.AsNoTracking().ToList();
                }
                else
                {
                    var userDivisionContext = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).ToList(); ;
                    var userDivisionIds = userDivisionContext.Select(_ => _.DivisionId).ToList();
                    var userSalesOrgIds = userDivisionContext.Select(_ => _.SalesOrganizationId).ToList();
                    var userDistChanIds = userDivisionContext.Select(_ => _.DistributionChannelId).ToList();
                    if (inputDto.IsToReturnInactiveData)
                    {
                        if (userDivisionContext == null || !userDivisionContext.Any())
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking().ToList();
                        }
                        else
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => userDivisionIds.Contains(_.DivisionId)
                            && userSalesOrgIds.Contains(_.SalesOrganizationId) && userDistChanIds.Contains(_.DistributionChannelId)).ToList();
                        }
                    }
                    else
                    {
                        if (userDivisionContext == null || !userDivisionContext.Any())
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking().ToList();
                        }
                        else
                        {
                            oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => userDivisionIds.Contains(_.DivisionId)
                            && userSalesOrgIds.Contains(_.SalesOrganizationId) && userDistChanIds.Contains(_.DistributionChannelId)).ToList();
                        }
                    }
                }


                var outputDto = oiltype.AsEnumerable()
                    .Select(s => new OilTypeDto()
                    {
                        Id = s.Id,
                        EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                        Name = s.Name,
                        Code = s.SAPCode,
                        VerticalId = s.DivisionId,
                        VerticalName = s.Division.Name,
                        DistributionChannelName = s.DistributionChannel.Name,
                        SalesOrganizationName = s.SalesOrganization.Name,
                        //  LitreConversion = s.LitreConversion,
                        IsActive = s.IsActive,
                        DistributionChannelId = s.DistributionChannelId,
                        SalesOrganizationId = s.SalesOrganizationId
                        //IsRasoi = s.IsRasoi
                    }).ToList();

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Plant Master

        public ResultDto GetPlantMaster(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPlantMaster";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Depot> plantData;
                if (loginUserIdDto.IsToReturnInactiveData)
                    plantData = _emamiContext.Depots.AsNoTracking().Where(_ => _.IsPlant);
                else
                    plantData = _emamiContext.Depots.AsNoTracking()
                  .Where(_ => _.IsPlant && _.IsActive);

                var outputDto = plantData.ToList()
                    .Select(s => new DepotDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        Email = s.Email,
                        //ZoneId = s.ZoneId,
                        //ZoneName = s.Zone != null ? s.Zone.Name : string.Empty,
                        //StateId = s.StateId,
                        //State = s.State?.StateName,
                        //DistrictId = s.DistrictId,
                        //District = s.District?.DistrictName,
                        //CityId = s.CityId,
                        //City = s.City?.CityName,
                        MobileNumber = s.MobileNumber,
                        PinCode = s.Pincode,
                        Location = s.Location,
                        IsActive = s.IsActive,
                        //TerritoryId = s.TerritoryId,
                        //TerritoryName = s.Territory?.Name,
                        Usage = s?.Usage
                    }).ToList();

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto AddPlants(DepotDto inputDto)
        {
            _methodName = "AddPlants";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                    inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }
                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantNameEmpty;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(inputDto.Code))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantCodeEmpty;
                    return resultDto;
                }
                //if (string.IsNullOrEmpty(inputDto.Email))
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.Message = Constants.EmailEmpty;
                //    return resultDto;
                //}

                var plantNameValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Name == inputDto.Name && c.Id != inputDto.Id && c.IsPlant);
                if (plantNameValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantNameExists;
                    return resultDto;
                }
                var plantCodeValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Code == inputDto.Code && c.Id != inputDto.Id && c.IsPlant);
                if (plantCodeValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantCodeExists;
                    return resultDto;
                }
                var plantEmailValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Email == inputDto.Email && inputDto.Email != null && c.Id != inputDto.Id && c.IsPlant);
                if (plantEmailValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.EmailExists;
                    return resultDto;
                }

                //var plantEntity = new Plant();
                //plantEntity.Code = plantDto.PlantCode;
                //plantEntity.Name = plantDto.PlantName;
                //plantEntity.Email = plantDto.Email;
                //plantEntity.StateId = plantDto.StateId;
                //plantEntity.TerritoryId = plantDto.TerritoryId;
                //plantEntity.DistrictId = plantDto.DistrictId;
                //plantEntity.CityId = plantDto.CityId;
                //plantEntity.Pincode = plantDto.PinCode;
                //plantEntity.Location = plantDto.PlantLocation;
                //plantEntity.IsActive = plantDto.IsActive;
                //plantEntity.CreatedBy = plantDto.UserId;
                //plantEntity.CreatedDate = DateTime.UtcNow;

                //_emamiContext.Plants.Add(plantEntity);
                //_emamiContext.SaveChanges();

                var depotEntity = new Depot();
                depotEntity.Code = inputDto.Code;
                depotEntity.Name = inputDto.Name;
                depotEntity.Email = inputDto.Email;
                //depotEntity.StateId = inputDto.StateId;
                //depotEntity.ZoneId = inputDto.ZoneId;
                ////depotEntity.TerritoryId = inputDto.TerritoryId;
                //depotEntity.DistrictId = inputDto.DistrictId;
                //depotEntity.CityId = inputDto.CityId;
                depotEntity.Pincode = inputDto.PinCode;
                depotEntity.Location = inputDto.Location;
                depotEntity.IsActive = inputDto.IsActive;
                depotEntity.IsPlant = inputDto.IsPlant;
                depotEntity.CreatedBy = inputDto.UserId;
                depotEntity.MobileNumber = inputDto.MobileNumber;
                depotEntity.Usage = inputDto.Usage;
                depotEntity.StorageTypeId = (int)StorageType.Plant;
                depotEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.Depots.Add(depotEntity);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdatePlants(DepotDto inputDto)
        {
            _methodName = "AddPlants";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                    inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }
                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantNameEmpty;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(inputDto.Code))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantCodeEmpty;
                    return resultDto;
                }
                //if (string.IsNullOrEmpty(inputDto.Email))
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.Message = Constants.EmailEmpty;
                //    return resultDto;
                //}

                var plantId = _emamiContext.Depots.AsNoTracking().FirstOrDefault(c => c.Id == inputDto.Id && c.IsPlant);
                if (plantId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                var plantNameValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Name == inputDto.Name && c.Id != inputDto.Id && c.IsPlant);
                if (plantNameValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantNameExists;
                    return resultDto;
                }
                var plantCodeValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Code == inputDto.Code && c.Id != inputDto.Id && c.IsPlant);
                if (plantCodeValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PlantCodeExists;
                    return resultDto;
                }
                var plantEmailValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Email == inputDto.Email && inputDto.Email != null && c.Id != inputDto.Id && c.IsPlant);
                if (plantEmailValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.EmailExists;
                    return resultDto;
                }

                var entity = _emamiContext.Depots.FirstOrDefault(c => c.Id == inputDto.Id && c.IsPlant);
                if (entity != null)
                {
                    entity.Code = inputDto.Code;
                    entity.Name = inputDto.Name;
                    entity.Email = inputDto.Email;
                    //entity.ZoneId = inputDto.ZoneId;
                    //entity.StateId = inputDto.StateId;
                    ////entity.TerritoryId = inputDto.TerritoryId;
                    //entity.DistrictId = inputDto.DistrictId;
                    //entity.CityId = inputDto.CityId;
                    entity.Pincode = inputDto.PinCode;
                    entity.Location = inputDto.Location;
                    entity.IsActive = inputDto.IsActive;
                    entity.IsPlant = inputDto.IsPlant;
                    entity.ModifiedBy = inputDto.UserId;
                    entity.MobileNumber = inputDto.MobileNumber;
                    entity.Usage = inputDto.Usage;
                    entity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetPlantMasterById(DepotDto inputDto)
        {
            _methodName = "GetPlantMaster";
            var resultDto = new ResultDto();
            var input = new DepotDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                var depotData = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id && f.IsPlant);
                if (depotData != null)
                {
                    input = new DepotDto()
                    {
                        Id = depotData.Id,
                        EncryptedId = inputDto.EncryptedId,
                        Code = depotData.Code,
                        Name = depotData.Name,
                        Email = depotData.Email,
                        //ZoneId = depotData.ZoneId,
                        //StateId = depotData.StateId,
                        ////TerritoryId = depotData.TerritoryId,
                        //DistrictId = depotData.DistrictId,
                        //CityId = depotData.CityId,
                        PinCode = depotData.Pincode,
                        MobileNumber = depotData.MobileNumber,
                        Location = depotData.Location,
                        IsActive = depotData.IsActive,
                        Usage = depotData.Usage
                    };
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                resultDto.SuccessDto.Response = input;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetPlantMasterddl()
        {
            _methodName = "GetPlantMasterddl";
            var resultDto = new ResultDto();
            try
            {
                var plantList = _emamiContext.Depots.AsNoTracking().Where(_ => _.IsPlant && _.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name + "-" + s.Code
                    }).ToList();

                resultDto.SuccessDto.Response = plantList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetPlantMasterddlbased(PlantDDLDto plant)
        {
            _methodName = "GetPlantMasterddlbased";
            var resultDto = new ResultDto();
            try
            {
                var plantList = _emamiContext.Depots.AsNoTracking().Select(s => new DropDownDto()
                {
                    Id = s.Id,
                    Name = s.Name + "-" + s.Code
                }).ToList();

                resultDto.SuccessDto.Response = plantList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }
        public ResultDto ExportPlantMaster(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportPlantMaster";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Depot> plantData;
                if (loginUserIdDto.IsToReturnInactiveData)
                    plantData = _emamiContext.Depots.AsNoTracking().Where(_ => _.IsPlant);
                else
                    plantData = _emamiContext.Depots.AsNoTracking()
                  .Where(_ => _.IsPlant && _.IsActive);

                var outputDto = plantData.ToList()
                    .Select(s => new DepotDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        Email = s.Email,
                        //ZoneId = s.ZoneId,
                        MobileNumber = s.MobileNumber,
                        //ZoneName = s.Zone != null ? s.Zone.Name : string.Empty,
                        //StateId = s.StateId,
                        //State = s.State?.StateName,
                        //DistrictId = s.DistrictId,
                        //District = s.District?.DistrictName,
                        //CityId = s.CityId,
                        //City = s.City?.CityName,
                        PinCode = s.Pincode,
                        Location = s.Location,
                        IsActive = s.IsActive,
                        //TerritoryId = s.TerritoryId,
                        //TerritoryName = s.Territory?.Name,
                        Usage = s?.Usage
                    }).ToList();

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetPlantListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetPlantListWithPagination";
            var resultDto = new ResultDto();
            try
            {
                List<Depot> plantData;
                if (inputDto.IsToReturnInactiveData)
                    plantData = _emamiContext.Depots.AsNoTracking().Where(_ => _.IsPlant).ToList();
                else
                    plantData = _emamiContext.Depots.AsNoTracking()
                  .Where(_ => _.IsPlant && _.IsActive).ToList();

                var outputDto = plantData.AsEnumerable()
                    .Select(s => new DepotDto()
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        Email = s.Email,
                        MobileNumber = s.MobileNumber,
                        //ZoneId = s.ZoneId,
                        //ZoneName = s.Zone != null ? s.Zone.Name : string.Empty,
                        //StateId = s.StateId,
                        //State = s.State?.StateName,
                        //DistrictId = s.DistrictId,
                        //District = s.District?.DistrictName,
                        //CityId = s.CityId,
                        //City = s.City?.CityName,
                        PinCode = s.Pincode,
                        Location = s.Location,
                        IsActive = s.IsActive,
                        //TerritoryId = s.TerritoryId,
                        //TerritoryName = s.Territory?.Name,
                        Usage = s?.Usage
                    }).ToList();

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Depot Master
        public ResultDto GetTransportModeBasedonDepotRake(IdInputDto inputDto)
        {
            _methodName = "GetTransportModeBasedonDepotRake";
            var resultDto = new ResultDto();
            try
            {
                var transporList = new List<DropDownDto>();
                var storageTypeId = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id)?.StorageTypeId;
                if ((storageTypeId != null && storageTypeId > 0 && storageTypeId == (int)StorageType.Plant)
                    || (storageTypeId != null && storageTypeId > 0 && storageTypeId == (int)StorageType.Depot))
                {
                    transporList = _emamiContext.TransportModes.AsNoTracking().Where(w => w.Id == (int)DTO.Enums.TransportMode.Truck).Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();
                }
                else if (storageTypeId != null && storageTypeId > 0 && storageTypeId == (int)StorageType.Rake)
                {
                    transporList = _emamiContext.TransportModes.AsNoTracking().Where(w => w.Id == (int)DTO.Enums.TransportMode.Rake).Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();
                }
                resultDto = _resultService.SuccessObject(transporList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetDepotPlantddList(IdInputDto inputDto)
        {
            try
            {
                var result = new List<DepotRakeDto>();
                if (inputDto.IsToReturnInactiveData)
                {
                    result = _emamiContext.Depots.AsNoTracking().Where(w => w.StorageTypeId == (int)StorageType.Plant || w.StorageTypeId == (int)StorageType.Depot)
                    .Select(s => new DepotRakeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        StorageType = s.StorageTypeId == (int)StorageType.Plant ? StorageType.Plant.ToString() : s.StorageTypeId == (int)StorageType.Depot ? StorageType.Depot.ToString() : ""
                    }).OrderBy(o => o.StorageType).ToList();
                }
                else
                {
                    result = _emamiContext.Depots.AsNoTracking().Where(w => w.IsActive && w.StorageTypeId == (int)StorageType.Plant || w.StorageTypeId == (int)StorageType.Depot)
                    .Select(s => new DepotRakeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        StorageType = s.StorageTypeId == (int)StorageType.Plant ? StorageType.Plant.ToString() : s.StorageTypeId == (int)StorageType.Depot ? StorageType.Depot.ToString() : ""
                    }).OrderBy(o => o.StorageType).ToList();
                }
                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDepotList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetDepotList";
            var resultDto = new ResultDto();
            try
            {
                var depotsList = new List<DropDownDto>();
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    depotsList = _emamiContext.Depots.AsNoTracking().Where(_ => !_.IsPlant && _.StorageTypeId == (int)StorageType.Depot).Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name
                    }).OrderBy(o => o.Name).ToList();
                }
                else
                {
                    depotsList = _emamiContext.Depots.AsNoTracking()
                  .Where(_ => _.IsActive && !_.IsPlant && _.StorageTypeId == (int)StorageType.Depot).Select(s => new DropDownDto()
                  {
                      Id = s.Id,
                      Code = s.Code,
                      Name = s.Name
                  }).OrderBy(o => o.Name).ToList();
                }

                resultDto.SuccessDto.Response = depotsList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }
        public ResultDto GetDepotMaster(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetDepotMaster";
            var resultDto = new ResultDto();
            try
            {
                var outputDto = new List<DepotDto>();
                var depots = new List<Depot>();
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    depots = _emamiContext.Depots.AsNoTracking().Where(_ => !_.IsPlant && _.StorageTypeId == (int)StorageType.Depot).ToList();
                }
                else
                {
                    depots = _emamiContext.Depots.AsNoTracking()
                  .Where(_ => _.IsActive && !_.IsPlant && _.StorageTypeId == (int)StorageType.Depot).ToList();
                }

                if (depots != null && depots.Any())
                {
                    outputDto = depots.Select(s => new DepotDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        Email = s.Email,
                        //ZoneId = s.ZoneId,
                        //ZoneName = s.Zone != null ? s.Zone.Name : string.Empty,
                        //StateId = s.StateId,
                        //State = s.State?.StateName,
                        ////TerritoryName = s.Territory?.Name,
                        ////TerritoryId = s.TerritoryId,
                        //DistrictId = s.DistrictId,
                        //District = s.District?.DistrictName,
                        //CityId = s.CityId,
                        //City = s.City?.CityName,
                        PinCode = s.Pincode,
                        Location = s.Location,
                        IsActive = s.IsActive,
                        IsPlant = s.IsPlant,
                        PlantCode = string.Join(",", GetDepotMappedPlantCode(s.Id).ToList())
                        //Usage = s?.Usage
                    }).ToList();
                }

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto AddDepots(DepotDto depotDto)
        {
            _methodName = "AddDepots";
            var resultDto = new ResultDto();
            try
            {
                if (depotDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(depotDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotNameEmpty;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(depotDto.Code))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotCodeEmpty;
                    return resultDto;
                }
                //if (string.IsNullOrEmpty(depotDto.Email))
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.Message = Constants.EmailEmpty;
                //    return resultDto;
                //}

                var depotNameValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Name == depotDto.Name && !c.IsPlant);
                if (depotNameValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotNameExists;
                    return resultDto;
                }
                var depotCodeValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Code == depotDto.Code && !c.IsPlant);
                if (depotCodeValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotCodeExists;
                    return resultDto;
                }
                var depotEmailValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Email == depotDto.Email && !c.IsPlant && !string.IsNullOrEmpty(depotDto.Email));
                if (depotEmailValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.EmailExists;
                    return resultDto;
                }

                var depotEntity = new Depot();
                depotEntity.Code = depotDto.Code;
                depotEntity.Name = depotDto.Name;
                depotEntity.Email = depotDto.Email;
                //depotEntity.ZoneId = depotDto.ZoneId;
                //depotEntity.StateId = depotDto.StateId;
                ////depotEntity.TerritoryId = depotDto.TerritoryId;
                //depotEntity.DistrictId = depotDto.DistrictId;
                //depotEntity.CityId = depotDto.CityId;
                depotEntity.Pincode = depotDto.PinCode;
                depotEntity.Location = depotDto.Location;
                depotEntity.IsActive = depotDto.IsActive;
                depotEntity.IsPlant = depotDto.IsPlant;
                depotEntity.CreatedBy = depotDto.UserId;
                depotEntity.StorageTypeId = (int)StorageType.Depot;
                depotEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.Depots.Add(depotEntity);
                _emamiContext.SaveChanges();

                foreach (var plantid in depotDto.MappedPlantIds)
                {
                    var depotplantEntity = new PlantDepotMapping();
                    depotplantEntity.PlantId = plantid;
                    depotplantEntity.DepotId = depotEntity.Id;
                    depotplantEntity.CreatedBy = depotDto.UserId;
                    depotplantEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.PlantDepotMapping.Add(depotplantEntity);
                    _emamiContext.SaveChanges();
                }
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateDepots(DepotDto depotDto)
        {
            _methodName = "UpdateDepots";
            var resultDto = new ResultDto();
            try
            {
                if (depotDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(depotDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotNameEmpty;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(depotDto.Code))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotCodeEmpty;
                    return resultDto;
                }
                //if (string.IsNullOrEmpty(depotDto.Email))
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.Message = Constants.EmailEmpty;
                //    return resultDto;
                //}

                var depotData = _emamiContext.Depots.AsNoTracking().FirstOrDefault(c => c.Id == depotDto.Id);
                if (depotData == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                var depotNameValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Name == depotDto.Name && c.Id != depotDto.Id);
                if (depotNameValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotNameExists;
                    return resultDto;
                }
                var depotCodeValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Code == depotDto.Code && c.Id != depotDto.Id);
                if (depotCodeValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.DepotCodeExists;
                    return resultDto;
                }
                var depotEmailValidate = _emamiContext.Depots.AsNoTracking().Count(c => c.Email == depotDto.Email && c.Id != depotDto.Id && !string.IsNullOrEmpty(depotDto.Email));
                if (depotEmailValidate > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.EmailExists;
                    return resultDto;
                }

                var depotEntity = _emamiContext.Depots.FirstOrDefault(c => c.Id == depotDto.Id);
                depotEntity.Code = depotDto.Code;
                depotEntity.Name = depotDto.Name;
                depotEntity.Email = depotDto.Email;
                //depotEntity.ZoneId = depotDto.ZoneId;
                //depotEntity.StateId = depotDto.StateId;
                ////depotEntity.TerritoryId = depotDto.TerritoryId;
                //depotEntity.DistrictId = depotDto.DistrictId;
                //depotEntity.CityId = depotDto.CityId;
                depotEntity.Pincode = depotDto.PinCode;
                depotEntity.Location = depotDto.Location;
                depotEntity.IsActive = depotDto.IsActive;
                depotEntity.IsPlant = depotDto.IsPlant;
                depotEntity.ModifiedBy = depotDto.UserId;
                depotEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                if (!depotDto.IsPlant)
                {
                    var removedPlantIds = new List<long>();
                    var PlantIds = _emamiContext.PlantDepotMapping.Where(w => w.DepotId == depotDto.Id).Select(s => s.PlantId);

                    var newIds = depotDto.MappedPlantIds.Where(a => !PlantIds.Contains(a));
                    removedPlantIds = PlantIds.Where(a => !depotDto.MappedPlantIds.Contains(a)).ToList();

                    if (newIds != null && newIds.Any())
                    {
                        foreach (var plantid in newIds)
                        {
                            var plantDepotEntity = new PlantDepotMapping();
                            plantDepotEntity.PlantId = plantid;
                            plantDepotEntity.DepotId = depotEntity.Id;
                            plantDepotEntity.CreatedBy = depotDto.UserId;
                            plantDepotEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            _emamiContext.PlantDepotMapping.Add(plantDepotEntity);
                        }
                    }

                    if (removedPlantIds != null && removedPlantIds.Any())
                    {
                        foreach (var id in removedPlantIds)
                        {
                            var plantDepotEntity = new PlantDepotMapping();
                            plantDepotEntity = _emamiContext.PlantDepotMapping.FirstOrDefault(f => f.PlantId == id && f.DepotId == depotDto.Id);
                            _emamiContext.PlantDepotMapping.Remove(plantDepotEntity);
                        }
                    }
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetDepotMasterById(DepotDto depotDto)
        {
            _methodName = "GetDepotMasterById";
            var resultDto = new ResultDto();
            var depot = new DepotDto();
            try
            {
                if (depotDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var depotData = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == depotDto.Id);
                if (depotData != null)
                {
                    depot = new DepotDto()
                    {
                        Id = depotData.Id,
                        Code = depotData.Code,
                        Name = depotData.Name,
                        Email = depotData.Email,
                        //ZoneId = depotData.ZoneId,
                        //StateId = depotData.StateId,
                        ////TerritoryId = depotData.TerritoryId,
                        //DistrictId = depotData.DistrictId,
                        //CityId = depotData.CityId,
                        PinCode = depotData.Pincode,
                        Location = depotData.Location,
                        IsActive = depotData.IsActive,
                        //Usage = depotData.Usage
                    };

                    var mappedPlant = _emamiContext.PlantDepotMapping.AsNoTracking().Where(w => w.DepotId == depotData.Id).Select(s => s.PlantId).ToList();
                    depot.MappedPlantIds = mappedPlant;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                resultDto.SuccessDto.Response = depot;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public List<string> GetDepotMappedPlants(long depotId)
        {
            var plantsName = new List<string>();
            var plantsIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(w => w.DepotId == depotId).Select(s => s.PlantId).ToList();
            if (plantsIds != null && plantsIds.Any())
            {
                //plantsName = _emamiContext.Plants.AsNoTracking().Where(w => plantsIds.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                plantsName = _emamiContext.Depots.AsNoTracking().Where(w => plantsIds.Any(a => a == w.Id) && w.IsPlant).Select(s => s.Name).ToList();
            }
            return plantsName;
        }

        public List<string> GetDepotMappedPlantCode(long depotId)
        {
            var plantsCode = new List<string>();
            var plantsIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(w => w.DepotId == depotId).Select(s => s.PlantId).ToList();
            if (plantsIds != null && plantsIds.Any())
            {
                plantsCode = _emamiContext.Depots.AsNoTracking().Where(w => plantsIds.Any(a => a == w.Id) && w.IsPlant).Select(s => s.Code).Distinct().ToList();
            }
            return plantsCode;
        }

        public List<string> GetStatesFromIds(string stateIds)
        {
            var ids = UtilityHelper.ConvertStringToLongList(stateIds);
            var stateNames = _emamiContext.State.AsNoTracking().Where(w => ids.Contains(w.Id)).Select(s => s.StateName).ToList();
            return stateNames;
        }

        public ResultDto GetDepotAndPlantList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetDepotAndPlantList";
            var resultDto = new ResultDto();
            try
            {
                var depotList = new List<DepotDto>();
                var depots = new List<Depot>();
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    depots = _emamiContext.Depots.AsNoTracking().ToList();
                }
                else
                {
                    depots = _emamiContext.Depots.AsNoTracking()
                  .Where(_ => _.IsActive).ToList();
                }

                if (depots != null && depots.Any())
                {
                    depotList = depots.Select(s => new DepotDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        Email = s.Email,
                        //StateId = s.StateId,
                        //State = s.State?.StateName,
                        ////TerritoryName = s.Territory?.Name,
                        ////TerritoryId = s.TerritoryId,
                        //DistrictId = s.DistrictId,
                        //District = s.District?.DistrictName,
                        //CityId = s.CityId,
                        //City = s.City?.CityName,
                        PinCode = s.Pincode,
                        Location = s.Location,
                        IsActive = s.IsActive,
                        IsPlant = s.IsPlant,
                        //Usage = s?.Usage
                    }).ToList();
                }

                resultDto.SuccessDto.Response = depotList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #region Depot Based on Plant

        public ResultDto GetDepotsByPlantId(IdInputDto inputDto)
        {
            _methodName = "GetDepotsByPlantId";
            var resultDto = new ResultDto();
            var resultList = new List<DropDownDto>();
            try
            {
                var depotIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(w => w.PlantId == inputDto.Id).Select(s => s.DepotId).ToList();
                if (depotIds != null && depotIds.Any())
                {
                    resultList = _emamiContext.Depots.AsNoTracking().Where(w => depotIds.Any(a => a == w.Id) && !w.IsPlant)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Name = _.Name,
                        }).ToList();
                }

                resultDto.SuccessDto.Response = resultList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        public ResultDto ExportDepot(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportDepot";
            var resultDto = new ResultDto();
            try
            {
                var outputDto = new List<DepotDto>();
                var depots = _emamiContext.Depots.AsNoTracking().Where(_ => !_.IsPlant && _.StorageTypeId == (int)StorageType.Depot).ToList();

                if (depots != null && depots.Any())
                {
                    outputDto = depots.Select(s => new DepotDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        Email = s.Email,
                        //ZoneId = s.ZoneId,
                        //ZoneName = s.Zone != null ? s.Zone.Name : string.Empty,
                        //StateId = s.StateId,
                        //State = s.State?.StateName,
                        ////TerritoryName = s.Territory?.Name,
                        ////TerritoryId = s.TerritoryId,
                        //DistrictId = s.DistrictId,
                        //District = s.District?.DistrictName,
                        //CityId = s.CityId,
                        //City = s.City?.CityName,
                        PinCode = s.Pincode,
                        Location = s.Location,
                        IsActive = s.IsActive,
                        IsPlant = s.IsPlant,
                        PlantCode = string.Join(",", GetDepotMappedPlantCode(s.Id).ToList())
                    }).ToList();
                }

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetDepotListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetDepotListWithPagination";
            var resultDto = new ResultDto();
            try
            {
                var outputDto = new List<DepotDto>();
                IQueryable<Depot> resultData;
                if (inputDto.IsToReturnInactiveData)
                {
                    resultData = _emamiContext.Depots.AsNoTracking().Where(_ => !_.IsPlant && _.StorageTypeId == (int)StorageType.Depot);
                }
                else
                {
                    resultData = _emamiContext.Depots.AsNoTracking()
                  .Where(_ => _.IsActive && !_.IsPlant && _.StorageTypeId == (int)StorageType.Depot);
                }

                if (resultData != null && resultData.Any())
                {
                    outputDto = resultData.ToList().Select(s => new DepotDto()
                    {
                        Id = s.Id,
                        Code = s.Code,
                        Name = s.Name,
                        Email = s.Email,
                        //ZoneId = s.ZoneId,
                        //ZoneName = s.Zone != null ? s.Zone.Name : string.Empty,
                        //StateId = s.StateId,
                        //State = s.State?.StateName,
                        ////TerritoryName = s.Territory?.Name,
                        ////TerritoryId = s.TerritoryId,
                        //DistrictId = s.DistrictId,
                        //District = s.District?.DistrictName,
                        //CityId = s.CityId,
                        //City = s.City?.CityName,
                        PinCode = s.Pincode,
                        Location = s.Location,
                        IsActive = s.IsActive,
                        IsPlant = s.IsPlant,
                        PlantCode = string.Join(",", GetDepotMappedPlantCode(s.Id).ToList())
                    }).ToList();
                }

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Zone Mapping

        public ResultDto GetZoneList()
        {
            _methodName = "GetZoneList";
            var resultDto = new ResultDto();
            try
            {
                var zoneMapping = _emamiContext.Zones.AsEnumerable()
                    .GroupJoin(_emamiContext.ZoneStateMappings.AsEnumerable(), z => z.Id, s => s.ZoneId, (z, s) => new { z, States = s.Select(_ => _.State.StateName) }).ToList()
                    .Select(s => new ZoneDto()
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(s.z.Id.ToString(), SecurityConstants.EncryptionKey),
                        Id = s.z.Id,
                        Name = s.z.Name,
                        isActive = s.z.IsActive,
                        States = string.Join(",", s.States),
                    }).ToList();

                resultDto.SuccessDto.Response = zoneMapping;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetZoneListForDropdown()
        {
            _methodName = "GetZoneListForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var zoneMapping = _emamiContext.Zones.AsNoTracking().Where(_ => _.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

                resultDto.SuccessDto.Response = zoneMapping;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetStateListByZoneIdForDropdown(long zoneId)
        {
            _methodName = "GetStateListByZoneIdForDropdown";
            var resultDto = new ResultDto();
            try
            {

                var zoneMappedStates = _emamiContext.ZoneStateMappings.Where(s => s.ZoneId == zoneId && s.State.IsActive).Select(s => s.State);
                var states = zoneMappedStates.Select(st => new DropDownDto()
                {
                    Id = st.Id,
                    Name = st.StateName
                });

                resultDto.SuccessDto.Response = states.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetZoneStateList(long zoneId)
        {
            _methodName = "GetZoneStateList";
            var resultDto = new ResultDto();
            try
            {

                var zoneMappedStates = _emamiContext.ZoneStateMappings.Where(s => s.ZoneId == zoneId).Select(s => s.StateId);
                var states = _emamiContext.State.Where(s => zoneMappedStates.Contains(s.Id)).Select(st => new StateDto()
                {
                    StateId = st.Id,
                    StateName = st.StateName
                });

                resultDto.SuccessDto.Response = states.ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }


        public ResultDto GetStateListByZoneIds(List<long> zoneIds)
        {
            _methodName = "GetStateListByZoneIds";
            var resultDto = new ResultDto();

            try
            {

                var zoneMappedStates = _emamiContext.ZoneStateMappings.Where(s => zoneIds.Contains(s.ZoneId)).Select(s => s.StateId);
                var states = _emamiContext.State.Where(s => zoneMappedStates.Contains(s.Id)).Select(st => new DropDownDto()
                {
                    Id = st.Id,
                    Name = st.StateName
                }).ToList();



                resultDto.SuccessDto.Response = states;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto NewZone()
        {
            _methodName = "NewZone";
            var resultDto = new ResultDto();
            try
            {

                var zone = new Zone();
                var otherZoneStateIds = _emamiContext.ZoneStateMappings.Select(s => s.StateId).ToList();
                var zoneMappedStates = _emamiContext.ZoneStateMappings.DefaultIfEmpty().Where(s => s.ZoneId == 0).ToList();
                var zoneMapping = _emamiContext.State.AsNoTracking().ToList().Where(s => !otherZoneStateIds.Contains(s.Id)).ToList()
                    .Select(s => new CheckBoxDto()
                    {
                        Id = s.Id,
                        Name = s.StateName,
                        Checked = zoneMappedStates.Any(z => z.StateId == s.Id)
                    }).ToList();

                AddorUpdateZoneDto addorUpdateZoneDto = new AddorUpdateZoneDto()
                {
                    Id = zone.Id,
                    isActive = zone.IsActive,
                    Name = zone.Name ?? string.Empty,
                    States = zoneMapping
                };
                resultDto.SuccessDto.Response = addorUpdateZoneDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }
        public ResultDto EditZone(string zoneId)
        {
            _methodName = "EditZone";
            var resultDto = new ResultDto();
            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(zoneId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var zone = _emamiContext.Zones.FirstOrDefault(s => s.Id == Id) ?? new Zone();
                var otherZoneStateIds = _emamiContext.ZoneStateMappings.Where(s => s.ZoneId != Id).Select(s => s.StateId).ToList();
                var zoneMappedStates = _emamiContext.ZoneStateMappings.DefaultIfEmpty().Where(s => s.ZoneId == Id).ToList();
                var zoneMapping = _emamiContext.State.AsNoTracking().ToList().Where(s => !otherZoneStateIds.Contains(s.Id)).ToList()
                    .Select(s => new CheckBoxDto()
                    {
                        Id = s.Id,
                        Name = s.StateName,
                        Checked = zoneMappedStates.Any(z => z.StateId == s.Id)
                    }).ToList();

                AddorUpdateZoneDto addorUpdateZoneDto = new AddorUpdateZoneDto()
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(zone.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = zone.Id,
                    isActive = zone.IsActive,
                    Name = zone.Name ?? string.Empty,
                    States = zoneMapping
                };
                resultDto.SuccessDto.Response = addorUpdateZoneDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateZone(AddorUpdateZoneDto inputDto)
        {

            _methodName = "UpdateZone";
            var resultDto = new ResultDto();
            try
            {
                Zone zone;
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                    inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }

                if (inputDto.Id > 0)
                {
                    zone = _emamiContext.Zones.FirstOrDefault(s => s.Id == inputDto.Id);
                    if (zone != null)
                    {
                        zone.IsActive = inputDto.isActive;
                        zone.Name = inputDto.Name;
                    }
                }

                var zoneStateMapping = _emamiContext.ZoneStateMappings.Where(s => s.ZoneId == inputDto.Id).ToList();

                var newStates = inputDto.States != null ? inputDto.States.Where(s => s.Checked).Select(s => s.Id).Except(zoneStateMapping.Select(s => s.StateId)).Select(st => new ZoneStateMapping() { ZoneId = inputDto.Id, StateId = st }) : null;
                var deletedStates = inputDto.States != null ? zoneStateMapping.Select(s => s.StateId).Except(inputDto.States.Where(s => s.Checked).Select(s => s.Id)) : null;
                var deletedrow = deletedStates != null ? zoneStateMapping.Where(s => deletedStates.Contains(s.StateId)) : null;
                if (newStates != null)
                {
                    newStates.ToList().ForEach(s => _emamiContext.ZoneStateMappings.Add(s));
                }

                if (deletedrow != null)
                {
                    deletedrow.ToList().ForEach(s => _emamiContext.ZoneStateMappings.Remove(s));
                }


                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = inputDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto AddZone(AddorUpdateZoneDto dto)
        {

            _methodName = "AddZone";
            var resultDto = new ResultDto();
            try
            {
                var zoneStateMapping = new ZoneStateMapping();
                var isExists = _emamiContext.Zones.Where(_ => _.Name == dto.Name).FirstOrDefault();
                //var isExists = !_emamiContext.Zones.Any(s => s.Id != dto.Id && s.Name == dto.Name);
                if (isExists == null)
                {
                    var zone = new Zone() { IsActive = dto.isActive, Name = dto.Name, CreatedBy = dto.UserId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow), ModifiedBy = dto.UserId, ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow) };
                    _emamiContext.Zones.Add(zone);
                    _emamiContext.SaveChanges();
                    if (dto.States != null)
                    {
                        dto.States.Where(s => s.Checked).ToList().ForEach(
                       s => _emamiContext.ZoneStateMappings.Add(new ZoneStateMapping() { ZoneId = zone.Id, StateId = s.Id }));
                        _emamiContext.SaveChanges();
                    }

                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = "The zone name is Already Exists";

                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto ExportZone(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportZone";
            var resultDto = new ResultDto();
            try
            {
                var zoneMapping = _emamiContext.Zones.AsNoTracking()
                    .GroupJoin(_emamiContext.ZoneStateMappings.AsNoTracking(), z => z.Id, s => s.ZoneId, (z, s) => new { z, States = s.Select(_ => _.State.StateName) }).ToList()
                    .Select(s => new ZoneDto()
                    {
                        Id = s.z.Id,
                        Name = s.z.Name,
                        isActive = s.z.IsActive,
                        States = string.Join(",", s.States),
                    }).ToList();

                resultDto.SuccessDto.Response = zoneMapping;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Sku

        /// <summary>
        /// Method to Save Sku
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto SaveSku(SkuDto inputDto)
        {
            _methodName = "SaveSku";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }



                if (string.IsNullOrEmpty(inputDto.SkuName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ProductNameIsEmpty;
                    return resultDto;
                }

                if (string.IsNullOrEmpty(inputDto.SkuCode))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ProductCodeIsEmpty;
                    return resultDto;
                }

                var productCodeExist = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == inputDto.SkuCode
                && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.VerticalId);
                if (productCodeExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ProductCodeExist;
                    return resultDto;
                }

                var productNameExist = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuName == inputDto.SkuName
                && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.VerticalId);
                if (productNameExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ProductNameExist;
                    return resultDto;
                }

                //if (inputDto.IsBaseSku)
                //{
                //    var baseSkuExist = _emamiContext.Skus.AsNoTracking()
                //    .FirstOrDefault(f => f.OilTypeId == inputDto.OilTypeId
                //    && f.PackGroupId == inputDto.OilPackingTypeId
                //    && f.IsActive && f.IsBaseSku);
                //    if (baseSkuExist != null)
                //    {
                //        return _resultService.ErrorMessage($"OilType : {baseSkuExist.OilType.Name}, base SKU : {baseSkuExist.SkuName} already defined");
                //    }
                //}

                var sku = new Sku
                {
                    SkuName = inputDto.SkuName,
                    SkuCode = inputDto.SkuCode,
                    //DepotId = inputDto.DepotId,
                    OilTypeId = inputDto.OilTypeId,
                    PackGroupId = inputDto.OilPackingTypeId,
                    OilPackGroupTypeId = inputDto.OilPackGroupTypeId,
                    //SubCategoryId = inputDto.SubCategoryId,
                    Quantity = inputDto.Quantity,
                    UomId = inputDto.QuantityTypeUomId,
                    //ProcessCost = inputDto.ProcessCost,
                    PackTypeId = 1,
                    //VerticalId = inputDto.VerticalId,
                    SalesOrganizationId = inputDto.SalesOrganizationId,
                    DistributionChannelId = inputDto.DistributionChannelId,
                    DivisionId = inputDto.VerticalId,
                    //MaterialTypeId = inputDto.MaterialTypeId,
                    IsActive = inputDto.IsActive,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    //IsBaseSku = inputDto.IsBaseSku,
                    //IsRequiredToAttachTT = inputDto.IsRequiredToAttachTT,
                    //GrossWeight = inputDto.GrossWeight,
                    BusinessLine = inputDto.BusinessLine,
                    ParentMaterialCode = inputDto.ParentMaterialCode,
                    QuantityTypeUom = inputDto.QuantityTypeUom,
                    //PremiumAmount = inputDto.PremiumAmount,
                    //StorageLocation = inputDto.StorageLocation,
                    LineId = inputDto.LineId != null ? string.Join(",", inputDto.LineId) : null,
                    DiscountAutomationConversionUomId = inputDto.DiscountAutomationConversion_UomId,
                    DiscountAutomationConversionFactor1 = inputDto.DiscountAutomationConversionFactor1,
                    DiscountAutomationConversionFactor2 = inputDto.DiscountAutomationConversionFactor2,
                    DiscountAutomationConversionRelationUomId = inputDto.DiscountAutomationConversion_RelationalUomId
                };
                _emamiContext.Skus.Add(sku);
                _emamiContext.SaveChanges();

                //if (inputDto.ConversionFactor1 > 0)
                //{
                //    var skuUom1 = new SkuUomMapping
                //    {
                //        SkuId = sku.Id,
                //        UomId = inputDto.Conversion2_UomId,
                //        RelationUomId = 0,
                //        ConversionFactor = inputDto.ConversionFactor1,
                //        CreatedBy = inputDto.LoginUserId,
                //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                //    };
                //    _emamiContext.SkuUomMapping.Add(skuUom1);
                //}

                if (inputDto.Conversion2_UomId > 0 && inputDto.Conversion3_UomId > 0 && inputDto.ConversionFactor1 > 0 && inputDto.ConversionFactor2 > 0)
                {
                    var skuUom2 = new SkuUomMapping
                    {
                        SkuId = sku.Id,
                        UomId = inputDto.Conversion2_UomId,
                        RelationUomId = inputDto.Conversion3_UomId,
                        ConversionFactor1 = inputDto.ConversionFactor1,
                        ConversionFactor2 = inputDto.ConversionFactor2,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.SkuUomMapping.Add(skuUom2);
                }

                //if (inputDto.Conversion3_UomId > 0 && inputDto.ConversionFactor2 > 0)
                //{
                //    var skuUom3 = new SkuUomMapping
                //    {
                //        SkuId = sku.Id,
                //        UomId = inputDto.Conversion3_UomId,
                //       // RelationUomId = inputDto.Conversion3_RelationUomId,
                //        ConversionFactor = inputDto.ConversionFactor2,
                //        CreatedBy = inputDto.LoginUserId,
                //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                //    };
                //    _emamiContext.SkuUomMapping.Add(skuUom3);
                //}
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get Sku List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSkuList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSkuList";
            var resultDto = new ResultDto();
            var outputDto = new List<SkuDto>();
            try
            {
                IQueryable<Sku> resultContext;
                if (inputDto.IsToReturnInactiveData)
                {
                    resultContext = _emamiContext.Skus.AsNoTracking().Where(w => (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0));
                }
                else
                {
                    resultContext = _emamiContext.Skus.AsNoTracking().Where(w => w.IsActive && (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0));
                }

                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.Select(c => new SkuDto
                    {
                        Id = c.Id,
                        SkuName = c.SkuName,
                        SkuCode = c.SkuCode,
                        //DepotId = c.DepotId,
                        //DepotName = c.Depot != null ? c.Depot.Name : string.Empty,
                        SubCategoryId = c.SubCategoryId,
                        SubCategory = c.SubCategory != null ? c.SubCategory.Name : string.Empty,
                        OilTypeId = c.OilTypeId,
                        OilType = c.OilType != null ? c.OilType.Name + "-" + c.SalesOrganization.Code + "/" + c.DistributionChannel.Code + "/" + c.Division.Code : string.Empty,
                        OilTypeCode = c.OilType != null ? c.OilType.SAPCode : string.Empty,
                        OilPackingTypeId = c.PackGroupId,
                        OilPackingType = c.PackGroup != null ? c.PackGroup.Name : string.Empty,
                        //Quantity = c.Quantity,
                        QuantityTypeUomId = c.UomId,
                        //ProcessCost = c.ProcessCost,
                        TDAndPacktype = c.PackType != null ? c.PackType.Name : string.Empty,
                        //Vertical = c.Vertical != null ? c.Vertical.Name : string.Empty,
                        //VerticalCode = c.Vertical != null ? c.Vertical.Code : string.Empty,
                        SalesOrganizationId = c.SalesOrganizationId,
                        DistributionChannelId = c.DistributionChannelId,
                        SalesOrganization = c.SalesOrganization.Name,
                        DistributionChannel = c.DistributionChannel.Name,
                        Vertical = c.Division != null ? c.Division.Name : string.Empty,
                        VerticalCode = c.Division != null ? c.Division.Code : string.Empty,
                        IsActive = c.IsActive,
                        UOM1_No = c.UomId,
                        //MaterialTypeId = c.MaterialTypeId,
                        //MaterialTypeName = c.MaterialType != null ? c.MaterialType.Name : string.Empty,
                        //QuantityTypeUom = c.Uom != null ? c.Uom.Name : string.Empty,
                        //IsBaseSku = c.IsBaseSku,
                        //IsRequiredToAttachTT = c.IsRequiredToAttachTT,
                        BusinessLine = c.BusinessLine,
                        ParentMaterialCode = c.ParentMaterialCode,
                        QuantityTypeUom = c.QuantityTypeUom,
                        GrossWeight = c.GrossWeight,
                        //PremiumAmkkount = c.PremiumAmount,
                        StorageLocation = c.StorageLocation,
                        //LineName = _emamiContext.Line.Where(_ => c.LineId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList().Contains(_.Id))
                    }).ToList();


                    var uomContext = _emamiContext.Uom.AsNoTracking().ToList();

                    foreach (var sku in outputDto)
                    {
                        if (sku.OilPackingTypeId == 0)
                        {
                            sku.NewlyAdded = "Yes, OilPackingTypeId Missing";
                        }
                        //if (string.IsNullOrEmpty(sku.SubCategory))
                        //{
                        //    sku.NewlyAdded = "Yes, SubCategory Missing";
                        //}
                        if (string.IsNullOrEmpty(sku.OilPackingType))
                        {
                            sku.NewlyAdded = "Yes, OilPackingType Missing";
                        }
                        //if (sku.ProcessCost == 0 && sku.Vertical.CompareTo("HBC") != 0)
                        //{
                        //    sku.NewlyAdded = "Yes, ProcessCost Missing";
                        //}
                        //if (sku.Quantity == 0)
                        //{
                        //    sku.NewlyAdded = "Yes, Quantity Missing";
                        //}
                        //if (sku.QuantityTypeUomId == 0)
                        //{
                        //    sku.NewlyAdded = "Yes, QuantityTypeUomId Missing";
                        //}

                        var skuUomMapping = _emamiContext.SkuUomMapping.FirstOrDefault(_ => _.SkuId == sku.Id);
                        //var skuUomMapping1 = skuUomMapping.FirstOrDefault(_ => _.UomId == (int)DTO.Enums.Uom.Case);
                        if (skuUomMapping != null)
                        {
                            sku.UomMappingId1 = skuUomMapping.Id;
                            sku.Conversion1_UomId = skuUomMapping.UomId;
                            sku.Conversion1_RelationUomId = skuUomMapping.RelationUomId;
                            sku.ConversionFactor1 = skuUomMapping.ConversionFactor1;
                            sku.ConversionFactor2 = skuUomMapping.ConversionFactor2;
                            sku.UOMName = uomContext.FirstOrDefault(s => s.Id == skuUomMapping.UomId).SAPName;
                        }
                        else
                        {
                            sku.NewlyAdded = "Yes, skuUomMapping Missing";
                        }

                        //var skuUomMapping2 = skuUomMapping.FirstOrDefault(_ => _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                        //if (skuUomMapping2 != null)
                        //{
                        //    sku.UomMappingId2 = skuUomMapping2.Id;
                        //    sku.Conversion2_UomId = skuUomMapping2.UomId;
                        //    sku.Conversion2_RelationUomId = skuUomMapping2.RelationUomId;
                        //    sku.ConversionFactor2 = skuUomMapping2.ConversionFactor;
                        //}
                        //else
                        //{
                        //    sku.NewlyAdded = "Yes, skuUomMapping2 Missing";
                        //}
                    }

                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSkuListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetSkuListWithPagination";
            var resultDto = new ResultDto();
            var outputDto = new List<SkuDto>();
            DataSourceResult result = new DataSourceResult();
            try
            {
                List<Sku> resultContext;
                if (inputDto.IsToReturnInactiveData)
                {
                    resultContext = _emamiContext.Skus.AsNoTracking().Where(w => (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0)).ToList();
                }
                else
                {
                    resultContext = _emamiContext.Skus.AsNoTracking().Where(w => w.IsActive && (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0)).ToList();
                }
                if (resultContext != null && resultContext.Any())
                {
                    var outputContext = resultContext.AsEnumerable().Select(c => new SkuDto
                    {
                        Id = c.Id,
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        SkuName = c.SkuName,
                        SkuCode = c.SkuCode,
                        //DepotId = c.DepotId,
                        //DepotName = c.Depot != null ? c.Depot.Name : string.Empty,
                        SubCategoryId = c.SubCategoryId,
                        SubCategory = c.SubCategory != null ? c.SubCategory.Name : string.Empty,
                        OilTypeId = c.OilTypeId,
                        OilType = c.OilType != null ? c.OilType.Name + "-" + c.SalesOrganization.Code + "/" + c.DistributionChannel.Code + "/" + c.Division.Code : string.Empty,
                        //OilTypeCode= c.OilType != null ? c.OilType.Name+"/"+c.SalesOrganization.Code+"/"+c.DistributionChannel.Code+"/"+c.Division.Code : string.Empty,
                        OilPackingTypeId = c.PackGroupId,
                        OilPackingBPCPTypeId = c.OilPackGroupTypeId,
                        OilPackingBPCPType = c.OilPackGroupTypeId != null && c.OilPackGroupTypeId > 0 ? UtilityHelper.GetEnumDescription((BpCpType)c.OilPackGroupTypeId) : "",
                        OilPackingType = c.PackGroup != null ? c.PackGroup.Name : string.Empty,
                        //Quantity = c.Quantity,
                        QuantityTypeUomId = c.UomId,
                        //ProcessCost = c.ProcessCost,
                        TDAndPacktype = c.PackType != null ? c.PackType.Name : string.Empty,
                        //Vertical = c.Vertical != null ? c.Vertical.Name : string.Empty,
                        //VerticalCode = c.Vertical != null ? c.Vertical.Code : string.Empty,
                        SalesOrganization = c.SalesOrganization.Name,
                        DistributionChannel = c.DistributionChannel.Name,
                        Vertical = c.Division != null ? c.Division.Name : string.Empty,
                        VerticalCode = c.Division != null ? c.Division.Code : string.Empty,
                        IsActive = c.IsActive,
                        UOM1_No = c.UomId,
                        //MaterialTypeId = c.MaterialTypeId,
                        //MaterialTypeName = c.MaterialType != null ? c.MaterialType.Name : string.Empty,
                        //QuantityTypeUom = c.Uom != null ? c.Uom.Name : string.Empty,
                        //IsBaseSku = c.IsBaseSku,
                        //IsRequiredToAttachTT = c.IsRequiredToAttachTT,
                        GrossWeight = c.GrossWeight,
                        BusinessLine = c.BusinessLine,
                        ParentMaterialCode = c.ParentMaterialCode,
                        QuantityTypeUom = c.QuantityTypeUom,
                        //PremiumAmount = c.PremiumAmount,
                        StorageLocation = c.StorageLocation,
                        LineId = c.LineId != null ? c.LineId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList() : null
                    });

                    outputDto = outputContext.ToList();
                    foreach (var sku in outputDto)
                    {
                        if (sku.OilPackingTypeId == 0)
                        {
                            sku.NewlyAdded = "Yes, OilPackingTypeId Missing";
                        }
                        //if (string.IsNullOrEmpty(sku.SubCategory))
                        //{
                        //    sku.NewlyAdded = "Yes, SubCategory Missing";
                        //}
                        if (string.IsNullOrEmpty(sku.OilPackingType))
                        {
                            sku.NewlyAdded = "Yes, OilPackingType Missing";
                        }
                        ////if (sku.ProcessCost == 0 && sku.Vertical.CompareTo("HBC") != 0)
                        ////{
                        ////    sku.NewlyAdded = "Yes, ProcessCost Missing";
                        ////}
                        //if (sku.Quantity == 0)
                        //{
                        //    sku.NewlyAdded = "Yes, Quantity Missing";
                        //}
                        //if (sku.QuantityTypeUomId == 0)
                        //{
                        //    sku.NewlyAdded = "Yes, QuantityTypeUomId Missing";
                        //}

                        var skuUomMapping = _emamiContext.SkuUomMapping.FirstOrDefault(_ => _.SkuId == sku.Id);
                        //var skuUomMapping1 = skuUomMapping.FirstOrDefault(_ => _.UomId == (int)DTO.Enums.Uom.Case);
                        if (skuUomMapping != null)
                        {
                            sku.UomMappingId1 = skuUomMapping.Id;
                            sku.Conversion1_UomId = skuUomMapping.UomId;
                            sku.Conversion1_RelationUomId = skuUomMapping.RelationUomId;
                            sku.ConversionFactor1 = skuUomMapping.ConversionFactor1;
                            sku.ConversionFactor2 = skuUomMapping.ConversionFactor2;
                        }
                        else
                        {
                            sku.NewlyAdded = "Yes, skuUomMapping Missing";
                        }

                        //var skuUomMapping2 = skuUomMapping.FirstOrDefault(_ => _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                        //if (skuUomMapping2 != null)
                        //{
                        //    sku.UomMappingId2 = skuUomMapping2.Id;
                        //    sku.Conversion2_UomId = skuUomMapping2.UomId;
                        //    sku.Conversion2_RelationUomId = skuUomMapping2.RelationUomId;
                        //    sku.ConversionFactor2 = skuUomMapping2.ConversionFactor;
                        //}
                        //else
                        //{
                        //    sku.NewlyAdded = "Yes, skuUomMapping2 Missing";
                        //}
                    }

                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : result;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to get Get Sku Details By Id
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public ResultDto GetSkuDetailsById(string skuId)
        {
            _methodName = "GetSkuDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new SkuDto();
            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(skuId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    outputDto.EncryptedId = skuId;
                    outputDto.Id = resultContext.Id;
                    outputDto.SkuName = resultContext.SkuName;
                    outputDto.SkuCode = resultContext.SkuCode;
                    //outputDto.DepotId = resultContext.DepotId;
                    //outputDto.DepotName = resultContext.Depot != null ? resultContext.Depot.Name : string.Empty;
                    outputDto.SubCategoryId = resultContext.SubCategoryId;
                    outputDto.SubCategory = resultContext.SubCategory != null ? resultContext.SubCategory.Name : string.Empty;
                    outputDto.OilTypeId = resultContext.OilTypeId;
                    outputDto.OilType = resultContext.OilType != null ? resultContext.OilType.Name : string.Empty;
                    outputDto.OilPackingTypeId = resultContext.PackGroupId;
                    outputDto.OilPackingType = resultContext.PackGroup != null ? resultContext.PackGroup.Name : string.Empty;
                    outputDto.OilPackGroupTypeId = resultContext.OilPackGroupTypeId;
                    //outputDto.QuantityTypeUomId = resultContext.UomId;
                    outputDto.Quantity = resultContext.Quantity;
                    //outputDto.ProcessCost = resultContext.ProcessCost;
                    outputDto.SalesOrganizationId = resultContext.SalesOrganizationId;
                    outputDto.DistributionChannelId = resultContext.DistributionChannelId;
                    outputDto.SalesOrganization = resultContext.SalesOrganization.Name;
                    outputDto.DistributionChannel = resultContext.DistributionChannel.Name;
                    //outputDto.VerticalId = resultContext.VerticalId;
                    outputDto.VerticalId = resultContext.DivisionId;
                    outputDto.PackTypeId = resultContext.PackTypeId;
                    outputDto.IsActive = resultContext.IsActive;
                    //outputDto.MaterialTypeId = resultContext.MaterialTypeId;
                    //outputDto.MaterialTypeName = resultContext.MaterialType?.Name;
                    //outputDto.IsBaseSku = resultContext.IsBaseSku;
                    //outputDto.IsRequiredToAttachTT = resultContext.IsRequiredToAttachTT;
                    outputDto.GrossWeight = resultContext.GrossWeight;
                    outputDto.BusinessLine = resultContext.BusinessLine;
                    outputDto.ParentMaterialCode = resultContext.ParentMaterialCode;
                    outputDto.QuantityTypeUom = resultContext.QuantityTypeUom;
                    outputDto.SalesOrganization = resultContext.SalesOrganization.Name;
                    outputDto.DistributionChannel = resultContext.DistributionChannel.Name;
                    //outputDto.PremiumAmount = resultContext.PremiumAmount;
                    outputDto.StorageLocation = resultContext.StorageLocation;
                    outputDto.LineId = resultContext.LineId != null ? resultContext.LineId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList() : null;
                    outputDto.DiscountAutomationConversion_UomId = resultContext.DiscountAutomationConversionUomId;
                    outputDto.DiscountAutomationConversion_RelationalUomId = resultContext.DiscountAutomationConversionRelationUomId;
                    outputDto.DiscountAutomationConversionFactor1 = resultContext.DiscountAutomationConversionFactor1;
                    outputDto.DiscountAutomationConversionFactor2 = resultContext.DiscountAutomationConversionFactor2;
                }

                //Get Sku Mapping records based on Sku and Uom
                var skuUomMapping = _emamiContext.SkuUomMapping.FirstOrDefault(_ => _.SkuId == Id);
                // var skuUomMapping1 = skuUomMapping.FirstOrDefault(_ => _.UomId == (int)DTO.Enums.Uom.Nos && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                if (skuUomMapping != null)
                {
                    outputDto.UomMappingId1 = skuUomMapping.Id;
                    // outputDto.Conversion1_UomId = skuUomMapping.UomId;
                    outputDto.Conversion2_UomId = skuUomMapping.UomId;
                    outputDto.ConversionFactor1 = skuUomMapping.ConversionFactor1;
                    outputDto.ConversionFactor2 = skuUomMapping.ConversionFactor2;
                }

                //var skuUomMapping2 = skuUomMapping.FirstOrDefault(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                //if (skuUomMapping2 != null)
                //{
                //    outputDto.UomMappingId2 = skuUomMapping2.Id;
                //    outputDto.Conversion2_UomId = skuUomMapping2.UomId;
                //    outputDto.Conversion2_RelationUomId = skuUomMapping2.RelationUomId;
                //    outputDto.ConversionFactor2 = skuUomMapping2.ConversionFactor;
                //}

                //var skuUomMapping3 = skuUomMapping.FirstOrDefault(_ => _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                //if (skuUomMapping3 != null)
                //{
                //    outputDto.UomMappingId3 = skuUomMapping3.Id;
                //    outputDto.Conversion3_UomId = skuUomMapping3.UomId;
                //    outputDto.Conversion3_RelationUomId = skuUomMapping3.RelationUomId;
                //    outputDto.ConversionFactor3 = skuUomMapping3.ConversionFactor;
                //}



                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Update Sku
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateSku(SkuDto inputDto)
        {
            _methodName = "UpdateSku";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                    inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }

                var productCodeExist = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == inputDto.SkuCode && _.Id != inputDto.Id
                && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.VerticalId);
                if (productCodeExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ProductCodeExist;
                    return resultDto;
                }

                var productNameExist = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuName == inputDto.SkuName && _.Id != inputDto.Id
                && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.VerticalId);
                if (productNameExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ProductNameExist;
                    return resultDto;
                }
                //if (inputDto.IsBaseSku)
                //{
                //    var baseSkuExist = _emamiContext.Skus.AsNoTracking()
                //    .FirstOrDefault(f => f.OilTypeId == inputDto.OilTypeId
                //    && f.PackGroupId == inputDto.OilPackingTypeId
                //    && f.IsActive && f.IsBaseSku && f.Id != inputDto.Id);
                //    if (baseSkuExist != null)
                //    {
                //        return _resultService.ErrorMessage($"OilType : {baseSkuExist.OilType.Name} base SKU : {baseSkuExist.SkuName} already defined");
                //    }
                //}

                var result = _emamiContext.Skus.FirstOrDefault(_ => _.Id == inputDto.Id);
                result.OilTypeId = inputDto.OilTypeId;
                //result.SubCategoryId = inputDto.SubCategoryId;
                result.PackGroupId = inputDto.OilPackingTypeId;
                //result.DepotId = inputDto.DepotId;
                result.SkuName = inputDto.SkuName;
                result.UomId = inputDto.QuantityTypeUomId;
                //result.Quantity = inputDto.Quantity;
                //result.QuantityTypeUom = inputDto.QuantityTypeUom;
                //result.ProcessCost = inputDto.ProcessCost;
                result.PackTypeId = 1;
                result.OilPackGroupTypeId = inputDto.OilPackGroupTypeId;
                result.SalesOrganizationId = inputDto.SalesOrganizationId;
                result.DistributionChannelId = inputDto.DistributionChannelId;
                //result.VerticalId = inputDto.VerticalId;
                result.DivisionId = inputDto.VerticalId;
                result.IsActive = inputDto.IsActive;
                //result.MaterialTypeId = inputDto.MaterialTypeId;
                result.ModifiedBy = inputDto.LoginUserId;
                result.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //result.IsBaseSku = inputDto.IsBaseSku;
                //result.IsRequiredToAttachTT = inputDto.IsRequiredToAttachTT;
                //result.GrossWeight = inputDto.GrossWeight;
                result.BusinessLine = inputDto.BusinessLine;
                result.ParentMaterialCode = inputDto.ParentMaterialCode;
                //result.PremiumAmount = inputDto.PremiumAmount;
                //result.StorageLocation = inputDto.StorageLocation;
                result.LineId = inputDto.LineId != null ? string.Join(",", inputDto.LineId) : string.Empty;

                result.DiscountAutomationConversionUomId = inputDto.DiscountAutomationConversion_UomId;
                result.DiscountAutomationConversionFactor1 = inputDto.DiscountAutomationConversionFactor1;
                result.DiscountAutomationConversionFactor2 = inputDto.DiscountAutomationConversionFactor2;
                result.DiscountAutomationConversionRelationUomId = inputDto.DiscountAutomationConversion_RelationalUomId;

                var isSkuMappingExists = _emamiContext.SkuUomMapping.Where(_ => _.SkuId == inputDto.Id).ToList();
                if (isSkuMappingExists != null && isSkuMappingExists.Any())
                {
                    foreach (var uom in isSkuMappingExists)
                    {
                        _emamiContext.SkuUomMapping.Remove(uom);
                        _emamiContext.SaveChanges();
                    }
                }

                //if (inputDto.ConversionFactor1 > 0)
                //{
                //    var skuUom1 = new SkuUomMapping
                //    {
                //        SkuId = inputDto.Id,
                //        UomId = (int)DTO.Enums.Uom.Nos,
                //        RelationUomId = (int)DTO.Enums.Uom.Nos,
                //        ConversionFactor = inputDto.ConversionFactor1,
                //        CreatedBy = inputDto.LoginUserId,
                //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                //    };
                //    _emamiContext.SkuUomMapping.Add(skuUom1);
                //}

                if (inputDto.Conversion2_UomId > 0 && inputDto.Conversion3_UomId > 0 && inputDto.ConversionFactor1 > 0 && inputDto.ConversionFactor2 > 0)
                {
                    var skuUom2 = new SkuUomMapping
                    {
                        SkuId = inputDto.Id,
                        UomId = inputDto.Conversion2_UomId,
                        RelationUomId = inputDto.Conversion2_UomId,
                        ConversionFactor1 = inputDto.ConversionFactor1,
                        ConversionFactor2 = inputDto.ConversionFactor2,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.SkuUomMapping.Add(skuUom2);
                }

                //if (inputDto.Conversion3_UomId > 0 && inputDto.ConversionFactor3 > 0)
                //{
                //    var skuUom3 = new SkuUomMapping
                //    {
                //        SkuId = inputDto.Id,
                //        UomId = inputDto.Conversion3_UomId,
                //        RelationUomId = inputDto.Conversion3_RelationUomId,
                //        ConversionFactor = inputDto.ConversionFactor3,
                //        CreatedBy = inputDto.LoginUserId,
                //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                //    };
                //    _emamiContext.SkuUomMapping.Add(skuUom3);
                //}


                //var isSkuMappingExists = _emamiContext.SkuUomMapping.Where(_ => _.SkuId == inputDto.Id);
                //if (isSkuMappingExists != null && isSkuMappingExists.Any())
                //{
                //    //Update Sku Mapping records based on Sku and Uom
                //    var skuUomMapping1 = isSkuMappingExists.FirstOrDefault(_ => _.Id == inputDto.UomMappingId1); 
                //    if (skuUomMapping1 != null)
                //    {
                //        skuUomMapping1.UomId = (int)DTO.Enums.Uom.Nos;
                //        skuUomMapping1.RelationUomId = (int)DTO.Enums.Uom.Nos;
                //        skuUomMapping1.ConversionFactor = inputDto.ConversionFactor1;
                //        skuUomMapping1.ModifiedBy = inputDto.LoginUserId;
                //        skuUomMapping1.ModifiedDate = DateTime.UtcNow;

                //    }

                //    var skuUomMapping2 = isSkuMappingExists.FirstOrDefault(_ => _.Id == inputDto.UomMappingId2);    
                //    if (skuUomMapping2 != null)
                //    {
                //        skuUomMapping2.UomId = inputDto.Conversion2_UomId;
                //        skuUomMapping2.RelationUomId = inputDto.Conversion2_RelationUomId;
                //        skuUomMapping2.ConversionFactor = inputDto.ConversionFactor2;
                //        skuUomMapping2.ModifiedBy = inputDto.LoginUserId;
                //        skuUomMapping2.ModifiedDate = DateTime.UtcNow;

                //    }

                //    var skuUomMapping3 = isSkuMappingExists.FirstOrDefault(_ => _.Id == inputDto.UomMappingId3);
                //    if (skuUomMapping3 != null)
                //    {
                //        skuUomMapping3.UomId = inputDto.Conversion3_UomId;
                //        skuUomMapping3.RelationUomId = inputDto.Conversion3_RelationUomId;
                //        skuUomMapping3.ConversionFactor = inputDto.ConversionFactor3;
                //        skuUomMapping3.ModifiedBy = inputDto.LoginUserId;
                //        skuUomMapping3.ModifiedDate = DateTime.UtcNow;

                //    }                    
                //}
                //else
                //{
                //    if (inputDto.ConversionFactor1 > 0)
                //    {
                //        var skuUom1 = new SkuUomMapping
                //        {
                //            SkuId = inputDto.Id,
                //            UomId = inputDto.Conversion1_UomId,
                //            RelationUomId = inputDto.Conversion1_RelationUomId,
                //            ConversionFactor = inputDto.ConversionFactor1,
                //            CreatedBy = inputDto.LoginUserId,
                //            CreatedDate = DateTime.UtcNow,
                //        };
                //        _emamiContext.SkuUomMapping.Add(skuUom1);
                //    }

                //    if (inputDto.Conversion2_UomId > 0 && inputDto.ConversionFactor2 > 0)
                //    {
                //        var skuUom2 = new SkuUomMapping
                //        {
                //            SkuId = inputDto.Id,
                //            UomId = inputDto.Conversion2_UomId,
                //            RelationUomId = inputDto.Conversion2_RelationUomId,
                //            ConversionFactor = inputDto.ConversionFactor2,
                //            CreatedBy = inputDto.LoginUserId,
                //            CreatedDate = DateTime.UtcNow,
                //        };
                //        _emamiContext.SkuUomMapping.Add(skuUom2);
                //    }

                //    if (inputDto.Conversion3_UomId > 0 && inputDto.ConversionFactor3 > 0)
                //    {
                //        var skuUom3 = new SkuUomMapping
                //        {
                //            SkuId = inputDto.Id,
                //            UomId = inputDto.Conversion3_UomId,
                //            RelationUomId = inputDto.Conversion3_RelationUomId,
                //            ConversionFactor = inputDto.ConversionFactor3,
                //            CreatedBy = inputDto.LoginUserId,
                //            CreatedDate = DateTime.UtcNow,
                //        };
                //        _emamiContext.SkuUomMapping.Add(skuUom3);
                //    }


                //}
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region State

        public ResultDto GetStates()
        {
            _methodName = "GetStates";
            var resultDto = new ResultDto();
            var statesDto = new List<StateDto>();
            try
            {
                statesDto = _emamiContext.State.AsEnumerable().OrderByDescending(_ => _.StateName).Select(c => new StateDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    StateId = c.Id,
                    StateName = c.StateName,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = statesDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddStates(AddStateDto addStateDto)
        {
            _methodName = "AddStates";
            var resultDto = new ResultDto();
            try
            {
                if (addStateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(addStateDto.StateName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var stateContext = _emamiContext.State.AsNoTracking().Count(_ => _.StateName == addStateDto.StateName);
                if (stateContext > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameExist;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }
                var addstateContext = new State
                {
                    StateName = addStateDto.StateName.Trim(),
                    CountryId = (int)DTO.Enums.Country.India,
                    IsActive = addStateDto.IsActive,
                    CreatedBy = addStateDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.State.Add(addstateContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.RecordSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto UpdateStates(UpdateStateDto updateStateDto)
        {
            _methodName = "UpdateStates";
            var resultDto = new ResultDto();
            try
            {
                if (updateStateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(updateStateDto.EncryptedId))
                {
                    updateStateDto.EncryptedId = updateStateDto.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(updateStateDto.EncryptedId, SecurityConstants.EncryptionKey);

                    updateStateDto.StateId = UtilityHelper.IntTryToParse(decryptedId);
                }
                if (updateStateDto.StateId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.IdEmpty;
                    resultDto.ErrorDto.Message = Constants.IdEmpty;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(updateStateDto.StateName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var stateContextExist = _emamiContext.State.AsNoTracking().Count(_ => _.StateName == updateStateDto.StateName && _.Id != updateStateDto.StateId);
                if (stateContextExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameExist;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }
                var stateContext = _emamiContext.State.FirstOrDefault(_ => _.Id == updateStateDto.StateId);
                if (stateContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
                stateContext.StateName = updateStateDto.StateName.Trim();
                stateContext.IsActive = updateStateDto.IsActive;
                stateContext.ModifiedBy = updateStateDto.ModifiedBy;
                stateContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ViewState(UpdateStateDto updateStateDto)
        {
            _methodName = "ViewState";
            var resultDto = new ResultDto();
            var stateDto = new StateDto();
            try
            {
                updateStateDto.EncryptedId = updateStateDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(updateStateDto.EncryptedId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var stateContext = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (stateContext != null)
                {
                    stateDto.EncryptedId = updateStateDto.EncryptedId;
                    stateDto.StateId = stateContext.Id;
                    stateDto.StateName = stateContext.StateName;
                    stateDto.IsActive = stateContext.IsActive;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ExportStates(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportStates";
            var resultDto = new ResultDto();
            var statesDto = new List<StateDto>();
            try
            {
                statesDto = _emamiContext.State.AsNoTracking().OrderByDescending(_ => _.StateName).Select(c => new StateDto
                {
                    StateId = c.Id,
                    StateName = c.StateName,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = statesDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetStateListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetStateListWithPagination";
            var resultDto = new ResultDto();
            var outputDto = new List<StateDto>();
            try
            {
                List<State> state;
                if (inputDto.IsToReturnInactiveData)
                {
                    state = _emamiContext.State.AsNoTracking().ToList();
                }
                else
                {
                    state = _emamiContext.State.AsNoTracking().Where(_ => _.IsActive).ToList();
                }

                outputDto = state.AsEnumerable().Select(c => new StateDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    StateId = c.Id,
                    StateName = c.StateName,
                    IsActive = c.IsActive
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.StateId).ToDataSourceResult(inputDto.DataSourceRequest) : outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region District

        public ResultDto GetDistricts()
        {
            _methodName = "GetDistricts";
            var resultDto = new ResultDto();
            var districtDto = new List<DistrictDto>();
            try
            {
                districtDto = _emamiContext.District.AsEnumerable().OrderByDescending(_ => _.DistrictName).Select(c => new DistrictDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    DistrictId = c.Id,
                    DistrictName = c.DistrictName,
                    StateId = c.StateId,
                    StateName = c.State.StateName,
                    // TerritoryId = c.TerritoryId,
                    //TerritoryName = c.Territory.Name,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = districtDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddDistrict(AddDistrictDto addDistrictDto)
        {
            _methodName = "AddDistrict";
            var resultDto = new ResultDto();
            try
            {
                if (addDistrictDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(addDistrictDto.DistrictName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var DistrictContext = _emamiContext.District.AsNoTracking().Count(_ => _.DistrictName == addDistrictDto.DistrictName);
                if (DistrictContext > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }
                var adddistrictContext = new District
                {
                    DistrictName = addDistrictDto.DistrictName.Trim(),
                    StateId = addDistrictDto.StateId,
                    //TerritoryId = addDistrictDto.TerritoryId,
                    IsActive = addDistrictDto.IsActive,
                    CreatedBy = addDistrictDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.District.Add(adddistrictContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto UpdateDistrict(UpdateDistrictDto updateDistrictDto)
        {
            _methodName = "UpdateDistrict";
            var resultDto = new ResultDto();
            try
            {
                if (updateDistrictDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(updateDistrictDto.EncryptedId))
                {
                    updateDistrictDto.EncryptedId = updateDistrictDto.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(updateDistrictDto.EncryptedId, SecurityConstants.EncryptionKey);

                    updateDistrictDto.DistrictId = UtilityHelper.IntTryToParse(decryptedId);
                }
                if (updateDistrictDto.DistrictId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.IdEmpty;
                    resultDto.ErrorDto.Message = Constants.IdEmpty;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(updateDistrictDto.DistrictName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var districtContextExist = _emamiContext.District.AsNoTracking().Count(_ => _.DistrictName == updateDistrictDto.DistrictName && _.Id != updateDistrictDto.DistrictId);
                if (districtContextExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameExist;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }
                var districtContext = _emamiContext.District.FirstOrDefault(_ => _.Id == updateDistrictDto.DistrictId);
                if (districtContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
                districtContext.DistrictName = updateDistrictDto.DistrictName.Trim();
                districtContext.StateId = updateDistrictDto.StateId;
                //districtContext.TerritoryId = updateDistrictDto.TerritoryId;
                districtContext.IsActive = updateDistrictDto.IsActive;
                districtContext.ModifiedBy = updateDistrictDto.ModifiedBy;
                districtContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ViewDistrict(UpdateDistrictDto updateDistrictDto)
        {
            _methodName = "ViewDistrict";
            var resultDto = new ResultDto();
            var stateDto = new DistrictDto();
            try
            {
                updateDistrictDto.EncryptedId = updateDistrictDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(updateDistrictDto.EncryptedId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var districtContext = _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (districtContext != null)
                {
                    stateDto.EncryptedId = updateDistrictDto.EncryptedId;
                    stateDto.DistrictId = districtContext.Id;
                    stateDto.DistrictName = districtContext.DistrictName;
                    stateDto.StateId = districtContext.StateId;
                    stateDto.StateName = districtContext.State.StateName;
                    // stateDto.TerritoryId = districtContext.TerritoryId;
                    //stateDto.TerritoryName = districtContext.Territory.Name;
                    stateDto.IsActive = districtContext.IsActive;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ExportDistrict(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportDistrict";
            var resultDto = new ResultDto();
            var districtDto = new List<DistrictDto>();
            try
            {
                districtDto = _emamiContext.District.AsNoTracking().OrderByDescending(_ => _.DistrictName).Select(c => new DistrictDto
                {
                    DistrictId = c.Id,
                    DistrictName = c.DistrictName,
                    StateId = c.StateId,
                    StateName = c.State.StateName,
                    //TerritoryId = c.TerritoryId,
                    //TerritoryName = c.Territory.Name,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = districtDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region City

        public ResultDto GetCities()
        {
            _methodName = "GetCities";
            var resultDto = new ResultDto();
            var cityDto = new List<CityDto>();
            try
            {
                cityDto = _emamiContext.City.AsEnumerable().OrderByDescending(_ => _.CityName).Select(c => new CityDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    CityId = c.Id,
                    CityName = c.CityName,
                    DistrictId = c.DistrictId,
                    DistrictName = c.District.DistrictName,
                    //TerritoryName = c.Territory.Name,
                    StateName = c.District.State != null ? c.District.State.StateName : string.Empty,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddCity(AddCityDto addCityDto)
        {
            _methodName = "AddCity";
            var resultDto = new ResultDto();
            try
            {
                if (addCityDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(addCityDto.CityName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var CityContext = _emamiContext.City.AsNoTracking().Count(_ => _.CityName == addCityDto.CityName
                && _.DistrictId == addCityDto.DistrictId);
                if (CityContext > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameExist;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }
                var addCityContext = new City
                {
                    CityName = addCityDto.CityName.Trim(),
                    DistrictId = addCityDto.DistrictId,
                    //TerritoryId = addCityDto.TerritoryId,
                    IsActive = addCityDto.IsActive,
                    CreatedBy = addCityDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.City.Add(addCityContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto UpdateCity(UpdateCityDto updateCityDto)
        {
            _methodName = "UpdateCity";
            var resultDto = new ResultDto();
            try
            {
                if (updateCityDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(updateCityDto.EncryptedId))
                {
                    updateCityDto.EncryptedId = updateCityDto.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(updateCityDto.EncryptedId, SecurityConstants.EncryptionKey);

                    updateCityDto.CityId = UtilityHelper.IntTryToParse(decryptedId);
                }
                if (string.IsNullOrEmpty(updateCityDto.CityName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var cityContextExist = _emamiContext.City.AsNoTracking().Count(_ => _.CityName == updateCityDto.CityName && _.Id != updateCityDto.CityId);
                if (cityContextExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameExist;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }
                var cityContext = _emamiContext.City.FirstOrDefault(_ => _.Id == updateCityDto.CityId);
                if (cityContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
                cityContext.CityName = updateCityDto.CityName.Trim();
                cityContext.DistrictId = updateCityDto.DistrictId;
                //cityContext.TerritoryId = updateCityDto.TerritoryId;
                cityContext.IsActive = updateCityDto.IsActive;
                cityContext.ModifiedBy = updateCityDto.ModifiedBy;
                cityContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ViewCity(UpdateCityDto updateCityDto)
        {
            _methodName = "ViewCity";
            var resultDto = new ResultDto();
            var cityDto = new CityDto();
            try
            {

                updateCityDto.EncryptedId = updateCityDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(updateCityDto.EncryptedId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (cityContext != null)
                {
                    cityDto.EncryptedId = updateCityDto.EncryptedId;
                    cityDto.CityId = cityContext.Id;
                    cityDto.CityName = cityContext.CityName;
                    cityDto.DistrictId = cityContext.District.Id;
                    cityDto.DistrictName = cityContext.District.DistrictName;
                    //cityDto.TerritoryId = cityContext.TerritoryId;
                    //cityDto.TerritoryName = cityContext.Territory.Name;
                    cityDto.StateId = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == cityContext.District.StateId).Id;
                    cityDto.IsActive = cityContext.IsActive;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ExportCity(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportCity";
            var resultDto = new ResultDto();
            var cityDto = new List<CityDto>();
            try
            {
                cityDto = _emamiContext.City.AsNoTracking().OrderByDescending(_ => _.CityName).Select(c => new CityDto
                {
                    CityId = c.Id,
                    CityName = c.CityName,
                    DistrictId = c.DistrictId,
                    DistrictName = c.District.DistrictName,
                    //TerritoryName = c.Territory.Name,
                    StateName = c.District.State != null ? c.District.State.StateName : string.Empty,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion        

        #region Territory

        /// <summary>
        /// Add New Territories Based on State
        /// </summary>
        /// <param name="territoryDto"></param>
        /// <returns></returns>
        public ResultDto AddTerritory(TerritoryDto territoryDto)
        {
            _methodName = "AddTerritory";
            var resultDto = new ResultDto();
            try
            {
                if (territoryDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (territoryDto.District.Count <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DistrictNameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.DistrictNameIsEmpty;
                    return resultDto;
                }

                var terrotoryModel = new Territory()
                {
                    Name = territoryDto.TerritoryName.Trim(),
                    StateId = territoryDto.StateId,
                    IsActive = territoryDto.IsActive,
                    CreatedBy = territoryDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.Territory.Add(terrotoryModel);

                _emamiContext.SaveChanges();

                //District territory mapping
                var districtIds = territoryDto.District.Where(w => w.Checked).Select(s => s.Id).ToList();
                var stateMappedDistricts = _emamiContext.District.Where(w => w.StateId == territoryDto.StateId && districtIds.Contains(w.Id)).ToList();
                if (stateMappedDistricts != null && stateMappedDistricts.Any())
                {
                    foreach (var district in stateMappedDistricts)
                    {
                        //district.TerritoryId = terrotoryModel.Id;
                        district.ModifiedBy = territoryDto.LoginUserId;
                        district.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    }
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// Update Territories
        /// </summary>
        /// <param name="territoryDto"></param>
        /// <returns></returns>
        public ResultDto UpdateTerritory(TerritoryDto territoryDto)
        {
            _methodName = "UpdateTerritory";
            var resultDto = new ResultDto();
            try
            {
                if (territoryDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (territoryDto.District.Count <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DistrictNameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.DistrictNameIsEmpty;
                    return resultDto;
                }
                //var isExistName = _emamiContext.Territory.AsNoTracking().Any(f => f.Id != territoryDto.Id && f.Name == territoryDto.TerritoryName);
                //if (isExistName)
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.Message = Constants.TerritoryNameExists;
                //    return resultDto;
                //}

                var territory = _emamiContext.Territory.FirstOrDefault(f => f.Id == territoryDto.Id);
                if (territory != null)
                {
                    territory.Name = territoryDto.TerritoryName;
                    territory.StateId = territoryDto.StateId;
                    territory.IsActive = territoryDto.IsActive;
                    territory.ModifiedBy = territoryDto.LoginUserId;
                    territory.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                    //District territory mapping
                    var districtIds = territoryDto.District.Where(w => w.Checked).Select(s => s.Id).ToList();


                    var stateMappedDistricts = _emamiContext.District.Where(w => w.StateId == territoryDto.StateId && districtIds.Contains(w.Id)/* && w.TerritoryId == 0*/).ToList();
                    var removedDistricts = _emamiContext.District.Where(w => w.StateId == territoryDto.StateId && !districtIds.Contains(w.Id) /*&& w.TerritoryId == territoryDto.Id*/).ToList();

                    if (stateMappedDistricts != null && stateMappedDistricts.Any())
                    {
                        foreach (var district in stateMappedDistricts)
                        {
                            // district.TerritoryId = territoryDto.Id;
                            district.ModifiedBy = territoryDto.LoginUserId;
                            district.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        }
                    }

                    if (removedDistricts != null && removedDistricts.Any())
                    {
                        foreach (var district in removedDistricts)
                        {
                            // district.TerritoryId = 0;
                            district.ModifiedBy = territoryDto.LoginUserId;
                            district.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        }
                    }

                    _emamiContext.SaveChanges();

                    resultDto.IsSuccess = true;
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
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        /// <summary>
        /// Get Territories based on id
        /// </summary>
        /// <param name="idInputDto"></param>
        /// <returns></returns>
        public ResultDto GerTerritoryById(int id)
        {
            _methodName = "GerTerritoryById";
            var resultDto = new ResultDto();
            try
            {
                var territory = _emamiContext.Territory.AsNoTracking().FirstOrDefault(f => f.Id == id);
                if (territory != null)
                {
                    var result = new TerritoryDto()
                    {
                        Id = territory.Id,
                        TerritoryName = territory.Name,
                        StateId = territory.StateId,
                        IsActive = territory.IsActive
                    };

                    var districtDetails = _emamiContext.District.AsNoTracking().Where(w => /*(w.TerritoryId == id || w.TerritoryId == 0) &&*/ w.StateId == territory.StateId).ToList();
                    if (districtDetails != null && districtDetails.Any())
                    {
                        districtDetails.ForEach(f =>
                        {
                            result.District.Add(new CheckBoxDto()
                            {
                                Id = f.Id,
                                Name = f.DistrictName,
                                //  Checked = f.TerritoryId == 0 ? false : true
                            });
                        });
                    }

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = result;
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
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        /// <summary>
        /// Get all Territories based on param
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GerTerritoryList(LoginUserIdDto inputDto)
        {
            _methodName = "GerTerritoryList";
            var resultDto = new ResultDto();
            IQueryable<Territory> territories;
            var result = new List<TerritoryDto>();
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    territories = _emamiContext.Territory.AsNoTracking();
                }
                else
                {
                    territories = _emamiContext.Territory.AsNoTracking().Where(w => w.IsActive);
                }

                if (territories != null && territories.Any())
                {
                    result = territories.Select(s => new TerritoryDto()
                    {
                        Id = s.Id,
                        TerritoryName = s.Name,
                        StateId = s.StateId,
                        StateName = s.State.StateName,
                        IsActive = s.IsActive
                    }).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = result != null ? result.OrderByDescending(_ => _.Id).ToList() : result;
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
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        /// <summary>
        /// Get all Territories based on param
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GerTerritoryMappedDistrict(TerritoryDistrictParam inputDto)
        {
            _methodName = "GerTerritoryList";
            var resultDto = new ResultDto();
            IQueryable<District> districts;
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    districts = _emamiContext.District.AsNoTracking()/*.Where(w => w.TerritoryId == inputDto.Id)*/;
                }
                else
                {
                    districts = _emamiContext.District.AsNoTracking().Where(w =>/* w.TerritoryId == inputDto.Id &&*/ w.IsActive);
                }

                if (districts != null && districts.Any())
                {
                    var result = districts.Select(s => new DistrictDto()
                    {
                        DistrictId = s.Id,
                        DistrictName = s.DistrictName
                    }).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = result;
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
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        /// <summary>
        /// Get Territories based on stateid
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GerTerritoryListByStateForDropdown(int stateId)
        {
            _methodName = "GerTerritoryStateBase";
            var resultDto = new ResultDto();
            try
            {
                var result = _emamiContext.Territory.AsNoTracking().Where(w => w.StateId == stateId && w.IsActive)
                .Select(s => new DropDownDto()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        /// <summary>
        /// Method to Get District List By TerritoryId
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public ResultDto GetDistrictListBaseTerritoryForDropdown(int territoryId)
        {
            _methodName = "GetDistrictListBaseTerritory";
            var resultDto = new ResultDto();
            var stateDto = new List<DistrictDto>();
            try
            {
                stateDto = _emamiContext.District.AsNoTracking().Where(_ => _.StateId == territoryId && _.IsActive)
                    .Select(_ => new DistrictDto { DistrictId = _.Id, DistrictName = _.DistrictName }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto ExportTerritory(LoginUserIdDto inputDto)
        {
            _methodName = "ExportTerritory";
            var resultDto = new ResultDto();
            var resultList = new List<TerritoryDto>();
            try
            {
                resultList = _emamiContext.Territory.AsNoTracking()
                    .Select(s => new TerritoryDto()
                    {
                        Id = s.Id,
                        TerritoryName = s.Name,
                        StateId = s.StateId,
                        StateName = s.State.StateName,
                        IsActive = s.IsActive
                    }).OrderByDescending(_ => _.Id).ToList();

                if (resultList == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                foreach (var item in resultList)
                {
                    var districts = _emamiContext.District.AsNoTracking()/*.Where(w => w.TerritoryId == item.Id)*/
                    .Select(s => new DistrictDto()
                    {
                        DistrictId = s.Id,
                        DistrictName = s.DistrictName
                    }).ToList();
                    item.DistrictList = districts;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = resultList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        #endregion

        #region IncoTerms & Plant

        public ResultDto GetIncoTermsList()
        {
            _methodName = "GetIncoTermsList";
            var resultDto = new ResultDto();
            try
            {
                var incotermList = _emamiContext.IncoTerms.AsNoTracking().Where(_ => _.IsActive)
                    .Select(s => new IncoTermsDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsActive = s.IsActive
                    }).ToList();

                return _resultService.SuccessObject(incotermList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPlantDepotList()
        {
            _methodName = "GetPlantDepotList";
            var resultDto = new ResultDto();
            try
            {
                var incotermList = _emamiContext.Depots.AsNoTracking().Where(_ => _.IsActive)
                    .Select(s => new DepotDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsPlant = s.IsPlant,
                        IsActive = s.IsActive
                    }).ToList();

                return _resultService.SuccessObject(incotermList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPlantDepotListBasedOnUser(LoginUserIdDto inputDto)
        {
            _methodName = "GetPlantDepotListBasedOnUser";

            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }

            var resultDto = new ResultDto();
            try
            {
                var depotList =
                               from depot in _emamiContext.Depots.AsNoTracking()
                               join depotMapping in _emamiContext.UserDepotMapping.AsNoTracking() on depot.Id equals depotMapping.DepotId
                               where depotMapping.UserId == inputDto.LoginUserId && depot.IsActive
                               select new DepotDto
                               {
                                   Id = depot.Id,
                                   Name = depot.Name,
                                   Code = depot.Code,
                                   IsPlant = depot.IsPlant,
                                   IsActive = depot.IsActive
                               };

                if (depotList == null || !depotList.Any())
                {
                    _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                return _resultService.SuccessObject(depotList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetIncotermListBasedOnUser(LoginUserIdDto inputDto)
        {
            _methodName = "GetIncotermListBasedOnUser";

            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }

            var resultDto = new ResultDto();
            try
            {
                var incoTermList =
                            (from incoterm in _emamiContext.IncoTerms.AsNoTracking()
                             join userIncoterm in _emamiContext.UserIncoTerms.AsNoTracking() on incoterm.Id equals userIncoterm.IncoTermsId
                             where userIncoterm.UserId == inputDto.LoginUserId && incoterm.IsActive
                             select new IncoTermsDto
                             {
                                 Id = incoterm.Id,
                                 Name = incoterm.Name,
                                 Code = incoterm.Code,
                                 Type = incoterm.Type,
                                 IsActive = incoterm.IsActive
                             }).ToList();

                if (incoTermList == null || !incoTermList.Any())
                {
                    _resultService.ErrorMessage(Constants.RecordNotFound);
                }



                return _resultService.SuccessObject(incoTermList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// Get Depts Based on Plant Ids
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetDepotsByPlantIds(DepotDropDownParam inputDto)
        {
            _methodName = "GetDepotsByPlantIds";
            var resultDto = new ResultDto();
            var resultList = new List<DropDownDto>();
            try
            {
                var depotIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(w => inputDto.PlantIds.Contains(w.PlantId))
                    .Select(s => s.DepotId).ToList();
                if (depotIds != null && depotIds.Any())
                {
                    resultList = _emamiContext.Depots.AsNoTracking().Where(w => depotIds.Any(a => a == w.Id) && !w.IsPlant)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Name = _.Name,
                        }).ToList();
                }

                resultDto.SuccessDto.Response = resultList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region State,Territory

        public ResultDto GetStatesBasedOnZone(List<int> zoneId)
        {
            _methodName = "GetZoneStateList";
            var resultDto = new ResultDto();
            try
            {

                var zoneMappedStates = _emamiContext.ZoneStateMappings.Where(s => zoneId.Any(a => a == s.ZoneId)).Select(s => s.StateId);
                var states = _emamiContext.State.Where(s => zoneMappedStates.Contains(s.Id)).Select(st => new StateDto()
                {
                    StateId = st.Id,
                    StateName = st.StateName
                });

                resultDto.SuccessDto.Response = states;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetTerritoryListByStateIdsForDropdown(List<int> stateIds)
        {
            _methodName = "GetTerritoryListByStateIdsForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var result = _emamiContext.Territory.AsNoTracking().Where(w => stateIds.Any(a => a == w.StateId) && w.IsActive)
                .Select(s => new DropDownDto()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        #endregion

        #region Notification

        public ResultDto GetCurrentFinancialYear()
        {
            _methodName = "GetCurrentFinancialYear";
            var resultDto = new ResultDto();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var financialyearList = _emamiContext.FinancialYears.AsNoTracking().FirstOrDefault(_ => _.IsActive && DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(currentDate) &&
                DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(currentDate));

                var financialyearDto = new FinancialYearDto()
                {
                    Id = financialyearList.Id,
                    Year = financialyearList.Year,
                    EffectiveFrom = financialyearList.EffectiveFrom,
                    EffectiveTo = financialyearList.EffectiveTo,
                    IsActive = financialyearList.IsActive
                };

                return _resultService.SuccessObject(financialyearList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetNotification(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetSalesNotification";
            var resultDto = new ResultDto();
            List<NotificationDto> notificationDtos = new List<NotificationDto>();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var StartDateForIndentNotification = currentDate.AddDays(Constants.NumberOfDaysTakenForNotification);
                //var SaudaNotificationlist = (from Notify in _emamiContext.Notifications.AsNoTracking()
                //                             join saudaorder in _emamiContext.SaudaOrders.AsNoTracking() on Notify.ReferenceId equals saudaorder.Id
                //                             join biddingwindow in _emamiContext.BiddingWindowTiming.AsNoTracking() on saudaorder.BiddingwindowId equals biddingwindow.Id into ps
                //                             from biddingwindow in ps.DefaultIfEmpty()
                //                             where Notify.Request == Constants.NotificationSauda && Notify.CreatedBy == loginUserIdDto.LoginUserId
                //                             && DbFunctions.TruncateTime(Notify.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                //                             select new NotificationDto
                //                             {
                //                                 Request = Notify.Request,
                //                                 Notification = Notify.Notification,
                //                                 BiddingDate = biddingwindow.BiddingDate,
                //                                 FromHour = biddingwindow.FromHours,
                //                                 ToHour = biddingwindow.ToHours,
                //                                 NotificationDateTime = Notify.CreatedDate,
                //                                 RequestId = Notify.RequestId,
                //                                 StatusId = Notify.StatusId,
                //                                 ReferenceId = Notify.ReferenceId,
                //                                 SaudaId = saudaorder.SaudaId
                //                             }).ToList();

                //var SaudaNotificationlist = _emamiContext.Notifications.AsNoTracking().Join(_emamiContext.SaudaOrders.AsNoTracking(), n => n.ReferenceId, s => s.Id, (n, s) => new { n, s })
                //                           .GroupJoin(_emamiContext.BiddingWindowTiming.AsNoTracking(), x => x.s.BiddingwindowId, b => b.Id, (x, b) => new { x.n, x.s, b })
                //                           .Where(_ => _.n.Request == Constants.NotificationSauda && _.n.CreatedBy == loginUserIdDto.LoginUserId
                //                            && DbFunctions.TruncateTime(_.n.CreatedDate) == DbFunctions.TruncateTime(currentDate)).SelectMany(_ => _.b.DefaultIfEmpty(), (x, b) => new { x.n, x.s, b }).ToList()
                //                            .Select(_ => new NotificationDto()
                //                            {
                //                                Request = _.n.Request,
                //                                Notification = _.n.StatusId == (int)DTO.Enums.Status.Pending ? "Accepted" : _.n.Notification,
                //                                BiddingDate = _.b?.BiddingDate,
                //                                FromHour = _.b?.FromHours,
                //                                ToHour = _.b?.ToHours,
                //                                NotificationDateTime = _.n.CreatedDate,
                //                                RequestId = _.n.RequestId,
                //                                StatusId = _.n.StatusId,
                //                                ReferenceId = _.n.ReferenceId,
                //                                SaudaId = _.s.SaudaId
                //                            }).ToList();


                //if (SaudaNotificationlist != null)
                //{
                //    notificationDtos.AddRange(SaudaNotificationlist);
                //}

                var IndentNotificationlist = (from Notify in _emamiContext.Notifications.AsNoTracking()
                                              join Indent in _emamiContext.LiftingRequest.AsNoTracking() on Notify.ReferenceId equals Indent.Id
                                              where Notify.Request == Constants.NotificationIndent && Notify.CreatedBy == loginUserIdDto.LoginUserId
                                              && DbFunctions.TruncateTime(Notify.CreatedDate) >= DbFunctions.TruncateTime(StartDateForIndentNotification)
                                              && DbFunctions.TruncateTime(Notify.CreatedDate) <= DbFunctions.TruncateTime(currentDate)
                                              select new NotificationDto
                                              {
                                                  Request = Notify.Request,
                                                  Notification = Notify.Notification,
                                                  NotificationDateTime = Notify.CreatedDate,
                                                  RequestId = Notify.RequestId,
                                                  StatusId = Notify.StatusId,
                                                  ReferenceId = Notify.ReferenceId
                                              }).OrderByDescending(_ => _.NotificationDateTime).ToList();
                foreach (var item in IndentNotificationlist)
                {
                    item.NotificationDateTime = DateHelper.UtcToIndia(item.NotificationDateTime.Value);
                }
                if (IndentNotificationlist != null)
                {
                    notificationDtos.AddRange(IndentNotificationlist);
                }

                //var CounterBidNotificationlist = (from Notify in _emamiContext.Notifications.AsNoTracking()
                //                                  where Notify.Request == Constants.NotificationCounterBid
                //                                    && DbFunctions.TruncateTime(Notify.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                //                                    && Notify.CreatedBy == loginUserIdDto.LoginUserId
                //                                  select new NotificationDto
                //                                  {
                //                                      Request = Notify.Request,
                //                                      Notification = Notify.Notification,
                //                                      NotificationDateTime = Notify.CreatedDate,
                //                                      RequestId = Notify.RequestId,
                //                                      StatusId = Notify.StatusId,
                //                                      ReferenceId = Notify.ReferenceId
                //                                  }).ToList();

                //if (CounterBidNotificationlist != null)
                //{
                //    notificationDtos.AddRange(CounterBidNotificationlist);
                //}

                var SaudaLimitNotificationlist = (from Notify in _emamiContext.Notifications.AsNoTracking()
                                                  join saudalimit in _emamiContext.SaudaLimit.AsNoTracking() on Notify.ReferenceId equals saudalimit.Id
                                                  where Notify.Request == Constants.NotificationSaudaLimit && Notify.CreatedBy == loginUserIdDto.LoginUserId
                                                  && DbFunctions.TruncateTime(Notify.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                                                  select new NotificationDto
                                                  {
                                                      Request = Notify.Request,
                                                      Notification = Notify.Notification,
                                                      NotificationDateTime = Notify.CreatedDate,
                                                      RequestId = Notify.RequestId,
                                                      StatusId = Notify.StatusId,
                                                      ReferenceId = Notify.ReferenceId
                                                  }).ToList();
                if (SaudaLimitNotificationlist != null)
                {
                    notificationDtos.AddRange(SaudaLimitNotificationlist);
                }

                //var SpecialRequestNotificationlist = (from Notify in _emamiContext.Notifications.AsNoTracking()
                //                                      join specialRequest in _emamiContext.SpecialRate.AsNoTracking() on Notify.ReferenceId equals specialRequest.Id
                //                                      where Notify.Request == Constants.NotificationSpecialRate && Notify.CreatedBy == loginUserIdDto.LoginUserId
                //                                      && DbFunctions.TruncateTime(Notify.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                //                                      select new NotificationDto
                //                                      {
                //                                          Request = Notify.Request,
                //                                          Notification = Notify.Notification,
                //                                          NotificationDateTime = Notify.CreatedDate,
                //                                          RequestId = Notify.RequestId,
                //                                          StatusId = Notify.StatusId,
                //                                          ReferenceId = Notify.ReferenceId
                //                                      }).ToList();
                //if (SpecialRequestNotificationlist != null)
                //{
                //    notificationDtos.AddRange(SpecialRequestNotificationlist);
                //}

                //var saudaConversionNotificationlist = (from Notify in _emamiContext.Notifications.AsNoTracking()
                //                                       join saudaConversion in _emamiContext.SaudaConversion.AsNoTracking() on Notify.ReferenceId equals saudaConversion.Id
                //                                       where Notify.RequestId == (int)DTO.Enums.NotificationRequest.SaudaConversion && Notify.CreatedBy == loginUserIdDto.LoginUserId
                //                                       && DbFunctions.TruncateTime(Notify.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                //                                       select new NotificationDto
                //                                       {
                //                                           Request = Notify.Request,
                //                                           Notification = Notify.Notification,
                //                                           NotificationDateTime = Notify.CreatedDate,
                //                                           RequestId = Notify.RequestId,
                //                                           StatusId = Notify.StatusId,
                //                                           ReferenceId = Notify.ReferenceId
                //                                       }).ToList();
                //if (saudaConversionNotificationlist != null)
                //{
                //    notificationDtos.AddRange(saudaConversionNotificationlist);
                //}

                //var saudaExtensionNotificationlist = (from Notify in _emamiContext.Notifications.AsNoTracking()
                //                                      join saudaExtension in _emamiContext.SaudaConversion.AsNoTracking() on Notify.ReferenceId equals saudaExtension.Id
                //                                      where Notify.RequestId == (int)DTO.Enums.NotificationRequest.SaudaExtension && Notify.CreatedBy == loginUserIdDto.LoginUserId
                //                                      && DbFunctions.TruncateTime(Notify.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                //                                      select new NotificationDto
                //                                      {
                //                                          Request = Notify.Request,
                //                                          Notification = Notify.Notification,
                //                                          NotificationDateTime = Notify.CreatedDate,
                //                                          RequestId = Notify.RequestId,
                //                                          StatusId = Notify.StatusId,
                //                                          ReferenceId = Notify.ReferenceId
                //                                      }).ToList();
                //if (saudaExtensionNotificationlist != null)
                //{
                //    notificationDtos.AddRange(saudaExtensionNotificationlist);
                //}

                #region CounterBid Notifications
                var dealerIds = new List<long>();
                var userData = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(f => f.UserId == loginUserIdDto.LoginUserId);

                if (userData.RoleId == (long)DTO.Enums.Role.StateTrader)
                {
                    var userMappedCustomer = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(w => w.UserId == loginUserIdDto.LoginUserId).Select(s => s.CustomerId).ToList();
                    dealerIds = _emamiContext.CustomerGroupDetails.AsNoTracking()
                        .Where(w => userMappedCustomer.Contains(w.CustomerId)).Select(s => s.CustomerId).ToList();
                }
                else if (userData.RoleId == (long)DTO.Enums.Role.Dealer)
                {
                    dealerIds.Add(loginUserIdDto.LoginUserId);
                }

                var CounterBidNotificationDatas = _emamiContext.CounterBidNotifications.AsNoTracking()
                    .OrderByDescending(o => o.Id)
                    .Where(_ => dealerIds.Contains(_.DealerId)
                    && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate))
                    .ToList();

                if (CounterBidNotificationDatas.IsAny())
                {
                    foreach (var notification in CounterBidNotificationDatas)
                    {
                        var biddingWindow = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(f => f.Id == notification.BiddingWindowId);
                        var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.CounterBidOfferNotificationSMS);

                        var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(f => f.Id == notification.SkuId).SkuName;
                        var smsPlainTemplate = smsTemplate.PlainTemplate
                            .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                            .Replace(Constants.SkuName, skuName)
                            .Replace(Constants.CounterBidOfferPrice, (Math.Round(notification.CounterBidOffer, 2)).ToString());
                        var smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);

                        notificationDtos.Add(new NotificationDto()
                        {
                            Request = DTO.Enums.NotificationRequest.CounterBid.ToString(),
                            RequestId = (int)DTO.Enums.NotificationRequest.CounterBid,
                            Notification = smsMessage,
                            BiddingDate = notification.CreatedDate,
                            FromHour = null,
                            ToHour = null,
                            NotificationDateTime = notification.CreatedDate,
                            StatusId = notification.StatusId,
                            ReferenceId = notification.SaudaBiddingCartId,
                            SaudaId = notification.SaudaBiddingCartId
                        });
                    }
                }
                #endregion

                resultDto.SuccessDto.Response = notificationDtos;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetRequestNotification(LoginUserIdDto inputDto)
        {
            _methodName = "GetRequestNotification";
            var resultDto = new ResultDto();
            List<NotificationDto> notificationDtos = new List<NotificationDto>();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var zhList = new List<long?>();
                var bdoList = new List<long>();
                var roleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId).RoleId;
                var userContext = _emamiContext.Users.AsNoTracking();
                var userReportingContext = _emamiContext.UserReportingToMappings.AsNoTracking();
                if (roleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    zhList = userReportingContext.Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(s => (long?)s.UserId).ToList();
                    //zhList = userContext.Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).Cast<long?>().ToList();
                    bdoList = userReportingContext.Where(_ => zhList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                }
                else if (roleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    bdoList = userReportingContext.Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                }
                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                if (bdoList != null && bdoList.Any())
                {
                    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    if (dealersList != null && dealersList.Any())
                    {
                        string status = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Pending);
                        string request = UtilityHelper.GetEnumDescription(DTO.Enums.NotificationRequest.Sauda);
                        //var SaudaNotificationlist = _emamiContext.SaudaOrders.AsNoTracking()
                        //                       .GroupJoin(_emamiContext.BiddingWindowTiming.AsNoTracking(), x => x.BiddingwindowId, b => b.Id, (x, b) => new
                        //                       {
                        //                           s = x,
                        //                           b,
                        //                           Status = status,
                        //                           RequestId = (int)DTO.Enums.NotificationRequest.Sauda,
                        //                           Request = request
                        //                       })
                        //                       .Where(_ => _.s.Sauda != null && dealersList.Contains(_.s.Sauda.UserId) && _.s.StatusId == (int)DTO.Enums.Status.Pending
                        //                        && DbFunctions.TruncateTime(_.s.CreatedDate) == DbFunctions.TruncateTime(currentDate)).SelectMany(_ => _.b.DefaultIfEmpty(), (x, b) => new { x.s, b, x.Status, x.RequestId, x.Request })
                        //                        .Select(_ => new NotificationDto()
                        //                        {
                        //                            Request = _.Request,
                        //                            Notification = _.Status,
                        //                            BiddingDate = _.b != null ? _.b.BiddingDate : (DateTime?)null,
                        //                            FromHour = _.b != null ? _.b.FromHours : (TimeSpan?)null,
                        //                            ToHour = _.b != null ? _.b.ToHours : (TimeSpan?)null,
                        //                            NotificationDateTime = _.s.CreatedDate,
                        //                            RequestId = _.RequestId,
                        //                            StatusId = _.s.StatusId,
                        //                            ReferenceId = _.s.Id,
                        //                            SaudaId = _.s.SaudaId
                        //                        }).ToList();

                        //if (SaudaNotificationlist != null && SaudaNotificationlist.Any())
                        //{
                        //    notificationDtos.AddRange(SaudaNotificationlist);
                        //}

                        request = UtilityHelper.GetEnumDescription(DTO.Enums.NotificationRequest.Indent);
                        var IndentStatus = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Approved);
                        var IndentNotificationlist = _emamiContext.LiftingRequest.AsNoTracking()
                            .Where(_ => dealersList.Contains(_.UserId)
                            && _.StatusId == (int)DTO.Enums.Status.Approved
                                                && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate))
                            .Select(s => new
                            {
                                lr = s,
                                Status = IndentStatus,
                                RequestId = (int)DTO.Enums.NotificationRequest.Indent,
                                Request = request
                            })
                                                .Select(_ => new NotificationDto()
                                                {
                                                    Request = _.Request,
                                                    Notification = _.Status,
                                                    NotificationDateTime = _.lr.CreatedDate,
                                                    RequestId = _.RequestId,
                                                    StatusId = _.lr.StatusId,
                                                    ReferenceId = _.lr.Id
                                                }).ToList();

                        if (IndentNotificationlist != null && IndentNotificationlist.Any())
                        {
                            notificationDtos.AddRange(IndentNotificationlist);
                        }

                        request = UtilityHelper.GetEnumDescription(DTO.Enums.NotificationRequest.SaudaLimit);
                        var SaudaLimitNotificationlist = _emamiContext.SaudaLimit.AsNoTracking()
                            .Where(_ => dealersList.Contains(_.UserId)
                            && _.StatusId == (int)DTO.Enums.Status.Pending
                                                && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate)).Select(s => new
                                                {
                                                    sl = s,
                                                    Status = status,
                                                    RequestId = (int)DTO.Enums.NotificationRequest.SaudaLimit,
                                                    Request = request
                                                })
                                                .Select(_ => new NotificationDto()
                                                {
                                                    Request = _.Request,
                                                    Notification = _.Status,
                                                    NotificationDateTime = _.sl.CreatedDate,
                                                    RequestId = _.RequestId,
                                                    StatusId = _.sl.StatusId,
                                                    ReferenceId = _.sl.Id
                                                }).ToList();
                        if (SaudaLimitNotificationlist != null)
                        {
                            notificationDtos.AddRange(SaudaLimitNotificationlist);
                        }

                        request = UtilityHelper.GetEnumDescription(DTO.Enums.NotificationRequest.SpecialRate);
                        var SpecialRequestNotificationlist = _emamiContext.SpecialRate.AsNoTracking()
                            .Where(_ => dealersList.Contains(_.UserId)
                                                && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate)).Select(s => new
                                                {
                                                    sr = s,
                                                    Status = status,
                                                    RequestId = (int)DTO.Enums.NotificationRequest.SpecialRate,
                                                    Request = request
                                                })
                                                .Select(_ => new NotificationDto()
                                                {
                                                    Request = _.Request,
                                                    Notification = _.Status,
                                                    NotificationDateTime = _.sr.CreatedDate,
                                                    RequestId = _.RequestId,
                                                    StatusId = _.sr.StatusId,
                                                    ReferenceId = _.sr.Id
                                                }).ToList();
                        if (SpecialRequestNotificationlist != null)
                        {
                            notificationDtos.AddRange(SpecialRequestNotificationlist);
                        }

                        //request = UtilityHelper.GetEnumDescription(DTO.Enums.NotificationRequest.SaudaConversion);
                        //var saudaConversionList = _emamiContext.SaudaConversion.AsNoTracking().Where(_ => dealersList.Contains(_.DealerId) && _.StatusId == (int)DTO.Enums.Status.Pending
                        //                        && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate) && _.IsConversion).Select(s => new
                        //                        {
                        //                            SaudaConversion = s,
                        //                            Status = status,
                        //                            RequestId = (int)DTO.Enums.NotificationRequest.SaudaConversion,
                        //                            Request = request
                        //                        })
                        //                        .Select(_ => new NotificationDto()
                        //                        {
                        //                            Request = _.Request,
                        //                            Notification = _.Status,
                        //                            NotificationDateTime = _.SaudaConversion.CreatedDate,
                        //                            RequestId = _.RequestId,
                        //                            StatusId = (long)_.SaudaConversion.StatusId,
                        //                            ReferenceId = _.SaudaConversion.Id
                        //                        }).ToList();
                        //if (saudaConversionList != null)
                        //{
                        //    notificationDtos.AddRange(saudaConversionList);
                        //}

                        //request = UtilityHelper.GetEnumDescription(DTO.Enums.NotificationRequest.SaudaExtension);
                        //var saudaExtensionList = _emamiContext.SaudaConversion.AsNoTracking().Where(_ => dealersList.Contains(_.DealerId) && _.ExtensionStatusId == (int)DTO.Enums.Status.Pending
                        //                        && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate) && _.IsExtension).Select(s => new
                        //                        {
                        //                            SaudaExtension = s,
                        //                            Status = status,
                        //                            RequestId = (int)DTO.Enums.NotificationRequest.SaudaExtension,
                        //                            Request = request
                        //                        })
                        //                        .Select(_ => new NotificationDto()
                        //                        {
                        //                            Request = _.Request,
                        //                            Notification = _.Status,
                        //                            NotificationDateTime = _.SaudaExtension.CreatedDate,
                        //                            RequestId = _.RequestId,
                        //                            StatusId = (long)_.SaudaExtension.StatusId,
                        //                            ReferenceId = _.SaudaExtension.Id
                        //                        }).ToList();
                        //if (saudaExtensionList != null)
                        //{
                        //    notificationDtos.AddRange(saudaExtensionList);
                        //}

                    }
                }

                resultDto.SuccessDto.Response = notificationDtos;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }


        #endregion

        #region District,Territory and City

        public ResultDto GetDistrictListByTerritoryIdsForDropdown(List<int> territoryIds)
        {
            _methodName = "GetDistrictListByTerritoryIdsForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var result = _emamiContext.District.AsNoTracking().Where(w => territoryIds.Any(a => a == w.StateId) && w.IsActive)
                .Select(s => new DropDownDto()
                {
                    Id = s.Id,
                    Name = s.DistrictName
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetCityListByDistrictIdsForDropdown(List<int> districtIds)
        {
            _methodName = "GetCityListByDistrictIdsForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var cities = _emamiContext.City.AsNoTracking().ToList();


                var result = _emamiContext.City.AsNoTracking().Where(w => districtIds.Contains(w.DistrictId) && w.IsActive)
                .Select(s => new DropDownDto()
                {
                    Id = s.Id,
                    Name = s.CityName
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        ///// <summary>
        ///// Method to Get FreightRoute List
        ///// </summary>
        ///// <param name="inputDto"></param>
        ///// <returns></returns>
        //public ResultDto GetFreightRouteByZone(List<long> districtIds)
        //{
        //    _methodName = "GetFreightRouteByZone";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        var resultList = _emamiContext.FreightRoutes.AsNoTracking().Where(w => districtIds.Contains(w.FreightZoneId))
        //            .Select(s => new DropDownDto()
        //            {
        //                Id = s.Id,
        //                Name = s.Name
        //            }).ToList();

        //        resultDto.SuccessDto.Response = resultList;
        //        resultDto.IsSuccess = true;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.Message = Constants.Exception;
        //        _logger.Error(message);
        //    }
        //    return resultDto;
        //}

        #endregion

        #region SubCategory

        /// <summary>
        /// Method to Save SubCategory
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto SaveSubCategory(SubCategoryDto inputDto)
        {
            _methodName = "SaveSubCategory";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }

                var nameExist = _emamiContext.SubCategory.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name && _.IsActive);
                if (nameExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }

                var SubCategory = new SubCategory
                {
                    Name = inputDto.Name,
                    IsActive = inputDto.IsActive,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };
                _emamiContext.SubCategory.Add(SubCategory);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Update SubCategory
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateSubCategory(SubCategoryDto inputDto)
        {
            _methodName = "UpdateSubCategory";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.IsActive)
                {
                    var nameExist = _emamiContext.SubCategory.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name && _.IsActive && _.Id != inputDto.Id);
                    if (nameExist != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.NameExist;
                        return resultDto;
                    }
                }

                var result = _emamiContext.SubCategory.FirstOrDefault(_ => _.Id == inputDto.Id);
                result.Name = inputDto.Name;
                result.IsActive = inputDto.IsActive;
                result.ModifiedBy = inputDto.LoginUserId;
                result.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get SubCategory List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSubCategoryList(KendoGridResult inputDto)
        {
            _methodName = "GetSubCategoryList";
            var resultDto = new ResultDto();
            var result = new DataSourceResult();
            try
            {
                IQueryable<SubCategory> resultContext;
                if (inputDto.IsToReturnInactiveData)
                {
                    resultContext = _emamiContext.SubCategory.AsNoTracking();
                }
                else
                {
                    resultContext = _emamiContext.SubCategory.AsNoTracking().Where(w => w.IsActive);
                }

                if (resultContext != null && resultContext.Any())
                {
                    result = resultContext.Select(c => new SubCategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        IsActive = c.IsActive,
                    }).ToDataSourceResult(inputDto.DataSourceRequest);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result; //outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to get Get SubCategory Details By Id
        /// </summary>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        public ResultDto GetSubCategoryDetailsById(long subCategoryId)
        {
            _methodName = "GetSubCategoryDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new SubCategoryDto();
            try
            {
                var resultContext = _emamiContext.SubCategory.AsNoTracking().FirstOrDefault(_ => _.Id == subCategoryId);
                if (resultContext != null)
                {
                    outputDto.Id = resultContext.Id;
                    outputDto.Name = resultContext.Name;
                    outputDto.IsActive = resultContext.IsActive;
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
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ExportSubCategory(LoginUserIdDto inputDto)
        {
            _methodName = "ExportSubCategory";
            var resultDto = new ResultDto();
            var result = new List<SubCategoryDto>();
            try
            {
                result = _emamiContext.SubCategory.AsNoTracking()
                    .Select(c => new SubCategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        IsActive = c.IsActive,
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result; //outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region DistributionChannel

        public ResultDto AddorUpdateDistributionChannel(DistributionChannelDto inputDto)
        {
            _methodName = "AddorUpdateDistributionChannel";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {

                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                    inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }

                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.Id > 0)
                {
                    var DistributionChannelExist = _emamiContext.DistributionChannel.FirstOrDefault(_ => _.Id == inputDto.Id);
                    if (DistributionChannelExist != null)
                    {
                        var DistributionChannelAlreadyExist = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.Id != inputDto.Id);
                        if (DistributionChannelAlreadyExist != null)
                        {
                            return _resultService.ErrorMessage(Constants.NameExist);
                        }
                        var SapCodeExists = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Code == inputDto.SAPCode && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.Id != inputDto.Id);
                        if (SapCodeExists != null)
                        {
                            return _resultService.ErrorMessage(Constants.CodeExist);
                        }
                        DistributionChannelExist.Code = inputDto.SAPCode;
                        DistributionChannelExist.Name = inputDto.Name;
                        DistributionChannelExist.SalesOrganizationId = inputDto.SalesOrganizationId;
                        DistributionChannelExist.IsActive = inputDto.IsActive;
                        DistributionChannelExist.ModifiedBy = inputDto.LoginUserId;
                        DistributionChannelExist.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();

                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.InvalidRequest);
                    }
                }
                else
                {
                    var DistributionExistAlready = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name && _.SalesOrganizationId == inputDto.SalesOrganizationId);
                    if (DistributionExistAlready != null)
                    {
                        return _resultService.ErrorMessage(Constants.DistributionChannelAlreadyExists);
                    }
                    var SapCodeExists = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Code == inputDto.SAPCode && _.SalesOrganizationId == inputDto.SalesOrganizationId);
                    if (SapCodeExists != null)
                    {
                        return _resultService.ErrorMessage(Constants.CodeExist);
                    }

                    var distributionChannel = new DistributionChannel
                    {
                        Code = inputDto.SAPCode,
                        Name = inputDto.Name,
                        IsActive = inputDto.IsActive,
                        CreatedBy = inputDto.LoginUserId,
                        SalesOrganizationId = inputDto.SalesOrganizationId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.DistributionChannel.Add(distributionChannel);
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDistributionChannelList()
        {
            _methodName = "GetDistributionChannelList";
            var resultDto = new ResultDto();
            var distributionChannelDto = new List<DistributionChannelDto>();
            try
            {
                distributionChannelDto = _emamiContext.DistributionChannel.AsEnumerable().OrderByDescending(_ => _.Name).Select(c => new DistributionChannelDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    Name = c.Name,
                    SAPCode = c.Code,
                    Id = c.Id,
                    SalesOrganizationId = c.SalesOrganizationId,
                    IsActive = c.IsActive,
                    SalesOrganization = c.SalesOrganization.Name,
                }).ToList();
                return _resultService.SuccessObject(distributionChannelDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDistributionChannelDetailsById(string distributionChannelId)
        {
            _methodName = "GetDistributionChannelDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new DistributionChannelDto();
            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(distributionChannelId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    outputDto.EncryptedId = distributionChannelId;
                    outputDto.Id = resultContext.Id;
                    outputDto.SAPCode = resultContext.Code;
                    outputDto.Name = resultContext.Name;
                    outputDto.SalesOrganizationId = resultContext.SalesOrganizationId;
                    outputDto.IsActive = resultContext.IsActive;
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

        #region SalesOrganization

        public ResultDto AddorUpdateSalesOrganization(SalesOrganizationDto inputDto)
        {
            _methodName = "AddorUpdateSalesOrganization";
            var resultDto = new ResultDto();
            try
            {
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                    inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }

                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.Id > 0)
                {
                    var SalesOrganizationExist = _emamiContext.SalesOrganization.FirstOrDefault(_ => _.Id == inputDto.Id);
                    if (SalesOrganizationExist != null)
                    {
                        var SalesOrgExistAlready = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name && _.Id != inputDto.Id);
                        if (SalesOrgExistAlready != null)
                        {
                            return _resultService.ErrorMessage(Constants.SalesOrgAlreadyExists);
                        }
                        var SapCodeExists = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Code == inputDto.SAPCode && _.Id != inputDto.Id);
                        if (SapCodeExists != null)
                        {
                            return _resultService.ErrorMessage(Constants.CodeExist);
                        }

                        SalesOrganizationExist.Code = inputDto.SAPCode;
                        SalesOrganizationExist.Name = inputDto.Name;
                        SalesOrganizationExist.IsActive = inputDto.IsActive;
                        SalesOrganizationExist.ModifiedBy = inputDto.LoginUserId;
                        SalesOrganizationExist.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }
                else
                {
                    var SalesOrgExistAlready = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name);
                    if (SalesOrgExistAlready != null)
                    {
                        return _resultService.ErrorMessage(Constants.SalesOrgAlreadyExists);
                    }
                    var SapCodeExists = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Code == inputDto.SAPCode);
                    if (SapCodeExists != null)
                    {
                        return _resultService.ErrorMessage(Constants.CodeExist);
                    }

                    var salesOrganization = new SalesOrganization
                    {
                        Code = inputDto.SAPCode,
                        Name = inputDto.Name,
                        IsActive = inputDto.IsActive,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.SalesOrganization.Add(salesOrganization);
                    _emamiContext.SaveChanges();
                }



                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSalesOrganizationList()
        {
            _methodName = "GetSalesOrganizationList";
            var resultDto = new ResultDto();
            var salesOrganizationDto = new List<SalesOrganizationDto>();
            try
            {
                //var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                salesOrganizationDto = _emamiContext.SalesOrganization.AsEnumerable().OrderByDescending(_ => _.Name).Select(c => new SalesOrganizationDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    Name = c.Name,
                    SAPCode = c.Code,
                    Id = c.Id,
                    IsActive = c.IsActive

                }).ToList();
                return _resultService.SuccessObject(salesOrganizationDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSalesOrganizationDetailsById(string EncryptedId)
        {
            _methodName = "GetSalesOrganizationDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new SalesOrganizationDto();
            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

                var salesId = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Id == salesId);
                if (resultContext != null)
                {
                    outputDto.Id = resultContext.Id;
                    outputDto.EncryptedId = EncryptedId;
                    outputDto.SAPCode = resultContext.Code;
                    outputDto.Name = resultContext.Name;
                    outputDto.IsActive = resultContext.IsActive;
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

        #region CustomerGroupFive

        public ResultDto AddorUpdateCustomerGroupFive(CustomerGroupFiveDto inputDto)
        {
            _methodName = "AddorUpdateCustomerGroupFive";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {

                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                    inputDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }
                if (inputDto.Id > 0)
                {
                    var GroupExist = _emamiContext.CustomerGroupFive.FirstOrDefault(_ => _.Id == inputDto.Id);
                    if (GroupExist != null)
                    {
                        var GroupNameAlreadyExist = _emamiContext.CustomerGroupFive.AsNoTracking().FirstOrDefault(_ => _.GroupName == inputDto.GroupName && _.GroupCode == inputDto.GroupCode && _.Id != inputDto.Id);
                        if (GroupNameAlreadyExist != null)
                        {
                            return _resultService.ErrorMessage(Constants.GroupAlreadyExists);
                        }
                        GroupExist.GroupCode = inputDto.GroupCode;
                        GroupExist.GroupName = inputDto.GroupName;
                        GroupExist.IsActive = inputDto.IsActive;
                        GroupExist.ModifiedBy = inputDto.LoginUserId;
                        GroupExist.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    }
                    _emamiContext.SaveChanges();
                }
                else
                {
                    var GroupExistAlready = _emamiContext.CustomerGroupFive.AsNoTracking().FirstOrDefault(_ => _.GroupName == inputDto.GroupName && _.GroupCode == inputDto.GroupCode);
                    if (GroupExistAlready != null)
                    {
                        return _resultService.ErrorMessage(Constants.GroupAlreadyExists);
                    }
                    var customerGroup5 = new CustomerGroupFive
                    {
                        GroupCode = inputDto.GroupCode,
                        GroupName = inputDto.GroupName,
                        IsActive = inputDto.IsActive,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.CustomerGroupFive.Add(customerGroup5);
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetCustomerGroupFiveList()
        {
            _methodName = "GetCustomerGroupFiveList";
            var resultDto = new ResultDto();
            var customerGroupFiveDto = new List<CustomerGroupFiveDto>();
            try
            {
                customerGroupFiveDto = _emamiContext.CustomerGroupFive.AsEnumerable().OrderByDescending(_ => _.GroupName).Select(c => new CustomerGroupFiveDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    GroupName = c.GroupName,
                    GroupCode = c.GroupCode,
                    Id = c.Id,
                    IsActive = c.IsActive

                }).ToList();
                return _resultService.SuccessObject(customerGroupFiveDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetCustomerGroupFiveDetailsById(string customerGroupId)
        {
            _methodName = "GetcustomergroupFiveDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new CustomerGroupFiveDto();
            try
            {
                customerGroupId = customerGroupId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(customerGroupId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.CustomerGroupFive.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    outputDto.EncryptedId = customerGroupId;
                    outputDto.Id = resultContext.Id;
                    outputDto.GroupCode = resultContext.GroupCode;
                    outputDto.GroupName = resultContext.GroupName;
                    outputDto.IsActive = resultContext.IsActive;
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

        #region VehicleLoadabilities

        public ResultDto AddOrUpdateVehicleLoadabilities(VehicleLoadabilitiesDto inputDto)
        {
            _methodName = "AddOrUpdateVehicleLoadabilities";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {

                    resultDto = _resultService.ErrorMessage(Constants.InvalidRequest);

                    return resultDto;
                }
                if (inputDto.VehicleSize <= 0)
                {
                    resultDto = _resultService.ErrorMessage(Constants.VehicleSize);
                    return resultDto;
                }


                if (inputDto.Id > 0)
                {

                    var VehicleLoadabilityExists = _emamiContext.VehicleLodability.FirstOrDefault(_ => _.Id == inputDto.Id);

                    if (VehicleLoadabilityExists != null)
                    {
                        _emamiContext.VehicleLodability.Remove(VehicleLoadabilityExists);
                        _emamiContext.SaveChanges();

                        var stateExistsInZone = _emamiContext.ZoneStateMappings.AsNoTracking().ToList();
                        //var freightZoneExistsInStateZone = _emamiContext.FreightZones.AsNoTracking().ToList();

                        foreach (var zone in inputDto.ZoneIds)
                        {
                            var StateIds = stateExistsInZone.Where(_ => _.ZoneId == zone && inputDto.StateIds.Contains(_.StateId)).Select(_ => _.StateId).ToList();

                            if (StateIds != null && StateIds.Any())
                            {
                                foreach (var state in StateIds)
                                {
                                    //var freightzoneids = freightZoneExistsInStateZone.Where(_ => _.ZoneId == zone && _.StateId == state && _.IsActive && inputDto.FreightZoneIds.Contains(_.Id)).Select(_ => _.Id).ToList();
                                    //if (freightzoneids != null && freightzoneids.Any())
                                    //{
                                    //    foreach (var freightzone in freightzoneids)
                                    //    {
                                    var vehicleAlreadyExistsSameValues = _emamiContext.VehicleLodability.FirstOrDefault(_ => _.StateId == state && _.ZoneId == zone
                                    //&& _.FreightZoneId == freightzone
                                    && _.VehicleSize == inputDto.VehicleSize && _.IsActive == inputDto.IsActiveBool);
                                    var vehicleAlreadyExistsWithDifferentStatus = _emamiContext.VehicleLodability.FirstOrDefault(_ => _.StateId == state && _.ZoneId == zone
                                    //&& _.FreightZoneId == freightzone
                                    && _.VehicleSize == inputDto.VehicleSize && _.IsActive != inputDto.IsActiveBool);
                                    if (vehicleAlreadyExistsSameValues != null)
                                    {
                                        vehicleAlreadyExistsSameValues.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        vehicleAlreadyExistsSameValues.ModifiedBy = inputDto.LoginUserId;
                                        _emamiContext.SaveChanges();

                                    }
                                    else if (vehicleAlreadyExistsWithDifferentStatus != null)
                                    {
                                        vehicleAlreadyExistsWithDifferentStatus.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        vehicleAlreadyExistsWithDifferentStatus.ModifiedBy = inputDto.LoginUserId;
                                        vehicleAlreadyExistsWithDifferentStatus.IsActive = inputDto.IsActiveBool;
                                        _emamiContext.SaveChanges();

                                    }
                                    else
                                    {
                                        var input = new VehicleLodability
                                        {
                                            StateId = state,
                                            ZoneId = zone,
                                            //FreightZoneId = freightzone,
                                            VehicleSize = inputDto.VehicleSize,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            CreatedBy = inputDto.LoginUserId,
                                            IsActive = inputDto.IsActiveBool
                                        };
                                        _emamiContext.VehicleLodability.Add(input);
                                        _emamiContext.SaveChanges();
                                    }
                                }
                                //    }

                                //}
                            }
                        }
                    }
                }
                else
                {
                    var VehicleLoadabilityIsExists = _emamiContext.VehicleLodability.FirstOrDefault(_ => inputDto.StateIds.Contains(_.StateId) && inputDto.ZoneIds.Contains(_.ZoneId)
                    //&& inputDto.FreightZoneIds.Contains(_.FreightZoneId) 
                    && _.VehicleSize == inputDto.VehicleSize && _.IsActive == inputDto.IsActiveBool);

                    if (VehicleLoadabilityIsExists != null)
                    {

                        return resultDto = _resultService.ErrorMessage(Constants.VehicleAlreadyExists + " " + VehicleLoadabilityIsExists.State.StateName + "  , " + VehicleLoadabilityIsExists.Zone.Name
                            //+ " , " 
                            //+ VehicleLoadabilityIsExists.FreightZone.Name 
                            + " ");

                    }
                    else
                    {
                        var stateExistsInZone = _emamiContext.ZoneStateMappings.AsNoTracking().ToList();
                        //var freightZoneExistsInStateZone = _emamiContext.FreightZones.AsNoTracking().ToList();
                        foreach (var zone in inputDto.ZoneIds)
                        {
                            var StateIds = stateExistsInZone.Where(_ => _.ZoneId == zone && inputDto.StateIds.Contains(_.StateId)).Select(_ => _.StateId).ToList();
                            if (StateIds != null && StateIds.Any())
                            {
                                foreach (var state in StateIds)
                                {
                                    //var freightzoneids = freightZoneExistsInStateZone.Where(_ => _.ZoneId == zone && _.StateId == state && _.IsActive && inputDto.FreightZoneIds.Contains(_.Id)).Select(_ => _.Id).ToList();
                                    //if (freightzoneids != null && freightzoneids.Any())
                                    //{

                                    //    foreach (var freightzone in freightzoneids)
                                    //    {

                                    var input = new VehicleLodability
                                    {
                                        StateId = state,
                                        ZoneId = zone,
                                        //FreightZoneId = freightzone,
                                        VehicleSize = inputDto.VehicleSize,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        CreatedBy = inputDto.LoginUserId,
                                        IsActive = inputDto.IsActiveBool
                                    };
                                    _emamiContext.VehicleLodability.Add(input);
                                    _emamiContext.SaveChanges();
                                    //    }
                                    //}
                                }
                            }


                        }
                    }
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetVehicleLoadabilitiesList()
        {
            _methodName = "GetVehicleLoadabilitiesList";
            var resultDto = new ResultDto();
            var vehicleloadabilitiesDto = new List<VehicleLoadabilitiesGridDataDto>();
            try
            {

                vehicleloadabilitiesDto = _emamiContext.VehicleLodability.AsNoTracking().Select(_ => new VehicleLoadabilitiesGridDataDto
                {
                    StateId = _.StateId,
                    StateName = _.State.StateName,
                    ZoneId = _.ZoneId,
                    ZoneName = _.Zone.Name,
                    //FreightZoneId = _.FreightZoneId,
                    //FreightZoneName = _.FreightZone.Name,
                    VehicleSize = _.VehicleSize,
                    Id = _.Id,
                    IsActive = _.IsActive

                }).OrderByDescending(_ => _.Id).ToList();

                return resultDto = _resultService.SuccessMessageWitObject(vehicleloadabilitiesDto, "");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";

                _logger.Error(message);
                return resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetVehicleLoadabilitiesById(VehicleLoadabilitiesDto inputDto)
        {
            _methodName = "GetVehicleLoadabilitiesById";
            var resultDto = new ResultDto();
            var input = new VehicleLoadabilitiesDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var vehicleloadabilitiesData = _emamiContext.VehicleLodability.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);
                if (vehicleloadabilitiesData != null)
                {
                    input = new VehicleLoadabilitiesDto()
                    {
                        StateIds = new List<int> { vehicleloadabilitiesData.StateId },
                        StateName = vehicleloadabilitiesData.State.StateName,
                        ZoneIds = new List<long> { vehicleloadabilitiesData.ZoneId },
                        ZoneName = vehicleloadabilitiesData.Zone.Name,
                        //FreightZoneIds = new List<long> { vehicleloadabilitiesData.FreightZoneId },
                        //FreightZoneName = vehicleloadabilitiesData.FreightZone.Name,
                        VehicleSize = vehicleloadabilitiesData.VehicleSize,
                        Id = vehicleloadabilitiesData.Id,
                        IsActiveBool = vehicleloadabilitiesData.IsActive
                    };
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                resultDto.SuccessDto.Response = input;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto ExportVehicleLoadabiliities(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportVehicleLoadabiliities";
            var resultDto = new ResultDto();
            var vehicleLoadabilitiesDto = new List<VehicleLoadabilitiesDto>();
            try
            {
                vehicleLoadabilitiesDto = _emamiContext.VehicleLodability.AsNoTracking().OrderByDescending(_ => _.Zone.Name).Select(c => new VehicleLoadabilitiesDto
                {
                    ZoneId = c.ZoneId,
                    ZoneName = c.Zone.Name,
                    StateId = c.StateId,
                    StateName = c.State.StateName,
                    //FreightZoneId = c.FreightZoneId,
                    //FreightZoneName = c.FreightZone.Name,
                    VehicleSize = c.VehicleSize,
                    IsActiveBool = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = vehicleLoadabilitiesDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetZonalHeadBasedonZoneState(ZonalHeadMappingDto inputDto)
        {
            _methodName = "GetZonalHeadBasedonZoneState";
            var resultDto = new ResultDto();
            try
            {
                var list = inputDto.StateIds;
                var zoneMappedStates = _emamiContext.ZoneStateMappings.Where(s => inputDto.ZoneIds.Any(a => a == s.ZoneId)).Select(s => s.StateId);
                var States = _emamiContext.State.Where(s => zoneMappedStates.Contains(s.Id)).Select(s => (long)s.Id);

                var zonalHeadList = _emamiContext.Users.AsNoTracking()
                                    .Join(_emamiContext.UserRoles, usr => usr.Id, ur => ur.UserId, (usr, ur) => new { usr, ur })
                                    .Join(_emamiContext.Roles, r => r.ur.RoleId, ro => ro.Id, (r, ro) => new { r, ro })
                                    .Where(s => s.r.ur.RoleId == (int)DTO.Enums.Role.ZonalTrader && States.Contains(s.r.usr.StateId))
                                    .Select(_ => new ZonalHeadMappingDto
                                    {
                                        ZonalHeadId = (int)_.r.usr.Id,
                                        ZonalHeadName = _.r.usr.Name
                                    }
                                    ).ToList();

                resultDto.SuccessDto.Response = zonalHeadList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetOilTypeBasedonVerticals(OilTypeMappingDto inputDto)
        {
            _methodName = "GetOilTypeBasedonVerticals";
            var resultDto = new ResultDto();
            var oilTypeList = new List<OilTypeMappingDto>(); ;
            try
            {

                if (inputDto.ZoneIds != null && inputDto.StateIds != null && inputDto.ZHIds != null && inputDto.BDOIds != null && inputDto.VerticalId > 0)
                {
                    var list = inputDto.StateIds;
                    var zoneMappedStates = _emamiContext.ZoneStateMappings.Where(s => inputDto.ZoneIds.Any(a => a == s.ZoneId)).Select(s => s.StateId);
                    var States = _emamiContext.State.Where(s => zoneMappedStates.Contains(s.Id)).Select(s => (long)s.Id);
                    var ZonalTrader = _emamiContext.Users.Where(a => States.Contains(a.StateId)).Select(a => (long)a.Id);

                    var verticalIdList = _emamiContext.UserDivisionMappings.Where(b => ZonalTrader.Contains(b.UserId)
                    //&& b.DivisionId == inputDto.VerticalId
                    ).Select(b => (long)b.DivisionId);
                    var oilTypes = _emamiContext.OilTypes.Where(o => verticalIdList.Contains(o.DivisionId) && verticalIdList.Contains(inputDto.VerticalId) && o.IsActive);

                    oilTypeList = oilTypes
                        .Select(s => new OilTypeMappingDto()
                        {
                            OilTypeId = (int)s.Id,
                            OilTypeName = s.Name,

                        }).ToList();

                }
                else if (inputDto.VerticalId > 0)
                {
                    var oilTypes = _emamiContext.OilTypes.Where(o => inputDto.VerticalId == o.DivisionId && o.IsActive);

                    oilTypeList = oilTypes
                        .Select(s => new OilTypeMappingDto()
                        {
                            OilTypeId = (int)s.Id,
                            OilTypeName = s.Name,

                        }).ToList();
                }
                else
                {
                    var oilTypes = _emamiContext.OilTypes.Where(o => o.IsActive);

                    oilTypeList = oilTypes
                        .Select(s => new OilTypeMappingDto()
                        {
                            OilTypeId = (int)s.Id,
                            OilTypeName = s.Name,

                        }).ToList();
                }

                resultDto.SuccessDto.Response = oilTypeList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region VolumeLoadability

        public ResultDto AddOrUpdateVolumeLoadability(DTO.VolumeLoadability inputDto)
        {
            _methodName = "AddOrUpdateVolumeLoadability";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto = _resultService.ErrorMessage(Constants.InvalidRequest);
                    return resultDto;
                }

                if (inputDto.Id > 0)
                {
                    var volumeloadabilityExists = _emamiContext.VolumeLoadability.FirstOrDefault(_ => _.Id == inputDto.Id);
                    if (volumeloadabilityExists != null)
                    {
                        volumeloadabilityExists.MaxAllowableSinglesku = inputDto.MaxAllowableSingleSku;
                        volumeloadabilityExists.MaxAllowableMultiplesku = inputDto.MaxAllowableMultipleSku;
                        volumeloadabilityExists.VehicleSize = inputDto.VehicleSize;
                        volumeloadabilityExists.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        volumeloadabilityExists.ModifiedBy = inputDto.LoginUserId;
                        volumeloadabilityExists.IsActive = inputDto.IsActive;
                        _emamiContext.SaveChanges();
                    }
                }
                else
                {
                    var checkIsExists = _emamiContext.VolumeLoadability
                        .Where(w => w.VehicleSize == inputDto.VehicleSize
                        && inputDto.SkuIds.Contains(w.SkuId)
                        && w.PlantId == inputDto.PlantId
                        && w.IsActive
                        && DbFunctions.TruncateTime(inputDto.ValidFrom) <= DbFunctions.TruncateTime(w.ValidTo)
                        && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo)).ToList();

                    if (checkIsExists != null && checkIsExists.Any())
                    {
                        foreach (var item in checkIsExists)
                        {
                            item.IsActive = false;
                            _emamiContext.SaveChanges();
                        }
                    }

                    foreach (var sku in inputDto.SkuIds)
                    {
                        var input = new Data.Entities.VolumeLoadability
                        {
                            SkuId = sku,
                            PlantId = inputDto.PlantId,
                            MaxAllowableSinglesku = inputDto.MaxAllowableSingleSku,
                            MaxAllowableMultiplesku = inputDto.MaxAllowableMultipleSku,
                            ValidFrom = inputDto.ValidFrom,
                            ValidTo = inputDto.ValidTo,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            CreatedBy = inputDto.LoginUserId,
                            IsActive = inputDto.IsActive,
                            VehicleSize = inputDto.VehicleSize,
                        };
                        _emamiContext.VolumeLoadability.Add(input);
                    }
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetVolumeLoadabilityList()
        {
            _methodName = "GetVolumeLoadabilityList";
            var resultDto = new ResultDto();
            var volumeLoadabilityDto = new List<VolumeLoadabilityGridDataDto>();
            try
            {
                volumeLoadabilityDto = _emamiContext.VolumeLoadability.AsNoTracking().Select(_ => new VolumeLoadabilityGridDataDto
                {
                    Id = _.Id,
                    Sku = _.Sku.SkuName,
                    Plant = _.Plant.Name,
                    ValidFrom = _.ValidFrom,
                    ValidTo = _.ValidTo,
                    MaxAllowableSingleSku = _.MaxAllowableSinglesku,
                    MaxAllowableMultipleSku = _.MaxAllowableMultiplesku,
                    IsActive = _.IsActive,
                    VehicleSize = _.VehicleSize,
                    SkuCode = _.Sku.SkuCode
                }).OrderByDescending(_ => _.Id).ToList();

                return resultDto = _resultService.SuccessMessageWitObject(volumeLoadabilityDto, "");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";

                _logger.Error(message);
                return resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetVolumeLoadabilityById(DTO.VolumeLoadability inputDto)
        {
            _methodName = "GetVolumeLoadabilityById";
            var resultDto = new ResultDto();
            var input = new DTO.VolumeLoadability();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var volumeloadabilityData = _emamiContext.VolumeLoadability.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);
                if (volumeloadabilityData != null)
                {
                    input = new DTO.VolumeLoadability()
                    {
                        Id = volumeloadabilityData.Id,
                        IsActive = volumeloadabilityData.IsActive,
                        SkuIds = new List<long> { volumeloadabilityData.SkuId },
                        PlantId = volumeloadabilityData.PlantId,
                        ValidFrom = volumeloadabilityData.ValidFrom,
                        ValidTo = volumeloadabilityData.ValidTo,
                        MaxAllowableSingleSku = volumeloadabilityData.MaxAllowableSinglesku,
                        MaxAllowableMultipleSku = volumeloadabilityData.MaxAllowableMultiplesku,
                        OilTypeId = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == volumeloadabilityData.SkuId).OilTypeId ?? 0,
                        VerticalId = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == volumeloadabilityData.SkuId).DivisionId,
                        VehicleSize = volumeloadabilityData.VehicleSize
                    };
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                resultDto.SuccessDto.Response = input;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto ExportVolumeLoadability(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportVolumeLoadability";
            var resultDto = new ResultDto();
            var volumeLoadabilityDto = new List<VolumeLoadabilityGridDataDto>();
            try
            {
                volumeLoadabilityDto = _emamiContext.VolumeLoadability.AsNoTracking().Select(_ => new VolumeLoadabilityGridDataDto
                {
                    Id = _.Id,
                    Sku = _.Sku.SkuName,
                    Plant = _.Plant.Name,
                    ValidFrom = _.ValidFrom,
                    ValidTo = _.ValidTo,
                    MaxAllowableSingleSku = _.MaxAllowableSinglesku,
                    MaxAllowableMultipleSku = _.MaxAllowableMultiplesku,
                    IsActive = _.IsActive,
                    VehicleSize = _.VehicleSize,
                    SkuCode = _.Sku.SkuCode
                }).OrderByDescending(_ => _.Id).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = volumeLoadabilityDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region SchemeGeographyReport

        public ResultDto GetGeographySchemeBasedOnState(List<int> stateId)
        {
            _methodName = "GetGeographySchemeBasedOnStateList";
            var resultDto = new ResultDto();
            try
            {

                var stateMappedSchemes = (from sdg in _emamiContext.SchemeDiscountGeographyMappings
                                          join c in _emamiContext.City on sdg.CityId equals c.Id
                                          select sdg.SchemeDiscountGeographyId);
                var GeographySchemes = _emamiContext.SchemeDiscountGeography.Where(s => stateMappedSchemes.Contains(s.Id)).Select(st => new SchemeDiscountGeographyDto()
                {
                    Id = st.Id,
                    Name = st.Name
                });

                resultDto.SuccessDto.Response = GeographySchemes;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion


        public ResultDto GetProfileImageUrl(UserProfileDto inputDto)
        {
            _methodName = "GetProfileImageUrl";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                }
                else
                {
                    var result = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                    var userDto = new UserProfileDto()
                    {
                        ProfilePath = result.ProfilePath,
                        PostMessage = "Success",
                        PostStatus = true,

                    };
                    resultDto.SuccessDto.Response = userDto;
                    resultDto.IsSuccess = true;
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #region Line
        public ResultDto AddLineDetails(AddAndUpdateLineDto InputDto)
        {
            _methodName = "AddLine";
            var resultDto = new ResultDto();
            try
            {
                var isExists = _emamiContext.Line.Where(_ => _.Name == InputDto.LineName).FirstOrDefault();

                if (isExists == null)
                {
                    var Line = new Line()
                    {
                        IsActive = InputDto.IsActive,
                        Name = InputDto.LineName,
                        CreatedBy = InputDto.UserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        ModifiedBy = InputDto.UserId,
                        ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    };

                    _emamiContext.Line.Add(Line);
                    _emamiContext.SaveChanges();

                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = "The Line name is Already Exists";
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }

            return resultDto;
        }

        public ResultDto UpdateLineDetails(AddAndUpdateLineDto InputDto)
        {
            _methodName = "AddLine";
            var resultDto = new ResultDto();
            try
            {
                var lineDecryptValue = Convert.ToInt64(UtilityHelper.ConvertMd5ToString(InputDto.EncryptedId, SecurityConstants.EncryptionKey));

                if (lineDecryptValue != 0)
                {
                    var lineData = _emamiContext.Line.Where(_ => _.Id == lineDecryptValue).FirstOrDefault();

                    if (lineData != null)
                    {
                        lineData.IsActive = InputDto.IsActive;
                        lineData.Name = InputDto.LineName;
                        lineData.ModifiedBy = InputDto.UserId;
                        lineData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        _emamiContext.SaveChanges();

                        resultDto.IsSuccess = true;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "The Line name is Already Exists";
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }

            return resultDto;
        }

        public ResultDto GetLineListForddl()
        {
            _methodName = "GetLineListForddl";
            var resultDto = new ResultDto();
            List<LineddlDto> lineList = new List<LineddlDto>();

            try
            {
                lineList = (from line in _emamiContext.Line.AsNoTracking()
                            where line.IsActive
                            select new LineddlDto
                            {
                                LineId = line.Id,
                                LineName = line.Name
                            }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = lineList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }

            return resultDto;
        }

        public ResultDto GetLineListForGrid()
        {
            _methodName = "GetLineListForddl";
            var resultDto = new ResultDto();
            List<LineGridDto> lineList = new List<LineGridDto>();

            try
            {
                var data = _emamiContext.Line.AsNoTracking().ToList();

                if (data.Any())
                {
                    lineList = (from line in data.AsEnumerable()
                                    //where line.IsActive == true
                                select new LineGridDto
                                {
                                    EncryptedId = UtilityHelper.ConvertToMd5(line.Id.ToString(), SecurityConstants.EncryptionKey),
                                    LineId = line.Id,
                                    LineName = line.Name,
                                    IsActive = line.IsActive,
                                    CreatedBy = line.CreatedBy,
                                    CreatedDate = line.CreatedDate,
                                    ModifiedBy = line.ModifiedBy,
                                    ModifiedDate = line.ModifiedDate
                                }).ToList();

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = lineList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }

            return resultDto;
        }

        public ResultDto GetLineDetailsById(string LineId)
        {
            _methodName = "GetLineDetailsById";
            var resultDto = new ResultDto();

            AddAndUpdateLineDto lineDetails = new AddAndUpdateLineDto();

            try
            {
                var lineDecryptId = Convert.ToInt64(UtilityHelper.ConvertMd5ToString(LineId, SecurityConstants.EncryptionKey));
                if (lineDecryptId != 0)
                {
                    lineDetails = (from line in _emamiContext.Line.AsNoTracking()
                                   where line.Id == lineDecryptId
                                   select new AddAndUpdateLineDto
                                   {
                                       LineId = line.Id,
                                       LineName = line.Name,
                                       IsActive = line.IsActive,
                                       CreatedBy = line.CreatedBy,
                                       EncryptedId = LineId
                                   }).FirstOrDefault();
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = lineDetails;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }

            return resultDto;
        }

        public ResultDto ExportLine(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportZone";
            var resultDto = new ResultDto();
            try
            {
                var lineMapping = _emamiContext.Line.AsNoTracking()
                    .Select(s => new LineGridDto()
                    {
                        LineId = s.Id,
                        LineName = s.Name,
                        IsActive = s.IsActive
                    }).ToList();

                resultDto.SuccessDto.Response = lineMapping;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetDONumberListByDistributorId(List<string> selectedIdsList)
        {
            _methodName = "GetDONumberList";
            var resultDto = new ResultDto();
            List<DONumberddlDto> doNumbersList = new List<DONumberddlDto>();

            try
            {
                var donumbercontext = _emamiContext.CompletedDoNumbers.AsNoTracking().Select(s => s.DoNumber).ToList();
                var fromdate = DateTime.Now.AddDays(-7);
                var doNumberList = _emamiContext.SalesRegister.AsNoTracking()
                    .Where(_ => selectedIdsList.Contains(_.CustomerCode) && _.DeliveryNumber != null && DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(fromdate)).OrderByDescending(s => s.InvoiceDate).ToList();
                //.Select(_ => new { 
                //    Id = _.UserId,
                //    value = _.SAPDeliveryNo
                //}).ToList();

                foreach (var item in doNumberList)
                {
                    //var listValue = item.value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    //doNumbersList.AddRange(listValue);

                    if (!donumbercontext.Contains(item.DeliveryNumber))
                    {
                        doNumbersList.Add(new DONumberddlDto()
                        {
                            Id = 0,
                            Value = item.DeliveryNumber,
                            BillingNo = item.InvoiceNumber,
                            BillingDate = item.InvoiceDate
                        });
                    }

                }

                doNumbersList = doNumbersList.GroupBy(s => new { s.Value, s.BillingNo }).Select(s => new DONumberddlDto()
                {
                    BillingNo = s.Key.BillingNo,
                    Value = s.Key.Value,
                    BillingDate = s.FirstOrDefault() != null ? s.FirstOrDefault().BillingDate : DateTime.Now
                }).ToList();
                resultDto.SuccessDto.Response = doNumbersList.GroupBy(s => s.BillingNo).Select(s => new DONumberddlDto()
                {
                    Value = String.Join(",", s.Select(_ => _.Value).Distinct()),
                    BillingNo = s.Key + " / " + (s.FirstOrDefault() != null ? s.FirstOrDefault().BillingDate.ToString("dd-MM-yyyy") : DateTime.Now.ToString("dd-MM-yyyy")),
                    BillingDate = s.FirstOrDefault() != null ? s.FirstOrDefault().BillingDate : DateTime.Now
                }).Distinct().ToList();
                //    .GroupBy(s => s.BillingNo).Select(s => new DONumberddlDto() { 
                //    Value=s.FirstOrDefault().Value,
                //    BillingNo=s.Key,
                //}).Distinct().ToList();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region GamificationDashboard

        //public ResultDto GetGamificationDashboard()
        //{
        //    _methodName = "GetGamificationDashboard";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        var gamificationdashboard = _emamiContext.GamificationDashboards.Where(s => s.IsActive).Select(s => new GamificationDashboardDto()
        //        {
        //            Id = s.Id,
        //            DistributorId = s.DistributorId,
        //            DistributorCode = s.DistributorCode,
        //            DistributorAchievementTillN1MT = s.DistributorAchievementTillN1MT,
        //            RemainingTargetToAchieveMT = s.RemainingTargetToAchieveMT,
        //            EarnedPoints = s.EarnedPoints,
        //            CurrentSlab = s.CurrentSlab,
        //            NextHigherSlab = s.NextHigherSlab,
        //            PointsToBeEarnedToReachNextHigherSlab = s.PointsToBeEarnedToReachNextHigherSlab,
        //            TotalEarningsInRs = s.TotalEarningsInRs,
        //            SpecialBonusMessage = s.SpecialBonusMessage,
        //            WholePointsStructure = s.WholePointsStructure,
        //            IsDiamond = s.IsDiamond,
        //            IsActive = s.IsActive

        //        }).ToList();
        //        resultDto.SuccessDto.Response = gamificationdashboard;
        //        resultDto.IsSuccess = true;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.Message = Constants.Exception;
        //        _logger.Error(message);
        //    }
        //    return resultDto;
        //}

        public ResultDto GetGamificationDashboard(GamificationDashboardDto inputDto)
        {
            _methodName = "GetGamificationDashboard";
            var resultDto = new ResultDto();
            try
            {
                var result = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);


                //if (result != null) {
                var gamificationdashboard = _emamiContext.GamificationDashboards.Where(s => s.DistributorCode == inputDto.DistributorCode && s.IsActive).Select(s => new GamificationDashboardDto()
                {
                    Id = s.Id,
                    DistributorId = s.DistributorId,
                    DistributorCode = s.DistributorCode,
                    DistributorTargetMT = s.DistributorTargetMT,
                    DistributorAchievementTillN1MT = s.DistributorAchievementTillN1MT,
                    RemainingTargetToAchieveMT = s.RemainingTargetToAchieveMT,
                    EarnedPoints = s.EarnedPoints,
                    //CurrentSlab = s.CurrentSlab,
                    //NextHigherSlab = s.NextHigherSlab,
                    PointsToBeEarnedToReachNextHigherSlab = s.PointsToBeEarnedToReachNextHigherSlab,
                    TotalEarningsInRs = s.TotalEarningsInRs,
                    SpecialBonusMessage = s.SpecialBonusMessage,
                    WholePointsStructure = s.WholePointsStructure,
                    IsDiamond = s.IsDiamond,
                    IsActive = s.IsActive

                }).ToList();

                var label = new List<string>(ConfigurationManager.AppSettings["LabelList"].Split(new char[] { ',' }));
                Dictionary<string, string> labelDictionary = new Dictionary<string, string>();
                List<Dictionary<string, string>> ListOflabelDictionary = new List<Dictionary<string, string>>();
                for (int i = 0; i < gamificationdashboard.Count; i++)
                {
                    labelDictionary.Add(label[0], gamificationdashboard[i].DistributorTargetMT.ToString());
                    labelDictionary.Add(label[1], gamificationdashboard[i].DistributorAchievementTillN1MT.ToString());
                    labelDictionary.Add(label[2], gamificationdashboard[i].RemainingTargetToAchieveMT.ToString());
                    labelDictionary.Add(label[3], gamificationdashboard[i].EarnedPoints.ToString());
                    //labelDictionary.Add(label[4], gamificationdashboard[i].CurrentSlab.ToString());
                    //labelDictionary.Add(label[5], gamificationdashboard[i].NextHigherSlab.ToString());
                    labelDictionary.Add(label[6], gamificationdashboard[i].PointsToBeEarnedToReachNextHigherSlab.ToString());
                    labelDictionary.Add(label[7], gamificationdashboard[i].TotalEarningsInRs.ToString());
                    labelDictionary.Add(label[8], gamificationdashboard[i].SpecialBonusMessage?.ToString() ?? "-");
                }
                ListOflabelDictionary.Add(labelDictionary);

                ResponsesDto ResponseList = new ResponsesDto()
                {
                    LabelListDto = ListOflabelDictionary,
                    GamificationDashboardDto = gamificationdashboard
                };
                resultDto.SuccessDto.Response = ResponseList;

                //resultDto.SuccessDto.Response = gamificationdashboard;
                resultDto.IsSuccess = true;


                //resultDto.SuccessDto.Response = gamificationdashboard;
                //resultDto.IsSuccess = true;
                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GCPApidata()
        {
            _logger.Info("Google API Call Start");
            string _keyPath = ConfigurationManager.AppSettings["keyPath"];
            bool isFilePath = Convert.ToBoolean(ConfigurationManager.AppSettings["IsFilePathForGD"]);
            string _projectId = ConfigurationManager.AppSettings["projectId"];
            string query = ConfigurationManager.AppSettings["GCPquery"];
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(rootPath, _keyPath);
            if (isFilePath)
            {
                filePath = _keyPath;
            }

            _logger.Info("File Path : " + filePath);
            _logger.Info("Google API Call Start");
            // Create credentials using the service account key
            GoogleCredential credential;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            }

            // Initialize the BigQuery client with the credentials and project ID
            BigQueryClient client = BigQueryClient.Create(_projectId, credential);

            // Define the query with the dataset and table provided
            //        string query = @"
            //SELECT CD, WOF_CD_Target, CD_Total_Ach_Target, Target_To_WoF_Eligibility, 
            //       CD_Total_Points, SLAB_RANK, Points_To_Next_Slab, NEXT_HIGHER_SLAB, Total_Payout
            //FROM `prj-awl-dl-prod.Wheel_Of_Fortune.Wheel_Of_Fortune`
            //LIMIT 10";

            // Run the query
            BigQueryResults results = client.ExecuteQuery(query, parameters: null);
            _logger.Info("Google API Call Responce" + results.TotalRows);
            // Create a list to hold the DTOs
            var resultDto = new ResultDto();
            try
            {
                var gamificationDashboardList = new List<GamificationDashboardDatatableDto>();
                foreach (var row in results)
                {
                    // Create a dictionary to hold column name and value pairs
                    Dictionary<string, object> resultRow = new Dictionary<string, object>();
                    foreach (var field in row.Schema.Fields)
                    {
                        // Add each field name and its corresponding value to the dictionary
                        resultRow[field.Name] = row[field.Name];
                    }

                    // Map the data to GamificationDashboard model
                    GamificationDashboardDatatableDto gamificationDashboard = new GamificationDashboardDatatableDto
                    {
                        //DistributorCode = resultRow["CD"]?.ToString() ?? string.Empty,
                        //DistributorTargetMT = decimal.TryParse(resultRow["WOF_CD_Target"]?.ToString(), out var distributorTargetMT) ? distributorTargetMT : 0,
                        //DistributorAchievementTillN1MT = decimal.TryParse(resultRow["CD_Total_Ach_Target"]?.ToString(), out var distributorAchievementTarget) ? distributorAchievementTarget : 0,
                        //RemainingTargetToAchieveMT = decimal.TryParse(resultRow["Target_To_WoF_Eligibility"]?.ToString(), out var remainingAchievementTarget) ? remainingAchievementTarget : 0,
                        //EarnedPoints = resultRow["CD_Total_Points"] == DBNull.Value ? 0 : Convert.ToInt64(GetValue(resultRow["CD_Total_Points"].ToString())),
                        //CurrentSlab = resultRow["SLAB_RANK"]?.ToString() ?? string.Empty,
                        //NextHigherSlab = resultRow["NEXT_HIGHER_SLAB"]?.ToString() ?? string.Empty,
                        //PointsToBeEarnedToReachNextHigherSlab = decimal.TryParse(resultRow["Points_To_Next_Slab"]?.ToString(), out var pointsToBeEarnedToReachNextHigherSlab) ? pointsToBeEarnedToReachNextHigherSlab : 0,
                        //TotalEarningsInRs = decimal.TryParse(resultRow["Total_Payout"]?.ToString(), out var totalEarningsInRs) ? totalEarningsInRs : 0,
                        //SpecialBonusMessage = resultRow["SpecialBonusMessage"]?.ToString() ?? string.Empty,
                        //IsDiamond = resultRow["Diamond_NonDiamond"]?.ToString().Trim().Equals("Diamond", StringComparison.OrdinalIgnoreCase) ?? false
                        //DistributorCode = resultRow["CD"]?.ToString() ?? string.Empty,
                        //DistributorCode = resultRow["CD"]?.ToString() ?? string.Empty,

                        //DistributorCode = resultRow.ContainsKey("CD") ? resultRow["CD"]?.ToString() ?? string.Empty : string.Empty,
                        //DistributorTargetMT = decimal.TryParse(resultRow["Target"]?.ToString(), out var distributorTargetMT) ? distributorTargetMT : 0,
                        //DistributorAchievementTillN1MT = decimal.TryParse(resultRow["Achievement"]?.ToString(), out var distributorAchievementTarget) ? distributorAchievementTarget : 0,
                        //RemainingTargetToAchieveMT = decimal.TryParse(resultRow["Balance_Target"]?.ToString(), out var remainingAchievementTarget) ? remainingAchievementTarget : 0,
                        //EarnedPoints = resultRow["Earned_Points"] == DBNull.Value ? 0 : Convert.ToInt64(GetValue(resultRow["Earned_Points"].ToString())),
                        //CurrentSlab = resultRow["CD_Name"]?.ToString() ?? string.Empty,
                        ////CurrentSlab = resultRow["SLAB_RANK"]?.ToString() ?? string.Empty,
                        ////NextHigherSlab = resultRow["NEXT_HIGHER_SLAB"]?.ToString() ?? string.Empty,
                        //PointsToBeEarnedToReachNextHigherSlab = decimal.TryParse(resultRow["Points_To_Next_Slab"]?.ToString(), out var pointsToBeEarnedToReachNextHigherSlab) ? pointsToBeEarnedToReachNextHigherSlab : 0,
                        //TotalEarningsInRs = decimal.TryParse(resultRow["Qualified_Points"]?.ToString(), out var totalEarningsInRs) ? totalEarningsInRs : 0,
                        //SpecialBonusMessage = resultRow["Udaan_Reward"]?.ToString() ?? string.Empty,
                        //IsDiamond = resultRow["Qualification_Status"]?.ToString().Trim().Equals("Diamond", StringComparison.OrdinalIgnoreCase) ?? false

                        DistributorCode = resultRow != null && resultRow.ContainsKey("CD") && resultRow["CD"] != null ? resultRow["CD"].ToString() : string.Empty,
                        DistributorTargetMT = resultRow != null && resultRow.ContainsKey("Target") && resultRow["Target"] != null && decimal.TryParse(resultRow["Target"].ToString(), out var distributorTargetMT) ? distributorTargetMT : 0,
                        DistributorAchievementTillN1MT = resultRow != null && resultRow.ContainsKey("Achievement") && resultRow["Achievement"] != null && decimal.TryParse(resultRow["Achievement"].ToString(), out var distributorAchievementTarget) ? distributorAchievementTarget : 0,
                        RemainingTargetToAchieveMT = resultRow != null && resultRow.ContainsKey("Balance_Target") && resultRow["Balance_Target"] != null && decimal.TryParse(resultRow["Balance_Target"].ToString(), out var remainingAchievementTarget) ? remainingAchievementTarget : 0,
                        EarnedPoints = resultRow != null && resultRow.ContainsKey("Earned_Points") && resultRow["Earned_Points"] != null && resultRow["Earned_Points"] != DBNull.Value ? Convert.ToInt64(GetValue(resultRow["Earned_Points"].ToString())) : 0,
                        CurrentSlab = resultRow != null && resultRow.ContainsKey("CD_Name") && resultRow["CD_Name"] != null ? resultRow["CD_Name"].ToString() : string.Empty,
                        PointsToBeEarnedToReachNextHigherSlab = resultRow != null && resultRow.ContainsKey("Points_To_Next_Slab") && resultRow["Points_To_Next_Slab"] != null && decimal.TryParse(resultRow["Points_To_Next_Slab"].ToString(), out var pointsToBeEarnedToReachNextHigherSlab) ? pointsToBeEarnedToReachNextHigherSlab : 0,
                        TotalEarningsInRs = resultRow != null && resultRow.ContainsKey("Qualified_Points") && resultRow["Qualified_Points"] != null && decimal.TryParse(resultRow["Qualified_Points"].ToString(), out var totalEarningsInRs) ? totalEarningsInRs : 0,
                        SpecialBonusMessage = resultRow != null && resultRow.ContainsKey("Udaan_Reward") && resultRow["Udaan_Reward"] != null ? resultRow["Udaan_Reward"].ToString() : string.Empty,
                        IsDiamond = resultRow != null && resultRow.ContainsKey("Qualification_Status") && resultRow["Qualification_Status"] != null && resultRow["Qualification_Status"].ToString().Trim().Equals("Diamond", StringComparison.OrdinalIgnoreCase),


                    };
                    gamificationDashboardList.Add(gamificationDashboard);
                    // Call stored procedure to save the data
                }
                SaveGamificationDashboard(gamificationDashboardList);
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        private void SaveGamificationDashboard(List<GamificationDashboardDatatableDto> gamificationDashboardList)
        {
            //GamificationDashboardDatatableDto result = new GamificationDashboardDatatableDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                {
                    try
                    {
                        //var gamificationDashboardList = new List<GamificationDashboard>();
                        var GamificationDatalist = ResultService.ConvertToDataTable(gamificationDashboardList);

                        //var IsDiamond = gamificationDashboard.IsDiamond == "1" ? true : false;
                        connection.Open();
                        var result = connection.Execute("[UpdateOrInsertGamificationData]", new
                        {
                            GamificationDatalist = GamificationDatalist.AsTableValuedParameter("[dbo].[DistributorPerformanceTableType]")
                            //gamificationDashboard.DistributorCode,
                            //gamificationDashboard.DistributorTargetMT,
                            //gamificationDashboard.DistributorAchievementTillN1MT,
                            //gamificationDashboard.RemainingTargetToAchieveMT,
                            //gamificationDashboard.EarnedPoints,
                            //gamificationDashboard.CurrentSlab,
                            //gamificationDashboard.NextHigherSlab,
                            //gamificationDashboard.PointsToBeEarnedToReachNextHigherSlab,
                            //gamificationDashboard.TotalEarningsInRs,
                            //gamificationDashboard.SpecialBonusMessage,
                            //inputDto.WholePointsStructure,
                            //inputDto.IsActive,
                            //gamificationDashboard.IsDiamond
                        }, commandType: System.Data.CommandType.StoredProcedure);
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }

        private string GetValue(string value)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                result = value.Trim('{', '}');
            }

            return result;
        }

        #endregion


        public ResultDto GetRolesList()
        {
            _methodName = "GetRolesList";
            var resultDto = new ResultDto();
            try
            {
                var roleIds = new List<long> { 5, 7, 9, 12 };

                var rolesList = _emamiContext.Roles
                    .Where(r => !r.IsDeleted && roleIds.Contains(r.Id))
                    .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                    }).ToList();

                resultDto.SuccessDto.Response = rolesList;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }



        #region //TANNumber Mobile API

        public ResultDto GetTANNumber(DealerTANDto dealerTANDto)
        {
            _methodName = "GetTANNumber";
            var resultDto = new ResultDto();
            var delardto = new DealerTANDto();
            try
            {
                // Fetch the user based on the provided UserId
                var resultContext = _emamiContext.Users.AsNoTracking()
                    .FirstOrDefault(user => user.Id == dealerTANDto.UserId && !string.IsNullOrEmpty(user.Code));

                if (resultContext != null)
                {
                    // Set TANNumber to an empty string if it is null
                    delardto.TANNumber = resultContext.TANNumber ?? string.Empty;
                    return _resultService.SuccessObject(delardto);
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = "User not found or invalid TAN number.";
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        /// <summary>
        /// Method to Update SubCategory
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateTANNumber(DealerTANDto dealerTANDto)
        {
            _methodName = "UpdateTANNumber";
            var resultDto = new ResultDto();
            try
            {
                if (dealerTANDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                // Retrieve the current user by UserId
                var currentUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == dealerTANDto.UserId);

                if (currentUser == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = "User not found."; // Handle case where user is not found
                    return resultDto;
                }

                // Check if any other user already has the same TAN number
                var existingUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(c => c.TANNumber == dealerTANDto.TANNumber && c.Id != dealerTANDto.UserId);

                if (existingUser != null)
                {
                    // If an existing user with the same TAN number is found, return an error
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.TannumberalreadyExist;
                    return resultDto;
                }

                // Update the TAN number for the current user
                currentUser.TANNumber = dealerTANDto.TANNumber;

                _emamiContext.SaveChanges();
                resultDto.SuccessDto.Message = "TAN Number updated successfully.";
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = "An unexpected error occurred. Please try again later.";
                _logger.Error(message);
                return resultDto;
            }
        }
        #endregion  ValidateCalendar

        public ResultDto ValidateCalendar()
        {
            _methodName = "ValidateCalendar";
            var resultDto = new ResultDto();

            // Ensure nested objects are initialized
            resultDto.SuccessDto = new SuccessDto();
            resultDto.ErrorDto = new ErrorDto();

            try
            {
                var configContext = _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.AccountStatementDays);
                if (configContext != null)
                {
                    //resultDto.SuccessDto.Response = (int)DTO.Enums.Configuration.NotificationEmail;
                    resultDto.SuccessDto.Response = configContext;
                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = "Configuration not found";
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception.Message}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }

            return resultDto;
        }

        #region Account Statement


        public ResultDto AccountStatementCount(CustomerAccountStatementDto inputDto)
        {
            _methodName = nameof(AccountStatementCount);
            var resultDto = new ResultDto();

            try
            {
                if (inputDto.CustomerUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = "CustomerUserId cannot be null or empty.";
                    return resultDto;
                }

                var accountStatement = new Adani.Solution.Data.CustomerAccountStatement()
                {
                    CustomerUserId = inputDto.CustomerUserId,
                    IsSubmitted = false,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };

                _emamiContext.CustomerAccountStatement.Add(accountStatement);
                _emamiContext.SaveChanges();

                var totalCount = _emamiContext.CustomerAccountStatement.Where(x => x.CustomerUserId == inputDto.CustomerUserId && x.IsSubmitted).Count();

                var configContext = _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.AccountStatementHitCount);

                var countLimit = configContext.Value;

                var statementcount = new CustomerAccountStatementDto()
                {
                    Requestid = accountStatement.Id,
                    CustomerUserId = inputDto.CustomerUserId,
                    Totalcount = totalCount,
                    CountLimit = countLimit,
                };

                resultDto.SuccessDto.Response = new
                {
                    //TotalCount = totalCount,
                    AccountStatements = statementcount
                };
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }

            return resultDto;
        }


        /// <summary>
        /// Method to Update SubCategory
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateAccountStatementStatus(CustomerAccountStatementDto customerAccountStatementDto)
        {
            _methodName = "UpdateAccountStatementStatus";
            var resultDto = new ResultDto();
            try
            {
                if (customerAccountStatementDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var currentRequest = _emamiContext.CustomerAccountStatement.FirstOrDefault(_ => _.Id == customerAccountStatementDto.Requestid);

                if (currentRequest == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = "RequestId not found.";
                    return resultDto;
                }

                currentRequest.IsSubmitted = true;

                _emamiContext.SaveChanges();

                resultDto.SuccessDto.Message = "Account statement status updated successfully.";
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = "An unexpected error occurred. Please try again later.";
                _logger.Error(message);
                return resultDto;
            }
        }

        public async Task<ResultDto> AddAndUpdateSAPEmailStatement(SAPEmailStatementInputDto inputDto)
        {
            _methodName = "AddAndUpdateSAPEmailStatement";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null || inputDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return await Task.FromResult(resultDto);
                }

                var customerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                var sapEmailContext = _emamiContext.SAPEmailStatement.FirstOrDefault(_ => _.Id == inputDto.Id);

                if (sapEmailContext != null)
                {
                    sapEmailContext.Currency = inputDto.Currency;
                    sapEmailContext.CustomerName = inputDto.CustomerName;
                    sapEmailContext.CompanyName = inputDto.CompanyName;
                    sapEmailContext.IsWithoutSpecialGL = inputDto.IsWithoutSpecialGL;
                    sapEmailContext.FromDate = inputDto.FromDate;
                    sapEmailContext.ToDate = inputDto.ToDate;
                    sapEmailContext.DocumentType = inputDto.DocumentType;
                    sapEmailContext.ModifiedBy = inputDto.LoginUserId;
                    sapEmailContext.ModifiedDate = DateTime.UtcNow;

                    _emamiContext.SAPEmailStatement.AddOrUpdate(sapEmailContext);
                    _emamiContext.SaveChanges();
                }
                else
                {
                    var emailContext = new SAPEmailStatement
                    {
                        CompanyName = inputDto.CompanyName,
                        CustomerName = inputDto.CompanyName,
                        Currency = inputDto.Currency,
                        DocumentType = inputDto.DocumentType,
                        IsWithoutSpecialGL = inputDto.IsWithoutSpecialGL,
                        FromDate = inputDto.FromDate,
                        ToDate = inputDto.ToDate,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };

                    _emamiContext.SAPEmailStatement.Add(emailContext);
                    _emamiContext.SaveChanges();
                    sapEmailContext = emailContext;
                }
                int accountStatementId = (int)sapEmailContext.Id;  
                string startDate = inputDto.FromDate.ToString("yyyyMMdd"); 
                string endDate = inputDto.ToDate.ToString("yyyyMMdd");

                int formatOption = sapEmailContext.DocumentType; 
                SAPEmailStatementDocumentType documentType = (SAPEmailStatementDocumentType)formatOption;

                string format = UtilityHelper.GetEnumDescription(documentType);

                var sapStatement = new SAPStatementDto
                {
                    statement = new List<statement>
                    {
                        new statement
                        {
                            AccountStatementId = accountStatementId, 
                            compCode = "9010", 
                            customer = customerContext.Code, 
                            startDate = startDate, 
                            endDate = endDate, 
                            spGL_A = "X", 
                            spGL_H =  "",
                            format = format,
                        }
                    }
                };
                
                string jsonPayload = JsonConvert.SerializeObject(sapStatement);
                _logger.Info($"Sending JSON Payload: {jsonPayload}");
                var response = _resultService.PostAsyncWithBaicAuthentication(ConsoleSettings.CustomerStatement, sapStatement);
                _logger.Info($"Sap Statement Response: {response}");
                var status = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Message = Constants.RecordSaved;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = $"SAP API Error: {response.StatusCode}";
                }

                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = "An unexpected error occurred. Please try again later.";
                _logger.Error(message);
                return resultDto;
            }
        }

        public async Task<ResultDto> UpdateEmailStatementSAPStatus(SAPEmailStatementDStatusDto inputDto)
        {
            _methodName = "UpdateSAPEmailStatementStatus";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return await Task.FromResult(resultDto);
                }

                var sapEmailContext = _emamiContext.SAPEmailStatement.FirstOrDefault(_ => _.Id == inputDto.AccountStatementId);

                if(sapEmailContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidAccountSatementId;
                    return await Task.FromResult(resultDto);
                }

                if (sapEmailContext != null)
                {
                    sapEmailContext.SAPStatus = inputDto.StatusMessage;
                    _emamiContext.SAPEmailStatement.AddOrUpdate(sapEmailContext);
                    _emamiContext.SaveChanges();
                }
                var user = _emamiContext.Users.FirstOrDefault(u => u.Id == sapEmailContext.CreatedBy);
                string statusDaysConfig = ConfigurationManager.AppSettings["EmailStatementSAPStatusDays"];
                List<int> statusDays = statusDaysConfig.Split(',').Select(int.Parse).ToList();

                string statusMessageConfig = ConfigurationManager.AppSettings["EmailStatementSAPStatusMessage"];

                string message;

                if (statusDays.Contains(DateTime.Now.Day))
                {
                    message = statusMessageConfig;
                }
                else
                {
                    message = $"Your SAP email statement status has been updated to: {inputDto.StatusMessage}";
                }
                var pushNotificationInputDto = new PushNotificationInputDto
                {
                    PushTokenKey = user.PushTokenKey,
                    RegistrationTypeId = (int)user.RegistrationTypeId,
                    Title = "SAP Email Statement Update",
                    //Message = $"Your SAP email statement status has been updated to: {inputDto.StatusMessage}",
                    Message = message,
                    Id = sapEmailContext.Id.ToString()
                };
                Adani.Solution.Service.INotificationService notificationService = new Adani.Solution.Service.NotificationService(_emamiContext, _resultService);
                notificationService.SendPushNotificationThroughFirebaseNew(pushNotificationInputDto);


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.AccountSatementStatus;
                return await Task.FromResult(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = "An unexpected error occurred. Please try again later.";
                _logger.Error(message);
                return resultDto;
            }
        }
        #endregion


        #region GeographyDiscount
        public async Task<ResultDto> ImportGeographyDiscount(List<GeographyDiscountImportStatus> inputDto)
        {
            _methodName = "ImportGeographyDiscount";
            var resultDto = new ResultDto();
            var discountData = new List<GeographyDiscountImportStatus>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return await Task.FromResult(resultDto);
                }

                using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                {
                    try
                    {
                        var geographyDiscountImportList = InsertGeographyImportStatusData(inputDto);

                        foreach (var input in geographyDiscountImportList)
                        {
                            var spaceremoved = string.Join(",", input.MaterialCode.Split(',')
                                .Select(x => x.Trim())
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .ToList());

                            input.MaterialCode = spaceremoved;

                            var discountInfo = connection.Query<GeographyDiscountImportStatus>("SetGeographyDiscount", new
                            {
                                input.LoginUserId,
                                input.SalesOrganization,
                                input.DistributionChannel,
                                input.Division,
                                input.Discount,
                                input.DiscountReason,
                                input.MaterialCode,
                                input.Zone,
                                input.State,
                                input.District,
                                input.City,
                                input.ValidFrom,
                                input.ValidTo,
                                input.OilType,
                                input.PackGroup,
                                input.PackType,
                                input.IsActive
                            }, 
                            commandTimeout: 0, 
                            commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                            discountInfo.Id = input.Id;
                            discountData.Add(discountInfo);

                            UpdateGeographyImportStatusData(discountData);
                        }

                        UpdateGeographyImportStatusData(discountData);
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.AccountSatementStatus;
                return await Task.FromResult(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = "An unexpected error occurred. Please try again later.";
                _logger.Error(message);
                return resultDto;
            }
        }


        private List<GeographyDiscountImportStatus> InsertGeographyImportStatusData(List<GeographyDiscountImportStatus> inputDto)
        {
            var discountInfo = new List<GeographyDiscountImportStatus>();

            if (inputDto.Any())
            {
                using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                {
                    var datatable = ResultService.ConvertToDataTable(inputDto);
                    var parameters = new DynamicParameters();
                    parameters.Add("@DiscountGeographyImportStatusData", datatable.AsTableValuedParameter("[dbo].[DiscountGeographyImportStatusType]"));

                    discountInfo = connection.Query<GeographyDiscountImportStatus>("[dbo].[BulkInsertDiscountGeographyImportStatus]", parameters, 
                    commandTimeout: 0,
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }

            return discountInfo;
        }

        private List<GeographyDiscountImportStatus> UpdateGeographyImportStatusData(List<GeographyDiscountImportStatus> inputDto)
        {
            var discountInfo = new List<GeographyDiscountImportStatus>();

            if (inputDto.Any())
            {
                using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                {
                    var datatable = ResultService.ConvertToDataTable(inputDto);
                    var parameters = new DynamicParameters();
                    parameters.Add("@DiscountGeographyImportStatusData", datatable.AsTableValuedParameter("[dbo].[DiscountGeographyImportStatusType]"));

                    discountInfo = connection.Query<GeographyDiscountImportStatus>("[dbo].[BulkInsertDiscountGeographyImportStatus]", parameters,
                    commandTimeout: 0,
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }

            return discountInfo;
        }

        #endregion
    }
}