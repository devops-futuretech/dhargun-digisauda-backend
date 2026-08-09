using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Adani.Solution.DTO.Enums;

namespace Adani.Solution.Service
{
    public interface IImportService
    {
        ResultDto ImportAddresses(string decryptedString);

    }
    public class ImportService : IImportService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Import Service");
        private const string ServiceName = "Import Service";
        private string _methodName;
        public ImportService(IAdaniContext emamiContext)
        {
            try
            {
                _emamiContext = emamiContext;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Import Service", exception);
            }
        }

        #region ImportAddresses
        /// <summary>
        /// Save States list
        /// </summary>
        /// <param name="decryptedString"></param>
        /// <returns></returns>
        public ResultDto ImportAddresses(string decryptedString)
        {
            _methodName = "ImportAddresses";
            var messageSync = string.Empty;
            var addressDataSyncResultDto = new SapDataSyncResultDto();
            addressDataSyncResultDto.SyncStartedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
            var dataSynced = 0;
            var resultDto = new ResultDto();
            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            var AddressDtoList = JsonConvert.DeserializeObject<List<ImportAddressDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
            addressDataSyncResultDto.OutstandingResult.DataRetrieved = AddressDtoList.Count;
            try
            {
                var errorAddressList = new List<ImportAddressDto>();
                var userId = UtilityHelper.LongTryToParse(jarray[0]["loginUserId"].ToString());
                var errorMessageList = new List<string>();
                foreach (var addressDto in AddressDtoList)
                {
                    var errorFlag = true;
                    var errorMessage = addressDto.Id.ToString();
                    if (addressDto == null)
                    {
                        errorMessage = Constants.BindErrorMessage(Constants.InvalidRequest, errorMessage);
                        errorFlag = false;
                    }
                    if (errorFlag)
                    {
                        if (!string.IsNullOrEmpty(addressDto.CountryName))
                        {
                            var countryContext = _emamiContext.Country.AsNoTracking().FirstOrDefault(_ => _.Name.ToLower() == addressDto.CountryName.ToLower());
                            if (countryContext == null)
                            {
                                countryContext = new Data.Entities.Country();
                                countryContext.Name = addressDto.CountryName;
                                if (!string.IsNullOrEmpty(addressDto.CountryCode))
                                {
                                    countryContext.Code = addressDto.CountryCode;
                                }
                                if (!string.IsNullOrEmpty(addressDto.CurrencyName))
                                {
                                    countryContext.CurrencyName = addressDto.CurrencyName;
                                }
                                countryContext.IsActive = addressDto.CountryIsActive;
                                _emamiContext.Country.Add(countryContext);
                                _emamiContext.SaveChanges();
                            }
                            addressDto.CountryId = countryContext.Id;
                        }
                        if (!string.IsNullOrEmpty(addressDto.StateName))
                        {
                            var stateContext = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.StateName.ToLower() == addressDto.StateName.ToLower());
                            if (stateContext != null)
                            {
                                addressDto.StateId = stateContext.Id;
                            }
                            else if (stateContext == null && !string.IsNullOrEmpty(addressDto.CountryName))
                            {
                                stateContext = new State
                                {
                                    StateName = addressDto.StateName,
                                    CountryId = addressDto.CountryId,
                                    CreatedBy = userId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    IsActive = addressDto.StateIsActive,
                                };
                                _emamiContext.State.Add(stateContext);
                                _emamiContext.SaveChanges();
                                addressDto.StateId = stateContext.Id;
                            }
                            else if (!string.IsNullOrEmpty(addressDto.StateName) && string.IsNullOrEmpty(addressDto.CountryName))
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.CountryNameIsMissing + " - " + addressDto.StateName, errorMessage);
                                errorFlag = false;
                            }
                        }
                        if (!string.IsNullOrEmpty(addressDto.TerritoryName) && !string.IsNullOrEmpty(addressDto.StateName))
                        {
                            var territoryContext = _emamiContext.Territory.AsNoTracking().FirstOrDefault(_ => _.Name.ToLower() == addressDto.TerritoryName.ToLower() && _.StateId == addressDto.StateId);
                            if (territoryContext == null)
                            {
                                territoryContext = new Territory
                                {
                                    Name = addressDto.TerritoryName,
                                    StateId = addressDto.StateId,
                                    CreatedBy = userId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    IsActive = addressDto.TerritoryIsActive,
                                };
                                _emamiContext.Territory.Add(territoryContext);
                                _emamiContext.SaveChanges();
                            }
                            addressDto.TerritoryId = territoryContext.Id;
                        }
                        else if (!string.IsNullOrEmpty(addressDto.TerritoryName) && string.IsNullOrEmpty(addressDto.StateName))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.StateNameIsMissing + " - " + addressDto.TerritoryName, errorMessage);
                            errorFlag = false;
                        }
                        if (!string.IsNullOrEmpty(addressDto.DistrictName) && !string.IsNullOrEmpty(addressDto.TerritoryName) && !string.IsNullOrEmpty(addressDto.StateName))
                        {
                            var districtContext = _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.DistrictName.ToLower() == addressDto.DistrictName.ToLower()
                                                    /*&& _.TerritoryId == addressDto.TerritoryId*/ && _.StateId == addressDto.StateId);
                            if (districtContext == null)
                            {
                                districtContext = new District
                                {
                                    DistrictName = addressDto.DistrictName,
                                    StateId = addressDto.StateId,
                                  //  TerritoryId = addressDto.TerritoryId,
                                    CreatedBy = userId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    IsActive = addressDto.DistrictIsActive,
                                };
                                _emamiContext.District.Add(districtContext);
                                _emamiContext.SaveChanges();
                            }
                            addressDto.DistrictId = districtContext.Id;
                        }
                        else if (!string.IsNullOrEmpty(addressDto.DistrictName) && (string.IsNullOrEmpty(addressDto.TerritoryName) || string.IsNullOrEmpty(addressDto.StateName)))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.TerritoryStateNameMissing + " - " + addressDto.DistrictName, errorMessage);
                            errorFlag = false;
                        }
                        if (!string.IsNullOrEmpty(addressDto.CityName) && !string.IsNullOrEmpty(addressDto.DistrictName) && !string.IsNullOrEmpty(addressDto.TerritoryName) && !string.IsNullOrEmpty(addressDto.StateName))
                        {
                            var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.CityName.ToLower() == addressDto.CityName.ToLower()
                                                && _.DistrictId == addressDto.DistrictId/* && _.TerritoryId == addressDto.TerritoryId*/);
                            if (cityContext == null)
                            {
                                cityContext = new City
                                {
                                    CityName = addressDto.CityName,
                                    DistrictId = addressDto.DistrictId,
                                    //TerritoryId = addressDto.TerritoryId,
                                    CreatedBy = userId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    IsActive = addressDto.CityIsActive,
                                };
                                _emamiContext.City.Add(cityContext);
                                _emamiContext.SaveChanges();
                            }
                        }
                        else if (!string.IsNullOrEmpty(addressDto.CityName) && (string.IsNullOrEmpty(addressDto.DistrictName) || string.IsNullOrEmpty(addressDto.TerritoryName) || string.IsNullOrEmpty(addressDto.StateName)))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.DistrictTerritoryStateNameMissing + " - " + addressDto.CityName, errorMessage);
                            errorFlag = false;
                        }
                    }

                    if (errorFlag)
                    {
                        dataSynced++;
                    }
                    else
                    {
                        errorMessageList.Add(errorMessage);
                    }
                }
                addressDataSyncResultDto.SyncCompletedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (errorMessageList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = addressDataSyncResultDto;
                    resultDto.ErrorDto.Message = string.Join(",", errorMessageList);
                }
                else
                {
                    addressDataSyncResultDto.OutstandingResult.DataSynced = dataSynced;
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = addressDataSyncResultDto;
                    resultDto.SuccessDto.Message = Constants.SyncSuccessMessage;
                }
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                messageSync = message;
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Response = addressDataSyncResultDto;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = messageSync;
                _logger.Error(message);
                return resultDto;
            }
        }
        #endregion
    }
}
