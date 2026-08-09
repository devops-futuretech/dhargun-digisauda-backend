using Dapper;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Adani.Solution.MVC.Controllers;
using System.Globalization;

namespace Adani.Solution.MVC.ServiceClient
{
    public class ImportClient : BaseClient
    {
        private const string ServiceName = "Import Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        static string connectionString = ConfigHelper.SPConnectionString;

        #region Import Masters

        public StateUploadDto InsertState(string countryName, string stateName, string isActive)
        {
            _methodName = "InsertState";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            StateUploadDto result = new StateUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<StateUploadDto>("SetState", new { CountryName = countryName, StateName = stateName, IsActive = isActive }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;// Helper.GetResourceString("msg_InsertStateError");
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertStateError");
                _logger.Error(message);
            }
            return result;
        }

        public TerritoryUploadDto InsertTerritory(string territoryName, string stateName, string isActive)
        {
            _methodName = "InsertTerritory";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            TerritoryUploadDto result = new TerritoryUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<TerritoryUploadDto>("SetTerritory", new { TerritoryName = territoryName, StateName = stateName, IsActive = isActive }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;// Helper.GetResourceString("msg_InsertStateError");
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertTerritoryError");
                _logger.Error(message);
            }
            return result;
        }

        public CityUploadDto InsertCity(string CityName, string DistrictName, string StateName, string IsActive)
        {
            _methodName = "InsertCity";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            CityUploadDto result = new CityUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<CityUploadDto>("SetCity", new { CityName, DistrictName, StateName, IsActive }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertCityError");
                _logger.Error(message);
            }
            return result;
        }

        public DistrictUploadDto InsertDistrict(string districtName, string stateName, string isActive)
        {
            _methodName = "InsertDistrict";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DistrictUploadDto result = new DistrictUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DistrictUploadDto>("SetDistrict", new { DistrictName = districtName, StateName = stateName, IsActive = isActive }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }

        public DistrictUploadDto InsertTerritoryDistrictMapping(string stateName, string territoryName, string districtName, string isActive)
        {
            _methodName = "InsertTerritoryDistrictMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DistrictUploadDto result = new DistrictUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DistrictUploadDto>("SetTerritoryDistrict", new { StateName = stateName, TerritoryName = territoryName, DistrictName = districtName, IsActive = isActive }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertTerritoryDistrictMappingError");
                _logger.Error(message);
            }
            return result;
        }

        public FreightZoneUploadDto InsertFreightZone(FreightZoneUploadDto inputDto)
        {
            _methodName = "InsertFreightZone";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            FreightZoneUploadDto result = new FreightZoneUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<FreightZoneUploadDto>("SetFreightZone", new
                        {
                            inputDto.Name,
                            inputDto.StateName,
                            inputDto.ZoneName,
                            inputDto.CreatedBy,
                            inputDto.IsActive,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertFreightZoneError");
                _logger.Error(message);
            }
            return result;
        }

        public FreightRouteUploadDto InsertFreightRoute(FreightRouteUploadDto inputDto)
        {
            _methodName = "InsertFreightRoute";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            FreightRouteUploadDto result = new FreightRouteUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<FreightRouteUploadDto>("SetFreightRoute", new
                        {
                            inputDto.Name,
                            inputDto.FreightZoneName,
                            inputDto.IsActive,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertFreightRouteError");
                _logger.Error(message);
            }
            return result;
        }

        public OilTypeUploadDto InsertOilType(OilTypeUploadDto inputDto)
        {
            _methodName = "InsertOilType";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            OilTypeUploadDto result = new OilTypeUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<OilTypeUploadDto>("SetOilType", new
                        {
                            inputDto.DivisionCode,
                            inputDto.Name,
                            // inputDto.LitreConversion,
                            inputDto.IsActive,
                            inputDto.CreatedBy,
                            //inputDto.Code,
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDepotCostError");
                _logger.Error(message);
            }
            return result;
        }

        public VehicleLoadabilitiesDto InsertVehicleLoadabilities(VehicleLoadabilitiesDto inputDto)
        {
            _methodName = "InsertVehicleLoadabilities";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            VehicleLoadabilitiesDto result = new VehicleLoadabilitiesDto();
            try
            {
                bool tempIsActive = false;
                if (inputDto.IsActive == 1)
                {
                    tempIsActive = true;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<VehicleLoadabilitiesDto>("SetVehicleLoadabilities", new
                        {
                            ZoneName = inputDto.ZoneName,
                            StateName = inputDto.StateName,
                            FreightZoneName = inputDto.FreightZoneName,
                            IsActive = tempIsActive,
                            VehicleSize = inputDto.VehicleSize,
                            CreatedBy = inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertVehicleLoadabilities");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import Pricing

        public MaterialCostUploadDto InsertMaterialCost(MaterialCostUploadDto inputDto)
        {
            _methodName = "InsertMaterialCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            MaterialCostUploadDto result = new MaterialCostUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<MaterialCostUploadDto>("SetMaterialCost", new
                        {
                            inputDto.PlantCode,
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.RateOrMT,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertMaterialCostError");
                _logger.Error(message);
            }
            return result;
        }

        public RAMaterialCostUploadDto InsertRAMaterialCost(RAMaterialCostUploadDto inputDto)
        {
            _methodName = "InsertRAMaterialCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            RAMaterialCostUploadDto result = new RAMaterialCostUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<RAMaterialCostUploadDto>("SetRAMaterialCost", new
                        {
                            inputDto.PlantCode,
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.RateOrMT,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertMaterialCostError");
                _logger.Error(message);
            }
            return result;
        }

        public PackingCostUploadDto InsertPackingCost(PackingCostUploadDto inputDto)
        {
            _methodName = "InsertPackingCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            PackingCostUploadDto result = new PackingCostUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PackingCostUploadDto>("SetPackingCost", new
                        {
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.PlantCode,
                            inputDto.ActualPackingCost,
                            inputDto.SalesPackingCost,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertPackingCostError");
                _logger.Error(message);
            }
            return result;
        }

        public PrimaryFreightUploadDto InsertPrimaryFreight(PrimaryFreightUploadDto inputDto)
        {
            _methodName = "InsertPrimaryFreight";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            PrimaryFreightUploadDto result = new PrimaryFreightUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PrimaryFreightUploadDto>("SetPrimaryFreight", new
                        {
                            inputDto.VerticalCode,
                            inputDto.DepotCode,
                            inputDto.TransportMode,
                            inputDto.PlantCode,
                            inputDto.LoadCapacity,
                            inputDto.ActualFreight,
                            inputDto.SalesFreight,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertPrimaryFreightError");
                _logger.Error(message);
            }
            return result;
        }

        public DepotCostUploadDto InsertDepotCost(DepotCostUploadDto inputDto)
        {
            _methodName = "InsertDepotCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DepotCostUploadDto result = new DepotCostUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DepotCostUploadDto>("SetDepotCost", new
                        {
                            inputDto.VerticalCode,
                            inputDto.DepotCode,
                            inputDto.SkuCode,
                            inputDto.PackGroup,
                            inputDto.OilType,
                            inputDto.CostPerMT,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDepotCostError");
                _logger.Error(message);
            }
            return result;
        }

        public DetentionCostUploadDto InsertDetentionCost(DetentionCostUploadDto inputDto)
        {
            _methodName = "InsertDetentionCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DetentionCostUploadDto result = new DetentionCostUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DetentionCostUploadDto>("SetDetentionCost", new
                        {
                            inputDto.VerticalCode,
                            inputDto.DepotCode,
                            inputDto.CostPerMT,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDetentionCostError");
                _logger.Error(message);
            }
            return result;
        }

        public HoneyCombCostUploadDto InsertHoneyCombCost(HoneyCombCostUploadDto inputDto)
        {
            _methodName = "InsertHoneyCombCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            HoneyCombCostUploadDto result = new HoneyCombCostUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<HoneyCombCostUploadDto>("SetHoneyCombCost", new
                        {
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.PlantCode,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.Zone,
                            inputDto.State,
                            inputDto.TransportMode,
                            inputDto.CostPerMT,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertHoneyCombCostError");
                _logger.Error(message);
            }
            return result;
        }

        public ProfitMarginUploadDto InsertProfitMargin(ProfitMarginUploadDto inputDto)
        {
            _methodName = "InsertProfitMargin";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            ProfitMarginUploadDto result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<ProfitMarginUploadDto>("SetProfitMargin", new
                        {
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.OilPackingType,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.ZoneName,
                            inputDto.City,
                            inputDto.District,
                            inputDto.Territory,
                            inputDto.StateName,
                            inputDto.RatePerMt,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = exception.Message;
                _logger.Error(message);
            }

            return result;
        }

        public CushionMarginUploadDto InsertCushionMargin(CushionMarginUploadDto inputDto)
        {
            _methodName = "InsertCushionMargin";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            CushionMarginUploadDto result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<CushionMarginUploadDto>("SetCushionMargin", new
                        {
                            inputDto.SalesOrganization,
                            inputDto.DistributionChannel,
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.OilPackingType,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.ZoneName,
                            inputDto.StateName,
                            inputDto.Territory,
                            inputDto.District,
                            inputDto.City,
                            inputDto.RatePerMt,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = exception.Message;
                _logger.Error(message);
            }


            return result;
        }

        public RAMarginUploadDto InsertRAMargin(RAMarginUploadDto inputDto)
        {
            _methodName = "InsertRAMargin";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            RAMarginUploadDto result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<RAMarginUploadDto>("SetRAMargin", new
                        {
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.OilPackingType,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.ZoneName,
                            inputDto.StateName,
                            inputDto.Territory,
                            inputDto.District,
                            inputDto.City,
                            inputDto.RatePerMt,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = exception.Message;
                _logger.Error(message);
            }

            return result;
        }

        public SchemeCostUploadDto InsertSchemeCost(SchemeCostUploadDto inputDto)
        {
            _methodName = "InsertSchemeCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            SchemeCostUploadDto result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SchemeCostUploadDto>("SetSchemeCosts", new
                        {
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.PackGroup,
                            Zone = inputDto.ZoneName,
                            inputDto.City,
                            inputDto.District,
                            inputDto.Territory,
                            State = inputDto.StateName,
                            inputDto.RatePerMt,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy,
                            inputDto.SkuCode,
                            inputDto.SkuName
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = exception.Message;
                _logger.Error(message);
            }
            return result;
        }

        public LoadCapacityConversionUploadDto InsertLoadCapacity(LoadCapacityConversionUploadDto inputDto)
        {
            _methodName = "InsertHoneyCombCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            LoadCapacityConversionUploadDto result = new LoadCapacityConversionUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<LoadCapacityConversionUploadDto>("SetLoadCapacity", new
                        {
                            inputDto.VerticalCode,
                            inputDto.OilType,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.TransportMode,
                            inputDto.LoadCapacity,
                            inputDto.LoadQuantity,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy,
                            inputDto.ActualLoadQuantity
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertLoadCapacityError");
                _logger.Error(message);
            }
            return result;
        }

        public IngredientsUploadDto InsertIngredients(IngredientsUploadDto inputDto)
        {
            _methodName = "InsertFreightZone";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            IngredientsUploadDto result = new IngredientsUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<IngredientsUploadDto>("SetIngredients", new
                        {
                            inputDto.Name,
                            inputDto.Vertical,
                            inputDto.IsActive,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertFreightZoneError");
                _logger.Error(message);
            }
            return result;
        }

        public IngredientCostUploadDto InsertIngredientsCost(IngredientCostUploadDto inputDto)
        {
            _methodName = "InsertFreightZone";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            IngredientCostUploadDto result = new IngredientCostUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<IngredientCostUploadDto>("SetIngredientCost", new
                        {
                            inputDto.IngredientName,
                            inputDto.Vertical,
                            inputDto.LooseOilRate,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy,
                            inputDto.PlantCode,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertFreightZoneError");
                _logger.Error(message);
            }
            return result;
        }

        public SkuIngredientUploadDto InsertSkuIngredient(SkuIngredientUploadDto inputDto)
        {
            _methodName = "InsertSkuIngredient";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            SkuIngredientUploadDto result = new SkuIngredientUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SkuIngredientUploadDto>("SetSKUIngredient", new
                        {
                            inputDto.OilType,
                            inputDto.Vertical,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.PlantCode,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.Ingredients,
                            inputDto.CreatedBy,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertFreightZoneError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import Sku

        public SkuUploadDto InsertSku(SkuUploadDto inputDto)
        {
            _methodName = "InsertSku";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            SkuUploadDto result = new SkuUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.MaterialCode))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            result = connection.Query<SkuUploadDto>("SetSku", new
                            {
                                inputDto.DivisionCode,
                                inputDto.OilTypeName,
                                //inputDto.OilTypeCode,
                                inputDto.MaterialName,
                                inputDto.MaterialCode,
                                //inputDto.PackType,
                                //inputDto.PackSize,
                                //inputDto.PackSizeQuantity,
                                inputDto.PackGroup,
                                inputDto.OilPackGroupType,
                                //inputDto.ProcessCost,
                                //inputDto.SubCategory,
                                inputDto.IsActive,
                                inputDto.UOM,
                                inputDto.ConversionFactor1,
                                inputDto.ConversionFactor2,
                                inputDto.RelationalUOM,
                                inputDto.BusinessLine,
                                inputDto.ParentMaterialCode,
                                //inputDto.UOM1_No,
                                //inputDto.Uom2_CaseToNumberConversion,
                                //inputDto.Uom3_MetricTonToNumberConversion,
                                inputDto.SapStatusId,
                                inputDto.CreatedBy,
                                //inputDto.MaterialTypeName,
                                //inputDto.IsRequiredToAttachTradeTicket,
                                //inputDto.GrossWeight,
                                //inputDto.PremiumAmount,
                                //inputDto.StorageLocation,
                                inputDto.SalesOrganizationCode,
                                inputDto.DistributionChannelCode,
                                inputDto.DiscountAutomationConversionUom,
                                inputDto.DiscountAutomationConversionRelationalUom,
                                inputDto.DiscountAutomationConversionFactor1,
                                inputDto.DiscountAutomationConversionFactor2
                            }, commandType: CommandType.StoredProcedure).First();
                        }
                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSkuError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import Broker

        public BrokerUploadDto InsertBroker(BrokerUploadDto inputDto)
        {
            _methodName = "InsertBroker";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            BrokerUploadDto result = new BrokerUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.Code))
                {
                    inputDto.RoleId = (int)DTO.Enums.Role.Broker;
                    //inputDto.IncoTerms = inputDto.IncoTerms.Replace("'", "''");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            result = connection.Query<BrokerUploadDto>("SetBroker", new
                            {
                                inputDto.Code,
                                inputDto.Name,
                                inputDto.MobileNumber,
                                inputDto.MobileNumber2,
                                inputDto.Email,
                                inputDto.GSTN,
                                inputDto.ZoneName,
                                inputDto.StateName,
                                //inputDto.TerritoryName,
                                inputDto.DistrictName,
                                inputDto.CityName,
                                inputDto.Pincode,
                                inputDto.Address1,
                                inputDto.Address2,
                                inputDto.IsActive,
                                inputDto.CreatedBy,
                                inputDto.RoleId,
                                //inputDto.SalesOrganizationCode,
                                //inputDto.DistributionChannelCode,
                                //inputDto.DivisionCode,
                                inputDto.Password,
                                inputDto.EncryptedPassword,
                                //inputDto.CompanyCode
                                //inputDto.FreightZoneName,
                                //inputDto.FreightRouteName,
                                //inputDto.PlantTruckCapacity,
                                //inputDto.DepotTruckCapacity,
                                //inputDto.IncoTerms,
                                //inputDto.TransportMode,
                                //inputDto.SaudaBookingType,
                                //inputDto.SaudaValidityPeriod,
                                //inputDto.SaudaLimit,
                            }, commandType: CommandType.StoredProcedure).First();
                        }
                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertBrokerError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import User Targets
        public UserCustomerSalesTargetUploadDto InsertUserCustomerTarget(UserCustomerSalesTargetUploadDto inputDto)
        {
            _methodName = "InsertUserCustomerSalesTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            UserCustomerSalesTargetUploadDto result = new UserCustomerSalesTargetUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<UserCustomerSalesTargetUploadDto>("SetUserCustomerTarget", new
                        {
                            inputDto.AssignedFromUserCode,
                            inputDto.AssignedToUserCode,
                            inputDto.Quarter,
                            inputDto.Month,
                            inputDto.Year,
                            inputDto.Target,
                            inputDto.FinancialYear,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserCustomerSalesTargetError");
                _logger.Error(message);
            }
            result.PostStatus = true;
            return result;
        }

        public UserCustomerSalesTargetUploadDto InsertUserCustomerSalesTarget(UserCustomerSalesTargetUploadDto inputDto)
        {
            _methodName = "InsertUserCustomerSalesTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            UserCustomerSalesTargetUploadDto result = new UserCustomerSalesTargetUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<UserCustomerSalesTargetUploadDto>("SetUserCustomerSalesTarget", new
                        {
                            inputDto.OilTypeName,
                            inputDto.DivisionCode,
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            inputDto.AssignedFromUserCode,
                            inputDto.AssignedToUserCode,
                            inputDto.Quarter,
                            inputDto.Month,
                            inputDto.Year,
                            inputDto.Target,
                            inputDto.FinancialYear,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserCustomerSalesTargetError");
                _logger.Error(message);
            }
            result.PostStatus = true;
            return result;
        }

        public UserCustomerSaudaTargetUploadDto InsertUserCustomerSaudaTarget(UserCustomerSaudaTargetUploadDto inputDto)
        {
            _methodName = "InsertUserCustomerSalesTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            UserCustomerSaudaTargetUploadDto result = new UserCustomerSaudaTargetUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<UserCustomerSaudaTargetUploadDto>("SetUserCustomerSaudaTarget", new
                        {
                            inputDto.OilTypeName,
                            inputDto.DivisionCode,
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            inputDto.AssignedFromUserCode,
                            inputDto.AssignedToUserCode,
                            inputDto.Quarter,
                            inputDto.Month,
                            inputDto.Year,
                            inputDto.Target,
                            inputDto.FinancialYear,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserCustomerSalesTargetError");
                _logger.Error(message);
            }
            result.PostStatus = true;
            return result;
        }

        //public UserSalesSaudaTargetUploadDto InsertUserSalesSaudaTarget(UserSalesSaudaTargetUploadDto inputDto)
        //{
        //    _methodName = "InsertUserSalesSaudaTarget";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    UserSalesSaudaTargetUploadDto result = new UserSalesSaudaTargetUploadDto();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            try
        //            {
        //                connection.Open();
        //                result = connection.Query<UserSalesSaudaTargetUploadDto>("SetUserSalesSaudaTarget", new
        //                {
        //                    inputDto.AssignedFrom,
        //                    inputDto.AssignedTo,
        //                    inputDto.Quarter,
        //                    inputDto.Month,
        //                    inputDto.Year,
        //                    inputDto.SaudaTarget,
        //                    inputDto.SalesTarget,
        //                    inputDto.CreatedBy
        //                }, commandType: CommandType.StoredProcedure).First();
        //            }
        //            catch (Exception exception)
        //            {
        //                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //                result.PostStatus = false;
        //                result.PostMessage = exception.Message;
        //                _logger.Error(message);
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        result.PostStatus = false;
        //        result.PostMessage = Helper.GetResourceString("msg_InsertUserCustomerSalesTargetError");
        //        _logger.Error(message);
        //    }
        //    result.PostStatus = true;
        //    return result;
        //}

        //public UserOilTypeTargetUploadDto InsertUserOilTypeTarget(UserOilTypeTargetUploadDto inputDto)
        //{
        //    _methodName = "InsertUserOilTypeTarget";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    UserOilTypeTargetUploadDto result = new UserOilTypeTargetUploadDto();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            try
        //            {
        //                connection.Open();
        //                result = connection.Query<UserOilTypeTargetUploadDto>("SetUserOilTypeTarget", new
        //                {
        //                    inputDto.AssignedFrom,
        //                    inputDto.AssignedTo,
        //                    inputDto.Quarter,
        //                    inputDto.Month,
        //                    inputDto.Year,
        //                    inputDto.FinancialYear,
        //                    inputDto.OilTypeName,
        //                    inputDto.Target,
        //                    inputDto.CreatedBy
        //                }, commandType: CommandType.StoredProcedure).First();
        //            }
        //            catch (Exception exception)
        //            {
        //                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //                result.PostStatus = false;
        //                result.PostMessage = exception.Message;
        //                _logger.Error(message);
        //            }
        //            finally
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        result.PostStatus = false;
        //        result.PostMessage = Helper.GetResourceString("msg_InsertUserCustomerSalesTargetError");
        //        _logger.Error(message);
        //    }
        //    result.PostStatus = true;
        //    return result;
        //}

        #endregion

        #region Import Retailer

        public RetailerUploadDto InsertRetailer(RetailerUploadDto inputDto)
        {
            _methodName = "InsertRetailer";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            RetailerUploadDto result = new RetailerUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        var IsActive = inputDto.IsActive == "1" ? true : false;

                        result = connection.Query<RetailerUploadDto>("SetRetailer", new
                        {
                            inputDto.Code,
                            inputDto.AccountName,
                            inputDto.MobileNumber,
                            inputDto.Email,
                            inputDto.SPFZoneName,
                            inputDto.StateName,
                            inputDto.DistrictName,
                            inputDto.CityName,
                            inputDto.TerritoryName,
                            inputDto.Address,
                            inputDto.Pincode,
                            IsActive,
                            inputDto.AccountManager,
                            inputDto.AccountType,
                            inputDto.AreaName,
                            inputDto.OwnersName,
                            inputDto.DistributorName,
                            inputDto.DecisionMakerName,
                            inputDto.DecisionMakerNumber,
                            inputDto.ChefName,
                            inputDto.ChefNumber,
                            inputDto.VerticalCode,
                            inputDto.DealerCode,
                            inputDto.CreatedBy,
                            inputDto.SalesOrganizationName,
                            inputDto.DistributionChannelName
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertRetailerError");
                _logger.Error(message);
            }
            result.PostStatus = true;
            return result;
        }


        #endregion

        #region Import Plant & Depot

        public PlantUploadDto InsertPlants(PlantUploadDto inputDto)
        {
            _methodName = "InsertPlants";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            PlantUploadDto result = new PlantUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PlantUploadDto>("SetPlant", new
                        {
                            inputDto.Name,
                            inputDto.Code,
                            //inputDto.Zone,
                            inputDto.Email,
                            inputDto.MobileNumber,
                            //inputDto.StateName,
                            //inputDto.TerritoryName,
                            //inputDto.DistrictName,
                            //inputDto.CityName,
                            inputDto.Address,
                            inputDto.Pincode,
                            inputDto.IsActive,
                            inputDto.CreatedBy,
                            inputDto.StorageTypeId
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }

        public PlantUploadDto InsertDepots(PlantUploadDto inputDto)
        {
            _methodName = "InsertDepots";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            PlantUploadDto result = new PlantUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PlantUploadDto>("SetDepot", new
                        {
                            inputDto.Name,
                            inputDto.Code,
                            inputDto.Email,
                            inputDto.Zone,
                            inputDto.StateName,
                            inputDto.TerritoryName,
                            inputDto.DistrictName,
                            inputDto.CityName,
                            inputDto.Address,
                            inputDto.Pincode,
                            inputDto.IsActive,
                            inputDto.CreatedBy,
                            inputDto.StorageTypeId
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }

        public RakeUploadDto InsertRake(RakeUploadDto inputDto)
        {
            _methodName = "InsertRake";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            RakeUploadDto result = new RakeUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<RakeUploadDto>("SetRake", new
                        {
                            inputDto.Name,
                            inputDto.Code,
                            inputDto.Email,
                            inputDto.Zone,
                            inputDto.StateName,
                            inputDto.TerritoryName,
                            inputDto.DistrictName,
                            inputDto.CityName,
                            inputDto.Address,
                            inputDto.Pincode,
                            inputDto.IsActive,
                            inputDto.CreatedBy,
                            inputDto.DepotCode,
                            inputDto.StorageTypeId,
                            inputDto.MappedStateName,
                            inputDto.MappedPlantCode
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }

        public PlantDepotMappingUploadDto InsertPlantDepotMapping(string PlantCode, string DepotCode, long CreatedBy)
        {
            _methodName = "InsertPlantDepotMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new PlantDepotMappingUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PlantDepotMappingUploadDto>("SetPlantDepotMapping", new
                        {
                            PlantCode,
                            DepotCode,
                            CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }

        public UserDepotMappingUploadDto InsertUserDepotMapping(UserDepotMappingUploadDto inputDto)
        {
            _methodName = "InsertPlantDepotMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new UserDepotMappingUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<UserDepotMappingUploadDto>("SetUserDepotMapping", new
                        {
                            inputDto.UserCode,
                            inputDto.DepotCode,
                            inputDto.IsDealer,
                            inputDto.DivisionCode,
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }

        public UserCustomerMappingUploadDto InsertUserCustomerMapping(UserCustomerMappingUploadDto inputDto)
        {
            _methodName = "InsertUserCustomerMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new UserCustomerMappingUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<UserCustomerMappingUploadDto>("SetUserCustomerMapping", new
                        {
                            inputDto.UserCode,
                            inputDto.CustomerCode,
                            inputDto.CreatedBy,
                            inputDto.IsDeleteOldMapping,
                            inputDto.IsUnassign
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }


        #endregion

        #region Import User

        public UserUploadDto InsertUser(UserUploadDto inputDto)
        {
            _methodName = "InsertUser";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            UserUploadDto result = new UserUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.Code))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            var IsActive = inputDto.IsActive == "1" ? true : false;
                            connection.Open();
                            result = connection.Query<UserUploadDto>("SetUser", new
                            {
                                inputDto.Code,
                                inputDto.Name,
                                inputDto.MobileNumber,
                                //inputDto.CompanyCode,
                                inputDto.Email,
                                inputDto.Designation,
                                inputDto.ZoneName,
                                inputDto.StateName,
                                //inputDto.TerritoryName,
                                inputDto.DistrictName,
                                inputDto.CityName,
                                inputDto.Pincode,
                                //inputDto.Address,
                                inputDto.Address1,
                                inputDto.Address2,
                                IsActive,
                                inputDto.RoleName,
                                //inputDto.SalesOrganizationCode,
                                //inputDto.DistributionChannelCode ,
                                //inputDto.DivisionCode,
                                inputDto.CustomerCode,
                                inputDto.CreatedBy,
                                //inputDto.Headquarters,
                                inputDto.Password,
                                inputDto.EncryptedPassword,
                                //inputDto.SalesReportingToUserCode,
                                //inputDto.OrgReportingToUserCode,
                                inputDto.ReportingToUserCode,
                                //inputDto.SaudaBookingType,
                                //inputDto.CustomerGroupOneName,
                                //inputDto.CustomerGroupTwoName
                            }, commandType: CommandType.StoredProcedure).First();
                        }

                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import Geography - State Territory District City

        public GeographyUploadDto InsertGeography(GeographyUploadDto inputDto)
        {
            _methodName = "InsertGeography";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            GeographyUploadDto result = new GeographyUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<GeographyUploadDto>("SetGeography", new
                        {
                            inputDto.CountryName,
                            inputDto.StateName,
                            inputDto.TerritoryName,
                            inputDto.DistrictName,
                            inputDto.CityName,
                            inputDto.CreatedBy,
                            inputDto.IsActive
                        }, commandType: CommandType.StoredProcedure).First();
                    }

                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import Pending Sauda

        public PendingSaudaUploadDto InsertPendingSauda(PendingSaudaUploadDto inputDto)
        {
            _methodName = "InsertPendingSauda";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            PendingSaudaUploadDto result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PendingSaudaUploadDto>("SetPendingSauda", new
                        {
                            inputDto.PlantCode,
                            inputDto.IncoTerms,
                            SaudaNumber = inputDto.ContractNo,
                            inputDto.SaudaDate,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CustomerCode,
                            inputDto.CustomerVerticalCode,
                            //inputDto.BrokerCode,
                            //inputDto.BrokerVerticalCode,
                            //inputDto.ContractQuantity,
                            //inputDto.DispatchQuantity,
                            BidQuantityCase = inputDto.PendingQuantity,
                            BidQuantityMT = inputDto.PendingQuantityMT,
                            inputDto.BasicRate,
                            inputDto.PONumber,
                            inputDto.TradeTicketNumber,
                            inputDto.SaudaBookingType,
                            inputDto.SkuCode,
                            inputDto.SkuName,
                            inputDto.SkuVerticalCode,
                            inputDto.CreatedBy,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = exception.Message;
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import Secondary Freight Master With FreightZone And FreightRoute

        public SecondaryFreightUploadDto InsertSecondaryFreightMaster(SecondaryFreightUploadDto inputDto)
        {
            _methodName = "InsertSecondaryFreightMaster";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            SecondaryFreightUploadDto result = new SecondaryFreightUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SecondaryFreightUploadDto>("SetSecondaryFreightMaster", new
                        {
                            inputDto.VerticalCode,
                            inputDto.PlantOrDepotCode,
                            inputDto.ZoneName,
                            inputDto.StateName,
                            inputDto.FreightZone,
                            inputDto.FreightRoute,
                            inputDto.TransportMode,
                            inputDto.LoadCapacity,
                            inputDto.ActualFreight,
                            inputDto.SalesFreight,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            //inputDto.IsActive,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSecondaryFreightError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import User Master With Depot and Customer Mapping

        public DealerUploadDto InsertCustomerMaster(DealerUploadDto inputDto)
        {
            _methodName = "InsertCustomerMaster";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DealerUploadDto result = new DealerUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.Code))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            var RoleId = (int)DTO.Enums.Role.Dealer;
                            connection.Open();
                            result = connection.Query<DealerUploadDto>("SetCustomerMaster", new
                            {
                                inputDto.Code,
                                inputDto.Name,
                                inputDto.MobileNumber,
                                inputDto.Email,
                                inputDto.SaudaValidityPeriod,
                                //inputDto.SaudaLimit,
                                inputDto.GSTN,
                                inputDto.IncoTerms,
                                inputDto.ZoneName,
                                inputDto.StateName,
                                //inputDto.TerritoryName,
                                inputDto.DistrictName,
                                inputDto.CityName,
                                inputDto.Pincode,
                                inputDto.Address1,
                                inputDto.Address2,
                                inputDto.BrokerCode,
                                inputDto.IsActive,
                                inputDto.CreatedBy,
                                inputDto.RoleId,
                                //inputDto.SalesOrganizationCode,
                                //inputDto.DistributionChannelCode,
                                //inputDto.DivisionCode,
                                inputDto.PlantCode,
                                inputDto.UserCode,
                                inputDto.Password,
                                inputDto.EncryptedPassword,
                                inputDto.ShipToPartyCode,
                                inputDto.CustomerGroupFiveName,
                                //inputDto.CompanyCode
                                //inputDto.CustomerGroupOneName,
                                //inputDto.CustomerGroupTwoName,
                                //inputDto.DepotCode,
                                //inputDto.FreightZoneName,
                                //inputDto.FreightRouteName,
                                //inputDto.Address,
                                //inputDto.TransportMode,
                                //inputDto.SaudaBookingType,
                                //inputDto.PlantTruckCapacity,
                                //inputDto.DepotTruckCapacity,
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import TradeTicket

        public TradeTicketUploadDto InsertTradeTicket(TradeTicketUploadDto inputDto)
        {
            _methodName = "InsertTradeTicket";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            TradeTicketUploadDto result = new TradeTicketUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<TradeTicketUploadDto>("SetTradeTicket", new
                        {
                            inputDto.ContractType,
                            inputDto.BookingType,
                            inputDto.MaterialType,
                            inputDto.ContractQuantityInMT,
                            inputDto.UnitOfMeasurement,
                            inputDto.ContractDate,
                            inputDto.PlantCode,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.OtherElementsInRsPerMT,
                            inputDto.TradeDetails_OT_OilCost_Proportion,
                            inputDto.ProcessCost,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertTradeTicketError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import ShipToParty

        public ShipToPartyUploadDto InsertShipToParty(ShipToPartyUploadDto inputDto)
        {
            _methodName = "InsertShipToParty";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            ShipToPartyUploadDto result = new ShipToPartyUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.Code))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            var RoleId = (int)DTO.Enums.Role.ShipToParty;
                            connection.Open();
                            result = connection.Query<ShipToPartyUploadDto>("SetShipToParty", new
                            {
                                inputDto.Code,
                                inputDto.Name,
                                inputDto.MobileNumber,
                                //inputDto.CompanyCode,
                                inputDto.Email,
                                //inputDto.SaudaValidityPeriod,
                                //inputDto.SaudaLimit,
                                inputDto.GSTN,
                                // inputDto.PlantTruckCapacity,
                                //inputDto.DepotTruckCapacity,
                                inputDto.IncoTerms,
                                //inputDto.TransportMode,
                                //inputDto.SaudaBookingType,
                                inputDto.ZoneName,
                                inputDto.StateName,
                                //inputDto.TerritoryName,
                                inputDto.DistrictName,
                                inputDto.CityName,
                                inputDto.Pincode,
                                inputDto.Address1,
                                inputDto.Address2,
                                //inputDto.FreightZoneName,
                                //inputDto.FreightRouteName,
                                inputDto.BrokerCode,
                                inputDto.IsActive,
                                inputDto.CreatedBy,
                                RoleId,
                                //inputDto.SalesOrganizationCode,
                                //inputDto.DistributionChannelCode,
                                //inputDto.DivisionCode,
                                inputDto.PlantCode,
                                //inputDto.DepotCode,
                                //inputDto.UserCode,
                                inputDto.Password,
                                inputDto.EncryptedPassword,
                                // inputDto.Latitude,
                                //inputDto.Longitude,
                                inputDto.CustomerGroupFiveName
                            }, commandType: CommandType.StoredProcedure).First();
                        }
                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertShipToPartyError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import CustomerGroup

        public CustomerGroupUploadDto InsertCustomerGroup(CustomerGroupUploadDto inputDto)
        {
            _methodName = "InsertCustomerGroup";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            CustomerGroupUploadDto result = new CustomerGroupUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.CustomerGroupName))
                {
                    if (!string.IsNullOrEmpty(inputDto.CustomerCode))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                result = connection.Query<CustomerGroupUploadDto>("SetCustomerGroup", new
                                {
                                    inputDto.CustomerGroupName,
                                    inputDto.CustomerCode,
                                    inputDto.IsBaseGroup,
                                    inputDto.IsActive,
                                    inputDto.CreatedBy,
                                    inputDto.VerticalCode,
                                }, commandType: CommandType.StoredProcedure).First();
                            }
                            catch (Exception exception)
                            {
                                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                                result.PostStatus = false;
                                result.PostMessage = exception.Message;
                                _logger.Error(message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                    else
                    {
                        result.Message = Helper.GetResourceString("msg_CustomersAreEmpty");
                    }
                }
                else
                {
                    result.Message = Helper.GetResourceString("msg_CustomerGroupNameIsEmpty");
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertShipToPartyError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import PercentileNumber

        public PercentileNumbersUploadDto InsertPercentileNumbers(PercentileNumbersUploadDto inputDto)
        {
            _methodName = "InsertPercentileNumbers";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            PercentileNumbersUploadDto result = new PercentileNumbersUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PercentileNumbersUploadDto>("SetDepot", new
                        {
                            inputDto.OilTypeId,
                            inputDto.PackGroupId,
                            inputDto.PercentileNumbers,
                            inputDto.IsActive,
                            inputDto.CreatedBy,
                            inputDto.CreatedDate,
                            inputDto.ValidFrom,
                            inputDto.ValidTo
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertDistrictError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import GST

        public GSTUploadDto InsertGST(GSTUploadDto inputDto)
        {
            _methodName = "InsertGST";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            GSTUploadDto result = new GSTUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<GSTUploadDto>("SetGstNew", new
                        {
                            inputDto.SourceState,
                            inputDto.DestinationState,
                            inputDto.PlantName,
                            inputDto.OilTypeName,
                            inputDto.CGST,
                            inputDto.SGST,
                            inputDto.IGST,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertShipToPartyError");
                _logger.Error(message);
            }
            return result;
        }

        public List<GSTUploadDto> InsertGSTNew(List<GSTUploadDto> gstList)
        {
            _methodName = "InsertGST";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            GSTUploadDto result = new GSTUploadDto();
            var resultMsg = new List<GSTUploadDto>();
            long parentId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        foreach (var inputDto in gstList)
                        {
                            //connection.Open();
                            result = connection.Query<GSTUploadDto>("SetGstNew", new
                            {
                                inputDto.SourceState,
                                inputDto.DestinationState,
                                inputDto.PlantName,
                                inputDto.OilTypeName,
                                inputDto.CGST,
                                inputDto.SGST,
                                inputDto.IGST,
                                inputDto.ValidFrom,
                                inputDto.ValidTo,
                                inputDto.CreatedBy,
                                parentId
                            }, commandType: CommandType.StoredProcedure).First();
                            if (parentId == 0)
                            {
                                if (result != null)
                                {
                                    parentId = result.ParentId;
                                }
                            }
                            resultMsg.Add(result);
                        }
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertShipToPartyError");
                _logger.Error(message);
            }
            return resultMsg;
        }

        public GSTUploadOldDto InsertGSTOld(GSTUploadOldDto inputDto)
        {
            _methodName = "InsertGST";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            GSTUploadOldDto result = new GSTUploadOldDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<GSTUploadOldDto>("SetGst", new
                        {
                            inputDto.SourceZone,
                            inputDto.SourceState,
                            inputDto.PlantName,
                            inputDto.DestinationZone,
                            inputDto.DestinationState,
                            inputDto.FreightZoneName,
                            inputDto.FreightRouteName,
                            inputDto.VerticalCode,
                            inputDto.OilTypeName,
                            inputDto.SkuCode,
                            inputDto.CGST,
                            inputDto.SGST,
                            inputDto.IGST,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertShipToPartyError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Final Price Excel Export

        public List<TPPricingExportDto> TpFinalPriceExportToList(long priceId, DateTime SearchDate)
        {
            _methodName = "TpFinalPriceExcelExport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<TPPricingExportDto> pricingList = new List<TPPricingExportDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "GetFinalPriceDataExport";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PriceId", priceId);
                    cmd.Parameters.AddWithValue("@SearchDate", SearchDate);
                    cmd.CommandTimeout = 0;
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        pricingList.Add(new TPPricingExportDto()
                        {
                            OilTypeName = rdr["OiltypeName"].ToString(),
                            OilPackingType = rdr["OilPackingType"].ToString(),
                            SaudaBookingType = rdr["SaudaBookingType"].ToString(),
                            SkuName = rdr["SkuName"].ToString(),
                            Plant = rdr["PlantName"].ToString(),
                            Depot = rdr["DepotName"].ToString(),
                            State = rdr["StateName"].ToString(),
                            FrieghtZone = rdr["FrieghtZone"].ToString(),
                            FrieghtRoute = rdr["FrieghtRoute"].ToString(),
                            TransportMode = rdr["TransportMode"].ToString(),
                            Loadability = Convert.ToDecimal(rdr["LoadQuantity"]),
                            MaterialCost = Convert.ToDecimal(rdr["MaterialCost"].ToString()),
                            PackingCost = Convert.ToDecimal(rdr["PackingCost"]),
                            PrimaryFrieght = Convert.ToDecimal(rdr["PrimaryFrieght"]),
                            SecondaryFrieght = Convert.ToDecimal(rdr["SecondaryFrieght"]),
                            PlantSecondaryFrieght = Convert.ToDecimal(rdr["PlantSecondaryFrieght"]),
                            DepotCost = Convert.ToDecimal(rdr["DepotCost"]),
                            DetentionCost = Convert.ToDecimal(rdr["DetentionCost"]),
                            HoneycombCost = Convert.ToDecimal(rdr["HoneycombCost"]),
                            Margin = Convert.ToDecimal(rdr["Margin"]),
                            CushionMargin = Convert.ToDecimal(rdr["CushionMargin"]),
                            SchemeCostRecovery = Convert.ToDecimal(rdr["SchemeCostRecovery"]),
                            ExPlantPrice = Convert.ToDecimal(rdr["ExPlantPrice"].ToString()),
                            ForDepotPrice = Convert.ToDecimal(rdr["ForDepotPrice"].ToString()),
                            ForPlantPrice = Convert.ToDecimal(rdr["ForPlantPrice"].ToString()),
                            ExDepotPrice = Convert.ToDecimal(rdr["ExDepotPrice"].ToString()),
                            ExRakePrice = Convert.ToDecimal(rdr["ExRakePrice"].ToString()),
                            ForRakePrice = Convert.ToDecimal(rdr["ForRakePrice"].ToString()),
                            AdditionalCost = Convert.ToDecimal(rdr["AdditionalCost"].ToString()),
                            OilTransferCost = Convert.ToDecimal(rdr["OilTransferCost"].ToString())
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return pricingList;
        }

        public List<TPPricingExportDto> TpFinalPriceExportForAllRecordsToList(long skip, long take)
        {
            _methodName = "TpFinalPriceExportForAllRecordsToList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<TPPricingExportDto> pricingList = new List<TPPricingExportDto>();
            //DataTable dataTable = new DataTable();
            //SqlDataReader rdr = null;
            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "GetFinalPriceDataExportForAllRecords";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Skip", skip);
                    cmd.Parameters.AddWithValue("@Take", take);
                    cmd.CommandTimeout = 0;
                    var rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {


                        pricingList.Add(new TPPricingExportDto()
                        {
                            OilTypeName = rdr["OiltypeName"].ToString(),
                            OilPackingType = rdr["OilPackingType"].ToString(),
                            SaudaBookingType = rdr["SaudaBookingType"].ToString(),
                            SkuName = rdr["SkuName"].ToString(),
                            Plant = rdr["PlantName"].ToString(),
                            Depot = rdr["DepotName"].ToString(),
                            State = rdr["StateName"].ToString(),
                            FrieghtZone = rdr["FrieghtZone"].ToString(),
                            FrieghtRoute = rdr["FrieghtRoute"].ToString(),
                            TransportMode = rdr["TransportMode"].ToString(),
                            Loadability = Convert.ToDecimal(rdr["LoadQuantity"]),
                            MaterialCost = Convert.ToDecimal(rdr["MaterialCost"].ToString()),
                            PackingCost = Convert.ToDecimal(rdr["PackingCost"]),
                            PrimaryFrieght = Convert.ToDecimal(rdr["PrimaryFrieght"]),
                            SecondaryFrieght = Convert.ToDecimal(rdr["SecondaryFrieght"]),
                            PlantSecondaryFrieght = Convert.ToDecimal(rdr["PlantSecondaryFrieght"]),
                            DepotCost = Convert.ToDecimal(rdr["DepotCost"]),
                            DetentionCost = Convert.ToDecimal(rdr["DetentionCost"]),
                            HoneycombCost = Convert.ToDecimal(rdr["HoneycombCost"]),
                            Margin = Convert.ToDecimal(rdr["Margin"]),
                            CushionMargin = Convert.ToDecimal(rdr["CushionMargin"]),
                            SchemeCostRecovery = Convert.ToDecimal(rdr["SchemeCostRecovery"]),
                            ExPlantPrice = Convert.ToDecimal(rdr["ExPlantPrice"].ToString()),
                            ForDepotPrice = Convert.ToDecimal(rdr["ForDepotPrice"].ToString()),
                            ForPlantPrice = Convert.ToDecimal(rdr["ForPlantPrice"].ToString()),
                            ExDepotPrice = Convert.ToDecimal(rdr["ExDepotPrice"].ToString()),
                            ExRakePrice = Convert.ToDecimal(rdr["ExRakePrice"].ToString()),
                            ForRakePrice = Convert.ToDecimal(rdr["ForRakePrice"].ToString())
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            //catch (Exception)
            //{
            //    return dataTable;
            //}
            //finally
            //{

            //    if (rdr != null)
            //    {
            //        rdr.Close();
            //    }
            //}
            return pricingList;
        }

        public DataTable TpFinalPriceExportToDatatable(long priceId)
        {
            _methodName = "TpFinalPriceExportDatatable";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var dataTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "GetFinalPriceDataExport";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PriceId", priceId);
                    cmd.CommandTimeout = 0;
                    IDataReader rdr = cmd.ExecuteReader();
                    dataTable.Load(rdr);
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return dataTable;
        }


        public List<GetCountBasedOnCurrentDateFromPricingsDto> GetTotalCountBasedOnCurrentDateFromPricings()
        {
            var result = new List<GetCountBasedOnCurrentDateFromPricingsDto>();
            try
            {
                using (SqlConnection query = new SqlConnection(connectionString))
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("Select Count(1) as CountOfRecords From Pricings  ");
                    sb.Append(" Where Convert(varchar, CreatedDate, 111) = Convert(varchar, GETDATE(), 111) and IsPublish = 1 ");

                    result = query.Query<GetCountBasedOnCurrentDateFromPricingsDto>(sb.ToString(), new { connectionString }).ToList();



                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return result;

        }
        #endregion


        #region Import CustomerGroupOne

        public CustomerGroupOneAndTwoUploadDto InsertCustomerGroupOne(CustomerGroupOneAndTwoUploadDto inputDto)
        {
            _methodName = "InsertCustomerGroupOne";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            CustomerGroupOneAndTwoUploadDto result = new CustomerGroupOneAndTwoUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsActive = inputDto.IsActive == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<CustomerGroupOneAndTwoUploadDto>("SetCustomerGroupOne", new
                        {
                            inputDto.LoginUserId,
                            inputDto.CustomerGroupName,
                            inputDto.CustomerGroupCode,
                            IsActive,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertCustomerGroupOneError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import CustomerGroupTwo

        public CustomerGroupOneAndTwoUploadDto InsertCustomerGroupTwo(CustomerGroupOneAndTwoUploadDto inputDto)
        {
            _methodName = "InsertCustomerGroupTwo";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            CustomerGroupOneAndTwoUploadDto result = new CustomerGroupOneAndTwoUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsActive = inputDto.IsActive == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<CustomerGroupOneAndTwoUploadDto>("SetCustomerGroupTwo", new
                        {
                            inputDto.LoginUserId,
                            inputDto.CustomerGroupName,
                            inputDto.CustomerGroupCode,
                            IsActive,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertCustomerGroupTwoError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import CustomerGroupFive

        public CustomerGroupFiveUploadDto InsertCustomerGroupFive(CustomerGroupFiveUploadDto inputDto)
        {
            _methodName = "InsertCustomerGroupTwo";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            CustomerGroupFiveUploadDto result = new CustomerGroupFiveUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsActive = inputDto.IsActive == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<CustomerGroupFiveUploadDto>("SetCustomerGroupFive", new
                        {
                            inputDto.LoginUserId,
                            inputDto.CustomerGroupName,
                            inputDto.CustomerGroupCode,
                            IsActive,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertCustomerGroupTwoError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import SalesOrganization

        public SalesOrganizationUploadDto InsertSalesOrganization(SalesOrganizationUploadDto inputDto)
        {
            _methodName = "InsertSalesOrganization";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            SalesOrganizationUploadDto result = new SalesOrganizationUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsActive = inputDto.IsActive == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<SalesOrganizationUploadDto>("SetSalesOrganization", new
                        {
                            inputDto.LoginUserId,
                            inputDto.Name,
                            inputDto.SAPCode,
                            IsActive,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertCustomerGroupTwoError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import DistributionChannel

        public DistributionChannelUploadDto InsertDistributionChannel(DistributionChannelUploadDto inputDto)
        {
            _methodName = "InsertDistributionChannel";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DistributionChannelUploadDto result = new DistributionChannelUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsActive = inputDto.IsActive == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<DistributionChannelUploadDto>("SetDistributionChannel", new
                        {
                            inputDto.SalesOrganizationCode,
                            inputDto.LoginUserId,
                            inputDto.Name,
                            inputDto.SAPCode,
                            IsActive,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertCustomerGroupTwoError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region Import Division

        public DivisionUploadDto InsertDivision(DivisionUploadDto inputDto)
        {
            _methodName = "InsertDivision";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DivisionUploadDto result = new DivisionUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsActive = inputDto.IsActive == "1" ? true : false;
                        var ZPR4 = inputDto.ZPR4 == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<DivisionUploadDto>("SetDivision", new
                        {
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            //inputDto.CCArea,
                            inputDto.LoginUserId,
                            inputDto.Name,
                            inputDto.Code,
                            IsActive,
                            inputDto.SalesDocumentType,
                            inputDto.SalesOrderDocumentType,
                            ZPR4

                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertCustomerGroupTwoError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region Import SaudaConversionUnitAndBaseRateDifference

        public SaudaConversionUnitAndDiffRateUploadDto InsertSaudaConversionUnitAndBaseRateDifference(SaudaConversionUnitAndDiffRateUploadDto inputDto)
        {
            _methodName = "InsertSaudaConversionUnitAndBaseRateDifference";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            SaudaConversionUnitAndDiffRateUploadDto result = new SaudaConversionUnitAndDiffRateUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsActive = inputDto.IsActive == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<SaudaConversionUnitAndDiffRateUploadDto>("SetSaudaConversionUnitAndBaseRateDifference", new
                        {
                            inputDto.OilType,
                            inputDto.PlantOrDepot,
                            inputDto.State,
                            inputDto.FromPackGroup,
                            inputDto.FromSku,
                            inputDto.FromUnit,
                            inputDto.ToPackGroup,
                            inputDto.ToSku,
                            inputDto.ToUnit,
                            inputDto.BasicRate,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            IsActive,
                            inputDto.CreatedBy,
                            inputDto.FromSkuCode,
                            inputDto.ToSkuCode
                        }, commandType: CommandType.StoredProcedure).First();

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSaudaConversionUnitAndBaseRateDifferenceError");
                _logger.Error(message);
            }
            return result;
        }
        #endregion


        #region Import CMSUsers
        public UserUploadDto InsertCMSUser(UserUploadDto inputDto)
        {
            _methodName = "InsertCMSUser";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            UserUploadDto result = new UserUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.Code))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            var IsActive = inputDto.IsActive == "1" ? true : false;
                            connection.Open();
                            result = connection.Query<UserUploadDto>("SetCMSUser", new
                            {
                                inputDto.Code,
                                inputDto.Name,
                                inputDto.MobileNumber,
                                inputDto.Email,
                                inputDto.ZoneName,
                                inputDto.StateName,
                                inputDto.TerritoryName,
                                inputDto.DistrictName,
                                inputDto.CityName,
                                inputDto.Pincode,
                                inputDto.Address,
                                inputDto.Designation,
                                inputDto.RoleName,
                                IsActive,
                                inputDto.CreatedBy,
                                inputDto.Headquarters,
                                inputDto.Password,
                                inputDto.CMSReportingToUserCode,
                                inputDto.EncryptedPassword
                            }, commandType: CommandType.StoredProcedure).First();
                        }

                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region UpdateDealerSaudaValidityAndSaudaLimit

        public DealerSaudaValidityUpdateDto UpdateDealerSaudaValidity(DealerSaudaValidityUpdateDto inputDto)
        {
            _methodName = "UpdateDealerSaudaValidity";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DealerSaudaValidityUpdateDto result = new DealerSaudaValidityUpdateDto();
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DealerSaudaValidityUpdateDto>("SP_UpdateDealerSaudaValidity", new
                        {
                            inputDto.DealerCode,
                            //inputDto.SaudaBookingType,
                            inputDto.SaudaValidityPeriod,
                            //inputDto.DivisionCode,
                            //inputDto.SalesOrganizationCode,
                            //inputDto.DistributionChannelCode,
                            inputDto.ModifiedBy,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_UpdateDealerSaudaValidityError");
                _logger.Error(message);
            }
            return result;
        }

        public DealerSaudaValidityUpdateDto UpdateDealerSaudaLimit(DealerSaudaValidityUpdateDto inputDto)
        {
            _methodName = "UpdateDealerSaudaLimit";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DealerSaudaValidityUpdateDto result = new DealerSaudaValidityUpdateDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DealerSaudaValidityUpdateDto>("SP_UpdateDealerSaudaLimit", new
                        {
                            inputDto.DealerCode,
                            inputDto.DivisionCode,
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            inputDto.ModifiedBy,
                            inputDto.SaudaLimit
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_UpdateDealerSaudaValidityError");
                _logger.Error(message);
            }
            return result;
        }


        public async Task<IList<DealerConsentImageUploadDto>> UploadDealerConsentImage(List<DealerConsentImageUploadDto> inputDto)
        {
            _methodName = "UploadDealerConsentImage";

            var apiUrl = ApiUrl.WebApiUrlPostUploadConsentImage;
            return await GetListAsync<DealerConsentImageUploadDto>(apiUrl, inputDto);
        }

        public DealerConsentImageUploadDto UploadBrokerCallRecordingDetails(DealerConsentImageUploadDto inputDto)
        {
            _methodName = "UploadBrokerCallRecordingDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DealerConsentImageUploadDto result = new DealerConsentImageUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var ActiveForCallToCustomers = inputDto.ActiveForCallToCustomers == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<DealerConsentImageUploadDto>("SP_UpdateBrokerCallRecordingDetails", new
                        {
                            inputDto.BrokerCode,
                            inputDto.SaudaBookingType,
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            inputDto.DivisionCode,
                            inputDto.ModifiedBy,
                            inputDto.AdditionalMobileNumber,
                            inputDto.ContactPersonName,
                            ActiveForCallToCustomers
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_UpdateDealerSaudaValidityError");
                _logger.Error(message);
            }
            return result;
        }
        #endregion

        #region Import MaterialType

        public MaterialTypeUploadDto InsertMaterialType(MaterialTypeUploadDto inputDto)
        {
            _methodName = "InsertMaterialType";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            MaterialTypeUploadDto result = new MaterialTypeUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<MaterialTypeUploadDto>("SetMaterialType", new
                        {
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            inputDto.DivisionCode,
                            inputDto.IsActive,
                            inputDto.MaterialType,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSkuError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Volume Loadability

        public VolumeLoadabilityUploadDto InsertVolumeLoadability(VolumeLoadabilityUploadDto inputDto)
        {
            _methodName = "InsertVolumeLoadability";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            VolumeLoadabilityUploadDto result = new VolumeLoadabilityUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<VolumeLoadabilityUploadDto>("SetVolumeLoadability", new
                        {
                            inputDto.SkuCode,
                            inputDto.PlantCode,
                            inputDto.MaxAllowableSingleSku,
                            inputDto.MaxAllowableMultipleSku,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.CreatedBy,
                            inputDto.IsActive,
                            inputDto.VehicleSize
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSkuError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region Volume Loadability

        public UserDivisionUploadDto InsertUserDivisionMapping(UserDivisionUploadDto inputDto)
        {
            _methodName = "InsertUserDivisionMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            UserDivisionUploadDto result = new UserDivisionUploadDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<UserDivisionUploadDto>("SetUserDivisionMapping", new
                        {
                            inputDto.UserCode,
                            inputDto.RoleName,
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            inputDto.DivisionCode,
                            inputDto.SaudaLimit,
                            inputDto.ContractValidityPeriodDays,  
                            inputDto.PlantDepot,
                            inputDto.CreatedBy,
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSkuError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region Import UserDiscount

        public UserDiscount UserDiscount(UserDiscount inputDto)
        {
            _methodName = "UserDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            UserDiscount result = new UserDiscount();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var spaceremoved = string.Join(",", inputDto.MaterialCode.Split(',')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList());

                        inputDto.MaterialCode = spaceremoved;
                        connection.Open();
                        result = connection.Query<UserDiscount>("SetUserDiscount", new
                        {
                            inputDto.LoginUserId,
                            inputDto.SalesOrganization,
                            inputDto.DistributionChannel,
                            inputDto.Division,
                            inputDto.Discount,
                            inputDto.DiscountReason,
                            inputDto.MaterialCode,
                            inputDto.EmployeeCode,
                            inputDto.ValidFrom,
                            inputDto.ValidTo,
                            inputDto.StateName
                        }, commandTimeout: 300, commandType: CommandType.StoredProcedure).First();
                    }

                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Import GeographyDiscount

        public GeographyDiscount GeographyDiscount(GeographyDiscount inputDto)
        {
            _methodName = "UserDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            GeographyDiscount result = new GeographyDiscount();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var spaceremoved = string.Join(",", inputDto.MaterialCode.Split(',')
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList());

                        inputDto.MaterialCode = spaceremoved;

                        connection.Open();
                        result = connection.Query<GeographyDiscount>("SetGeographyDiscount", new
                        {
                            inputDto.LoginUserId,
                            inputDto.SalesOrganization,
                            inputDto.DistributionChannel,
                            inputDto.Division,
                            inputDto.Discount,
                            inputDto.DiscountReason,
                            inputDto.MaterialCode,
                            inputDto.Zone,
                            inputDto.State,
                            inputDto.District,
                            inputDto.City,
                            inputDto.ValidFrom,
                            inputDto.ValidTo
                        }, commandTimeout: 0, commandType: CommandType.StoredProcedure).First();
                    }

                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region MyRegion

        public LineUploadDto InsertLine(LineUploadDto inputDto)
        {
            _methodName = "InsertLine";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            LineUploadDto result = new LineUploadDto();
            try
            {
                if (!string.IsNullOrEmpty(inputDto.LineName))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            bool isActive = inputDto.IsActive == "1";
                            connection.Open();
                            var distributorCodeList = inputDto.DistributorCode?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => new { Code = _ }).ToList();
                            var materialCodeList = inputDto.MaterialCode?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => new { SkuCode = _ }).ToList();

                            var distributorCode = JsonConvert.SerializeObject(distributorCodeList);
                            var materialCode = JsonConvert.SerializeObject(materialCodeList);

                            result = connection.Query<LineUploadDto>("SetLine", new
                            {
                                inputDto.LineName,
                                isActive,
                                inputDto.CreatedBy,
                                distributorCode,
                                materialCode
                            }, commandType: CommandType.StoredProcedure).First();
                        }

                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertUserError");
                _logger.Error(message);
            }

            result.MaterialCode = inputDto.MaterialCode;
            result.DistributorCode = inputDto.DistributorCode;

            return result;
        }

        #endregion

        #region QpsDiscount

        public List<QpsDiscountUploadDto> InsertQpsDiscount(DataTable qpsDatatable)
        {
            _methodName = "InsertQpsDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<QpsDiscountUploadDto> result = new List<QpsDiscountUploadDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "SetQpsDiscountUpload";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = cmd.Parameters.AddWithValue("@QpsDisUpload", qpsDatatable);
                    parameter.SqlDbType = SqlDbType.Structured;
                    cmd.CommandTimeout = 0;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                result.Add(new QpsDiscountUploadDto()
                                {
                                    StartDate = rdr["StartDate"].ToString(),
                                    EndDate = rdr["EndDate"].ToString(),
                                    SalesOrgCode = Convert.ToInt32(rdr["SalesOrgCode"].ToString()),
                                    DistributionChannelCode = Convert.ToInt32(rdr["DistributionChannelCode"].ToString()),
                                    DivisionCode = Convert.ToInt32(rdr["DivisionCode"].ToString().ToString()),
                                    //OilTypeId = rdr["OilTypeId"].ToString(),
                                    OilTypeName = rdr["OilTypeName"].ToString(),
                                    SkuCode = rdr["SkuCode"].ToString(),
                                    ZoneName = rdr["ZoneName"].ToString(),
                                    StateName = rdr["StateName"].ToString(),
                                    //SkuId = rdr["SkuId"].ToString(),
                                    //ZoneId = rdr["ZoneId"].ToString(),
                                    //StateId = rdr["StateId"].ToString(),
                                    SlabCount = Convert.ToInt32(rdr["SlabCount"].ToString()),
                                    FromRange = Convert.ToInt32(rdr["FromRange"].ToString()),
                                    ToRange = Convert.ToInt32(rdr["ToRange"].ToString()),
                                    Discount = Convert.ToDecimal(rdr["Discount"].ToString()),
                                    QpsParentId = Convert.ToInt32(rdr["QpsParentId"].ToString()),
                                    QpsRowId = Convert.ToInt32(rdr["QpsRowId"].ToString()),
                                    //CreatedBy = Convert.ToInt32(rdr["CreatedBy"].ToString()),
                                    //UpdateBy = Convert.ToInt32(rdr["UpdateBy"].ToString()),
                                    Message = rdr["Message"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return result;
        }

        #endregion

        #region PackGroupMapping

        public PackGroupTypeMapping InsertPackGroupMapping(PackGroupTypeMapping inputDto)
        {
            _methodName = "InsertPackGroupMapping";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            PackGroupTypeMapping result = new PackGroupTypeMapping();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PackGroupTypeMapping>("SetOilTypePackType", new
                        {
                            inputDto.SalesOrganizationCode,
                            inputDto.DistributionChannelCode,
                            inputDto.DivisionCode,
                            inputDto.SkuCode,
                            inputDto.PackGroupType,
                            inputDto.CreatedBy
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSkuError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region  GamificationDashboard

        public GamificationDashboardImportDto InsertGamificationDashboard(GamificationDashboardImportDto inputDto)
        {
            _methodName = "InsertGamificationDashboard";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            GamificationDashboardImportDto result = new GamificationDashboardImportDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var IsDiamond = inputDto.IsDiamond == "1" ? true : false;
                        connection.Open();
                        result = connection.Query<GamificationDashboardImportDto>("SetGamificationDashboard", new
                        {
                            inputDto.DistributorCode,
                            inputDto.DistributorTargetMT,
                            inputDto.DistributorAchievementTillN1MT,
                            inputDto.RemainingTargetToAchieveMT,
                            inputDto.EarnedPoints,
                            inputDto.CurrentSlab,
                            inputDto.NextHigherSlab,
                            inputDto.PointsToBeEarnedToReachNextHigherSlab,
                            inputDto.TotalEarningsInRs,
                            inputDto.SpecialBonusMessage,
                            //inputDto.WholePointsStructure,
                            //inputDto.IsActive,
                            IsDiamond
                        }, commandType: CommandType.StoredProcedure).First();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        result.PostStatus = false;
                        result.PostMessage = exception.Message;
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
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertGamificationDashboardError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region Geography
        public async Task<List<GeographyDiscount>> ImportGeographyDiscount(List<GeographyDiscountImportStatus> geographyDiscounts)
        {
            var result = new List<GeographyDiscount>();
            _methodName = "ImportGeographyDiscount";

            try
            {
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlImportgeographyDiscount, geographyDiscounts);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    if (jsonResponse != null)
                    {
                        result = JsonConvert.DeserializeObject<List<GeographyDiscount>>(ja[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.ForEach(_ => _.Message = ja[0]["message"].ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public async Task<List<GeographyDiscount>> GetGeographyDiscountStatus()
        {
            _methodName = "GetGeographyDiscountStatus";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<GeographyDiscount> result = new List<GeographyDiscount>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var query = "SELECT SalesOrganization,DistributionChannel,Division,MaterialCode,DiscountReason,Discount,ValidFrom,ValidTo,LoginUserId,Zone,State,District,City,ISNULL(Message, 'In Progress') AS Message,PackType,OilType as oiltype,PackGroup,IsActive FROM DiscountGeographyImportStatus";
                    result = connection.Query<GeographyDiscount>(query, commandTimeout: 0, commandType: CommandType.Text).ToList();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return await Task.FromResult(result);
        }

        #endregion

        #region Quantity Limit

        public  QuantityLimitDTO InsertQuantityLimit(QuantityLimitDTO inputDto)
        {
            _methodName = "InsertQuantityLimit";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            QuantityLimitDTO result = new QuantityLimitDTO();
            try
            {
                //if (!string.IsNullOrEmpty(inputDto.MaterialCode))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            result = connection.Query<QuantityLimitDTO>("usp_ImportQuantityLimit", new
                            {
                                inputDto.LoginUserId,
                                inputDto.SalesOrganizationCode,
                                inputDto.DistributionChannelCode,
                                inputDto.DivisionCode,
                                inputDto.OilTypeName,
                                inputDto.EmployeeCode,
                                inputDto.QuantityLimit,
                                inputDto.ValidFrom,
                                inputDto.ValidTo
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        catch (Exception exception)
                        {
                            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                            result.PostStatus = false;
                            result.PostMessage = exception.Message;
                            _logger.Error(message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_InsertSkuError");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region SaudaConditionalBooking Configuration

        public SaudaConditionalBookingConfigurationImportDto InsertAndUpdateSaudaConditionalBookingConfiguration(SaudaConditionalBookingConfigurationImportDto inputDto)
        {
            _methodName = "InsertAndUpdateSaudaConditionalBookingConfiguration";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            SaudaConditionalBookingConfigurationImportDto result = new SaudaConditionalBookingConfigurationImportDto();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var mantatotySkus = string.Empty;
                        var essentialSkus = string.Empty;
                        ValidateSaudaConditionalConfigurationData(ref inputDto,ref mantatotySkus,ref essentialSkus);

                        if (string.IsNullOrEmpty(inputDto.Message))
                        {
                            result = connection.Query<SaudaConditionalBookingConfigurationImportDto>("[dbo].[usp_AddOrUpdateCrossAndUpsellConfiguration]",
                                    new
                                    {
                                        Id = 0,
                                        SalesOrganizationCode = inputDto.SalesOrganizationCode,
                                        DistributionChannelCode = inputDto.DistributionChannelCode,
                                        DivisionCode = inputDto.DivisionCode,
                                        StateName = string.Join(",", inputDto.StateName),
                                        ZoneName = string.Join(",", inputDto.ZoneName),
                                        OilTypeName = inputDto.OilTypeName,
                                        PackGroup = inputDto.PackGroup,
                                        StartDate = Convert.ToDateTime(inputDto.StartDate),
                                        EndDate = Convert.ToDateTime(inputDto.EndDate),
                                        EssentialSkus = essentialSkus,
                                        MantatorySkus = mantatotySkus,
                                        LoginUserId = inputDto.LoginUserId,
                                    },
                                    commandType: CommandType.StoredProcedure,
                                    commandTimeout: 0
                                    ).FirstOrDefault();

                            if(result != null)
                            {
                                result.MandatorySkuCode = inputDto.MandatorySkuCode;
                                result.EssentialSkuCode = inputDto.EssentialSkuCode;
                                result.MandatorySkuPercentage = inputDto.MandatorySkuPercentage;
                            }
                        }
                        else
                        {
                            return inputDto;
                        }

                        return result;
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                        return inputDto;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                inputDto.Message = "Unexpected error occurred during validation.";
                return result;
            }
        }

        public SaudaConditionalBookingConfigurationImportDto ValidateSaudaConditionalConfigurationData(
           ref SaudaConditionalBookingConfigurationImportDto inputDto, ref string mantatotySkus,ref string essentialSkus)
        {
            try
            {
                var mandatorySkuList = inputDto.MandatorySkuCode?.Split(',', (char)StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).Distinct().ToList() ?? new List<string>();

                var essentialSkuList = inputDto.EssentialSkuCode?.Split(',', (char)StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).Distinct().ToList() ?? new List<string>();

                using (var con = new SqlConnection(connectionString))
                {
                    var duplicateSkus = mandatorySkuList.Intersect(essentialSkuList, StringComparer.OrdinalIgnoreCase).ToList();

                    if (duplicateSkus.Any())
                    {
                        inputDto.Message = "Failed, The Essential and Mandatory sku's should'n be same.";
                        return inputDto;
                    }

                    var allCodes = mandatorySkuList.Concat(essentialSkuList).Distinct().ToArray();

                    var foundSkus = con.Query<SkuDto>("SELECT s.* FROM Skus s JOIN SalesOrganizations so ON s.SalesOrganizationId = so.Id JOIN DistributionChannels d ON s.DistributionChannelId = d.Id  JOIN Divisions v ON s.DivisionId = v.Id  JOIN OilTypes o ON o.Name = @Oiltype AND o.Id = s.OilTypeId JOIN PackGroups p ON p.Name = @PackGroup AND s.PackGroupId = p.Id WHERE so.Code = @SalesOrganizationCode  AND d.Code = @DistributionChannelCode  AND v.Code = @DivisionCode AND s.SkuCode IN @SkuCode",
                        new 
                        {
                            Oiltype = inputDto.OilTypeName,
                            PackGroup = inputDto.PackGroup,
                            SalesOrganizationCode = inputDto.SalesOrganizationCode,
                            DistributionChannelCode = inputDto.DistributionChannelCode,
                            DivisionCode = inputDto.DivisionCode,
                            SkuCode = allCodes 
                        }, commandType: CommandType.Text).ToList();

                    var foundCodes = foundSkus.Select(s => s.SkuCode);

                    var invalidMandatorySkus = mandatorySkuList.Where(code => !foundCodes.Contains(code)).ToList();
                    var invalidEssentialSkus = essentialSkuList.Where(code => !foundCodes.Contains(code)).ToList();

                    if (invalidMandatorySkus.Any())
                    {
                        inputDto.Message = "Failed, Invalid Mandatory Sku Code(s): " + string.Join(", ", invalidMandatorySkus);
                        return inputDto;
                    }

                    if (invalidEssentialSkus.Any())
                    {
                        inputDto.Message = "Failed, Invalid Essential Sku Code(s): " + string.Join(", ", invalidEssentialSkus);
                        return inputDto;
                    }

                    if (!string.IsNullOrEmpty(inputDto.MandatorySkuPercentage))
                    {
                        var percentageList = inputDto.MandatorySkuPercentage
                            .Split(',', (char)StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => Convert.ToInt64(p.Trim())).ToList();

                        if (percentageList.Count != mandatorySkuList.Count)
                        {
                            inputDto.Message = "Failed, mismatch between Mandatory SKUs and their percentages.";
                            return inputDto;
                        }

                        var mandatoryMappingList = new List<SaudaConditionalBookingMandatorySkuMappingDto>();

                        for (int i = 0; i < mandatorySkuList.Count; i++)
                        {
                            if (percentageList[i] == 0)
                            {
                                inputDto.Message = $"Failed, Sku {mandatorySkuList[i]} percentage value should not be 0.";
                                return inputDto;
                            }
                            
                            var sku = foundSkus.FirstOrDefault(s => s.SkuCode == mandatorySkuList[i]);
                            if (sku != null)
                            {
                                mandatoryMappingList.Add(new SaudaConditionalBookingMandatorySkuMappingDto
                                {
                                    MandatorySkuCode = sku.SkuCode,
                                    MandatorySkuName = sku.SkuName,
                                    MandatorySkuId = sku.Id,
                                    MandatoryOilTypeId = sku.OilTypeId ?? 0,
                                    MandatoryPackGroupId= sku.PackTypeId,
                                    MandatoryBookingQuantityPercentage = percentageList[i]
                                });
                            }
                        }

                        mantatotySkus = JsonConvert.SerializeObject(mandatoryMappingList);
                        essentialSkus = string.Join(",", foundSkus.Where(_ => essentialSkuList.Contains(_.SkuCode)).Select(_ => _.Id).ToList());
                    }
                }

                return inputDto;
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex.Message}";
                _logger.Error(message);
                inputDto.Message = "Unexpected error occurred during validation.";
                return inputDto;
            }
        }

        #endregion
    }
}
