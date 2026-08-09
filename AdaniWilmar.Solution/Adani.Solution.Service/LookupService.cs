using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using GMCore.Logger;
using GMCore.Helper;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Adani.Solution.DTO.Enums;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Adani.Solution.Service
{
    public interface ILookupService
    {
        ResultDto GetStateList();
        ResultDto GetActiveStateList();
        ResultDto GetActiveUserState(IdInputDto input);
        ResultDto GetIncoTermList(LoginUserIdDto loginUserIdDto);
        ResultDto GetOilPackingTypeList();
        ResultDto GetOilPackingGroupTypeList();
        ResultDto GetDistricts(DistrictInputDto districtInputDto);
        ResultDto GetStateListByEmployees(LoginUserIdDto inputDto);

        #region Dropdown Details
        //ResultDto GetIngredientCostddl(LoginUserIdDto loginUserIdDto);
        #endregion
        ResultDto GetCityList();
        ResultDto GetDistrictListByStateId(int stateId);
        ResultDto GetCityListByDistrictIdForDropdown(int districtId);
        ResultDto GetCityListByDistrictId(int districtId);
        ResultDto GetCityListByStateId(int stateId);

        ResultDto GetOilTypesBasedOnVerticalId(IdInputDto inputDto);
        ResultDto GetSkusBasedOnOilTypeId(IdInputDto inputDto);

        ResultDto GetUomList();
        ResultDto GetOilTypesBasedOnVerticle(OilTypeInputDto oilTypeInput);
        ResultDto GetSkusBasedOnOilType(SkuInputDto skuInputDto);
        ResultDto GetSkusUnitBasedOnOilType(SkuInputDto skuInputDto);

        ResultDto GetDealerAndBrokerDetails(ReportingUsersInputDto inputDto);
        ResultDto GetDealerDetails(LoginDealerIdDto loginDealerIdDto);
        ResultDto GetDealersBasedOnState(ReportingUsersInputDto inputDto);
        ResultDto GetCustomerOnCity(List<int> x);

        #region Users

        ResultDto GetUsersByRoleIdddl(IdInputDto inputDto);

        #endregion
        ResultDto GetUnMappedDistrictListByStateId(int stateId);

        ResultDto GetPackGroupListBySkuId(IdInputDto inputDto);
        ResultDto GetSubCategoryListddl();
        ResultDto GetConfigurationList();
        ResultDto UpdateConfiguration(List<ConfigurationDto> configurationDtoList);
        ResultDto GetSkuListByPackGroupId(SkuDropDownInputDto inputDto);

        //Competitor


        ResultDto GetCompetitorList(LoginUserIdDto loginUserIdDto);
        ResultDto SaveCompititor(CompetitorDto competitorDto);
        ResultDto UpdateCompititors(CompetitorDto competitorDto);
        ResultDto GetCompititorById(string competitorId);
        ResultDto GetCompititors(LoginUserIdDto loginUserIdDto);
        ResultDto GetSkuBasedOnOilTypes(CompetitorSkuInputDto inputDto);
        ResultDto ExportCompetitor(LoginUserIdDto loginUserIdDto);
        ResultDto GetCompetitorListWithPagination(KendoGridResult inputDto);

        //Key Performance indicator
        ResultDto AddKeyPerformance(KeyPerformanceDto inputDto);
        ResultDto AddDateRange(DateRangeDTO inputDto);

        ResultDto UpdateKeyPerformance(KeyPerformanceDto inputDto);
        ResultDto GetKeyPerformanceById(IdInputDto inputDto);
        ResultDto GetKeyPerformanceList(LoginUserIdDto inputDto);

        ResultDto GetDealerDetailsByVertical(DealerBrokerParamDto inputDto);
        ResultDto GetDealerAndBrokerListForBDO(ReportingUsersInputDto inputDto);

        //Sku Ingredient OilTypes
        ResultDto GetSkuIngredienOilTypes(IdInputDto inputDto);
        ResultDto MaterialCostOilTypesBasedOnVerticalId(IdInputDto inputDto);

        ResultDto GetSkuBasedOnOilTypeSubCategoryForDropdown(SkuDropDownInputDto inputDto);
        //ResultDto GetOilTypeIsRasoiOrNot(IdInputDto inputDto);

        ResultDto GetOilTypesByVerticalId(IdInputDto inputDto);
        ResultDto GetOilTypesById(string inputDto);
        ResultDto GetVerticalsById(string inputDto);
        ResultDto GetShipToPartyListBasedOnVertical(DealerBrokerParamDto inputDto);

        ResultDto GetPlantDepotRakeByStateId(IdInputDto inputDto);
        //ResultDto GetFreightZoneByStateId(IdInputDto inputDto);

        //ResultDto GetCustomerGroupOne();
        ResultDto GetCustomerGroupFive();
        ResultDto GetSalesOrganization();
        //ResultDto GetCustomerGroupTwo();

        ResultDto GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown(SkuDropDownInputDto inputDto);

        ResultDto GetStatesBasedOnCustomerGroupId(IdInputDto inputDto);

        ResultDto GetOilTypeListByVerticalIdsForDropDown(IdInputDto inputDto);
        ResultDto GetOilPackingTypeListForDropdown();
        ResultDto GetVerticalListForDropdown(LoginUserIdDto inputDto);
        ResultDto GetSkuListByOilTypeIdsPackGroupIdsForDropdown(DropDownInputDto inputDto);
        ResultDto GetZonalHeadList();
        ResultDto GetZonalHeadListNew(LoginUserIdDto inputDto);
        ResultDto GetSkuListData(FinalPriceSkuInputDto skuId);
        ResultDto GetBDOBasedOnZonalHead(List<long> zonalheadId);
        ResultDto GetZonalHeadBasedNH(long NHId);
        ResultDto GetZonalHeadBasedNHComb(BookedSaudaInputDto NHId);
        ResultDto GetSkuBasedOnOilTypeSubCategoryForMobile(SkuDropDownInputDto inputDto);

        //Notification
        ResultDto GetBdoddlList(LoginUserIdDto inputDto);
        ResultDto GetDealerListBasedOnBDO(NotificationInputDto inputDto);
        ResultDto AddNotification(NotificationsDto inputDto);
        ResultDto GetTPNotificationList(LoginUserIdDto inputDto);
        ResultDto GetTPNotificationDetailsById(long tpNotificationId);
        ResultDto GetTPNotificationById(IdInputDto inputDto);
        ResultDto GetMappedDealerListByTPNotificationId(NotificationGridInputDto inputDto);
        ResultDto UpdateTPNotification(NotificationsDto inputDto);
        ResultDto ExportTPNotificationList(LoginUserIdDto inputDto);
        ResultDto GetActiveStateListBasedOnZonalHeadIds(List<long> zonalHeadIds);
        ResultDto SendNotification(SmsInputDto inputDto);
        ResultDto GetActiveStateCityList();
        ResultDto SendSmsNotification(NotificationsSmsSendInputDto inputDto);
        ResultDto GetSaudaConversionList();
        ResultDto UpdateSaudaConversionType(List<SaudaConversionTypeDto> inputDto);
        ResultDto GetActiveOilTypeList(LoginUserIdDto inputDto);
        ResultDto AddSaudaExtensionPolicy(SaudaExtensionPolicyAddDto inputDto);
        ResultDto GetSaudaExtensionList(long verticalId);
        ResultDto GetRemarksGroup(IdInputDto inputDto);
        ResultDto AddDeleteListRemarks(AddDeleteListRemarks inputDto);
        ResultDto GetDealersDetailsList(FreightZoneAndRouteDropDownInputDto inputDto);
        ResultDto CheckPermissionForVertical(LoginUserIdDto inputDto);
        ResultDto SaveConfigurationforSaudaValidityAndSaudaReportMails(SaudaValidityAndSaudaReportMailConfigurationDto inputDto);
        ResultDto GetVerticalListBasedOnSaudaValidity();
        ResultDto GetVerticalListAndMailIds(long verticalId);
        ResultDto GetSkusBasedOnEmployeeDiscount(SkuInputDto skuInputDto);
        ResultDto GetDealerBasedOnBdo(List<long> bdoIds);
        ResultDto GetDealerCodeBasedOnBdo(List<long> bdoIds);
        ResultDto GetZHBasedOnVertical(LoginUserIdDto inputDto);
        ResultDto GetOilTypesBasedOnVertical(IdInputDto inputDto);

        ResultDto GetZonalHeadListByNH(NationalHeadDto inputDto);
        ResultDto GetNationalHeadUserList(LoginUserIdDto inputDto);
        ResultDto GetOilPackingTypeListWithAll();
        ResultDto GetDistributionChannel(IdInputDto id);
        ResultDto GetPlantBasedOnStateId(IdInputDto inputDto);
        ResultDto GetDateRange(long DealerId);
        ResultDto GetSkuBasedOnCombination(LoginUserIdDto inputDto);
        ResultDto SaudaBookingConfiguration(SaudaBookingConfigurationDto inputDto);
        ResultDto SaudaBookingConfigurationForMobile(SaudaBookingConfigurationDto inputDto);
        ResultDto GetSaudaBookingConfigurationDetails(string EncryptedId);
        ResultDto SaudaBookingConfigurationRolewise(UserInputDto inputDto);

        ResultDto GetSkuDataWithLiftingandDoNumber(LiftingSkuInputDto inputDto);

        //GamificationDashboard
        ResultDto GetGamificationDashboardList(string inputDto);
        ResultDto GetGamificationDashboardWithPagination(KendoGridResult inputDto);
        ResultDto AddOrUpdateGamificationDashboardDetails(GamificationDashboardDto gamificationDashboardDto);
        ResultDto SaudaSalesAreaRestrictionConfiguration(SaudaSalesAreaRestrictionDto inputDto);
        ResultDto GetSaudaSalesAreaRestrictionConfigurationDetails(string EncryptedId);

    }

    public class LookupService : ILookupService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Lookup Service");
        private const string ServiceName = "Lookup Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public LookupService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
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
                _logger.Error("Error instantiating dependencies for Lookup Service", exception);
            }
        }

        #region Lookup

        /// <summary>
        /// Method to Get State List
        /// </summary>
        /// <returns></returns>
        public ResultDto GetStateList()
        {
            _methodName = "GetStateList";
            var resultDto = new ResultDto();
            var stateDto = new List<StateDto>();
            try
            {
                stateDto = _emamiContext.State.AsNoTracking().Select(_ => new StateDto { StateId = _.Id, StateName = _.StateName }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get State List by Employee Ids
        /// </summary>
        /// <returns></returns>
        public ResultDto GetStateListByEmployees(LoginUserIdDto inputDto)
        {
            _methodName = "GetStateListByEmployees";
            var resultDto = new ResultDto();
            var stateDto = new List<StateDto>();
            try
            {
                var stateTraderIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                    .Where(_ => inputDto.ZonalHeadIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId);

                var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                    .Where(_ => stateTraderIds.Contains(_.UserId)).Select(_ => _.CustomerId);

                var allEmployees = stateTraderIds
                    .Union(dealerIds)
                    .Distinct()
                    .ToList();
                stateDto = _emamiContext.Users.AsNoTracking().Where(_ => allEmployees.Contains(_.Id) && _.StateId != 0)
                    .Join(_emamiContext.State.AsNoTracking(), u => u.StateId, s => s.Id, (u, s) => new { state = s })
                    .Select(_ => new StateDto { StateId = _.state.Id, StateName = _.state.StateName }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetActiveStateList()
        {
            _methodName = "GetActiveStateList";
            var resultDto = new ResultDto();
            var stateDto = new List<StateDto>();
            try
            {
                stateDto = _emamiContext.State.AsNoTracking().Where(_ => _.IsActive).Select(_ => new StateDto { StateId = _.Id, StateName = _.StateName }).Distinct().ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        public ResultDto GetActiveUserState(IdInputDto input)
        {
            _methodName = "GetActiveUserState";
            var resultDto = new ResultDto();
            var stateDto = new StateOutputDto();
            try
            {
                stateDto = _emamiContext.Users.AsNoTracking()
                    .Join(_emamiContext.State.AsNoTracking(), u => u.StateId, st => st.Id, (u, st) => new { u, st })
                    .Where(_ => _.u.Id == input.LoginUserId).Select(s => new StateOutputDto()
                    {
                        LoginUserId = s.u.Id,
                        StateId = s.u.StateId,
                        StateName = s.st.StateName
                    }).FirstOrDefault();
                //stateDto = _emamiContext.State.AsNoTracking().Where(_ => _.IsActive).Select(_ => new StateDto { StateId = _.Id, StateName = _.StateName }).Distinct().ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }


        public ResultDto GetActiveStateListBasedOnZonalHeadIds(List<long> zonalHeadIds)
        {
            _methodName = "GetActiveStateListBasedOnZonalHeadIds";
            var resultDto = new ResultDto();
            var stateDto = new List<DropDownDto>();
            try
            {
                if (zonalHeadIds == null && !zonalHeadIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                var bdoIds = _emamiContext.Users.AsNoTracking().Where(_ => zonalHeadIds.Contains((long)_.ReportingToId)).Select(s => s.Id).ToList();
                var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(s => s.CustomerId).ToList();
                var stateIds = _emamiContext.Users.AsNoTracking().Where(_ => dealerIds.Contains(_.Id)).Select(s => s.StateId).ToList();
                //var StateIds = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserCustomerMapping.AsNoTracking(), a => a.Id, b => b.UserId, (a, b) => new { a, b })
                //    .Join(_emamiContext.Users.AsNoTracking(), c => c.b.CustomerId, dealer => dealer.Id, (c, dealer) => new { c, dealer }).Where(
                //    _ => zonalHeadIds.Contains(_.c.a.ReportingToId.Value) && _.c.a.IsActive && _.dealer.IsActive).Select(x => x.dealer.StateId).Distinct().ToList();
                stateDto = _emamiContext.State.AsNoTracking().Where(_ => stateIds.Contains(_.Id) && _.IsActive).Select(_ => new DropDownDto { Id = _.Id, Name = _.StateName }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetActiveStateCityList()
        {
            _methodName = "GetActiveStateCityList";
            var resultDto = new ResultDto();
            var stateDto = new List<StateDto>();
            try
            {
                stateDto = _emamiContext.City.AsNoTracking().Where(_ => _.IsActive &&
                                                                        _.District != null && _.District.IsActive &&
                                                                        _.District.State != null && _.District.State.IsActive)
                                                                    .Distinct()
                                                                    .GroupBy(_ => _.District.StateId)
                                                                    .Select(group => new StateDto
                                                                    {
                                                                        StateId = group.Key,
                                                                        StateName = group.FirstOrDefault().District.State.StateName,
                                                                        Cities = group.ToList().Select(c => new CityDto { CityId = c.Id, CityName = c.CityName }).ToList()
                                                                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetActiveOilTypeList(LoginUserIdDto inputDto)
        {
            _methodName = "GetActiveOilTypeList";
            var resultDto = new ResultDto();
            var oilTypeDto = new List<OilTypeDto>();
            try
            {
                var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (inputDto.LoginUserId > 0 && userrole != null && userrole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
            .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                    oilTypeDto = (from ud in _emamiContext.OilTypes.AsNoTracking()
                                  join lud in divisionslogieduser on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                equals new { SalesOrganizationId = lud.SalesOrganizationId, DistributionChannelId = lud.DistributionChannelId, DivisionId = lud.DivisionId }
                                  select new OilTypeDto()
                                  {
                                      Id = ud.Id,
                                      Name = ud.Name + "-" + ud.SalesOrganization.Code + "/" + ud.DistributionChannel.Code + "/" + ud.Division.Code
                                  }
                                  ).Distinct().ToList();


                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = oilTypeDto;
                }
                else
                {
                    oilTypeDto = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.IsActive)
                    .Select(_ => new OilTypeDto { Id = _.Id, Name = _.Name + "-" + _.SalesOrganization.Code + "/" + _.DistributionChannel.Code + "/" + _.Division.Code /*+" - "+_.SAPCode*/ }).Distinct().ToList();
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = oilTypeDto;
                }

                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get State List
        /// </summary>
        /// <returns></returns>
        public ResultDto GetIncoTermList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetStateList";
            var resultDto = new ResultDto();
            var incoTermDto = new List<DropDownDto>();
            try
            {
                incoTermDto = _emamiContext.IncoTerms.AsNoTracking().Where(_ => _.IsActive).Select(_ => new DropDownDto { Id = _.Id, Name = _.Name }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = incoTermDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetDistricts(DistrictInputDto districtInputDto)
        {
            _methodName = "GetDistricts";
            var districtList = new List<DistrictDto>();
            var resultDto = new ResultDto();
            try
            {
                var districtContext = new List<Data.Entities.District>();
                var districtContextList = _emamiContext.District.AsNoTracking().Where(_ => _.IsActive).OrderBy(_ => _.SortOrder).AsQueryable();
                if (districtInputDto.StateId > 0)
                {
                    districtContext = districtContextList.Where(_ => _.StateId == districtInputDto.StateId).OrderBy(_ => _.SortOrder).ToList();
                }
                else
                {
                    districtContext = districtContextList.ToList();
                }
                if (!districtContext.Any())
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = districtList;
                    return resultDto;
                }
                foreach (var district in districtContext)
                {
                    var districtDto = new DistrictDto
                    {
                        DistrictId = district.Id,
                        DistrictName = district.DistrictName,
                    };
                    districtList.Add(districtDto);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = districtList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get State List
        /// </summary>
        /// <returns></returns>
        public ResultDto GetOilPackingTypeList()
        {
            _methodName = "GetOilPackingTypeList";
            var resultDto = new ResultDto();
            var oilPackingTypeDto = new List<OilPackingTypeDto>();
            try
            {
                oilPackingTypeDto = _emamiContext.OilPackingTypes.AsNoTracking().Select(_ => new OilPackingTypeDto { Id = _.Id, Name = _.Name }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = oilPackingTypeDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get Oil Packing Group Type List
        /// </summary>
        /// <returns></returns>
        public ResultDto GetOilPackingGroupTypeList()
        {
            _methodName = "GetOilPackingGroupTypeList";
            var resultDto = new ResultDto();
            var oilPackingTypeDto = new List<OilPackingTypeDto>();
            try
            {
                var enumValues = Enum.GetValues(typeof(DTO.Enums.BpCpType));

                foreach (var value in enumValues)
                {
                    oilPackingTypeDto.Add(new OilPackingTypeDto
                    {
                        Id = (int)value,
                        Name = value.ToString()
                    });
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = oilPackingTypeDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get City List
        /// </summary>
        /// <returns></returns>
        public ResultDto GetCityList()
        {
            _methodName = "GetCityList";
            var resultDto = new ResultDto();
            var stateDto = new List<CityDto>();
            try
            {
                stateDto = _emamiContext.City.AsNoTracking().Select(_ => new CityDto { CityId = _.Id, CityName = _.CityName }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get District List By StateId
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public ResultDto GetDistrictListByStateId(int stateId)
        {
            _methodName = "GetDistrictListByStateId";
            var resultDto = new ResultDto();
            var stateDto = new List<DistrictDto>();
            try
            {
                var stateContext = _emamiContext.District.AsNoTracking().Where(_ => _.IsActive);
                if (stateId > 0)
                {
                    stateContext = stateContext.Where(_ => _.StateId == stateId);
                }
                stateDto = stateContext.Select(_ => new DistrictDto { DistrictId = _.Id, DistrictName = _.DistrictName }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get City List By district name
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public ResultDto GetCityListByDistrictIdForDropdown(int districtId)
        {
            _methodName = "GetCityListByDistrictIdForDropdown";
            var resultDto = new ResultDto();
            var cityDto = new List<DropDownDto>();
            try
            {
                var cityContext = _emamiContext.City.AsNoTracking().Where(_ => _.IsActive);
                if (districtId > 0)
                {
                    cityContext = cityContext.Where(_ => _.DistrictId == districtId);
                }
                cityDto = cityContext.Select(_ => new DropDownDto { Id = _.Id, Name = _.CityName }).Distinct().ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get City List By district name
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public ResultDto GetCityListByStateId(int stateId)
        {
            _methodName = "GetCityListByDistrictIdForDropdown";
            var resultDto = new ResultDto();
            var cityDto = new List<CityDto>();
            try
            {

                //var districtIds = _emamiContext.District.Where(d => d.StateId == stateId).GroupBy(d => d.DistrictName.Trim().ToLower()).Select(g => g.FirstOrDefault().Id).Distinct().ToList();
                var districtIds = _emamiContext.District.Where(d => d.StateId == stateId).GroupBy(d => d.DistrictName.ToLower().Replace(" ", "")).Select(g => g.FirstOrDefault().Id).ToList();

                var cityContext = _emamiContext.City.AsNoTracking().Where(c => c.IsActive && districtIds.Contains(c.DistrictId));

                var uniqueDistrictNames = _emamiContext.District.Where(d => d.StateId == stateId).Select(d => new { NormalizedName = d.DistrictName.ToLower().Replace(" ", "") }).Distinct().Select(d => d.NormalizedName).ToList();

                cityDto = cityContext.Select(c => new CityDto { CityId = c.Id, CityName = c.CityName }).Distinct().ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetCityListByDistrictId(int districtId)
        {
            _methodName = "GetCityListByDistrictId";
            var resultDto = new ResultDto();
            var cityDto = new List<CityDto>();
            try
            {
                cityDto = _emamiContext.City.AsNoTracking().Where(_ => _.DistrictId == districtId && _.IsActive)
                    .Select(_ => new CityDto { CityId = _.Id, CityName = _.CityName }).Distinct().ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get Uom List
        /// </summary>
        /// <returns></returns>
        public ResultDto GetUomList()
        {
            _methodName = "GetUomList";
            var resultDto = new ResultDto();
            var uomDtos = new List<UomDto>();
            try
            {
                uomDtos = _emamiContext.Uom.AsNoTracking().Where(_ => _.IsActive).Select(_ => new UomDto { Id = _.Id, Name = _.SAPName, IsQuantityType = _.IsQuantityType }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = uomDtos;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Private Methods


        #endregion

        #region Dropdown Details

        //public ResultDto GetIngredientCostddl(LoginUserIdDto loginUserIdDto)
        //{
        //    _methodName = "GetIngredientCostddl";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        IQueryable<Ingredient> ingredents;
        //        if (loginUserIdDto.IsToReturnInactiveData)
        //        {
        //            ingredents = _emamiContext.Ingredient.AsNoTracking()
        //          .Where(w => w.Isactive);
        //        }
        //        else
        //        {
        //            ingredents = _emamiContext.Ingredient.AsNoTracking();
        //        }

        //        var ingredientList = ingredents.Where(_ => _.DivisionId == loginUserIdDto.VerticalId)
        //            .Select(s => new IngredientDownDto()
        //            {
        //                IngredientId = s.Id,
        //                IngredientName = s.Name
        //            }).ToList();

        //        resultDto.SuccessDto.Response = ingredientList;
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

        public ResultDto GetOilTypesBasedOnVerticalId(IdInputDto inputDto)
        {
            _methodName = "GetOilTypesBasedOnVerticalId";
            var resultDto = new ResultDto();
            var oiltypeList = new List<DropDownDto>();
            try
            {
                oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => w.DivisionId == inputDto.Id && w.IsActive)
               .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();

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

        public ResultDto GetOilTypesBasedOnVertical(IdInputDto inputDto)
        {
            _methodName = "GetOilTypesBasedOnVertical";
            var resultDto = new ResultDto();
            var oiltypeList = new List<DropDownDto>();
            try
            {
                if (inputDto.VerticalId > 0)
                {
                    oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => w.DivisionId == inputDto.VerticalId && w.IsActive)
                   .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                }
                else
                {
                    oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => w.IsActive)
                   .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                }

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

        public ResultDto GetStatesBasedOnCustomerGroupId(IdInputDto inputDto)
        {
            _methodName = "GetStatesBasedOnCustomerGroupId";
            var resultDto = new ResultDto();
            var stateList = new List<DropDownDto>();
            var customerIds = new List<long>();
            var stateIds = new List<long>();

            try
            {

                var customerGroupContext = _emamiContext.CustomerGroupDetails.AsNoTracking().Where(w => w.CustomerGroupId == inputDto.Id).ToList();
                if (customerGroupContext != null && customerGroupContext.Any())
                {
                    foreach (var Ids in customerGroupContext)
                    {
                        customerIds.Add(Ids.CustomerId);
                    }

                    var userContext = _emamiContext.Users.AsNoTracking().Where(_ => customerIds.Contains(_.Id)).ToList();
                    if (userContext != null && userContext.Any())
                    {
                        foreach (var Ids in userContext)
                        {
                            stateIds.Add(Ids.StateId);
                        }

                        stateList = _emamiContext.State.AsNoTracking().Where(_ => stateIds.Contains(_.Id) && _.IsActive).Select(s => new DropDownDto()
                        {
                            Id = s.Id,
                            Name = s.StateName
                        }).ToList();
                    }

                }
                resultDto.SuccessDto.Response = stateList;
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

        public ResultDto GetSkusBasedOnOilTypeId(IdInputDto inputDto)
        {
            _methodName = "GetSkusBasedOnOilTypeId";
            var resultDto = new ResultDto();
            try
            {
                var skuResult = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == inputDto.Id && w.IsActive);

                var skuList = skuResult.Select(s => new DropDownDto()
                {
                    Id = s.Id,
                    Name = s.SkuCode + " - " + s.SkuName

                }).ToList();

                resultDto.SuccessDto.Response = skuList;
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

        public ResultDto GetPackGroupListBySkuId(IdInputDto inputDto)
        {
            _methodName = "GetPackGroupListBySkuId";
            var resultDto = new ResultDto();
            try
            {
                var skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.Id == inputDto.Id && w.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.PackGroup != null ? s.PackGroup.Name : string.Empty
                    }).ToList();

                resultDto.SuccessDto.Response = skuList;
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

        public ResultDto GetCustomerOnCity(List<int> cityIds)
        {
            _methodName = "GetCustomerOnCity";
            var resultDto = new ResultDto();
            try
            {
                var resultList = _emamiContext.Users.AsNoTracking().Where(w => cityIds.Contains(w.CityId) && w.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

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

        #region Verticle Oiltype Sku

        public ResultDto GetOilTypesBasedOnVerticle(OilTypeInputDto oilTypeInput)
        {
            _methodName = "GetOilTypesBasedOnVerticleId";
            var resultDto = new ResultDto();
            try
            {
                var oiltypeList = _emamiContext.OilTypes.AsNoTracking()
                    .Where(w => w.DivisionId == oilTypeInput.VerticalId && w.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
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

        public ResultDto GetSkusBasedOnOilType(SkuInputDto skuInputDto)
        {
            _methodName = "GetSkusBasedOnOilTypeId";
            var skuList = new List<SkuDropDown>();
            var resultDto = new ResultDto();
            try
            {
                if (skuInputDto.IsToReturnInactiveData)
                {
                    skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == skuInputDto.OilTypeId)
                    .Select(s => new SkuDropDown() { SkuId = s.Id, SkuName = s.SkuName, Code = s.SkuCode }).ToList();
                }
                else
                {
                    skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == skuInputDto.OilTypeId && w.IsActive)
                    .Select(s => new SkuDropDown() { SkuId = s.Id, SkuName = s.SkuName, Code = s.SkuCode }).ToList();
                }
                //Case to Metric ton value conversion
                foreach (var sku in skuList)
                {
                    sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.SkuId);
                }

                resultDto.SuccessDto.Response = skuList;
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

        public ResultDto GetSkusBasedOnEmployeeDiscount(SkuInputDto skuInputDto)
        {
            _methodName = "GetSkusBasedOnEmployeeDiscount";
            var skuList = new List<DropDownDto>();
            var resultDto = new ResultDto();
            try
            {
                skuList = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.Id == skuInputDto.EmployeeDiscountParentId || w.ParentId == skuInputDto.EmployeeDiscountParentId)
                .Select(s => new DropDownDto() { Id = s.SkuId, Name = s.Sku.SkuName, Code = s.Sku.SkuCode }).ToList();

                //Case to Metric ton value conversion
                foreach (var sku in skuList)
                {
                    sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.Id);
                }

                resultDto.SuccessDto.Response = skuList;
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
        public ResultDto GetSkusUnitBasedOnOilType(SkuInputDto skuInputDto)
        {
            _methodName = "GetSkusUnitBasedOnOilType";
            var skuList = new List<SkuDropDown>();
            var resultDto = new ResultDto();
            try
            {
                if (skuInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (skuInputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerIdEmpty);
                }
                if (skuInputDto.PlantOrDepotId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PlantOrDepotEmpty);
                }
                if (skuInputDto.IsToReturnInactiveData)
                {
                    skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == skuInputDto.OilTypeId)
                    .Select(s => new SkuDropDown() { SkuId = s.Id, SkuName = s.SkuName + "-" + s.SkuCode, Code = s.SkuCode }).ToList();
                }
                else
                {
                    skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == skuInputDto.OilTypeId && w.IsActive)
                    .Select(s => new SkuDropDown() { SkuId = s.Id, SkuName = s.SkuName + "-" + s.SkuCode, Code = s.SkuCode }).ToList();
                }

                var StateId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == skuInputDto.DealerId).StateId;
                var CurrentDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                var Saudaconversion = _emamiContext.SaudaConversionUnitAndDifferenceRates.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(_.FromDate) <= DbFunctions.TruncateTime(CurrentDateTime)
                    && DbFunctions.TruncateTime(CurrentDateTime) <= DbFunctions.TruncateTime(_.ToDate) && _.SourceId == skuInputDto.PlantOrDepotId && _.StateId == StateId).ToList();

                //Case to Metric ton value conversion
                foreach (var sku in skuList)
                {
                    sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.SkuId);
                    //Get Unit value from saudaconversionunitandbaserate table
                    var Conversion = Saudaconversion.Where(_ => _.FromSkuId == sku.SkuId).OrderByDescending(_ => _.CreatedDate).FirstOrDefault();
                    sku.Unit = Conversion != null ? Conversion.FromUnit : 0;
                }

                resultDto.SuccessDto.Response = skuList;
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

        #region Dealer And Broker Details

        public ResultDto GetDealerAndBrokerDetails(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetDealerAndBrokerDetails";
            var resultDto = new ResultDto();
            try
            {
                var userList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer || w.UserRoles.RoleId == (int)DTO.Enums.Role.Broker) && w.Users.SaudaBookingTypeId == inputDto.SaudaBookingTypeId
                    //&& w.Users.DivisionId == inputDto.VerticalId
                    )

                    .Select(s => new DealerBrokerDto()
                    {
                        Id = s.Users.Id,
                        Code = s.Users.Code,
                        Name = s.Users.Name,
                        MobileNumber = s.Users.MobileNumber,
                        Email = s.Users.Email,
                        State = s.Users.State,
                        Address = s.Users.Address1,
                        RoleName = s.UserRoles.Role.Name
                    }).ToList();

                resultDto.SuccessDto.Response = userList;
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

        public ResultDto GetDealerAndBrokerListForBDO(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetDealerAndBrokerListBasedOnVertical";
            var resultDto = new ResultDto();
            var dealerBrokerDtoList = new List<DealerBrokerDto>();
            try
            {
                var divisionMappingUserIds = _emamiContext.UserDivisionMappings
                    .Where(w => inputDto.DivisionIds.Contains((long)w.DivisionId)
                    && inputDto.SalesOrganizationIds.Contains((long)w.SalesOrganizationId)
                    && inputDto.DistributionChannelIds.Contains((long)w.DistributionChannelId)
                    ).Select(_ => _.UserId).Distinct().ToList();
                var userList = new List<Data.Entities.User>();
                //if (inputDto.UserId > 0)
                //{
                //    //var mappedUserIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader)
                //    //    .Join(_emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId != inputDto.UserId), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                //    //    .Select(_ => _.UserCustomerMapping.CustomerId).ToList();

                //    userList = _emamiContext.Users.ToList();
                //}
                //else
                //{
                //    //var mappedUserIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader)
                //    //    .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                //    //    .Select(_ => _.UserCustomerMapping.CustomerId).ToList();

                //    userList = _emamiContext.Users.Where(_ => _.SaudaBookingTypeId == inputDto.SaudaBookingTypeId
                //    /*&& _.DivisionId == inputDto.VerticalId && !mappedUserIds.Contains(_.Id)*/).ToList();
                //}
                userList = _emamiContext.Users.ToList();
                var stateIds = userList.Select(s => s.StateId).Distinct().ToList();
                var stateDatas = _emamiContext.State.AsNoTracking().Where(w => stateIds.Contains(w.Id)).Select(s => new { Id = s.Id, Name = s.StateName }).ToList();

                var districtIds = userList.Select(s => s.DistrictId).Distinct().ToList();
                var districtDatas = _emamiContext.District.AsNoTracking().Where(w => districtIds.Contains(w.Id)).Select(s => new { Id = s.Id, Name = s.DistrictName }).ToList();

                dealerBrokerDtoList = userList.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                       .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer)
                       && divisionMappingUserIds.Contains(w.Users.Id)
                       )
                       .Select(s => new DealerBrokerDto()
                       {
                           Id = s.Users.Id,
                           Code = s.Users.Code,
                           Name = s.Users.Name,
                           MobileNumber = s.Users.MobileNumber,
                           Email = s.Users.Email,
                           State = stateDatas.FirstOrDefault(f => f.Id == s.Users.StateId)?.Name,
                           District = districtDatas.FirstOrDefault(f => f.Id == s.Users.DistrictId)?.Name,
                           Address = s.Users.Address1,
                           RoleName = s.UserRoles.Role.Name
                       }).ToList();

                //var customerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Select(s => new { CustomerId= s.CustomerId ,UserId=s.UserId}).Distinct().ToList();
                //var totalcustIds = customerIds.Select(_ => _.CustomerId).Distinct().ToList();
                //if (inputDto.UserId > 0)
                //{
                //    var custIds = customerIds.Where(_ => _.UserId == inputDto.UserId).Select(s => s.CustomerId).Distinct().ToList();
                //    dealerBrokerDtoList = dealerBrokerDtoList.Where(_ => custIds.Contains(_.Id) || !totalcustIds.Contains(_.Id)).ToList();
                //}
                //else
                //{
                //    dealerBrokerDtoList = dealerBrokerDtoList.Where(_ => !totalcustIds.Contains(_.Id)).ToList();
                //}

                //dealerBrokerDtoList= dealerBrokerDtoList.Where(_ => customerIds.Contains(_.Id)).ToList();
                resultDto.SuccessDto.Response = dealerBrokerDtoList;
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

        #region Dealer Details

        public ResultDto GetDealerDetails(LoginDealerIdDto loginDealerIdDto)
        {
            _methodName = "GetDealerDetails";
            var resultDto = new ResultDto();
            try
            {
                var userList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    .Where(w => w.ur.RoleId == (int)DTO.Enums.Role.Dealer && w.u.SaudaBookingTypeId == loginDealerIdDto.SaudaBookingTypeId)
                    .Select(s => new DealerBrokerDto()
                    {
                        Id = s.u.Id,
                        Code = s.u.Code,
                        Name = s.u.Name,
                        MobileNumber = s.u.MobileNumber,
                        Email = s.u.Email,
                        //State = s.u.State,
                        Address = s.u.Address1,
                        RoleName = s.ur.Role.Name
                    }).ToList();

                resultDto.SuccessDto.Response = userList;
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

        #region Users Based on Role

        public ResultDto GetUsersByRoleIdddl(IdInputDto inputDto)
        {
            _methodName = "GetUsersByRoleId";
            var resultDto = new ResultDto();
            try
            {
                var userList = _emamiContext.Users.Where(_ => _.IsActive).Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    .Where(w => w.ur.RoleId == inputDto.Id)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.u.Id,
                        Name = s.u.Name,
                    }).ToList();

                resultDto.SuccessDto.Response = userList;
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


        public ResultDto GetDealersDetailsList(FreightZoneAndRouteDropDownInputDto inputDto)
        {
            _methodName = "GetDealersDetailsList";
            var resultDto = new ResultDto();
            var dealersList = new List<DropDownDto>();
            try
            {
                if (inputDto.FreightZoneId != 0 && inputDto.FreightRouteId != 0)
                {
                    dealersList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), a => a.Id, b => b.UserId, (a, b) => new { a, b })
                    .Where(_ => _.a.IsActive && _.b.RoleId == (int)DTO.Enums.Role.Dealer
                    //&& _.a.FreightZoneId == inputDto.FreightZoneId && _.a.FreightRouteId == inputDto.FreightRouteId
                    ).Select(_ => new DropDownDto { Id = _.a.Id, Name = _.a.Name }).Distinct().ToList();
                }
                else if (inputDto.FreightZoneId != 0 && inputDto.FreightRouteId == 0)
                {
                    dealersList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), a => a.Id, b => b.UserId, (a, b) => new { a, b })
                    .Where(_ => _.a.IsActive && _.b.RoleId == (int)DTO.Enums.Role.Dealer
                    //&& _.a.FreightZoneId == inputDto.FreightZoneId
                    ).Select(_ => new DropDownDto { Id = _.a.Id, Name = _.a.Name }).Distinct().ToList();
                }
                else if (inputDto.LoginUserId > 0)
                {
                    IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
            .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                    dealersList = (from u in _emamiContext.Users.AsNoTracking()
                                   join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                   where ur.RoleId == (int)DTO.Enums.Role.Dealer
                                   select u into user
                                   join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on user.Id equals ud.UserId
                                   join lud in divisionslogieduser on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                equals new { SalesOrganizationId = lud.SalesOrganizationId, DistributionChannelId = lud.DistributionChannelId, DivisionId = lud.DivisionId }
                                   select new DropDownDto
                                   {
                                       Id = user.Id,
                                       Name = user.Name
                                   }
                                 ).ToList();

                }
                else
                {
                    dealersList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), a => a.Id, b => b.UserId, (a, b) => new { a, b })
                    .Where(_ => _.a.IsActive && _.b.RoleId == (int)DTO.Enums.Role.Dealer)
                    .Select(_ => new DropDownDto { Id = _.a.Id, Name = _.a.Name }).Distinct().ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dealersList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        #region SubCategory

        public ResultDto GetSubCategoryListddl()
        {
            _methodName = "GetSubCategoryList";
            var resultDto = new ResultDto();
            var dropDownDtos = new List<DropDownDto>();
            try
            {
                dropDownDtos = _emamiContext.SubCategory.AsNoTracking().Where(_ => _.IsActive)
                    .Select(_ => new DropDownDto { Id = _.Id, Name = _.Name }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dropDownDtos;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Configuration

        public ResultDto GetConfigurationList()
        {
            _methodName = "GetConfigurationList";
            var resultDto = new ResultDto();
            var configuration = new List<ConfigurationDto>();
            try
            {
                //configuration = _emamiContext.Configurations.AsNoTracking().Where(_ => _.Isactive)
                //    .Select(_ => new ConfigurationDto { Id = _.Id, Name = _.Name, Value = _.Value, Type = _.Type, IsNotification = _.Value.Equals("True") }).ToList();

                configuration = _emamiContext.Configurations.AsNoTracking().Where(_ => _.Isactive)
                                  .Select(_ => new ConfigurationDto { Id = _.Id, Value = _.Value, IsNotification = _.Value.Equals("True"), Name = _.Name, Type = _.TypeId }).ToList();


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = configuration;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto UpdateConfiguration(List<ConfigurationDto> inputDto)
        {
            _methodName = "UpdateConfiguration";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null && !inputDto.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                foreach (var configuration in inputDto)
                {
                    var configurationContext = _emamiContext.Configurations.FirstOrDefault(f => f.Id == configuration.Id);
                    if (configurationContext == null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                        return resultDto;
                    }

                    //configurationContext.Name = configuration.Name;
                    configurationContext.Value = configuration.Type == (int)DTO.Enums.DataType.Boolean ? configuration.IsNotification.ToString() : configuration.Value;
                    configuration.Type = configuration.Type;
                    _emamiContext.SaveChanges();
                }
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        #endregion

        #region Key Performance Indicator

        public ResultDto AddKeyPerformance(KeyPerformanceDto inputDto)
        {
            _methodName = "AddKeyPerformance";
            var resultDto = new ResultDto();
            try
            {
                var contentCount = _emamiContext.KeyPerformanceIndicator.AsNoTracking().Count(f => f.RoleId == inputDto.RoleId && f.Content == inputDto.Content);
                if (contentCount > 0)
                {
                    return _resultService.ErrorMessage(Constants.KeyPerformenceContentDuplicate);
                }

                var KeyPerformance = new KeyPerformanceIndicator();
                KeyPerformance.RoleId = inputDto.RoleId;
                KeyPerformance.Content = inputDto.Content;
                KeyPerformance.IsActive = inputDto.IsActive;
                KeyPerformance.CreatedBy = inputDto.LoginUserId;
                KeyPerformance.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.KeyPerformanceIndicator.Add(KeyPerformance);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }



        public ResultDto AddDateRange(DateRangeDTO inputDto)
        {
            _methodName = "AddDateRange";
            var resultDto = new ResultDto()
            {
                IsSuccess = true,
            };
            try
            {
                if (inputDto.FromRange2 != inputDto.ToRange1 + 1)
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = "Please Provide Correct From Range 2";
                    resultDto.SuccessDto.Response = inputDto;
                }
                else if (inputDto.FromRange3 != inputDto.ToRange2 + 1)
                {

                    inputDto.PostStatus = false;
                    inputDto.PostMessage = "Please Provide Correct From Range 3";
                    resultDto.SuccessDto.Response = inputDto;
                }
                //else if (inputDto.FromRange4 != inputDto.ToRange3 + 1)
                //{

                //    inputDto.PostStatus = false;
                //    inputDto.PostMessage = "Please Provide Correct From Range 4";
                //    resultDto.SuccessDto.Response = inputDto;
                //}
                else if (inputDto.FromRange4 != inputDto.ToRange3 + 1)
                {

                    inputDto.PostStatus = false;
                    inputDto.PostMessage = "Please Provide Correct To Range 4";
                    resultDto.SuccessDto.Response = inputDto;
                }
                else if (inputDto.ToRange1 < inputDto.FromRange1)
                {

                    inputDto.PostStatus = false;
                    inputDto.PostMessage = "Please Provide Correct To Range 1";
                    resultDto.SuccessDto.Response = inputDto;
                }
                else if (inputDto.ToRange2 < inputDto.FromRange2)
                {

                    inputDto.PostStatus = false;
                    inputDto.PostMessage = "Please Provide Correct To Range 2";
                    resultDto.SuccessDto.Response = inputDto;
                }
                else if (inputDto.ToRange3 < inputDto.FromRange3)
                {

                    inputDto.PostStatus = false;
                    inputDto.PostMessage = "Please Provide Correct To Range 3";
                    resultDto.SuccessDto.Response = inputDto;
                }
                //else if (inputDto.ToRange4 < inputDto.FromRange4)
                //{

                //    inputDto.PostStatus = false;
                //    inputDto.PostMessage = "Please Provide Correct To Range 4";
                //    resultDto.SuccessDto.Response = inputDto;
                //}
                else
                {
                    var dateRange = _emamiContext.DateRanges.ToList();
                    if (dateRange.IsAny())
                    {
                        var date = _emamiContext.DateRanges.FirstOrDefault();
                        date.FromRange1 = inputDto.FromRange1;
                        date.FromRange3 = inputDto.FromRange3;
                        date.FromRange3 = inputDto.FromRange3;
                        date.FromRange4 = inputDto.FromRange4;
                        date.ToRange1 = inputDto.ToRange1;
                        date.ToRange2 = inputDto.ToRange2;
                        date.ToRange3 = inputDto.ToRange3;
                        date.IsActive = true;
                        date.ModifiedBy = inputDto.LoginUserId;
                        date.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        _emamiContext.SaveChanges();
                        inputDto.Id = date.Id;
                    }
                    else
                    {
                        var dateRangeInput = new DateRange()
                        {
                            FromRange1 = inputDto.FromRange1,
                            FromRange2 = inputDto.FromRange2,
                            FromRange3 = inputDto.FromRange3,
                            // FromRange4 = inputDto.FromRange4,
                            ToRange1 = inputDto.ToRange1,
                            ToRange2 = inputDto.ToRange2,
                            ToRange3 = inputDto.ToRange3,
                            // ToRange4 = inputDto.ToRange4,
                            FromRange4 = inputDto.FromRange4,
                            IsActive = true,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.DateRanges.Add(dateRangeInput);
                        _emamiContext.SaveChanges();
                        inputDto.Id = dateRangeInput.Id;
                    }

                    //if ((dateRange != null || dateRange.ToList().Count != 0) && inputDto.IsActive )
                    //{
                    //    foreach (var range in dateRange)
                    //    {
                    //        if (range.IsActive)
                    //        {
                    //            range.ModifiedBy = inputDto.LoginUserId;
                    //            range.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    //            range.IsActive = false;
                    //        }

                    //    }
                    //}
                    //_emamiContext.SaveChanges();
                    //var dateRangeInput = new DateRange()
                    //{
                    //    FromRange1 = inputDto.FromRange1,
                    //    FromRange2 = inputDto.FromRange2,
                    //    FromRange3 = inputDto.FromRange3,
                    //   // FromRange4 = inputDto.FromRange4,
                    //    ToRange1 = inputDto.ToRange1,
                    //    ToRange2 = inputDto.ToRange2,
                    //    ToRange3 = inputDto.ToRange3,
                    //   // ToRange4 = inputDto.ToRange4,
                    //    FromRange4 = inputDto.FromRange4,
                    //    IsActive = inputDto.IsActive,
                    //    CreatedBy=inputDto.LoginUserId,
                    //    CreatedDate= DateHelper.UtcToIndia(DateTime.UtcNow)
                    //};
                    //_emamiContext.DateRanges.Add(dateRangeInput);
                    //_emamiContext.SaveChanges();
                    //inputDto.Id = dateRangeInput.Id;
                    inputDto.PostStatus = true;
                    inputDto.PostMessage = "Update Successfully";
                    resultDto.SuccessDto.Message = "Update Successfully";
                    resultDto.SuccessDto.Response = inputDto;
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetDateRange(long DealerId)
        {
            _methodName = "GetDateRange";
            var resultDto = new ResultDto();
            var daterange = new List<DateRange>();
            try
            {
                daterange = _emamiContext.DateRanges.AsNoTracking().ToList();
                if (daterange != null && daterange.Any())
                {
                    var result = new DateRangeDTO()
                    {
                        FromRange1 = daterange.FirstOrDefault().FromRange1,
                        ToRange1 = daterange.FirstOrDefault().ToRange1,
                        FromRange2 = daterange.FirstOrDefault().FromRange2,
                        ToRange2 = daterange.FirstOrDefault().ToRange2,
                        FromRange3 = daterange.FirstOrDefault().FromRange3,
                        ToRange3 = daterange.FirstOrDefault().ToRange3,
                        FromRange4 = daterange.FirstOrDefault().FromRange4
                    };
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = result;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
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

        public ResultDto UpdateKeyPerformance(KeyPerformanceDto inputDto)
        {
            _methodName = "UpdateKeyPerformance";
            var resultDto = new ResultDto();
            try
            {
                var contentCount = _emamiContext.KeyPerformanceIndicator.AsNoTracking().Count(f => f.RoleId == inputDto.RoleId && f.Content == inputDto.Content && f.Id != inputDto.Id);
                if (contentCount > 0)
                {
                    return _resultService.ErrorMessage(Constants.KeyPerformenceContentDuplicate);
                }

                var KeyPerformanceEntity = _emamiContext.KeyPerformanceIndicator.FirstOrDefault(f => f.Id == inputDto.Id);
                KeyPerformanceEntity.RoleId = inputDto.RoleId;
                KeyPerformanceEntity.Content = inputDto.Content;
                KeyPerformanceEntity.IsActive = inputDto.IsActive;
                KeyPerformanceEntity.ModifiedBy = inputDto.LoginUserId;
                KeyPerformanceEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetKeyPerformanceById(IdInputDto inputDto)
        {
            _methodName = "GetKeyPerformanceById";
            var resultDto = new ResultDto();
            var keyPerformence = new KeyPerformanceDto();
            try
            {
                var KeyPerformanceEntity = _emamiContext.KeyPerformanceIndicator.FirstOrDefault(f => f.Id == inputDto.Id);
                keyPerformence.Id = KeyPerformanceEntity.Id;
                keyPerformence.RoleId = KeyPerformanceEntity.RoleId;
                keyPerformence.Content = KeyPerformanceEntity.Content;
                keyPerformence.IsActive = KeyPerformanceEntity.IsActive;

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = keyPerformence;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetKeyPerformanceList(LoginUserIdDto inputDto)
        {
            _methodName = "GetKeyPerformanceList";
            var resultDto = new ResultDto();
            var KeyPerformanceEntity = new List<KeyPerformanceIndicator>();
            try
            {
                if (inputDto.IsToReturnInactiveData)
                    KeyPerformanceEntity = _emamiContext.KeyPerformanceIndicator.AsNoTracking().ToList();
                else
                    KeyPerformanceEntity = _emamiContext.KeyPerformanceIndicator.AsNoTracking().Where(w => w.IsActive).ToList();

                if (KeyPerformanceEntity != null && KeyPerformanceEntity.Any())
                {
                    var result = KeyPerformanceEntity.Select(s => new KeyPerformanceDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleId,
                        RoleName = _emamiContext.Roles.AsNoTracking().FirstOrDefault(f => f.Id == s.RoleId).Name,
                        Content = s.Content,
                        IsActive = s.IsActive
                    });
                    return _resultService.SuccessObject(result);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        #endregion

        #region Dealer Based On vertical Details

        public ResultDto GetDealerDetailsByVertical(DealerBrokerParamDto inputDto)
        {
            _methodName = "GetDealerDetailsByVertical";
            var resultDto = new ResultDto();
            try
            {
                //var userList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                //    .Where(w => w.ur.RoleId == (int)DTO.Enums.Role.Dealer && w.u.DivisionId == inputDto.VerticalId && w.u.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                //    .Select(s => new DealerBrokerDto()
                //    {
                //        Id = s.u.Id,
                //        Code = s.u.Code,
                //        Name = s.u.Name,
                //        MobileNumber = s.u.MobileNumber,
                //        Email = s.u.Email,
                //        //State = s.u.State,
                //        Address = s.u.Address,
                //        RoleName = s.ur.Role.Name
                //    }).ToList();
                //var userList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                //    .Where(w => w.ur.RoleId == (int)DTO.Enums.Role.Dealer && inputDto.DivisionIds.Contains((long)w.u.DivisionId) && w.u.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                //    .Select(s => new DealerBrokerDto()
                //    {
                //        Id = s.u.Id,
                //        Code = s.u.Code,
                //        Name = s.u.Name,
                //        MobileNumber = s.u.MobileNumber,
                //        Email = s.u.Email,
                //        //State = s.u.State,
                //        Address = s.u.Address,
                //        RoleName = s.ur.Role.Name
                //    }).ToList();

                var userContext = from u in _emamiContext.Users.AsNoTracking().Where(_ => _.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                                  join ur in _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer) on u.Id equals ur.UserId
                                  join udm in _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => inputDto.DivisionIds.Contains((long)_.DivisionId)) on u.Id equals udm.UserId
                                  select new { Users = u, UserRoles = ur };
                var userList = userContext.ToList().Select(s => new DealerBrokerDto()
                {
                    Id = s.Users.Id,
                    Code = s.Users.Code,
                    Name = s.Users.Name,
                    MobileNumber = s.Users.MobileNumber,
                    Email = s.Users.Email,
                    //State = s.u.State,
                    Address = s.Users.Address1,
                    RoleName = s.UserRoles.Role.Name
                }).ToList();

                resultDto.SuccessDto.Response = userList;
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

        #region Dealers based on vertical and state

        public ResultDto GetDealersBasedOnState(ReportingUsersInputDto inputDto)
        {
            _methodName = "GetDealersBasedOnState";
            var resultDto = new ResultDto();
            try
            {
                var userList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Join(_emamiContext.State.AsNoTracking().Where(_ => _.Id == inputDto.StateId), x => x.Users.StateId, s => s.Id, (x, s) => new { x.Users, x.UserRoles, State = s.StateName })
                    .Join(_emamiContext.District.AsNoTracking(), x => x.Users.DistrictId, d => d.Id, (x, d) => new { x.Users, x.UserRoles, x.State, District = d.DistrictName })
                    .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer
                    //&& w.Users.DivisionId == inputDto.VerticalId
                    && w.Users.StateId == inputDto.StateId)
                    .Select(s => new DealerBrokerDto()
                    {
                        Id = s.Users.Id,
                        Code = s.Users.Code,
                        Name = s.Users.Name,
                        MobileNumber = s.Users.MobileNumber,
                        Email = s.Users.Email,
                        State = s.State,
                        District = s.District,
                        Address = s.Users.Address1,
                        RoleName = s.UserRoles.Role.Name
                    }).ToList();

                resultDto.SuccessDto.Response = userList;
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

        #region Sku Ingredient OilTypes

        public ResultDto GetSkuIngredienOilTypes(IdInputDto inputDto)
        {
            _methodName = "GetSkuIngredienOilTypes";
            var resultDto = new ResultDto();
            var oilTypeIds = new List<long>();
            var oilTypeList = new List<DropDownDto>();
            try
            {
                if (inputDto.Id == (int)DTO.Enums.Division.SpecialityFat)
                {
                    oilTypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => w.DivisionId == inputDto.Id && w.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();
                }
                //else if (inputDto.Id == (int)DTO.Enums.Vertical.Hbc)
                //{
                //    // oilTypeIds = _emamiContext.OilTypes.Where(w => w.IsRasoi && w.IsActive).Select(s => s.Id).ToList();

                //    // if (oilTypeIds != null && oilTypeIds.Any())
                //    //{
                //    oilTypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => w.IsRasoi && w.IsActive)
                //   .Select(s => new DropDownDto()
                //   {
                //       Id = s.Id,
                //       Name = s.Name
                //   }).ToList();
                //    //}
                //}

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

        #region Material Cost Oiltypes

        public ResultDto MaterialCostOilTypesBasedOnVerticalId(IdInputDto inputDto)
        {
            _methodName = "MaterialCostOilTypesBasedOnVerticalId";
            var resultDto = new ResultDto();
            var oiltypeList = new List<DropDownDto>();
            //try
            //{
            //    var rasoiOilTypeIds = _emamiContext.OilTypes.Where(w => w.IsRasoi).Select(s => s.Id).ToList();

            //    oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => w.DivisionId == inputDto.Id && w.IsActive)
            //   .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name }).ToList();

            //    if (rasoiOilTypeIds != null && rasoiOilTypeIds.Any())
            //    {
            //        oiltypeList = oiltypeList.Where(w => !rasoiOilTypeIds.Contains(w.Id)).ToList();
            //    }
            //    resultDto.SuccessDto.Response = oiltypeList;
            //    resultDto.IsSuccess = true;
            //}
            //catch (Exception exception)
            //{
            //    var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
            //    resultDto.IsSuccess = false;
            //    resultDto.ErrorDto.Message = Constants.Exception;
            //    _logger.Error(message);
            //}
            return resultDto;
        }

        #endregion

        #region Sku Dropdown - Param : OilTypeId,SubCategoryId
        public ResultDto GetSkuBasedOnOilTypeSubCategoryForDropdown(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuBasedOnOilTypeSubCategoryForDropdown";
            var resultDto = new ResultDto();
            var result = new List<DropDownDto>();
            try
            {
                IQueryable<Data.Entities.Sku> skuData = _emamiContext.Skus.AsNoTracking().Where(w => w.IsActive);

                if (skuData != null)
                {
                    if (inputDto.OilTypeId > 0 && inputDto.SubCategoryId > 0 && inputDto.PackGroupId > 0)
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId
                            && w.SubCategoryId == inputDto.SubCategoryId
                            && w.PackGroupId == inputDto.PackGroupId)
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                    else if (inputDto.OilTypeId > 0 && inputDto.PackGroupId > 0)
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId && w.PackGroupId == inputDto.PackGroupId)
               .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                    else if (inputDto.OilTypeId > 0 && inputDto.SubCategoryId > 0)
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId && w.SubCategoryId == inputDto.SubCategoryId)
               .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                    else
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId)
               .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                }

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        public ResultDto GetSkuBasedOnOilTypeSubCategoryForMobile(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuBasedOnOilTypeSubCategory";
            var resultDto = new ResultDto();
            var result = new List<SkuOutputDto>();
            try
            {
                IQueryable<Data.Entities.Sku> skuData;
                if (inputDto.IsToReturnInactiveData)
                {
                    skuData = _emamiContext.Skus.AsNoTracking();
                }
                else
                {
                    skuData = _emamiContext.Skus.AsNoTracking().Where(w => w.IsActive);
                }

                if (skuData != null)
                {
                    if (inputDto.OilTypeId > 0 && inputDto.SubCategoryId > 0 && inputDto.PackGroupId > 0)
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId
                            && w.SubCategoryId == inputDto.SubCategoryId
                            && w.PackGroupId == inputDto.PackGroupId)
                            .Select(s => new SkuOutputDto() { SkuId = s.Id, Name = s.SkuName, Code = s.SkuCode, PackGroupId = s.PackGroupId ?? 0, PackGroupName = s.PackGroup.Name ?? string.Empty }).ToList();
                    }
                    else if (inputDto.OilTypeId > 0 && inputDto.PackGroupId > 0)
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId && w.PackGroupId == inputDto.PackGroupId)
               .Select(s => new SkuOutputDto() { SkuId = s.Id, Name = s.SkuName, Code = s.SkuCode, PackGroupId = s.PackGroupId ?? 0, PackGroupName = s.PackGroup.Name ?? string.Empty }).ToList();
                    }
                    else if (inputDto.OilTypeId > 0 && inputDto.SubCategoryId > 0)
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId && w.SubCategoryId == inputDto.SubCategoryId)
               .Select(s => new SkuOutputDto() { SkuId = s.Id, Name = s.SkuName, Code = s.SkuCode, PackGroupId = s.PackGroupId ?? 0, PackGroupName = s.PackGroup.Name ?? string.Empty }).ToList();
                    }
                    else
                    {
                        result = skuData.Where(w => w.OilTypeId == inputDto.OilTypeId)
               .Select(s => new SkuOutputDto() { SkuId = s.Id, Name = s.SkuName, Code = s.SkuCode, PackGroupId = s.PackGroupId ?? 0, PackGroupName = s.PackGroup.Name ?? string.Empty }).ToList();
                    }
                }

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        //public ResultDto GetOilTypeIsRasoiOrNot(IdInputDto inputDto)
        //{
        //    _methodName = "GetOilTypeIsRasoiOrNot";
        //    var resultDto = new ResultDto();
        //    var result = new OilTypeNameDto();
        //    try
        //    {
        //        var oilData = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);
        //        if (oilData != null)
        //            result.IsRasoi = oilData.IsRasoi;
        //        return _resultService.SuccessObject(result);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(exception.Message);
        //    }
        //}


        public ResultDto GetOilTypesById(string inputDto)
        {
            _methodName = "GetOilTypesById";
            var resultDto = new ResultDto();
            var oiltype = new OilTypeDto();
            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto, SecurityConstants.EncryptionKey);
                var Id = UtilityHelper.LongTryToParse(decryptedId);

                var resultContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    oiltype.Id = resultContext.Id;
                    oiltype.EncryptedId = inputDto;
                    oiltype.Name = resultContext.Name;
                    oiltype.Code = resultContext.SAPCode;
                    oiltype.IsActive = resultContext.IsActive;
                    oiltype.SalesOrganizationId = resultContext.SalesOrganizationId;
                    oiltype.DistributionChannelId = resultContext.DistributionChannelId;
                    oiltype.VerticalId = resultContext.DivisionId;
                    // oiltype.LitreConversion = resultContext.LitreConversion;
                }


                return _resultService.SuccessObject(oiltype);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        public ResultDto GetVerticalsById(string inputDto)
        {
            _methodName = "GetVerticalsById";
            var resultDto = new ResultDto();
            var vertical = new VerticalDto();
            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    vertical.EncryptedId = inputDto;
                    vertical.Id = resultContext.Id;
                    vertical.Name = resultContext.Name;
                    vertical.IsActive = resultContext.IsActive;
                    vertical.ZPR4 = resultContext.ZPR4;
                    vertical.SalesDocumentType = resultContext.SalesDocumentType;
                    vertical.SalesOrganizationId = resultContext.SalesOrganizationId;
                    vertical.DistributionChannelId = resultContext.DistributionChannelId;
                    vertical.SalesOrderDocumentType = resultContext.SalesOrderDocumentType;
                    vertical.Code = resultContext.Code;
                }
                //var verticalDetails = _emamiContext.DivisionDetails.Where(_ => _.DivisionId == vertical.Id).Select(_ => _.CCArea).ToList();
                //vertical.CCArea = string.Join(",", verticalDetails);
                return _resultService.SuccessObject(vertical);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        public ResultDto GetOilTypesByVerticalId(IdInputDto inputDto)
        {
            _methodName = "GetOilTypesByVerticalId";
            var resultDto = new ResultDto();
            var oiltypeList = new List<DropDownDto>();
            try
            {

                oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => w.DivisionId == inputDto.Id && w.IsActive)
               .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name }).ToList();

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

        #endregion

        #region ShipToParty

        public ResultDto GetShipToPartyListBasedOnVertical(DealerBrokerParamDto inputDto)
        {
            _methodName = "GetShipToPartyListBasedOnVertical";
            var resultDto = new ResultDto();
            try
            {
                //var userContext = _emamiContext.Users.AsNoTracking().Where(_ => _.DivisionId == inputDto.VerticalId)
                //    .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur });


                var userContext = from u in _emamiContext.Users.AsNoTracking()
                                  join ur in _emamiContext.UserRoles.AsNoTracking()
                                  on u.Id equals ur.UserId
                                  join udm in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals udm.UserId
                                  where ur.RoleId == (int)DTO.Enums.Role.ShipToParty
                                  select new { Users = u, UserRoles = ur, UserDivision = udm };

                //var userContext = _emamiContext.Users.AsNoTracking().Where(_ => inputDto.DivisionIds.Contains((long)_.DivisionId))
                //    .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur });
                var userList = new List<DealerBrokerDto>();
                foreach (var userdiv in inputDto.DivisionList)
                {
                    var users = userContext.ToList().Where(_ => _.UserDivision.SalesOrganizationId == userdiv.SalesOrganizationId && _.UserDivision.DistributionChannelId == userdiv.DistributionChannelId && _.UserDivision.DivisionId == userdiv.DivisionId).Select(s => new DealerBrokerDto()
                    {
                        Id = s.Users.Id,
                        Code = s.Users.ShipToPartyCode,
                        Name = s.Users.Name,
                        MobileNumber = s.Users.MobileNumber,
                        Email = s.Users.Email,
                        State = s.Users.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == s.Users.StateId).StateName : string.Empty,
                        District = s.Users.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == s.Users.DistrictId).DistrictName : string.Empty,
                        Address = s.Users.Address1,
                        RoleName = s.UserRoles.Role.Name
                    }).ToList();
                    userList.AddRange(users);
                }
                userList = userList.GroupBy(_ => _.Id).Select(s => s.FirstOrDefault()).ToList();
                userList = userList.Distinct().ToList();

                resultDto.SuccessDto.Response = userList.Distinct().ToList();
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

        #region Lookup

        /// <summary>
        /// Method to Get District List By StateId
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public ResultDto GetUnMappedDistrictListByStateId(int stateId)
        {
            _methodName = "GetUnMappedDistrictListByStateId";
            var resultDto = new ResultDto();
            var stateDto = new List<DistrictDto>();
            try
            {
                stateDto = _emamiContext.District.AsNoTracking().Where(_ => _.StateId == stateId /*&& _.TerritoryId == 0*/)
                    .Select(_ => new DistrictDto { DistrictId = _.Id, DistrictName = _.DistrictName }).Distinct().ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = stateDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to get sku based on pack group id
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSkuListByPackGroupId(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuListByPackGroupId";
            var resultDto = new ResultDto();
            try
            {
                var skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == inputDto.OilTypeId && w.PackGroupId == inputDto.PackGroupId)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.SkuName,
                        Code = s.SkuCode
                    }).ToList();

                resultDto.SuccessDto.Response = skuList;
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

        public ResultDto GetPlantDepotRakeByStateId(IdInputDto inputDto)
        {
            _methodName = "GetPlantDepotRakeByStateId";
            var resultDto = new ResultDto();
            var PlantDepotRakeList = new List<DepotRakeDto>();
            try
            {
                PlantDepotRakeList = _emamiContext.Depots.AsNoTracking()
                .Select(s => new DepotRakeDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    StorageType = s.StorageTypeId == (int)StorageType.Plant ? StorageType.Plant.ToString() : s.StorageTypeId == (int)StorageType.Depot ? StorageType.Depot.ToString() : s.StorageTypeId == (int)StorageType.Rake ? StorageType.Rake.ToString() : ""
                }).OrderBy(o => o.StorageType).ToList();
                resultDto.SuccessDto.Response = PlantDepotRakeList;
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

        //public ResultDto GetFreightZoneByStateId(IdInputDto inputDto)
        //{
        //    _methodName = "GetFreightZoneByStateId";
        //    var resultDto = new ResultDto();
        //    var FreightZoneList = new List<DepotRakeDto>();
        //    try
        //    {
        //        FreightZoneList = _emamiContext.FreightZones.AsNoTracking().Where(w => w.StateId == inputDto.Id)
        //        .Select(s => new DepotRakeDto()
        //        {
        //            Id = s.Id,
        //            Name = s.Name
        //        }).ToList();
        //        resultDto.SuccessDto.Response = FreightZoneList;
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

        public ResultDto GetSalesOrganization()
        {
            _methodName = " GetSalesOrganization";
            var resultDto = new ResultDto();
            var salesOrganization = new List<SalesOrganizationddlDto>();
            try
            {
                salesOrganization = _emamiContext.SalesOrganization.AsNoTracking().Where(_ => _.IsActive).Select(c => new SalesOrganizationddlDto
                {
                    Id = c.Id,
                    SalesOrganizationName = c.Name
                }).ToList();
                return _resultService.SuccessObject(salesOrganization);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDistributionChannel(IdInputDto id)
        {
            _methodName = " GetDistributionChannel";
            var resultDto = new ResultDto();
            var distributions = new List<DistributionChannelddlDto>();
            try
            {
                distributions = _emamiContext.DistributionChannel.AsNoTracking().Where(_ => _.IsActive && _.SalesOrganizationId == id.Id).Select(c => new DistributionChannelddlDto
                {
                    Id = c.Id,
                    DistributionChannelName = c.Name
                }).ToList();
                return _resultService.SuccessObject(distributions);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetCustomerGroupFive()
        {
            _methodName = " GetCustomerGroupFive";
            var resultDto = new ResultDto();
            var customerGroupFive = new List<CustomerGroupFiveddlDto>();
            try
            {
                customerGroupFive = _emamiContext.CustomerGroupFive.AsNoTracking().Where(_ => _.IsActive).Select(c => new CustomerGroupFiveddlDto
                {
                    CustomerGroupId = c.Id,
                    CustomerGroupName = c.GroupCode + "-" + c.GroupName
                }).ToList();
                return _resultService.SuccessObject(customerGroupFive);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        //public ResultDto GetCustomerGroupOne()
        //{
        //    _methodName = " GetCustomerGroupOne";
        //    var resultDto = new ResultDto();
        //    var customerGroupOne = new List<CustomerGroupOneandTwoddlDto>();
        //    try
        //    {
        //        customerGroupOne = _emamiContext.CustomerGroupOne.AsNoTracking().OrderByDescending(_ => _.GroupName).Select(c => new CustomerGroupOneandTwoddlDto
        //        {
        //            CustomerGroupId = c.Id,
        //            CustomerGroupName = c.GroupCode + "-" + c.GroupName
        //        }).ToList();
        //        return _resultService.SuccessObject(customerGroupOne);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto GetCustomerGroupTwo()
        //{
        //    _methodName = " GetCustomerGroupTwo";
        //    var resultDto = new ResultDto();
        //    var customerGroupTwo = new List<CustomerGroupOneandTwoddlDto>();
        //    try
        //    {
        //        customerGroupTwo = _emamiContext.CustomerGroupTwo.AsNoTracking().OrderByDescending(_ => _.GroupName).Select(c => new CustomerGroupOneandTwoddlDto
        //        {
        //            CustomerGroupId = c.Id,
        //            CustomerGroupName = c.GroupName
        //        }).ToList();
        //        return _resultService.SuccessObject(customerGroupTwo);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        public ResultDto GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuListBasedOnOilTypeSubCategoryPackGroupForDropdown";
            var resultDto = new ResultDto();
            var result = new List<DropDownDto>();
            try
            {
                var skuContext = _emamiContext.Skus.AsNoTracking().Where(w => w.IsActive);

                if (skuContext != null)
                {
                    if (inputDto.OilTypeId > 0 && inputDto.SubCategoryId > 0 && inputDto.PackGroupId > 0)
                    {
                        result = skuContext.Where(w => w.OilTypeId == inputDto.OilTypeId
                            && w.SubCategoryId == inputDto.SubCategoryId
                            && w.PackGroupId == inputDto.PackGroupId)
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                    else if (inputDto.OilTypeId > 0 && inputDto.PackGroupId > 0)
                    {
                        result = skuContext.Where(w => w.OilTypeId == inputDto.OilTypeId && w.PackGroupId == inputDto.PackGroupId)
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                    else if (inputDto.OilTypeId > 0 && inputDto.SubCategoryId > 0)
                    {
                        result = skuContext.Where(w => w.OilTypeId == inputDto.OilTypeId && w.SubCategoryId == inputDto.SubCategoryId)
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                    else
                    {
                        result = skuContext.Where(w => w.OilTypeId == inputDto.OilTypeId)
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                }

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        public ResultDto GetOilTypeListByVerticalIdsForDropDown(IdInputDto inputDto)
        {
            _methodName = "GetOilTypeListByVerticalIdsForDropDown";
            var resultDto = new ResultDto();
            try
            {
                var oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(w => inputDto.IdList.Contains(w.DivisionId) && w.IsActive)
                   .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name }).ToList();

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

        public ResultDto GetOilPackingTypeListForDropdown()
        {
            _methodName = "GetOilPackingTypeList";
            var resultDto = new ResultDto();
            try
            {
                var oilPackingTypeList = _emamiContext.OilPackingTypes.AsNoTracking().Where(w => w.IsActive)
                    .Select(_ => new DropDownDto { Id = _.Id, Name = _.Name }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = oilPackingTypeList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetVerticalListForDropdown(LoginUserIdDto inputDto)
        {
            _methodName = "GetVerticalListForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var verticalList = _emamiContext.Divisions.AsNoTracking().Where(w => w.IsActive && (inputDto.VerticalId > 0 ? w.Id == inputDto.VerticalId : w.Id > 0))
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
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

        public ResultDto GetSkuListByOilTypeIdsPackGroupIdsForDropdown(DropDownInputDto inputDto)
        {
            _methodName = "GetSkuListByOilTypeIdsPackGroupIdsForDropdown";
            var result = new List<DropDownDto>();
            try
            {
                var skuContext = _emamiContext.Skus.AsNoTracking().Where(w => w.IsActive);

                if (skuContext != null)
                {
                    if (inputDto.OilTypeIds.IsAny() && inputDto.PackGroupIds.IsAny() && inputDto.PackTypeId > 0)
                    {
                        result = skuContext.Where(w => inputDto.OilTypeIds.Contains((long)w.OilTypeId) && inputDto.PackGroupIds.Contains((long)w.PackGroupId) && inputDto.PackTypeId == w.OilPackGroupTypeId )
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode }).ToList();
                    }
                    else if (inputDto.OilTypeIds.IsAny() && inputDto.PackGroupIds.IsAny())
                    {
                        result = skuContext.Where(w => inputDto.OilTypeIds.Contains((long)w.OilTypeId) && inputDto.PackGroupIds.Contains((long)w.PackGroupId))
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode, OilTypeId = s.OilTypeId, PackGroupId = s.PackGroupId }).ToList();
                    }
                    else if (inputDto.OilTypeIds.IsAny())
                    {
                        result = skuContext.Where(w => inputDto.OilTypeIds.Contains((long)w.OilTypeId))
                            .Select(s => new DropDownDto() { Id = s.Id, Name = s.SkuName, Code = s.SkuCode, OilTypeId = s.OilTypeId,PackGroupId = s.PackGroupId}).ToList();
                    }
                }

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        #endregion

        #region 
        public ResultDto GetZonalHeadList()
        {
            _methodName = "GetZonalHeadList";
            var resultDto = new ResultDto();
            try
            {

                var ZonalHeadList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader && w.Users.IsActive)
                        .Select(s => new DropDownDto()
                        {
                            Id = s.Users.Id,
                            Name = s.Users.Name
                        }).ToList();

                resultDto.SuccessDto.Response = ZonalHeadList;
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
        public ResultDto GetZonalHeadListNew(LoginUserIdDto inputDto)
        {
            _methodName = "GetZonalHeadListNew";
            var resultDto = new ResultDto();
            try
            {
                var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (inputDto.LoginUserId > 0 && userrole != null && userrole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
            .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                    var ZonalHeadList = (from u in _emamiContext.Users.AsNoTracking()
                                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                         where ur.RoleId == (int)DTO.Enums.Role.ZonalTrader && u.IsActive
                                         select u into user
                                         join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on user.Id equals ud.UserId
                                         join lud in divisionslogieduser on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                      equals new { SalesOrganizationId = lud.SalesOrganizationId, DistributionChannelId = lud.DistributionChannelId, DivisionId = lud.DivisionId }
                                         select new DropDownDto
                                         {
                                             Id = user.Id,
                                             Name = user.Name
                                         }
                                 ).ToList();
                    resultDto.SuccessDto.Response = ZonalHeadList;
                    resultDto.IsSuccess = true;
                }
                else
                {
                    var ZonalHeadList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader && w.Users.IsActive)
                        .Select(s => new DropDownDto()
                        {
                            Id = s.Users.Id,
                            Name = s.Users.Name
                        }).ToList();

                    resultDto.SuccessDto.Response = ZonalHeadList;
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

        public ResultDto GetSkuListData(FinalPriceSkuInputDto inputDto)
        {
            _methodName = "GetSkuListData";
            var resultDto = new ResultDto();
            var outputDto = new List<FinalPriceSkuOutputDto>();
            try
            {
                var skuDetails = _emamiContext.Skus.FirstOrDefault(_ => _.Id == inputDto.SkuId);
                var skuIdsList = _emamiContext.Skus.Where(_ => _.SalesOrganizationId == skuDetails.SalesOrganizationId && _.DistributionChannelId == skuDetails.DistributionChannelId && _.DivisionId == skuDetails.DivisionId).Select(_ => _.Id).ToList();

                var cityId = 0;
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                cityId = Convert.ToInt32(userContext.CityId);

                if (inputDto.PlantId == 0)
                    return _resultService.ErrorMessage(Constants.PlantMissing);

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                var skuDatas = _emamiContext.Skus.AsNoTracking().Where(sku => skuIdsList.Contains(sku.Id))
                  .Select(s => new
                  {
                      Id = s.Id,
                      Name = s.SkuName + "-" + s.SkuCode,
                      Code = s.SkuCode,
                      OilType = s.OilTypeId,
                      Quantity = s.Quantity,
                      UomId = s.UomId,
                      s.PremiumAmount,
                      s.StorageLocation,
                      OilPackGroupTypeId = s.OilPackGroupTypeId
                  }).ToList();


                var tempoutput = _emamiContext.TodayPricing.AsNoTracking().Where(_ =>
                 _.PlantId == inputDto.PlantId && skuIdsList.Contains(_.SkuId)).Take(2000000).AsQueryable();

                outputDto = tempoutput.ToList()
                   .Select(_ => new FinalPriceSkuOutputDto
                   {
                       PricingId = _.Id,
                       SkuId = _.SkuId,
                       SkuName = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).Name : "",
                       PlantId = _.PlantId,
                       Price = _.Price,
                       OilTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilType.GetValueOrDefault() : 0,
                       OilPackGroupTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilPackGroupTypeId : null
                   }).ToList();


                var RecentPricings = from e in outputDto
                                     group e by new { e.SkuId, e.PlantId } into dptgrp
                                     let topsal = dptgrp.Max(x => x.PricingId)
                                     select new FinalPriceSkuOutputDto
                                     {
                                         SkuId = dptgrp.Key.SkuId,
                                         PlantId = dptgrp.Key.PlantId,
                                         Price = dptgrp.First(y => y.PricingId == topsal).Price,
                                         PricingId = dptgrp.First(y => y.PricingId == topsal).PricingId,
                                         SkuName = dptgrp.First(y => y.PricingId == topsal).SkuName,
                                         OilTypeId = dptgrp.First(y => y.PricingId == topsal).OilTypeId,
                                         OilPackGroupTypeId = dptgrp.First(y => y.PricingId == topsal).OilPackGroupTypeId,
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
                _logger.Info($"skuIds: {JsonConvert.SerializeObject(skuIds)}");

                //var discountGeographyDatas = _emamiContext.DiscountGeography.AsNoTracking()
                //    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                //    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)
                //    && ((_.CityId == cityId || _.CityId == 0) && (_.DistrictId == userContext.DistrictId || _.DistrictId == 0) && (_.StateId == userContext.StateId || _.StateId == 0) && _.ZoneId == userContext.ZoneId)
                //    && skuIds.Contains(_.SkuId) && _.IsActive)
                //    .Select(s => new
                //    {
                //        Id = s.Id,
                //        CityId = s.CityId,
                //        ActualDiscount = s.ActualDiscount,
                //        SkuId = s.SkuId,
                //        OilTypeId = s.OilTypeId
                //    }).ToList();
                var discountGeographyDatas = (
                  from dg in _emamiContext.DiscountGeography.AsNoTracking()
                  join sku in _emamiContext.Skus.AsNoTracking()
                      on dg.SkuId equals sku.Id into skuGroup
                  from sku in skuGroup.DefaultIfEmpty()
                  where DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(dg.ValidFrom)
                      && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(dg.ValidTo)
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
                //_logger.Info($"discountGeographyDatas: {JsonConvert.SerializeObject(discountGeographyDatas)}");

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
                    .Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)
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
                _logger.Info($"skuUomMappingDatas: {JsonConvert.SerializeObject(skuUomMappingDatas)}");
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
                    if (skuUomdata == null)
                    {
                        return _resultService.ErrorMessage(Constants.SkuUomIdRecordNotFound);
                    }
                    uomId = skuUomdata.UomId;
                    pricing.UOMId = uomId;
                    pricing.UOM = uomList.FirstOrDefault(_ => _.Id == uomId).SAPName;
                    pricing.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, skuId);

                    //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                    //{
                    //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);
                    //    if (discountGeographySkuContext != null)
                    //    {
                    //        var geographyDiscount = discountGeographySkuContext.ActualDiscount;
                    //    }
                    //}

                    if (premiumGeographyDatas != null && premiumGeographyDatas.Any())
                    {
                        var premiumGeographySkuContext = premiumGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);
                        if (premiumGeographySkuContext != null)
                        {
                            var geoGraphyPremium = premiumGeographySkuContext.ActualPremium;
                            premium = premium + geoGraphyPremium;
                        }
                    }

                    var userrole1 = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(user => user.UserId == inputDto.LoginUserId).RoleId;
                    if (discountUserDatas != null && discountUserDatas.Any())
                    {
                        if (userrole1 == (int)DTO.Enums.Role.ZonalTrader || userrole1 == (int)DTO.Enums.Role.StateTrader)
                        {
                            var discountLoginUserContext = discountUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId && _.StateId == userContext.StateId);
                            if (discountLoginUserContext != null && discountLoginUserContext.ActualDiscount > 0)
                            {
                                pricing.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                            }
                            else
                            {
                                //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                //{
                                //    //var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);
                                //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                //    if(discountGeographySkuContext == null)
                                //    {
                                //        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId);
                                //    }

                                //    //if (discountGeographySkuContext != null)
                                //    //{
                                //    //    pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                //    //    pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                //    //}
                                //    if (discountGeographySkuContext != null)
                                //    {
                                //        if (pricing.OilPackGroupTypeId != null)
                                //        {
                                //            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                //            }
                                //            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(discountGeographySkuContext.ActualDiscount, discountGeographySkuContext.SkuId, pricing.SkuId);
                                //                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                //            }
                                //        }
                                //    }
                                //}
                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                        .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                    if (discountGeographySkuContext != null)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else
                                    {
                                        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                            .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId ==
                                pricing.OilPackGroupTypeId);

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
                                //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                //{
                                //    //var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);
                                //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                //    if(discountGeographySkuContext == null)
                                //    {
                                //        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId);
                                //    }

                                //    //if (discountGeographySkuContext != null)
                                //    //{
                                //    //    pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                //    //    pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                //    //}
                                //    if (discountGeographySkuContext != null)
                                //    {
                                //        if (pricing.OilPackGroupTypeId != null)
                                //        {
                                //            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                //            }
                                //            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(discountGeographySkuContext.ActualDiscount, discountGeographySkuContext.SkuId, pricing.SkuId);
                                //                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                //            }
                                //        }
                                //    }
                                //}

                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                        .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                    if (discountGeographySkuContext != null)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else
                                    {
                                        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                            .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId ==
                                pricing.OilPackGroupTypeId);

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
                        //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                        //{
                        //    //var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);
                        //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                        //    if(discountGeographySkuContext == null)
                        //    {
                        //        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId);
                        //    }

                        //    //if (discountGeographySkuContext != null)
                        //    //{
                        //    //    pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                        //    //    pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                        //    //}
                        //    if (discountGeographySkuContext != null)
                        //    {
                        //        if (pricing.OilPackGroupTypeId != null)
                        //        {
                        //            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                        //            {
                        //                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                        //            }
                        //            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                        //            {
                        //                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(discountGeographySkuContext.ActualDiscount, discountGeographySkuContext.SkuId, pricing.SkuId);
                        //                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                        //            }
                        //        }
                        //    }
                        //}
                        if (discountGeographyDatas != null && discountGeographyDatas.Any())
                        {
                            var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                            if (discountGeographySkuContext != null)
                            {
                                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                            }
                            else
                            {
                                discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                    .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId ==
                        pricing.OilPackGroupTypeId);

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
                _logger.Info($"finalOutputDto: {JsonConvert.SerializeObject(finalOutputDto)}");
                return _resultService.SuccessObject(finalOutputDto);
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

        public ResultDto GetZHBasedOnVertical(LoginUserIdDto inputDto)
        {
            _methodName = "GetZHBasedOnVertical";
            var resultDto = new ResultDto();
            var ZonalHeadList = new List<DropDownDto>();
            try
            {
                if (inputDto.VerticalId > 0 && inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0)
                {
                    ZonalHeadList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Join(_emamiContext.UserDivisionMappings, u => u.Users.Id, ud => ud.UserId, (u, ud) => new { u, ud })
                       .Where(w => w.u.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader
                       //&& w.Users.DivisionId == inputDto.VerticalId 
                       && w.u.Users.IsActive && (w.ud.SalesOrganizationId == inputDto.SalesOrganizationId && w.ud.DistributionChannelId == inputDto.DistributionChannelId && w.ud.DivisionId == inputDto.VerticalId))
                       .Select(s => new DropDownDto()
                       {
                           Id = s.u.Users.Id,
                           Name = s.u.Users.Name
                       }).ToList();
                }
                else
                {
                    ZonalHeadList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader && w.Users.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Users.Id,
                        Name = s.Users.Name
                    }).ToList();
                }
                resultDto.SuccessDto.Response = ZonalHeadList;
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

        public ResultDto GetBDOBasedOnZonalHead(List<long> ZonalTrader)
        {
            _methodName = "GetBDOBasedOnZonalHead";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.User> entity;
                ZonalHeadDto ZHList = new ZonalHeadDto();
                ZHList.ZHIds = ZonalTrader;

                if (ZHList.ZHIds.IsAny())
                {
                    entity = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Join(_emamiContext.Users.AsNoTracking(), ur => ur.UserId, u => u.Id, (ur, u) => new { ur, u })
                        .Where(_ => ZHList.ZHIds.Contains(_.ur.ReportingToUserId) && _.ur.RoleId == (int)DTO.Enums.Role.StateTrader && _.u.IsActive).Select(s => s.u);

                    //entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    // .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                    // .Where(_ => ZHList.ZHIds.Contains(_.a.u.ReportingToId ?? 0) && _.r.Id == (int)DTO.Enums.Role.StateTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                else
                {
                    entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                     .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                     .Where(_ => _.r.Id == (int)DTO.Enums.Role.StateTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                var bdoList = entity.Select(_ => new DropDownDto()
                {
                    Id = _.Id,
                    Name = _.Name
                }).Distinct().ToList();

                //var bdoList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                //     .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                //     .Where(_ => ZHList.ZHIds.Contains(_.a.u.OrganizationReportingToId ?? 0) && _.r.Id == (int)DTO.Enums.Role.StateTrader && _.a.u.IsActive)
                //     .Select(_ => new DropDownDto()
                //     {
                //         Id = _.a.u.Id,
                //         Name = _.a.u.Name
                //     }).ToList();

                resultDto.SuccessDto.Response = bdoList;
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

        public ResultDto GetZonalHeadBasedNHComb(BookedSaudaInputDto inputDto)
        {
            _methodName = "GetZonalHeadBasedNHComb";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.User> entity;
                //ZonalHeadDto ZHList = new ZonalHeadDto();
                //ZHList.ZHIds = ZonalTrader;
                var userrole = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).FirstOrDefault();
                if (inputDto.LoginUserId > 0 && userrole.RoleId != (int)DTO.Enums.Role.Admin && inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                {
                    entity = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Join(_emamiContext.Users.AsNoTracking(), ur => ur.UserId, u => u.Id, (ur, u) => new { ur, u })
                        .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), ux => ux.u.Id, ud => ud.UserId, (ux, ud) => new { u = ux.u, ur = ux.ur, ud })
                        .Where(_ => _.ur.ReportingToUserId == inputDto.LoginUserId && _.ur.RoleId == (int)DTO.Enums.Role.ZonalTrader && _.u.IsActive
                        && _.ud.SalesOrganizationId == inputDto.SalesOrganizationId && _.ud.DistributionChannelId == inputDto.DistributionChannelId
                        && _.ud.DivisionId == inputDto.DivisionId
                        )
                        .Select(s => s.u).Distinct();

                    //entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    // .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                    // .Where(_ => _.a.u.ReportingToId==NHId && _.r.Id == (int)DTO.Enums.Role.ZonalTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                else if (userrole.RoleId == (int)DTO.Enums.Role.Admin && inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                {
                    entity = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Join(_emamiContext.Users.AsNoTracking(), ur => ur.UserId, u => u.Id, (ur, u) => new { ur, u })
                        .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), ux => ux.u.Id, ud => ud.UserId, (ux, ud) => new { u = ux.u, ur = ux.ur, ud })
                        .Where(_ => /*_.ur.ReportingToUserId == inputDto.LoginUserId &&*/ _.ur.RoleId == (int)DTO.Enums.Role.ZonalTrader && _.u.IsActive
                        && _.ud.SalesOrganizationId == inputDto.SalesOrganizationId && _.ud.DistributionChannelId == inputDto.DistributionChannelId
                        && _.ud.DivisionId == inputDto.DivisionId
                        )
                        .Select(s => s.u).Distinct();
                }
                else
                {

                    entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                      .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                      .Where(_ => _.r.Id == (int)DTO.Enums.Role.ZonalTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                var zhlist = entity.Select(_ => new DropDownDto()
                {
                    Id = _.Id,
                    Name = _.Name
                }).ToList();

                //var bdoList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                //     .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                //     .Where(_ => ZHList.ZHIds.Contains(_.a.u.OrganizationReportingToId ?? 0) && _.r.Id == (int)DTO.Enums.Role.StateTrader && _.a.u.IsActive)
                //     .Select(_ => new DropDownDto()
                //     {
                //         Id = _.a.u.Id,
                //         Name = _.a.u.Name
                //     }).ToList();

                resultDto.SuccessDto.Response = zhlist;
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


        public ResultDto GetZonalHeadBasedNH(long NHId)
        {
            _methodName = "GetBDOBasedOnZonalHead";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.User> entity;
                //ZonalHeadDto ZHList = new ZonalHeadDto();
                //ZHList.ZHIds = ZonalTrader;

                if (NHId > 0)
                {
                    entity = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Join(_emamiContext.Users.AsNoTracking(), ur => ur.UserId, u => u.Id, (ur, u) => new { ur, u })
                        .Where(_ => _.ur.ReportingToUserId == NHId && _.ur.RoleId == (int)DTO.Enums.Role.ZonalTrader && _.u.IsActive).Select(s => s.u);

                    //entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    // .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                    // .Where(_ => _.a.u.ReportingToId==NHId && _.r.Id == (int)DTO.Enums.Role.ZonalTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                else
                {

                    entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                      .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                      .Where(_ => _.r.Id == (int)DTO.Enums.Role.ZonalTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                var zhlist = entity.Select(_ => new DropDownDto()
                {
                    Id = _.Id,
                    Name = _.Name
                }).ToList();

                //var bdoList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                //     .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                //     .Where(_ => ZHList.ZHIds.Contains(_.a.u.OrganizationReportingToId ?? 0) && _.r.Id == (int)DTO.Enums.Role.StateTrader && _.a.u.IsActive)
                //     .Select(_ => new DropDownDto()
                //     {
                //         Id = _.a.u.Id,
                //         Name = _.a.u.Name
                //     }).ToList();

                resultDto.SuccessDto.Response = zhlist;
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

        public ResultDto GetDealerBasedOnBdo(List<long> bdoIds)
        {
            _methodName = "GetDealerBasedOnBdo";
            var resultDto = new ResultDto();
            var dealerList = new List<DropDownDto>();
            try
            {
                var DealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(w => bdoIds.Contains(w.UserId)).Select(_ => _.CustomerId).ToList();
                if (DealerIds.Any())
                {
                    var dealerDetails = _emamiContext.Users.AsNoTracking().Where(w => DealerIds.Contains(w.Id)).ToList();
                    dealerList = dealerDetails.Select(_ => new DropDownDto()
                    {
                        Id = _.Id,
                        Name = _.Name
                    }).ToList();
                }
                resultDto.SuccessDto.Response = dealerList;
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

        public ResultDto GetDealerCodeBasedOnBdo(List<long> bdoIds)
        {
            _methodName = "GetDealerBasedOnBdo";
            var resultDto = new ResultDto();
            var dealerList = new List<DropDownDto>();
            try
            {
                var DealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(w => bdoIds.Contains(w.UserId)).Select(_ => _.CustomerId).ToList();
                if (DealerIds.Any())
                {
                    var dealerDetails = _emamiContext.Users.AsNoTracking().Where(w => DealerIds.Contains(w.Id)).ToList();
                    dealerList = dealerDetails.Select(_ => new DropDownDto()
                    {
                        Code = _.Code,
                        Name = _.Name
                    }).ToList();
                }
                resultDto.SuccessDto.Response = dealerList;
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


        #region TPNotification
        public ResultDto GetBdoddlList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBdoddlList";
            var resultDto = new ResultDto();
            var bdoIds = new List<long>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (inputDto.LoginUserId > 0 && userrole != null && userrole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
            .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                    var bdoList = (from u in _emamiContext.Users.AsNoTracking()
                                   join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                   where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                   select u into user
                                   join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on user.Id equals ud.UserId
                                   join lud in divisionslogieduser on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                equals new { SalesOrganizationId = lud.SalesOrganizationId, DistributionChannelId = lud.DistributionChannelId, DivisionId = lud.DivisionId }
                                   select new DropDownDto
                                   {
                                       Id = user.Id,
                                       Name = user.Name
                                   }
                                 ).ToList();
                    resultDto.SuccessDto.Response = bdoList;
                    resultDto.IsSuccess = true;
                }
                else
                {
                    bdoIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader).Select(_ => _.UserId).ToList();

                    var bdoList = _emamiContext.Users.Where(w => bdoIds.Contains(w.Id) && w.IsActive)
                        .Select(s => new DropDownDto()
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList();

                    resultDto.SuccessDto.Response = bdoList;
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

        public ResultDto GetDealerListBasedOnBDO(NotificationInputDto inputDto)
        {
            _methodName = "GetDealerListBasedOnBDO";
            var resultDto = new ResultDto();
            try
            {
                var DealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(w => inputDto.BdoIds.Contains(w.UserId)).Select(_ => _.CustomerId).ToList();
                if (DealerIds.Any())
                {
                    var dealerDetails = _emamiContext.Users.AsNoTracking().Where(w => DealerIds.Contains(w.Id)).ToList();
                    var outputDto = dealerDetails.Select(_ => new NotificationDetailDto()
                    {
                        CustomerId = _.Id,
                        Code = _.Code,
                        CustomerName = _.Name,
                        MobileNumber = _.MobileNumber,
                        Email = _.Email,
                        State = _.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.StateId)?.StateName : string.Empty,
                        District = _.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(s => s.Id == _.DistrictId)?.DistrictName : string.Empty,
                        Territory = _.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(s => s.Id == _.TerritoryId)?.Name : string.Empty,
                        Zone = _.ZoneId > 0 ? _emamiContext.Zones.AsNoTracking().FirstOrDefault(s => s.Id == _.ZoneId)?.Name : string.Empty,
                        City = _.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.CityId)?.CityName : string.Empty,
                    }).ToDataSourceResult(inputDto.DataSourceRequest);

                    resultDto.SuccessDto.Response = outputDto;
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

        public ResultDto AddNotification(NotificationsDto inputDto)
        {
            _methodName = "AddNotification";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                List<long> SelectedDealerIds = UtilityHelper.ConvertStringToLongList(inputDto.SelecteDealerIdsString);

                var customerIds = inputDto.NotificationDetailDtoList.Select(s => s.CustomerId).Distinct().ToList();


                var checkIsExists = _emamiContext.TPNotification
                    .Join(_emamiContext.TPNotificationDetails.AsNoTracking(), n => n.Id, nd => nd.TPNotificationId, (n, nd) => new { Notification = n, NotificationDetail = nd })
                    .Where(w => inputDto.NotificationActionIds.Contains(w.NotificationDetail.NotificationActionId)
                    && customerIds.Contains(w.NotificationDetail.DealerId)).ToList();

                if (checkIsExists != null && checkIsExists.Any())
                {
                    foreach (var item in checkIsExists)
                    {
                        item.NotificationDetail.IsActive = false;
                        _emamiContext.SaveChanges();
                    }
                }

                var TPNotification = new TPNotification();
                TPNotification.SMS = inputDto.SMS;
                TPNotification.Email = inputDto.IsEmail;
                TPNotification.InAppNotification = inputDto.InAppNotification;

                TPNotification.CreatedBy = inputDto.CreatedBy;
                TPNotification.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.TPNotification.Add(TPNotification);
                _emamiContext.SaveChanges();


                var TPNotificationDetail = new TPNotificationDetails();

                foreach (var notificationAction in inputDto.NotificationActionIds)
                {
                    foreach (var item in inputDto.NotificationDetailDtoList)
                    {

                        TPNotificationDetail.TPNotificationId = TPNotification.Id;

                        TPNotificationDetail.NotificationActionId = notificationAction;
                        TPNotificationDetail.DealerId = item.CustomerId;
                        TPNotificationDetail.CreatedBy = inputDto.CreatedBy;
                        TPNotificationDetail.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        TPNotificationDetail.IsActive = true;
                        _emamiContext.TPNotificationDetails.Add(TPNotificationDetail);
                        _emamiContext.SaveChanges();
                    }
                }

                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto UpdateTPNotification(NotificationsDto inputDto)
        {
            _methodName = "UpdateTPNotification";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var NotificationEntity = _emamiContext.TPNotification.FirstOrDefault(f => f.Id == inputDto.Id);

                NotificationEntity.SMS = inputDto.SMS;
                NotificationEntity.Email = inputDto.IsEmail;
                NotificationEntity.InAppNotification = inputDto.InAppNotification;
                NotificationEntity.ModifiedBy = inputDto.ModifiedBy;
                NotificationEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();


                var customerIds = inputDto.NotificationDetailDtoList.Select(s => s.CustomerId).Distinct().ToList();


                var checkIsExists = _emamiContext.TPNotification
                    .Join(_emamiContext.TPNotificationDetails.AsNoTracking(), n => n.Id, nd => nd.TPNotificationId, (n, nd) => new { Notification = n, NotificationDetail = nd })
                    .Where(w => w.NotificationDetail.IsActive
                    && w.NotificationDetail.TPNotificationId != inputDto.Id
                    && inputDto.NotificationActionIds.Contains(w.NotificationDetail.NotificationActionId)
                    && customerIds.Contains(w.NotificationDetail.DealerId)).ToList();

                if (checkIsExists != null && checkIsExists.Any())
                {
                    foreach (var item in checkIsExists)
                    {
                        item.NotificationDetail.IsActive = false;
                    }
                    _emamiContext.SaveChanges();
                }

                var notificationDetailExist = _emamiContext.TPNotificationDetails.Where(f => f.TPNotificationId == inputDto.Id);
                if (notificationDetailExist != null && notificationDetailExist.Any())
                {
                    foreach (var recordDelete in notificationDetailExist)
                    {
                        _emamiContext.TPNotificationDetails.Remove(recordDelete);
                    }
                    _emamiContext.SaveChanges();
                }

                var TPNotificationDetailsEntity = new TPNotificationDetails();

                foreach (var notificationAction in inputDto.NotificationActionIds)
                {
                    foreach (var item in inputDto.NotificationDetailDtoList)
                    {
                        TPNotificationDetailsEntity.TPNotificationId = inputDto.Id;
                        TPNotificationDetailsEntity.NotificationActionId = notificationAction;
                        TPNotificationDetailsEntity.DealerId = item.CustomerId;
                        TPNotificationDetailsEntity.CreatedBy = inputDto.ModifiedBy;
                        TPNotificationDetailsEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        TPNotificationDetailsEntity.IsActive = true;
                        _emamiContext.TPNotificationDetails.Add(TPNotificationDetailsEntity);
                        _emamiContext.SaveChanges();
                    }

                }


                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }
        public ResultDto GetTPNotificationList(LoginUserIdDto inputDto)
        {
            _methodName = "GetTPNotificationList";
            var resultDto = new ResultDto();
            var tpNotificationEntity = new List<TPNotification>();
            List<NotificationsDto> notificationList = new List<NotificationsDto>();
            try
            {
                tpNotificationEntity = _emamiContext.TPNotification.AsNoTracking().ToList();

                var tpNotificationDetailEntity = _emamiContext.TPNotificationDetails.AsNoTracking();

                if (tpNotificationEntity != null && tpNotificationEntity.Any())
                {
                    foreach (var s in tpNotificationEntity)
                    {
                        var actionIds = tpNotificationDetailEntity.Where(w => w.TPNotificationId == s.Id).Select(n => n.NotificationActionId).Distinct().ToList().ToArray();
                        NotificationsDto tpNotificationDto = new NotificationsDto();
                        tpNotificationDto.EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey);
                        tpNotificationDto.Id = s.Id;
                        tpNotificationDto.SMS = s.SMS;
                        tpNotificationDto.IsEmail = s.Email;
                        tpNotificationDto.InAppNotification = s.InAppNotification;
                        tpNotificationDto.NotificationActions = Utility.GetEnumFromString<DTO.Enums.NotificationActionTP>(actionIds);
                        tpNotificationDto.CreatedBy = s.CreatedBy;
                        tpNotificationDto.CreatedDate = s.CreatedDate;
                        notificationList.Add(tpNotificationDto);
                    }


                    return _resultService.SuccessObject(notificationList);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }
        public ResultDto GetTPNotificationDetailsById(long tpNotificationId)
        {
            _methodName = "GetTPNotificationDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new List<NotificationDetailDto>();
            try
            {
                var resultContext = _emamiContext.TPNotificationDetails.AsNoTracking().Where(_ => _.TPNotificationId == tpNotificationId && _.IsActive);

                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.ToList()
                    .Select(_ => new NotificationDetailDto
                    {
                        NotificationId = _.Id,
                        CustomerName = _.Dealer.Name ?? string.Empty,
                        IsActive = _.IsActive,
                        State = _.Dealer.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(w => w.Id == _.Dealer.StateId).StateName : string.Empty,
                        District = _.Dealer.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(w => w.Id == _.Dealer.DistrictId).DistrictName : string.Empty,


                    })
                    .ToList();
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
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetTPNotificationById(IdInputDto inputDto)
        {
            _methodName = "GetTPNotificationById";
            var resultDto = new ResultDto();
            var TPNotification = new NotificationsDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                var NotificationEntity = _emamiContext.TPNotification.FirstOrDefault(f => f.Id == inputDto.Id);
                if (NotificationEntity != null)
                {
                    TPNotification.Id = NotificationEntity.Id;
                    TPNotification.SMS = NotificationEntity.SMS;
                    TPNotification.IsEmail = NotificationEntity.Email;
                    TPNotification.InAppNotification = NotificationEntity.InAppNotification;

                }

                var tpNotificationDetails = _emamiContext.TPNotificationDetails.Where(f => f.TPNotificationId == inputDto.Id).ToList();

                if (tpNotificationDetails != null && tpNotificationDetails.Any())
                {

                    TPNotification.NotificationActionIds = tpNotificationDetails.Select(s => s.NotificationActionId).Distinct().ToList();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = TPNotification;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }
        public ResultDto GetMappedDealerListByTPNotificationId(NotificationGridInputDto inputDto)
        {
            _methodName = "GetMappedCustomerListByCustomerGroupId";
            var resultDto = new ResultDto();
            DataSourceResult result = new DataSourceResult();
            try
            {
                if (inputDto.NotificationId != 0)
                {

                    var mappedDealers = _emamiContext.TPNotificationDetails.AsNoTracking()
                        .Where(_ => _.TPNotificationId == inputDto.NotificationId).ToList();


                    var Dealers = mappedDealers.Select(s => new
                    {
                        CustomerName = s.Dealer.Name,
                        CustomerId = s.DealerId,
                        NotificationId = s.TPNotificationId,
                        MobileNumber = s.Dealer.MobileNumber,
                        Email = s.Dealer.Email,
                        Code = s.Dealer.Code,
                        Zone = s.Dealer.ZoneId > 0 ? _emamiContext.Zones.AsNoTracking().FirstOrDefault(w => w.Id == s.Dealer.ZoneId).Name : string.Empty,
                        Territory = s.Dealer.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(w => w.Id == s.Dealer.TerritoryId).Name : string.Empty,
                        State = s.Dealer.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(w => w.Id == s.Dealer.StateId).StateName : string.Empty,
                        District = s.Dealer.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(w => w.Id == s.Dealer.DistrictId).DistrictName : string.Empty,
                        City = s.Dealer.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(w => w.Id == s.Dealer.CityId).CityName : string.Empty
                    }).Distinct();


                    if (Dealers != null)
                    {
                        result = Dealers.ToDataSourceResult(inputDto.DataSourceRequest);
                    }
                }
                resultDto.SuccessDto.Response = result;
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
        public ResultDto ExportTPNotificationList(LoginUserIdDto inputDto)
        {
            _methodName = "ExportTPNotificationList";
            var resultDto = new ResultDto();
            try
            {
                var outputDto = new List<NotificationsDto>();
                var resultContext = _emamiContext.TPNotification.AsNoTracking();
                var detailresultContext = _emamiContext.TPNotificationDetails.AsNoTracking();
                outputDto = resultContext.ToList().Select(s => new NotificationsDto()
                {
                    Id = s.Id,
                    SMS = s.SMS,
                    IsEmail = s.Email,
                    InAppNotification = s.InAppNotification,
                    NotificationActions = Utility.GetEnumFromString<DTO.Enums.Status>(string.Join(",", detailresultContext.Where(w => w.TPNotificationId == s.Id).Select(n => n.NotificationActionId)).Split(',').Select(Int64.Parse).Distinct().ToList().ToArray()),

                    NotificationDetailDtoList = detailresultContext.ToList().Where(w => w.TPNotificationId == s.Id).Select(_ => new NotificationDetailDto
                    {
                        NotificationId = _.Id,
                        CustomerName = _.Dealer.Name ?? string.Empty,
                        State = _.Dealer.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(w => w.Id == _.Dealer.StateId).StateName : string.Empty,
                        District = _.Dealer.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(w => w.Id == _.Dealer.DistrictId).DistrictName : string.Empty,
                        IsActive = _.IsActive,

                    }).ToList()
                }).ToList();

                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
            return resultDto;
        }
        #endregion

        #region SendSms

        public ResultDto SendNotification(SmsInputDto inputDto)
        {
            _methodName = "SendNotification";
            var result = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                    var userDetails = new List<SmsInputDto>();

                    if (inputDto.LiveOrTesting == (int)LiveOrTesting.Testing)
                    {
                        if (inputDto.NotificationType == (int)AppNotificationType.SMS)
                        {
                            if (inputDto.TestMobileNumber.Contains(','))
                            {
                                var mobileNumbers = inputDto.TestMobileNumber.Split(',').ToList();
                                if (mobileNumbers != null && mobileNumbers.Any())
                                {
                                    foreach (var number in mobileNumbers)
                                    {
                                        userDetails.Add(new SmsInputDto() { MobileNumber = number });
                                    }
                                }
                            }
                            else
                            {
                                userDetails = new List<SmsInputDto>() { new SmsInputDto() { MobileNumber = inputDto.TestMobileNumber } };
                            }
                        }
                        else if (inputDto.NotificationType == (int)AppNotificationType.Email)
                        {
                            if (!string.IsNullOrEmpty(inputDto.TestEmail))
                            {
                                if (inputDto.TestEmail.Contains(','))
                                {
                                    var mails = inputDto.TestEmail.Split(',').ToList();
                                    if (mails != null && mails.Any())
                                    {
                                        foreach (var mail in mails)
                                        {
                                            userDetails.Add(new SmsInputDto() { Email = mail });
                                        }
                                    }
                                }
                                else
                                {
                                    userDetails = new List<SmsInputDto>() { new SmsInputDto() { Email = inputDto.TestEmail } };
                                }
                            }
                        }
                        else if (inputDto.NotificationType == (int)AppNotificationType.Pushnotification)
                        {
                            if (!string.IsNullOrEmpty(inputDto.TestMobileNumber))
                            {
                                if (inputDto.TestMobileNumber.Contains(','))
                                {
                                    var mobileNumbers = inputDto.TestMobileNumber.Split(',').ToList();
                                    if (mobileNumbers != null && mobileNumbers.Any())
                                    {
                                        foreach (var number in mobileNumbers)
                                        {
                                            userDetails.Add(new SmsInputDto() { MobileNumber = number });
                                        }
                                    }
                                }
                                else
                                {
                                    userDetails = new List<SmsInputDto>() { new SmsInputDto() { MobileNumber = inputDto.TestMobileNumber } };
                                }
                            }
                        }
                    }
                    else
                    {
                        string query = @"Select r.Name as RoleName,u.Name as UserName,u.MobileNumber,u.Email,u.PushTokenKey,u.RegistrationTypeId 
                                    From Users u Join UserRoles ur ON u.Id = ur.UserId
                                    Join UserDivisionMappings as udiv on u.Id = udiv.UserId
                                    Where ur.RoleId = @RoleId AND u.IsActive = @IsActive AND udiv.SalesOrganizationId = @SalesOrganizationId and
                                    udiv.DistributionChannelId = @DistributionChannelId and udiv.DivisionId = @VerticalId ";
                        userDetails = conn.Query<SmsInputDto>(query, new
                        {
                            RoleId = inputDto.RoleId,
                            VerticalId = inputDto.VerticalId,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            IsActive = true
                        }).ToList();
                    }

                    if (inputDto != null)
                    {
                        switch (inputDto.NotificationType)
                        {
                            case (int)AppNotificationType.SMS:
                                result = SendSms(userDetails, inputDto.SmsContent);
                                break;

                            case (int)AppNotificationType.Email:
                                result = SendEmail(userDetails, inputDto.SmsContent, inputDto.Subject);
                                break;
                            case (int)AppNotificationType.Pushnotification:
                                result = SendPushNotification(userDetails, inputDto.SmsContent);
                                break;
                            default:
                                return _resultService.ErrorMessage(Constants.NotificationError);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result = _resultService.ErrorMessage(Constants.NotificationError);
            }
            return result;
        }

        public ResultDto SendSms(List<SmsInputDto> userDetails, string smsContent)
        {
            _methodName = "SendSms";
            string messageResult = "";
            try
            {
                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                int totalMobilenumber = 0;
                int smsSendMobilenumber = 0;
                var mobileNumberList = userDetails.Select(s => s.MobileNumber).Distinct().ToList();
                if (mobileNumberList.Any())
                {
                    totalMobilenumber = mobileNumberList.Count;
                    for (int i = 0; i < mobileNumberList.Count; i++)
                    {
                        try
                        {
                            var mobileTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name == Constants.OtpSms);

                            var replaceMobileTemplates = mobileTemplate.PlainTemplate.Replace(Constants.OtpValue, "000000");
                            //if (mobileNumberList[i].TrimAndReduce().Count() == 10)
                            //{
                            //    mobileNumberList[i] = "+91 " + mobileNumberList[i].TrimAndReduce();
                            //}
                            if (mobileTemplate != null)
                            {
                                amazonNotificationService.SendMessage(replaceMobileTemplates, mobileNumberList[i], mobileTemplate.SMSTemplateID);
                                smsSendMobilenumber++;
                            }

                        }
                        catch (Exception ex)
                        {
                            var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                            _logger.Error(message);
                        }
                    }
                }
                messageResult = "Total Mobile Number : " + totalMobilenumber + ", SMS Sended : " + smsSendMobilenumber;
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.NotificationError);
            }
            return _resultService.SuccessMessage(messageResult);
        }

        public ResultDto SendEmail(List<SmsInputDto> userDetails, string smsContent, string subject)
        {
            _methodName = "SendEmail";
            var result = new ResultDto();
            try
            {
                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                var emailList = userDetails.Select(s => s.Email).ToList();
                if (emailList.Any())
                {
                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(f => f.Name == Constants.ForgotPasswordEmail);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<h2 style=\"margin-bottom:30px;\">Dear Customer,</h2>");
                    sb.Append("<p style=\"margin-bottom:40px;\">" + smsContent + "</p>");
                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, sb.ToString());

                    string messageResult = "";
                    int messageCount = 0;
                    int skipCount = 0;
                    int takeCount = Config.MaximumEmailCount;
                    decimal divider = Convert.ToDecimal(string.Format("{0:0.0}", takeCount));
                    decimal count = 0;

                    if (emailList.Count < takeCount)
                        count = 1;
                    else
                        count = Math.Round(emailList.Count / divider);

                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            var emailResult = emailList.Skip(skipCount).Take(takeCount).ToList();
                            result = amazonNotificationService.SendEmail(emailResult, subject, string.Empty, htmlTemplate, true);
                            if (result.IsSuccess)
                            {
                                messageCount += emailResult.Count;
                            }
                            skipCount += takeCount;
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                    messageResult = "Total Emails : " + userDetails.Count + ", Sended : " + messageCount;
                    result.SuccessDto.Response = messageResult;
                }
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                result = _resultService.ErrorMessage(Constants.NotificationError);
            }
            return result;
        }

        public ResultDto SendPushNotification(List<SmsInputDto> userDetails, string smsContent)
        {
            _methodName = "SendEmail";
            var result = new ResultDto();
            try
            {
                if (userDetails.IsAny())
                {

                    foreach (var data in userDetails)
                    {
                        var userData = _emamiContext.Users.AsNoTracking().Where(_ => _.MobileNumber == data.MobileNumber).Select(s => new SmsInputDto()
                        {
                            PushTokenKey = s.PushTokenKey,
                            RegistrationTypeId = s.RegistrationTypeId,

                        }).FirstOrDefault();
                        //string query = @"Select r.Name as RoleName,u.Name as UserName,u.MobileNumber,u.Email,u.PushTokenKey,u.RegistrationTypeId 
                        //            From Users u Join UserRoles ur ON u.Id = ur.UserId Join Roles r ON ur.RoleId = r.Id  Where r.Id = @RoleId And u.DivisionId = @VerticalId AND u.IsActive = @IsActive";
                        //userDetails = conn.Query<SmsInputDto>(query, new
                        //{
                        //    RoleId = inputDto.RoleId,
                        //    VerticalId = inputDto.VerticalId,
                        //    IsActive = true
                        //}).ToList();
                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                        {
                            PushTokenKey = userData.PushTokenKey,
                            RegistrationTypeId = userData.RegistrationTypeId != null ? (int)userData.RegistrationTypeId : 0,
                            Title = "Test",
                            Message = smsContent,
                            //Id = saudaOrderContext.Id,
                        };
                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                    }


                    //SendPushNotificationThroughFirebase(pushNotificationInputDto);
                }
                result = _resultService.SuccessMessage(Constants.SuccessMessage);

            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                result = _resultService.ErrorMessage(Constants.NotificationError);
            }
            return result;
        }

        #endregion

        public ResultDto SendSmsNotification(NotificationsSmsSendInputDto inputDto)
        {
            int totalMobilenumber = 0;
            int smsSendMobilenumber = 0;
            try
            {
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();

                    string query = @"Select r.Name as RoleName,u.Name as UserName,u.MobileNumber From Users u
                                    Join UserRoles ur ON u.Id = ur.UserId
                                    Join Roles r ON ur.RoleId = r.Id
                                    Where r.Id = @RoleId";
                    var userDetails = conn.Query<NotificationsSmsSendDto>(query,
                                new
                                {
                                    RoleId = inputDto.RoleId
                                }).ToList();

                    if (userDetails != null && userDetails.Any())
                    {
                        var mobileNumberList = userDetails.Select(s => s.MobileNumber).Distinct().ToList();
                        if (mobileNumberList != null && mobileNumberList.Any())
                        {
                            totalMobilenumber = mobileNumberList.Count;
                            for (int i = 0; i < mobileNumberList.Count; i++)
                            {
                                try
                                {
                                    //if (mobileNumberList[i].TrimAndReduce().Count() == 10)
                                    //{
                                    //    mobileNumberList[i] = "+91 " + mobileNumberList[i].TrimAndReduce();
                                    //}
                                    amazonNotificationService.SendMessage(inputDto.SmsContent, mobileNumberList[i]);
                                    smsSendMobilenumber++;
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            string messageResult = "Total " + totalMobilenumber + " - " + " SMS Send " + smsSendMobilenumber;
            return _resultService.SuccessMessage(messageResult);
        }
        #region sauda Conversion type
        public ResultDto GetSaudaConversionList()
        {
            _methodName = "GetSaudaConversionList";
            var resultDto = new ResultDto();
            var ConversionTypes = new List<SaudaConversionTypeDto>();
            try
            {
                ConversionTypes = _emamiContext.saudaConversionTypes.AsNoTracking().Select(_ => new SaudaConversionTypeDto()
                {
                    Id = _.Id,
                    Name = _.Name,
                    IsActive = _.IsActive
                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = ConversionTypes;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }


        public ResultDto UpdateSaudaConversionType(List<SaudaConversionTypeDto> inputDto)
        {
            _methodName = "UpdateSaudaConversionType";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null && !inputDto.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                foreach (var Conversiontype in inputDto)
                {
                    var conversiontypeContext = _emamiContext.saudaConversionTypes.FirstOrDefault(f => f.Id == Conversiontype.Id);
                    if (conversiontypeContext == null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                        return resultDto;
                    }
                    conversiontypeContext.IsActive = Conversiontype.IsActive;
                    conversiontypeContext.ModifiedDate = DateTime.Now;

                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto AddSaudaExtensionPolicy(SaudaExtensionPolicyAddDto inputDto)
        {
            _methodName = "AddSaudaExtensionPolicy";
            var resultDto = new ResultDto();
            if (inputDto == null)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidRequest;
                return resultDto;
            }
            if (inputDto.UserId <= 0)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidUser;
                return resultDto;
            }
            if (inputDto.OilIds == null && !inputDto.OilIds.Any())
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidOilTypeId;
                return resultDto;
            }
            if (inputDto.StateIds == null && !inputDto.StateIds.Any())
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidState;
                return resultDto;
            }

            if (inputDto.Days < 0)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidExtensionDays;
                return resultDto;
            }

            //Isactive false - Existing extension policy
            var ExtensionContext = _emamiContext.SaudaExtension.Where(_ => inputDto.OilIds.Contains(_.OilTypeId) && inputDto.StateIds.Contains(_.StateId) && _.IsActive &&
            DbFunctions.TruncateTime(inputDto.ValidFrom) <= DbFunctions.TruncateTime(_.ValidTo) && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo));
            if (ExtensionContext != null)
            {
                foreach (var data in ExtensionContext)
                {
                    data.IsActive = false;
                    data.ModifiedBy = inputDto.UserId;
                    data.ModifiedDate = DateTime.Now;
                }
            }


            //Create new Extension policy
            foreach (var stateId in inputDto.StateIds)
            {
                foreach (var oilId in inputDto.OilIds)
                {
                    _emamiContext.SaudaExtension.Add(new SaudaExtension()
                    {
                        OilTypeId = oilId,
                        StateId = stateId,
                        ExtensionDays = inputDto.Days,
                        IsActive = true,
                        ValidFrom = inputDto.ValidFrom,
                        ValidTo = inputDto.ValidTo,
                        CreatedBy = inputDto.UserId,
                        CreatedDate = DateTime.Now
                    });

                }

            }
            _emamiContext.SaveChanges();

            resultDto.IsSuccess = true;
            return resultDto;
        }

        public ResultDto GetSaudaExtensionList(long verticalId)
        {
            _methodName = "GetSaudaExtensionList";
            var resultDto = new ResultDto();
            var ExtensionList = new List<SaudaExtensionPolicyViewDto>();
            try
            {
                if (verticalId > 0)
                {
                    ExtensionList = _emamiContext.SaudaExtension.AsNoTracking().Where(_ => _.OilType.DivisionId == verticalId).Select(_ => new SaudaExtensionPolicyViewDto()
                    {
                        Id = _.Id,
                        IsActive = _.IsActive,
                        CreatedDate = _.CreatedDate,
                        Days = _.ExtensionDays,
                        OilId = _.OilTypeId,
                        OilTypeName = _.OilType.Name + "-" + _.OilType.SalesOrganization.Code + "/" + _.OilType.DistributionChannel.Code + "/" + _.OilType.Division.Code,
                        //OilTypeCode = _.OilType.SAPCode,
                        StateName = _.State.StateName,
                        StateId = _.StateId,
                        ValidFrom = _.ValidFrom,
                        ValidTo = _.ValidTo
                    }).OrderByDescending(_ => _.CreatedDate).ToList();
                }
                else
                {
                    ExtensionList = _emamiContext.SaudaExtension.AsNoTracking().Select(_ => new SaudaExtensionPolicyViewDto()
                    {
                        Id = _.Id,
                        IsActive = _.IsActive,
                        CreatedDate = _.CreatedDate,
                        Days = _.ExtensionDays,
                        OilId = _.OilTypeId,
                        OilTypeName = _.OilType.Name + "-" + _.OilType.SalesOrganization.Code + "/" + _.OilType.DistributionChannel.Code + "/" + _.OilType.Division.Code,
                        //OilTypeCode = _.OilType.SAPCode,
                        StateName = _.State.StateName,
                        StateId = _.StateId,
                        ValidFrom = _.ValidFrom,
                        ValidTo = _.ValidTo
                    }).OrderByDescending(_ => _.CreatedDate).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = ExtensionList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        #endregion

        #region Delete List Creation
        public ResultDto GetRemarksGroup(IdInputDto inputDto)
        {
            _methodName = "GetRemarksGroup";
            var resultDto = new ResultDto();
            var RemarkGroupList = new List<DeleteListCreateDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var result = _emamiContext.DeleteListCreations.AsNoTracking().Where(_ => _.DeleteListId == inputDto.Id && _.IsActive).ToList();
                RemarkGroupList = result.Select(_ => new DeleteListCreateDto
                {
                    Id = _.Id,
                    DeleteListId = _.DeleteListId,
                    DeleteListName = UtilityHelper.GetEnumDescription((DTO.Enums.DeleteListCreation)inputDto.Id),
                    Remarks = _.Remarks
                }).ToList();


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = RemarkGroupList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto AddDeleteListRemarks(AddDeleteListRemarks inputDto)
        {
            _methodName = "GetRemarksGroup";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (!inputDto.DeleteListRemark.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var EnumIds = inputDto.DeleteListRemark.Select(_ => _.DeleteListId).Distinct();
                foreach (var enumId in EnumIds)
                {
                    var RemarkList = _emamiContext.DeleteListCreations.Where(_ => _.DeleteListId == enumId && _.IsActive);
                    foreach (var item in RemarkList)
                    {
                        var data = inputDto.DeleteListRemark.FirstOrDefault(_ => _.Id == item.Id);
                        if (data == null)
                        {
                            item.IsActive = false;
                            item.ModifiedBy = inputDto.LoginUserId;
                            item.ModifiedDate = DateTime.Now;
                        }
                    }
                }
                var newRemarks = inputDto.DeleteListRemark.Where(_ => _.Id == 0);
                foreach (var newitem in newRemarks)
                {
                    _emamiContext.DeleteListCreations.Add(new Data.Entities.DeleteListCreation()
                    {
                        DeleteListId = newitem.DeleteListId,
                        Remarks = newitem.Remarks,
                        IsActive = true,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateTime.Now
                    });
                }
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Permission checking - Verticals
        public ResultDto CheckPermissionForVertical(LoginUserIdDto inputDto)
        {
            _methodName = "CheckPermissionForVertical";
            var resultDto = new ResultDto();
            var result = new VerticalPermissionCheckDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
                //var verticalofLoginUserId = userContext.Division.Name;
                var configurationName = UtilityHelper.GetEnumDescription(DTO.Enums.ConfigurationForVerticalsAndEmails.VerticalsBasedOnSaudaValidityDate);
                var configurationforVerticals = _emamiContext.ConfigurationForDivisionsAndEmails.AsNoTracking().FirstOrDefault(configuration => configuration.Name == configurationName).Value;
                var configurationforVerticalsList = configurationforVerticals.Split(',').ToList();
                //if (configurationforVerticalsList.Contains(verticalofLoginUserId))
                //{
                //    result.IsVerticalPermission = true;
                //}
                //else
                //{
                //    result.IsVerticalPermission = false;
                //}
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        #endregion

        #region Sauda validity and Sauda report email configuration
        public ResultDto SaveConfigurationforSaudaValidityAndSaudaReportMails(SaudaValidityAndSaudaReportMailConfigurationDto inputDto)
        {
            _methodName = "SaveConfigurationforSaudaValidityAndSaudaReportMails";
            var resultDto = new ResultDto();
            var result = new SaudaValidityAndSaudaReportMailConfigurationDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var SaudaValidityName = UtilityHelper.GetEnumDescription(DTO.Enums.ConfigurationForVerticalsAndEmails.VerticalsBasedOnSaudaValidityDate);
                var SaudaReportMailKey = UtilityHelper.GetEnumDescription(DTO.Enums.ConfigurationForVerticalsAndEmails.EmailsBasedOnVerticalsForSaudaReport);

                var configurationcontext = _emamiContext.ConfigurationForDivisionsAndEmails.ToList();
                var configurationcontextForSaudaValidtyExists = configurationcontext.FirstOrDefault(_ => _.Name == SaudaValidityName);

                var Verticals = _emamiContext.Divisions.AsNoTracking().Where(vertical => inputDto.VerticalsBasedOnSaudaValidity.Contains(vertical.Id)).Select(_ => _.Name).ToList();
                if (configurationcontextForSaudaValidtyExists != null)
                {
                    if (configurationcontextForSaudaValidtyExists.Value != "")
                    {
                        var verticalsList = configurationcontextForSaudaValidtyExists.Value.Split(',').ToList();
                        if (verticalsList.IsAny())
                        {
                            foreach (var name in verticalsList)
                            {
                                var verticalContext = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(vertical => vertical.Name == name);
                                if (verticalContext == null)
                                {
                                    resultDto.IsSuccess = false;
                                    resultDto.ErrorDto.Message = name + " " + " not exists";
                                    return resultDto;
                                }
                            }
                        }
                    }

                    configurationcontextForSaudaValidtyExists.Value = string.Join(",", Verticals);
                    _emamiContext.SaveChanges();
                    result.Id = configurationcontextForSaudaValidtyExists.Id;
                }
                var VerticalName = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.VerticalId).Name;
                var configurationcontextForSaudaReportMailExists = configurationcontext.FirstOrDefault(_ => _.Key == SaudaReportMailKey && _.Name == VerticalName);
                if (configurationcontextForSaudaReportMailExists != null)
                {
                    configurationcontextForSaudaReportMailExists.Value = inputDto.EmailIds;
                    _emamiContext.SaveChanges();
                    result.Id = configurationcontextForSaudaReportMailExists.Id;
                }
                else
                {
                    var configurationContext = new Data.Entities.ConfigurationForDivisionsAndEmails()
                    {
                        Name = VerticalName,
                        Value = inputDto.EmailIds,
                        Key = SaudaReportMailKey,
                        TypeId = (int)DTO.Enums.DataType.String,
                        SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                        Isactive = true,
                    };
                    _emamiContext.ConfigurationForDivisionsAndEmails.Add(configurationContext);
                    _emamiContext.SaveChanges();
                    result.Id = configurationContext.Id;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetVerticalListBasedOnSaudaValidity()
        {
            _methodName = "GetVerticalListBasedOnSaudaValidity";
            var resultDto = new ResultDto();
            var verticals = new List<long>();
            try
            {
                var SaudaValidityName = UtilityHelper.GetEnumDescription(DTO.Enums.ConfigurationForVerticalsAndEmails.VerticalsBasedOnSaudaValidityDate);
                var verticalsInstring = _emamiContext.ConfigurationForDivisionsAndEmails.AsNoTracking().FirstOrDefault(_ => _.Name == SaudaValidityName).Value;
                var verticalListofString = verticalsInstring.Split(',').ToList();
                verticals = _emamiContext.Divisions.AsNoTracking().Where(_ => verticalListofString.Contains(_.Name)).Select(a => a.Id).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = verticals;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetVerticalListAndMailIds(long verticalId)
        {
            _methodName = "GetVerticalListBasedOnSaudaValidity";
            var resultDto = new ResultDto();
            var result = new SaudaValidityAndSaudaReportMailConfigurationDto();
            try
            {
                var SaudaValidityName = UtilityHelper.GetEnumDescription(DTO.Enums.ConfigurationForVerticalsAndEmails.VerticalsBasedOnSaudaValidityDate);
                var verticalsInstring = _emamiContext.ConfigurationForDivisionsAndEmails.AsNoTracking().FirstOrDefault(_ => _.Name == SaudaValidityName).Value;
                var verticalListofString = verticalsInstring.Split(',').ToList();
                result.VerticalsBasedOnSaudaValidity = _emamiContext.Divisions.AsNoTracking().Where(_ => verticalListofString.Contains(_.Name)).Select(a => a.Id).ToList();
                var verticalName = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Id == verticalId).Name;
                var SaudaReportMailKey = UtilityHelper.GetEnumDescription(DTO.Enums.ConfigurationForVerticalsAndEmails.EmailsBasedOnVerticalsForSaudaReport);
                var configurationcontext = _emamiContext.ConfigurationForDivisionsAndEmails.AsNoTracking().FirstOrDefault(_ => _.Key == SaudaReportMailKey && _.Name == verticalName);
                if (configurationcontext != null)
                {
                    result.EmailIds = configurationcontext.Value;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        public ResultDto GetZonalHeadListByNH(NationalHeadDto inputDto)
        {
            _methodName = "GetZonalHeadListByNH";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Data.Entities.User> entity;
                entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.User);
                entity = entity.Where(_ => _.IsActive
                //&& (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0)
                );

                if (inputDto.NHIds.IsAny())
                {
                    entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                         .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                         .Where(_ => inputDto.NHIds.Contains(_.a.u.ReportingToId ?? 0) && _.r.Id == (int)DTO.Enums.Role.ZonalTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                else
                {
                    entity = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                         .Join(_emamiContext.Roles.AsNoTracking(), a => a.ur.RoleId, r => r.Id, (a, r) => new { a, r })
                         .Where(_ => _.r.Id == (int)DTO.Enums.Role.ZonalTrader && _.a.u.IsActive).Select(_ => _.a.u);
                }
                var zhList = entity.Select(_ => new DropDownDto()
                {
                    Id = _.Id,
                    Name = _.Name
                }).ToList();

                resultDto.SuccessDto.Response = zhList;
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


        #region National Head User List

        public ResultDto GetNationalHeadUserList(LoginUserIdDto inputDto)
        {
            _methodName = "GetNationalHeadUserList";
            var resultDto = new ResultDto();
            var outputDto = new List<DropDownDto>();
            try
            {
                IQueryable<Data.Entities.User> entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.User);
                entity = entity.Where(_ => _.IsActive
                //&& (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0)
                );

                outputDto = entity.Select(c => new DropDownDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        public ResultDto GetOilPackingTypeListWithAll()
        {
            _methodName = "GetOilPackingTypeList";
            var resultDto = new ResultDto();
            var oilPackingTypeDto = new List<OilPackingTypeDto>();
            try
            {
                oilPackingTypeDto = _emamiContext.OilPackingTypes.AsNoTracking().Select(_ => new OilPackingTypeDto { Id = _.Id, Name = _.Name }).ToList();

                var allItem = new OilPackingTypeDto();
                allItem.Id = 0;
                allItem.Name = "All";
                oilPackingTypeDto.Insert(0, allItem);


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = oilPackingTypeDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetPlantBasedOnStateId(IdInputDto inputDto)
        {
            _methodName = "GetPlantBasedOnStateId";
            var resultDto = new ResultDto();
            try
            {
                var plantResult = _emamiContext.Depots.AsNoTracking().Where(w => /*w.StateId == inputDto.Id &&*/ w.IsActive && w.IsPlant);

                var plantList = plantResult.Select(s => new DropDownDto()
                {
                    Id = s.Id,
                    Name = s.Name
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

        #region Competitor
        public ResultDto SaveCompititor(CompetitorDto competitorDto)
        {
            _methodName = "SaveCompititor";
            var resultDto = new ResultDto();
            try
            {
                if (competitorDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                var nameValidate = _emamiContext.Competitor.AsNoTracking().FirstOrDefault(f => f.Name == competitorDto.Name);
                if (nameValidate != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.CompetitorNameExists;
                    return resultDto;
                }

                var competitorData = new Competitor()
                {
                    Name = competitorDto.Name,
                    ZoneId = competitorDto.ZoneId,
                    StateId = competitorDto.StateId,
                    //TerritoryId = competitorDto.TerritoryId,
                    //DistrictId = competitorDto.DistrictId,
                    //CityId = competitorDto.CityId,
                    //Pincode = competitorDto.Pincode,
                    Address = competitorDto.Address,
                    IsActive = competitorDto.IsActive,
                    CreatedBy = competitorDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.Competitor.Add(competitorData);
                _emamiContext.SaveChanges();

                if (competitorDto.SelectedSkuIds != null && competitorDto.SelectedSkuIds.Any())
                {
                    foreach (var comp in competitorDto.SelectedSkuIds)
                    {
                        var competitorSku = new CompetitorSku()
                        {
                            CompetitorId = competitorData.Id,
                            SkuId = comp,
                            CreatedBy = competitorDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.CompetitorSku.Add(competitorSku);
                    }
                    _emamiContext.SaveChanges();
                }
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto UpdateCompititors(CompetitorDto competitorDto)
        {
            _methodName = "UpdateCompititors";
            var resultDto = new ResultDto();
            var newCompetitorSku = new List<long>();
            try
            {
                if (competitorDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(competitorDto.EncryptedId))
                {
                    var decryptedId = UtilityHelper.ConvertMd5ToString(competitorDto.EncryptedId, SecurityConstants.EncryptionKey);

                    competitorDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }
                var nameValidate = _emamiContext.Competitor.AsNoTracking().FirstOrDefault(f => f.Name == competitorDto.Name && f.Id != competitorDto.Id);
                if (nameValidate != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.CompetitorNameExists;
                    return resultDto;
                }

                var competitorData = _emamiContext.Competitor.FirstOrDefault(f => f.Id == competitorDto.Id);
                competitorData.Name = competitorDto.Name;
                competitorData.StateId = competitorDto.StateId;
                competitorData.ZoneId = competitorDto.ZoneId;
                competitorData.TerritoryId = competitorDto.TerritoryId;
                competitorData.DistrictId = competitorDto.DistrictId;
                competitorData.CityId = competitorDto.CityId;
                competitorData.Pincode = competitorDto.Pincode;
                competitorData.Address = competitorDto.Address;
                competitorData.IsActive = competitorDto.IsActive;
                competitorData.ModifiedBy = competitorDto.LoginUserId;
                competitorData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                _emamiContext.SaveChanges();

                var competitorSkuIds = _emamiContext.CompetitorSku.AsNoTracking().Where(w => w.CompetitorId == competitorDto.Id).Select(s => s.SkuId).AsEnumerable();
                if (competitorSkuIds != null && competitorSkuIds.Any()
                    && competitorDto.SelectedSkuIds != null && competitorDto.SelectedSkuIds.Any())
                    newCompetitorSku = competitorDto.SelectedSkuIds.Where(c => !competitorSkuIds.Contains(c)).ToList();
                else
                    newCompetitorSku = competitorDto.SelectedSkuIds;

                if (newCompetitorSku != null && newCompetitorSku.Any())
                {

                    if (newCompetitorSku != null && newCompetitorSku.Any())
                    {
                        foreach (var comp in newCompetitorSku)
                        {
                            var competitorSku = new CompetitorSku()
                            {
                                CompetitorId = competitorData.Id,
                                SkuId = comp,
                                CreatedBy = competitorDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.CompetitorSku.Add(competitorSku);
                        }
                        _emamiContext.SaveChanges();
                    }
                }

                //old remove
                if (competitorDto.RemovedSkuIds != null && competitorDto.RemovedSkuIds.Any())
                {
                    foreach (var skuId in competitorDto.RemovedSkuIds)
                    {
                        var competitorSkuData = _emamiContext.CompetitorSku.FirstOrDefault(f => f.CompetitorId == competitorDto.Id && f.SkuId == skuId);
                        if (competitorData != null)
                        {
                            _emamiContext.CompetitorSku.Remove(competitorSkuData);
                        }
                    }
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }


        public ResultDto GetCompititorById(string competitorId)
        {
            _methodName = "GetCompititorById";
            var resultDto = new ResultDto();
            var competitor = new CompetitorDto();

            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(competitorId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var competitorData = _emamiContext.Competitor.AsNoTracking().FirstOrDefault(f => f.Id == Id);
                if (competitorData != null)
                {
                    competitor = new CompetitorDto()
                    {
                        EncryptedId = competitorId,
                        Id = competitorData.Id,
                        Name = competitorData.Name,
                        ZoneId = competitorData.ZoneId,
                        StateId = competitorData.StateId,
                        TerritoryId = competitorData.TerritoryId,
                        DistrictId = competitorData.DistrictId,
                        CityId = competitorData.CityId,
                        Pincode = competitorData.Pincode,
                        Address = competitorData.Address,
                        IsActive = competitorData.IsActive
                    };
                    competitor.SelectedSkuIds = _emamiContext.CompetitorSku.AsNoTracking().Where(w => w.CompetitorId == Id).Select(s => s.SkuId).ToList();
                    if (competitor.SelectedSkuIds != null && competitor.SelectedSkuIds.Any())
                    {
                        competitor.SelectedOilTypeIds = _emamiContext.Skus.AsNoTracking().Where(w => competitor.SelectedSkuIds.Any(a => a == w.Id)).Select(s => s.OilType.Id).ToList();
                        competitor.SelectedSkuIdsCount = competitor.SelectedSkuIds.Count;
                    }

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = competitor;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetCompititors(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetCompititors";
            var resultDto = new ResultDto();
            List<Competitor> competitors;
            var competitorList = new List<CompetitorDto>();
            try
            {
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    competitors = _emamiContext.Competitor.AsNoTracking().ToList();
                }
                else
                {
                    competitors = _emamiContext.Competitor.AsNoTracking().Where(w => w.IsActive).ToList();
                }

                if (competitors != null && competitors.Any())
                {
                    competitorList = competitors
                        .GroupJoin(_emamiContext.CompetitorSku.AsEnumerable().Where(_ => _.Sku != null), c => c.Id, cs => cs.CompetitorId, (c, cs) => new { c, Skus = cs.Select(_ => _.Sku.SkuCode) }).ToList().AsEnumerable()
                        .Select(s => new CompetitorDto()
                        {
                            EncryptedId = UtilityHelper.ConvertToMd5(s.c.Id.ToString(), SecurityConstants.EncryptionKey),
                            Id = s.c.Id,
                            Name = s.c.Name,
                            ZoneId = s.c.ZoneId,
                            ZoneName = s.c.Zone != null ? s.c.Zone.Name : string.Empty,
                            StateName = s.c.State != null ? s.c.State.StateName : string.Empty,
                            //TerritoryName = s.Territory != null ? s.Territory.Name : string.Empty,
                            //DistrictName = s.District != null ? s.District.DistrictName : string.Empty,
                            //CityName = s.City != null ? s.City.CityName : string.Empty,
                            Pincode = s.c.Pincode,
                            Address = s.c.Address,
                            IsActive = s.c.IsActive,
                            MappedSkus = string.Join(",", s.Skus),
                        }).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = competitorList;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetSkuBasedOnOilTypes(CompetitorSkuInputDto inputDto)
        {
            _methodName = "GetSkuBasedOnOilTypes";
            var resultDto = new ResultDto();
            IQueryable<Data.Entities.Sku> skuEntity;
            var skuData = new List<SkuDto>();
            try
            {
                if (inputDto.OilTypeIds != null && inputDto.OilTypeIds.Any())
                {
                    if (inputDto.IsToReturnInactiveData)
                        skuEntity = _emamiContext.Skus.AsNoTracking().Where(w => inputDto.OilTypeIds.Any(a => a == w.OilTypeId));
                    else
                        skuEntity = _emamiContext.Skus.AsNoTracking().Where(w => inputDto.OilTypeIds.Any(a => a == w.OilTypeId) && w.IsActive);

                    if (skuEntity != null && skuEntity.Any())
                    {
                        skuData = skuEntity.Select(s => new SkuDto()
                        {
                            Id = s.Id,
                            SkuCode = s.SkuCode,
                            SkuName = s.SkuName,
                            //DepotCode = s.Depot != null ? s.Depot.Code : string.Empty,
                            //DepotName = s.Depot != null ? s.Depot.Name : string.Empty,
                            OilType = s.OilType != null ? s.OilType.Name : string.Empty,
                            IsActive = s.IsActive
                        }).ToList();
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = skuData;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto ExportCompetitor(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportCompetitor";
            var resultDto = new ResultDto();
            var competitorList = new List<CompetitorDto>();
            try
            {
                competitorList = _emamiContext.Competitor.AsNoTracking()
                        .GroupJoin(_emamiContext.CompetitorSku.AsNoTracking().Where(_ => _.Sku != null), c => c.Id, cs => cs.CompetitorId, (c, cs) => new { c, Skus = cs.Select(_ => _.Sku.SkuCode) }).ToList()
                        .Select(s => new CompetitorDto()
                        {
                            Id = s.c.Id,
                            Name = s.c.Name,
                            ZoneId = s.c.ZoneId,
                            ZoneName = s.c.Zone != null ? s.c.Zone.Name : string.Empty,
                            StateName = s.c.State != null ? s.c.State.StateName : string.Empty,
                            Pincode = s.c.Pincode,
                            Address = s.c.Address,
                            IsActive = s.c.IsActive,
                            MappedSkus = string.Join(",", s.Skus),
                        }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = competitorList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetCompetitorListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetCompetitorListWithPagination";
            var resultDto = new ResultDto();
            var outputDto = new List<CompetitorDto>();
            try
            {
                List<Competitor> competitors;
                if (inputDto.IsToReturnInactiveData)
                {
                    competitors = _emamiContext.Competitor.AsNoTracking().ToList();
                }
                else
                {
                    competitors = _emamiContext.Competitor.AsNoTracking().Where(w => w.IsActive).ToList();
                }

                if (competitors != null && competitors.Any())
                {
                    outputDto = competitors
                        .GroupJoin(_emamiContext.CompetitorSku.AsQueryable().Where(_ => _.Sku != null), c => c.Id, cs => cs.CompetitorId, (c, cs) => new { c, Skus = cs.Select(_ => _.Sku.SkuCode) }).ToList().AsQueryable()
                        .Select(s => new CompetitorDto()
                        {
                            EncryptedId = UtilityHelper.ConvertToMd5(s.c.Id.ToString(), SecurityConstants.EncryptionKey),
                            Id = s.c.Id,
                            Name = s.c.Name,
                            ZoneId = s.c.ZoneId,
                            ZoneName = s.c.Zone != null ? s.c.Zone.Name : string.Empty,
                            StateName = s.c.State != null ? s.c.State.StateName : string.Empty,
                            Pincode = s.c.Pincode,
                            Address = s.c.Address,
                            IsActive = s.c.IsActive,
                            MappedSkus = string.Join(",", s.Skus),
                        }).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }


        public ResultDto GetCompetitorList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetCompetitorList";
            try
            {
                var competitorListDto = new List<CompetitorListDto>();
                if (loginUserIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (loginUserIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                IQueryable<Competitor> competitorListContext = _emamiContext.Competitor.AsNoTracking().Where(_ => _.StateId == userContext.StateId && _.IsActive);
                if (competitorListContext != null && competitorListContext.Any())
                {
                    competitorListDto = competitorListContext.Select(_ => new CompetitorListDto
                    {
                        CompetitorId = _.Id,
                        Name = _.Name,
                        State = _.State != null ? _.State.StateName : String.Empty,
                    }).ToList();
                }
                if (competitorListDto != null && competitorListDto.Any())
                {
                    return _resultService.SuccessObject(competitorListDto);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
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

        public ResultDto GetSkuBasedOnCombination(LoginUserIdDto inputDto)
        {
            _methodName = "GetSkuBasedOnCombination";
            var resultDto = new ResultDto();
            var skuData = new List<DropDownDto>();
            try
            {
                if (inputDto.LoginUserId > 0)
                {
                    IEnumerable<DivisionDetailsDto> combinationlist = _emamiContext.UserDivisionMappings.AsNoTracking().Where(s => s.UserId == inputDto.LoginUserId).Select(s => new DivisionDetailsDto
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.DivisionId
                    });


                    var skuList = (from s in _emamiContext.Skus.AsNoTracking()
                                   join divm in combinationlist on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId } equals
                                   new { SalesOrganizationId = divm.SalesOrganizationId, DistributionChannelId = divm.DistributionChannelId, DivisionId = divm.DivisionId }
                                   join salesorg in _emamiContext.SalesOrganization.AsNoTracking() on s.SalesOrganizationId equals salesorg.Id
                                   join distchl in _emamiContext.DistributionChannel.AsNoTracking() on s.DistributionChannelId equals distchl.Id
                                   join div in _emamiContext.Divisions.AsNoTracking() on s.DivisionId equals div.Id
                                   where s.IsActive
                                   select new
                                   {
                                       SalesOrganizationId = s.SalesOrganizationId,
                                       DistributionChannelId = s.DistributionChannelId,
                                       DivisionId = s.DivisionId,
                                       SkuId = s.Id,
                                       SkuName = s.SkuName + "/" + s.SkuCode /* + "-" + s.PackGroup.Name*/,
                                       SkuCode = s.SkuCode,
                                   }).ToList();


                    if (skuList != null && skuList.Any())
                    {
                        if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                        {
                            skuData = skuList.Where(s => s.SalesOrganizationId == inputDto.SalesOrganizationId && s.DistributionChannelId ==
                            inputDto.DistributionChannelId && s.DivisionId == inputDto.DivisionId).Select(s => new DropDownDto()
                            {
                                Id = s.SkuId,
                                Code = s.SkuCode,
                                Name = s.SkuName,
                            }).ToList();
                        }
                        else
                        {
                            skuData = skuList.Select(s => new DropDownDto()
                            {
                                Id = s.SkuId,
                                Code = s.SkuCode,
                                Name = s.SkuName,
                            }).ToList();
                        }
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = skuData;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto SaudaBookingConfiguration(SaudaBookingConfigurationDto inputDto)
        {
            _methodName = "SaudaBookingConfiguration";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.StartDate != DateTime.MinValue)
                {
                    if (!string.IsNullOrEmpty(inputDto.EncryptedId))
                    {
                        var configurationId = Convert.ToInt64(UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey));

                        if (configurationId > 0)
                        {
                            var configContext = _emamiContext.SaudaBookingConfiguration.FirstOrDefault(config => config.Id == configurationId);

                            if (configContext != null)
                            {

                                bool isCombinationExists = false;

                                var configurationList = _emamiContext.SaudaBookingConfiguration
                                    .AsNoTracking()
                                    .Where(c => c.RoleId == inputDto.RoleId && c.Id != configContext.Id && c.IsActive)
                                    .ToList();

                                foreach (var item in configurationList)
                                {
                                    var oiltypeList = item.OilTypeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                                    var userIdList = item.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                                    bool oilMatch = false;
                                    bool userMatch = false;

                                    switch ((DTO.Enums.Role)inputDto.RoleId)
                                    {
                                        case DTO.Enums.Role.Dealer:
                                            userMatch = userIdList.Intersect(inputDto.UserIdsForDistributor).Any();
                                            break;
                                        case DTO.Enums.Role.ZonalTrader:
                                            oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                            userMatch = userIdList.Intersect(inputDto.UserIdsForZonalTrader).Any();
                                            break;
                                        case DTO.Enums.Role.StateTrader:
                                            oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                            userMatch = userIdList.Intersect(inputDto.UserIdsForStateTrader).Any();
                                            break;
                                    }

                                    if (inputDto.RoleId != (int)DTO.Enums.Role.Dealer)
                                    {
                                        if (oilMatch && userMatch)
                                        {
                                            isCombinationExists = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (userMatch)
                                        {
                                            isCombinationExists = true;
                                            break;
                                        }
                                    }
                                }

                                if (isCombinationExists)
                                {
                                    return _resultService.ErrorMessage(Constants.SaudaBookingConfigurationCombinationExits);
                                }

                                configContext.StartDate = inputDto.StartDate;
                                configContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                configContext.ModifiedBy = inputDto.LoginUserId;
                                configContext.IsActive = configContext.RoleId == (int)DTO.Enums.Role.Dealer
                                    ? inputDto.DealerIsActive
                                    : inputDto.IsActive;

                                configContext.OilTypeIds = inputDto.OilTypeIds.IsAny()
                                    ? string.Join(",", inputDto.OilTypeIds)
                                    : string.Empty;

                                if (configContext.RoleId == (int)DTO.Enums.Role.Dealer)
                                {
                                    configContext.UserIds = inputDto.UserIdsForDistributor.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForDistributor)
                                        : string.Empty;
                                }
                                else if (configContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                                {
                                    configContext.UserIds = inputDto.UserIdsForStateTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForStateTrader)
                                        : string.Empty;
                                }
                                else if (configContext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                                {
                                    configContext.UserIds = inputDto.UserIdsForZonalTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForZonalTrader)
                                        : string.Empty;
                                }

                                _emamiContext.SaveChanges();
                            }
                        }

                        resultDto = _resultService.SuccessMessage(Constants.SaudaBookingConfigurationUpdate);
                        return resultDto;
                    }
                    else
                    {
                        bool isCombinationExists = false;

                        var configurationList = _emamiContext.SaudaBookingConfiguration
                            .AsNoTracking()
                            .Where(c => c.RoleId == inputDto.RoleId && c.IsActive)
                            .ToList();

                        foreach (var item in configurationList)
                        {
                            var oiltypeList = item.OilTypeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                            var userIdList = item.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                            bool oilMatch = false;
                            bool userMatch = false;

                            switch ((DTO.Enums.Role)inputDto.RoleId)
                            {
                                case DTO.Enums.Role.Dealer:
                                    userMatch = userIdList.Intersect(inputDto.UserIdsForDistributor).Any();
                                    break;
                                case DTO.Enums.Role.ZonalTrader:
                                    oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                    userMatch = userIdList.Intersect(inputDto.UserIdsForZonalTrader).Any();
                                    break;
                                case DTO.Enums.Role.StateTrader:
                                    oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                    userMatch = userIdList.Intersect(inputDto.UserIdsForStateTrader).Any();
                                    break;
                            }

                            if (inputDto.RoleId != (int)DTO.Enums.Role.Dealer)
                            {
                                if (oilMatch && userMatch)
                                {
                                    isCombinationExists = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (userMatch)
                                {
                                    isCombinationExists = true;
                                    break;
                                }
                            }
                        }

                        if (isCombinationExists)
                        {
                            return _resultService.ErrorMessage(Constants.SaudaBookingConfigurationCombinationExits);
                        }

                        var configData = new SaudaBookingConfiguration()
                        {
                            RoleId = inputDto.RoleId,
                            IsActive = inputDto.RoleId == (int)DTO.Enums.Role.Dealer
                                ? inputDto.DealerIsActive
                                : inputDto.IsActive,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            StartDate = inputDto.StartDate,
                            OilTypeIds = inputDto.OilTypeIds.IsAny()
                                ? string.Join(",", inputDto.OilTypeIds)
                                : string.Empty,
                            UserIds =
                                inputDto.RoleId == (int)DTO.Enums.Role.Dealer
                                    ? inputDto.UserIdsForDistributor.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForDistributor)
                                        : string.Empty
                                : inputDto.RoleId == (int)DTO.Enums.Role.StateTrader
                                    ? inputDto.UserIdsForStateTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForStateTrader)
                                        : string.Empty
                                : inputDto.RoleId == (int)DTO.Enums.Role.ZonalTrader
                                    ? inputDto.UserIdsForZonalTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForZonalTrader)
                                        : string.Empty
                                : string.Empty
                        };

                        _emamiContext.SaudaBookingConfiguration.Add(configData);
                        _emamiContext.SaveChanges();
                    }
                }

                resultDto = _resultService.SuccessMessage(Constants.SaudaBookingConfiguration);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }

            return resultDto;
        }

        public ResultDto SaudaBookingConfigurationForMobile(SaudaBookingConfigurationDto inputDto)
        {
            _methodName = "SaudaBookingConfigurationForMobile";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.StartDate != DateTime.MinValue)
                {
                    if (inputDto.Id != 0)
                    {

                        if (inputDto.Id > 0)
                        {
                            var configContext = _emamiContext.SaudaBookingConfiguration.FirstOrDefault(config => config.Id == inputDto.Id);

                            if (configContext != null)
                            {

                                bool isCombinationExists = false;

                                var configurationList = _emamiContext.SaudaBookingConfiguration
                                    .AsNoTracking()
                                    .Where(c => c.RoleId == inputDto.RoleId && c.Id != configContext.Id && c.IsActive)
                                    .ToList();

                                foreach (var item in configurationList)
                                {
                                    var oiltypeList = item.OilTypeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                                    var userIdList = item.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                                    bool oilMatch = false;
                                    bool userMatch = false;

                                    switch ((DTO.Enums.Role)inputDto.RoleId)
                                    {
                                        case DTO.Enums.Role.Dealer:
                                            userMatch = userIdList.Intersect(inputDto.UserIdsForDistributor).Any();
                                            break;
                                        case DTO.Enums.Role.ZonalTrader:
                                            oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                            userMatch = userIdList.Intersect(inputDto.UserIdsForZonalTrader).Any();
                                            break;
                                        case DTO.Enums.Role.StateTrader:
                                            oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                            userMatch = userIdList.Intersect(inputDto.UserIdsForStateTrader).Any();
                                            break;
                                    }

                                    if (inputDto.RoleId != (int)DTO.Enums.Role.Dealer)
                                    {
                                        if (oilMatch && userMatch)
                                        {
                                            isCombinationExists = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (userMatch)
                                        {
                                            isCombinationExists = true;
                                            break;
                                        }
                                    }
                                }

                                if (isCombinationExists)
                                {
                                    return _resultService.ErrorMessage(Constants.SaudaBookingConfigurationCombinationExits);
                                }

                                configContext.StartDate = inputDto.StartDate;
                                configContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                configContext.ModifiedBy = inputDto.LoginUserId;
                                configContext.IsActive = inputDto.IsActive;

                                configContext.OilTypeIds = inputDto.OilTypeIds.IsAny()
                                    ? string.Join(",", inputDto.OilTypeIds)
                                    : string.Empty;

                                if (configContext.RoleId == (int)DTO.Enums.Role.Dealer)
                                {
                                    configContext.UserIds = inputDto.UserIdsForDistributor.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForDistributor)
                                        : string.Empty;
                                }
                                else if (configContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                                {
                                    configContext.UserIds = inputDto.UserIdsForStateTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForStateTrader)
                                        : string.Empty;
                                }
                                else if (configContext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                                {
                                    configContext.UserIds = inputDto.UserIdsForZonalTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForZonalTrader)
                                        : string.Empty;
                                }

                                _emamiContext.SaveChanges();
                            }
                        }

                        resultDto = _resultService.SuccessMessage(Constants.SaudaBookingConfigurationUpdate);
                        return resultDto;
                    }
                    else
                    {
                        bool isCombinationExists = false;

                        var configurationList = _emamiContext.SaudaBookingConfiguration
                            .AsNoTracking()
                            .Where(c => c.RoleId == inputDto.RoleId && c.IsActive)
                            .ToList();

                        foreach (var item in configurationList)
                        {
                            var oiltypeList = item.OilTypeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                            var userIdList = item.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                            bool oilMatch = false;
                            bool userMatch = false;

                            switch ((DTO.Enums.Role)inputDto.RoleId)
                            {
                                case DTO.Enums.Role.Dealer:
                                    userMatch = userIdList.Intersect(inputDto.UserIdsForDistributor).Any();
                                    break;
                                case DTO.Enums.Role.ZonalTrader:
                                    oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                    userMatch = userIdList.Intersect(inputDto.UserIdsForZonalTrader).Any();
                                    break;
                                case DTO.Enums.Role.StateTrader:
                                    oilMatch = oiltypeList.Intersect(inputDto.OilTypeIds).Any();
                                    userMatch = userIdList.Intersect(inputDto.UserIdsForStateTrader).Any();
                                    break;
                            }

                            if (inputDto.RoleId != (int)DTO.Enums.Role.Dealer)
                            {
                                if (oilMatch && userMatch)
                                {
                                    isCombinationExists = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (userMatch)
                                {
                                    isCombinationExists = true;
                                    break;
                                }
                            }
                        }

                        if (isCombinationExists)
                        {
                            return _resultService.ErrorMessage(Constants.SaudaBookingConfigurationCombinationExits);
                        }

                        var configData = new SaudaBookingConfiguration()
                        {
                            RoleId = inputDto.RoleId,
                            IsActive = inputDto.IsActive,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            StartDate = inputDto.StartDate,
                            OilTypeIds = inputDto.OilTypeIds.IsAny()
                                ? string.Join(",", inputDto.OilTypeIds)
                                : string.Empty,
                            UserIds =
                                inputDto.RoleId == (int)DTO.Enums.Role.Dealer
                                    ? inputDto.UserIdsForDistributor.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForDistributor)
                                        : string.Empty
                                : inputDto.RoleId == (int)DTO.Enums.Role.StateTrader
                                    ? inputDto.UserIdsForStateTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForStateTrader)
                                        : string.Empty
                                : inputDto.RoleId == (int)DTO.Enums.Role.ZonalTrader
                                    ? inputDto.UserIdsForZonalTrader.IsAny()
                                        ? string.Join(",", inputDto.UserIdsForZonalTrader)
                                        : string.Empty
                                : string.Empty
                        };

                        _emamiContext.SaudaBookingConfiguration.Add(configData);
                        _emamiContext.SaveChanges();
                    }
                }

                resultDto = _resultService.SuccessMessage(Constants.SaudaBookingConfiguration);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }

            return resultDto;
        }

        public ResultDto SaudaBookingConfiguration_Old(SaudaBookingConfigurationDto inputDto)
        {
            _methodName = "SaudaBookingConfiguration";
            var result = new ResultDto();
            try
            {
                var configuration = _emamiContext.SaudaBookingConfiguration;
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                bool hasChanges = false;

                if (inputDto.StartDateForDistributor != DateTime.MinValue)
                {
                    var configContext = configuration.FirstOrDefault(Config => Config.RoleId == (int)DTO.Enums.Role.Dealer);

                    if (configContext != null)
                    {
                        configContext.StartDate = inputDto.StartDateForDistributor;
                        configContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        configContext.ModifiedBy = inputDto.LoginUserId;
                        configContext.IsActive = inputDto.DealerIsActive;
                        configContext.OilTypeIds = inputDto.OilTypeIdsForDistributor.IsAny() ? string.Join(",", inputDto.OilTypeIdsForDistributor) : string.Empty;
                        configContext.UserIds = inputDto.UserIdsForDistributor.IsAny() ? string.Join(",", inputDto.UserIdsForDistributor) : string.Empty;
                        //_emamiContext.SaveChanges();
                    }
                    else
                    {
                        var configdata = new SaudaBookingConfiguration()
                        {
                            RoleId = (int)DTO.Enums.Role.Dealer,
                            IsActive = inputDto.DealerIsActive,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            StartDate = inputDto.StartDateForDistributor,
                            OilTypeIds = inputDto.OilTypeIdsForDistributor.IsAny() ? string.Join(",", inputDto.OilTypeIdsForDistributor) : string.Empty,
                            UserIds = inputDto.UserIdsForDistributor.IsAny() ? string.Join(",", inputDto.UserIdsForDistributor) : string.Empty,
                        };
                        _emamiContext.SaudaBookingConfiguration.Add(configdata);
                        //_emamiContext.SaveChanges();
                    }
                    hasChanges = true;
                    result = _resultService.SuccessMessage(Constants.SaudaBookingConfiguration);
                }

                if (inputDto.StartDateForST != DateTime.MinValue)
                {
                    var configContext = configuration.FirstOrDefault(Config => Config.RoleId == (int)DTO.Enums.Role.StateTrader);

                    if (configContext != null)
                    {
                        configContext.StartDate = inputDto.StartDateForST;
                        configContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        configContext.ModifiedBy = inputDto.LoginUserId;
                        configContext.IsActive = inputDto.StateIsActive;
                        configContext.OilTypeIds = inputDto.OilTypeIdsForStateTrader.IsAny() ? string.Join(",", inputDto.OilTypeIdsForStateTrader) : string.Empty;
                        configContext.UserIds = inputDto.UserIdsForStateTrader.IsAny() ? string.Join(",", inputDto.UserIdsForStateTrader) : string.Empty;
                        //_emamiContext.SaveChanges();
                    }
                    else
                    {
                        var configdata = new SaudaBookingConfiguration()
                        {
                            RoleId = (int)DTO.Enums.Role.StateTrader,
                            IsActive = inputDto.StateIsActive,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            StartDate = inputDto.StartDateForST,
                            OilTypeIds = inputDto.OilTypeIdsForStateTrader.IsAny() ? string.Join(",", inputDto.OilTypeIdsForStateTrader) : string.Empty,
                            UserIds = inputDto.UserIdsForStateTrader.IsAny() ? string.Join(",", inputDto.UserIdsForStateTrader) : string.Empty,
                        };
                        _emamiContext.SaudaBookingConfiguration.Add(configdata);
                        //_emamiContext.SaveChanges();
                    }
                    hasChanges = true;
                    result = _resultService.SuccessMessage(Constants.SaudaBookingConfiguration);
                }

                if (inputDto.StartDateForZT != DateTime.MinValue)
                {
                    var configContext = configuration.FirstOrDefault(Config => Config.RoleId == (int)DTO.Enums.Role.ZonalTrader);

                    if (configContext != null)
                    {
                        configContext.StartDate = inputDto.StartDateForZT;
                        configContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        configContext.ModifiedBy = inputDto.LoginUserId;
                        configContext.IsActive = inputDto.ZonalIsActive;
                        configContext.OilTypeIds = inputDto.OilTypeIdsForZonalTrader.IsAny() ? string.Join(",", inputDto.OilTypeIdsForZonalTrader) : string.Empty;
                        configContext.UserIds = inputDto.UserIdsForZonalTrader.IsAny() ? string.Join(",", inputDto.UserIdsForZonalTrader) : string.Empty;
                        //_emamiContext.SaveChanges();
                    }
                    else
                    {
                        var configdata = new SaudaBookingConfiguration()
                        {
                            RoleId = (int)DTO.Enums.Role.ZonalTrader,
                            IsActive = inputDto.ZonalIsActive,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            StartDate = inputDto.StartDateForZT,
                            OilTypeIds = inputDto.OilTypeIdsForZonalTrader.IsAny() ? string.Join(",", inputDto.OilTypeIdsForZonalTrader) : string.Empty,
                            UserIds = inputDto.UserIdsForZonalTrader.IsAny() ? string.Join(",", inputDto.UserIdsForZonalTrader) : string.Empty
                        };
                        _emamiContext.SaudaBookingConfiguration.Add(configdata);
                        //_emamiContext.SaveChanges();
                    }
                    hasChanges = true;
                    result = _resultService.SuccessMessage(Constants.SaudaBookingConfiguration);
                }
                if (hasChanges)
                {
                    _emamiContext.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result = _resultService.ErrorMessage("Some error occurred, Please try again");
            }
            return result;
        }

        public ResultDto GetSaudaBookingConfigurationList_Old()
        {
            _methodName = "GetSaudaBookingConfigurationList";
            var resultDto = new ResultDto();
            var suadaConfig = new SaudaBookingConfigurationDto();
            try
            {
                var configuration = _emamiContext.SaudaBookingConfiguration.AsNoTracking();

                suadaConfig.StartDateForDistributor = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer) != null ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer).StartDate.Value : DateTime.MinValue;
                suadaConfig.StartDateForST = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader) != null ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader).StartDate.Value : DateTime.MinValue;
                suadaConfig.StartDateForZT = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader) != null ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader).StartDate.Value : DateTime.MinValue;
                suadaConfig.OilTypeIdsForDistributor = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer) != null ? (!string.IsNullOrEmpty(configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer).OilTypeIds) ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer).OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>()) : new List<long>();
                suadaConfig.OilTypeIdsForStateTrader = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader) != null ? (!string.IsNullOrEmpty(configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader).OilTypeIds) ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader).OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>()) : new List<long>();
                suadaConfig.OilTypeIdsForZonalTrader = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader) != null ? (!string.IsNullOrEmpty(configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader).OilTypeIds) ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader).OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>()) : new List<long>();
                suadaConfig.UserIdsForDistributor = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer) != null ? (!string.IsNullOrEmpty(configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer).UserIds) ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer).UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>()) : new List<long>();
                suadaConfig.UserIdsForStateTrader = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader) != null ? (!string.IsNullOrEmpty(configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader).UserIds) ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader).UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>()) : new List<long>();
                suadaConfig.UserIdsForZonalTrader = configuration.FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader) != null ? (!string.IsNullOrEmpty(configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader).UserIds) ? configuration.AsNoTracking().FirstOrDefault(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader).UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>()) : new List<long>();
                suadaConfig.ZonalIsActive = configuration.Any(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.ZonalTrader);
                suadaConfig.StateIsActive = configuration.Any(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                suadaConfig.DealerIsActive = configuration.Any(_ => _.IsActive && _.RoleId == (int)DTO.Enums.Role.Dealer);
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = suadaConfig;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudaBookingConfigurationDetails(string EncryptedId)
        {
            _methodName = "GetSaudaBookingConfigurationList";
            var resultDto = new ResultDto();
            var suadaConfig = new SaudaBookingConfigurationDto();
            try
            {
                if (string.IsNullOrEmpty(EncryptedId))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.IdEmpty;
                    return resultDto;
                }

                var configurationId = Convert.ToInt64(UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey));

                if (configurationId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaBookingConfig = _emamiContext.SaudaBookingConfiguration.AsNoTracking().FirstOrDefault(_ => _.Id == configurationId);

                if (saudaBookingConfig != null)
                {
                    suadaConfig.Id = saudaBookingConfig.Id;
                    suadaConfig.EncryptedId = UtilityHelper.ConvertToMd5(saudaBookingConfig.Id.ToString(), SecurityConstants.EncryptionKey);
                    suadaConfig.RoleId = saudaBookingConfig.RoleId;
                    suadaConfig.IsActive = saudaBookingConfig.IsActive;
                    suadaConfig.OilTypeIds = saudaBookingConfig.OilTypeIds != null ? saudaBookingConfig.OilTypeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList() : new List<long>();
                    suadaConfig.StartDate = (DateTime)saudaBookingConfig.StartDate;
                    suadaConfig.IsActive = saudaBookingConfig.IsActive;
                    suadaConfig.DealerIsActive = saudaBookingConfig.IsActive;

                    if (saudaBookingConfig.RoleId == (int)DTO.Enums.Role.Dealer)
                        suadaConfig.UserIdsForDistributor = saudaBookingConfig.UserIds != null ? saudaBookingConfig.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList() : new List<long>();
                    else if (saudaBookingConfig.RoleId == (int)DTO.Enums.Role.StateTrader)
                        suadaConfig.UserIdsForStateTrader = saudaBookingConfig.UserIds != null ? saudaBookingConfig.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList() : new List<long>();
                    else if (saudaBookingConfig.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                        suadaConfig.UserIdsForZonalTrader = saudaBookingConfig.UserIds != null ? saudaBookingConfig.UserIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList() : new List<long>();

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = suadaConfig;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto SaudaBookingConfigurationRolewise(UserInputDto inputDto)
        {
            _methodName = "SaudaBookingConfigurationRolewise";
            var result = new ResultDto();
            var resultdata = new SaudaBoookingConfig();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(ur => ur.UserId == inputDto.LoginUserId).RoleId;
                var saudaConfigDataList = _emamiContext.SaudaBookingConfiguration.Where(Config => Config.RoleId == userrole && Config.IsActive).ToList();
                var oilTypes = _emamiContext.OilTypes.AsNoTracking().Where(s => s.IsActive).ToList();

                if (userrole == (int)DTO.Enums.Role.Dealer)
                {
                    resultdata.IsActive = true;

                    var stIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == inputDto.LoginUserId).Select(s => s.UserId).ToList();
                    var ztIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => stIds.Contains(_.UserId)).Select(s => s.ReportingToUserId).ToList();

                    var stConfigDataList = _emamiContext.SaudaBookingConfiguration.Where(config => config.RoleId == (int)DTO.Enums.Role.StateTrader && config.IsActive).ToList();
                    var ztConfigDataList = _emamiContext.SaudaBookingConfiguration.Where(config => config.RoleId == (int)DTO.Enums.Role.ZonalTrader && config.IsActive).ToList();

                    foreach (var configdata in saudaConfigDataList)
                    {
                        // Dealer as a login user
                       
                        if (configdata != null && configdata.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                        {
                            var configuredUserIds = !string.IsNullOrEmpty(configdata.UserIds) ? configdata.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                            if (DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= configdata.StartDate.Value.TimeOfDay)
                            {
                                if (configuredUserIds.Any() && configuredUserIds.Contains(inputDto.LoginUserId))
                                {
                                    resultdata.IsActive = false;  // sauad booking restricted
                                    resultdata.Message = "The distributor is restricted.";
                                    result.IsSuccess = true;
                                    result.SuccessDto.Response = resultdata;
                                    return result;
                                }
                            }
                        }
                    }

                  
                    foreach (var stConfigData in stConfigDataList)
                    {
                        if (stConfigData != null && stConfigData.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                        {
                            var configuredUserIds = !string.IsNullOrEmpty(stConfigData.UserIds) ? stConfigData.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();
                            var matchedIds = stIds.Intersect(configuredUserIds).ToList();
                            var userDivMapData = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => matchedIds.Contains(_.UserId) &&
                            _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId &&
                            _.DivisionId == inputDto.DivisionId).Select(s => new
                            {
                                s.SalesOrganizationId,
                                s.DistributionChannelId,
                                s.DivisionId
                            }).Distinct().ToList();

                            if (inputDto.SkuId != 0 && DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= stConfigData.StartDate.Value.TimeOfDay)
                            {
                                var configuredOilTypes = !string.IsNullOrEmpty(stConfigData.OilTypeIds) ? stConfigData.OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                                var oilTypeData = _emamiContext.Skus.AsNoTracking().Join(_emamiContext.OilTypes.AsNoTracking(), s => s.OilTypeId,
                                    o => o.Id, (s, o) => new { skus = s, oilTypes = o }).Where(_ => _.skus.Id == inputDto.SkuId &&
                                 _.skus.SalesOrganizationId == inputDto.SalesOrganizationId && _.skus.DistributionChannelId == inputDto.DistributionChannelId &&
                                 _.skus.DivisionId == inputDto.DivisionId).Select(_ => _.oilTypes.Id).FirstOrDefault();
                                if (configuredOilTypes.Contains(oilTypeData) && userDivMapData.Count > 0)
                                {
                                    resultdata.IsActive = false; // sauda booking restricted
                                    string oiltypeName = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Id == oilTypeData).Select(_ => _.Name).FirstOrDefault();
                                    resultdata.Message = $"Sauda time is over for this {oiltypeName} Oil. You may still book other oiltype.";
                                    result.IsSuccess = true;
                                    result.SuccessDto.Response = resultdata;
                                    return result;
                                }
                            }
                        }
                    }

                    foreach (var ztConfigData in ztConfigDataList)
                    {
                        if (ztConfigData != null && ztConfigData.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                        {
                            var configuredUserIds = !string.IsNullOrEmpty(ztConfigData.UserIds) ? ztConfigData.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();
                            var macthedIds = ztIds.Intersect(configuredUserIds).ToList();
                            var userDivMapData = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => macthedIds.Contains(_.UserId) &&
                            _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId &&
                            _.DivisionId == inputDto.DivisionId).Select(s => new
                            {
                                s.SalesOrganizationId,
                                s.DistributionChannelId,
                                s.DivisionId
                            }).Distinct().ToList();

                            if (inputDto.SkuId != 0 && DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= ztConfigData.StartDate.Value.TimeOfDay)
                            {
                                var configuredOilTypes = !string.IsNullOrEmpty(ztConfigData.OilTypeIds) ? ztConfigData.OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                                var oilTypeData = _emamiContext.Skus.AsNoTracking().Join(_emamiContext.OilTypes.AsNoTracking(), s => s.OilTypeId,
                                    o => o.Id, (s, o) => new { skus = s, oilTypes = o }).Where(_ => _.skus.Id == inputDto.SkuId &&
                                 _.skus.SalesOrganizationId == inputDto.SalesOrganizationId && _.skus.DistributionChannelId == inputDto.DistributionChannelId &&
                                 _.skus.DivisionId == inputDto.DivisionId).Select(_ => _.oilTypes.Id).FirstOrDefault();
                                if (configuredOilTypes.Contains(oilTypeData) && userDivMapData.Count > 0)
                                {
                                    resultdata.IsActive = false; // sauda booking restricted
                                    string oiltypeName = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Id == oilTypeData).Select(_ => _.Name).FirstOrDefault();
                                    resultdata.Message = $"Sauda time is over for this {oiltypeName} Oil. You may still book other oiltype.";
                                    result.IsSuccess = true;
                                    result.SuccessDto.Response = resultdata;
                                    return result;
                                }
                            }
                        }
                    }

                    result.IsSuccess = true;
                    result.SuccessDto.Response = resultdata;
                }
                else if (userrole == (int)DTO.Enums.Role.StateTrader)
                {
                    // StateTrader as a login user
                    resultdata.IsActive = true;
                    var ztIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).Select(s => s.ReportingToUserId).ToList();

                    var ztConfigDataList = _emamiContext.SaudaBookingConfiguration.Where(config => config.RoleId == (int)DTO.Enums.Role.ZonalTrader && config.IsActive).ToList();
                    var dealerConfigDataList = _emamiContext.SaudaBookingConfiguration.Where(config => config.RoleId == (int)DTO.Enums.Role.Dealer && config.IsActive).ToList();

                    foreach (var configdata in saudaConfigDataList)
                    {
                        if (configdata != null && configdata.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                        {
                            if (DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= configdata.StartDate.Value.TimeOfDay)
                            {
                                var configuredUserIds = !string.IsNullOrEmpty(configdata.UserIds) ? configdata.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                                if (inputDto.SkuId != 0)
                                {
                                    var configuredOilTypes = !string.IsNullOrEmpty(configdata.OilTypeIds) ? configdata.OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                                    var oilTypeData = _emamiContext.Skus.AsNoTracking().Join(_emamiContext.OilTypes.AsNoTracking(), s => s.OilTypeId,
                                        o => o.Id, (s, o) => new { skus = s, oilTypes = o }).Where(_ => _.skus.Id == inputDto.SkuId &&
                                     _.skus.SalesOrganizationId == inputDto.SalesOrganizationId && _.skus.DistributionChannelId == inputDto.DistributionChannelId &&
                                     _.skus.DivisionId == inputDto.DivisionId).Select(_ => _.oilTypes.Id).FirstOrDefault();
                                    if (configuredOilTypes.Contains(oilTypeData) && configuredUserIds.Any() && configuredUserIds.Contains(inputDto.LoginUserId))
                                    {
                                        resultdata.IsActive = false; // sauda booking restricted
                                        string oiltypeName = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Id == oilTypeData).Select(_ => _.Name).FirstOrDefault();
                                        resultdata.Message = $"Sauda time is over for this {oiltypeName} Oil. You may still book other oiltype.";
                                        result.IsSuccess = true;
                                        result.SuccessDto.Response = resultdata;
                                        return result;
                                    }
                                }
                            }
                        }
                    }

                    foreach (var ztConfigData in ztConfigDataList)
                    {
                        if (ztConfigData != null && ztConfigData.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                        {
                            var ztConfiguredUserIds = !string.IsNullOrEmpty(ztConfigData.UserIds) ? ztConfigData.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();
                            var macthedIds = ztIds.Intersect(ztConfiguredUserIds).ToList();
                            var userDivMapData = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => macthedIds.Contains(_.UserId) &&
                            _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId &&
                            _.DivisionId == inputDto.DivisionId).Select(s => new
                            {
                                s.SalesOrganizationId,
                                s.DistributionChannelId,
                                s.DivisionId
                            }).Distinct().ToList();

                            if (inputDto.SkuId != 0 && DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= ztConfigData.StartDate.Value.TimeOfDay)
                            {
                                var configuredOilTypes = !string.IsNullOrEmpty(ztConfigData.OilTypeIds) ? ztConfigData.OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                                var oilTypeData = _emamiContext.Skus.AsNoTracking().Join(_emamiContext.OilTypes.AsNoTracking(), s => s.OilTypeId,
                                    o => o.Id, (s, o) => new { skus = s, oilTypes = o }).Where(_ => _.skus.Id == inputDto.SkuId &&
                                 _.skus.SalesOrganizationId == inputDto.SalesOrganizationId && _.skus.DistributionChannelId == inputDto.DistributionChannelId &&
                                 _.skus.DivisionId == inputDto.DivisionId).Select(_ => _.oilTypes.Id).FirstOrDefault();
                                if (configuredOilTypes.Contains(oilTypeData) && userDivMapData.Count > 0)
                                {
                                    resultdata.IsActive = false; // sauda booking restricted
                                    string oiltypeName = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Id == oilTypeData).Select(_ => _.Name).FirstOrDefault();
                                    resultdata.Message = $"Sauda time is over for this {oiltypeName} Oil. You may still book other oiltype.";
                                    result.IsSuccess = true;
                                    result.SuccessDto.Response = resultdata;
                                    return result;
                                }
                            }
                        }
                    }

                    foreach (var dealerConfigData in dealerConfigDataList)
                    {
                        if (inputDto.DealerId != 0 && dealerConfigData != null && dealerConfigData.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                        {
                            var dConfiguredUserIds = !string.IsNullOrEmpty(dealerConfigData.UserIds) ? dealerConfigData.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();
                            if (DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= dealerConfigData.StartDate.Value.TimeOfDay && dConfiguredUserIds.Any() && dConfiguredUserIds.Contains(inputDto.DealerId))
                            {
                                resultdata.IsActive = false; // sauda booking restricted
                                resultdata.Message = "The distributor is restricted.";

                                result.IsSuccess = true;
                                result.SuccessDto.Response = resultdata;
                                return result;
                            }
                        }
                    }

                    result.IsSuccess = true;
                    result.SuccessDto.Response = resultdata;
                    return result;
                }
                else if (userrole == (int)DTO.Enums.Role.ZonalTrader)
                {
                    // ZonalTrader as a login user
                    resultdata.IsActive = true;

                    var stConfigDataList = _emamiContext.SaudaBookingConfiguration.Where(config => config.RoleId == (int)DTO.Enums.Role.StateTrader && config.IsActive).ToList();
                    var dealerConfigDataList = _emamiContext.SaudaBookingConfiguration.Where(config => config.RoleId == (int)DTO.Enums.Role.Dealer && config.IsActive).ToList();

                    foreach (var configdata in saudaConfigDataList)
                    {
                        if (configdata != null && configdata.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                        {
                            var configuredUserIds = !string.IsNullOrEmpty(configdata.UserIds) ? configdata.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                            if (DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= configdata.StartDate.Value.TimeOfDay)
                            {
                                if (inputDto.SkuId != 0)
                                {
                                    var configuredOilTypes = !string.IsNullOrEmpty(configdata.OilTypeIds) ? configdata.OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                                    var oilTypeData = _emamiContext.Skus.AsNoTracking().Join(_emamiContext.OilTypes.AsNoTracking(), s => s.OilTypeId,
                                        o => o.Id, (s, o) => new { skus = s, oilTypes = o }).Where(_ => _.skus.Id == inputDto.SkuId &&
                                     _.skus.SalesOrganizationId == inputDto.SalesOrganizationId && _.skus.DistributionChannelId == inputDto.DistributionChannelId &&
                                     _.skus.DivisionId == inputDto.DivisionId).Select(_ => _.oilTypes.Id).FirstOrDefault();
                                    if (configuredOilTypes.Contains(oilTypeData) && configuredUserIds.Any() && configuredUserIds.Contains(inputDto.LoginUserId))
                                    {
                                        resultdata.IsActive = false; // sauda booking restricted
                                        string oiltypeName = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Id == oilTypeData).Select(_ => _.Name).FirstOrDefault();
                                        resultdata.Message = $"Sauda time is over for this {oiltypeName} Oil. You may still book other oiltype.";
                                        result.IsSuccess = true;
                                        result.SuccessDto.Response = resultdata;
                                        return result;
                                    }
                                }
                            }
                        }
                    }

                    if (inputDto.StateTraderId != 0)
                    {
                        foreach (var stConfigData in stConfigDataList)
                        {
                            if (stConfigData != null && stConfigData.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                            {
                                var stConfiguredUserIds = !string.IsNullOrEmpty(stConfigData.UserIds) ? stConfigData.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();
                                var stIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(s => s.UserId).ToList();

                                var matchedIds = stIds.Intersect(stConfiguredUserIds).ToList();
                                var userDivMapDataSt = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => matchedIds.Contains(_.UserId) &&
                                _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId &&
                                _.DivisionId == inputDto.DivisionId).Select(s => new
                                {
                                    s.SalesOrganizationId,
                                    s.DistributionChannelId,
                                    s.DivisionId
                                }).Distinct().ToList();

                                if (inputDto.SkuId != 0 && DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= stConfigData.StartDate.Value.TimeOfDay)
                                {
                                    var configuredOilTypes = !string.IsNullOrEmpty(stConfigData.OilTypeIds) ? stConfigData.OilTypeIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();

                                    var oilTypeData = _emamiContext.Skus.AsNoTracking().Join(_emamiContext.OilTypes.AsNoTracking(), s => s.OilTypeId,
                                        o => o.Id, (s, o) => new { skus = s, oilTypes = o }).Where(_ => _.skus.Id == inputDto.SkuId &&
                                     _.skus.SalesOrganizationId == inputDto.SalesOrganizationId && _.skus.DistributionChannelId == inputDto.DistributionChannelId &&
                                     _.skus.DivisionId == inputDto.DivisionId).Select(_ => _.oilTypes.Id).FirstOrDefault();
                                    if (configuredOilTypes.Contains(oilTypeData) && userDivMapDataSt.Count > 0 && stConfiguredUserIds.Contains(inputDto.StateTraderId))
                                    {
                                        resultdata.IsActive = false; // sauda booking restricted
                                        string oiltypeName = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.Id == oilTypeData).Select(_ => _.Name).FirstOrDefault();
                                        resultdata.Message = $"Sauda time is over for this {oiltypeName} Oil. You may still book other oiltype.";
                                        result.IsSuccess = true;
                                        result.SuccessDto.Response = resultdata;
                                        return result;
                                    }
                                }
                            }
                        }

                        foreach (var dealerConfigData in dealerConfigDataList)
                        {
                            if (inputDto.DealerId != 0 && dealerConfigData != null && dealerConfigData.StartDate.Value.Date == DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                            {
                                var dConfiguredUserIds = !string.IsNullOrEmpty(dealerConfigData.UserIds) ? dealerConfigData.UserIds.Split(',').ToList().ConvertAll(long.Parse) : new List<long>();
                                if (DateHelper.UtcToIndia(DateTime.UtcNow).TimeOfDay >= dealerConfigData.StartDate.Value.TimeOfDay &&
                                    dConfiguredUserIds.Any() && dConfiguredUserIds.Contains(inputDto.DealerId))
                                {
                                    resultdata.IsActive = false; // sauda booking restricted
                                    resultdata.Message = "The distributor is restricted.";

                                    result.IsSuccess = true;
                                    result.SuccessDto.Response = resultdata;
                                    return result;
                                }
                            }
                        }
                    }

                    result.IsSuccess = true;
                    result.SuccessDto.Response = resultdata;
                }
                else
                {
                    resultdata.IsActive = true;  // sauad booking allowed

                    result.IsSuccess = true;
                    result.SuccessDto.Response = resultdata;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result = _resultService.ErrorMessage("Some error occurred, Please try again");
            }
            return result;
        }
        public ResultDto GetSkuDataWithLiftingandDoNumber(LiftingSkuInputDto inputDto)
        {
            _methodName = "GetSkuDataWithLiftingandDoNumber";
            var resultDto = new ResultDto();
            var output = new List<TrackSkuOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.TripNotCreated);
                }
                if (!inputDto.DoNumbers.Any())
                {
                    return _resultService.ErrorMessage(Constants.TripNotCreated);
                }
                var salescontext = _emamiContext.SalesRegister.AsNoTracking();
                var skucontext = _emamiContext.Skus.AsNoTracking();
                var oiltypecontext = _emamiContext.OilTypes.AsNoTracking();

                var donumbers = inputDto.DoNumbers.Select(s => s.DoNumber).ToList();
                foreach (var donumber in donumbers)
                {
                    output.Add(salescontext.Where(_ => _.DeliveryNumber != null).ToList().Where(_ => _.DeliveryNumber == donumber)
                    .Select(s => new TrackSkuOutputDto()
                    {
                        LiftingRequestId = s.Id,
                        DoNumber = s.DeliveryNumber,
                        BillingNumber = s.InvoiceNumber,
                        BillingDate = s.InvoiceDate,
                        ShipToParty = s.ShiptoParty
                    }).FirstOrDefault());
                }



                output.ForEach(f =>
                {
                    f.Materials = salescontext.Where(_ => _.DeliveryNumber == f.DoNumber).Select(s => new TrackSkuDto()
                    {
                        SkuName = skucontext.FirstOrDefault(_ => _.Id == s.SkuId) != null ? skucontext.FirstOrDefault(_ => _.Id == s.SkuId).SkuName : string.Empty,
                        Quantity = s.QuantityMT,
                        QuantityCase = s.QuantityCase,
                        ShipToParty = s.ShiptoParty,
                        OilTypeName = skucontext.FirstOrDefault(_ => _.Id == s.SkuId) != null ? oiltypecontext.FirstOrDefault(o => o.Id == skucontext.FirstOrDefault(_ => _.Id == s.SkuId).OilTypeId) != null ? oiltypecontext.FirstOrDefault(o => o.Id == skucontext.FirstOrDefault(_ => _.Id == s.SkuId).OilTypeId).Name : string.Empty : string.Empty
                    }).ToList();
                });


                foreach (var donumber in inputDto.DoNumbers)
                {
                    if (donumber.Status.ToLower() == "completed")
                    {
                        var completedo = new CompletedDoNumber()
                        {
                            DoNumber = donumber.DoNumber,
                            CreatedDate = DateTime.Now,
                        };
                        _emamiContext.CompletedDoNumbers.Add(completedo);
                        _emamiContext.SaveChanges();
                    }
                }

                _logger.Info($" SKU Data : {JsonHelper.ConvertObjectToJson(output)}");
                resultDto.SuccessDto.Response = output;
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

        #region GamificationDashboard

        public ResultDto GetGamificationDashboardList(string inputDto)
        {
            _methodName = "GetGamificationDashboardList";
            var resultDto = new ResultDto();
            var gamificationDashboard = new GamificationDashboardDto();
            try
            {
                var Id = Convert.ToInt64(inputDto);

                var resultContext = _emamiContext.GamificationDashboards.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    gamificationDashboard.Id = resultContext.Id;
                    //gamificationDashboard.DistributorCode = _emamiContext.DistributionChannel.FirstOrDefault(dis => dis.Code == resultContext.DistributorCode).Name ?? "DistributorCode not found";
                    gamificationDashboard.DistributorCode = _emamiContext.Users.FirstOrDefault(us => us.Code == resultContext.DistributorCode).Name ?? "DistributorCode not found";
                    gamificationDashboard.DistributorTargetMT = resultContext.DistributorTargetMT;
                    gamificationDashboard.DistributorAchievementTillN1MT = resultContext.DistributorAchievementTillN1MT;
                    gamificationDashboard.RemainingTargetToAchieveMT = resultContext.RemainingTargetToAchieveMT;
                    gamificationDashboard.EarnedPoints = resultContext.EarnedPoints;
                    gamificationDashboard.CurrentSlab = resultContext.CurrentSlab;
                    gamificationDashboard.NextHigherSlab = resultContext.NextHigherSlab;
                    gamificationDashboard.PointsToBeEarnedToReachNextHigherSlab = resultContext.PointsToBeEarnedToReachNextHigherSlab;
                    gamificationDashboard.TotalEarningsInRs = resultContext.TotalEarningsInRs;
                    gamificationDashboard.SpecialBonusMessage = resultContext.SpecialBonusMessage;
                    //gamificationDashboard.WholePointsStructure = resultContext.WholePointsStructure;
                    gamificationDashboard.IsActive = resultContext.IsActive;
                    gamificationDashboard.IsDiamond = resultContext.IsDiamond;
                    //materialCapacityAllocation.MaterialName = _emamiContext.Skus.FirstOrDefault(sku => sku.SkuCode == resultContext.MaterialCode).SkuName ?? "Material not found";

                }

                return _resultService.SuccessObject(gamificationDashboard);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetGamificationDashboardWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetGamificationDashboardWithPagination";
            var resultDto = new ResultDto();
            var outputDto = new List<GamificationDashboardDto>();
            DataSourceResult result = new DataSourceResult();
            try
            {
                var gamificationData = _emamiContext.GamificationDashboards.AsNoTracking()
                      .Select(_ => new GamificationDashboardDto
                      {

                          Id = _.Id,
                          DistributorCode = _emamiContext.Users.FirstOrDefault(us => us.Code == _.DistributorCode).Name ?? "DistributorCode not found",
                          DistributorTargetMT = _.DistributorTargetMT,
                          DistributorAchievementTillN1MT = _.DistributorAchievementTillN1MT,
                          RemainingTargetToAchieveMT = _.RemainingTargetToAchieveMT,
                          EarnedPoints = _.EarnedPoints,
                          CurrentSlab = _.CurrentSlab,
                          NextHigherSlab = _.NextHigherSlab,
                          PointsToBeEarnedToReachNextHigherSlab = _.PointsToBeEarnedToReachNextHigherSlab,
                          TotalEarningsInRs = _.TotalEarningsInRs,
                          SpecialBonusMessage = _.SpecialBonusMessage,
                          //WholePointsStructure = _.WholePointsStructure,
                          IsActive = _.IsActive,
                          IsDiamond = _.IsDiamond
                      }).AsQueryable();


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = gamificationData != null ? gamificationData.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : result;
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
        /// Add or Update Oil Type
        /// </summary>
        /// <param name="materialCapacityAllocationDto"></param>
        /// <returns></returns>
        public ResultDto AddOrUpdateGamificationDashboardDetails(GamificationDashboardDto gamificationDashboardDto)
        {
            _methodName = "AddOrUpdateGamificationDashboardDetails";
            var resultDto = new ResultDto();
            var gamificationDashboard = new GamificationDashboard();

            try
            {
                if (gamificationDashboardDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if ((gamificationDashboardDto.Id == 0))
                {
                    var isExists = _emamiContext.GamificationDashboards.AsNoTracking().Where(_ => _.DistributorCode == gamificationDashboardDto.DistributorCode).FirstOrDefault();

                    if (isExists != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.GamificationDashboard;
                        return resultDto;
                    }

                    gamificationDashboard.DistributorCode = gamificationDashboardDto.DistributorCode;
                    gamificationDashboard.DistributorTargetMT = gamificationDashboardDto.DistributorTargetMT;
                    gamificationDashboard.DistributorAchievementTillN1MT = gamificationDashboardDto.DistributorAchievementTillN1MT;
                    gamificationDashboard.RemainingTargetToAchieveMT = gamificationDashboardDto.RemainingTargetToAchieveMT;
                    gamificationDashboard.EarnedPoints = gamificationDashboardDto.EarnedPoints;
                    gamificationDashboard.CurrentSlab = gamificationDashboardDto.CurrentSlab;
                    gamificationDashboard.NextHigherSlab = gamificationDashboardDto.NextHigherSlab;
                    gamificationDashboard.PointsToBeEarnedToReachNextHigherSlab = gamificationDashboardDto.PointsToBeEarnedToReachNextHigherSlab;
                    gamificationDashboard.TotalEarningsInRs = gamificationDashboardDto.TotalEarningsInRs;
                    gamificationDashboard.SpecialBonusMessage = gamificationDashboardDto.SpecialBonusMessage;
                    //gamificationDashboard.WholePointsStructure = gamificationDashboardDto.WholePointsStructure;
                    gamificationDashboard.IsActive = gamificationDashboardDto.IsActive;
                    gamificationDashboard.IsDiamond = gamificationDashboardDto.IsDiamond;

                    _emamiContext.GamificationDashboards.Add(gamificationDashboard);

                    resultDto.SuccessDto.Message = "Add Successfully";
                }
                else
                {

                    var isExists = _emamiContext.GamificationDashboards.AsNoTracking().Where(_ => _.DistributorCode == gamificationDashboardDto.DistributorCode).FirstOrDefault();

                    if (isExists != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.GamificationDashboard;
                        return resultDto;
                    }

                    gamificationDashboard = _emamiContext.GamificationDashboards.FirstOrDefault(_ => _.Id == gamificationDashboardDto.Id);
                    //gamificationDashboard.DistributorCode = gamificationDashboardDto.DistributorCode;
                    //gamificationDashboard.DistributorAchievementTillN1MT = gamificationDashboardDto.DistributorAchievementTillN1MT;
                    //gamificationDashboard.RemainingTargetToAchieveMT = gamificationDashboardDto.RemainingTargetToAchieveMT;
                    //gamificationDashboard.EarnedPoints = gamificationDashboardDto.EarnedPoints;
                    //gamificationDashboard.CurrentSlab = gamificationDashboardDto.CurrentSlab;
                    //gamificationDashboard.NextHigherSlab = gamificationDashboardDto.NextHigherSlab;
                    //gamificationDashboard.PointsToBeEarnedToReachNextHigherSlab = gamificationDashboardDto.PointsToBeEarnedToReachNextHigherSlab;
                    //gamificationDashboard.TotalEarningsInRs = gamificationDashboardDto.TotalEarningsInRs;
                    //gamificationDashboard.SpecialBonusMessage = gamificationDashboardDto.SpecialBonusMessage;
                    //gamificationDashboard.WholePointsStructure = gamificationDashboardDto.WholePointsStructure;
                    gamificationDashboard.IsActive = gamificationDashboardDto.IsActive;
                    gamificationDashboard.IsDiamond = gamificationDashboardDto.IsDiamond;

                    resultDto.SuccessDto.Message = "Update Successfully";
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

        #endregion

        #region SaudaSalesAreaRestrictionConfiguration

        public ResultDto SaudaSalesAreaRestrictionConfiguration(SaudaSalesAreaRestrictionDto inputDto)
        {
            _methodName = "SaudaSalesAreaRestrictionConfiguration";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.ValidFrom != DateTime.MinValue && inputDto.ValidTo != DateTime.MinValue)
                {
                    if (!string.IsNullOrEmpty(inputDto.EncryptedId))
                    {
                        var configurationId = Convert.ToInt64(UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey));

                        if (configurationId > 0)
                        {
                            var configContext = _emamiContext.SaudaSalesAreaRestrictions.FirstOrDefault(config => config.Id == configurationId);

                            if (configContext != null)
                            {

                                bool isCombinationExists = false;

                                var configurationList = _emamiContext.SaudaSalesAreaRestrictions
                                                        .AsNoTracking()
                                                        .Where(c => c.SalesOrganizationId == inputDto.SalesOrganizationId && c.DistributionChannelId == inputDto.DistributionChannelId
                                                        && c.DivisionId == inputDto.DivisionId && c.IsActive && c.Id != configurationId)
                                                        .ToList();

                                if (configurationList.Any())
                                    isCombinationExists = true;

                                if (isCombinationExists)
                                {
                                    return _resultService.ErrorMessage(Constants.SaudaBookingConfigurationCombinationExits);
                                }

                                configContext.SalesOrganizationId = inputDto.SalesOrganizationId;
                                configContext.DistributionChannelId = inputDto.DistributionChannelId;
                                configContext.DivisionId = inputDto.DivisionId;
                                configContext.TimeRestriction = inputDto.TimeRestriction;
                                configContext.ValidFrom = inputDto.ValidFrom;
                                configContext.ValidTo = inputDto.ValidTo;
                                configContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                configContext.ModifiedBy = inputDto.LoginUserId;
                                configContext.IsActive = true;

                                _emamiContext.SaveChanges();
                            }
                        }

                        resultDto = _resultService.SuccessMessage(Constants.SaudaSalesAreaRestricitonConfigurationUpdate);
                        return resultDto;
                    }
                    else
                    {
                        bool isCombinationExists = false;

                        var configurationList = _emamiContext.SaudaSalesAreaRestrictions
                            .AsNoTracking()
                            .Where(c => c.SalesOrganizationId == inputDto.SalesOrganizationId && c.DistributionChannelId == inputDto.DistributionChannelId 
                            && c.DivisionId == inputDto.DivisionId && c.IsActive)
                            .ToList();

                        if(configurationList.Any())
                            isCombinationExists = true;

                        if (isCombinationExists)
                        {
                            return _resultService.ErrorMessage(Constants.SaudaSalesAreaRestrictionConfigurationCombinationExits);
                        }

                        var configData = new SaudaSalesAreaRestriction()
                        {
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            DivisionId = inputDto.DivisionId,
                            TimeRestriction = inputDto.TimeRestriction,
                            ValidFrom = inputDto.ValidFrom,
                            ValidTo = inputDto.ValidTo,
                            IsActive = true,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };

                        _emamiContext.SaudaSalesAreaRestrictions.Add(configData);
                        _emamiContext.SaveChanges();
                    }
                }

                resultDto = _resultService.SuccessMessage(Constants.SaudaSalesAreaRestricitonConfiguration);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }

            return resultDto;
        }

        public ResultDto GetSaudaSalesAreaRestrictionConfigurationDetails(string EncryptedId)
        {
            _methodName = "GetSaudaSalesAreaRestrictionConfigurationDetails";
            var resultDto = new ResultDto();
            var suadaConfig = new SaudaSalesAreaRestrictionDto();
            try
            {
                if (string.IsNullOrEmpty(EncryptedId))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.IdEmpty;
                    return resultDto;
                }

                var configurationId = Convert.ToInt64(UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey));

                if (configurationId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaBookingConfig = _emamiContext.SaudaSalesAreaRestrictions.AsNoTracking().FirstOrDefault(_ => _.Id == configurationId);

                if (saudaBookingConfig != null)
                {
                    suadaConfig.Id = saudaBookingConfig.Id;
                    suadaConfig.EncryptedId = UtilityHelper.ConvertToMd5(saudaBookingConfig.Id.ToString(), SecurityConstants.EncryptionKey);
                    suadaConfig.SalesOrganizationId = saudaBookingConfig.SalesOrganizationId;
                    suadaConfig.DistributionChannelId = saudaBookingConfig.DistributionChannelId;
                    suadaConfig.DivisionId = saudaBookingConfig.DivisionId;
                    suadaConfig.TimeRestriction = saudaBookingConfig.TimeRestriction;
                    suadaConfig.ValidFrom = saudaBookingConfig.ValidFrom;
                    suadaConfig.ValidTo = saudaBookingConfig.ValidTo;
                    suadaConfig.IsActive = saudaBookingConfig.IsActive;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = suadaConfig;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion
    }
}
