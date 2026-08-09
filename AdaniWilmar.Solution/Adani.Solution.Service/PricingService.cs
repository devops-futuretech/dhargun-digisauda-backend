using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Common;
using Adani.Solution.Service.Common;
using Dapper;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Adani.Solution.Service
{
    public interface IPricingService
    {
        ResultDto UpdateRoleBasedDiscount(RoleDiscountDto roleDiscountDto);
        ResultDto GetRoleBasedDiscounts(RoleDiscountDto roleDiscountDto);
        ResultDto GetRoleBasedDiscountById(RoleDiscountDto roleDiscountDto);
        ResultDto AddDiscount(SkuDepotDiscountDto skuDepotDiscountDto);
        ResultDto UpdateDiscount(SkuDepotDiscountDto skuDepotDiscountDto);
        ResultDto GetDiscountList(CustomerDiscountinputDto discountinputDto);
        ResultDto GetSkuDepotBasedDiscountById(CustomerDiscountinputDto customerDiscountinputDto);

        ResultDto GetOilTypeDetailsddl(OilTypeDto oilTypeDto);
        ResultDto GetDepotDetailsddl(DepotDto depotDto);
        ResultDto GetUserDetailsddl(LoginUserIdDto loginUserIdDto);
        ResultDto GetSkuDetailsddl(OilTypeDto oilTypeDto);

        //Role Discount
        ResultDto AddRoleDiscount(RoleDisocuntDto roleDisocuntDto);
        ResultDto UpdateRoleDiscount(RoleDisocuntDto roleDisocuntDto);
        ResultDto GetRoleDiscountbyId(RoleDisocuntInputDto roleDisocuntInputDto);
        ResultDto GetRoleDiscountsAll(RoleDisocuntInputDto roleDisocuntInputDto);

        //Request Discount
        ResultDto GetRequestDiscountbyId(RequestDisocuntInputDto requestDiscountDto);
        ResultDto UpdateRequestDiscount(RequestDisocuntUpdateDto requestDisocuntDto);
        ResultDto ApproveRequestedDiscount(ApproveRequestDiscountDto approveRequestDiscountDto);
        ResultDto GetRequestDiscountsAll(LoginUserIdDto loginUserIdDto);
        ResultDto GetRequestDiscountList(RequestDisocuntInputDto requestDisocuntInputDto);
        ResultDto GetRequestDiscountDetailsById(RequestDisocuntInputDto requestDisocuntInputDto);

        //Approve Discount
        ResultDto GetRequestedDiscounts(RequestDisocuntInputDto requestDisocuntInputDto);

        //Premium Discount
        ResultDto GetPremiumDiscountsAll(PremiumDisocuntInputDto premiumDisocuntInputDto);
        ResultDto GetPremiumDiscountbyId(PremiumDisocuntInputDto premiumDisocuntInputDto);
        ResultDto AddPremiumDiscount(PremiumDisocuntDto premiumDisocuntDto);
        ResultDto UpdatePremiumDiscount(PremiumDisocuntDto premiumDisocuntDto);

        //Request Premium Discount
        ResultDto GetPremiumRequestDiscountList(PremiumDisocuntRequestInputDto requestDisocuntInputDto);
        ResultDto UpdatePremiumRequestDiscount(PremiumDisocuntRequestDto requestDisocuntDto);
        ResultDto GetPremiumRequestDiscountDetailsById(PremiumDisocuntRequestInputDto requestDisocuntInputDto);
        ResultDto GetSkuPremiumDiscountRequestById(PremiumDisocuntRequestInputDto requestDisocuntInputDto);

        //Approve Pending Request
        ResultDto GetPremiumDiscountForPending(PremiumDisocuntRequestInputDto requestDisocuntInputDto);
        ResultDto UpdateApprovePremiumDiscount(ApprovePremiunDiscountRequestDto premiumDisocuntRequestInputDto);

        //Primary Discount User
        ResultDto AddPrimaryDiscountForUser(PrimaryDiscountUserDto primaryDiscountUserDto);
        ResultDto UpdatePrimaryDiscountForUser(PrimaryDiscountUserDto primaryDiscountUserDto);
        ResultDto GetPrimaryDiscountForUserList(PrimaryDiscountUserInputDto discountinputDto);
        ResultDto GetPrimaryDiscountForUserById(PrimaryDiscountUserInputDto discountinputDto);

        //Ra Margin
        ResultDto SaveRaMargin(RaMarginDto inputDto);
        ResultDto GetRaMarginList(LoginUserIdDto inputDto);
        ResultDto GetRaMarginListWithPaging(KendoGridResult inputDto);
        ResultDto ExportRaMargin(LoginUserIdDto inputDto);
        ResultDto GetRaMarginDetailsById(long raMarginId);
        ResultDto UpdateRaMargin(RaMarginDto inputDto);

        //New Discount
        ResultDto AddDiscountGeography(DiscountInputDto inputDto);
        ResultDto GetCityDetailsBasedOnTerritory(TerritoryId territoryId);
        ResultDto GetGeographyList(LoginUserIdDto inputDto);
        ResultDto GetGeographyCityList(GeographyCityListParam inputDto);
        ResultDto GetGeographyCityListMobile(GeographyDiscountCityListParam inputDto);
        ResultDto GetGeographyDetailsById(long inputDto);
        ResultDto UpdateDiscountGeography(DiscountInputDto inputDto);

        //Discount User
        ResultDto AddDiscountUsers(DiscountUserDto inputDto);
        ResultDto GetDiscountUserList(LoginUserIdDto inputDto);
        ResultDto DiscountUserExport(LoginUserIdDto inputDto);
        ResultDto GetDiscountUserById(long discountId);
        ResultDto UpdateDiscountUsers(DiscountUserDto inputDto);
        ResultDto GetDiscountUserDetailList(GeographyCityListParam inputDto);
        ResultDto GetEmployeeAndUserDiscountList(LoginUserIdDto inputDto);

        //PriceNotifyConfiguration
        ResultDto AddorUpdatePriceNotifyConfiguration(PriceNotifyConfigurationDto inputDto);
        ResultDto GetPriceNotifyConfigurationList(SaudaLimitInputDto inputDto);
        ResultDto GetPriceNotifyConfigurationCityList(IdInputDto inputDto);
        ResultDto GetPriceNotifyconfigurationDetailsById(long inputDto);
        ResultDto UpdatePriceNotifyconfiguration(PriceNotifyConfigurationDto inputDto);
        ResultDto GetEmployeeAndUserDiscountById(IdInputDto inputDto);
        ResultDto AddEmployeeAndUserDiscount(EmployeeUserDiscountDto inputDto);

        //SP Geography Discount
        ResultDto GetSpecialityFatGeographyList(LoginUserIdDto inputDto);
        ResultDto GetSpecialityFatGeographyCityList(GeographyCityListParam inputDto);
        ResultDto GetSpecialityFatGeographyDetailsById(long geographyId);
        ResultDto AddSpecialityFatDiscountGeography(SpecialityFatDiscountInputDto inputDto);
        ResultDto UpdateSpecialityFatDiscountGeography(SpecialityFatDiscountInputDto inputDto);
        ResultDto GetCityDetailsBasedOnTerritoryAndCity(TerritoryId inputDto);

        //SP User Discount
        ResultDto AddSpecialityFatDiscountUsers(SpecialityFatDiscountUserDto inputDto);
        ResultDto UpdateSpecialityFatDiscountUsers(SpecialityFatDiscountUserDto inputDto);
        ResultDto GetSpecialityFatDiscountUserList(LoginUserIdDto inputDto);
        ResultDto GetSpecialityFatDiscountUserExport(LoginUserIdDto inputDto);
        ResultDto GetSpecialityFatDiscountUserById(long discountId);
        ResultDto GetSpecialityFatDiscountUserDetailList(GeographyCityListParam inputDto);
        ResultDto GetSpecialityFatEmployeeDiscountList(LoginUserIdDto inputDto);
        ResultDto GetSpecialityFatEmployeeDiscountExport(LoginUserIdDto inputDto);

        ResultDto GetSpecialityFatEmployeeDiscountById(IdInputDto inputDto);
        ResultDto AddSpecialityFatEmployeeDiscount(SpecialityFatEmployeeDiscountDto inputDto);

        ResultDto AddSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto);
        ResultDto UpdateSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto);
        ResultDto GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(SpecialtyFatQuantityRequestSearchDto inputDto);
        ResultDto GetSpecialtyFatQuantityRequestsListForMobile(SpecialtyFatQuantityRequestSearchDto inputDto);
        ResultDto GetSpecialtyFatQuantityRequestsList(SpecialtyFatQuantityRequestSearchDto inputDto);
        ResultDto UpdateSpecialtyFatQuantityLimit(SpecialtyFatQuantityRequestDto inputDto);

        #region  Auto Allocation

        ResultDto GetAutoAllocationUserListByRoleIds(AutoAllocationInputDto inputDto);
        ResultDto GetAutoAllocationDetailsByUserId(AutoAllocationInputDto inputDto);
        ResultDto SaveSpecalityFatDiscountUsers(SaveAutoAllocationDetailDto inputDto);
        #endregion

        ////RAMaterialCost
        //ResultDto SaveRAMaterialCost(RAMaterialCostDto inputDto);
        //ResultDto GetRAMaterialCostListNew(KendoGridResult inputDto);
        //ResultDto GetRAMaterialCostDetailsById(long ramaterialCostId);
        //ResultDto UpdateRAMaterialCost(RAMaterialCostDto inputDto);
        ResultDto GetSpecialityFatDiscountEmployeeDetailList(GeographyCityListParam inputDto);

        //ResultDto GetMaterialCostListWithPagination(LoginUserIdDto inputDto);
        //ResultDto GetPriceDetails(PriceScreenInputDto inputDto);
    }

    public class PricingService : IPricingService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Pricing Service");
        private const string ServiceName = "Pricing Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public PricingService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
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

        #region User Role Discount

        public ResultDto UpdateRoleBasedDiscount(RoleDiscountDto roleDiscountDto)
        {
            _methodName = "UpdateRoleBasedDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var roleEntity = _emamiContext.Roles.Where(f => f.RoleTypeId == roleDiscountDto.RoleId).ToList();
                if (roleEntity != null)
                {
                    foreach (var item in roleEntity)
                    {
                        if (roleDiscountDto.VerticleId == (int)DTO.Enums.Division.Hbc)
                        {
                            //item.HbcDiscout = roleDiscountDto.Discount;
                        }
                        else
                        {
                            //item.SpecialityFatDiscount = roleDiscountDto.Discount;
                        }
                        item.ModifiedBy = roleDiscountDto.LoginUserId;
                        item.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetRoleBasedDiscounts(RoleDiscountDto roleDiscountDto)
        {
            _methodName = "GetRoleBasedDiscounts";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                IQueryable<Adani.Solution.Data.Entities.Role> roles;
                IList<RoleDiscountDto> roleList;
                if (roleDiscountDto.IsToReturnInactiveData)
                {
                    roles = _emamiContext.Roles.AsNoTracking().Where(w => w.IsActive);
                }
                else
                {
                    roles = _emamiContext.Roles.AsNoTracking();
                }

                if (roles != null && roles.Any())
                {
                    roleList = roles.Select(s => new RoleDiscountDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleTypeId,
                        RoleName = s.RoleType.Name,
                        //HbcDiscout = s.HbcDiscout,
                        //SpecialityFatDiscount = s.SpecialityFatDiscount,
                        IsActive = s.IsActive
                    }).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = roleList;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetRoleBasedDiscountById(RoleDiscountDto roleDiscountDto)
        {
            _methodName = "GetRoleBasedDiscountById";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var discountDto = new RoleDiscountDto();

                var discountEntity = _emamiContext.Roles.AsNoTracking().FirstOrDefault(w => w.Id == roleDiscountDto.Id);

                if (discountEntity != null)
                {
                    discountDto.Id = discountEntity.Id;
                    //discountDto.HbcDiscout = discountEntity.HbcDiscout;
                    //discountDto.SpecialityFatDiscount = discountEntity.SpecialityFatDiscount;
                    discountDto.RoleId = discountEntity.RoleTypeId;
                    discountDto.RoleName = discountEntity.RoleType.Name;
                    discountDto.IsActive = discountEntity.IsActive;
                    //if (discountEntity.HbcDiscout > 0)
                    //{
                    //    discountDto.Discount = discountEntity.HbcDiscout;
                    //}
                    //else if (discountEntity.SpecialityFatDiscount > 0)
                    //{
                    //    discountDto.Discount = discountEntity.SpecialityFatDiscount;
                    //}

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = discountDto;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Customer Discount

        public ResultDto AddDiscount(SkuDepotDiscountDto skuDepotDiscountDto)
        {
            _methodName = "AddDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                if (skuDepotDiscountDto != null)
                {
                    if (skuDepotDiscountDto.DiscountType == (int)DiscountTypes.Customer)
                    {
                        resultDto = AddCustomerDiscount(skuDepotDiscountDto);
                    }
                    else if (skuDepotDiscountDto.DiscountType == (int)DiscountTypes.Product)
                    {
                        resultDto = AddSkuDiscount(skuDepotDiscountDto);
                    }
                    else if (skuDepotDiscountDto.DiscountType == (int)DiscountTypes.Geography)
                    {
                        resultDto = AddGeographyDiscount(skuDepotDiscountDto);
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        public ResultDto AddCustomerDiscount(SkuDepotDiscountDto inputDto)
        {
            _methodName = "AddCustomerDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var entity = new DiscountUsers()
                {
                    SkuId = inputDto.SkuId,
                    //DepotId = inputDto.DepotId,
                    UserId = inputDto.CustomerId,
                    SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                    Status = true,
                    ActualDiscount = inputDto.ActualDiscount,
                    ValidFrom = inputDto.ValidFrom,
                    ValidTo = inputDto.ValidTo,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    OilTypeId = inputDto.OilTypeId,
                };
                _emamiContext.DiscountUsers.Add(entity);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto AddSkuDiscount(SkuDepotDiscountDto inputDto)
        {
            _methodName = "AddSkuDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var entity = new DiscountSku()
                {
                    SkuId = inputDto.SkuId,
                    ActualDiscount = inputDto.ActualDiscount,
                    SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                    ValidFrom = inputDto.ValidFrom,
                    ValidTo = inputDto.ValidTo,
                    Status = true,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    OilTypeId = inputDto.OilTypeId,
                };
                _emamiContext.DiscountSku.Add(entity);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto AddGeographyDiscount(SkuDepotDiscountDto inputDto)
        {
            _methodName = "AddGeographyDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var entity = new DiscountGeography()
                {
                    SkuId = inputDto.SkuId,
                    ZoneId = inputDto.ZoneId,
                    StateId = inputDto.StateId,
                    TerritoryId = inputDto.TerritoryId,
                    DistrictId = inputDto.DistrictId,
                    CityId = inputDto.CityId,
                    ActualDiscount = inputDto.ActualDiscount,
                    ValidFrom = inputDto.ValidFrom,
                    ValidTo = inputDto.ValidTo,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    OilTypeId = inputDto.OilTypeId,
                };
                _emamiContext.DiscountGeography.Add(entity);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto UpdateDiscount(SkuDepotDiscountDto skuDepotDiscountDto)
        {
            _methodName = "UpdateDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                if (skuDepotDiscountDto.DiscountType == (int)DiscountTypes.Customer)
                {
                    resultDto = UpdateCustomerDiscount(skuDepotDiscountDto);
                }
                else if (skuDepotDiscountDto.DiscountType == (int)DiscountTypes.Product)
                {
                    resultDto = UpdateSkuDiscount(skuDepotDiscountDto);
                }
                else if (skuDepotDiscountDto.DiscountType == (int)DiscountTypes.Geography)
                {
                    resultDto = UpdateGeographyDiscount(skuDepotDiscountDto);
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateCustomerDiscount(SkuDepotDiscountDto inputDto)
        {
            _methodName = "UpdateCustomerDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var discountEntity = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id);
                if (discountEntity != null)
                {
                    discountEntity.SkuId = inputDto.SkuId;
                    discountEntity.UserId = inputDto.CustomerId;
                    discountEntity.ActualDiscount = inputDto.ActualDiscount;
                    discountEntity.ValidFrom = inputDto.ValidFrom;
                    discountEntity.ValidTo = inputDto.ValidTo;
                    discountEntity.ModifiedBy = inputDto.LoginUserId;
                    discountEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    discountEntity.OilTypeId = inputDto.OilTypeId;
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

        public ResultDto UpdateSkuDiscount(SkuDepotDiscountDto inputDto)
        {
            _methodName = "UpdateSkuDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var disocuntSku = _emamiContext.DiscountSku.FirstOrDefault(f => f.Id == inputDto.Id);
                if (disocuntSku != null)
                {
                    disocuntSku.SkuId = inputDto.SkuId;
                    disocuntSku.ActualDiscount = inputDto.ActualDiscount;
                    disocuntSku.ValidFrom = inputDto.ValidFrom;
                    disocuntSku.ValidTo = inputDto.ValidTo;
                    disocuntSku.ModifiedBy = inputDto.LoginUserId;
                    disocuntSku.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    disocuntSku.OilTypeId = inputDto.OilTypeId;
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

        public ResultDto UpdateGeographyDiscount(SkuDepotDiscountDto inputDto)
        {
            _methodName = "UpdateGeographyDiscount";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var disocuntGeography = _emamiContext.DiscountGeography.FirstOrDefault(f => f.Id == inputDto.Id);
                if (disocuntGeography != null)
                {
                    disocuntGeography.SkuId = inputDto.SkuId;
                    disocuntGeography.ZoneId = inputDto.ZoneId;
                    disocuntGeography.StateId = inputDto.StateId;
                    disocuntGeography.TerritoryId = inputDto.TerritoryId;
                    disocuntGeography.DistrictId = inputDto.DistrictId;
                    disocuntGeography.CityId = inputDto.CityId;
                    disocuntGeography.ActualDiscount = inputDto.ActualDiscount;
                    disocuntGeography.ValidFrom = inputDto.ValidFrom;
                    disocuntGeography.ValidTo = inputDto.ValidTo;
                    disocuntGeography.ModifiedBy = inputDto.LoginUserId;
                    disocuntGeography.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    disocuntGeography.OilTypeId = inputDto.OilTypeId;

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

        public ResultDto GetDiscountList(CustomerDiscountinputDto discountinputDto)
        {
            _methodName = "GetDiscountList";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                if (discountinputDto.DiscountType == (int)DiscountTypes.Customer)
                {
                    resultDto = GetCustomerDiscountList(discountinputDto);
                }
                else if (discountinputDto.DiscountType == (int)DiscountTypes.Product)
                {
                    resultDto = GetSkuDiscountList(discountinputDto);
                }
                else if (discountinputDto.DiscountType == (int)DiscountTypes.Geography)
                {
                    resultDto = GetGeographyDiscountList(discountinputDto);
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetCustomerDiscountList(CustomerDiscountinputDto inputDto)
        {
            _methodName = "GetCustomerDiscountList";
            var resultDto = new ResultDto();
            try
            {
                var discountUsers = _emamiContext.DiscountUsers.AsNoTracking().ToList();
                var result = discountUsers.Select(s => new SkuDepotDiscountDto()
                {
                    Id = s.Id,
                    CustomerName = s.User?.Name,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeName = s.OilType?.Name,
                    //DepotName = s.Depot.Name,
                    ActualDiscount = s.ActualDiscount,
                    DiscountType = inputDto.DiscountType,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo
                });
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

        public ResultDto GetSkuDiscountList(CustomerDiscountinputDto inputDto)
        {
            _methodName = "GetSkuDiscountList";
            var resultDto = new ResultDto();
            try
            {
                var discountUsers = _emamiContext.DiscountSku.AsNoTracking().ToList();
                var result = discountUsers.Select(s => new SkuDepotDiscountDto()
                {
                    Id = s.Id,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeName = s.OilType.Name,
                    ActualDiscount = s.ActualDiscount,
                    DiscountType = inputDto.DiscountType,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo
                });
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

        public ResultDto GetGeographyDiscountList(CustomerDiscountinputDto inputDto)
        {
            _methodName = "GetGeographyDiscountList";
            var resultDto = new ResultDto();
            try
            {
                var discountUsers = _emamiContext.DiscountGeography.AsNoTracking().ToList();
                var result = discountUsers.Select(s => new SkuDepotDiscountDto()
                {
                    Id = s.Id,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeName = s.OilType.Name,
                    ActualDiscount = s.ActualDiscount,
                    DiscountType = inputDto.DiscountType,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                    StateName = _emamiContext.State.AsNoTracking().FirstOrDefault(f => f.Id == s.StateId).StateName,
                    DistrictName = _emamiContext.District.AsNoTracking().FirstOrDefault(f => f.Id == s.DistrictId).DistrictName,
                    CityName = _emamiContext.City.AsNoTracking().FirstOrDefault(f => f.Id == s.CityId).CityName,
                });

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

        public ResultDto GetSkuDepotBasedDiscountById(CustomerDiscountinputDto discountinputDto)
        {
            _methodName = "GetSkuDepotBasedDiscountById";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var discountDto = new SkuDepotDiscountDto();

                if (discountinputDto.DiscountType == (int)DiscountTypes.Customer)
                {
                    resultDto = GetCustomerDiscount(discountinputDto);
                }
                else if (discountinputDto.DiscountType == (int)DiscountTypes.Product)
                {
                    resultDto = GetSkuDiscount(discountinputDto);
                }
                else if (discountinputDto.DiscountType == (int)DiscountTypes.Geography)
                {
                    resultDto = GetGeographyDiscount(discountinputDto);
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

        public ResultDto GetCustomerDiscount(CustomerDiscountinputDto inputDto)
        {
            _methodName = "GetCustomerDiscount";
            var resultDto = new ResultDto();
            var discountDto = new SkuDepotDiscountDto();
            try
            {
                var discountEntity = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.Id);
                if (discountEntity != null)
                {
                    discountDto.Id = discountEntity.Id;
                    discountDto.DivisionId = discountEntity.OilType.DivisionId;
                    discountDto.OilTypeId = (int)discountEntity.OilTypeId;
                    discountDto.SkuId = discountEntity.SkuId;
                    //discountDto.DepotId = discountEntity.DepotId;
                    discountDto.CustomerId = discountEntity.UserId;
                    discountDto.ActualDiscount = discountEntity.ActualDiscount;
                    discountDto.DiscountType = inputDto.DiscountType;
                    discountDto.ValidFrom = discountEntity.ValidFrom;
                    discountDto.ValidTo = discountEntity.ValidTo;

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = discountDto;
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

        public ResultDto GetSkuDiscount(CustomerDiscountinputDto inputDto)
        {
            _methodName = "GetSkuDiscount";
            var resultDto = new ResultDto();
            var discountDto = new SkuDepotDiscountDto();
            try
            {
                var discountEntity = _emamiContext.DiscountSku.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.Id);
                if (discountEntity != null)
                {
                    discountDto.Id = discountEntity.Id;
                    discountDto.DivisionId = discountEntity.OilType.DivisionId;
                    discountDto.OilTypeId = (int)discountEntity.OilTypeId;
                    discountDto.SkuId = discountEntity.SkuId;
                    discountDto.ActualDiscount = discountEntity.ActualDiscount;
                    discountDto.DiscountType = inputDto.DiscountType;
                    discountDto.ValidFrom = discountEntity.ValidFrom;
                    discountDto.ValidTo = discountEntity.ValidTo;

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = discountDto;
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

        public ResultDto GetGeographyDiscount(CustomerDiscountinputDto inputDto)
        {
            _methodName = "GetGeographyDiscount";
            var resultDto = new ResultDto();
            var discountDto = new SkuDepotDiscountDto();
            try
            {
                var discountEntity = _emamiContext.DiscountGeography.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.Id);
                if (discountEntity != null)
                {
                    discountDto.Id = discountEntity.Id;
                    discountDto.DivisionId = discountEntity.OilType.DivisionId;
                    discountDto.OilTypeId = discountEntity.OilTypeId;
                    discountDto.SkuId = discountEntity.SkuId;
                    discountDto.ZoneId = discountEntity.ZoneId;
                    discountDto.StateId = discountEntity.StateId;
                    discountDto.TerritoryId = discountEntity.TerritoryId;
                    discountDto.DistrictId = discountEntity.DistrictId;
                    discountDto.CityId = discountEntity.CityId;
                    discountDto.ActualDiscount = discountEntity.ActualDiscount;
                    discountDto.DiscountType = inputDto.DiscountType;
                    discountDto.ValidFrom = discountEntity.ValidFrom;
                    discountDto.ValidTo = discountEntity.ValidTo;

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = discountDto;
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

        public ResultDto GetOilTypeDetailsddl(OilTypeDto oilTypeDto)
        {
            _methodName = "GetOilTypeDetailsddl";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<OilType> oiltype;
                if (oilTypeDto.IsToReturnInactiveData)
                {
                    oiltype = _emamiContext.OilTypes.AsNoTracking()
                  .Where(w => w.IsActive);
                }
                else
                {
                    oiltype = _emamiContext.OilTypes.AsNoTracking();
                }

                var oiltypeList = oiltype
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

        public ResultDto GetDepotDetailsddl(DepotDto depotDto)
        {
            _methodName = "GetDepotDetailsddl";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<Depot> depots;
                if (depotDto.IsToReturnActiveData)
                {
                    depots = _emamiContext.Depots.AsNoTracking()
                  .Where(w => w.IsActive);
                }
                else
                {
                    depots = _emamiContext.Depots.AsNoTracking();
                }

                var oiltypeList = depots
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

        public ResultDto GetUserDetailsddl(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetUserDetailsddl";
            var resultDto = new ResultDto();
            try
            {
                IQueryable<User> users;
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    users = _emamiContext.Users.AsNoTracking()
                  .Where(w => w.IsActive);
                }
                else
                {
                    users = _emamiContext.Users.AsNoTracking();
                }

                var oiltypeList = users
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

        public ResultDto GetSkuDetailsddl(OilTypeDto oilTypeDto)
        {
            _methodName = "GetSkuDetailsddl";
            var resultDto = new ResultDto();
            try
            {
                IList<DropDownDto> dropDownsDtos = GetSkuDetails(oilTypeDto);
                resultDto.SuccessDto.Response = dropDownsDtos;
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

        public List<DropDownDto> GetSkuDetails(OilTypeDto oilTypeDto)
        {
            _methodName = "GetSkuDetailsddl";
            var resultDto = new ResultDto();
            //&& w.sk.IsActive == oilTypeDto.IsToReturnInactiveData
            var dropDownsDtos = _emamiContext.OilTypes.AsNoTracking()
                        .Join(_emamiContext.Divisions.AsNoTracking(), ot => ot.DivisionId, ve => ve.Id, (ot, ve) => new { ot = ot, ve = ve })
                        .Join(_emamiContext.Skus.AsNoTracking(), v => v.ve.Id, sk => sk.DivisionId, (v, sk) => new { v = v, sk = sk })
                        .Where(w => w.v.ot.Id == oilTypeDto.SelectedOilTypeId)
                        .Select(s => new DropDownDto()
                        {
                            Id = s.sk.Id,
                            Name = s.sk.SkuName
                        }).ToList();

            return dropDownsDtos;
        }

        #endregion

        #region Role Discount

        public ResultDto AddRoleDiscount(RoleDisocuntDto roleDisocuntDto)
        {
            _methodName = "AddRoleDiscount";
            var resultDto = new ResultDto();
            try
            {
                if (roleDisocuntDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (roleDisocuntDto != null)
                {
                    foreach (var item in roleDisocuntDto.SkuDiscounts)
                    {
                        var nameValidation = _emamiContext.RoleDiscount.AsNoTracking()
                    .Count(c => c.SkuId == item.SkuDropDown.SkuId && c.RoleId == roleDisocuntDto.RoleId);
                        if (nameValidation > 0)
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = Constants.RoleDiscountSkuNameExists;
                            return resultDto;
                        }

                        var roleDisocunt = new RoleDiscount()
                        {
                            RoleId = roleDisocuntDto.RoleId,
                            SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                            Status = (int)DiscountStatus.Approved,
                            ApprovedBy = roleDisocuntDto.LoginUserId,
                            OilTypeId = roleDisocuntDto.OilTypeId,
                            SkuId = item.SkuDropDown.SkuId,
                            ActualDiscount = item.ActualDiscount,
                            ValidFrom = roleDisocuntDto.ValidFrom,
                            ValidTo = roleDisocuntDto.ValidTo,
                            CreatedBy = roleDisocuntDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.RoleDiscount.Add(roleDisocunt);
                    }
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
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

        public ResultDto UpdateRoleDiscount(RoleDisocuntDto roleDisocuntDto)
        {
            _methodName = "UpdateRoleDiscount";
            var resultDto = new ResultDto();
            try
            {
                if (roleDisocuntDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (roleDisocuntDto != null)
                {
                    foreach (var item in roleDisocuntDto.SkuDiscounts)
                    {
                        if (item.Id > 0)
                        {
                            var nameValidation = _emamiContext.RoleDiscount.AsNoTracking()
                    .Count(c => c.SkuId == item.SkuDropDown.SkuId && c.RoleId == roleDisocuntDto.RoleId && c.Id != item.Id);
                            if (nameValidation > 0)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = Constants.RoleDiscountSkuNameExists;
                                return resultDto;
                            }
                        }
                        else
                        {
                            var nameValidation = _emamiContext.RoleDiscount.AsNoTracking()
                    .Count(c => c.SkuId == item.SkuDropDown.SkuId && c.RoleId == roleDisocuntDto.RoleId);
                            if (nameValidation > 0)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = Constants.RoleDiscountSkuNameExists;
                                return resultDto;
                            }
                        }
                    }
                }

                if (roleDisocuntDto != null)
                {
                    foreach (var item in roleDisocuntDto.SkuDiscounts)
                    {
                        if (item.Id > 0)
                        {
                            var roleEntity = _emamiContext.RoleDiscount.FirstOrDefault(f => f.Id == item.Id);
                            roleEntity.ActualDiscount = item.ActualDiscount;
                            roleEntity.ModifiedBy = roleDisocuntDto.LoginUserId;
                            roleEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            _emamiContext.SaveChanges();
                        }
                        else
                        {
                            var roleDisocunt = new RoleDiscount()
                            {
                                RoleId = roleDisocuntDto.RoleId,
                                SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                Status = (int)DiscountStatus.Approved,
                                ApprovedBy = roleDisocuntDto.LoginUserId,
                                SkuId = item.SkuDropDown.SkuId,
                                OilTypeId = roleDisocuntDto.OilTypeId,
                                ActualDiscount = item.ActualDiscount,
                                ValidFrom = roleDisocuntDto.ValidFrom,
                                ValidTo = roleDisocuntDto.ValidTo,
                                CreatedBy = roleDisocuntDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.RoleDiscount.Add(roleDisocunt);
                        }
                    }
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
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

        public ResultDto GetRoleDiscountbyId(RoleDisocuntInputDto roleDisocuntInputDto)
        {
            _methodName = "GetRoleDiscountbyId";
            var resultDto = new ResultDto();
            var roleDisocuntData = new RoleDisocuntDto();
            var roleDiscountList = new List<SkuDiscounts>();

            try
            {
                var roleDiscounts = _emamiContext.RoleDiscount.AsNoTracking()
                    .Where(w => w.RoleId == roleDisocuntInputDto.RoleId && w.OilTypeId == roleDisocuntInputDto.OilTypeId).ToList();

                if (roleDiscounts != null && roleDiscounts.Any())
                {
                    roleDisocuntData.Id = roleDiscounts.FirstOrDefault().Id;
                    roleDisocuntData.RoleId = roleDiscounts.FirstOrDefault().RoleId;
                    roleDisocuntData.OilTypeId = roleDiscounts.FirstOrDefault().OilTypeId;
                    roleDisocuntData.VerticleId = roleDiscounts.FirstOrDefault().Sku.OilType.DivisionId;
                    roleDisocuntData.ValidFrom = roleDiscounts.FirstOrDefault().ValidFrom;
                    roleDisocuntData.ValidTo = roleDiscounts.FirstOrDefault().ValidTo;
                    foreach (var item in roleDiscounts)
                    {
                        SkuDropDown sku = new SkuDropDown();
                        sku.SkuId = item.SkuId;
                        sku.SkuName = item.Sku.SkuName;
                        var roleActualDiscount = new SkuDiscounts()
                        {
                            Id = item.Id,
                            SkuDropDown = sku,
                            ActualDiscount = item.ActualDiscount
                        };
                        roleDiscountList.Add(roleActualDiscount);
                    }
                    roleDisocuntData.SkuDiscounts = roleDiscountList;

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = roleDisocuntData;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetRoleDiscountsAll(RoleDisocuntInputDto roleDisocuntInputDto)
        {
            _methodName = "GetRoleDiscountsAll";
            var resultDto = new ResultDto();
            try
            {

                var roleDiscountList = _emamiContext.RoleDiscount.AsNoTracking()
                    .Where(w => w.Status == (int)DiscountStatus.Approved && w.RoleId == roleDisocuntInputDto.RoleId)
                    .Select(s => new RoleDisocuntDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleId,
                        RoleName = s.Role.Name,
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.Sku.OilType.Name,
                        SkuName = s.Sku.SkuName,
                        SaudaBookingTypeName = s.SaudaBookingType.Name,
                        ActualDiscount = s.ActualDiscount,
                        Status = s.Status,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleDiscountList;
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

        #region Request Discount

        public ResultDto GetRequestDiscountsAll(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetRequestDiscountsAll";
            var resultDto = new ResultDto();
            try
            {

                var roleDiscountList = _emamiContext.RoleDiscount.AsNoTracking()
                    .Where(w => w.Status == (int)DiscountStatus.Pending)
                    .GroupBy(g => g.RoleId)
                    .Select(s => new RequestDisocuntDto()
                    {
                        RoleId = s.Key,
                        RoleName = s.FirstOrDefault().Role.Name
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleDiscountList;
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

        public ResultDto GetRequestDiscountList(RequestDisocuntInputDto requestDisocuntInputDto)
        {
            _methodName = "GetRequestDiscountList";
            var resultDto = new ResultDto();
            try
            {

                var roleDiscountList = _emamiContext.RoleDiscount.AsNoTracking()
                    .Where(w => w.RoleId == requestDisocuntInputDto.RoleId && w.Status == (int)DiscountStatus.Pending)
                    .Select(s => new RequestDisocuntDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleId,
                        RoleName = s.Role.Name,
                        OilTypeName = s.OilType.Name,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        SaudaBookingTypeName = s.SaudaBookingType.Name,
                        ActualDiscount = s.ActualDiscount,
                        RequestedDiscount = s.RequestedDiscount,
                        Status = s.Status
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleDiscountList;
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

        public ResultDto GetRequestDiscountbyId(RequestDisocuntInputDto requestDiscountDto)
        {
            _methodName = "GetRequestDiscountbyId";
            var resultDto = new ResultDto();
            var requestDisocunt = new RequestDisocuntDto();

            try
            {
                var discounts = _emamiContext.RoleDiscount.AsNoTracking().FirstOrDefault(w => w.Id == requestDiscountDto.Id);

                if (discounts != null)
                {
                    requestDisocunt.Id = discounts.Id;
                    requestDisocunt.SkuId = discounts.SkuId;
                    requestDisocunt.OilTypeId = discounts.OilTypeId;
                    requestDisocunt.VerticleId = discounts.Sku.OilType.DivisionId;
                    requestDisocunt.RoleId = discounts.RoleId;
                    requestDisocunt.ActualDiscount = discounts.ActualDiscount;
                    requestDisocunt.RequestedDiscount = discounts.RequestedDiscount;
                    requestDisocunt.Status = discounts.Status;
                    requestDisocunt.ApprovedBy = discounts.ApprovedBy;

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = requestDisocunt;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateRequestDiscount(RequestDisocuntUpdateDto requestDisocuntDto)
        {
            _methodName = "UpdateRequestDiscount";
            var resultDto = new ResultDto();
            var requestDisocunt = new RequestDisocuntDto();

            try
            {
                var roleDiscount = _emamiContext.RoleDiscount.FirstOrDefault(f => f.Id == requestDisocuntDto.Id);

                if (roleDiscount != null)
                {
                    roleDiscount.RequestedDiscount = requestDisocuntDto.RequestedDiscount;
                    roleDiscount.Status = (int)DiscountStatus.Pending;
                    roleDiscount.ModifiedBy = requestDisocuntDto.LoginUserId;
                    roleDiscount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetRequestDiscountDetailsById(RequestDisocuntInputDto requestDisocuntInputDto)
        {
            _methodName = "GetRequestDiscountDetailsById";
            var resultDto = new ResultDto();
            var roleDiscountData = new RequestDisocuntDto();
            try
            {

                var roleDiscount = _emamiContext.RoleDiscount.AsNoTracking()
                    .FirstOrDefault(w => w.RoleId == requestDisocuntInputDto.RoleId && w.SkuId == requestDisocuntInputDto.SkuId && w.Status == (int)DiscountStatus.Approved);
                if (roleDiscount != null)
                {
                    roleDiscountData = new RequestDisocuntDto()
                    {
                        Id = roleDiscount.Id,
                        RoleId = roleDiscount.RoleId,
                        RoleName = roleDiscount.Role.Name,
                        OilTypeName = roleDiscount.Sku.OilType.Name,
                        SkuName = roleDiscount.Sku.SkuName,
                        ActualDiscount = roleDiscount.ActualDiscount,
                        RequestedDiscount = roleDiscount.RequestedDiscount,
                        Status = roleDiscount.Status
                    };
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = roleDiscountData;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Approve Discount

        public ResultDto GetRequestedDiscounts(RequestDisocuntInputDto requestDisocuntInputDto)
        {
            _methodName = "GetRequestedDiscounts";
            var resultDto = new ResultDto();
            try
            {
                var roleDiscountList = _emamiContext.RoleDiscount.AsNoTracking()
                    .Where(w => w.Status == (int)DiscountStatus.Pending && w.RoleId == requestDisocuntInputDto.RoleId)
                    .Select(s => new RequestDisocuntDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleId,
                        RoleName = s.Role.Name,
                        OilTypeName = s.Sku.OilType.Name,
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        SaudaBookingTypeName = s.SaudaBookingType.Name,
                        ActualDiscount = s.ActualDiscount,
                        RequestedDiscount = s.RequestedDiscount,
                        Status = s.Status
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleDiscountList;
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

        public ResultDto ApproveRequestedDiscount(ApproveRequestDiscountDto approveRequestDiscountDto)
        {
            _methodName = "ApproveRequestedDiscount";
            var resultDto = new ResultDto();
            int reasonType = 0;
            try
            {
                if (approveRequestDiscountDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (approveRequestDiscountDto != null)
                {
                    var roleDiscounts = _emamiContext.RoleDiscount.FirstOrDefault(f => f.Id == approveRequestDiscountDto.Id);
                    if (roleDiscounts != null)
                    {
                        if (approveRequestDiscountDto.ReasonType == 1)
                        {
                            roleDiscounts.Status = (int)DiscountStatus.Approved;
                            reasonType = (int)DiscountStatus.Approved;
                        }
                        if (approveRequestDiscountDto.ReasonType == 2)
                        {
                            roleDiscounts.Status = (int)DiscountStatus.Canceled;
                            reasonType = (int)DiscountStatus.Canceled;
                        }
                        roleDiscounts.ModifiedBy = approveRequestDiscountDto.LoginUserId;
                        roleDiscounts.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();

                        #region Reason Table
                        var entity = new Remarks()
                        {
                            TableId = roleDiscounts.Id,
                            TableName = "RoleDiscount",
                            ReasonTypeId = reasonType,
                            Description = approveRequestDiscountDto.Reason,
                            CreatedBy = approveRequestDiscountDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        InsertReason(entity);
                        #endregion

                        resultDto.IsSuccess = true;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
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


        public void InsertReason(Remarks remarks)
        {
            try
            {
                _emamiContext.Remarks.Add(remarks);
                _emamiContext.SaveChanges();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Premium Discount

        public ResultDto GetPremiumDiscountsAll(PremiumDisocuntInputDto premiumDisocuntInputDto)
        {
            _methodName = "GetPremiumDiscountsAll";
            var resultDto = new ResultDto();
            try
            {

                var premiumDiscountList = _emamiContext.PremiumDiscount.AsNoTracking()
                    .Where(w => w.Status == (int)DiscountStatus.Approved && w.RoleId == premiumDisocuntInputDto.RoleId)
                    .Select(s => new PremiumDisocuntDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleId,
                        RoleName = s.Role.Name,
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.Sku.OilType.Name,
                        SkuName = s.Sku.SkuName,
                        SaudaBookingTypeName = s.SaudaBookingType.Name,
                        ActualDiscount = s.ActualDiscount,
                        Status = s.Status,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = premiumDiscountList;
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

        public ResultDto GetPremiumDiscountbyId(PremiumDisocuntInputDto premiumDisocuntInputDto)
        {
            _methodName = "GetPremiumDiscountbyId";
            var resultDto = new ResultDto();
            var premiumDisocuntData = new PremiumDisocuntDto();
            var premiumDiscountList = new List<SkuDiscounts>();

            try
            {
                var premiumDiscounts = _emamiContext.PremiumDiscount.AsNoTracking()
                    .Where(w => w.RoleId == premiumDisocuntInputDto.RoleId && w.OilTypeId == premiumDisocuntInputDto.OilTypeId).ToList();

                if (premiumDiscounts != null && premiumDiscounts.Any())
                {
                    premiumDisocuntData.Id = premiumDiscounts.FirstOrDefault().Id;
                    premiumDisocuntData.RoleId = premiumDiscounts.FirstOrDefault().RoleId;
                    premiumDisocuntData.OilTypeId = premiumDiscounts.FirstOrDefault().OilTypeId;
                    premiumDisocuntData.VerticleId = premiumDiscounts.FirstOrDefault().Sku.OilType.DivisionId;
                    premiumDisocuntData.ValidFrom = premiumDiscounts.FirstOrDefault().ValidFrom;
                    premiumDisocuntData.ValidTo = premiumDiscounts.FirstOrDefault().ValidTo;
                    foreach (var item in premiumDiscounts)
                    {
                        SkuDropDown sku = new SkuDropDown();
                        sku.SkuId = item.SkuId;
                        sku.SkuName = item.Sku.SkuName;
                        var roleActualDiscount = new SkuDiscounts()
                        {
                            Id = item.Id,
                            SkuDropDown = sku,
                            ActualDiscount = item.ActualDiscount
                        };
                        premiumDiscountList.Add(roleActualDiscount);
                    }
                    premiumDisocuntData.SkuDiscounts = premiumDiscountList;

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = premiumDisocuntData;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto AddPremiumDiscount(PremiumDisocuntDto premiumDisocuntDto)
        {
            _methodName = "AddPremiumDiscount";
            var resultDto = new ResultDto();
            try
            {
                if (premiumDisocuntDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (premiumDisocuntDto != null)
                {
                    foreach (var item in premiumDisocuntDto.SkuDiscounts)
                    {
                        var nameValidation = _emamiContext.PremiumDiscount.AsNoTracking()
                    .Count(c => c.OilTypeId == premiumDisocuntDto.OilTypeId && c.SkuId == item.SkuDropDown.SkuId && c.RoleId == premiumDisocuntDto.RoleId);
                        if (nameValidation > 0)
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = Constants.RoleDiscountSkuNameExists;
                            return resultDto;
                        }

                        var premiumDisocunt = new PremiumDiscount()
                        {
                            RoleId = premiumDisocuntDto.RoleId,
                            SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                            Status = (int)DiscountStatus.Approved,
                            ApprovedBy = premiumDisocuntDto.LoginUserId,
                            OilTypeId = premiumDisocuntDto.OilTypeId,
                            SkuId = item.SkuDropDown.SkuId,
                            ActualDiscount = item.ActualDiscount,
                            ValidFrom = premiumDisocuntDto.ValidFrom,
                            ValidTo = premiumDisocuntDto.ValidTo,
                            CreatedBy = premiumDisocuntDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.PremiumDiscount.Add(premiumDisocunt);
                    }
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
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

        public ResultDto UpdatePremiumDiscount(PremiumDisocuntDto premiumDisocuntDto)
        {
            _methodName = "UpdatePremiumDiscount";
            var resultDto = new ResultDto();
            try
            {
                if (premiumDisocuntDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                #region Validation
                if (premiumDisocuntDto != null)
                {
                    foreach (var item in premiumDisocuntDto.SkuDiscounts)
                    {
                        if (item.Id > 0)
                        {
                            var nameValidation = _emamiContext.PremiumDiscount.AsNoTracking()
                    .Count(c => c.OilTypeId == premiumDisocuntDto.OilTypeId && c.SkuId == item.SkuDropDown.SkuId && c.RoleId == premiumDisocuntDto.RoleId && c.Id != item.Id);
                            if (nameValidation > 0)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = Constants.RoleDiscountSkuNameExists;
                                return resultDto;
                            }
                        }
                        else
                        {
                            var nameValidation = _emamiContext.PremiumDiscount.AsNoTracking()
                    .Count(c => c.OilTypeId == premiumDisocuntDto.OilTypeId && c.SkuId == item.SkuDropDown.SkuId && c.RoleId == premiumDisocuntDto.RoleId);
                            if (nameValidation > 0)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = Constants.RoleDiscountSkuNameExists;
                                return resultDto;
                            }
                        }
                    }
                }
                #endregion

                if (premiumDisocuntDto != null)
                {
                    foreach (var item in premiumDisocuntDto.SkuDiscounts)
                    {
                        if (item.Id > 0)
                        {
                            var roleEntity = _emamiContext.PremiumDiscount.FirstOrDefault(f => f.Id == item.Id);
                            roleEntity.ActualDiscount = item.ActualDiscount;
                            roleEntity.ValidFrom = premiumDisocuntDto.ValidFrom;
                            roleEntity.ValidTo = premiumDisocuntDto.ValidTo;
                            roleEntity.ModifiedBy = premiumDisocuntDto.LoginUserId;
                            roleEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            _emamiContext.SaveChanges();
                        }
                        else
                        {
                            var premiumDisocunt = new PremiumDiscount()
                            {
                                RoleId = premiumDisocuntDto.RoleId,
                                SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                Status = (int)DiscountStatus.Approved,
                                ApprovedBy = premiumDisocuntDto.LoginUserId,
                                SkuId = item.SkuDropDown.SkuId,
                                OilTypeId = premiumDisocuntDto.OilTypeId,
                                ActualDiscount = item.ActualDiscount,
                                ValidFrom = premiumDisocuntDto.ValidFrom,
                                ValidTo = premiumDisocuntDto.ValidTo,
                                CreatedBy = premiumDisocuntDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.PremiumDiscount.Add(premiumDisocunt);
                        }
                    }
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
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

        #endregion

        #region Premium Discount Request

        public ResultDto GetPremiumRequestDiscountList(PremiumDisocuntRequestInputDto requestDisocuntInputDto)
        {
            _methodName = "GetPremiumRequestDiscountList";
            var resultDto = new ResultDto();
            try
            {

                var roleDiscountList = _emamiContext.PremiumDiscount.AsNoTracking()
                    .Where(w => w.RoleId == requestDisocuntInputDto.RoleId && w.Status == (int)DiscountStatus.Pending)
                    .Select(s => new PremiumDisocuntRequestDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleId,
                        RoleName = s.Role.Name,
                        OilTypeName = s.OilType.Name,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        SaudaBookingTypeName = s.SaudaBookingType.Name,
                        ActualDiscount = s.ActualDiscount,
                        RequestedDiscount = s.RequestedDiscount,
                        Status = s.Status,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleDiscountList;
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

        public ResultDto UpdatePremiumRequestDiscount(PremiumDisocuntRequestDto requestDisocuntDto)
        {
            _methodName = "UpdatePremiumRequestDiscount";
            var resultDto = new ResultDto();
            var requestDisocunt = new RequestDisocuntDto();

            try
            {
                var roleDiscount = _emamiContext.PremiumDiscount.FirstOrDefault(f => f.Id == requestDisocuntDto.Id);

                if (roleDiscount != null)
                {
                    roleDiscount.RequestedDiscount = requestDisocuntDto.RequestedDiscount;
                    roleDiscount.Status = (int)DiscountStatus.Pending;
                    roleDiscount.ValidFrom = requestDisocuntDto.ValidFrom;
                    roleDiscount.ValidTo = requestDisocuntDto.ValidTo;
                    roleDiscount.ModifiedBy = requestDisocuntDto.LoginUserId;
                    roleDiscount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;   
        }

        public ResultDto GetPremiumRequestDiscountDetailsById(PremiumDisocuntRequestInputDto requestDisocuntInputDto)
        {
            _methodName = "GetPremiumRequestDiscountDetailsById";
            var resultDto = new ResultDto();
            var roleDiscountData = new PremiumDisocuntRequestDto();
            try
            {

                var roleDiscount = _emamiContext.PremiumDiscount.AsNoTracking()
                    .FirstOrDefault(w => w.Id == requestDisocuntInputDto.Id);
                if (roleDiscount != null)
                {
                    roleDiscountData = new PremiumDisocuntRequestDto()
                    {
                        Id = roleDiscount.Id,
                        RoleId = roleDiscount.RoleId,
                        RoleName = roleDiscount.Role.Name,
                        VerticleId = roleDiscount.OilType.DivisionId,
                        OilTypeId = roleDiscount.OilTypeId,
                        OilTypeName = roleDiscount.Sku.OilType.Name,
                        SkuId = roleDiscount.SkuId,
                        SkuName = roleDiscount.Sku.SkuName,
                        ActualDiscount = roleDiscount.ActualDiscount,
                        RequestedDiscount = roleDiscount.RequestedDiscount,
                        Status = roleDiscount.Status,
                        ValidFrom = roleDiscount.ValidFrom,
                        ValidTo = roleDiscount.ValidTo,
                    };
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = roleDiscountData;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetSkuPremiumDiscountRequestById(PremiumDisocuntRequestInputDto requestDisocuntInputDto)
        {
            _methodName = "GetPremiumDiscountRequestDetailsById";
            var resultDto = new ResultDto();
            var roleDiscountData = new PremiumDisocuntRequestDto();
            try
            {

                var roleDiscount = _emamiContext.PremiumDiscount.AsNoTracking()
                    .FirstOrDefault(w => w.RoleId == requestDisocuntInputDto.RoleId && w.SkuId == requestDisocuntInputDto.SkuId && w.Status == (int)DiscountStatus.Approved);
                if (roleDiscount != null)
                {
                    roleDiscountData = new PremiumDisocuntRequestDto()
                    {
                        Id = roleDiscount.Id,
                        RoleId = roleDiscount.RoleId,
                        RoleName = roleDiscount.Role.Name,
                        OilTypeName = roleDiscount.Sku.OilType.Name,
                        SkuName = roleDiscount.Sku.SkuName,
                        ActualDiscount = roleDiscount.ActualDiscount,
                        RequestedDiscount = roleDiscount.RequestedDiscount,
                        Status = roleDiscount.Status
                    };
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = roleDiscountData;
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region Approve Pending Request

        public ResultDto GetPremiumDiscountForPending(PremiumDisocuntRequestInputDto requestDisocuntInputDto)
        {
            _methodName = "GetPremiumDiscountForPending";
            var resultDto = new ResultDto();
            try
            {
                var roleDiscountList = _emamiContext.PremiumDiscount.AsNoTracking()
                    .Where(w => w.Status == (int)DiscountStatus.Pending && w.RoleId == requestDisocuntInputDto.RoleId)
                    .Select(s => new PremiumDisocuntRequestDto()
                    {
                        Id = s.Id,
                        RoleId = s.RoleId,
                        RoleName = s.Role.Name,
                        VerticleId = s.OilType.DivisionId,
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType.Name,
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        SaudaBookingTypeName = s.SaudaBookingType.Name,
                        ActualDiscount = s.ActualDiscount,
                        RequestedDiscount = s.RequestedDiscount,
                        Status = s.Status
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = roleDiscountList;
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

        public ResultDto UpdateApprovePremiumDiscount(ApprovePremiunDiscountRequestDto premiumDisocuntRequestInputDto)
        {
            _methodName = "ApprovePremiumDiscountRequested";
            var resultDto = new ResultDto();
            int reasonType = 0;
            try
            {
                if (premiumDisocuntRequestInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (premiumDisocuntRequestInputDto != null)
                {
                    var roleDiscounts = _emamiContext.PremiumDiscount.FirstOrDefault(f => f.Id == premiumDisocuntRequestInputDto.Id);
                    if (roleDiscounts != null)
                    {
                        if (premiumDisocuntRequestInputDto.ReasonType == 1)
                        {
                            roleDiscounts.Status = (int)DiscountStatus.Approved;
                            reasonType = (int)DiscountStatus.Approved;
                        }
                        if (premiumDisocuntRequestInputDto.ReasonType == 2)
                        {
                            roleDiscounts.Status = (int)DiscountStatus.Canceled;
                            reasonType = (int)DiscountStatus.Canceled;
                        }
                        roleDiscounts.ModifiedBy = premiumDisocuntRequestInputDto.LoginUserId;
                        roleDiscounts.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();

                        #region Reason Table
                        var entity = new Remarks()
                        {
                            TableId = roleDiscounts.Id,
                            TableName = "PremiumDiscount",
                            ReasonTypeId = reasonType,
                            Description = premiumDisocuntRequestInputDto.Reason,
                            CreatedBy = premiumDisocuntRequestInputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        InsertReason(entity);
                        #endregion

                        resultDto.IsSuccess = true;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
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

        #endregion

        #region Primary Discount Users

        public ResultDto AddPrimaryDiscountForUser(PrimaryDiscountUserDto primaryDiscountUserDto)
        {
            _methodName = "AddPrimaryDiscountForUser";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                if (primaryDiscountUserDto != null)
                {
                    if (primaryDiscountUserDto.DiscountType == 1)
                    {
                        var entity = new PremiumUser()
                        {
                            SkuId = primaryDiscountUserDto.SkuId,
                            //DepotId = primaryDiscountUserDto.DepotId,
                            UserId = primaryDiscountUserDto.CustomerId,
                            ActualPremium = primaryDiscountUserDto.ActualDiscount,
                            ValidFrom = primaryDiscountUserDto.ValidFrom,
                            ValidTo = primaryDiscountUserDto.ValidTo,
                            CreatedBy = primaryDiscountUserDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.PremiumUser.Add(entity);
                        _emamiContext.SaveChanges();
                        resultDto.IsSuccess = true;
                    }
                    else if (primaryDiscountUserDto.DiscountType == 2)
                    {
                        var entity = new PrimaryDiscountSku()
                        {
                            SkuId = primaryDiscountUserDto.SkuId,
                            ActualDiscount = primaryDiscountUserDto.ActualDiscount,
                            CreatedBy = primaryDiscountUserDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                            Status = true,
                            ApprovedBy = primaryDiscountUserDto.LoginUserId,
                            ValidFrom = primaryDiscountUserDto.ValidFrom,
                            ValidTo = primaryDiscountUserDto.ValidTo
                        };
                        _emamiContext.PrimaryDiscountSku.Add(entity);
                        _emamiContext.SaveChanges();
                        resultDto.IsSuccess = true;
                    }
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdatePrimaryDiscountForUser(PrimaryDiscountUserDto primaryDiscountUserDto)
        {
            _methodName = "UpdatePrimaryDiscountForUser";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                if (primaryDiscountUserDto.DiscountType == 1)
                {
                    var discountEntity = _emamiContext.PremiumUser.FirstOrDefault(f => f.Id == primaryDiscountUserDto.Id);
                    if (discountEntity != null)
                    {
                        discountEntity.ActualPremium = primaryDiscountUserDto.ActualDiscount;
                        discountEntity.ModifiedBy = primaryDiscountUserDto.LoginUserId;
                        discountEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                        resultDto.IsSuccess = true;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }
                else if (primaryDiscountUserDto.DiscountType == 2)
                {
                    var disocuntSku = _emamiContext.PrimaryDiscountSku.FirstOrDefault(f => f.Id == primaryDiscountUserDto.Id);
                    if (disocuntSku != null)
                    {
                        disocuntSku.ActualDiscount = primaryDiscountUserDto.ActualDiscount;
                        disocuntSku.ModifiedBy = primaryDiscountUserDto.LoginUserId;
                        disocuntSku.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                        resultDto.IsSuccess = true;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        public ResultDto GetPrimaryDiscountForUserList(PrimaryDiscountUserInputDto discountinputDto)
        {
            _methodName = "GetPrimaryDiscountForUserList";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var discountUsers = new List<PremiumUser>();
                var discountSku = new List<PrimaryDiscountSku>();
                var discountList = new List<PrimaryDiscountUserDto>(); ;

                if (discountinputDto.DiscountType == (int)CustomerDiscountType.Customer)
                {
                    discountUsers = _emamiContext.PremiumUser.AsNoTracking().ToList();
                    if (discountUsers != null && discountUsers.Any())
                    {
                        foreach (var item in discountUsers)
                        {
                            PrimaryDiscountUserDto discount = new PrimaryDiscountUserDto();
                            discount.Id = item.Id;
                            discount.CustomerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == item.UserId).Name;
                            discount.SkuId = item.SkuId;
                            discount.SkuName = item.Sku.SkuName;
                            discount.SkuCode = item.Sku.SkuCode;
                            discount.OilTypeName = item.Sku.OilType.Name;
                            //discount.DepotName = item.Depot.Name;
                            discount.ActualDiscount = item.ActualPremium;
                            discount.DiscountType = discountinputDto.DiscountType;
                            discount.ValidFrom = item.ValidFrom;
                            discount.ValidTo = item.ValidTo;
                            discountList.Add(discount);
                        }
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = discountList;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }
                else if (discountinputDto.DiscountType == (int)CustomerDiscountType.Product)
                {
                    discountSku = _emamiContext.PrimaryDiscountSku.AsNoTracking().ToList();
                    if (discountSku != null && discountSku.Any())
                    {
                        foreach (var item in discountSku)
                        {
                            PrimaryDiscountUserDto discount = new PrimaryDiscountUserDto();
                            discount.Id = item.Id;
                            discount.SkuId = item.SkuId;
                            discount.SkuName = item.Sku.SkuName;
                            discount.OilTypeName = item.Sku.OilType.Name;
                            discount.ActualDiscount = item.ActualDiscount;
                            discount.DiscountType = discountinputDto.DiscountType;
                            discount.ValidFrom = item.ValidFrom;
                            discount.ValidTo = item.ValidTo;
                            discountList.Add(discount);
                        }
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = discountList;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        public ResultDto GetPrimaryDiscountForUserById(PrimaryDiscountUserInputDto discountinputDto)
        {
            _methodName = "GetPrimaryDiscountForUserById";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                var discountDto = new PrimaryDiscountUserDto();

                if (discountinputDto.DiscountType == (int)CustomerDiscountType.Customer)
                {
                    var discountEntity = _emamiContext.PremiumUser.AsNoTracking().FirstOrDefault(w => w.Id == discountinputDto.Id);
                    if (discountEntity != null)
                    {
                        discountDto.Id = discountEntity.Id;
                        discountDto.VerticleId = discountEntity.Sku.DivisionId;
                        discountDto.OilTypeId = discountEntity.Sku.OilTypeId;
                        discountDto.SkuId = discountEntity.SkuId;
                        //discountDto.DepotId = discountEntity.DepotId;
                        discountDto.CustomerId = discountEntity.UserId;
                        discountDto.ActualDiscount = discountEntity.ActualPremium;
                        discountDto.DiscountType = discountinputDto.DiscountType;
                        discountDto.ValidFrom = discountEntity.ValidFrom;
                        discountDto.ValidTo = discountEntity.ValidTo;

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = discountDto;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                }
                else if (discountinputDto.DiscountType == (int)CustomerDiscountType.Product)
                {
                    var discountEntity = _emamiContext.PrimaryDiscountSku.AsNoTracking().FirstOrDefault(w => w.Id == discountinputDto.Id);
                    if (discountEntity != null)
                    {
                        discountDto.Id = discountEntity.Id;
                        discountDto.VerticleId = discountEntity.Sku.DivisionId;
                        discountDto.OilTypeId = discountEntity.Sku.OilTypeId;
                        discountDto.SkuId = discountEntity.SkuId;
                        discountDto.ActualDiscount = discountEntity.ActualDiscount;
                        discountDto.DiscountType = discountinputDto.DiscountType;
                        discountDto.ValidFrom = discountEntity.ValidFrom;
                        discountDto.ValidTo = discountEntity.ValidTo;

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = discountDto;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        #endregion

        #region RA - Reverse Auction Margin

        /// <summary>
        /// Method to Save Reverse Auction Margin
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public ResultDto SaveRaMargin(RaMarginDto inputDto)
        {
            _methodName = "SaveRaMargin";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                if (inputDto.ValidTo < inputDto.ValidFrom)
                {
                    return _resultService.ErrorMessage(Constants.ToDateInvalid);
                }

                bool isError = false;
                var errorMessage = Constants.CostAlreadyExistsForSku;
                foreach (var skuId in inputDto.SkuIds)
                {
                    var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);

                    var checkIsExists = _emamiContext.RaMargin
                    .Where(w => w.DivisionId == inputDto.VerticalId
                    && w.OilTypeId == inputDto.OilTypeId
                    && w.OilPackingTypeId == inputDto.OilPackingTypeId
                    && w.SkuId == skuId
                    && w.ZoneId == inputDto.ZoneId
                    && w.StateId == inputDto.StateId
                    //&& w.TerritoryId == inputDto.TerritoryId
                    //&& w.DistrictId == inputDto.DistrictId
                    //&& w.CityId == inputDto.CityId
                    && w.IsActive //&& !w.IsPublished
                    //&& ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //&& DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                    //|| (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //&& DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).ToList();
                    && inputDto.ValidFrom <= w.ValidTo && w.ValidFrom <= inputDto.ValidTo).ToList();

                    //if (raMarginCostCount > 0)
                    //{
                    //    isError = true;
                    //    errorMessage = string.Concat(errorMessage, " - ", skuContext.SkuName);
                    //}
                    if (checkIsExists != null && checkIsExists.Any())
                    {
                        foreach (var item in checkIsExists)
                        {
                            item.IsActive = false;
                            _emamiContext.SaveChanges();
                        }
                    }

                    var input = new RaMargin
                    {
                        Id = inputDto.Id,
                        CityId = inputDto.CityId,
                        StateId = inputDto.StateId,
                        TerritoryId = inputDto.TerritoryId,
                        DistrictId = inputDto.DistrictId,
                        DivisionId = inputDto.VerticalId,
                        OilTypeId = inputDto.OilTypeId,
                        OilPackingTypeId = inputDto.OilPackingTypeId,
                        CustomerCategoryWise = inputDto.CustomerCategoryWise,
                        SkuId = skuId,
                        RatePerMt = inputDto.RatePerMt,
                        ValidFrom = inputDto.ValidFrom,
                        ValidTo = inputDto.ValidTo,
                        ZoneId = inputDto.ZoneId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        IsActive = true,
                        IsPublished = false,
                    };
                    _emamiContext.RaMargin.Add(input);
                    _emamiContext.SaveChanges();
                }

                if (isError)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = errorMessage;
                }
                else
                {
                    resultDto.IsSuccess = true;
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
        /// Method to Get Reverse Auction Margin List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetRaMarginList(LoginUserIdDto inputDto)
        {
            _methodName = "GetRaMarginList";
            var resultDto = new ResultDto();
            var outputDto = new List<RaMarginDto>();
            try
            {
                var resultContext = _emamiContext.RaMargin.AsNoTracking().Where(w => (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0)).ToList();
                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.Select(c => new RaMarginDto
                    {
                        Id = c.Id,
                        CityId = c.CityId,
                        CityName = c.City != null ? c.City.CityName : string.Empty,
                        StateId = c.StateId,
                        StateName = c.State != null ? c.State.StateName : string.Empty,
                        TerritoryId = c.TerritoryId,
                        TerritoryName = c.Territory != null ? c.Territory.Name : string.Empty,
                        DistrictId = c.DistrictId,
                        DistrictName = c.District != null ? c.District.DistrictName : string.Empty,
                        SkuId = c.SkuId,
                        SkuName = c.Sku != null ? c.Sku.SkuName : string.Empty,
                        SkuCode = c.Sku != null ? c.Sku.SkuCode : string.Empty,
                        OilTypeId = c.OilTypeId,
                        OilTypeName = c.OilType != null ? c.OilType.Name : string.Empty,
                        VerticalId = c.DivisionId,
                        VerticalName = c.Division != null ? c.Division.Name : string.Empty,
                        OilPackingTypeId = c.OilPackingTypeId,
                        OilPackingType = c.OilPackingType != null ? c.OilPackingType.Name : string.Empty,
                        CustomerCategoryWise = c.CustomerCategoryWise,
                        RatePerMt = c.RatePerMt,
                        ZoneName = c.Zone?.Name,
                        ZoneId = c.ZoneId,
                        ValidFrom = c.ValidFrom,
                        ValidTo = c.ValidTo,
                        IsActive = c.IsActive,
                        IsPublished = c.IsPublished
                    }).ToList();
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
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Get Reverse Auction Margin List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetRaMarginListWithPaging(KendoGridResult inputDto)
        {
            _methodName = "GetRaMarginListWithPaging";
            var resultDto = new ResultDto();
            DataSourceResult result = new DataSourceResult();
            IQueryable<RaMargin> resultContext;
            try
            {
                //resultContext = _emamiContext.RaMargin.AsNoTracking().Where(w => (inputDto.VerticalId > 0 ? w.VerticalId == inputDto.VerticalId : w.VerticalId > 0));

                if (inputDto.IsToReturnInactiveData)
                {
                    resultContext = _emamiContext.RaMargin.AsNoTracking()
                        .Where(w => (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0));
                }
                else
                {
                    resultContext = _emamiContext.RaMargin.AsNoTracking()
                        .Where(w => w.IsActive && (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0));
                }

                if (resultContext != null && resultContext.Any())
                {
                    result = resultContext.Select(c => new RaMarginDto
                    {
                        Id = c.Id,
                        ZoneName = c.Zone != null ? c.Zone.Name : string.Empty,
                        StateName = c.State != null ? c.State.StateName : string.Empty,
                        //TerritoryName = c.Territory != null ? c.Territory.Name : string.Empty,
                        //DistrictName = c.District != null ? c.District.DistrictName : string.Empty,
                        //CityName = c.City != null ? c.City.CityName : string.Empty,                                    
                        SkuName = c.Sku != null ? c.Sku.SkuName : string.Empty,
                        SkuCode = c.Sku != null ? c.Sku.SkuCode : string.Empty,
                        OilTypeName = c.OilType != null ? c.OilType.Name : string.Empty,
                        VerticalName = c.Division != null ? c.Division.Name : string.Empty,
                        OilPackingType = c.OilPackingType != null ? c.OilPackingType.Name : string.Empty,
                        CustomerCategoryWise = c.CustomerCategoryWise,
                        RatePerMt = c.RatePerMt,
                        ValidFrom = c.ValidFrom,
                        ValidTo = c.ValidTo,
                        IsActive = c.IsActive,
                        IsPublished = c.IsPublished
                    }).ToDataSourceResult(inputDto.DataSourceRequest);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;  //outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
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

        public ResultDto ExportRaMargin(LoginUserIdDto inputDto)
        {
            _methodName = "ExportRaMargin";
            var resultDto = new ResultDto();
            var outputDto = new List<RaMarginDto>();
            try
            {
                var resultContext = _emamiContext.RaMargin.AsNoTracking().Where(w => (inputDto.VerticalId > 0 ? w.DivisionId == inputDto.VerticalId : w.DivisionId > 0)).ToList();
                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.ToList().Select(c => new RaMarginDto
                    {
                        Id = c.Id,
                        CityId = c.CityId,
                        CityName = c.City != null ? c.City.CityName : string.Empty,
                        StateId = c.StateId,
                        StateName = c.State != null ? c.State.StateName : string.Empty,
                        TerritoryId = c.TerritoryId,
                        TerritoryName = c.Territory != null ? c.Territory.Name : string.Empty,
                        DistrictId = c.DistrictId,
                        DistrictName = c.District != null ? c.District.DistrictName : string.Empty,
                        SkuId = c.SkuId,
                        SkuName = c.Sku != null ? c.Sku.SkuName : string.Empty,
                        SkuCode = c.Sku != null ? c.Sku.SkuCode : string.Empty,
                        OilTypeId = c.OilTypeId,
                        OilTypeName = c.OilType != null ? c.OilType.Name : string.Empty,
                        VerticalId = c.DivisionId,
                        VerticalName = c.Division != null ? c.Division.Name : string.Empty,
                        OilPackingTypeId = c.OilPackingTypeId,
                        OilPackingType = c.OilPackingType != null ? c.OilPackingType.Name : string.Empty,
                        CustomerCategoryWise = c.CustomerCategoryWise,
                        RatePerMt = c.RatePerMt,
                        ZoneName = c.Zone?.Name,
                        ZoneId = c.ZoneId,
                        ValidFrom = c.ValidFrom,
                        ValidTo = c.ValidTo,
                        IsActive = c.IsActive,
                        IsPublished = c.IsPublished,
                        RatePerCase = _resultService.ConvertRatePerMetricToRatePerCase((long)c.SkuId, c.RatePerMt),
                    }).ToList();
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
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to get Get Reverse Auction Margin Details By Id
        /// </summary>
        /// <param name="cushionMarginId"></param>
        /// <returns></returns>
        public ResultDto GetRaMarginDetailsById(long raMarginId)
        {
            _methodName = "GetRaMarginDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new RaMarginDto();
            try
            {
                var resultContext = _emamiContext.RaMargin.AsNoTracking().FirstOrDefault(_ => _.Id == raMarginId);
                if (resultContext != null)
                {
                    outputDto.Id = resultContext.Id;
                    outputDto.CityId = resultContext.CityId;
                    outputDto.CityName = resultContext.City != null ? resultContext.City.CityName : string.Empty;
                    outputDto.StateId = resultContext.StateId;
                    outputDto.StateName = resultContext.State != null ? resultContext.State.StateName : string.Empty;
                    outputDto.TerritoryId = resultContext.TerritoryId;
                    outputDto.DistrictId = resultContext.DistrictId;
                    outputDto.DistrictName = resultContext.District != null ? resultContext.District.DistrictName : string.Empty;
                    outputDto.VerticalId = resultContext.DivisionId;
                    outputDto.VerticalName = resultContext.Division != null ? resultContext.Division.Name : string.Empty;
                    outputDto.SkuId = resultContext.SkuId;
                    outputDto.SkuName = resultContext.Sku != null ? resultContext.Sku.SkuName : string.Empty;
                    outputDto.OilTypeId = resultContext.OilTypeId;
                    outputDto.OilTypeName = resultContext.OilType != null ? resultContext.OilType.Name : string.Empty;
                    outputDto.OilPackingTypeId = resultContext.OilPackingTypeId;
                    outputDto.OilPackingType = resultContext.OilPackingType != null ? resultContext.OilPackingType.Name : string.Empty;
                    outputDto.CustomerCategoryWise = resultContext.CustomerCategoryWise;
                    outputDto.RatePerMt = resultContext.RatePerMt;
                    outputDto.ValidFrom = resultContext.ValidFrom;
                    outputDto.ValidTo = resultContext.ValidTo;
                    outputDto.ZoneId = resultContext.ZoneId;
                    outputDto.ZoneName = resultContext.Zone?.Name;
                    outputDto.SkuIds = new List<long> { Convert.ToInt64(resultContext.SkuId) };

                    if (resultContext.DivisionId == ((long)DTO.Enums.Division.Hbc))
                    {
                        //bool isRasoi = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(f => f.Id == resultContext.OilTypeId).IsRasoi;
                        //if (isRasoi)
                        //{
                        //    outputDto.SubCategoryId = resultContext.Sku.SubCategoryId;
                        //}
                    }
                    else
                    {
                        outputDto.SubCategoryId = resultContext.Sku.SubCategoryId;
                    }
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

        /// <summary>
        /// Method to Update Reverse Auction Margin
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateRaMargin(RaMarginDto inputDto)
        {
            _methodName = "UpdateRaMargin";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                if (inputDto.ValidTo < inputDto.ValidFrom)
                {
                    return _resultService.ErrorMessage(Constants.ToDateInvalid);
                }

                bool isError = false;
                var errorMessage = Constants.CostAlreadyExistsForSku;
                foreach (var skuId in inputDto.SkuIds)
                {
                    var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);

                    var checkIsExists = _emamiContext.RaMargin
                    .Where(w => w.DivisionId == inputDto.VerticalId
                    && w.OilTypeId == inputDto.OilTypeId
                    && w.OilPackingTypeId == inputDto.OilPackingTypeId
                    && w.SkuId == skuId
                    && w.ZoneId == inputDto.ZoneId
                    && w.StateId == inputDto.StateId
                    //&& w.TerritoryId == inputDto.TerritoryId
                    //&& w.DistrictId == inputDto.DistrictId
                    //&& w.CityId == inputDto.CityId
                    && w.Id != inputDto.Id
                    && w.IsActive //&& !w.IsPublished
                    //&& ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //&& DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                    //|| (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //&& DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).ToList();
                    && inputDto.ValidFrom <= w.ValidTo && w.ValidFrom <= inputDto.ValidTo).ToList();

                    if (checkIsExists != null && checkIsExists.Any())
                    {
                        foreach (var item in checkIsExists)
                        {
                            item.IsActive = false;
                            _emamiContext.SaveChanges();
                        }
                    }

                    //if (raMarginCostCount == 0)
                    //{
                    var result = _emamiContext.RaMargin.FirstOrDefault(_ => _.Id == inputDto.Id);
                    result.OilPackingTypeId = inputDto.OilPackingTypeId;
                    result.CustomerCategoryWise = inputDto.CustomerCategoryWise;
                    result.DivisionId = inputDto.VerticalId;
                    result.OilTypeId = inputDto.OilTypeId;
                    result.SkuId = skuId;
                    result.CityId = inputDto.CityId;
                    result.StateId = inputDto.StateId;
                    result.TerritoryId = inputDto.TerritoryId;
                    result.DistrictId = inputDto.DistrictId;
                    result.RatePerMt = inputDto.RatePerMt;
                    result.ValidFrom = inputDto.ValidFrom;
                    result.ValidTo = inputDto.ValidTo;
                    result.ZoneId = inputDto.ZoneId;
                    result.ModifiedBy = inputDto.LoginUserId;
                    result.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                    //}
                    //else
                    //{
                    //    isError = true;
                    //    errorMessage = string.Concat(errorMessage, " - ", skuContext.SkuName);
                    //}
                }

                if (isError)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = errorMessage;
                }
                else
                {
                    resultDto.IsSuccess = true;
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

        #endregion

        #region Geography Discounts

        public ResultDto GetCityDetailsBasedOnTerritory(TerritoryId territoryId)
        {
            _methodName = "GetCityDetailsBasedOnTerritory";
            var resultDto = new ResultDto();
            var cityList = new List<CityDetails>();
            try
            {

                if (territoryId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                using (var connection = new SqlConnection(Config.DBConnectionString))
                {
                    string districtQuery = @"SELECT Id FROM Districts WHERE StateId IN @TerritoryIds";

                    var districtIds = (connection.Query<long>(districtQuery, new { TerritoryIds = territoryId.TerritoryIds })).ToList();

                    if (districtIds.Any())
                    {
                        string cityQuery = @"SELECT c.Id AS CityId,c.CityName,d.Id AS DistrictId,d.DistrictName,s.Id AS StateId, 
                                            s.StateName, z.ZoneId, zn.Name AS ZoneName
                                            FROM Cities c
                                            INNER JOIN Districts d ON c.DistrictId = d.Id
                                            INNER JOIN States s ON d.StateId = s.Id
                                            INNER JOIN ZoneStateMappings z ON s.Id = z.StateId
                                            INNER JOIN Zones zn ON z.ZoneId = zn.Id
                                            WHERE c.DistrictId IN @DistrictIds";

                        var cityDetails = (connection.Query<CityDetails>(cityQuery, new { DistrictIds = districtIds })).ToList();

                        if (cityDetails.Any())
                        {
                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = cityDetails;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = Constants.RecordNotFound;
                        }
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        public ResultDto AddDiscountGeography(DiscountInputDto inputDto)
        {
            _methodName = "AddDiscountGeography";
            var resultDto = new ResultDto();
         
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.Cities == null || !inputDto.Cities.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.CityListEmpty;
                    return resultDto;
                }

                if (inputDto.SkuIds == null || !inputDto.SkuIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.SkuIsEmpty;
                    return resultDto;
                }

                if (inputDto != null && inputDto.Cities != null && inputDto.Cities.Any())
                {

                    #region Validation
                    var cityIds = inputDto.Cities.Select(s => s.CityId).ToList();
                    var geographyDiscountCount = _emamiContext.DiscountGeography.AsNoTracking()
                       .Where(w => w.OilTypeId == inputDto.OilTypeId
                       && w.PackGroupId == inputDto.PackGroupId
                       && w.PackTypeId == inputDto.PackTypeId
                       && inputDto.SkuIds.Contains(w.SkuId)
                       && cityIds.Contains(w.CityId)
                       && w.IsActive
                       && ((w.ValidFrom >= inputDto.ValidFrom
                        && w.ValidFrom <= inputDto.ValidTo)
                        || (w.ValidTo >= inputDto.ValidFrom
                        && w.ValidTo <= inputDto.ValidTo)))
                       .Select(s => s.CityId).ToList();

                    if (geographyDiscountCount != null && geographyDiscountCount.Any())
                    {
                        var cityName = _emamiContext.City.AsNoTracking().Where(w => geographyDiscountCount.Any(a => a == w.Id)).Select(s => s.CityName);

                        if (cityName.Count() <= Constants.GeoMinimumCityCount)
                            return _resultService.ErrorMessage(string.Concat(Constants.CostAlreadyExistiInThisCity,string.Join(",", cityName.Select(s => s))));
                        else
                            return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisCities);
                    }
                    #endregion

                    using (var connection = new SqlConnection(Config.DBConnectionString))
                    {
                        try
                        {
                            connection.Execute("[dbo].[InsertDiscountGeography]", new
                            {
                                SkuIds = string.Join(",", inputDto.SkuIds).ToString(),
                                OilTypeId = inputDto.OilTypeId,
                                PackGroupId = inputDto.PackGroupId,
                                PackTypeId = inputDto.PackTypeId,
                                Cities = JsonConvert.SerializeObject(inputDto.Cities.Select(_ => new
                                {
                                    ZoneId = _.ZoneId,
                                    StateId = _.StateId,
                                    TerritoryId = _.TerritoryId,
                                    DistrictId = _.DistrictId,
                                    CityId = _.CityId
                                })),
                                DiscountReason = inputDto.DiscountReason,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                LoginUserId = inputDto.LoginUserId,
                                ActualDiscount = inputDto.ActualDiscount,
                                IsActive = inputDto.IsActive
                            }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 0);

                            resultDto.IsSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                            _logger.Error(message);
                            resultDto = _resultService.ErrorMessage(Constants.Exception);
                        }
                    }
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

        public ResultDto UpdateDiscountGeography(DiscountInputDto inputDto)
        {
            _methodName = "UpdateDiscountGeography";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.Cities == null || !inputDto.Cities.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.CityListEmpty;
                    return resultDto;
                }

                if (inputDto.SkuIds == null || !inputDto.SkuIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.SkuIsEmpty;
                    return resultDto;
                }

                if (inputDto != null && inputDto.Cities != null && inputDto.Cities.Any())
                {
                    #region Validation
                    var cityIds = inputDto.Cities.Select(s => s.CityId).ToList();
                    var geographyDiscountCount = _emamiContext.DiscountGeography.AsNoTracking()
                       .Where(w => w.OilTypeId == inputDto.OilTypeId
                       && inputDto.SkuIds.Contains(w.SkuId)
                       && w.OilTypeId == inputDto.OilTypeId
                       && w.PackGroupId == inputDto.PackGroupId
                       && w.PackTypeId == inputDto.PackTypeId
                       && cityIds.Contains(w.CityId)
                       && ((w.ValidFrom >= inputDto.ValidFrom
                        && w.ValidFrom <= inputDto.ValidTo)
                        || (w.ValidTo >= inputDto.ValidFrom
                        && w.ValidTo <= inputDto.ValidTo))).ToList();

                    var notWithInCity = geographyDiscountCount.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id)
                        .Select(s => s.CityId).ToList();

                    if (notWithInCity != null && notWithInCity.Any())
                    {
                        var cityName = _emamiContext.City.AsNoTracking().Where(w => notWithInCity.Contains(w.Id)).Select(s => s.CityName).ToList();
                        return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisCity + string.Join(",", cityName.Select(s => s)));
                    }

                    #endregion


                    var discountGeography = _emamiContext.DiscountGeography.Where(w => w.ParentId == inputDto.Id).ToList();

                    var discountGivenByHigherRole = discountGeography.FirstOrDefault().ActualDiscount;

                    if (inputDto.ActualDiscount > discountGivenByHigherRole)
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountGivenByHigherRole + ". Please enter less then or equal to discount");
                    }

                    if (discountGeography != null && discountGeography.Any())
                    {
                        var inputCityIds = inputDto.Cities.Select(s => s.CityId).ToList();

                        var dbSkuIds = discountGeography.Select(s => s.SkuId).Distinct().ToList();
                        var dbCityIds = discountGeography.Select(s => s.CityId).Distinct().ToList();

                        var newSkuIds = inputDto.SkuIds.Where(w => !dbSkuIds.Contains(w)).Distinct().ToList();
                        var removedSkuIds = dbSkuIds.Where(w => !inputDto.SkuIds.Contains(w)).Distinct().ToList();

                        if (removedSkuIds.Any())
                        {
                            var removedDiscountIds = discountGeography
                                .Where(w => removedSkuIds.Contains(w.SkuId))
                                .Select(_ => _.Id).ToList();

                            if (removedDiscountIds.Any())
                            {
                                RemoveGeographyDiscountSkuIds(removedDiscountIds);
                            }
                        }

                        var newCityIds = inputCityIds.Where(w => !dbCityIds.Contains(w)).Distinct().ToList();
                        var removedCityIds = dbCityIds.Where(w => !inputCityIds.Contains(w)).Distinct().ToList();

                        if (removedCityIds.Any())
                        {
                            var removedDiscountIds = discountGeography
                                .Where(w => removedCityIds.Contains(w.CityId))
                                .Select(_ => _.Id).ToList();

                            if (removedDiscountIds.Any())
                            {
                                RemoveGeographyDiscountSkuIds(removedDiscountIds);
                            }
                        }

                        if (newSkuIds.Any() && newCityIds.Any())
                        {
                            var newCityList = inputDto.Cities.Where(w => newCityIds.Contains(w.CityId));

                            foreach (var skuId in newSkuIds)
                            {
                                var skuContext = _emamiContext.Skus.AsNoTracking()
                                    .FirstOrDefault(s => s.Id == skuId && s.OilTypeId == inputDto.OilTypeId && s.PackGroupId == inputDto.PackGroupId && s.PackTypeId == inputDto.PackTypeId);
                                foreach (var city in newCityList)
                                {
                                    _emamiContext.DiscountGeography.Add(new DiscountGeography
                                    {
                                        SalesOrganizationId = skuContext.SalesOrganizationId,
                                        DistributionChannelId = skuContext.DistributionChannelId,
                                        DivisionId = skuContext.DivisionId,
                                        DiscountReason = inputDto.DiscountReason,
                                        OilTypeId = skuContext.OilTypeId ?? 0,
                                        PackGroupId = skuContext.PackGroupId ?? 0,
                                        PackTypeId = skuContext.PackTypeId,
                                        SkuId = skuId,
                                        ZoneId = city.ZoneId,
                                        StateId = city.StateId,
                                        TerritoryId = city.TerritoryId,
                                        DistrictId = city.DistrictId,
                                        CityId = city.CityId,
                                        ActualDiscount = inputDto.ActualDiscount,
                                        ValidFrom = inputDto.ValidFrom,
                                        ValidTo = inputDto.ValidTo,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        ParentId = inputDto.Id,
                                        IsActive = inputDto.IsActive
                                    });
                                }
                            }
                            _emamiContext.SaveChanges();
                        }
                        else if (newSkuIds.Any() && !newCityIds.Any())
                        {
                            foreach (var skuId in newSkuIds)
                            {
                                var skuContext = _emamiContext.Skus.AsNoTracking()
                                    .FirstOrDefault(s => s.Id == skuId && s.OilTypeId == inputDto.OilTypeId && s.PackGroupId == inputDto.PackGroupId && s.OilPackGroupTypeId == inputDto.PackTypeId);

                                foreach (var city in inputDto.Cities)
                                {
                                    _emamiContext.DiscountGeography.Add(new DiscountGeography
                                    {
                                        SalesOrganizationId = skuContext.SalesOrganizationId,
                                        DistributionChannelId = skuContext.DistributionChannelId,
                                        DivisionId = skuContext.DivisionId,
                                        DiscountReason = inputDto.DiscountReason,
                                        OilTypeId = skuContext.OilTypeId ?? 0,
                                        PackGroupId = skuContext.PackGroupId ?? 0,
                                        PackTypeId = skuContext.PackTypeId,
                                        SkuId = skuId,
                                        ZoneId = city.ZoneId,
                                        StateId = city.StateId,
                                        TerritoryId = city.TerritoryId,
                                        DistrictId = city.DistrictId,
                                        CityId = city.CityId,
                                        ActualDiscount = inputDto.ActualDiscount,
                                        ValidFrom = inputDto.ValidFrom,
                                        ValidTo = inputDto.ValidTo,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        ParentId = inputDto.Id,
                                        IsActive = inputDto.IsActive
                                    });
                                }
                            }
                            _emamiContext.SaveChanges();
                        }
                        else if (!newSkuIds.Any() && newCityIds.Any())
                        {
                            var newCityList = inputDto.Cities.Where(w => newCityIds.Contains(w.CityId));

                            foreach (var skuId in inputDto.SkuIds)
                            {
                                var skuContext = _emamiContext.Skus.AsNoTracking()
                                    .FirstOrDefault(s => s.Id == skuId && s.OilTypeId == inputDto.OilTypeId && s.PackGroupId == inputDto.PackGroupId && s.PackTypeId == inputDto.PackTypeId);

                                foreach (var city in newCityList)
                                {
                                    _emamiContext.DiscountGeography.Add(new DiscountGeography
                                    {
                                        SalesOrganizationId = skuContext.SalesOrganizationId,
                                        DistributionChannelId = skuContext.DistributionChannelId,
                                        DivisionId = skuContext.DivisionId,
                                        DiscountReason = inputDto.DiscountReason,
                                        OilTypeId = skuContext.OilTypeId ?? 0,
                                        PackGroupId = skuContext.PackGroupId ?? 0,
                                        PackTypeId = skuContext.PackTypeId,
                                        SkuId = skuId,
                                        ZoneId = city.ZoneId,
                                        StateId = city.StateId,
                                        TerritoryId = city.TerritoryId,
                                        DistrictId = city.DistrictId,
                                        CityId = city.CityId,
                                        ActualDiscount = inputDto.ActualDiscount,
                                        ValidFrom = inputDto.ValidFrom,
                                        ValidTo = inputDto.ValidTo,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        ParentId = inputDto.Id,
                                        IsActive = inputDto.IsActive
                                    });
                                }
                            }
                            _emamiContext.SaveChanges();
                        }

                        // Update existing data using Dapper
                        if (_emamiContext.DiscountGeography.Any(w => w.Id == inputDto.Id))
                        {
                            using (var connection = new SqlConnection(Config.DBConnectionString))
                            {
                                string updateQuery = @"UPDATE DiscountGeographies
                                                        SET ActualDiscount = @ActualDiscount,
                                                            ModifiedBy = @ModifiedBy,
                                                            ModifiedDate = @ModifiedDate,
                                                            ValidFrom = @ValidFrom,
                                                            ValidTo = @ValidTo,
                                                            IsActive = @IsActive
                                                        WHERE (ParentId = @ParentId OR Id = @ParentId)";

                                var parameters = new
                                {
                                    ActualDiscount = inputDto.ActualDiscount,
                                    ModifiedBy = inputDto.LoginUserId,
                                    ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    ValidFrom = inputDto.ValidFrom,
                                    ValidTo = inputDto.ValidTo,
                                    ParentId = inputDto.Id,
                                    IsActive = inputDto.IsActive
                                };

                                connection.Execute(updateQuery, parameters);
                            }
                        }

                        resultDto.IsSuccess = true;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        private static void RemoveGeographyDiscountSkuIds(List<long> removedDiscountIds)
        {
            using (var connection = new SqlConnection(Config.DBConnectionString))
            {
                string deleteQuery = @"DELETE FROM DiscountGeographies WHERE Id IN @removedDiscountIds";

                var parameters = new { removedDiscountIds };

                var resultData = connection.Execute(deleteQuery, parameters);
            }
        }


        public ResultDto GetGeographyList(LoginUserIdDto inputDto)
        {
            _methodName = "GetGeographyList";
            var resultDto = new ResultDto();
            var outputDto = new List<DiscountOutputDto>();
            int totalRecords = 0;
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var usercontext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (usercontext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                var userRolecontext = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (userRolecontext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                using (var con = new SqlConnection(Config.DBConnectionString))
                {
                    outputDto = con.Query<DiscountOutputDto>("[dbo].[GetGeographyDiscountDetailsForList]", new
                    {
                        DiscountInputDate = inputDto.Date,
                        RoleId = userRolecontext.RoleId,
                        ZoneId = inputDto.ZoneIds,
                        StateId = inputDto.StateIds,
                        DistrictId = inputDto.DistrictIds,
                        CityId = inputDto.CityIds,
                        pageNumber = inputDto?.DataSourceRequest != null ? (int?)inputDto.DataSourceRequest.Page : null,
                        pageSize = inputDto?.DataSourceRequest != null ? (int?)inputDto.DataSourceRequest.PageSize : null,
                        Status = inputDto.Status
                    }, commandType:System.Data.CommandType.StoredProcedure,commandTimeout : 0).ToList();
                }

                if(outputDto.Any() && outputDto != null)
                {
                    outputDto.ForEach(_ =>
                    {
                        _.EncryptedId = UtilityHelper.ConvertToMd5(_.Id.ToString(), SecurityConstants.EncryptionKey);
                        _.SkuIds = _.SkuIdsString.Split(new char [] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).Distinct().ToList();
                    });
                }

                resultDto.IsSuccess = true;
                var resultList = new { Data = outputDto, Total = outputDto.Select(_ => _.TotalRecords).FirstOrDefault()};
                resultDto.SuccessDto.Response = resultList;
                _logger.Info(JsonConvert.SerializeObject(resultList));
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetGeographyDetailsById(long geographyId)
        {
            _methodName = "GetGeographyDetailsById";
            var resultDto = new ResultDto();
            var result = new DiscountInputDto();
            try
            {

                var cityListDetail = _emamiContext.DiscountGeography.AsNoTracking().Where(w => w.Id != geographyId && w.ParentId == geographyId)
                    .Select(s => s).ToList();

                if (cityListDetail != null && cityListDetail.Any())
                {
                    var skuId = cityListDetail.FirstOrDefault().SkuId;
                    var data = cityListDetail.FirstOrDefault();
                    result.Id = data.ParentId;
                    result.DivisionId = data.OilType.DivisionId;
                    result.SalesOrganizationId = data.SalesOrganizationId;
                    result.DistributionChannelId = data.DistributionChannelId;
                    result.DiscountReason = data.DiscountReason;
                    result.OilTypeId = data.OilTypeId;
                    result.ActualDiscount = data.ActualDiscount;
                    result.ValidFrom = data.ValidFrom;
                    result.ValidTo = data.ValidTo;
                    result.OilPackingTypeId = (long)_emamiContext.Skus.FirstOrDefault(s => s.Id == skuId).PackGroupId;
                    result.PackGroupId = data.PackGroupId;
                    result.PackTypeId = data.PackTypeId;
                    result.ZoneId = cityListDetail.Select(s => s.ZoneId).Distinct().ToList();
                    result.StateId = cityListDetail.Select(s => s.StateId).Distinct().ToList();
                    result.TerritoryId = cityListDetail.Select(s => s.TerritoryId).Distinct().ToList();
                    result.DistrictId = cityListDetail.Select(s => s.DistrictId).Distinct().ToList();
                    result.CityId = cityListDetail.Select(s => s.CityId).Distinct().ToList();
                    result.SkuIds = cityListDetail.Select(s => s.SkuId).Distinct().ToList();
                    result.IsActive = data.IsActive;

                    cityListDetail.ForEach(f =>
                    {
                        DiscountSkuCityMappingDto model = new DiscountSkuCityMappingDto()
                        {
                            ZoneId = f.ZoneId,
                            StateId = f.StateId,
                            TerritoryId = f.TerritoryId,
                            DistrictId = f.DistrictId,
                            CityId = f.CityId
                        };

                        if (!result.Cities.Any(w => w.CityId == model.CityId))
                        {
                            result.Cities.Add(model);
                        }
                    });
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = Constants.RecordNotFound;
                }

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

        public ResultDto GetGeographyCityList(GeographyCityListParam inputDto)
        {
            _methodName = "GetGeographyCityList";
            _logger.Info($"Json GetGeographyCityList : {JsonConvert.SerializeObject(inputDto)}");
            var resultDto = new ResultDto();

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

            using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
            {
                try
                {
                    connection.Open();

                    var parameters = new
                    {
                        ParentId = inputDto.Id,
                        PageNumber = inputDto.PageNumber,
                        PageSize = inputDto.PageSize,
                        ZoneId = inputDto.ZoneIds,
                        StateId = inputDto.StateIds,
                        DistrictId = inputDto.DistrictIds,
                        CityId = inputDto.CityIds
                    };

                    var cityListDetail = connection.Query<CityDetails>("GetGeographyCityList",parameters,commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (!cityListDetail.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                    else
                    {
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = cityListDetail;
                    }
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                        _logger.Error(message, exception);

                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.Exception;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            _logger.Info($"Json GetGeographyCityListResponse : {JsonConvert.SerializeObject(resultDto)}");
            return resultDto;
        }

        public ResultDto GetGeographyCityListMobile(GeographyDiscountCityListParam inputDto)
        {
            _methodName = "GetGeographyCityList";
            _logger.Info($"Json GetGeographyCityList : {JsonConvert.SerializeObject(inputDto)}");
            var resultDto = new ResultDto();

            if (inputDto == null)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidRequest;
                return resultDto;
            }

            using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
            {
                try
                {
                    connection.Open();

                    var parameters = new
                    {
                        ParentId = inputDto.ParentId,
                        PageNumber = inputDto.PageNumber,
                        PageSize = inputDto.PageSize
                    };

                    var cityListDetail = connection.Query<CityMobileDetails>("GetGeographyCityMobileList", parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();

                    if (!cityListDetail.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
                    else
                    {
                        var skuIds = cityListDetail.Select(c => c.SkuId).Distinct().ToList();
                        var stateIds = cityListDetail.Select(c => c.StateId).Distinct().ToList();
                        var zoneIds = cityListDetail.Select(c => c.ZoneId).Distinct().ToList();

                        var mappedCities = cityListDetail.Select(city => new DiscountSkuCityMappingDto
                        {
                            ZoneId = city.ZoneId,
                            StateId = city.StateId,
                            TerritoryId = city.TerritoryId,
                            DistrictId = city.DistrictId,
                            CityId = city.CityId
                        }).ToList();

                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = new Dictionary<string, object>
                        {
                            { "skuIds", skuIds },
                            { "stateIds", stateIds },
                            { "zoneIds", zoneIds },
                            { "cities", mappedCities }
                        };
                    }
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                    _logger.Error(message, exception);

                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.Exception;
                }
                finally
                {
                    connection.Close();
                }
            }
            _logger.Info($"Json GetGeographyCityListResponse : {JsonConvert.SerializeObject(resultDto)}");
            return resultDto;
        }


        #endregion

        #region Discount Users
        public bool IsValidClaim(long loginUserId, Enum value)
        {
            #region Claims Check
            UserIdDto userIdDto = new UserIdDto() { UserId = loginUserId };
            var userClaims = GetUserRoleClaims(userIdDto);

            if (userClaims != null)
            {
                if (userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(value) && _.IsApplied))
                    return true;

                return userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(value) && _.IsApplied);
            }
            else
            {
                return false;
            }

            #endregion
        }

        public ResultDto AddDiscountUsers(DiscountUserDto inputDto)
        {
            _methodName = "AddDiscountUsers";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            bool isFirstRecord = false;
            long parentId = 0;
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.EmployeeIsEmpty;
                    return resultDto;
                }

                if (inputDto.StateIds == null || !inputDto.StateIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.StateEmpty;
                    return resultDto;
                }

                if (inputDto.SkuIds == null || !inputDto.SkuIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.SkuIsEmpty;
                    return resultDto;
                }

                #region Validation

                var userRoleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(f => f.UserId == inputDto.LoginUserId).RoleId;
                if (userRoleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    var discountPercentage = _emamiContext.DiscountUsers.AsNoTracking()
                   .FirstOrDefault(w => w.UserId == inputDto.LoginUserId
                   // && w.OilTypeId == inputDto.OilTypeId
                   && inputDto.SkuIds.Contains(w.SkuId)
                   && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                      && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                      || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                      && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo))));

                    if (discountPercentage != null && !(inputDto.ActualDiscount <= discountPercentage.ActualDiscount))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.DiscountAlreadyExistiInThisUser + discountPercentage.ActualDiscount;
                        return resultDto;
                    }
                }

                #endregion

                if (inputDto != null && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                {

                    #region Validation
                    //var userIds = inputDto.CustomerId;
                    //var geographyDiscountCount = _emamiContext.DiscountUsers.AsNoTracking()
                    //  .Where(w => w.OilTypeId == inputDto.OilTypeId
                    //  && inputDto.SkuIds.Contains(w.SkuId)  // == inputDto.SkuId
                    //  && userIds.Contains(w.UserId)
                    //  && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //   && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                    //   || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //   && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).Select(s => s.UserId).ToList();

                    //if (geographyDiscountCount != null && geographyDiscountCount.Any())
                    //{
                    //    var userName = _emamiContext.Users.AsNoTracking().Where(w => userIds.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                    //    return _resultService.ErrorMessage(Constants.DiscountAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)));
                    //}
                    #endregion

                    // var DiscountContext = _emamiContext.DiscountUsers.AsNoTracking()
                    //.Where(w => inputDto.CustomerId.Contains(w.UserId)
                    //&& w.OilTypeId == inputDto.OilTypeId
                    //&& inputDto.SkuIds.Contains(w.SkuId)
                    //&& ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //   && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                    //   || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                    //   && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).ToList();

                    // if (DiscountContext != null && DiscountContext.Any())
                    // {
                    //     foreach (var discount in DiscountContext)
                    //     {
                    //         var updateDiscountContext = _emamiContext.DiscountUsers.FirstOrDefault(_ => _.Id == discount.Id);
                    //         updateDiscountContext.Status = false;
                    //         _emamiContext.SaveChanges();
                    //     }
                    // }
                    foreach (var stateId in inputDto.StateIds)
                    {
                        foreach (var skuId in inputDto.SkuIds)
                        {
                            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(a => a.Id == skuId);
                            foreach (var userID in inputDto.CustomerId)
                            {
                                if (!isFirstRecord)
                                {
                                    var entity = new DiscountUsers()
                                    {
                                        SalesOrganizationId = skuContext.SalesOrganizationId,
                                        DistributionChannelId = skuContext.DistributionChannelId,
                                        DivisionId = skuContext.DivisionId,
                                        DiscountReason = inputDto.DiscountReason,
                                        SkuId = skuId,
                                        UserId = userID,
                                        SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                        Status = true,
                                        ActualDiscount = inputDto.ActualDiscount,
                                        ValidFrom = inputDto.ValidFrom,
                                        ValidTo = inputDto.ValidTo,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        OilTypeId = skuContext.OilTypeId ?? 0,
                                        ParentId = parentId
                                    };
                                    _emamiContext.DiscountUsers.Add(entity);
                                    _emamiContext.SaveChanges();
                                    isFirstRecord = true;
                                    parentId = entity.Id;
                                }

                                if (isFirstRecord)
                                {
                                    var entity = new DiscountUsers()
                                    {
                                        SalesOrganizationId = skuContext.SalesOrganizationId,
                                        DistributionChannelId = skuContext.DistributionChannelId,
                                        DivisionId = skuContext.DivisionId,
                                        DiscountReason = inputDto.DiscountReason,
                                        SkuId = skuId,
                                        UserId = userID,
                                        SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                        Status = true,
                                        ActualDiscount = inputDto.ActualDiscount,
                                        ValidFrom = inputDto.ValidFrom,
                                        ValidTo = inputDto.ValidTo,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        OilTypeId = skuContext.OilTypeId ?? 0,
                                        ParentId = parentId,
                                        StateId = stateId
                                    };
                                    _emamiContext.DiscountUsers.Add(entity);
                                }
                            }
                        }
                    }

                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = 1;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public bool IsNotNullOrEmpty(params object[] listParams)
        {
            foreach (var data in listParams)
            {

            }
            return true;
        }

        public ResultDto UpdateDiscountUsers(DiscountUserDto inputDto)
        {
            _methodName = "UpdateDiscountUsers";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            var isExistsData = false;

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.EmployeeIsEmpty;
                    return resultDto;
                }

                if (inputDto.StateIds == null || !inputDto.StateIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.StateEmpty;
                    return resultDto;
                }

                if (inputDto.SkuIds == null || !inputDto.SkuIds.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.SkuIsEmpty;
                    return resultDto;
                }

                #region Validation
                //var userId = inputDto.CustomerId;
                //var details = _emamiContext.DiscountUsers.AsNoTracking()
                //.Where(w => w.OilTypeId == inputDto.OilTypeId
                //&& inputDto.SkuIds.Contains(w.SkuId) // && (w.Id == inputDto.Id && w.ParentId == inputDto.Id)
                //&& userId.Contains(w.UserId)
                //&& ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                //&& DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                //|| (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                //&& DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).ToList();

                //if (details != null && details.Any())
                //{
                //    var notWithinCurrentDiscount = details.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.UserId).ToList();
                //    if (notWithinCurrentDiscount != null && notWithinCurrentDiscount.Any() && notWithinCurrentDiscount.Count > 0)
                //    {
                //        var userName = _emamiContext.Users.AsNoTracking().Where(w => notWithinCurrentDiscount.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                //        return _resultService.ErrorMessage(Constants.DiscountAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)));
                //    }
                //}


                #endregion

                var DiscountUserData = _emamiContext.DiscountUsers.AsNoTracking().Where(f => f.ParentId == inputDto.Id);

                if (IsValidClaim(inputDto.LoginUserId, Claims.ManageDiscounts))
                {
                    var discountDatas = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id).Select(s => new { SkuIds = s.SkuId, UserIds = s.UserId, StateId = s.StateId }).ToList();
                    var dbEmployess = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.UserIds).Distinct().ToList() : null;
                    var dbSkus = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.SkuIds).Distinct().ToList() : null;
                    var dbStates = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.StateId).Distinct().ToList() : null;


                    //Get Removed Employees and SKU's
                    IsNotNullOrEmpty(dbEmployess, dbSkus);
                    var removedEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                        ? dbEmployess.Where(w => !inputDto.CustomerId.Contains(w)).Distinct().ToList() : null;
                    var removedSkus = (dbSkus != null && dbSkus.Any() && inputDto.SkuIds != null && inputDto.SkuIds.Any()) ? dbSkus.Where(w => !inputDto.SkuIds.Contains(w)).Distinct().ToList() : null;

                    var removedStates = (dbStates != null && dbStates.Any() && inputDto.StateIds != null && inputDto.StateIds.Any()) ? dbStates.Where(w => !inputDto.StateIds.Contains(w)).Distinct().ToList() : null;

                    if (removedStates != null && removedStates.Any())
                    {
                        var removedData = _emamiContext.DiscountUsers.Where(f => removedStates.Contains(f.StateId) && f.ParentId == inputDto.Id).ToList();
                        if (removedData != null && removedData.Any())
                        {
                            removedData.ForEach(f => _emamiContext.DiscountUsers.Remove(f));
                            _emamiContext.SaveChanges();
                        }
                    }



                    if (removedEmployees != null && removedEmployees.Any())
                    {
                        var removedData = _emamiContext.DiscountUsers.Where(f => removedEmployees.Contains(f.UserId) && f.ParentId == inputDto.Id).ToList();
                        if (removedData != null && removedData.Any())
                        {
                            removedData.ForEach(f => _emamiContext.DiscountUsers.Remove(f));
                            _emamiContext.SaveChanges();
                        }
                    }

                    if (removedSkus != null && removedSkus.Any())
                    {
                        var removedData = _emamiContext.DiscountUsers.Where(f => removedSkus.Contains(f.SkuId) && f.ParentId == inputDto.Id).ToList();
                        if (removedData != null && removedData.Any())
                        {
                            removedData.ForEach(f => _emamiContext.DiscountUsers.Remove(f));
                            _emamiContext.SaveChanges();
                        }
                    }

                    var newEmployees = inputDto.CustomerId.Where(w => !dbEmployess.Contains(w)).Distinct().ToList();
                    var newSkus = inputDto.SkuIds.Where(w => !dbSkus.Contains(w)).Distinct().ToList();
                    var newStates = inputDto.StateIds.Where(w => !dbStates.Contains(w)).Distinct().ToList();

                    if (newSkus != null && newSkus.Any())
                    {
                        foreach (var skuId in newSkus)
                        {
                            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(a => a.Id == skuId);
                            foreach (var userID in inputDto.CustomerId)
                            {
                                foreach (var stateId in inputDto.StateIds)
                                {
                                    isExistsData = _emamiContext.DiscountUsers.AsNoTracking().Any(f => f.ParentId == inputDto.Id && f.SkuId == skuId && f.UserId == userID && f.StateId == stateId);
                                    if (!isExistsData)
                                    {
                                        if (!isFirstRecord)
                                        {
                                            var entity = new DiscountUsers()
                                            {
                                                SalesOrganizationId = skuContext.SalesOrganizationId,
                                                DistributionChannelId = skuContext.DistributionChannelId,
                                                DivisionId = skuContext.DivisionId,
                                                DiscountReason = inputDto.DiscountReason,
                                                SkuId = skuId,
                                                UserId = userID,
                                                SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                                Status = true,
                                                ActualDiscount = inputDto.ActualDiscount,
                                                ValidFrom = inputDto.ValidFrom,
                                                ValidTo = inputDto.ValidTo,
                                                CreatedBy = inputDto.LoginUserId,
                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                OilTypeId = skuContext.OilTypeId ?? 0,
                                                ParentId = inputDto.Id,
                                                StateId = stateId
                                            };
                                            _emamiContext.DiscountUsers.Add(entity);
                                            _emamiContext.SaveChanges();
                                        }
                                    }
                                }

                            }
                        }
                    }
                    if (newStates != null && newStates.Any())
                    {

                        foreach (var stateId in newStates)
                        {
                            foreach (var skuId in inputDto.SkuIds)
                            {
                                var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(a => a.Id == skuId);
                                foreach (var userID in inputDto.CustomerId)
                                {
                                    isExistsData = _emamiContext.DiscountUsers.AsNoTracking().Any(f => f.ParentId == inputDto.Id && f.SkuId == skuId && f.UserId == userID && f.StateId == stateId);
                                    if (!isExistsData)
                                    {
                                        if (!isFirstRecord)
                                        {
                                            var entity = new DiscountUsers()
                                            {
                                                SalesOrganizationId = skuContext.SalesOrganizationId,
                                                DistributionChannelId = skuContext.DistributionChannelId,
                                                DivisionId = skuContext.DivisionId,
                                                DiscountReason = inputDto.DiscountReason,
                                                SkuId = skuId,
                                                UserId = userID,
                                                SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                                Status = true,
                                                ActualDiscount = inputDto.ActualDiscount,
                                                ValidFrom = inputDto.ValidFrom,
                                                ValidTo = inputDto.ValidTo,
                                                CreatedBy = inputDto.LoginUserId,
                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                OilTypeId = skuContext.OilTypeId ?? 0,
                                                ParentId = inputDto.Id,
                                                StateId = stateId
                                            };
                                            _emamiContext.DiscountUsers.Add(entity);
                                            _emamiContext.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }

                    }

                    if (newEmployees != null && newEmployees.Any())
                    {

                        foreach (var stateId in inputDto.StateIds)
                        {
                            foreach (var skuId in inputDto.SkuIds)
                            {
                                var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(a => a.Id == skuId);
                                foreach (var userID in newEmployees)
                                {
                                    isExistsData = _emamiContext.DiscountUsers.AsNoTracking().Any(f => f.ParentId == inputDto.Id && f.SkuId == skuId && f.UserId == userID && f.StateId == stateId);
                                    if (!isExistsData)
                                    {
                                        if (!isFirstRecord)
                                        {
                                            var entity = new DiscountUsers()
                                            {
                                                SalesOrganizationId = skuContext.SalesOrganizationId,
                                                DistributionChannelId = skuContext.DistributionChannelId,
                                                DivisionId = skuContext.DivisionId,
                                                DiscountReason = inputDto.DiscountReason,
                                                SkuId = skuId,
                                                UserId = userID,
                                                SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                                Status = true,
                                                ActualDiscount = inputDto.ActualDiscount,
                                                ValidFrom = inputDto.ValidFrom,
                                                ValidTo = inputDto.ValidTo,
                                                CreatedBy = inputDto.LoginUserId,
                                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                OilTypeId = skuContext.OilTypeId ?? 0,
                                                ParentId = inputDto.Id,
                                                StateId = stateId
                                            };
                                            _emamiContext.DiscountUsers.Add(entity);
                                            _emamiContext.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                else if (IsValidClaim(inputDto.LoginUserId, Claims.ViewDiscounts))
                {
                    var parentDiscountId = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id).ParentDiscountId;
                    var discountData = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == parentDiscountId);

                    if (inputDto.ActualDiscount > discountData.ActualDiscount)
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less then or equal to discount");
                    }
                    if (!((discountData.ValidFrom >= inputDto.ValidFrom
                            && discountData.ValidFrom <= inputDto.ValidTo)
                            || (discountData.ValidTo >= inputDto.ValidFrom
                            && discountData.ValidTo <= inputDto.ValidTo)))
                    {
                        return _resultService.ErrorMessage("Discount date range is " + discountData.ValidFrom.ToString("dd-MMM-yyyy HH:mm") + " - " + discountData.ValidTo.ToString("dd-MMM-yyyy HH:mm") + ". Please select dates between the range");
                    }

                    var discountDatas = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id).Select(s => new { SkuIds = s.SkuId, UserIds = s.UserId }).ToList();
                    var dbEmployess = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.UserIds).Distinct().ToList() : null;
                    var dbSkus = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.SkuIds).Distinct().ToList() : null;

                    //Get Removed Employees
                    var removedEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                        ? dbEmployess.Where(w => !inputDto.CustomerId.Contains(w)).Distinct().ToList() : null;

                    if (removedEmployees != null && removedEmployees.Any())
                    {
                        var removedData = _emamiContext.DiscountUsers.Where(f => removedEmployees.Contains(f.UserId) && f.ParentId == inputDto.Id);
                        if (removedData != null)
                        {
                            removedData.ToList().ForEach(f => _emamiContext.DiscountUsers.Remove(f));
                            _emamiContext.SaveChanges();
                        }
                    }

                    var newEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                        ? inputDto.CustomerId.Where(w => !dbEmployess.Contains(w)) : null;

                    if (newEmployees != null && newEmployees.Any())
                    {
                        foreach (var skuId in inputDto.SkuIds)
                        {
                            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(a => a.Id == skuId);
                            foreach (var userID in newEmployees)
                            {
                                if (!isFirstRecord)
                                {
                                    var entity = new DiscountUsers()
                                    {
                                        SalesOrganizationId = skuContext.SalesOrganizationId,
                                        DistributionChannelId = skuContext.DistributionChannelId,
                                        DivisionId = skuContext.DivisionId,
                                        DiscountReason = inputDto.DiscountReason,
                                        SkuId = skuId,
                                        UserId = userID,
                                        SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                        Status = true,
                                        ActualDiscount = inputDto.ActualDiscount,
                                        ValidFrom = inputDto.ValidFrom,
                                        ValidTo = inputDto.ValidTo,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        OilTypeId = skuContext.OilTypeId ?? 0,
                                        ParentId = inputDto.Id,
                                        ParentDiscountId = parentDiscountId
                                    };
                                    _emamiContext.DiscountUsers.Add(entity);
                                    _emamiContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var discounts = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id || f.Id == inputDto.Id).ToList();
                if (discounts != null && discounts.Any())
                {
                    foreach (var discount in discounts)
                    {
                        discount.ActualDiscount = inputDto.ActualDiscount;
                        discount.DiscountReason = inputDto.DiscountReason;
                        discount.ValidFrom = inputDto.ValidFrom;
                        discount.ValidTo = inputDto.ValidTo;
                        discount.ModifiedBy = inputDto.LoginUserId;
                        discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = 1;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetDiscountUserList(LoginUserIdDto inputDto)
        {
            _methodName = "GetDiscountUserList";
            var resultDto = new ResultDto();
            var discountUsers = new List<DiscountUsers>();
            var result = new List<DiscountUserDto>();
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    discountUsers = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.CreatedBy == inputDto.LoginUserId &&
                    DbFunctions.TruncateTime(inputDto.Date) >= DbFunctions.TruncateTime(w.ValidFrom)
                  && DbFunctions.TruncateTime(inputDto.Date) <= DbFunctions.TruncateTime(w.ValidTo)
                    //&& (inputDto.VerticalId > 0 ? w.OilType.DivisionId == inputDto.VerticalId : w.OilType.DivisionId > 0)
                    ).ToList();
                }
                else
                {
                    discountUsers = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.Status && w.CreatedBy == inputDto.LoginUserId &&
                     DbFunctions.TruncateTime(inputDto.Date) >= DbFunctions.TruncateTime(w.ValidFrom)
                  && DbFunctions.TruncateTime(inputDto.Date) <= DbFunctions.TruncateTime(w.ValidTo)
                    //&& (inputDto.VerticalId > 0 ? w.OilType.DivisionId == inputDto.VerticalId : w.OilType.DivisionId > 0)
                    ).ToList();
                }

                if (discountUsers.Any())
                {
                    result = discountUsers.Where(s => s.ParentId == 0).AsEnumerable().Select(s => new DiscountUserDto()
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                        Id = s.Id,
                        SalesOrganizationId = s.SalesOrganizationId,
                        SalesOrganization = s.SalesOrganization.Name,
                        DistributionChannelId = s.DistributionChannelId,
                        DistributionChannel = s.DistributionChannel.Name,
                        DivisionId = s.DivisionId,
                        Division = s.Division.Name,
                        DiscountReason = s.DiscountReason,
                        SkuIds = discountUsers.Where(a => a.ParentId == s.Id).Select(b => b.SkuId).Distinct().ToList(),
                        CustomerId = discountUsers.Where(a => a.ParentId == s.Id).Select(b => b.UserId).Distinct().ToList(),
                        SkuName = string.Join(",", discountUsers.Where(a => a.ParentId == s.Id).Select(b => b.Sku.SkuName).Distinct().ToList()),
                        StateIds = discountUsers.Where(a => a.ParentId == s.Id).Select(b => b.StateId).Distinct().ToList(),
                        //SkuId = s.SkuId,
                        //OilPackingTypeId= (long)s.Sku.PackGroupId,
                        //SkuName = s.Sku.SkuName,
                        //SkuCode = s.Sku.SkuCode,
                        // OilTypeName = s.OilType != null ? s.OilType.Name+"-"+s.OilType.SalesOrganization.Code+"/"+s.OilType.DistributionChannel.Code+"/"+s.Division.Code : String.Empty,
                        //OilTypeCode = s.OilType?.SAPCode,
                        ActualDiscount = s.ActualDiscount,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList();
                }
                //}).ToList();

                //if (result != null && result.Any())
                //{
                //    foreach (var item in result)
                //    {
                //        var childrows = _emamiContext.DiscountUsers.Where(_ => _.ParentId == item.Id && _.Status).ToList();
                //        if(childrows != null && childrows.Any())
                //        {
                //            item.IsActive = true;
                //        }
                //        else
                //        {
                //            item.IsActive = false;
                //        }
                //    }
                //}
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


        public ResultDto DiscountUserExport(LoginUserIdDto inputDto)
        {
            _methodName = "DiscountUserExport";
            var resultDto = new ResultDto();
            var discountUsers = new List<DiscountUsers>();
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    discountUsers = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId && (inputDto.VerticalId > 0 ? w.OilType.DivisionId == inputDto.VerticalId : w.OilType.DivisionId > 0)
                    && DbFunctions.TruncateTime(inputDto.Date) >= DbFunctions.TruncateTime(w.ValidFrom)
                  && DbFunctions.TruncateTime(inputDto.Date) <= DbFunctions.TruncateTime(w.ValidTo)).ToList();
                }
                else
                {
                    discountUsers = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.Status && w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId && (inputDto.VerticalId > 0 ? w.OilType.DivisionId == inputDto.VerticalId : w.OilType.DivisionId > 0)
                    && DbFunctions.TruncateTime(inputDto.Date) >= DbFunctions.TruncateTime(w.ValidFrom)
                  && DbFunctions.TruncateTime(inputDto.Date) <= DbFunctions.TruncateTime(w.ValidTo)).ToList();
                }

                var statecontext = _emamiContext.State.AsNoTracking();

                var result = discountUsers.Select(s => new DiscountExportDto()
                {
                    Id = s.Id,
                    SalesOrganizationId = s.SalesOrganizationId,
                    SalesOrganization = s.SalesOrganization.Name,
                    DistributionChannelId = s.DistributionChannelId,
                    DistributionChannel = s.DistributionChannel.Name,
                    DivisionId = s.DivisionId,
                    Division = s.Division.Name,
                    //OilTypeName = s.OilType != null ? s.OilType.Name + "-" + s.OilType.SalesOrganization.Code + "/" + s.OilType.DistributionChannel.Code + "/" + s.Division.Code : String.Empty,
                    //OilTypeCode = s.OilType?.SAPCode,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo
                }).ToList();

                result.ForEach(item =>
                {
                    item.DiscountSkuDataList = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.ParentId == item.Id)
                   .Select(s => new DiscountExportInnerData()
                   {
                       SkuName = s.Sku.SkuName,
                       SkuCode = s.Sku.SkuCode,
                       Discount = s.ActualDiscount,
                       EmployeeName = s.User.Name,
                       Email = s.User.Email,
                       MobileNumber = s.User.MobileNumber,
                       State = statecontext.FirstOrDefault(_ => _.Id == s.StateId) != null ? statecontext.FirstOrDefault(_ => _.Id == s.StateId).StateName : string.Empty
                   }).ToList();
                });

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = discountUsers;
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
        public ResultDto GetDiscountUserById(long discountId)
        {
            _methodName = "GetDiscountUserById";
            var resultDto = new ResultDto();
            var discountUsers = new List<DiscountUsers>();
            try
            {

                discountUsers = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.Id != 0 && w.ParentId == discountId).ToList();
                if (discountUsers != null && discountUsers.Any())
                {
                    var data = discountUsers.FirstOrDefault();
                    var parentDiscountUsers = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(w => w.Id == data.ParentDiscountId);
                    List<long> discountUserIds = discountUsers.Select(_ => _.Id).ToList();
                    var childDiscountUsers = _emamiContext.DiscountUsers.AsNoTracking().Where(_ => discountUserIds.Contains(_.ParentDiscountId)).ToList();
                    bool IsProcessed = false;
                    if (childDiscountUsers != null && childDiscountUsers.Any())
                    {
                        IsProcessed = true;
                    }
                    var result = new DiscountUserDto()
                    {
                        Id = discountId,
                        SalesOrganizationId = data.SalesOrganizationId,
                        SalesOrganization = data.SalesOrganization.Name,
                        DistributionChannelId = data.DistributionChannelId,
                        DistributionChannel = data.DistributionChannel.Name,
                        DivisionId = data.DivisionId,
                        Division = data.Division.Name,
                        DiscountReason = data.DiscountReason,
                        // OilTypeId = data.OilTypeId,
                        SkuId = data.SkuId,
                        OilPackingTypeId = (long)data.Sku.PackGroupId,
                        ActualDiscount = data.ActualDiscount,
                        ValidFrom = data.ValidFrom,
                        ValidTo = data.ValidTo,
                        SkuIds = discountUsers.Select(s => s.SkuId).Distinct().ToList(),
                        CustomerId = discountUsers.Select(s => s.UserId).Distinct().ToList(),
                        StateIds = discountUsers.Select(s => s.StateId).Distinct().ToList(),
                        ParentValidFrom = parentDiscountUsers != null ? parentDiscountUsers.ValidFrom.ToString("dd-MMM-yyyy") : "",
                        ParentValidTo = parentDiscountUsers != null ? parentDiscountUsers.ValidTo.ToString("dd-MMM-yyyy") : "",
                        IsProcessed = IsProcessed
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

                //discountUsers = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.Id == discountId || w.ParentId == discountId).ToList();
                //if (discountUsers != null && discountUsers.Any())
                //{
                //    var data = discountUsers.FirstOrDefault();
                //    var result = new DiscountUserDto()
                //    {
                //        Id = data.Id,
                //        VerticleId = data.OilType.DivisionId,
                //        OilTypeId = data.OilTypeId,
                //        SkuId = data.SkuId,
                //        ActualDiscount = data.ActualDiscount,
                //        ValidFrom = data.ValidFrom,
                //        ValidTo = data.ValidTo,
                //        SkuIds = discountUsers.Select(s => s.SkuId).ToList(),
                //        CustomerId = discountUsers.Select(s => s.UserId).ToList()
                //    };
                //    resultDto.IsSuccess = true;
                //    resultDto.SuccessDto.Response = result;
                //}
                //else
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                //    return resultDto;
                //}
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetDiscountUserDetailList(GeographyCityListParam inputDto)
        {
            _methodName = "GetDiscountUserDetailList";
            var resultDto = new ResultDto();
            var discountlist = new List<DiscountUserQuantityOutput>();
            var discountMobilelist = new List<DiscountParentDto>();
            try
            {
                var stateContext = _emamiContext.State.AsNoTracking();
                var discountContext = _emamiContext.DiscountUsers.AsNoTracking();

                if (inputDto.IsRequestFromWeb)
                {
                    discountlist = discountContext.Where(w => w.ParentId == inputDto.Id)
                    .Select(s => new DiscountUserQuantityOutput()
                    {
                        Id = s.Id,
                        SalesOrganizationId = s.SalesOrganizationId,
                        SalesOrganization = s.SalesOrganization.Name,
                        DistributionChannelId = s.DistributionChannelId,
                        DistributionChannel = s.DistributionChannel.Name,
                        DivisionId = s.DivisionId,
                        Division = s.Division.Name,
                        DiscountReason = s.DiscountReason,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        Discount = s.ActualDiscount,
                        EmployeeName = s.User.Name,
                        Email = s.User.Email,
                        MobileNumber = s.User.MobileNumber,
                        StateId = s.StateId,
                        StateName = stateContext.FirstOrDefault(_ => _.Id == s.StateId) != null ? stateContext.FirstOrDefault(_ => _.Id == s.StateId).StateName : string.Empty
                        //Status = s.Status
                    }).ToList();

                    resultDto.SuccessDto.Response = discountlist;
                }
                else
                {
                    var usercontext = _emamiContext.Users.AsNoTracking();
                    discountMobilelist = discountContext.Where(w => w.ParentId == inputDto.Id)
                        .GroupBy(_ => _.UserId)
                        .Select(d => new DiscountParentDto()
                        {
                            UserId = d.Key,
                            EmployeeName = usercontext.FirstOrDefault(_ => _.Id == d.Key) != null ? usercontext.FirstOrDefault(_ => _.Id == d.Key).Name : string.Empty,
                            Email = usercontext.FirstOrDefault(_ => _.Id == d.Key) != null ? usercontext.FirstOrDefault(_ => _.Id == d.Key).Email : string.Empty,
                            MobileNumber = usercontext.FirstOrDefault(_ => _.Id == d.Key) != null ? usercontext.FirstOrDefault(_ => _.Id == d.Key).MobileNumber : string.Empty,
                            DiscountList = d.Select(s => new DiscountUserQuantityOutput()
                            {
                                Id = s.Id,
                                SalesOrganizationId = s.SalesOrganizationId,
                                SalesOrganization = s.SalesOrganization.Name,
                                DistributionChannelId = s.DistributionChannelId,
                                DistributionChannel = s.DistributionChannel.Name,
                                DivisionId = s.DivisionId,
                                Division = s.Division.Name,
                                DiscountReason = s.DiscountReason,
                                SkuName = s.Sku.SkuName,
                                SkuCode = s.Sku.SkuCode,
                                Discount = s.ActualDiscount,
                                EmployeeName = s.User.Name,
                                Email = s.User.Email,
                                MobileNumber = s.User.MobileNumber,
                                StateId = s.StateId,
                                StateName = stateContext.FirstOrDefault(_ => _.Id == s.StateId) != null ? stateContext.FirstOrDefault(_ => _.Id == s.StateId).StateName : string.Empty
                                //Status = s.Status
                            }).ToList(),

                        }
                        ).ToList();

                    resultDto.SuccessDto.Response = discountMobilelist;
                }


                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public static bool EqualsTwoList<T>(IList<T> col1, IList<T> col2)
        {
            if (col1 == null || col2 == null)
                return false;

            if (col1.Count != col2.Count)
                return false;

            return col1.SequenceEqual(col2);
        }

        #endregion

        #region PriceNotifyConfiguration
        public ResultDto AddorUpdatePriceNotifyConfiguration(PriceNotifyConfigurationDto inputDto)
        {
            _methodName = "AddorUpdatePriceNotifyConfiguration";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null || inputDto.CityIds == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (inputDto != null && inputDto.CityIds != null && inputDto.CityIds.Any())
                {
                    #region Validation
                    var cityIds = inputDto.CityIds;//inputDto.Cities.Select(s => s.CityId).ToList();

                    var pricenotifyConfigurationCityContext = _emamiContext.PriceNotifyConfiguration.AsNoTracking()
                       .Where(w => (DbFunctions.TruncateTime(w.NotificationDate) == DbFunctions.TruncateTime(inputDto.NotificationDate))
                        ).Select(s => s.CityId).ToList();

                    if (pricenotifyConfigurationCityContext != null && pricenotifyConfigurationCityContext.Any())
                    {
                        foreach (var city in pricenotifyConfigurationCityContext)
                        {
                            var dbCityIds = UtilityHelper.ConvertStringToLongList(city);
                            var isExistCity = cityIds.Any(a => dbCityIds.Contains(a));
                            if (isExistCity)
                            {
                                return _resultService.ErrorMessage(Constants.PricingNotifyConfigurationAlreadyExistiInThisCity);
                            }
                        }
                    }
                    #endregion

                    var entity = new PriceNotifyConfiguration()
                    {
                        IncoTermId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.IncoTermId),
                        CityId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.CityIds.ToList()),
                        IsSMS = inputDto.IsSMS,
                        IsEmail = inputDto.IsEmail,
                        StateId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateId),
                        TerritoryId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.TerritoryId),
                        ZoneId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.ZoneId),
                        IsPushNotification = inputDto.IsPushNotification,
                        NotificationDate = inputDto.NotificationDate,
                        SkuId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.SkuId),
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.PriceNotifyConfiguration.Add(entity);
                    _emamiContext.SaveChanges();
                    resultDto.IsSuccess = true;
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

        public ResultDto GetPriceNotifyConfigurationList(SaudaLimitInputDto inputDto)
        {
            _methodName = "GetPriceNotifyConfigurationList";
            var resultDto = new ResultDto();
            var priceNotifyConfigurationDto = new List<PriceNotifyConfigurationDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
            {
                return _resultService.ErrorMessage(Constants.FromDateEmpty);
            }
            if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
            {
                return _resultService.ErrorMessage(Constants.ToDateEmpty);
            }
            if (inputDto.FromDate > inputDto.ToDate)
            {
                return _resultService.ErrorMessage(Constants.FromDateInvalid);
            }
            if (inputDto.LoginUserId == 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            if (!_resultService.UserIsAcive(inputDto.LoginUserId))
            {
                return _resultService.ErrorMessage(Constants.InvalidUser);
            }


            try
            {
                var fromDate = inputDto.FromDate.Date.AddSeconds(1);
                var toDate = inputDto.ToDate.Date.AddDays(1).AddSeconds(-1);
                fromDate = fromDate.ToUniversalTime();
                toDate = toDate.ToUniversalTime();
                priceNotifyConfigurationDto = _emamiContext.PriceNotifyConfiguration.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(fromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(toDate)).AsEnumerable().Select(c => new PriceNotifyConfigurationDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = c.Id,
                    NotificationDate = c.NotificationDate,
                    IsSMS = c.IsSMS,
                    IsEmail = c.IsEmail,
                    IsPushNotification = c.IsPushNotification,
                    IncoTerms = c.IncoTermId,
                    HasChildren = string.IsNullOrEmpty(c.CityId) ? false : true
                }).ToList();

                foreach (var item in priceNotifyConfigurationDto)
                {
                    var incoTerms = UtilityHelper.ConvertStringToLongList(item.IncoTerms);
                    item.IncoTerms = string.Empty;
                    foreach (var incoTermId in incoTerms)
                    {
                        item.IncoTerms = item.IncoTerms + _emamiContext.IncoTerms.AsNoTracking().Where(_ => _.Id == incoTermId).Select(_ => _.Name).Single() + ", ";
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = priceNotifyConfigurationDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
                return resultDto;
            }
        }

        public ResultDto GetPriceNotifyConfigurationCityList(IdInputDto inputDto)
        {
            _methodName = "GetPriceNotifyConfigurationCityList";
            var resultDto = new ResultDto();
            var cityList = new List<CityDetails>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var cityListDetail = UtilityHelper.ConvertStringToLongList(_emamiContext.PriceNotifyConfiguration.AsNoTracking().Where(w => w.Id == inputDto.Id)
                    .Select(s => s.CityId).Single());

                if (cityListDetail != null && cityListDetail.Any())
                {
                    foreach (var item in cityListDetail)
                    {
                        CityDetails city = new CityDetails();
                        var cityContext = _emamiContext.City.AsNoTracking().Where(_ => _.Id == item).FirstOrDefault();
                        city.CityId = cityContext.Id;
                        city.CityName = cityContext.CityName;
                        city.DistrictId = cityContext.DistrictId;
                        city.DistrictName = cityContext.District.DistrictName;
                        //city.TerritoryId = cityContext.TerritoryId;
                        //city.TerritoryName = cityContext.Territory.Name;
                        city.StateId = cityContext.District.StateId;
                        city.StateName = cityContext.District.State.StateName;
                        city.ZoneId = _emamiContext.ZoneStateMappings.AsNoTracking().Where(_ => _.StateId == cityContext.District.StateId).Select(_ => _.Zone.Id).FirstOrDefault();
                        city.ZoneName = _emamiContext.ZoneStateMappings.AsNoTracking().Where(_ => _.StateId == cityContext.District.StateId).Select(_ => _.Zone.Name).FirstOrDefault(); ;
                        cityList.Add(city);
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetPriceNotifyconfigurationDetailsById(long priceNotifyId)
        {
            _methodName = "GetPriceNotifyconfigurationDetailsById";
            var resultDto = new ResultDto();
            var result = new PriceNotifyConfigurationDto();
            try
            {

                var cityListDetail = _emamiContext.PriceNotifyConfiguration.AsNoTracking().FirstOrDefault(w => w.Id == priceNotifyId);
                //.Select(s => s).ToList();

                if (cityListDetail != null)
                {
                    //var data = cityListDetail.FirstOrDefault();
                    result.Id = cityListDetail.Id;
                    result.NotificationDate = cityListDetail.NotificationDate;
                    result.IsEmail = cityListDetail.IsEmail;
                    result.IsSMS = cityListDetail.IsSMS;
                    result.IsPushNotification = cityListDetail.IsPushNotification;
                    result.IncoTermId = UtilityHelper.ConvertStringToLongList(cityListDetail.IncoTermId);
                    result.ZoneId = UtilityHelper.ConvertStringToLongList(cityListDetail.ZoneId);
                    result.StateId = UtilityHelper.ConvertStringToLongList(cityListDetail.StateId);
                    result.TerritoryId = UtilityHelper.ConvertStringToLongList(cityListDetail.TerritoryId);
                    result.CityId = UtilityHelper.ConvertStringToLongList(cityListDetail.CityId);
                    result.SkuId = UtilityHelper.ConvertStringToLongList(cityListDetail.SkuId);

                    var skuId = result.SkuId.Select(s => s).FirstOrDefault();
                    var sku = _emamiContext.Skus.FirstOrDefault(f => f.Id == skuId);
                    result.OilTypeId = sku.OilTypeId;
                    result.VerticalId = sku.OilType.DivisionId;
                    result.SalesOrganizationId = sku.OilType.SalesOrganizationId;
                    result.DistributionChannelId = sku.OilType.DistributionChannelId;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = Constants.RecordNotFound;
                }

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

        public ResultDto UpdatePriceNotifyconfiguration(PriceNotifyConfigurationDto inputDto)
        {
            _methodName = "UpdatePriceNotifyconfiguration";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto != null && inputDto.CityIds != null && inputDto.CityIds.Any())
                {
                    var priceNotifyConfiguration = _emamiContext.PriceNotifyConfiguration.Where(w => w.Id == inputDto.Id).FirstOrDefault();

                    #region Validation
                    var cityIds = inputDto.CityIds.ToList();


                    var pricenotifyConfigurationCityContext = _emamiContext.PriceNotifyConfiguration.AsNoTracking()
                       .Where(w => w.Id != inputDto.Id && (DbFunctions.TruncateTime(w.NotificationDate) == DbFunctions.TruncateTime(inputDto.NotificationDate))
                        ).Select(s => s.CityId).ToList();

                    if (pricenotifyConfigurationCityContext != null && pricenotifyConfigurationCityContext.Any())
                    {
                        foreach (var city in pricenotifyConfigurationCityContext)
                        {
                            var dbCityIds = UtilityHelper.ConvertStringToLongList(city);
                            var isExistCity = cityIds.Any(a => dbCityIds.Contains(a));
                            if (isExistCity)
                            {
                                return _resultService.ErrorMessage(Constants.PricingNotifyConfigurationAlreadyExistiInThisCity);
                            }
                        }
                    }
                    #endregion

                    if (priceNotifyConfiguration != null)
                    {
                        priceNotifyConfiguration.IncoTermId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.IncoTermId);
                        priceNotifyConfiguration.CityId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.CityIds.ToList());
                        priceNotifyConfiguration.IsSMS = inputDto.IsSMS;
                        priceNotifyConfiguration.IsEmail = inputDto.IsEmail;
                        priceNotifyConfiguration.StateId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateId);
                        priceNotifyConfiguration.TerritoryId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.TerritoryId);
                        priceNotifyConfiguration.ZoneId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.ZoneId);
                        priceNotifyConfiguration.IsPushNotification = inputDto.IsPushNotification;
                        priceNotifyConfiguration.NotificationDate = inputDto.NotificationDate;
                        priceNotifyConfiguration.SkuId = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.SkuId);
                        priceNotifyConfiguration.ModifiedBy = inputDto.LoginUserId;
                        priceNotifyConfiguration.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        _emamiContext.SaveChanges();
                    }
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

        public ResultDto GetEmployeeAndUserDiscountList(LoginUserIdDto inputDto)
        {
            _methodName = "GetEmployeeAndUserDiscountList";
            var resultDto = new ResultDto();
            var discountUsers = new List<DiscountUserDto>();
            try
            {
                var discountcontext = _emamiContext.DiscountUsers.AsNoTracking()
                  .Where(w => w.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(inputDto.Date) >= DbFunctions.TruncateTime(w.ValidFrom)
                  && DbFunctions.TruncateTime(inputDto.Date) <= DbFunctions.TruncateTime(w.ValidTo)).OrderByDescending(o => o.CreatedDate).ToList();

                var statecontext = _emamiContext.State.AsNoTracking();
                if (inputDto.IsRequestFromWeb)
                {
                    var skucontext = _emamiContext.Skus.AsNoTracking();
                    //discountUsers = discountcontext.Where(s => s.ParentId != 0).AsEnumerable()
                    //   .Select(s => new DiscountUserDto()
                    //   {
                    //       EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                    //       Id = s.Id,
                    //       SkuId = s.SkuId,
                    //       SkuName = s.Sku != null ? s.Sku.SkuName : "",
                    //       SkuCode = s.Sku != null ? s.Sku.SkuCode : "",
                    //       OilTypeId = s.OilTypeId,
                    //       OilTypeName = s.OilType != null ? s.OilType.Name : "",
                    //       ActualDiscount = s.ActualDiscount,
                    //       ValidFrom = s.ValidFrom,
                    //       ValidTo = s.ValidTo,
                    //       SkuIds = discountcontext.Where(a => a.ParentId == s.Id).Select(b => b.SkuId).Distinct().ToList(),
                    //       CustomerId = discountcontext.Where(a => a.ParentId == s.Id).Select(b => b.UserId).Distinct().ToList(),
                    //      // SkuName = string.Join(",", discountcontext.Where(a => a.ParentId == s.Id).Select(b => b.Sku.SkuName).Distinct().ToList()),
                    //       DiscountReason = s.DiscountReason
                    //   }).ToList();

                    discountUsers = discountcontext.Where(s => s.ParentId != 0).GroupBy(_ => new { _.StateId, _.ParentId })
                        .Select(s => new DiscountUserDto()
                        {
                            EncryptedId = UtilityHelper.ConvertToMd5(s.FirstOrDefault().Id.ToString(), SecurityConstants.EncryptionKey),
                            Id = s.FirstOrDefault().Id,
                            SkuId = s.FirstOrDefault().SkuId,
                            OilTypeId = s.FirstOrDefault().OilTypeId,
                            OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : "",
                            ActualDiscount = s.FirstOrDefault().ActualDiscount,
                            ValidFrom = s.FirstOrDefault().ValidFrom,
                            ValidTo = s.FirstOrDefault().ValidTo,
                            StateId = s.Key.StateId,
                            StateName = statecontext.FirstOrDefault(_ => _.Id == s.Key.StateId) != null ? statecontext.FirstOrDefault(_ => _.Id == s.Key.StateId).StateName : string.Empty,
                            //SkuIds = discountcontext.Where(a => a.ParentId == s.FirstOrDefault().Id).Select(b => b.SkuId).Distinct().ToList(),
                            //CustomerId = discountcontext.Where(a => a.ParentId == s.FirstOrDefault().Id).Select(b => b.UserId).Distinct().ToList(),
                            SkuName = string.Join(",", discountcontext.Where(a => a.ParentId == s.Key.ParentId).Select(b => b.Sku.SkuName).Distinct().ToList()),
                            DiscountReason = s.FirstOrDefault().DiscountReason,
                            SkuCode = string.Join(",", discountcontext.Where(a => a.ParentId == s.Key.ParentId).Select(b => b.Sku.SkuCode).Distinct().ToList()),
                        }).ToList();
                }
                else
                {
                    discountUsers = discountcontext.Where(s => s.ParentId != 0).GroupBy(_ => new { _.StateId, _.ParentId })
                       .Select(s => new DiscountUserDto()
                       {
                           EncryptedId = UtilityHelper.ConvertToMd5(s.FirstOrDefault().Id.ToString(), SecurityConstants.EncryptionKey),
                           Id = s.FirstOrDefault().Id,
                           SkuId = s.FirstOrDefault().SkuId,
                           OilTypeId = s.FirstOrDefault().OilTypeId,
                           OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : "",
                           ActualDiscount = s.FirstOrDefault().ActualDiscount,
                           ValidFrom = s.FirstOrDefault().ValidFrom,
                           ValidTo = s.FirstOrDefault().ValidTo,
                           SalesOrganizationId = s.FirstOrDefault().SalesOrganizationId,
                           DistributionChannelId = s.FirstOrDefault().DistributionChannelId,
                           DivisionId = s.FirstOrDefault().DivisionId,
                           StateId = s.Key.StateId,
                           StateName = statecontext.FirstOrDefault(_ => _.Id == s.Key.StateId) != null ? statecontext.FirstOrDefault(_ => _.Id == s.Key.StateId).StateName : string.Empty,
                           SkuIds = discountcontext.Where(a => a.ParentId == s.Key.ParentId).Select(b => b.SkuId).Distinct().ToList(),
                           CustomerId = discountcontext.Where(a => a.ParentId == s.Key.ParentId).Select(b => b.UserId).Distinct().ToList(),
                           SkuName = string.Join(",", discountcontext.Where(a => a.ParentId == s.Key.ParentId).Select(b => b.Sku.SkuName).Distinct().ToList()),
                           DiscountReason = s.FirstOrDefault().DiscountReason,
                           SkuCode = string.Join(",", discountcontext.Where(a => a.ParentId == s.Key.ParentId).Select(b => b.Sku.SkuCode).Distinct().ToList()),
                       }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = discountUsers;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetEmployeeAndUserDiscountById(IdInputDto inputDto)
        {
            _methodName = "GetEmployeeAndUserDiscountById";
            var resultDto = new ResultDto();
            var discountData = new DiscountUsers();
            try
            {
                discountData = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.Id);


                if (discountData != null)
                {
                    var statecontext = _emamiContext.State.AsNoTracking();
                    var result = new EmployeeUserDiscountDto()
                    {

                        Id = discountData.Id,
                        StateId = discountData.StateId,
                        StateName = statecontext.FirstOrDefault(_ => _.Id == discountData.StateId) != null ? statecontext.FirstOrDefault(_ => _.Id == discountData.StateId).StateName : string.Empty,
                        VerticleId = discountData.DivisionId,
                        SalesOrganizationId = discountData.SalesOrganizationId,
                        DistributionChannelId = discountData.DistributionChannelId,
                        OilTypeId = discountData.OilTypeId,
                        SkuId = discountData.SkuId,
                        SkuName = discountData.Sku.SkuName,
                        OilTypeName = discountData.OilType?.Name,
                        ActualDiscount = discountData.ActualDiscount,
                        ValidFrom = discountData.ValidFrom,
                        ValidTo = discountData.ValidTo,
                        SkuIds = _emamiContext.DiscountUsers.AsNoTracking().Where(s => s.ParentId == discountData.ParentId).Select(s => s.SkuId).Distinct().ToList()
                    };

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

        public ResultDto AddEmployeeAndUserDiscount(EmployeeUserDiscountDto inputDto)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
            try
            {
                if (inputDto == null || inputDto.CustomerId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var discountData = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    #region Validation

                    //var userId = inputDto.CustomerId;
                    //var details = _emamiContext.DiscountUsers.AsNoTracking()
                    //.Where(w => w.OilTypeId == inputDto.OilTypeId && w.SkuId == inputDto.SkuId // && (w.Id == inputDto.Id && w.ParentId == inputDto.Id)
                    //&& userId.Contains(w.UserId)
                    //&& ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    //&& DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))
                    //|| (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    //&& DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))));

                    //var notWithinCurrentDiscount = details.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.UserId).ToList();
                    //if (notWithinCurrentDiscount != null && notWithinCurrentDiscount.Any() && notWithinCurrentDiscount.Count > 0)
                    //{
                    //    var userName = _emamiContext.Users.AsNoTracking().Where(w => notWithinCurrentDiscount.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                    //    return _resultService.ErrorMessage(Constants.DiscountAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)));
                    //}

                    #endregion

                    if (!(inputDto.EmpValidFrom >= discountData.ValidFrom && inputDto.EmpValidFrom <= discountData.ValidTo
                        && inputDto.EmpValidTo <= discountData.ValidTo && inputDto.EmpValidTo >= discountData.ValidFrom))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Please select a Valid From and To date";
                        return resultDto;
                    }

                    if (!(inputDto.EmpActualDiscount <= discountData.ActualDiscount))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Discount limit is " + discountData.ActualDiscount + ". Please enter less than or equal to discount";
                        return resultDto;
                    }

                    foreach (var skuId in inputDto.SkuIds)
                    {
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(a => a.Id == skuId);
                        foreach (var userid in inputDto.CustomerId)
                        {
                            if (!isFirstRecord)
                            {
                                var parentDiscount = new DiscountUsers()
                                {
                                    SalesOrganizationId = skuContext.SalesOrganizationId,
                                    DistributionChannelId = skuContext.DistributionChannelId,
                                    DivisionId = skuContext.DivisionId,
                                    OilTypeId = skuContext.OilTypeId ?? 0,
                                    SkuId = skuId,
                                    UserId = userid,
                                    SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                    Status = true,
                                    ActualDiscount = inputDto.EmpActualDiscount,
                                    ValidFrom = inputDto.EmpValidFrom,
                                    ValidTo = inputDto.EmpValidTo,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    ParentId = parentId,
                                    ParentDiscountId = inputDto.Id,
                                    DiscountReason = inputDto.DiscountReason,
                                    StateId = inputDto.StateId
                                };
                                _emamiContext.DiscountUsers.Add(parentDiscount);
                                _emamiContext.SaveChanges();

                                parentId = parentDiscount.Id;
                                isFirstRecord = true;
                            }
                            if (isFirstRecord)
                            {
                                var discount = new DiscountUsers()
                                {
                                    SalesOrganizationId = skuContext.SalesOrganizationId,
                                    DistributionChannelId = skuContext.DistributionChannelId,
                                    DivisionId = skuContext.DivisionId,
                                    OilTypeId = skuContext.OilTypeId ?? 0,
                                    SkuId = skuId,
                                    UserId = userid,
                                    SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                    Status = true,
                                    ActualDiscount = inputDto.EmpActualDiscount,
                                    ValidFrom = inputDto.EmpValidFrom,
                                    ValidTo = inputDto.EmpValidTo,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    ParentId = parentId,
                                    ParentDiscountId = inputDto.Id,
                                    DiscountReason = inputDto.DiscountReason,
                                    StateId = inputDto.StateId
                                };
                                _emamiContext.DiscountUsers.Add(discount);
                            }
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

        #endregion

        #region Specialty Fat Quantity Requests

        public ResultDto AddSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto)
        {
            _methodName = "AddSpecialtyFatQuantityRequests";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto != null)
                {
                    #region Validation              
                    //if (inputDto.SkuId == 0)
                    //{
                    //    return _resultService.ErrorMessage(Constants.SkuMissing);
                    //}
                    if (inputDto.UserId == 0)
                    {
                        return _resultService.ErrorMessage(Constants.UserIdMissing);
                    }
                    #endregion
                    var exist = _emamiContext.SpecialtyFatQuantityRequestUserDetails.AsNoTracking()
                        .Join(_emamiContext.SpecialtyFatQuantityRequests.AsNoTracking(), s => s.SpecialtyFatQuantityRequestId, sr => sr.SpecialtyFatQuantityLimitId, (s, sr) => new { s, sr })
                        .FirstOrDefault(_ => _.s.UserId == inputDto.LoginUserId && _.sr.StatusId == (int)DTO.Enums.Status.Pending);
                    if (exist != null)
                    {
                        return _resultService.ErrorMessage(Constants.QuantityRequestExists);
                    }
                    var specialityfat = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SpecialtyFatQuantityLimitId);
                    if (specialityfat != null)
                    {
                        var skuIds = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OiltypeId
                        && _.SalesOrganizationId == specialityfat.SalesOrganizationId && _.DistributionChannelId == specialityfat.DistributionChannelId
                        && _.DivisionId == specialityfat.DivisionId
                    ).Select(s => s.Id).ToList();

                        foreach (var skuid in skuIds)
                        {
                            var entity = new SpecialtyFatQuantityRequest
                            {
                                SkuId = skuid,
                                Quantity = inputDto.Quantity,
                                OilTypeId = inputDto.OiltypeId,
                                StatusId = (int)DTO.Enums.Status.Pending,
                                SpecialtyFatQuantityLimitId = inputDto.SpecialtyFatQuantityLimitId,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateTime.Now,
                                DivisionId = inputDto.VerticleId
                            };
                            _emamiContext.SpecialtyFatQuantityRequests.Add(entity);
                        }
                        _emamiContext.SaveChanges();
                    }


                    var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                    {
                        UserId = inputDto.UserId,
                        SpecialtyFatQuantityRequestId = inputDto.SpecialtyFatQuantityLimitId,
                        StatusId = (int)DTO.Enums.Status.Pending,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    };
                    _emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
                    _emamiContext.SaveChanges();
                    inputDto.PostStatus = true;
                    inputDto.PostMessage = Constants.SpecialtyFatQuantityRequestsSuccess;
                    resultDto = _resultService.SuccessObject(inputDto);
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

        public ResultDto UpdateSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto)
        {
            _methodName = "UpdateSpecialtyFatQuantityRequests";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto != null)
                {

                    #region Validation              
                    if (inputDto.SkuId == 0)
                    {
                        return _resultService.ErrorMessage(Constants.SkuMissing);
                    }
                    if (inputDto.UserId == 0)
                    {
                        return _resultService.ErrorMessage(Constants.UserIdMissing);
                    }
                    #endregion

                    var specialtyFatQuantityRequests = _emamiContext.SpecialtyFatQuantityRequests.FirstOrDefault(w => w.Id == inputDto.Id);
                    if (specialtyFatQuantityRequests == null)
                    {
                        return _resultService.ErrorMessage(Constants.SpecialtyFatQuantityRequestsNotFound);
                    }
                    else
                    {
                        if (inputDto.Quantity != 0)
                        {
                            specialtyFatQuantityRequests.Quantity = inputDto.Quantity;
                        }
                        if (inputDto.StatusId != 0)
                        {
                            specialtyFatQuantityRequests.StatusId = inputDto.StatusId;
                        }
                        if (inputDto.SkuId != 0)
                        {
                            specialtyFatQuantityRequests.SkuId = inputDto.SkuId;
                            specialtyFatQuantityRequests.OilTypeId = inputDto.OiltypeId;
                        }
                        specialtyFatQuantityRequests.ModifiedBy = inputDto.LoginUserId;
                        specialtyFatQuantityRequests.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                        resultDto = _resultService.SuccessMessage(Constants.SpecialtyFatQuantityRequestsSuccessUpdated);
                    }
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

        public ResultDto GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId";
            var resultDto = new ResultDto();
            var specialtyFatQuantityRequestsList = new List<SpecialtyFatQuantityRequestDto>();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var userList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(w => w.ReportingToUserId == inputDto.LoginUserId).ToList();

                //var userList = _emamiContext.Users.AsNoTracking().Where(w => w.ReportingToId == inputDto.LoginUserId).ToList();


                specialtyFatQuantityRequestsList = (from us in userList
                                                    join sfu in _emamiContext.SpecialtyFatQuantityRequestUserDetails on us.UserId equals sfu.UserId
                                                    join sf in _emamiContext.SpecialtyFatQuantityRequests on sfu.SpecialtyFatQuantityRequestId equals sf.SpecialtyFatQuantityLimitId
                                                    join createus in _emamiContext.Users on sf.CreatedBy equals createus.Id
                                                    //join sfuqu in _emamiContext.SpecalityFatDiscountUsers on sf.SpecialtyFatQuantityLimitId equals sfuqu.Id
                                                    //join sfuqusub in _emamiContext.SpecalityFatDiscountUsers on sfuqu.ParentQuantityId equals sfuqusub.Id
                                                    orderby sf.Id
                                                    select new SpecialtyFatQuantityRequestDto
                                                    {
                                                        Id = sf.SpecialtyFatQuantityLimitId,
                                                        UserId = sfu.UserId,
                                                        UserName = sfu.User.Name,
                                                        SkuId = sf.SkuId,
                                                        SkuName = sf.Sku.SkuName,
                                                        SkuCode = sf.Sku.SkuCode,
                                                        Quantity = sf.Quantity,
                                                        Status = sf.Status != null ? sf.Status.Name : string.Empty,
                                                        StatusId = sf.StatusId,
                                                        OiltypeId = sf.OilTypeId,
                                                        OilTypeName = sf.OilType != null ? sf.OilType.Name + "-" + sf.OilType.SalesOrganization.Code + "/" + sf.OilType.DistributionChannel.Code + "/" + sf.OilType.Division.Code : String.Empty,
                                                        //OilTypeCode = sf.OilType.SAPCode,
                                                        CreatedBy = createus.Name,
                                                        SpecialtyFatQuantityRequestId = sfu.SpecialtyFatQuantityRequestId,
                                                        IsRequestedUser = ((sf.Id == sfu.SpecialtyFatQuantityRequestId && inputDto.LoginUserId == sfu.UserId) ? true : false),
                                                        VerticleId = sf.DivisionId
                                                        //RemainingQuantity = sfuqusub.RemainingQuantity
                                                    }).ToList();
                specialtyFatQuantityRequestsList = specialtyFatQuantityRequestsList.GroupBy(s => new { s.Id, s.UserId, s.OiltypeId, s.CreatedBy, s.StatusId, s.Status })
                    .Select(s => new SpecialtyFatQuantityRequestDto()
                    {
                        Id = s.Key.Id,
                        OiltypeId = s.Key.OiltypeId,
                        UserId = s.FirstOrDefault().UserId,
                        UserName = s.FirstOrDefault().UserName,
                        Quantity = s.FirstOrDefault().Quantity,
                        Status = s.FirstOrDefault().Status,
                        StatusId = s.FirstOrDefault().StatusId,
                        OilTypeName = s.FirstOrDefault().OilTypeName,
                        CreatedBy = s.FirstOrDefault().CreatedBy,
                        VerticleId = s.FirstOrDefault().VerticleId
                    }).ToList();
                if (specialtyFatQuantityRequestsList != null && specialtyFatQuantityRequestsList.Any())
                {
                    specialtyFatQuantityRequestsList.ForEach(f =>
                    {
                        f.IsRequestedUser = _emamiContext.SpecialtyFatQuantityRequestUserDetails.Any(w => w.SpecialtyFatQuantityRequestId == f.Id && w.UserId == inputDto.LoginUserId);
                    });
                }

                if (inputDto.StatusId != 0)
                {
                    specialtyFatQuantityRequestsList = specialtyFatQuantityRequestsList.Where(sp => sp.StatusId == inputDto.StatusId).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = specialtyFatQuantityRequestsList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }
        public ResultDto GetSpecialtyFatQuantityRequestsListForMobile(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListForMobile";
            var resultDto = new ResultDto();
            var specialtyFatQuantityRequestsList = new List<SpecialtyFatQuantityRequestDto>();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var userList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(w => w.ReportingToUserId == inputDto.LoginUserId).Select(s => s.UserId).ToList();
                userList.Add(inputDto.LoginUserId);

                specialtyFatQuantityRequestsList = (from sfu in _emamiContext.SpecialtyFatQuantityRequestUserDetails
                                                    join sf in _emamiContext.SpecialtyFatQuantityRequests on sfu.SpecialtyFatQuantityRequestId equals sf.SpecialtyFatQuantityLimitId
                                                    join createus in _emamiContext.Users on sf.CreatedBy equals createus.Id
                                                    where (userList.Contains(sfu.UserId))
                                                    orderby sf.Id
                                                    select new SpecialtyFatQuantityRequestDto
                                                    {
                                                        Id = sf.SpecialtyFatQuantityLimitId,
                                                        UserId = sfu.UserId,
                                                        UserName = sfu.User.Name,
                                                        SkuId = sf.SkuId,
                                                        SkuName = sf.Sku.SkuName,
                                                        SkuCode = sf.Sku.SkuCode,
                                                        Quantity = sf.Quantity,
                                                        Status = sf.Status != null ? sf.Status.Name : string.Empty,
                                                        StatusId = sf.StatusId,
                                                        OiltypeId = sf.OilTypeId,
                                                        OilTypeName = sf.OilType != null ? sf.OilType.Name + "-" + sf.OilType.SalesOrganization.Code + "/" + sf.OilType.DistributionChannel.Code + "/" + sf.OilType.Division.Code : String.Empty,
                                                        CreatedBy = createus.Name,
                                                        SpecialtyFatQuantityRequestId = sfu.SpecialtyFatQuantityRequestId,
                                                        IsRequestedUser = ((sf.SpecialtyFatQuantityLimitId == sfu.SpecialtyFatQuantityRequestId && inputDto.LoginUserId == sfu.UserId) ? true : false),
                                                        VerticleId = sf.DivisionId
                                                    }).ToList();
                specialtyFatQuantityRequestsList = specialtyFatQuantityRequestsList.GroupBy(s => new { s.Id, s.Quantity, s.OiltypeId, s.CreatedBy, s.StatusId, s.Status })
                    .Select(s => new SpecialtyFatQuantityRequestDto()
                    {
                        Id = s.Key.Id,
                        OiltypeId = s.Key.OiltypeId,
                        UserId = s.FirstOrDefault().UserId,
                        UserName = s.FirstOrDefault().UserName,
                        Quantity = s.FirstOrDefault().Quantity,
                        Status = s.FirstOrDefault().Status,
                        StatusId = s.FirstOrDefault().StatusId,
                        OilTypeName = s.FirstOrDefault().OilTypeName,
                        CreatedBy = s.FirstOrDefault().CreatedBy,
                        VerticleId = s.FirstOrDefault().VerticleId,
                        IsRequestedUser = s.FirstOrDefault().IsRequestedUser
                    }).OrderByDescending(o => o.Id).ToList();
                //if (specialtyFatQuantityRequestsList != null && specialtyFatQuantityRequestsList.Any())
                //{
                //    specialtyFatQuantityRequestsList.ForEach(f =>
                //    {
                //        f.IsRequestedUser = _emamiContext.SpecialtyFatQuantityRequestUserDetails.Any(w => w.SpecialtyFatQuantityRequestId == f.Id && w.UserId == inputDto.LoginUserId);
                //    });
                //}

                //specialtyFatQuantityRequestsList = specialtyFatQuantityRequestsList.Where(sp => sp.StatusId == inputDto.StatusId).ToList();


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = specialtyFatQuantityRequestsList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetSpecialtyFatQuantityRequestsList(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsList";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var specialtyFatQuantityRequestsList = (from sfu in _emamiContext.SpecialtyFatQuantityRequestUserDetails
                                                        join sf in _emamiContext.SpecialtyFatQuantityRequests on sfu.SpecialtyFatQuantityRequestId equals sf.SpecialtyFatQuantityLimitId
                                                        join us in _emamiContext.Users on sf.CreatedBy equals us.Id
                                                        where sfu.UserId == inputDto.LoginUserId
                                                        orderby sf.Id
                                                        select new SpecialtyFatQuantityRequestDto
                                                        {
                                                            Id = sf.SpecialtyFatQuantityLimitId,
                                                            UserId = sfu.UserId,
                                                            UserName = sfu.User.Name,
                                                            SkuId = sf.SkuId,
                                                            SkuName = sf.Sku.SkuName,
                                                            SkuCode = sf.Sku.SkuCode,
                                                            Quantity = sf.Quantity,
                                                            Status = sf.Status != null ? sf.Status.Name : string.Empty,
                                                            StatusId = sf.StatusId,
                                                            OiltypeId = sf.OilTypeId,
                                                            OilTypeName = sf.OilType != null ? sf.OilType.Name + "-" + sf.OilType.SalesOrganization.Code + "/" + sf.OilType.DistributionChannel.Code + "/" + sf.OilType.Division.Code : String.Empty,
                                                            //OilTypeCode = sf.OilType.SAPCode,
                                                            CreatedBy = us.Name,
                                                            VerticleId = sf.DivisionId,

                                                        }).ToList();
                if (specialtyFatQuantityRequestsList.Any() && specialtyFatQuantityRequestsList.Count > 0)
                {
                    specialtyFatQuantityRequestsList = specialtyFatQuantityRequestsList.GroupBy(s => new { s.Id, s.OiltypeId, s.CreatedBy, s.StatusId, s.Status })
                        .Select(s => new SpecialtyFatQuantityRequestDto()
                        {
                            Id = s.Key.Id,
                            OiltypeId = s.Key.OiltypeId,
                            UserId = s.FirstOrDefault().UserId,
                            UserName = s.FirstOrDefault().UserName,
                            Quantity = s.FirstOrDefault().Quantity,
                            Status = s.FirstOrDefault().Status,
                            StatusId = s.FirstOrDefault().StatusId,
                            OilTypeName = s.FirstOrDefault().OilTypeName,
                            CreatedBy = s.FirstOrDefault().CreatedBy,
                            VerticleId = s.FirstOrDefault().VerticleId
                        }).ToList();
                }
                //specialtyFatQuantityRequestsList = specialtyFatQuantityRequestsList.Where(sp => sp.StatusId == inputDto.StatusId).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = specialtyFatQuantityRequestsList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }


        public ResultDto UpdateSpecialtyFatQuantityLimit(SpecialtyFatQuantityRequestDto inputDto)
        {
            _methodName = "UpdateSpecialtyFatQuantityLimit";
            var resultDto = new ResultDto();
            var errorMessage = new StringBuilder();
            decimal remainingQuantity = 0;
            bool isValid = false;

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (inputDto.updateQuantity <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidupdateQuantityRequest;
                    return resultDto;
                }
                var specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault();
                var userRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (userRole.RoleId != (int)DTO.Enums.Role.NationalTrader)
                {
                    if (inputDto.StatusId != (int)DTO.Enums.Status.RequestForApproval && (specalityFatRemainingQty.RemainingQuantity < inputDto.updateQuantity || specalityFatRemainingQty.ActualDiscount < inputDto.updateQuantity))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.QuantityLimitRequest;
                        return resultDto;
                    } 
                }

                #region Claims Check
                UserIdDto userIdDto = new UserIdDto() { UserId = inputDto.LoginUserId };
                var userClaims = GetUserRoleClaims(userIdDto);

                if (userClaims != null)
                {
                    if (userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(Claims.ManageOrganization) && _.IsApplied))
                        isValid = true;

                    isValid = userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(Claims.SpecialtyFatQtyRequestForApprove) && _.IsApplied);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                #endregion

                if (inputDto != null && inputDto.QuantityRequestIds != null && inputDto.QuantityRequestIds.Any())
                {
                    List<long> quantityIds = inputDto.QuantityRequestIds.ToList();
                    foreach (var item in inputDto.QuantityRequestIds)
                    {
                        var specialtyFatQuantityRequests = _emamiContext.SpecialtyFatQuantityRequests.OrderByDescending(w => w.Id).FirstOrDefault(w => w.SpecialtyFatQuantityLimitId == item && w.StatusId != (int)DTO.Enums.Status.Approved);
                        if (specialtyFatQuantityRequests == null)
                        {
                            return _resultService.ErrorMessage(Constants.SpecialtyFatQuantityRequestsNotFound);
                        }
                        if (specialtyFatQuantityRequests.StatusId == (int)DTO.Enums.Status.Pending || specialtyFatQuantityRequests.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                        {
                            var specalityFatDiscountUsers = _emamiContext.SpecalityFatDiscountUsers
                                .FirstOrDefault(w => w.Id == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId
                                //|| w.ParentId== specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId
                                );
                            if (specalityFatDiscountUsers != null)
                            {

                                if (isValid && inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    if (inputDto.RoleId == (int)(DTO.Enums.Role.NationalTrader) /*|| inputDto.RoleId == (int)(DTO.Enums.Role.ZonalTrader)*/)
                                    {
                                        //if (specalityFatDiscountUsers.ParentQuantityId > 0)
                                        //{
                                        //    var parentqtylist = _emamiContext.SpecalityFatDiscountUsers
                                        //    .Where(w => w.Id == specalityFatDiscountUsers.ParentQuantityId || w.ParentId == specalityFatDiscountUsers.ParentQuantityId);
                                        //    foreach (var parentqty in parentqtylist)
                                        //    {
                                        //        specialtyFatQuantityRequests.Quantity = inputDto.updateQuantity;
                                        //        parentqty.ActualDiscount += inputDto.updateQuantity;
                                        //    }
                                        //    _emamiContext.SaveChanges();
                                        //}

                                        var spqtylist = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId
                                         || w.ParentId == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId).ToList();

                                        foreach (var itemdata in spqtylist)
                                        {
                                            //itemdata.ActualDiscount = itemdata.ActualDiscount + specialtyFatQuantityRequests.Quantity;
                                            itemdata.ActualDiscount += inputDto.updateQuantity;
                                            itemdata.RemainingQuantity += inputDto.updateQuantity;
                                        }
                                        _emamiContext.SaveChanges();

                                        var requestlist = (from sfqr in _emamiContext.SpecialtyFatQuantityRequests
                                                           join sfdu in _emamiContext.SpecalityFatDiscountUsers
                                                           on sfqr.SpecialtyFatQuantityLimitId equals sfdu.Id
                                                           where sfqr.SpecialtyFatQuantityLimitId == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId
                                                                 && (sfqr.StatusId == (int)DTO.Enums.Status.Pending || sfqr.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                                                           select sfqr)
                                                   .ToList();

                                        if (requestlist.Any() && requestlist.Count() > 0)
                                        {
                                            foreach (var requestitem in requestlist)
                                            {
                                                requestitem.StatusId = inputDto.StatusId;
                                                requestitem.Quantity = inputDto.updateQuantity;
                                                requestitem.Remarks = inputDto.Remarks;
                                                requestitem.ModifiedBy = inputDto.LoginUserId;
                                                requestitem.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            }
                                            _emamiContext.SaveChanges();
                                        }

                                    }
                                    else
                                    {
                                        specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers
                                        .FirstOrDefault(w => w.Id == specalityFatDiscountUsers.ParentQuantityId && w.ParentId == specalityFatDiscountUsers.ParentId);

                                        if (specalityFatRemainingQty != null)
                                        {
                                            remainingQuantity = specalityFatRemainingQty.RemainingQuantity;

                                            if (inputDto.updateQuantity <= remainingQuantity)
                                            {

                                                //var parentqtylist = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.Id == specalityFatDiscountUsers.ParentQuantityId && _.ParentId == specalityFatDiscountUsers.ParentId).ToList();


                                                var parentqtylist = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.ParentId == specalityFatRemainingQty.Id).ToList();

                                                // Check if the specific record should be included (it has ParentId = 0)
                                                if (!parentqtylist.Any(q => q.Id == specalityFatRemainingQty.Id))
                                                {
                                                    parentqtylist.Add(specalityFatRemainingQty);
                                                }

                                                if (parentqtylist.Any() && parentqtylist.Count() > 0)
                                                {
                                                    foreach (var qty in parentqtylist)
                                                    {
                                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                                        {
                                                            qty.RemainingQuantity -= inputDto.updateQuantity;
                                                        }
                                                    }
                                                    _emamiContext.SaveChanges();
                                                }

                                                var sqqtylist = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId
                                                  || w.ParentId == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId).ToList();

                                                if (sqqtylist.Any())
                                                {
                                                    foreach (var qty in sqqtylist)
                                                    {
                                                        if (qty != null && inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                                        {
                                                            qty.ActualDiscount += inputDto.updateQuantity;
                                                            qty.RemainingQuantity += inputDto.updateQuantity;
                                                            qty.RequestedDiscount += inputDto.updateQuantity;
                                                            qty.RequestedDiscountDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                        }
                                                    }
                                                    _emamiContext.SaveChanges();
                                                }
                                                var requestlist = (from sfqr in _emamiContext.SpecialtyFatQuantityRequests
                                                                   join sfdu in _emamiContext.SpecalityFatDiscountUsers
                                                                   on sfqr.SpecialtyFatQuantityLimitId equals sfdu.Id
                                                                   where sfqr.SpecialtyFatQuantityLimitId == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId
                                                                         && sfqr.StatusId == (int)DTO.Enums.Status.Pending
                                                                   select sfqr)
                                                                   .ToList();
                                                if (requestlist.Any() && requestlist.Count() > 0)
                                                {
                                                    foreach (var requestitem in requestlist)
                                                    {
                                                        requestitem.StatusId = inputDto.StatusId;
                                                        requestitem.Quantity = inputDto.updateQuantity;
                                                        requestitem.Remarks = inputDto.Remarks;
                                                        requestitem.ModifiedBy = inputDto.LoginUserId;
                                                        requestitem.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                    }
                                                    _emamiContext.SaveChanges();
                                                }
                                                var userrequest = _emamiContext.SpecialtyFatQuantityRequestUserDetails.FirstOrDefault(w => w.SpecialtyFatQuantityRequestId == specialtyFatQuantityRequests.Id);

                                                if (userrequest != null)
                                                {
                                                    userrequest.ModifiedBy = inputDto.LoginUserId;
                                                    userrequest.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                    userrequest.StatusId = inputDto.StatusId;

                                                    _emamiContext.SaveChanges();
                                                }
                                                //_emamiContext.SaveChanges();
                                                //var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                                                //{
                                                //    UserId = inputDto.LoginUserId,
                                                //    SpecialtyFatQuantityRequestId = specialtyFatQuantityRequests.Id,
                                                //    StatusId = inputDto.StatusId,
                                                //    CreatedBy = inputDto.LoginUserId,
                                                //    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                                //};
                                                //_emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
                                                //_emamiContext.SaveChanges();
                                            }
                                            else
                                            {
                                                var userName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == specialtyFatQuantityRequests.CreatedBy).Name;
                                                errorMessage.Append("USER : " + userName + " | OilType : " + specialtyFatQuantityRequests.OilType.Name + "");
                                            }
                                        }
                                        else
                                        {
                                            resultDto = _resultService.ErrorMessage(Constants.RecordNotFound);
                                        }
                                    }
                                }
                                else
                                {

                                    var quantityRequest = _emamiContext.SpecialtyFatQuantityRequests.Where(w => w.SpecialtyFatQuantityLimitId == item && w.StatusId != (int)DTO.Enums.Status.Approved);

                                    foreach (var request in quantityRequest)
                                    {
                                        request.StatusId = inputDto.StatusId;
                                        request.Remarks = inputDto.Remarks;
                                        request.ModifiedBy = inputDto.LoginUserId;
                                        request.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    }
                                    _emamiContext.SaveChanges();


                                    //_emamiContext.SaveChanges();
                                    var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                                    {
                                        UserId = inputDto.LoginUserId,
                                        SpecialtyFatQuantityRequestId = specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId,
                                        StatusId = inputDto.StatusId,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                    };
                                    _emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
                                    _emamiContext.SaveChanges();
                                }
                            }
                            else
                            {
                                resultDto = _resultService.ErrorMessage(Constants.RecordNotFound);
                            }
                            //resultDto = _resultService.SuccessMessage(Constants.SpecialtyFatQuantityRequestsSuccessUpdated);
                        }
                        else
                        {
                            quantityIds.Remove(item);
                        }
                    }

                    if (!string.IsNullOrEmpty(errorMessage.ToString()))
                    {
                        errorMessage.Append("Above users not approved. Your remaining quantity is " + remainingQuantity + ".");
                        errorMessage.Append("User requested quantity is greater then for your remaining quantity. so can't approve. Please raise the request");
                        resultDto = _resultService.ErrorMessage(errorMessage.ToString());
                    }
                    else
                    {
                        resultDto = _resultService.SuccessMessage(Constants.SpecialtyFatQuantityRequestsSuccessUpdated);
                    }

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        try
                        {
                            foreach (var item in quantityIds)
                            {
                                var requestedLimitContext = _emamiContext.SpecialtyFatQuantityRequests.AsNoTracking().FirstOrDefault(w => w.Id == item);
                                var allocatedLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(w => w.Id == requestedLimitContext.SpecialtyFatQuantityLimitId);
                                var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == allocatedLimitContext.SkuId)?.SkuName;
                                if (requestedLimitContext != null && allocatedLimitContext != null && skuName != null)
                                {
                                    decimal limit = 0;
                                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                    {
                                        limit = requestedLimitContext.Quantity;
                                    }
                                    else
                                    {
                                        limit = allocatedLimitContext.ActualDiscount;
                                    }

                                    var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == allocatedLimitContext.UserId);
                                    if (userContext != null)
                                    {
                                        List<string> toUsers = new List<string>();
                                        toUsers.Add(userContext.Email);
                                        string fromDate = allocatedLimitContext.ValidFrom.ToString("MMM dd,yyyy");
                                        string toDate = allocatedLimitContext.ValidTo.ToString("MMM dd,yyyy");

                                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                        if (_resultService.IsEmail())
                                        {
                                            var fromEmail = Constants.FromEmail;
                                            EmailTemplate emailTemplate = new EmailTemplate();
                                            var plainText = string.Empty;
                                            var emailSubject = string.Empty;
                                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                            {
                                                emailSubject = Constants.SpecialityFatLimitApprovalSubject;
                                                emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountAcceptEmail);
                                            }
                                            else
                                            {
                                                emailSubject = Constants.SpecialityFatLimitRejectSubject;
                                                emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountRejectEmail);
                                            }
                                            if (emailTemplate != null)
                                            {
                                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                                    .Replace(Constants.Quantity, Math.Round(limit, 0).ToString());
                                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                            }
                                        }
                                        var smsPlainTemplate = string.Empty;
                                        if (_resultService.IsSMS())
                                        {
                                            var smsMessage = string.Empty;
                                            EmailTemplate smsTemplate = new EmailTemplate();
                                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                            {
                                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountAcceptSMS);
                                            }
                                            else
                                            {
                                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountRejectSMS);
                                            }
                                            if (smsTemplate != null)
                                            {
                                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                                    .Replace(Constants.Quantity, Math.Round(limit, 0).ToString());
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                                try
                                                {
                                                    amazonNotificationService.SendMessage(smsMessage, userContext.MobileNumber);
                                                }
                                                catch (Exception ex)
                                                {

                                                }
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
        public List<UserClaimsDto> GetUserRoleClaims(UserIdDto userIdDto)
        {
            _methodName = "GetUserClaims";
            var resultDto = new ResultDto();
            var userClaimListDto = new List<UserClaimsDto>();
            try
            {
                var userRoleContext = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == userIdDto.UserId);
                var userClaimList = _emamiContext.RoleClaims.AsNoTracking().Where(_ => _.RoleId == userRoleContext.RoleId).OrderBy(_ => _.ClaimId).ToList();
                var claimListContext = _emamiContext.Claims.AsNoTracking().Where(_ => _.IsActive).OrderBy(_ => _.Id).ToList();
                if (claimListContext.Any())
                {
                    foreach (var claim in claimListContext)
                    {
                        var claimDto = new UserClaimsDto
                        {
                            ClaimId = claim.Id,
                            Name = claim.Name,
                            IsApplied = userClaimList.Any(_ => _.ClaimId == claim.Id)
                        };
                        userClaimListDto.Add(claimDto);
                    }
                }
                return userClaimListDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return userClaimListDto;
            }
        }

        #endregion

        #region SpecialityFat Geography Discounts

        public ResultDto GetCityDetailsBasedOnTerritoryAndCity(TerritoryId inputDto)
        {
            _methodName = "GetCityDetailsBasedOnTerritoryAndCity";
            var resultDto = new ResultDto();
            var cityList = new List<CityDetails>();
            try
            {

                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var districtIds = _emamiContext.District.AsNoTracking().ToList()/*.Where(w => inputDto.TerritoryIds.Any(a => a == w.TerritoryId))*/.Select(s => s.Id);

                if (districtIds != null && districtIds.Any())
                {
                    var cityDetails = _emamiContext.City.AsNoTracking().ToList().Where(w => districtIds.Any(a => a == w.DistrictId)).ToList();

                    if (cityDetails != null && cityDetails.Any())
                    {
                        foreach (var item in cityDetails)
                        {
                            CityDetails city = new CityDetails();

                            city.CityId = item.Id;
                            city.CityName = item.CityName;

                            city.DistrictId = item.DistrictId;
                            city.DistrictName = _emamiContext.District.AsNoTracking().ToList().FirstOrDefault(f => f.Id == item.DistrictId).DistrictName;

                            //city.TerritoryId = item.TerritoryId;
                            //city.TerritoryName = _emamiContext.Territory.AsNoTracking().ToList().FirstOrDefault(f => f.Id == item.TerritoryId).Name;

                            var stateDetail = _emamiContext.District.AsNoTracking().ToList().FirstOrDefault(f => f.Id == item.DistrictId);
                            city.StateId = stateDetail.StateId;
                            city.StateName = stateDetail?.State?.StateName;

                            var zoneDetail = _emamiContext.ZoneStateMappings.AsNoTracking().ToList().FirstOrDefault(f => f.StateId == stateDetail.StateId);
                            city.ZoneId = zoneDetail.ZoneId;
                            city.ZoneName = zoneDetail?.Zone?.Name;

                            //if (inputDto.CityIds.Contains(item.Id))
                            //    city.IsChecked = true;                            

                            cityList.Add(city);
                        }
                        resultDto.IsSuccess = true;
                        resultDto.SuccessDto.Response = cityList;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        public ResultDto GetSpecialityFatGeographyList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSpecialityFatGeographyList";
            var resultDto = new ResultDto();
            var outputDto = new List<SpecialityFatDiscountOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                outputDto = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking().Where(w => w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId)
                    .Select(ss => new SpecialityFatDiscountOutputDto()
                    {
                        Id = ss.Id,
                        OilTypeName = ss.OilType.Name,
                        SkuName = ss.Sku.SkuName,
                        SkuCode = ss.Sku.SkuCode,
                        ValidFrom = ss.ValidFrom,
                        ValidTo = ss.ValidTo,
                        ParentId = ss.ParentId,
                        ActualDiscount = ss.ActualDiscount,
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetSpecialityFatGeographyCityList(GeographyCityListParam inputDto)
        {
            _methodName = "GetSpecialityFatGeographyCityList";
            var resultDto = new ResultDto();
            var cityList = new List<CityDetails>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var cityListDetail = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking().Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id)
                    .Select(s => s).ToList();

                if (cityListDetail != null && cityListDetail.Any())
                {
                    foreach (var item in cityListDetail)
                    {
                        var stateDetail = _emamiContext.Territory.AsNoTracking().ToList().FirstOrDefault(f => f.Id == item.TerritoryId);
                        var zoneDetail = _emamiContext.ZoneStateMappings.AsNoTracking().ToList().FirstOrDefault(f => f.StateId == stateDetail.StateId);
                        CityDetails city = new CityDetails()
                        {
                            CityId = item.Id,
                            CityName = _emamiContext.City.AsNoTracking().ToList().FirstOrDefault(f => f.Id == item.CityId).CityName,
                            DistrictId = item.DistrictId,
                            DistrictName = _emamiContext.District.AsNoTracking().ToList().FirstOrDefault(f => f.Id == item.DistrictId).DistrictName,
                            TerritoryId = item.TerritoryId,
                            TerritoryName = _emamiContext.Territory.AsNoTracking().ToList().FirstOrDefault(f => f.Id == item.TerritoryId).Name,
                            StateId = stateDetail.StateId,
                            StateName = stateDetail?.State?.StateName,
                            ZoneId = zoneDetail.ZoneId,
                            ZoneName = zoneDetail?.Zone?.Name
                        };
                        cityList.Add(city);
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(exception.Message);
            }
            return resultDto;
        }

        public ResultDto GetSpecialityFatGeographyDetailsById(long geographyId)
        {
            _methodName = "GetSpecialityFatGeographyDetailsById";
            var resultDto = new ResultDto();
            var result = new SpecialityFatDiscountInputDto();
            try
            {

                var cityListDetail = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking().Where(w => w.Id == geographyId || w.ParentId == geographyId)
                    .Select(s => s).ToList();

                if (cityListDetail != null && cityListDetail.Any())
                {
                    var data = cityListDetail.FirstOrDefault();
                    result.Id = data.Id;
                    result.VerticleId = data.OilType.DivisionId;
                    result.OilTypeId = data.OilTypeId;
                    result.SkuId = data.SkuId;
                    result.ActualDiscount = data.ActualDiscount;
                    result.ValidFrom = data.ValidFrom;
                    result.ValidTo = data.ValidTo;

                    result.ZoneId = cityListDetail.Select(s => s.ZoneId).Distinct().ToList();
                    result.StateId = cityListDetail.Select(s => s.StateId).Distinct().ToList();
                    result.TerritoryId = cityListDetail.Select(s => s.TerritoryId).Distinct().ToList();
                    result.DistrictId = cityListDetail.Select(s => s.DistrictId).Distinct().ToList();
                    result.CityId = cityListDetail.Select(s => s.CityId).Distinct().ToList();

                    cityListDetail.ForEach(f =>
                    {
                        DiscountSkuCityMappingDto model = new DiscountSkuCityMappingDto()
                        {
                            ZoneId = f.ZoneId,
                            StateId = f.StateId,
                            TerritoryId = f.TerritoryId,
                            DistrictId = f.DistrictId,
                            CityId = f.CityId
                        };
                        result.Cities.Add(model);
                    });
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Response = Constants.RecordNotFound;
                }

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

        public ResultDto AddSpecialityFatDiscountGeography(SpecialityFatDiscountInputDto inputDto)
        {
            _methodName = "AddSpecialityFatDiscountGeography";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.Cities == null || inputDto.Cities.Count == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.CityListEmpty;
                    return resultDto;
                }

                if (inputDto != null && inputDto.Cities != null && inputDto.Cities.Any())
                {

                    #region Validation
                    var cityIds = inputDto.Cities.Select(s => s.CityId).ToList();
                    var geographyDiscountCount = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking()
                       .Where(w => w.OilTypeId == inputDto.OilTypeId && w.SkuId == inputDto.SkuId
                       && cityIds.Contains(w.CityId)
                       && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                        && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                        || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                        && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).Select(s => s.CityId).ToList();

                    if (geographyDiscountCount != null && geographyDiscountCount.Any())
                    {
                        var cityName = _emamiContext.City.AsNoTracking().Where(w => geographyDiscountCount.Any(a => a == w.Id)).Select(s => s.CityName).ToList();
                        return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisCity + string.Join(",", cityName.Select(s => s)));
                    }
                    #endregion

                    foreach (var item in inputDto.Cities)
                    {
                        var entity = new SpecalityFatDiscountGeography()
                        {
                            OilTypeId = inputDto.OilTypeId,
                            SkuId = inputDto.SkuId,
                            ZoneId = item.ZoneId,
                            StateId = item.StateId,
                            TerritoryId = item.TerritoryId,
                            DistrictId = item.DistrictId,
                            CityId = item.CityId,
                            SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                            ActualDiscount = inputDto.ActualDiscount,
                            ValidFrom = inputDto.ValidFrom,
                            ValidTo = inputDto.ValidTo,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            Status = true,
                            ParentId = parentId
                        };
                        _emamiContext.SpecalityFatDiscountGeographys.Add(entity);
                        if (!isFirstRecord)
                        {
                            _emamiContext.SaveChanges();
                            isFirstRecord = true;
                            parentId = entity.Id;
                        }
                    }
                    _emamiContext.SaveChanges();
                    resultDto.IsSuccess = true;
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

        public ResultDto UpdateSpecialityFatDiscountGeography(SpecialityFatDiscountInputDto inputDto)
        {
            _methodName = "UpdateSpecialityFatDiscountGeography";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            bool isFirstRecord = false;
            long parentId = 0;
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (inputDto.Cities == null || inputDto.Cities.Count == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.CityListEmpty;
                    return resultDto;
                }

                if (inputDto != null && inputDto.Cities != null && inputDto.Cities.Any())
                {
                    #region Validation
                    var cityIds = inputDto.Cities.Select(s => s.CityId).ToList();
                    var geographyDiscountCount = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking()
                       .Where(w => w.OilTypeId == inputDto.OilTypeId && w.SkuId == inputDto.SkuId
                       && cityIds.Contains(w.CityId)
                       && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                        && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                        || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                        && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).ToList();

                    var notWithInCity = geographyDiscountCount.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.CityId).ToList();
                    if (notWithInCity != null && notWithInCity.Any())
                    {
                        var cityName = _emamiContext.City.AsNoTracking().Where(w => notWithInCity.Contains(w.Id)).Select(s => s.CityName).ToList();
                        return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisCity + string.Join(",", cityName.Select(s => s)));
                    }
                    #endregion


                    var discountGeography = _emamiContext.SpecalityFatDiscountGeographys.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();

                    if (discountGeography != null && discountGeography.Any())
                    {
                        var dbCityIds = discountGeography.Select(s => s.CityId).ToList();
                        var selectedCityIds = inputDto.Cities.Select(s => s.CityId).ToList();

                        var isEqualAll = EqualsTwoList(dbCityIds, selectedCityIds);

                        if (isEqualAll)
                        {
                            //Update existing data
                            foreach (var discount in discountGeography)
                            {
                                discount.OilTypeId = inputDto.OilTypeId;
                                discount.SkuId = inputDto.SkuId;
                                discount.ActualDiscount = inputDto.ActualDiscount;
                                discount.ValidFrom = inputDto.ValidFrom;
                                discount.ValidTo = inputDto.ValidTo;
                                discount.ModifiedBy = inputDto.LoginUserId;
                                discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SaveChanges();
                            }
                        }
                        else
                        {
                            //Remove existing data
                            foreach (var geography in discountGeography)
                            {
                                _emamiContext.SpecalityFatDiscountGeographys.Remove(geography);
                                _emamiContext.SaveChanges();
                            }

                            //Insert new data
                            foreach (var item in inputDto.Cities)
                            {
                                var entity = new SpecalityFatDiscountGeography()
                                {
                                    OilTypeId = inputDto.OilTypeId,
                                    SkuId = inputDto.SkuId,
                                    ZoneId = item.ZoneId,
                                    StateId = item.StateId,
                                    TerritoryId = item.TerritoryId,
                                    DistrictId = item.DistrictId,
                                    CityId = item.CityId,
                                    SaudaBookingTypeId = (int)SaudaBookingTypes.TraditionalProcess,
                                    ActualDiscount = inputDto.ActualDiscount,
                                    ValidFrom = inputDto.ValidFrom,
                                    ValidTo = inputDto.ValidTo,
                                    Status = true,
                                    ParentId = parentId,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    ModifiedBy = inputDto.LoginUserId,
                                    ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                };
                                _emamiContext.SpecalityFatDiscountGeographys.Add(entity);
                                if (!isFirstRecord)
                                {
                                    _emamiContext.SaveChanges();
                                    isFirstRecord = true;
                                    parentId = entity.Id;
                                }
                            }
                            _emamiContext.SaveChanges();
                        }
                        resultDto.IsSuccess = true;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    }
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

        #endregion

        #region SpecialityFat Discount Users

        public ResultDto AddSpecialityFatDiscountUsers(SpecialityFatDiscountUserDto inputDto)
        {
            _methodName = "AddSpecialityFatDiscountUsers";
            var resultDto = new ResultDto();
            var PackingCostDto = new PackingCostDto();
            bool isFirstRecord = false;
            long parentId = 0;
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                //if (inputDto.SkuIds == null || !inputDto.SkuIds.Any())
                //{
                //    return _resultService.ErrorMessage(Constants.SkuEmpty);
                //}

                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeEmpty);
                }

                if (inputDto.QuantityLimit <= 0)
                {
                    return _resultService.ErrorMessage(Constants.QuantityLimitEmpty);
                }

                if (inputDto.OilTypeId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.OiltypeIsEmpty);
                }

                //if (inputDto.VerticleId == 0 || inputDto.VerticleId == null)
                //{
                //    var loginUserVerticalId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId)?.DivisionId;
                //    inputDto.VerticleId = loginUserVerticalId.Value;
                //}

                #region From Date and To Date validation                              

                var userRoleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(f => f.UserId == inputDto.LoginUserId).RoleId;
                if (userRoleId == (int)Adani.Solution.DTO.Enums.Role.ZonalTrader)
                {
                    var discountPercentage = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .FirstOrDefault(w => w.UserId == inputDto.LoginUserId
                     && w.OilTypeId == inputDto.OilTypeId
                     //&& inputDto.SkuIds.Contains(w.SkuId)
                     && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                     && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                     || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                     && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo))));

                    if (discountPercentage != null && !(inputDto.QuantityLimit <= discountPercentage.ActualDiscount))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.QtyLimitValidation + discountPercentage.ActualDiscount;
                        return resultDto;
                    }
                }

                #endregion

                #region Employee Already exists validation

                var userIds = inputDto.CustomerId;
                var geographyDiscountCount = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                  .Where(w => w.OilTypeId == inputDto.OilTypeId
                  //&& inputDto.SkuIds.Contains(w.SkuId)
                  && userIds.Contains(w.UserId)
                  && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                  && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                  || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                  && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo)))).Select(s => s.UserId).ToList();

                if (geographyDiscountCount != null && geographyDiscountCount.Any())
                {
                    var userName = _emamiContext.Users.AsNoTracking().Where(w => userIds.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                    return _resultService.ErrorMessage(Constants.QtyLimitAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)));
                }
                inputDto.SkuIds = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId
                  && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                  && _.DivisionId == inputDto.VerticleId).Select(s => s.Id).ToList();
                #endregion

                foreach (var empId in inputDto.CustomerId)
                {
                    isFirstRecord = false;
                    parentId = 0;
                    foreach (var skuId in inputDto.SkuIds)
                    {
                        var entity = new SpecalityFatDiscountUser()
                        {
                            SkuId = skuId,
                            UserId = empId,
                            ActualDiscount = inputDto.QuantityLimit,
                            ValidFrom = inputDto.ValidFrom,
                            ValidTo = inputDto.ValidTo,
                            OilTypeId = inputDto.OilTypeId,
                            ParentId = parentId,
                            RemainingQuantity = inputDto.QuantityLimit,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            DivisionId = inputDto.VerticleId,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId
                        };
                        _emamiContext.SpecalityFatDiscountUsers.Add(entity);

                        if (!isFirstRecord)
                        {
                            _emamiContext.SaveChanges();
                            isFirstRecord = true;
                            parentId = entity.Id;
                        }
                    }
                }
                _emamiContext.SaveChanges();

                try
                { SpecialityFatLimitNotification(inputDto); }
                catch (Exception) { }

                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto UpdateSpecialityFatDiscountUsers(SpecialityFatDiscountUserDto inputDto)
        {
            _methodName = "UpdateSpecialityFatDiscountUsers";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                //if (inputDto.SkuIds == null || !inputDto.SkuIds.Any())
                //{
                //    return _resultService.ErrorMessage(Constants.SkuEmpty);
                //}

                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeEmpty);
                }

                if (inputDto.QuantityLimit < 0)
                {
                    return _resultService.ErrorMessage(Constants.QuantityLimitEmpty);
                }

                if (inputDto.OilTypeId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.OiltypeIsEmpty);
                }

                #region Claims Check
                UserIdDto userIdDto = new UserIdDto() { UserId = inputDto.LoginUserId };
                var userClaims = GetUserRoleClaims(userIdDto);
                bool isValid = false;
                if (userClaims != null)
                {
                    isValid = userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(Claims.SpecialtyFatQuantityCreate) && _.IsApplied);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                #endregion

                //Functionality changed
                if (false)
                {
                    var specalityFatData = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();

                    if (specalityFatData != null && specalityFatData.Any())
                    {
                        //User
                        var employeeIds = inputDto.CustomerId;
                        var userIds = specalityFatData.Select(s => s.UserId).Distinct().ToList();
                        //SKU
                        var selectedSkuIds = inputDto.SkuIds;
                        var existSkuIds = specalityFatData.Select(s => s.SkuId).Distinct().ToList();
                        long secondParentId = 0;

                        var removedUserIds = userIds.Where(a => !employeeIds.Contains(a)).ToList();
                        var newUserIds = employeeIds.Where(a => !userIds.Contains(a)).ToList();
                        var removedSkuIds = existSkuIds.Where(a => !selectedSkuIds.Contains(a)).ToList();
                        var newSkuIds = selectedSkuIds.Where(a => !existSkuIds.Contains(a)).ToList();

                        #region Validation

                        var SpecalityFatDetails = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                        .Where(w => w.OilTypeId == inputDto.OilTypeId && inputDto.SkuIds.Contains(w.SkuId) // && (w.Id == inputDto.Id && w.ParentId == inputDto.Id)
                        && employeeIds.Contains(w.UserId)
                        && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                        && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                        || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.ValidFrom)
                        && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.ValidTo))));

                        var notWithinCurrentQuantity = SpecalityFatDetails.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.UserId).ToList();
                        if (notWithinCurrentQuantity != null && notWithinCurrentQuantity.Any() && notWithinCurrentQuantity.Count > 0)
                        {
                            var userName = _emamiContext.Users.AsNoTracking().Where(w => notWithinCurrentQuantity.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                            return _resultService.ErrorMessage(Constants.QtyLimitAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)));
                        }

                        #endregion

                        if (isValid)
                        {
                            #region Remove SKU

                            //var removedSkuIds = existSkuIds.Where(a => !selectedSkuIds.Contains(a)).ToList();
                            //var newSkuIds = selectedSkuIds.Where(a => !existSkuIds.Contains(a)).ToList();

                            if (removedSkuIds != null && removedSkuIds.Any())
                            {
                                var isParentData = specalityFatData.Any(w => w.ParentId == 0 && removedSkuIds.Contains(w.SkuId));
                                if (isParentData)
                                {
                                    secondParentId = specalityFatData.Where(w => w.ParentId != 0 && !removedSkuIds.Contains(w.SkuId)).OrderBy(o => o.Id)
                                       .Select(s => s.Id).FirstOrDefault();

                                    var removedSkus = specalityFatData.Where(w => removedSkuIds.Any(a => a == w.SkuId)).ToList();
                                    if (removedSkus != null && removedSkus.Any())
                                    {
                                        var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                        if (isMappedSku)
                                        {
                                            return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                        }
                                        //Remove existing data
                                        foreach (var sku in removedSkus)
                                        {
                                            _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                            _emamiContext.SaveChanges();
                                        }
                                    }

                                    //Update Parent Id
                                    var specalityFatData1 = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                    if (specalityFatData1 != null && specalityFatData1.Any())
                                    {
                                        bool isFirst = false;
                                        foreach (var data in specalityFatData1)
                                        {
                                            data.ParentId = isFirst ? secondParentId : 0;
                                            data.ActualDiscount = inputDto.QuantityLimit;
                                            data.ModifiedBy = inputDto.LoginUserId;
                                            data.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            _emamiContext.SaveChanges();
                                            isFirst = true;
                                        }
                                    }
                                    inputDto.Id = secondParentId;
                                }
                                else
                                {
                                    var removedSkus = specalityFatData.Where(w => removedSkuIds.Any(a => a == w.SkuId)).ToList();
                                    if (removedSkus != null && removedSkus.Any())
                                    {
                                        var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                        if (isMappedSku)
                                        {
                                            return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                        }
                                        //Remove existing data
                                        foreach (var sku in removedSkus)
                                        {
                                            _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                            _emamiContext.SaveChanges();
                                        }
                                    }

                                    var updateSpecalityFatData = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                    if (updateSpecalityFatData != null && updateSpecalityFatData.Any())
                                    {
                                        foreach (var specialtyData in updateSpecalityFatData)
                                        {
                                            specialtyData.ActualDiscount = inputDto.QuantityLimit;
                                            specialtyData.ModifiedBy = inputDto.LoginUserId;
                                            specialtyData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            _emamiContext.SaveChanges();
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Remove Users

                            //var removedUserIds = userIds.Where(a => !employeeIds.Contains(a)).ToList();
                            //var newUserIds = employeeIds.Where(a => !userIds.Contains(a)).ToList();

                            if (removedUserIds != null && removedUserIds.Any())
                            {
                                var specalityFatUserData = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                if (specalityFatUserData != null && specalityFatUserData.Any())
                                {
                                    var isParentData1 = specalityFatUserData.Any(w => w.ParentId == 0 && removedUserIds.Contains(w.UserId));
                                    if (isParentData1)
                                    {
                                        secondParentId = specalityFatUserData.Where(w => w.ParentId != 0 && !removedUserIds.Contains(w.UserId)).OrderBy(o => o.Id)
                                       .Select(s => s.Id).FirstOrDefault();

                                        //Remove existing data
                                        var removedUsers1 = specalityFatUserData.Where(w => removedUserIds.Any(a => a == w.UserId)).ToList();
                                        if (removedUsers1 != null && removedUsers1.Any())
                                        {

                                            var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                            if (isMappedSku)
                                            {
                                                return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                            }
                                            foreach (var sku in removedUsers1)
                                            {
                                                var isExistData = _emamiContext.SpecalityFatDiscountUsers.Any(f => f.Id == sku.Id);
                                                if (isExistData)
                                                {
                                                    _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                                    _emamiContext.SaveChanges();
                                                }
                                            }
                                        }

                                        //Update Parent Id
                                        var specalityFatUserData1 = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                        if (specalityFatUserData1 != null && specalityFatUserData1.Any())
                                        {
                                            bool isFirst = false;
                                            foreach (var data in specalityFatUserData1)
                                            {
                                                data.ParentId = isFirst ? secondParentId : 0;
                                                data.ActualDiscount = inputDto.QuantityLimit;
                                                data.ModifiedBy = inputDto.LoginUserId;
                                                data.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                _emamiContext.SaveChanges();
                                                isFirst = true;
                                            }
                                        }
                                        inputDto.Id = secondParentId;
                                    }
                                    else
                                    {
                                        //Remove existing data
                                        //var removedUsers1 = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                        var removedUsers1 = specalityFatData.Where(w => removedUserIds.Any(a => a == w.UserId)).ToList();
                                        if (removedUsers1 != null && removedUsers1.Any())
                                        {
                                            var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                            if (isMappedSku)
                                            {
                                                return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                            }
                                            foreach (var sku in removedUsers1)
                                            {
                                                _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                                _emamiContext.SaveChanges();
                                            }
                                        }
                                        var updateSpecalityFatData = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                        if (updateSpecalityFatData != null && updateSpecalityFatData.Any())
                                        {
                                            foreach (var specialtyData in specalityFatData)
                                            {
                                                specialtyData.ActualDiscount = inputDto.QuantityLimit;
                                                specialtyData.ModifiedBy = inputDto.LoginUserId;
                                                specialtyData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                _emamiContext.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                            if ((removedSkuIds == null || !removedSkuIds.Any()) && (removedUserIds == null || !removedUserIds.Any()))
                            {
                                foreach (var specialtyData in specalityFatData)
                                {
                                    specialtyData.ActualDiscount = inputDto.QuantityLimit;
                                    specialtyData.RemainingQuantity = inputDto.RemainingQuantity;
                                    specialtyData.ModifiedBy = inputDto.LoginUserId;
                                    specialtyData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    _emamiContext.SaveChanges();
                                }
                            }

                            #endregion

                            #region New SKU and User create
                            if (newSkuIds != null && newSkuIds.Any())
                            {
                                foreach (var skuId in newSkuIds)
                                {
                                    foreach (var empId in inputDto.CustomerId)
                                    {
                                        var entity = new SpecalityFatDiscountUser()
                                        {
                                            SkuId = skuId,
                                            UserId = empId,
                                            ActualDiscount = inputDto.QuantityLimit,
                                            ValidFrom = inputDto.ValidFrom,
                                            ValidTo = inputDto.ValidTo,
                                            OilTypeId = inputDto.OilTypeId,
                                            ParentId = (secondParentId == 0 ? inputDto.Id : secondParentId),
                                            ParentQuantityId = inputDto.ParentQuantityId,
                                            RemainingQuantity = inputDto.QuantityLimit,
                                            CreatedBy = inputDto.LoginUserId,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            ModifiedBy = inputDto.LoginUserId,
                                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                        };
                                        _emamiContext.SpecalityFatDiscountUsers.Add(entity);
                                    }
                                }
                            }

                            if (newUserIds != null && newUserIds.Any())
                            {
                                foreach (var empId in newUserIds)
                                {
                                    foreach (var skuId in inputDto.SkuIds)
                                    {
                                        var entity = new SpecalityFatDiscountUser()
                                        {
                                            SkuId = skuId,
                                            UserId = empId,
                                            ActualDiscount = inputDto.QuantityLimit,
                                            ValidFrom = inputDto.ValidFrom,
                                            ValidTo = inputDto.ValidTo,
                                            OilTypeId = inputDto.OilTypeId,
                                            ParentId = (secondParentId == 0 ? inputDto.Id : secondParentId),
                                            ParentQuantityId = inputDto.ParentQuantityId,
                                            RemainingQuantity = inputDto.QuantityLimit,
                                            CreatedBy = inputDto.LoginUserId,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            ModifiedBy = inputDto.LoginUserId,
                                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                        };
                                        _emamiContext.SpecalityFatDiscountUsers.Add(entity);
                                    }
                                }
                            }
                            _emamiContext.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            #region Remaing Quantity Validation

                            if (inputDto.ParentQuantityId > 0)
                            {
                                var parentEntity = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.ParentQuantityId);
                                if (parentEntity != null)
                                {
                                    int totalUserCount = inputDto.CustomerId.Count;
                                    decimal assignedQuantity = totalUserCount * inputDto.QuantityLimit;
                                    if (!(assignedQuantity <= parentEntity.ActualDiscount))
                                    {
                                        StringBuilder message = new StringBuilder();
                                        message.Append("Your quantity limit is " + parentEntity.ActualDiscount + ".");
                                        message.Append(" Assigned quantity is " + assignedQuantity + ". ");
                                        message.Append(Constants.QtyLimitExceeded);

                                        return _resultService.ErrorMessage(message.ToString());
                                    }
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                                }
                            }

                            #endregion

                            #region Remove SKU

                            //var removedSkuIds = existSkuIds.Where(a => !selectedSkuIds.Contains(a)).ToList();
                            //var newSkuIds = selectedSkuIds.Where(a => !existSkuIds.Contains(a)).ToList();

                            if (removedSkuIds != null && removedSkuIds.Any())
                            {
                                var isParentData = specalityFatData.Any(w => w.ParentId == 0 && removedSkuIds.Contains(w.SkuId));
                                if (isParentData)
                                {
                                    secondParentId = specalityFatData.Where(w => w.ParentId != 0 && !removedSkuIds.Contains(w.SkuId)).OrderBy(o => o.Id)
                                       .Select(s => s.Id).FirstOrDefault();

                                    var removedSkus = specalityFatData.Where(w => removedSkuIds.Any(a => a == w.SkuId)).ToList();
                                    if (removedSkus != null && removedSkus.Any())
                                    {
                                        var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                        if (isMappedSku)
                                        {
                                            return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                        }
                                        //Remove existing data
                                        foreach (var sku in removedSkus)
                                        {
                                            _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                            _emamiContext.SaveChanges();
                                        }
                                    }

                                    //Update Parent Id
                                    var specalityFatData1 = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                    if (specalityFatData1 != null && specalityFatData1.Any())
                                    {
                                        bool isFirst = false;
                                        foreach (var data in specalityFatData1)
                                        {
                                            data.ParentId = isFirst ? secondParentId : 0;
                                            data.ActualDiscount = inputDto.QuantityLimit;
                                            data.ModifiedBy = inputDto.LoginUserId;
                                            data.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            _emamiContext.SaveChanges();
                                            isFirst = true;
                                        }
                                    }
                                    inputDto.Id = secondParentId;
                                }
                                else
                                {
                                    var removedSkus = specalityFatData.Where(w => removedSkuIds.Any(a => a == w.SkuId)).ToList();
                                    if (removedSkus != null && removedSkus.Any())
                                    {
                                        var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                        if (isMappedSku)
                                        {
                                            return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                        }
                                        //Remove existing data
                                        foreach (var sku in removedSkus)
                                        {
                                            _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                            _emamiContext.SaveChanges();
                                        }
                                    }

                                    var updateSpecalityFatData = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                    if (updateSpecalityFatData != null && updateSpecalityFatData.Any())
                                    {
                                        foreach (var specialtyData in updateSpecalityFatData)
                                        {
                                            specialtyData.ActualDiscount = inputDto.QuantityLimit;
                                            specialtyData.ModifiedBy = inputDto.LoginUserId;
                                            specialtyData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            _emamiContext.SaveChanges();
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Remove Users

                            //var removedUserIds = userIds.Where(a => !employeeIds.Contains(a)).ToList();
                            //var newUserIds = employeeIds.Where(a => !userIds.Contains(a)).ToList();

                            if (removedUserIds != null && removedUserIds.Any())
                            {
                                var specalityFatUserData = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                if (specalityFatUserData != null && specalityFatUserData.Any())
                                {
                                    var isParentData1 = specalityFatUserData.Any(w => w.ParentId == 0 && removedUserIds.Contains(w.UserId));
                                    if (isParentData1)
                                    {
                                        secondParentId = specalityFatUserData.Where(w => w.ParentId != 0 && !removedUserIds.Contains(w.UserId)).OrderBy(o => o.Id)
                                       .Select(s => s.Id).FirstOrDefault();

                                        //Remove existing data
                                        var removedUsers1 = specalityFatUserData.Where(w => removedUserIds.Any(a => a == w.UserId)).ToList();
                                        if (removedUsers1 != null && removedUsers1.Any())
                                        {
                                            var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                            if (isMappedSku)
                                            {
                                                return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                            }
                                            foreach (var sku in removedUsers1)
                                            {
                                                var isExistData = _emamiContext.SpecalityFatDiscountUsers.Any(f => f.Id == sku.Id);
                                                if (isExistData)
                                                {
                                                    _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                                    _emamiContext.SaveChanges();
                                                }
                                            }
                                        }

                                        //Update Parent Id
                                        var specalityFatUserData1 = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                        if (specalityFatUserData1 != null && specalityFatUserData1.Any())
                                        {
                                            bool isFirst = false;
                                            foreach (var data in specalityFatUserData1)
                                            {
                                                data.ParentId = isFirst ? secondParentId : 0;
                                                data.ActualDiscount = inputDto.QuantityLimit;
                                                data.RemainingQuantity = inputDto.QuantityLimit;
                                                data.ModifiedBy = inputDto.LoginUserId;
                                                data.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                _emamiContext.SaveChanges();
                                                isFirst = true;
                                            }
                                        }
                                        inputDto.Id = secondParentId;
                                    }
                                    else
                                    {
                                        //Remove existing data
                                        //var removedUsers1 = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                        var removedUsers1 = specalityFatData.Where(w => removedUserIds.Any(a => a == w.UserId)).ToList();
                                        if (removedUsers1 != null && removedUsers1.Any())
                                        {
                                            var isMappedSku = _emamiContext.SpecalityFatDiscountUsers.Any(a => a.ParentQuantityId == inputDto.Id);
                                            if (isMappedSku)
                                            {
                                                return _resultService.ErrorMessage(Constants.SkuAlreadyMappedToUser);
                                            }
                                            foreach (var sku in removedUsers1)
                                            {
                                                _emamiContext.SpecalityFatDiscountUsers.Remove(sku);
                                                _emamiContext.SaveChanges();
                                            }
                                        }
                                        var updateSpecalityFatData = _emamiContext.SpecalityFatDiscountUsers.Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).ToList();
                                        if (updateSpecalityFatData != null && updateSpecalityFatData.Any())
                                        {
                                            foreach (var specialtyData in updateSpecalityFatData)
                                            {
                                                specialtyData.ActualDiscount = inputDto.QuantityLimit;
                                                specialtyData.RemainingQuantity = inputDto.QuantityLimit;
                                                specialtyData.ModifiedBy = inputDto.LoginUserId;
                                                specialtyData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                                _emamiContext.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }

                            if ((removedSkuIds == null || !removedSkuIds.Any()) && (removedUserIds == null || !removedUserIds.Any()))
                            {
                                foreach (var specialtyData in specalityFatData)
                                {
                                    specialtyData.ActualDiscount = inputDto.QuantityLimit;
                                    specialtyData.RemainingQuantity = inputDto.QuantityLimit;
                                    specialtyData.ModifiedBy = inputDto.LoginUserId;
                                    specialtyData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    _emamiContext.SaveChanges();
                                }
                            }

                            #endregion

                            #region New SKU and User create
                            if (newSkuIds != null && newSkuIds.Any())
                            {
                                foreach (var skuId in newSkuIds)
                                {
                                    foreach (var empId in inputDto.CustomerId)
                                    {
                                        var entity = new SpecalityFatDiscountUser()
                                        {
                                            SkuId = skuId,
                                            UserId = empId,
                                            ActualDiscount = inputDto.QuantityLimit,
                                            ValidFrom = inputDto.ValidFrom,
                                            ValidTo = inputDto.ValidTo,
                                            OilTypeId = inputDto.OilTypeId,
                                            ParentId = (secondParentId == 0 ? inputDto.Id : secondParentId),
                                            ParentQuantityId = inputDto.ParentQuantityId,
                                            RemainingQuantity = inputDto.QuantityLimit,
                                            CreatedBy = inputDto.LoginUserId,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            ModifiedBy = inputDto.LoginUserId,
                                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                        };
                                        _emamiContext.SpecalityFatDiscountUsers.Add(entity);
                                    }
                                }
                            }

                            if (newUserIds != null && newUserIds.Any())
                            {
                                foreach (var empId in newUserIds)
                                {
                                    foreach (var skuId in inputDto.SkuIds)
                                    {
                                        var entity = new SpecalityFatDiscountUser()
                                        {
                                            SkuId = skuId,
                                            UserId = empId,
                                            ActualDiscount = inputDto.QuantityLimit,
                                            ValidFrom = inputDto.ValidFrom,
                                            ValidTo = inputDto.ValidTo,
                                            OilTypeId = inputDto.OilTypeId,
                                            ParentId = (secondParentId == 0 ? inputDto.Id : secondParentId),
                                            ParentQuantityId = inputDto.ParentQuantityId,
                                            RemainingQuantity = inputDto.QuantityLimit,
                                            CreatedBy = inputDto.LoginUserId,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                            ModifiedBy = inputDto.LoginUserId,
                                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                        };
                                        _emamiContext.SpecalityFatDiscountUsers.Add(entity);
                                    }
                                }
                            }
                            _emamiContext.SaveChanges();
                            #endregion

                            #region Remaing Quantity Update
                            if (inputDto.ParentQuantityId > 0)
                            {
                                var sumOfAssignedQuantity = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.ParentQuantityId == inputDto.ParentQuantityId)
                                    .Select(s => s.ActualDiscount).DefaultIfEmpty(0).Sum();
                                var parentEntity = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(w => w.Id == inputDto.ParentQuantityId);
                                if (parentEntity != null)
                                {
                                    decimal remainingQuantity = parentEntity.ActualDiscount - sumOfAssignedQuantity;
                                    parentEntity.RemainingQuantity = remainingQuantity;
                                    _emamiContext.SaveChanges();
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                }
                else
                {

                    var specalityFatData = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.Id);
                    if (specalityFatData != null)
                    {
                        decimal assignedQuantity = 0;
                        if (specalityFatData.ParentQuantityId == 0)
                        {
                            assignedQuantity = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                .Where(w => w.ParentQuantityId == inputDto.Id && w.ParentId == 0).Select(s => s.ActualDiscount).DefaultIfEmpty(0).Sum();
                            if (inputDto.QuantityLimit >= assignedQuantity)
                            {
                                var spccontext = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.Id == inputDto.Id || _.ParentId == inputDto.Id);
                                foreach (var discount in spccontext)
                                {
                                    discount.ActualDiscount = inputDto.QuantityLimit;
                                    discount.RemainingQuantity = inputDto.QuantityLimit - assignedQuantity;
                                    discount.ModifiedBy = inputDto.LoginUserId;
                                    discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                }
                                _emamiContext.SaveChanges();
                            }
                            else
                            {
                                return _resultService.ErrorMessage(specalityFatData.User.Name + " Total quantity is " + specalityFatData.ActualDiscount + ". Used quantity is " + assignedQuantity + ". Total quantity is should be greater then or equal to assigned quantity");
                            }
                        }
                        else
                        {
                            var parentAssignedQuantity = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(w => w.Id == specalityFatData.ParentQuantityId);

                            var extraQuantity = inputDto.QuantityLimit - specalityFatData.ActualDiscount;

                            bool positive = extraQuantity > 0;
                            bool negative = extraQuantity < 0;

                            if (positive)
                            {
                                if (extraQuantity <= parentAssignedQuantity.RemainingQuantity)
                                {
                                    var parntcontext = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.Id == specalityFatData.ParentQuantityId || _.ParentId == specalityFatData.ParentQuantityId);

                                    foreach (var parent in parntcontext)
                                    {
                                        parent.RemainingQuantity = parentAssignedQuantity.RemainingQuantity - extraQuantity;
                                        parent.ModifiedBy = inputDto.LoginUserId;
                                        parent.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    }
                                    _emamiContext.SaveChanges();

                                    var speccontext = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.Id == specalityFatData.Id || _.ParentId == specalityFatData.Id);

                                    foreach (var disusers in speccontext)
                                    {
                                        disusers.ActualDiscount = inputDto.QuantityLimit;
                                        disusers.RemainingQuantity = specalityFatData.RemainingQuantity + extraQuantity;
                                        disusers.ModifiedBy = inputDto.LoginUserId;
                                        disusers.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    }
                                    _emamiContext.SaveChanges();
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.QtyLimitExceeded);
                                }
                            }
                            else
                            {
                                var overallSaudaStatuses = Constants.OverallSaudaStatus;
                                decimal requestedQuantity = inputDto.QuantityLimit;
                                var skuContext = _emamiContext.Skus.AsNoTracking().Where(_ => _.Id == parentAssignedQuantity.SkuId && _.IsActive);
                                var userrolecontext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);

                                if (skuContext != null)
                                {
                                    var userLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                    .FirstOrDefault(_ => inputDto.CustomerId.Contains(_.UserId)
                                    && _.OilTypeId == parentAssignedQuantity.OilTypeId && _.ParentId == 0
                                    && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(parentAssignedQuantity.ValidFrom)
                                    && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(parentAssignedQuantity.ValidTo));
                                    if (userLimitContext != null)
                                    {
                                        var dealerlist = new List<long>();
                                        if (userrolecontext.RoleId == (int)DTO.Enums.Role.NationalTrader)
                                        {
                                            var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => inputDto.CustomerId.Contains(_.ReportingToUserId)).Select(s => s.UserId).ToList();

                                            if (bdoIds.Any())
                                            {
                                                dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                            .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                            .Where(_ => bdoIds.Contains(_.uc.UserId) && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                            }
                                        }
                                        else if (userrolecontext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                                        {
                                            dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                            .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                            .Where(_ => inputDto.CustomerId.Contains(_.uc.UserId) && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                        }

                                        decimal saudaBidQuantity = 0;
                                        if (dealerlist != null && dealerlist.Any())
                                        {
                                            saudaBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.OilTypeId == parentAssignedQuantity.OilTypeId && dealerlist.Contains(_.Sauda.UserId)
                                                  && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(userLimitContext.ValidFrom)
                                                  && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(userLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda)
                                                  .Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
                                        }

                                        decimal orderedQuantity = 0;
                                        decimal totalQuantity = requestedQuantity;
                                        if (saudaBidQuantity != 0)
                                        {
                                            orderedQuantity = saudaBidQuantity;
                                            totalQuantity = specalityFatData.ActualDiscount - orderedQuantity;
                                        }
                                        if (totalQuantity < specalityFatData.RemainingQuantity)
                                        {
                                            var speccontext = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.Id == specalityFatData.Id || _.ParentId == specalityFatData.Id);

                                            foreach (var disusers in speccontext)
                                            {
                                                disusers.ActualDiscount = inputDto.QuantityLimit;
                                                disusers.RemainingQuantity = inputDto.QuantityLimit - orderedQuantity;
                                                disusers.ModifiedBy = inputDto.LoginUserId;
                                                disusers.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            }
                                            _emamiContext.SaveChanges();

                                            var parntcontext = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.Id == specalityFatData.ParentQuantityId || _.ParentId == specalityFatData.ParentQuantityId);

                                            foreach (var parent in parntcontext)
                                            {
                                                parent.RemainingQuantity = parentAssignedQuantity.RemainingQuantity - extraQuantity;
                                                parent.ModifiedBy = inputDto.LoginUserId;
                                                parent.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                            }
                                            _emamiContext.SaveChanges();
                                        }
                                        else
                                        {
                                            return _resultService.ErrorMessage(Constants.QtyLimitReduceExceed);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                }

                try
                {
                    SpecialityFatLimitNotification(inputDto);
                }
                catch (Exception)
                {
                }
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public void SpecialityFatLimitNotification(SpecialityFatDiscountUserDto inputDto)
        {
            try
            {
                var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SkuId)?.SkuName;
                {
                    var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => inputDto.CustomerId.Contains(_.Id)).ToList();
                    if (usersContext != null && usersContext.Any())
                    {
                        List<string> toUsers = new List<string>();
                        toUsers.AddRange(usersContext.Select(_ => _.Email));
                        string fromDate = inputDto.ValidFrom.ToString("MMM dd,yyyy");
                        string toDate = inputDto.ValidTo.ToString("MMM dd,yyyy");

                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        if (_resultService.IsEmail())
                        {
                            var fromEmail = Constants.FromEmail;
                            EmailTemplate emailTemplate = new EmailTemplate();
                            var plainText = string.Empty;
                            var emailSubject = Constants.SpecialityFatLimitSubject;
                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountUserEmail);
                            if (emailTemplate != null)
                            {
                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                    .Replace(Constants.Quantity, inputDto.QuantityLimit.ToString());
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                            }
                        }
                        var smsPlainTemplate = string.Empty;
                        if (_resultService.IsSMS())
                        {
                            var smsMessage = string.Empty;
                            EmailTemplate smsTemplate = new EmailTemplate();
                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountUserSMS);
                            if (smsTemplate != null)
                            {
                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                    .Replace(Constants.Quantity, inputDto.QuantityLimit.ToString());
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                try
                                {
                                    foreach (var mobileNumber in usersContext.Select(_ => _.MobileNumber).ToList())
                                    {
                                        amazonNotificationService.SendMessage(smsMessage, mobileNumber);
                                    }
                                }
                                catch (Exception ex)
                                {

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

        public ResultDto GetSpecialityFatDiscountUserList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSpecialityFatDiscountUserList";
            var resultDto = new ResultDto();
            var discountUsers = new List<SpecalityFatDiscountUser>();
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    discountUsers = _emamiContext.SpecalityFatDiscountUsers
                        .AsNoTracking().Where(w => w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId).ToList();
                }


                var result = discountUsers.AsEnumerable().Select(s => new SpecialityFatDiscountUserDto()
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = s.Id,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeName = s.OilType != null ? s.OilType.Name + "-" + s.OilType.SalesOrganization.Code + "/" + s.OilType.DistributionChannel.Code + "/" + s.OilType.Division.Code : String.Empty,
                    //OilTypeCode = s.OilType?.SAPCode,
                    QuantityLimit = s.ActualDiscount,
                    RemainingQuantity = s.RemainingQuantity,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                }).OrderByDescending(o => o.Id).ToList();
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

        public ResultDto GetSpecialityFatDiscountUserExport(LoginUserIdDto inputDto)
        {
            _methodName = "GetSpecialityFatDiscountUserList";
            var resultDto = new ResultDto();
            var result = new List<SpecialityFatEmployeeExportDto>();
            var discountUsers = new List<SpecalityFatDiscountUser>();
            try
            {
                discountUsers = _emamiContext.SpecalityFatDiscountUsers
                         .AsNoTracking().Where(w => w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId).ToList();


                result = discountUsers.Select(s => new SpecialityFatEmployeeExportDto()
                {
                    Id = s.Id,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeName = s.OilType != null ? s.OilType.Name + "-" + s.OilType.SalesOrganization.Code + "/" + s.OilType.DistributionChannel.Code + "/" + s.OilType.Division.Code : String.Empty,
                    //OilTypeCode = s.OilType?.SAPCode,
                    QuantityLimit = s.ActualDiscount,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                }).ToList();

                result.ForEach(f =>
                {
                    f.InnerList = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), sf => sf.UserId, u => u.Id, (sf, u) => new { SpecalityFat = sf, Users = u })
                    .Where(w => w.SpecalityFat.Id == f.Id || w.SpecalityFat.ParentId == f.Id)
                    .Select(s => new SpecialityFatEmployeeDto()
                    {
                        Id = s.SpecalityFat.Id,
                        SkuName = s.SpecalityFat.Sku.SkuName,
                        SkuCode = s.SpecalityFat.Sku.SkuCode,
                        EmployeeName = s.Users.Name,
                        Email = s.Users.Email,
                        MobileNumber = s.Users.MobileNumber,
                        Designation = s.Users.Designation,
                        Quantity = s.SpecalityFat.ActualDiscount,
                        RemainingQuantity = s.SpecalityFat.RemainingQuantity
                    }).ToList();
                });




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

        public ResultDto GetSpecialityFatDiscountUserById(long discountId)
        {
            _methodName = "GetSpecialityFatDiscountUserById";
            var resultDto = new ResultDto();
            var discountUsers = new List<SpecalityFatDiscountUser>();
            try
            {
                discountUsers = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.ParentId == discountId || w.Id == discountId).ToList();
                if (discountUsers != null && discountUsers.Any())
                {
                    var data = discountUsers.FirstOrDefault();
                    var salesOrg = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Id == data.SalesOrganizationId);
                    var distchnlId = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Id == data.DistributionChannelId);
                    var divisionId = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Id == data.DivisionId);
                    var oiltype = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == data.OilTypeId);
                    var users = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == data.UserId);
                    var result = new SpecialityFatDiscountUserDto()
                    {
                        Id = data.Id,
                        VerticleId = data.DivisionId,
                        SalesOrganizationId = data.SalesOrganizationId,
                        DistributionChannelId = data.DistributionChannelId,
                        OilTypeId = data.OilTypeId,
                        SkuId = data.SkuId,
                        DivisionName = divisionId != null ? divisionId.Name : string.Empty,
                        QuantityLimit = data.ActualDiscount,
                        ValidFrom = data.ValidFrom,
                        ValidTo = data.ValidTo,
                        SalesOrganizationName = salesOrg != null ? salesOrg.Name : string.Empty,
                        DistributionChannelName = distchnlId != null ? distchnlId.Name : string.Empty,
                        OilTypeName = oiltype != null ? oiltype.Name : string.Empty,
                        ParentQuantityId = data.ParentQuantityId,
                        ParentId = data.ParentId,
                        CustomerId = discountUsers.Select(s => s.UserId).Distinct().ToList(),
                        CustomerName = users != null ? users.Name : string.Empty,
                        SkuIds = discountUsers.Select(s => s.SkuId).Distinct().ToList(),
                        //SubCategoryId = data.Sku.SubCategoryId
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

        public ResultDto GetSpecialityFatDiscountUserDetailList(GeographyCityListParam inputDto)
        {
            _methodName = "GetSpecialityFatDiscountUserDetailList";
            var resultDto = new ResultDto();
            var result = new List<SpecialityFatEmployeeDto>();
            try
            {
                var userIds = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id).Select(s => s.UserId).ToList();

                result = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), sf => sf.UserId, u => u.Id, (sf, u) => new { SpecalityFat = sf, Users = u })
                    .Where(w => w.SpecalityFat.Id == inputDto.Id || w.SpecalityFat.ParentId == inputDto.Id)
                    .Select(s => new SpecialityFatEmployeeDto()
                    {
                        Id = s.SpecalityFat.Id,
                        SkuName = s.SpecalityFat.Sku.SkuName,
                        SkuCode = s.SpecalityFat.Sku.SkuCode,
                        EmployeeName = s.Users.Name,
                        Email = s.Users.Email,
                        MobileNumber = s.Users.MobileNumber,
                        Designation = s.Users.Designation,
                        Quantity = s.SpecalityFat.ActualDiscount,
                        RemainingQuantity = s.SpecalityFat.RemainingQuantity
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

        public ResultDto GetSpecialityFatEmployeeDiscountList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSpecialityFatEmployeeDiscountList";
            var resultDto = new ResultDto();
            var discountUsers = new List<SpecalityFatDiscountUser>();
            try
            {
                discountUsers = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.UserId == inputDto.LoginUserId && w.ParentId == 0).ToList();



                var result = discountUsers.Select(s => new SpecialityFatDiscountUserDto()
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = s.Id,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeId = s.OilTypeId,
                    OilTypeName = s.OilType != null ? s.OilType.Name + "-" + s.OilType.SalesOrganization.Code + "/" + s.OilType.DistributionChannel.Code + "/" + s.OilType.Division.Code : String.Empty,
                    //OilTypeCode = s.OilType?.SAPCode,
                    QuantityLimit = s.ActualDiscount,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                    RemainingQuantity = s.RemainingQuantity,
                    VerticleId = s.DivisionId,
                    EmployeeName = s.User.Name
                }).OrderByDescending(dto => dto.Id).ToList();
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
        public ResultDto GetSpecialityFatEmployeeDiscountExport(LoginUserIdDto inputDto)
        {
            _methodName = "GetSpecialityFatEmployeeDiscountExport";
            var resultDto = new ResultDto();
            var output = new List<SpecialityFatDiscountUserExportDto>();

            var discountUsers = new List<SpecalityFatDiscountUser>();
            try
            {
                discountUsers = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.UserId == inputDto.LoginUserId).ToList();


                var result = discountUsers.Select(s => new SpecialityFatDiscountUserExportDto()
                {
                    Id = s.Id,
                    OilTypeName = s.OilType != null ? s.OilType.Name + "-" + s.OilType.SalesOrganization.Code + "/" + s.OilType.DistributionChannel.Code + "/" + s.OilType.Division.Code : String.Empty,
                    //OilTypeCode = s.OilType?.SAPCode,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                    EmployeeName = s.User.Name
                });
                foreach (var item in result)
                {
                    item.SpecialityFatDiscountDetails = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                   .Where(w => w.Id == item.Id)
                    .Select(s => new SpecialityFatDiscountInnerListExportDto()
                    {
                        Id = s.Id,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        QuantityLimit = s.ActualDiscount,
                    }).ToList();
                    output.Add(item);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = output;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetSpecialityFatDiscountEmployeeDetailList(GeographyCityListParam inputDto)
        {
            _methodName = "GetSpecialityFatDiscountEmployeeDetailList";
            var resultDto = new ResultDto();
            var result = new List<SpecialityFatDiscountUserDto>();
            try
            {
                result = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                   .Where(w => w.Id == inputDto.Id || w.ParentId == inputDto.Id)
                    .Select(s => new SpecialityFatDiscountUserDto()
                    {
                        Id = s.Id,
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType.Name,
                        QuantityLimit = s.ActualDiscount,
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

        public ResultDto GetSpecialityFatEmployeeDiscountById(IdInputDto inputDto)
        {
            _methodName = "GetEmployeeAndUserDiscountById";
            var resultDto = new ResultDto();
            var discountData = new SpecalityFatDiscountUser();
            try
            {
                var result = new EmployeeUserDiscountDto();
                var specialityFatContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking();
                discountData = specialityFatContext.FirstOrDefault(w => w.Id == inputDto.Id);
                if (discountData != null)
                {
                    //if (discountData.DivisionId == (int)DTO.Enums.Division.Hbc)
                    //{
                    var totalActualDiscount = specialityFatContext.Where(w => w.Id == inputDto.Id).Select(s => s.ActualDiscount).Sum();
                    var totalRemainingDiscount = specialityFatContext.Where(w => w.Id == inputDto.Id).Select(s => s.RemainingQuantity).Sum();

                    var salesOrg = _emamiContext.SalesOrganization.AsNoTracking().FirstOrDefault(_ => _.Id == discountData.SalesOrganizationId);
                    var distchnlId = _emamiContext.DistributionChannel.AsNoTracking().FirstOrDefault(_ => _.Id == discountData.DistributionChannelId);
                    var divisionId = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Id == discountData.DivisionId);

                    result = new EmployeeUserDiscountDto()
                    {
                        Id = discountData.Id,
                        VerticleId = discountData.OilType.DivisionId,
                        OilTypeId = discountData.OilTypeId,
                        SkuId = discountData.SkuId,
                        SkuName = discountData.Sku.SkuName,
                        OilTypeName = discountData.OilType?.Name,
                        ActualDiscount = totalActualDiscount,
                        SalesOrganizationName = salesOrg != null ? salesOrg.Name : string.Empty,
                        DistributionChannelName = distchnlId != null ? distchnlId.Name : string.Empty,
                        DivisionName = divisionId != null ? divisionId.Name : string.Empty,
                        ValidFrom = discountData.ValidFrom,
                        ValidTo = discountData.ValidTo,
                        SalesOrganizationId = discountData.SalesOrganizationId,
                        DistributionChannelId = discountData.DistributionChannelId,
                        RemainingQuantity = totalRemainingDiscount
                    };
                    //}
                    //else
                    //{
                    //    result = new EmployeeUserDiscountDto()
                    //    {
                    //        Id = discountData.Id,
                    //        VerticleId = discountData.OilType.DivisionId,
                    //        OilTypeId = discountData.OilTypeId,
                    //        SkuId = discountData.SkuId,
                    //        SkuName = discountData.Sku.SkuName,
                    //        OilTypeName = discountData.OilType?.Name,
                    //        ActualDiscount = discountData.ActualDiscount,
                    //        ValidFrom = discountData.ValidFrom,
                    //        ValidTo = discountData.ValidTo,
                    //        RemainingQuantity = discountData.RemainingQuantity
                    //    };
                    //}


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

        public ResultDto AddSpecialityFatEmployeeDiscount(SpecialityFatEmployeeDiscountDto inputDto)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
            try
            {
                if (inputDto == null || inputDto.CustomerId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                //var skuIds = new List<long>();

                //skuIds.Add(inputDto.SkuId);
                var discountData = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    #region Validation


                    var userId = inputDto.CustomerId;
                    var details = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .Where(w => w.OilTypeId == inputDto.OilTypeId
                    //&& skuIds.Contains(w.SkuId) 
                    // && (w.Id == inputDto.Id && w.ParentId == inputDto.Id)
                    && userId.Contains(w.UserId)
                    && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))
                    || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))));

                    if (details.IsAny())
                    {
                        var notWithinCurrentDiscount = details.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.UserId).ToList();
                        var notWithinCurrentDiscountForSkus = details.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.OilType.Name).Distinct().ToList();
                        if (notWithinCurrentDiscount != null && notWithinCurrentDiscount.Any() && notWithinCurrentDiscount.Count > 0)
                        {
                            var userName = _emamiContext.Users.AsNoTracking().Where(w => notWithinCurrentDiscount.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                            return _resultService.ErrorMessage(Constants.QtyLimitAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)) + " with OilType : " + string.Join(",", notWithinCurrentDiscountForSkus));
                        }
                    }

                    #endregion

                    if (!(inputDto.EmpValidFrom.Date >= discountData.ValidFrom.Date && inputDto.EmpValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.EmpValidTo.Date <= discountData.ValidTo.Date && inputDto.EmpValidTo.Date >= discountData.ValidFrom.Date))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Please select a Valid From and Valid To date";
                        return resultDto;
                    }

                    var specialityFatDiscountUpdate = _emamiContext.SpecalityFatDiscountUsers.Where(w => (w.Id == discountData.Id || w.ParentId == discountData.Id)
                    //&& skuIds.Contains(w.SkuId)
                    ).ToList();
                    if (!(inputDto.EmpActualDiscount <= discountData.ActualDiscount))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Actual quantity limit is " + discountData.ActualDiscount + ". Please enter less than or equal to quantity";
                        return resultDto;
                    }

                    inputDto.SkuIds = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId &&
                      _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                      && _.DivisionId == inputDto.VerticleId && _.IsActive).Select(s => s.Id).ToList();
                    foreach (var userid in inputDto.CustomerId)
                    {
                        isFirstRecord = false;
                        parentId = 0;
                        foreach (var skuid in inputDto.SkuIds)
                        {
                            var result = new SpecalityFatDiscountUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = skuid,
                                UserId = userid,
                                ActualDiscount = inputDto.EmpActualDiscount,
                                ParentId = parentId,
                                ParentQuantityId = discountData.Id,
                                RemainingQuantity = inputDto.EmpActualDiscount,
                                ValidFrom = inputDto.EmpValidFrom,
                                ValidTo = inputDto.EmpValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                DivisionId = inputDto.VerticleId,
                                SalesOrganizationId = inputDto.SalesOrganizationId,
                                DistributionChannelId = inputDto.DistributionChannelId
                            };

                            _emamiContext.SpecalityFatDiscountUsers.Add(result);
                            if (!isFirstRecord)
                            {
                                isFirstRecord = true;
                                _emamiContext.SaveChanges();
                                parentId = result.Id;
                            }
                        }
                    }
                    var discountlist = _emamiContext.SpecalityFatDiscountUsers.Where(_ => _.Id == inputDto.Id || _.ParentId == inputDto.Id);
                    foreach (var discount in discountlist)
                    {
                        discount.RemainingQuantity = inputDto.RemainingQuantityHidden;
                    }
                    _emamiContext.SaveChanges();

                    try
                    {
                        var input = new SpecialityFatDiscountUserDto()
                        {
                            CustomerId = inputDto.CustomerId,
                            SkuId = inputDto.SkuId,
                            QuantityLimit = inputDto.EmpActualDiscount,
                            ValidFrom = inputDto.EmpValidFrom,
                            ValidTo = inputDto.EmpValidTo,
                        };
                        SpecialityFatLimitNotification(input);
                    }
                    catch (Exception ex)
                    {
                    }
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

        #endregion

        #region Auto Allocation

        public ResultDto GetAutoAllocationUserListByRoleIds(AutoAllocationInputDto inputDto)
        {
            _methodName = "GetAutoAllocationUserListByRoleIds";
            var resultDto = new ResultDto();
            var outputDto = new List<AutoAllocationDto>();
            try
            {
                List<long> roleIds = inputDto.RoleIds.Split(',').Select(long.Parse).ToList();
                outputDto = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking()
                    , sfd => sfd.UserId, u => u.Id, (sfd, u) => new { sfd, u })
                    .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => roleIds.Contains(_.RoleId)), u => u.u.Id, ur => ur.UserId, (u, ur)
                     => new { SpecalityFatDiscountUsers = u.sfd, Users = u.u, UserRoles = ur })
                    .Select(_ => new AutoAllocationDto
                    {
                        UserId = _.SpecalityFatDiscountUsers.UserId,
                        UserName = _.Users.Name,
                        MobileNumber = _.Users.MobileNumber,
                        RoleName = _.UserRoles.Role.Name,
                        //Vertical = _.Users.Division.Name
                    }).Distinct()
                    .ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetAutoAllocationDetailsByUserId(AutoAllocationInputDto inputDto)
        {
            _methodName = "GetAutoAllocationDetailsByUserId";
            var resultDto = new ResultDto();
            var outputDto = new List<AutoAllocationDetailDto>();
            try
            {
                if (inputDto.AverageDays <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                DateTime FromDate = DateTime.Now.AddDays(-inputDto.AverageDays);
                DateTime ToDate = DateTime.Now.AddDays(-1);


                var SpecalityFatDiscountContext = (from sfd in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                                   join sk in _emamiContext.Skus.AsNoTracking() on sfd.SkuId equals sk.Id
                                                   where
                                                   ((DbFunctions.TruncateTime(sfd.ValidFrom) >= DbFunctions.TruncateTime(FromDate) &&
                                                   DbFunctions.TruncateTime(sfd.ValidFrom) <= DbFunctions.TruncateTime(ToDate))
                                                   ||
                                                   (DbFunctions.TruncateTime(sfd.ValidTo) >= DbFunctions.TruncateTime(FromDate) &&
                                                   DbFunctions.TruncateTime(sfd.ValidTo) <= DbFunctions.TruncateTime(ToDate)))
                                                   && sfd.UserId == inputDto.UserId
                                                   select new AutoAllocationDetailDto()
                                                   {
                                                       UserId = sfd.UserId,
                                                       SkuId = sfd.SkuId,
                                                       SkuName = sfd.Sku.SkuName,
                                                       SkuCode = sfd.Sku.SkuCode,
                                                       ActualDiscount = sfd.ActualDiscount,
                                                       RequestedDiscount = sfd.RequestedDiscount
                                                   });

                var userlistContext = SpecalityFatDiscountContext.Select(_ => new
                {
                    userid = _.UserId,
                    skuid = _.SkuId,
                    skuname = _.SkuName,
                    _.SkuCode
                }).Distinct().ToList();

                foreach (var item in userlistContext)
                {
                    var specialityfatDiscount = SpecalityFatDiscountContext.Where(_ => _.UserId == item.userid && _.SkuId == item.skuid).ToList();
                    var dto = new AutoAllocationDetailDto()
                    {
                        UserId = item.userid,
                        SkuId = item.skuid,
                        SkuName = item.skuname,
                        SkuCode = item.SkuCode,
                        ActualDiscount = specialityfatDiscount.Sum(_ => _.ActualDiscount) / specialityfatDiscount.Count,
                        RequestedDiscount = specialityfatDiscount.Sum(_ => _.RequestedDiscount) / specialityfatDiscount.Count
                    };
                    outputDto.Add(dto);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto SaveSpecalityFatDiscountUsers(SaveAutoAllocationDetailDto inputDto)
        {
            _methodName = "SaveSpecalityFatDiscountUsers";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                string errorMessageList = string.Empty;
                foreach (var item in inputDto.autoAllocationDetailDtos)
                {
                    var specalityFatDiscountUsers = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .Where(w => w.SkuId == item.SkuId && w.UserId == item.UserId
                    && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(item.ValidFrom)
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(item.ValidTo))
                    || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(item.ValidFrom)
                    && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(item.ValidTo)))).Count();
                    string skuName = _emamiContext.Skus.FirstOrDefault(_ => _.Id == item.SkuId).SkuName;
                    if (specalityFatDiscountUsers > 0)
                    {

                        string Username = _emamiContext.Users.FirstOrDefault(_ => _.Id == item.UserId).Name;
                        var errorMessage = "Sku: " + skuName + ",User: " + Username + ",ValidFrom: " + item.ValidFrom.ToString("dd-MMM-yyyy")
                           + ",ValidTo: " + item.ValidTo.ToString("dd-MMM-yyyy") + ", Message:" + Constants.SpecalityFatDiscountAlreadyExistiInThisDate + ";";
                        errorMessageList = errorMessageList + errorMessage;
                    }
                    else
                    {
                        var input = new SpecalityFatDiscountUser
                        {
                            SkuId = item.SkuId,
                            UserId = item.UserId,
                            OilTypeId = _emamiContext.Skus.FirstOrDefault(_ => _.Id == item.SkuId).OilTypeId ?? 0,
                            ActualDiscount = item.ActualDiscount,
                            RequestedDiscount = item.RequestedDiscount,
                            ValidFrom = item.ValidFrom,
                            ValidTo = item.ValidTo,
                            CreatedBy = item.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            DivisionId = _emamiContext.Skus.FirstOrDefault(_ => _.Id == item.SkuId).DivisionId,
                            RemainingQuantity = item.ActualDiscount
                        };
                        _emamiContext.SpecalityFatDiscountUsers.Add(input);
                        _emamiContext.SaveChanges();
                        item.IsSentMail = true;
                    }
                }
                foreach (var item in inputDto.autoAllocationDetailDtos)
                {
                    try
                    {
                        if (item.IsSentMail)
                        {
                            string skuName = _emamiContext.Skus.FirstOrDefault(_ => _.Id == item.SkuId).SkuName;
                            User usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == item.UserId);
                            if (usersContext != null)
                            {
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                var emailSubject = string.Empty;
                                if (_resultService.IsEmail())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var plainText = string.Empty;
                                    EmailTemplate emailTemplate = new EmailTemplate();
                                    emailSubject = Constants.SpecalityFatDiscountUserSubject;
                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountUserSaveEmail);

                                    if (emailTemplate != null)
                                    {
                                        if (usersContext != null && !string.IsNullOrEmpty(usersContext.Email))
                                        {
                                            List<string> toUsers = new List<string>();
                                            toUsers.Add(usersContext.Email);
                                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, usersContext.Name)
                                                .Replace(Constants.SkuName, skuName).Replace(Constants.ActualDiscount, (Math.Round(item.ActualDiscount, 2)).ToString());
                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                        }
                                    }
                                }
                                var smsPlainTemplateCreatedBy = string.Empty;
                                var smsPlainTemplateDealer = string.Empty;
                                if (_resultService.IsSMS())
                                {
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountUserSaveSMS);

                                    if (smsTemplate != null)
                                    {
                                        if (usersContext != null && !string.IsNullOrEmpty(usersContext.MobileNumber))
                                        {
                                            smsPlainTemplateCreatedBy = smsTemplate.PlainTemplate.Replace(Constants.UserName, usersContext.Name)
                                                .Replace(Constants.SkuName, skuName).Replace(Constants.ActualDiscount, (Math.Round(item.ActualDiscount, 2)).ToString());
                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplateCreatedBy);
                                            amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
                                        }
                                    }
                                }
                                if (_resultService.IsPushNotification())
                                {
                                    if (usersContext != null && usersContext.RegistrationTypeId != null && usersContext.RegistrationTypeId > 0
                                        && !string.IsNullOrEmpty(usersContext.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = usersContext.PushTokenKey,
                                            RegistrationTypeId = usersContext.RegistrationTypeId != null ? (int)usersContext.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = smsPlainTemplateCreatedBy,
                                            //Id = result.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (errorMessageList != string.Empty)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = errorMessageList;
                }
                else
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Message = errorMessageList;
                }
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisDate);

            }
        }

        #endregion
    }
}
