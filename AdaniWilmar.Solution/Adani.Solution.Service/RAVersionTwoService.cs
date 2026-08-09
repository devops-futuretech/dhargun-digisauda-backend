using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Adani.Solution.DTO.Enums;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data.Entity;
using GMCore.Helper;
using System.Web.Hosting;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace Adani.Solution.Service
{
    public interface IRAVersionTwoService
    {

        #region SchemeDiscount - GeographyBased

        ResultDto SaveSchemeDiscountGeography(SchemeDiscountGeographyDto inputDto);
        ResultDto UpdateSchemeDiscountGeography(SchemeDiscountGeographyDto inputDto);
        ResultDto GetGeographyBasedSchemeDiscountListWithPagination(KendoGridResult inputDto);
        ResultDto GetGeographyBasedSchemeDiscountById(long SchemeDiscountId);
        ResultDto GetSchemeDiscountGeographyHierarchyListById(KendoGridResult inputDto);
        ResultDto ExportGeographyBasedSchemeDiscount(LoginUserIdDto inputDto);
        ResultDto UpdateSchemeDiscountGeographyListByIsActive(IdDiscountAndBenefitInputDto inputDto);
        #endregion

        #region Reporting To Users - Customer Group

        ResultDto GetRAZonalHeadUsersByCustomerGroup(CustomerGroupInputDto inputDto);
        ResultDto GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds(DropDownInputDto inputDto);
        ResultDto GetRABDOUsersByZonalHeadIdsAndVerticalIds(DropDownInputDto inputDto);

        #endregion

        #region Lookup

        ResultDto GetCustomerGroupListByVerticalIdsForDropdown(IdInputDto inputDto);
        ResultDto GetCustomerListByCustomerGroupIdsAndBDOsForDropdown(DropDownInputDto inputDto);
        ResultDto GetCustomerListByCustomerGroupIdsCityIdsForDropdown(DropDownInputDto inputDto);
        #endregion


    }

    public class RAVersionTwoService : IRAVersionTwoService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("RAVersionTwoService");
        private const string ServiceName = "RAVersionTwo Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public RAVersionTwoService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _notificationService = notificationService;
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for RAVersionTwo Service", exception);
            }
        }

        #region CustomerGroup

        /// <summary>
        /// Method to Get Customer Group List With Pagination
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetCustomerGroupListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetCustomerGroupListWithPagination";
            var resultDto = new ResultDto();
            try
            {
                DataSourceResult dataSourceResult = new DataSourceResult();
                var outputDto = new List<CustomerGroupDto>();
                IQueryable<CustomerGroups> resultContext;
                if (inputDto.IsToReturnInactiveData)
                {
                    resultContext = _emamiContext.CustomerGroups.AsNoTracking();
                }
                else
                {
                    resultContext = _emamiContext.CustomerGroups.AsNoTracking().Where(_ => _.IsActive);
                }

                if (resultContext != null && resultContext.Any())
                {
                    dataSourceResult = resultContext.Select(s => new CustomerGroupDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsBaseGroup = s.IsBaseGroup,
                        VerticalId = s.DivisionId,
                        Vertical = s.Division.Name,
                        IsActive = s.IsActive,
                    }).ToDataSourceResult(inputDto.DataSourceRequest);
                }

                resultDto.SuccessDto.Response = dataSourceResult;
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
        /// Method to Save Customer Group
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto SaveCustomerGroup(CustomerGroupDto inputDto)
        {
            _methodName = "AddCustomerGroup";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var nameExist = _emamiContext.CustomerGroups.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name);
                if (nameExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }

                var inputEntity = new CustomerGroups();
                inputEntity.Name = inputDto.Name;
                inputEntity.IsActive = inputDto.IsActive;
                inputEntity.IsBaseGroup = inputDto.IsBaseGroup;
                inputEntity.DivisionId = inputDto.VerticalId;
                inputEntity.CreatedBy = inputDto.LoginUserId;
                inputEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.CustomerGroups.Add(inputEntity);
                _emamiContext.SaveChanges();

                foreach (var item in inputDto.CustomerGroupDetailDtoList)
                {
                    var customerGroupDetail = new CustomerGroupDetails();
                    customerGroupDetail.CustomerGroupId = inputEntity.Id;
                    customerGroupDetail.CustomerId = item.CustomerId;
                    customerGroupDetail.CreatedBy = inputDto.LoginUserId;
                    customerGroupDetail.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.CustomerGroupDetails.Add(customerGroupDetail);
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

        /// <summary>
        /// Method to Update Customer Group
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateCustomerGroup(CustomerGroupDto inputDto)
        {
            _methodName = "UpdateCustomerGroup";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var nameExist = _emamiContext.CustomerGroups.AsNoTracking().FirstOrDefault(_ => _.Name == inputDto.Name && _.Id != inputDto.Id);
                if (nameExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }

                var inputEntity = _emamiContext.CustomerGroups.FirstOrDefault(_ => _.Id == inputDto.Id);
                inputEntity.Name = inputDto.Name;
                inputEntity.IsActive = inputDto.IsActive;
                inputEntity.IsBaseGroup = inputDto.IsBaseGroup;
                inputEntity.ModifiedBy = inputDto.LoginUserId;
                inputEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                var customerGroupDetailExist = _emamiContext.CustomerGroupDetails.Where(f => f.CustomerGroupId == inputDto.Id);
                if (customerGroupDetailExist != null && customerGroupDetailExist.Any())
                {
                    foreach (var recordDelete in customerGroupDetailExist)
                    {
                        _emamiContext.CustomerGroupDetails.Remove(recordDelete);
                    }
                    _emamiContext.SaveChanges();
                }

                foreach (var item in inputDto.CustomerGroupDetailDtoList)
                {
                    var customerGroupDetail = new CustomerGroupDetails();
                    customerGroupDetail.CustomerGroupId = inputDto.Id;
                    customerGroupDetail.CustomerId = item.CustomerId;
                    customerGroupDetail.CreatedBy = inputDto.LoginUserId;
                    customerGroupDetail.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.CustomerGroupDetails.Add(customerGroupDetail);
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

        /// <summary>
        /// Method to Get Customer Group By Id
        /// </summary>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public ResultDto GetCustomerGroupById(long customerGroupId)
        {
            _methodName = "GetCustomerGroupById";
            var resultDto = new ResultDto();
            var result = new CustomerGroupDto();
            try
            {
                var resultContext = _emamiContext.CustomerGroups.AsNoTracking().FirstOrDefault(f => f.Id == customerGroupId);
                if (resultContext != null)
                {
                    result = new CustomerGroupDto()
                    {
                        Id = resultContext.Id,
                        Name = resultContext.Name,
                        IsActive = resultContext.IsActive,
                        IsBaseGroup = resultContext.IsBaseGroup,
                        VerticalId = resultContext.DivisionId
                    };
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
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

        /// <summary>
        /// Method to Export Customer Group
        /// </summary>
        /// <param name="loginUserIdDto"></param>
        /// <returns></returns>
        public ResultDto ExportCustomerGroup(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportCustomerGroup";
            var resultDto = new ResultDto();
            try
            {
                var outputDto = new List<CustomerGroupDto>();
                var resultContext = _emamiContext.CustomerGroups.AsNoTracking();
                outputDto = resultContext.ToList().Select(c => new CustomerGroupDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    IsBaseGroup = c.IsBaseGroup,
                    CustomerGroupDetailDtoList = c.CustomerGroupDetails.Select(_ => new CustomerGroupDetailDto
                    {
                        CustomerGroupId = _.CustomerGroupId,
                        CustomerGroupName = _.CustomerGroup != null ? _.CustomerGroup.Name : string.Empty,
                        CustomerId = _.CustomerId,
                        CustomerName = _.Customer != null ? _.Customer.Name : string.Empty,
                        Code = _.Customer.Code,
                        MobileNumber = _.Customer.MobileNumber,
                        Email = _.Customer.Email,
                        State = _.Customer.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.StateId)?.StateName : string.Empty,
                        District = _.Customer.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.DistrictId)?.DistrictName : string.Empty,
                        Territory = _.Customer.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.TerritoryId)?.Name : string.Empty,
                        Zone = _.Customer.ZoneId > 0 ? _emamiContext.Zones.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.ZoneId)?.Name : string.Empty,
                        City = _.Customer.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.CityId)?.CityName : string.Empty,

                    }).ToList()
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

        /// <summary>
        /// Method to Get Customer Group Details List By Customer Group Id
        /// </summary>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public ResultDto GetCustomerGroupDetailsListById(KendoGridResult inputDto)
        {
            _methodName = "GetCustomerGroupDetailsListById";
            var resultDto = new ResultDto();
            var result = new DataSourceResult();
            try
            {
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    string query = @"Select cg.CustomerGroupId,c.Name as CustomerGroupName,cg.CustomerId,u.Name as CustomerName,
	                        u.Code,	u.MobileNumber,	u.Email,st.StateName as State,d.DistrictName as District,t.Name as Territory,
	                        z.Name as Zone,	ct.CityName as City
	                        From CustomerGroups c Left Join CustomerGroupDetails cg on c.Id = cg.CustomerGroupId
	                        Left Join Users u on u.Id = cg.CustomerId Left Join States st on st.Id = u.StateId
	                        Left Join Zones z on z.Id = u.ZoneId Left Join Territories t on t.Id = u.TerritoryId
	                        Left Join Districts d on d.Id = u.DistrictId Left Join Cities ct on ct.Id = u.CityId
	                        Where cg.CustomerGroupId = @CustomerGroupId";

                    result = conn.Query<CustomerGroupDetailDto>(query, new
                    {
                        CustomerGroupId = inputDto.Id
                    }).ToDataSourceResult(inputDto.DataSourceRequest);
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

        /// <summary>
        /// Method to Get Customer Group Details List By Customer Group Id
        /// </summary>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        //public ResultDto GetCustomerGroupDetailsListById(KendoGridResult inputDto)
        //{
        //    _methodName = "GetCustomerGroupDetailsListById";
        //    var resultDto = new ResultDto();
        //    var outputDto = new List<CustomerGroupDetailDto>();
        //    try
        //    {
        //        var resultContext = _emamiContext.CustomerGroupDetails.AsNoTracking().Where(_ => _.CustomerGroupId == customerGroupId);

        //        if (resultContext != null && resultContext.Any())
        //        {
        //            outputDto = resultContext.ToList()
        //            .Select(_ => new CustomerGroupDetailDto
        //            {
        //                CustomerGroupId = _.CustomerGroupId,
        //                CustomerGroupName = _.CustomerGroup != null ? _.CustomerGroup.Name : string.Empty,
        //                CustomerId = _.CustomerId,
        //                CustomerName = _.Customer != null ? _.Customer.Name : string.Empty,
        //                Code = _.Customer.Code,
        //                MobileNumber = _.Customer.MobileNumber,
        //                Email = _.Customer.Email,
        //                State = _.Customer.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.StateId)?.StateName : string.Empty,
        //                District = _.Customer.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.DistrictId)?.DistrictName : string.Empty,
        //                Territory = _.Customer.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.TerritoryId)?.Name : string.Empty,
        //                Zone = _.Customer.ZoneId > 0 ? _emamiContext.Zones.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.ZoneId)?.Name : string.Empty,
        //                City = _.Customer.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.CityId)?.CityName : string.Empty,
        //            })
        //            .ToList();
        //        }
        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = outputDto;
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

        /// <summary>
        /// Method to Get Customer List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetCustomerList(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetCustomerList";
            var resultDto = new ResultDto();
            var result = new DataSourceResult();
            try
            {
                //var addedCustomerIds = _emamiContext.CustomerGroupDetails.Where(_ => _.CustomerGroup.IsActive).Select(_ => _.CustomerId);

                //var userList = _emamiContext.Users.Where(_ => !addedCustomerIds.Contains(_.Id) && _.IsActive && _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                //    .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                //    .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer) && w.Users.VerticalId == inputDto.VerticalId);

                //if (inputDto.ZoneIds != null && inputDto.ZoneIds.Any())
                //{
                //    userList = userList.Where(_ => inputDto.ZoneIds.Contains(_.Users.ZoneId ?? 0));
                //}
                //if (inputDto.StateIds != null && inputDto.StateIds.Any())
                //{
                //    userList = userList.Where(_ => inputDto.StateIds.Contains(_.Users.StateId));
                //}
                //if (inputDto.TerritoryIds != null && inputDto.TerritoryIds.Any())
                //{
                //    userList = userList.Where(_ => inputDto.TerritoryIds.Contains(_.Users.TerritoryId));
                //}
                //if (inputDto.DistrictIds != null && inputDto.DistrictIds.Any())
                //{
                //    userList = userList.Where(_ => inputDto.DistrictIds.Contains(_.Users.DistrictId));
                //}
                //if (inputDto.CityIds != null && inputDto.CityIds.Any())
                //{
                //    userList = userList.Where(_ => inputDto.CityIds.Contains(_.Users.CityId));
                //}

                //var outputDto = userList.ToList().Select(_ => new CustomerGroupDetailDto()
                //{
                //    CustomerId = _.Users.Id,
                //    Code = _.Users.Code,
                //    CustomerName = _.Users.Name,
                //    MobileNumber = _.Users.MobileNumber,
                //    Email = _.Users.Email,
                //    State = _.Users.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.StateId)?.StateName : string.Empty,
                //    District = _.Users.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.DistrictId)?.DistrictName : string.Empty,
                //    Territory = _.Users.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.TerritoryId)?.Name : string.Empty,
                //    Zone = _.Users.ZoneId > 0 ? _emamiContext.Zones.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.ZoneId)?.Name : string.Empty,
                //    City = _.Users.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.CityId)?.CityName : string.Empty,
                //    RoleName = _.UserRoles.Role.Name,
                //    Vertical = _.Users?.Vertical?.Name,
                //    SaudaBookingTypeId = _.Users?.SaudaBookingTypeId,
                //    SaudaBookingType = _.Users?.SaudaBookingType?.Name
                //}).ToDataSourceResult(inputDto.DataSourceRequest);

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"Select u.Id as CustomerId,u.Name as CustomerName,u.Code,u.MobileNumber,u.Email,c.CityName as City,d.DistrictName as District,sb.Id as SaudaBookingTypeId,
                        s.StateName as State,t.Name as Territory,z.Name as Zone,r.Name as RoleName,v.Name as Vertical,sb.Name as SaudaBookingType
                        From Users u JOIN Zones z ON u.ZoneId = z.Id
                        JOIN States s ON u.StateId = s.Id
                        JOIN Territories t ON u.TerritoryId = t.Id
                        JOIN Districts d ON u.DistrictId = d.Id
                        JOIN Cities c ON u.CityId = c.Id
                        JOIN Verticals v ON u.VerticalId = v.Id
                        JOIN SaudaBookingTypes sb ON u.SaudaBookingTypeId = sb.Id
                        JOIN UserRoles ur ON u.Id = ur.UserId
                        JOIN Roles r ON ur.RoleId = r.Id
                        Where r.Id = @RoleId
                        And sb.Id = @SaudaBookingTypeId 
                        And u.IsActive = @IsActive
                        And u.VerticalId = @VerticalId      
                        AND u.Id NOT IN (Select DISTINCT cgd.CustomerId From CustomerGroups cg 
                        JOIN CustomerGroupDetails cgd ON cg.Id = cgd.CustomerGroupId
                        Where cg.IsActive = 1)");

                    if (inputDto.ZoneIds.IsAny())
                    {
                        sb.Append(" AND z.Id IN @ZoneIds ");
                    }
                    if (inputDto.StateIds.IsAny())
                    {
                        sb.Append(" AND s.Id IN @StateIds ");
                    }
                    if (inputDto.TerritoryIds.IsAny())
                    {
                        sb.Append(" AND t.Id IN @TerritoryIds ");
                    }
                    if (inputDto.DistrictIds.IsAny())
                    {
                        sb.Append(" AND d.Id IN @DistrictIds ");
                    }
                    if (inputDto.CityIds.IsAny())
                    {
                        sb.Append(" AND c.Id IN @CityIds ");
                    }

                    result = conn.Query<CustomerGroupDetailDto>(sb.ToString(), new
                    {
                        IsActive = 1,
                        VerticalId = inputDto.VerticalId,
                        RoleId = (int)DTO.Enums.RoleType.Dealer,
                        //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
                        ZoneIds = inputDto.ZoneIds,
                        StateIds = inputDto.StateIds,
                        TerritoryIds = inputDto.TerritoryIds,
                        DistrictIds = inputDto.DistrictIds,
                        CityIds = inputDto.CityIds
                    }).ToDataSourceResult(inputDto.DataSourceRequest);
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

        /// <summary>
        /// Method to Get Mapped Customer List By Customer Group Id
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetMappedCustomerListByCustomerGroupId(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetMappedCustomerListByCustomerGroupId";
            var resultDto = new ResultDto();
            DataSourceResult result = new DataSourceResult();
            try
            {
                var mappedCustomers = _emamiContext.CustomerGroupDetails.AsNoTracking()
                    .Where(_ => _.CustomerGroupId == inputDto.CustomerGroupId);

                //var userList = _emamiContext.Users.Join(mappedCustomers, u => u.Id, mc => mc.CustomerId, (u, mc) => new { Users = u, MappedCustomers = mc })
                //    .Join(_emamiContext.UserRoles.AsNoTracking()
                //    .Where(w => w.RoleId == (int)DTO.Enums.Role.Dealer), u => u.Users.Id, ur => ur.UserId, (u, ur) => new { u.Users, UserRoles = ur, u.MappedCustomers });

                if (mappedCustomers != null)
                {
                    result = mappedCustomers.ToList().Select(_ => new CustomerGroupDetailDto()
                    {
                        CustomerGroupDetailId = _.Id,
                        CustomerGroupId = _.CustomerGroupId,
                        CustomerId = _.CustomerId,
                        Code = _.Customer.Code,
                        CustomerName = _.Customer?.Name,
                        MobileNumber = _.Customer?.MobileNumber,
                        Email = _.Customer?.Email,
                        State = _.Customer.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.StateId)?.StateName : string.Empty,
                        District = _.Customer.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.DistrictId)?.DistrictName : string.Empty,
                        Territory = _.Customer.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.TerritoryId)?.Name : string.Empty,
                       // Zone = _.Customer?.Zone?.Name,
                        City = _.Customer.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.Customer.CityId)?.CityName : string.Empty,
                        //RoleName = _.Customer?.Role?.Name,
                        //Vertical = _.Customer?.Division?.Name,
                       // SaudaBookingType = _.Customer?.SaudaBookingType?.Name
                    }).ToDataSourceResult(inputDto.DataSourceRequest);
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

        /// <summary>
        /// Method to Remove Customers From Customer Group
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto RemoveCustomersFromCustomerGroup(CustomerDeleteInputDto inputDto)
        {
            _methodName = "RemoveCustomersFromCustomerGroup";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                //var customerGroupDetailExist = _emamiContext.CustomerGroupDetails.Where(f => f.CustomerGroupId == inputDto.CustomerGroupId
                //&& inputDto.CustomerIds.Contains(f.CustomerId));

                var customerGroupDetailExist = _emamiContext.CustomerGroupDetails.Where(f => inputDto.CustomerGroupDetailIds.Contains(f.Id));

                if (customerGroupDetailExist != null && customerGroupDetailExist.Any())
                {
                    foreach (var recordDelete in customerGroupDetailExist)
                    {
                        _emamiContext.CustomerGroupDetails.Remove(recordDelete);
                    }
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

        /// <summary>
        /// Method to Get Customer Group List By Vertical For Dropdown
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetCustomerGroupListByVerticalForDropdown(LoginUserIdDto inputDto)
        {
            _methodName = "GetCustomerGroupListByVerticalForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var customergroupList = _emamiContext.CustomerGroups.AsNoTracking()
                    .Where(_ => _.DivisionId == inputDto.VerticalId && _.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

                resultDto.SuccessDto.Response = customergroupList;
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
        /// Method to Get Customer List By Customer Group Id And StateTrader For Dropdown
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetCustomerListByCustomerGroupIdAndBDOForDropdown(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetCustomerListByCustomerGroupIdForDropdown";
            var resultDto = new ResultDto();
            var outputDto = new List<DropDownDto>();
            try
            {
                if (inputDto.CustomerGroupId > 0 && inputDto.BDOId > 0)
                {
                    var mappedCustomers = _emamiContext.CustomerGroupDetails.AsNoTracking()
                        .Where(_ => _.CustomerGroupId == inputDto.CustomerGroupId && _.CustomerGroup.IsActive).Select(_ => _.Customer);

                    var customerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(w => w.UserId == inputDto.BDOId)
                        .Select(s => s.CustomerId).ToList();

                    //Filter By StateTrader
                    outputDto = mappedCustomers.Where(_ => customerIds.Contains(_.Id) && _.IsActive)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }
                else if (inputDto.CustomerGroupId > 0)
                {
                    outputDto = _emamiContext.CustomerGroupDetails.AsNoTracking()
                        .Where(_ => _.CustomerGroupId == inputDto.CustomerGroupId && _.CustomerGroup.IsActive).Select(_ => _.Customer)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }
                else if (inputDto.BDOId > 0)
                {
                    var customerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(w => w.UserId == inputDto.BDOId)
                        .Select(s => s.CustomerId).ToList();

                    outputDto = _emamiContext.Users.AsNoTracking().Where(_ => customerIds.Contains(_.Id) && _.IsActive)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }

                resultDto.SuccessDto.Response = outputDto;
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

        public ResultDto GetBiddingWindowCustomerGroupListForddl(IdInputDto inputDto)
        {
            _methodName = "GetBiddingWindowCustomerGroupListForddl";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }


                var biddingWindowList = (from bdcg in _emamiContext.BiddingWindowCustomerGroups.AsNoTracking()
                                         join cg in _emamiContext.CustomerGroups.AsNoTracking() on bdcg.CustomerGroupId equals cg.Id
                                         where bdcg.BiddingWindowId == inputDto.Id && cg.IsActive
                                         select new DropDownDto() { Id = cg.Id, Name = cg.Name }).ToList();

                //var biddingWindowList = _emamiContext.BiddingWindowCustomerGroups.AsNoTracking()
                //    .Where(w => w.BiddingWindowId == inputDto.Id)
                //    .Select(s => new DropDownDto()
                //    {
                //        Id = s.CustomerGroup.Id,
                //        Name = s.CustomerGroup.Name
                //    }).ToList();

                return _resultService.SuccessMessageWitObject(biddingWindowList, "");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetCustomerListBasedOnCustomerGroup(RANotificationInputDto inputDto)
        {
            _methodName = "GetCustomerListBasedOnCustomerGroup";
            var resultDto = new ResultDto();
            try
            {
                var addedCustomerIds = _emamiContext.CustomerGroupDetails.AsNoTracking().Where(w => inputDto.CustomerGroupIds.Contains(w.CustomerGroupId)).Select(_ => _.CustomerId);
                if (addedCustomerIds.IsAny())
                {
                    var userList = _emamiContext.Users.Where(_ => addedCustomerIds.Contains(_.Id) && _.IsActive
                    //&& _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                    )
                        .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer));

                    if (userList.IsAny())
                    {
                        var customerGroupDetails = _emamiContext.CustomerGroupDetails.AsNoTracking().Where(w => inputDto.CustomerGroupIds.Contains(w.CustomerGroupId));

                        var outputDto = userList.ToList().Select(_ => new RaNotificationDetailDto()
                        {
                            CustomerId = _.Users.Id,
                            Code = _.Users.Code,
                            CustomerName = _.Users.Name,
                            MobileNumber = _.Users.MobileNumber,
                            Email = _.Users.Email,
                            State = _.Users.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.StateId)?.StateName : string.Empty,
                            District = _.Users.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.DistrictId)?.DistrictName : string.Empty,
                            Territory = _.Users.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.TerritoryId)?.Name : string.Empty,
                            Zone = _.Users.ZoneId > 0 ? _emamiContext.Zones.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.ZoneId)?.Name : string.Empty,
                            City = _.Users.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.Users.CityId)?.CityName : string.Empty,
                            RoleName = _.UserRoles.Role.Name,
                            //Vertical = _.Users?.Division?.Name,
                            CustomerGroupId = customerGroupDetails.AsNoTracking().FirstOrDefault(f => f.CustomerId == _.Users.Id)?.CustomerGroup?.Id ?? 0,
                            CustomerGroup = customerGroupDetails.AsNoTracking().FirstOrDefault(f => f.CustomerId == _.Users.Id)?.CustomerGroup?.Name,
                            SaudaBookingTypeId = _.Users?.SaudaBookingTypeId,
                           // SaudaBookingType = _.Users?.SaudaBookingType?.Name
                        }).ToDataSourceResult(inputDto.DataSourceRequest);

                        resultDto.SuccessDto.Response = outputDto;
                        resultDto.IsSuccess = true;
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

        #region SchemeDiscount - GeographyBased

        /// <summary>
        /// Method to Get Geography Based SchemeDiscount List With Pagination
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetGeographyBasedSchemeDiscountListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetGeographyBasedSchemeDiscountListWithPagination";
            var resultDto = new ResultDto();
            DataSourceResult result = new DataSourceResult();
            try
            {

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    string query = @"Select Distinct sd.Id,sd.Name,d.Name as Division,so.Name as SalesOrganization,dc.Name as DistributionChannel,sd.TargetQuantity,sd.ValidFrom,sd.ValidTo
                        ,(Select TOP 1 IsActive FROM SchemeDiscountGeographyMappings WHERE SchemeDiscountGeographyId = sd.Id ORDER BY IsActive DESC) as IsActive
                        From SchemeDiscountGeographies sd
                        Left Join SchemeDiscountGeographyMappings sdm on sd.Id = sdm.SchemeDiscountGeographyId
                        Left Join Skus sk on sk.Id = sdm.SkuId
                        Left Join Divisions d on d.Id = sk.DivisionId
                        Left Join SalesOrganizations so on sk.SalesOrganizationId=so.Id
                        Left Join DistributionChannels dc on sk.DistributionChannelId=dc.Id
";

                    result = conn.Query<SchemeDiscountGeographyDto>(query).ToDataSourceResult(inputDto.DataSourceRequest);
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

        /// <summary>
        /// Method to Get SchemeDiscount Geography Details By Id
        /// </summary>
        /// <param name="schemeDiscountId"></param>
        /// <returns></returns>
        public ResultDto GetSchemeDiscountGeographyHierarchyListById(KendoGridResult inputDto)
        {
            _methodName = "GetSchemeDiscountGeographyHierarchyListById";
            var resultDto = new ResultDto();
            DataSourceResult result = new DataSourceResult();
            try
            {
                result = _emamiContext.SchemeDiscountGeographyMappings.AsNoTracking()
                    .Where(_ => _.SchemeDiscountGeographyId == inputDto.Id)
                    .Select(_ => new SchemeDiscountGeographyMappingDto
                    {
                        SchemeDiscountGeographyMappingId = _.Id,
                        SkuName = _.Sku != null ? _.Sku.SkuName : string.Empty,
                        SkuCode = _.Sku != null ? _.Sku.SkuCode : string.Empty,
                        OilTypeName = _.Sku != null ? _.Sku.OilType.Name+"-"+ _.Sku.OilType.SalesOrganization.Code+"/"+ _.Sku.OilType.DistributionChannel.Code+"/"+ _.Sku.OilType.Division.Code : string.Empty,
                        //OilTypeCode = _.Sku != null ? _.Sku.OilType.SAPCode : string.Empty,
                        PackGroup = _.Sku != null ? _.Sku.PackGroup.Name : string.Empty,
                        UserName = _.Customer != null ? _.Customer.Name : string.Empty,
                        UserCode = _.Customer != null ? _.Customer.Code : string.Empty,

                        StateName = _.City.District.State != null ? _.City.District.State.StateName : string.Empty,
                        DistrictName = _.City.District != null ? _.City.District.DistrictName : string.Empty,
                        CityName = _.City != null ? _.City.CityName : string.Empty,
                        IsActive = _.IsActive
                    }).ToDataSourceResult(inputDto.DataSourceRequest);

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

        /// <summary>
        /// Method to Save Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto SaveSchemeDiscountGeography(SchemeDiscountGeographyDto inputDto)
        {
            _methodName = "SaveGeographyBasedSchemeDiscount";
            var resultDto = new ResultDto();
            var userDetailList = new List<SchemeDiscountGeographyMapping>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.ValidTo < inputDto.ValidFrom)
                {
                    return _resultService.ErrorMessage(Constants.ToDateInvalid);
                }

                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }

                var customerIds = new List<long>();
                if (inputDto.CustomerIds.IsNotAny())
                {
                    customerIds = GetRaCustomerDetailsBasedOnGeographies(inputDto.CustomerGroupIds, inputDto.CityIds, inputDto.DistrictIds, inputDto.TerritoryIds, inputDto.DivisionId, inputDto.SalesOrganizationId, inputDto.DistributionChannelId);
                }
                else
                {
                    customerIds = inputDto.CustomerIds.ToList();
                }

                var customerCityDetails = _emamiContext.Users.AsNoTracking().Where(w => customerIds.Contains(w.Id))
                    .Select(s => new
                    {
                        Id = s.Id,
                        CityId = s.CityId
                    }).ToList();

                var cityIds = customerCityDetails.Select(s => s.CityId).ToList();

                #region Exist Validation
                var checkIsExists = _emamiContext.SchemeDiscountGeography.AsNoTracking()
                           .Where(_ => DbFunctions.TruncateTime(inputDto.ValidFrom) <= DbFunctions.TruncateTime(_.ValidTo) && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                           .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking()
                           .Where(w => w.IsActive
                           && inputDto.SkuIds.Contains(w.SkuId)
                           && cityIds.Contains(w.CityId)
                           && customerIds.Contains(w.CustomerId))
                           , sdu => sdu.Id, sdum => sdum.SchemeDiscountGeographyId, (sdu, sdum) => new { sdu, sdum })
                           .Select(_ => _.sdum.Id).ToList();

                if (checkIsExists != null && checkIsExists.Any())
                {
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        string query = @"Update SchemeDiscountGeographyMappings Set IsActive = @IsActive, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate Where Id IN @Ids";

                        var result = conn.Execute(query, new
                        {
                            IsActive = false,
                            ModifiedBy = inputDto.LoginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            Ids = checkIsExists
                        });
                    }
                }
                #endregion


                if (customerCityDetails.IsAny())
                {

                    var inputEntity = new SchemeDiscountGeography();
                    inputEntity.Name = inputDto.Name;
                    inputEntity.ValidFrom = inputDto.ValidFrom;
                    inputEntity.ValidTo = inputDto.ValidTo;
                    inputEntity.Discount = inputDto.Discount;
                    inputEntity.TargetQuantity = inputDto.TargetQuantity;
                    inputEntity.DiscountReason = inputDto.DiscountReason;
                    inputEntity.CreatedBy = inputDto.LoginUserId;
                    inputEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                    _emamiContext.SchemeDiscountGeography.Add(inputEntity);
                    _emamiContext.SaveChanges();

                    foreach (var sku in inputDto.SkuIds)
                    {
                        foreach (var customer in customerIds)
                        {
                            var customerGroupId = 0;

                            var userDetail = new SchemeDiscountGeographyMapping();
                            userDetail.SchemeDiscountGeographyId = inputEntity.Id;
                            userDetail.SkuId = sku;
                            userDetail.CustomerId = customer;
                            userDetail.CustomerGroupId = customerGroupId;
                            userDetail.CityId = customerCityDetails.FirstOrDefault(f => f.Id == customer).CityId;
                            userDetail.IsActive = true;
                            userDetail.CreatedBy = inputDto.LoginUserId;
                            userDetail.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);


                            userDetailList.Add(userDetail);

                        }
                    }
                    if (userDetailList.Count <= ConsoleSettings.BulkInsertRecordCount)
                    {
                        _emamiContext.BulkInsertProxy(userDetailList);
                        _emamiContext.SaveChanges();
                        userDetailList = new List<SchemeDiscountGeographyMapping>();
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.CustomerRecordNotFound);
                }

                if (userDetailList.IsAny())
                {
                    _emamiContext.BulkInsertProxy(userDetailList);
                    _emamiContext.SaveChanges();
                    userDetailList = new List<SchemeDiscountGeographyMapping>();
                }

                resultDto.IsSuccess = true;
                return resultDto;
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
        public List<long> GetRaCustomerDetailsBasedOnGeographies(List<long> customerGroupIds, List<int> cityIds, List<int> districeIds, List<int> territoryIds, long DivisionId, long SalesOrganizationId, long DistributionChannelId)
        {
            var result = new List<long>();
            string query = string.Empty;
            try
            {
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    if (cityIds.IsNotAny())
                    {
                        if (districeIds.IsNotAny())
                        {
                            if (territoryIds.IsAny())
                            {
                                query = @"select  u.Id,u.Name, ur.RoleId  From Users u Join UserRoles ur on u.Id = ur.UserId
                            join UserDivisionMappings ud on u.Id=ud.UserId
                            Where u.IsActive = @IsActive  and ur.RoleId = @RoleID and ud.SalesOrganizationId=@SalesOrganizationId and ud.DistributionChannelId=@DistributionChannelId and ud.DivisionId=@DivisionId
                            Order By u.Name";
                                //                                query = @"select  u.Id,u.Name, ur.RoleId  From Users u Join UserRoles ur on u.Id = ur.UserId
                                //Where u.IsActive = @IsActive AND u.TerritoryId in @TerritoryId AND u.VerticalId = @VerticalId and ur.RoleId = @RoleID
                                //Order By u.Name";
                                result = conn.Query<long>(query, new
                                {
                                    RoleId = (int)DTO.Enums.Role.Dealer,
                                    IsActive = 1,
                                    DivisionId = DivisionId,
                                    SalesOrganizationId=SalesOrganizationId,
                                    DistributionChannelId=DistributionChannelId
                                }).ToList();
                            }
                        }
                        else
                        {
                            query = @"select  u.Id,u.Name, ur.RoleId  From Users u Join UserRoles ur on u.Id = ur.UserId
join UserDivisionMappings ud on u.Id=ud.UserId
Where u.IsActive = @IsActive AND u.DistrictId in @DistrictId  and ur.RoleId = @RoleID and ud.SalesOrganizationId=@SalesOrganizationId and ud.DistributionChannelId=@DistributionChannelId and ud.DivisionId=@DivisionId
Order By u.Name";
                            result = conn.Query<long>(query, new
                            {
                                DistrictId = districeIds,
                                RoleId = (int)DTO.Enums.Role.Dealer,
                                IsActive = 1,
                                DivisionId = DivisionId,
                                SalesOrganizationId = SalesOrganizationId,
                                DistributionChannelId = DistributionChannelId
                            }).ToList();
                        }
                    }
                    else
                    {
                        query = @"select  u.Id,u.Name, ur.RoleId  From Users u Join UserRoles ur on u.Id = ur.UserId
join UserDivisionMappings ud on u.Id=ud.UserId
Where u.IsActive = @IsActive AND u.CityId in @CityIds  and ur.RoleId = @RoleID and ud.SalesOrganizationId=@SalesOrganizationId and ud.DistributionChannelId=@DistributionChannelId and ud.DivisionId=@DivisionId
Order By u.Name";

                        result = conn.Query<long>(query, new
                        {
                            CityIds = cityIds,
                            RoleId = (int)DTO.Enums.Role.Dealer,
                            IsActive = 1,
                            DivisionId = DivisionId,
                            SalesOrganizationId = SalesOrganizationId,
                            DistributionChannelId = DistributionChannelId
                        }).ToList();
                    }

                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public ResultDto SaveSchemeDiscountGeographyOld(SchemeDiscountGeographyDto inputDto)
        {
            _methodName = "SaveGeographyBasedSchemeDiscount";
            var resultDto = new ResultDto();
            var userDetailList = new List<SchemeDiscountGeographyMapping>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.ValidTo < inputDto.ValidFrom)
                {
                    return _resultService.ErrorMessage(Constants.ToDateInvalid);
                }

                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }

                #region Exist Validation
                var checkIsExists = _emamiContext.SchemeDiscountGeography
                           .Where(_ => DbFunctions.TruncateTime(inputDto.ValidFrom) <= DbFunctions.TruncateTime(_.ValidTo) && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(inputDto.ValidTo))
                           .Join(_emamiContext.SchemeDiscountGeographyMappings
                           .Where(w => w.IsActive
                           && inputDto.SkuIds.Contains(w.SkuId)
                           && inputDto.CityIds.Contains(w.CityId)
                           && inputDto.CustomerIds.Contains(w.CustomerId))
                           , sdu => sdu.Id, sdum => sdum.SchemeDiscountGeographyId, (sdu, sdum) => new { sdu, sdum }).Select(_ => _.sdum).ToList();

                if (checkIsExists != null && checkIsExists.Any())
                {
                    foreach (var item in checkIsExists)
                    {
                        item.IsActive = false;
                        item.ModifiedBy = inputDto.LoginUserId;
                        item.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }
                #endregion

                var inputEntity = new SchemeDiscountGeography();
                inputEntity.Name = inputDto.Name;
                inputEntity.ValidFrom = inputDto.ValidFrom;
                inputEntity.ValidTo = inputDto.ValidTo;
                inputEntity.Discount = inputDto.Discount;
                inputEntity.CreatedBy = inputDto.LoginUserId;
                inputEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SchemeDiscountGeography.Add(inputEntity);
                _emamiContext.SaveChanges();

                var customerGroupDetails = _emamiContext.CustomerGroupDetails.AsNoTracking()
                    .Where(f => inputDto.CustomerIds.Contains(f.CustomerId)).ToList();

                foreach (var sku in inputDto.SkuIds)
                {
                    foreach (var cityid in inputDto.CityIds)
                    {
                        foreach (var customer in inputDto.CustomerIds)
                        {
                            var customerGroupId = customerGroupDetails.IsAny() ? customerGroupDetails.FirstOrDefault(_ => _.CustomerId == customer).CustomerGroupId : 0;

                            var userDetail = new SchemeDiscountGeographyMapping();
                            userDetail.SchemeDiscountGeographyId = inputEntity.Id;
                            userDetail.SkuId = sku;
                            userDetail.CustomerId = customer;
                            userDetail.CustomerGroupId = customerGroupId;
                            userDetail.CityId = cityid;
                            userDetail.IsActive = true;
                            userDetail.CreatedBy = inputDto.LoginUserId;
                            userDetail.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            //_emamiContext.SchemeDiscountGeographyMappings.Add(userDetail);
                            //_emamiContext.SaveChanges();

                            userDetailList.Add(userDetail);
                            if (userDetailList.Count == ConsoleSettings.BulkInsertRecordCount)
                            {
                                _emamiContext.BulkInsertProxy(userDetailList);
                                _emamiContext.SaveChanges();
                                userDetailList = new List<SchemeDiscountGeographyMapping>();
                            }
                        }
                    }
                }

                if (userDetailList.IsAny())
                {
                    _emamiContext.BulkInsertProxy(userDetailList);
                    _emamiContext.SaveChanges();
                    userDetailList = new List<SchemeDiscountGeographyMapping>();
                }

                resultDto.IsSuccess = true;
                return resultDto;
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
        /// Method to Update Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateSchemeDiscountGeography(SchemeDiscountGeographyDto inputDto)
        {
            _methodName = "UpdateSchemeDiscountGeography";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (string.IsNullOrEmpty(inputDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }

                var inputEntity = _emamiContext.SchemeDiscountGeography.FirstOrDefault(_ => _.Id == inputDto.Id);
                inputEntity.Name = inputDto.Name;
                inputEntity.Discount = inputDto.Discount;
                inputEntity.ModifiedBy = inputDto.LoginUserId;
                inputEntity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                var discountMappingList = _emamiContext.SchemeDiscountGeographyMappings.Where(f => f.SchemeDiscountGeographyId == inputDto.Id && f.IsActive).ToList();

                #region SKU - Check Is Removed/Deselected

                var existingSkuIds = discountMappingList.Select(_ => _.SkuId).Distinct().ToList();
                var removedSkuIds = existingSkuIds.Except(inputDto.SkuIds).ToList();
                var newSkuIdsFromInput = inputDto.SkuIds.Except(existingSkuIds).ToList();

                var skuToRemoveExist = discountMappingList.Where(_ => removedSkuIds.Contains(_.SkuId));
                if (skuToRemoveExist.IsAny())
                {
                    foreach (var item in skuToRemoveExist)
                    {
                        item.IsActive = false;
                    }
                    _emamiContext.SaveChanges();
                }

                #endregion

                #region Customer - Check Is Removed/Deselected

                var existingCustomerIds = discountMappingList.Select(_ => _.CustomerId).Distinct().ToList();
                var newlyAddedCustomerIds = inputDto.CustomerIds.Except(existingCustomerIds).ToList();
                var removedcustomerIds = existingCustomerIds.Except(inputDto.CustomerIds).ToList();

                var customerToRemoveExist = discountMappingList.Where(_ => removedcustomerIds.Contains(_.CustomerId));
                if (customerToRemoveExist.IsAny())
                {
                    foreach (var item in customerToRemoveExist)
                    {
                        item.IsActive = false;
                    }
                    _emamiContext.SaveChanges();
                }

                #endregion

                #region City - Check Is Removed/Deselected

                var existingCityIds = discountMappingList.Select(_ => _.CityId).Distinct().ToList();
                var newlyAddedCityIds = inputDto.CityIds.Except(existingCityIds).ToList();
                var removedCityIds = existingCityIds.Except(inputDto.CityIds).ToList();

                var cityToRemoveExist = discountMappingList.Where(_ => removedCityIds.Contains(_.CityId));
                if (cityToRemoveExist.IsAny())
                {
                    foreach (var item in cityToRemoveExist)
                    {
                        item.IsActive = false;
                    }
                    _emamiContext.SaveChanges();
                }

                #endregion

                #region Save - New Sku & Customer & City

                var customerGroupDetails = _emamiContext.CustomerGroupDetails.AsNoTracking()
                    .Where(f => inputDto.CustomerIds.Contains(f.CustomerId)).ToList();

                foreach (var skuId in inputDto.SkuIds)
                {
                    foreach (var cityid in inputDto.CityIds)
                    {
                        foreach (var customerId in inputDto.CustomerIds)
                        {
                            var exists = discountMappingList.Any(f => f.CustomerId == customerId && f.SkuId == skuId && f.CityId == cityid);
                            if (!exists)
                            {
                                var customerGroupId = customerGroupDetails.IsAny() ? customerGroupDetails.FirstOrDefault(_ => _.CustomerId == customerId).CustomerGroupId : 0;

                                var userDetail = new SchemeDiscountGeographyMapping();
                                userDetail.SchemeDiscountGeographyId = inputEntity.Id;
                                userDetail.SkuId = skuId;
                                userDetail.CustomerId = customerId;
                                userDetail.CustomerGroupId = customerGroupId;
                                userDetail.CityId = cityid;
                                userDetail.IsActive = true;
                                userDetail.CreatedBy = inputDto.LoginUserId;
                                userDetail.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SchemeDiscountGeographyMappings.Add(userDetail);
                                _emamiContext.SaveChanges();
                            }
                        }
                    }
                }
                _emamiContext.SaveChanges();

                #endregion

                resultDto.IsSuccess = true;
                return resultDto;
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
        /// Method to Get Geography Based SchemeDiscount By Id
        /// </summary>
        /// <param name="schemeDiscountId"></param>
        /// <returns></returns>
        public ResultDto GetGeographyBasedSchemeDiscountById(long schemeDiscountId)
        {
            _methodName = "GetGeographyBasedSchemeDiscountById";
            var resultDto = new ResultDto();
            var result = new SchemeDiscountGeographyDto();
            try
            {
                var resultContext = _emamiContext.SchemeDiscountGeography.AsNoTracking().FirstOrDefault(f => f.Id == schemeDiscountId);
                var geographyMappings = _emamiContext.SchemeDiscountGeographyMappings.Where(_ => _.SchemeDiscountGeographyId == schemeDiscountId && _.IsActive).ToList();

                if (resultContext != null && geographyMappings.IsAny())
                {
                    result.Id = resultContext.Id;
                    result.Name = resultContext.Name;
                    result.OilTypeIds = geographyMappings.Select(_ => (long)_.Sku.OilTypeId).Distinct().ToList();
                    result.CustomerGroupIds = geographyMappings.Select(_ => _.CustomerGroupId).Distinct().ToList();
                    result.DivisionId = geographyMappings.Select(_ => _.Sku.DivisionId).FirstOrDefault();

                    result.CustomerIds = geographyMappings.Select(_ => _.CustomerId).Distinct().ToList();

                    result.StateIds = geographyMappings.Select(_ => _.City.District.StateId).Distinct().ToList();
                    result.ZoneIds = _emamiContext.ZoneStateMappings.Where(_ => result.StateIds.Contains(_.StateId)).Select(_ => _.ZoneId).Distinct().ToList();
                    //result.TerritoryIds = geographyMappings.Select(_ => _.City.TerritoryId).Distinct().ToList();
                    result.DistrictIds = geographyMappings.Select(_ => _.City.DistrictId).Distinct().ToList();
                    result.CityIds = geographyMappings.Select(_ => _.CityId).Distinct().ToList();

                    result.OilPackingTypeIds = geographyMappings.Select(_ => (long)_.Sku.PackGroupId).Distinct().ToList();
                    result.Discount = resultContext.Discount;
                    result.ValidFrom = resultContext.ValidFrom;
                    result.ValidTo = resultContext.ValidTo;
                    result.SkuIds = geographyMappings.Select(s => s.SkuId).Distinct().ToList();
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
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

        /// <summary>
        /// Method to Export Geography Based SchemeDiscount
        /// </summary>
        /// <param name="loginUserIdDto"></param>
        /// <returns></returns>
        public ResultDto ExportGeographyBasedSchemeDiscount(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportGeographyBasedSchemeDiscount";
            var resultDto = new ResultDto();
            try
            {
                var outputDto = new List<SchemeDiscountGeographyDto>();
                var resultContext = _emamiContext.SchemeDiscountGeography.AsNoTracking();
                outputDto = resultContext.ToList().Select(s => new SchemeDiscountGeographyDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                    schemeDiscountGeographyDetailsList = _emamiContext.SchemeDiscountGeographyMappings.AsNoTracking()
                    .Where(_ => _.SchemeDiscountGeographyId == s.Id && _.IsActive).ToList()
                    .Select(_ => new SchemeDiscountGeographyMappingDto
                    {
                        SchemeDiscountGeographyMappingId = _.Id,
                        SchemeDiscountId = _.SchemeDiscountGeographyId,
                        OilTypeName = _.Sku.OilType?.Name,
                        SkuId = _.SkuId,
                        SkuName = _.Sku != null ? _.Sku.SkuName : string.Empty,
                        SkuCode = _.Sku != null ? _.Sku.SkuCode : string.Empty,
                        StateName = _.City.District.State != null ? _.City.District.State.StateName : string.Empty,
                        DistrictName = _.City.District != null ? _.City.District.DistrictName : string.Empty,
                        CityName = _.City != null ? _.City.CityName : string.Empty,
                        IsActive = _.IsActive,
                    }).ToList()
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


        public ResultDto UpdateSchemeDiscountGeographyListByIsActive(IdDiscountAndBenefitInputDto inputDto)
        {
            _methodName = "UpdateSchemeDiscountGeographyListByIsActive";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    string query = @"Update SchemeDiscountGeographyMappings Set IsActive = @IsActive, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate Where SchemeDiscountGeographyId = @SchemeDiscountGeographyId";

                    var result = conn.Execute(query, new
                    {
                        IsActive = false,
                        ModifiedBy = inputDto.LoginUserId,
                        ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        SchemeDiscountGeographyId = inputDto.Id
                    });
                }
                return _resultService.SuccessMessage(Constants.SuccessMessage);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        #region Reporting To Users - Customer Group

        public ResultDto GetRAZonalHeadUsersByCustomerGroup(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetReportingToRAZonalHeadUsersByCustomerGroup";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                var mappedCustomerIds = _emamiContext.CustomerGroupDetails.AsNoTracking()
                       .Where(_ => _.CustomerGroupId == inputDto.CustomerGroupId).Select(_ => _.CustomerId).ToList();

                var customerBDOs = _emamiContext.UserCustomerMapping.AsNoTracking()
                    .Where(w => mappedCustomerIds.Contains(w.CustomerId)).ToList();

                var customerReportingBDOIds = customerBDOs.Select(s => s.UserId).ToList();

                var bdoReportingZonalHeadIds = customerBDOs.Select(s => s.User?.ReportingToId).ToList();

                reportingToUsers = _emamiContext.Users
                    .Where(_ => bdoReportingZonalHeadIds.Contains(_.Id) && _.IsActive
                    //&& _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                    )
                    .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                    //&& w.Users.DivisionId == inputDto.VerticalId
                    ).Select(_ => _.Users)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
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

        public ResultDto GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds(DropDownInputDto inputDto)
        {
            _methodName = "GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                var mappedCustomerIds = _emamiContext.CustomerGroupDetails.AsNoTracking()
                       .Where(_ => inputDto.CustomerGroupIds.Contains(_.CustomerGroupId)).Select(_ => _.CustomerId).ToList();

                var customerBDOs = _emamiContext.UserCustomerMapping.AsNoTracking()
                    .Where(w => mappedCustomerIds.Contains(w.CustomerId)).ToList();

                var customerReportingBDOIds = customerBDOs.Select(s => s.UserId).ToList();

                var bdoReportingZonalHeadIds = customerBDOs.Select(s => s.User?.ReportingToId).ToList();

                reportingToUsers = _emamiContext.Users.AsNoTracking()
                    .Where(_ => bdoReportingZonalHeadIds.Contains(_.Id) && _.IsActive)
                    //&& _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                    .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                    //&& w.Users.DivisionId != null && inputDto.VerticalIds.Contains((long)w.Users.DivisionId)
                    ).Select(_ => _.Users)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
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

        public ResultDto GetRABDOUsersByZonalHeadIdsAndVerticalIds(DropDownInputDto inputDto)
        {
            _methodName = "GetRABDOUsersByZonalHeadIdsAndVerticalIds";
            var resultDto = new ResultDto();
            var reportingToUsers = new List<DropDownDto>();
            try
            {
                var userList = _emamiContext.Users.Where(_ => _.ReportingToId != null
                        && inputDto.UserIds.Contains((long)_.ReportingToId) && _.IsActive
                        //&& _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                        )
                        .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.StateTrader)
                        //&& w.Users.DivisionId != null
                        //&& inputDto.VerticalIds.Contains((long)w.Users.DivisionId)
                        );

                reportingToUsers = userList.ToList()
                 .Select(s => new DropDownDto()
                 {
                     Id = s.Users.Id,
                     Name = s.Users.Name
                 }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportingToUsers;
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

        public List<long> GetRAZonalHeadUserIdsByCustomerIds(List<long> customerIds)
        {
            _methodName = "GetRAZonalHeadUserIdsByCustomerIds";
            var reportingToUsers = new List<long>();
            try
            {

                var bdoReportingZonalHeadIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                    .Where(w => customerIds.Contains(w.CustomerId)).ToList().Select(s => s.User?.ReportingToId).ToList();

                reportingToUsers = _emamiContext.Users
                    .Where(_ => bdoReportingZonalHeadIds.Contains(_.Id) && _.IsActive
                    //&& _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                    )
                    .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader)).Select(_ => _.Users.Id).ToList();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return reportingToUsers;
        }

        public List<long> GetBDOByCustomerIds(List<long> customerIds)
        {
            _methodName = "GetBDOByCustomerIds";
            var customerReportingBDOIds = new List<long>();
            try
            {
                var customerBDOIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                     .Where(w => customerIds.Contains(w.CustomerId)).Select(s => s.UserId).ToList();

                customerReportingBDOIds = _emamiContext.Users.Where(_ => customerReportingBDOIds.Contains(_.Id) && _.IsActive)
                   .Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                   .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.StateTrader)).Select(_ => _.Users.Id).ToList();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return customerReportingBDOIds;
        }

        #endregion

        #region CustomerGroupMappings

        public ResultDto GetCustomerGroupListNotMappedInCustomerGroupMappings(long CustomerGroupId)
        {
            _methodName = "GetCustomerGroupListNotMappedInCustomerGroupMappings";
            var resultDto = new ResultDto();
            var OutputDto = new List<DropDownDto>();

            try
            {
                var CustomerGroupMappings = _emamiContext.CustomerGroupMappings.AsNoTracking();



                var mappedCustomerGroupList = CustomerGroupMappings.Select(x => x.CustomerGroupId).Distinct().ToList();

                if (CustomerGroupId > 0)
                {
                    //MappedCustomerGroupNeededDuringEdit = CustomerGroupMappings.FirstOrDefault(_ => _.Id == CustomerGroupMappingId).CustomerGroupId;

                    mappedCustomerGroupList.Remove(CustomerGroupId);
                }
                var CustomerGroupListNotMapped = _emamiContext.CustomerGroups.Where(w => !mappedCustomerGroupList.Contains(w.Id) && w.IsBaseGroup && w.IsActive);

                OutputDto = CustomerGroupListNotMapped.ToList().Select(c => new DropDownDto()
                {
                    Id = c.Id,
                    Name = c.Name

                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = OutputDto;
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

        public ResultDto GetDerivedCustomerGroupListNotMappedInCustomerGroupMappings(long CustomerGroupId)
        {
            _methodName = "GetDerivedCustomerGroupListNotMappedInCustomerGroupMappings";
            var resultDto = new ResultDto();
            var OutputDto = new List<DropDownDto>();
            var MappedDerivedCustomerGroupNeededDuringEdit = new List<long>();
            try
            {
                var CustomerGroupMappings = _emamiContext.CustomerGroupMappings.AsNoTracking();



                var mappedDerivedCustomerGroupList = CustomerGroupMappings.Where(_ => _.IsActive).Select(x => x.DerivedCustomerGroupId).Distinct().ToList();

                if (CustomerGroupId > 0)
                {
                    MappedDerivedCustomerGroupNeededDuringEdit = CustomerGroupMappings.Where(_ => _.CustomerGroupId == CustomerGroupId).Select(x => x.DerivedCustomerGroupId).ToList();
                    foreach (var item in MappedDerivedCustomerGroupNeededDuringEdit)
                    {
                        mappedDerivedCustomerGroupList.Remove(item);
                    }

                }

                var derivedCustomerGroupListNotMapped = _emamiContext.CustomerGroups.Where(w => !mappedDerivedCustomerGroupList.Contains(w.Id) && !w.IsBaseGroup && w.IsActive);

                OutputDto = derivedCustomerGroupListNotMapped.ToList().Select(c => new DropDownDto()
                {
                    Id = c.Id,
                    Name = c.Name

                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = OutputDto;
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

        public ResultDto AddorUpdateCustomerGroupMappings(CustomerGroupMappingDto inputDto)
        {
            _methodName = "AddorUpdateCustomerGroupMappings";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.CustomerGroupId > 0)
                {
                    var checkexists = _emamiContext.CustomerGroupMappings.Where(_ => _.CustomerGroupId == inputDto.CustomerGroupId);
                    if (checkexists != null)
                    {
                        foreach (var item in checkexists)
                        {
                            item.IsActive = false;
                        }
                        _emamiContext.SaveChanges();

                        foreach (var derivedCustomerGroup in inputDto.derivedCustomerGroupId)
                        {
                            var inputEntity = new CustomerGroupMappings();
                            inputEntity.CustomerGroupId = inputDto.CustomerGroupId;
                            inputEntity.DerivedCustomerGroupId = derivedCustomerGroup;
                            inputEntity.CreatedBy = inputDto.LoginUserId;
                            inputEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            inputEntity.IsActive = true;
                            _emamiContext.CustomerGroupMappings.Add(inputEntity);
                            _emamiContext.SaveChanges();
                        }
                    }

                }
                else
                {


                    foreach (var derivedCustomerGroup in inputDto.derivedCustomerGroupId)
                    {
                        var inputEntity = new CustomerGroupMappings();
                        inputEntity.CustomerGroupId = inputDto.CustomerGroupId;
                        inputEntity.DerivedCustomerGroupId = derivedCustomerGroup;
                        inputEntity.CreatedBy = inputDto.LoginUserId;
                        inputEntity.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        inputEntity.IsActive = true;
                        _emamiContext.CustomerGroupMappings.Add(inputEntity);
                        _emamiContext.SaveChanges();
                    }
                }
                resultDto.IsSuccess = true;
                return resultDto;
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

        public ResultDto GetCustomerGroupMappingListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetCustomerGroupMappingListWithPagination";
            var resultDto = new ResultDto();
            DataSourceResult result = new DataSourceResult();
            try
            {
                result = _emamiContext.CustomerGroupMappings.AsNoTracking()
                    .Select(s => new CustomerGroupMappingDto()
                    {
                        CustomerGroupId = s.CustomerGroupId,
                        CustomerGroupName = s.CustomerGroup != null ? s.CustomerGroup.Name : string.Empty
                    }).Distinct().ToList().ToDataSourceResult(inputDto.DataSourceRequest);

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

        public ResultDto GetCustomerGroupMappingsListDetailsById(long customerGroupMappingId)
        {
            _methodName = "GetCustomerGroupMappingsListDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new List<CustomerGroupMappingGridDto>();
            try
            {
                var resultContext = _emamiContext.CustomerGroupMappings.AsNoTracking().Where(_ => _.CustomerGroupId == customerGroupMappingId && _.IsActive);

                var CustomerGroups = _emamiContext.CustomerGroups.AsNoTracking();
                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.ToList()
                    .Select(_ => new CustomerGroupMappingGridDto
                    {
                        derivedCustomerGroupId = _.DerivedCustomerGroupId,
                        DerivedCustomerGroupName = _.DerivedCustomerGroupId > 0 ? CustomerGroups.FirstOrDefault(w => w.Id == _.DerivedCustomerGroupId).Name : string.Empty,

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

        public ResultDto GetCustomerGroupMappingDetailsByCustomerGroupMappingId(long customerGroupMappingId)
        {
            _methodName = "GetCustomerGroupMappingDetailsByCustomerGroupMappingId";
            var resultDto = new ResultDto();
            var result = new CustomerGroupMappingDto();
            var derivedCustomerGroupIds = new List<long>();
            long Id = 0;
            try
            {
                var resultContext = _emamiContext.CustomerGroupMappings.AsNoTracking().Where(f => f.CustomerGroupId == customerGroupMappingId && f.IsActive).ToList();
                foreach (var item in resultContext)
                {
                    derivedCustomerGroupIds.Add(item.DerivedCustomerGroupId);
                    Id = item.Id;
                }

                if (resultContext != null)
                {

                    result = new CustomerGroupMappingDto()
                    {
                        Id = Id,
                        CustomerGroupId = customerGroupMappingId,
                        derivedCustomerGroupId = derivedCustomerGroupIds
                    };

                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
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

        #endregion

        #region Lookup

        /// <summary>
        /// Method to Get Customer Group List By Vertical For Dropdown
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetCustomerGroupListByVerticalIdsForDropdown(IdInputDto inputDto)
        {
            _methodName = "GetCustomerGroupListByVerticalIdsForDropdown";
            var resultDto = new ResultDto();
            try
            {
                var customergroupList = _emamiContext.CustomerGroups.AsNoTracking()
                    .Where(_ => inputDto.IdList.Contains(_.DivisionId) && _.IsActive)
                    .Select(s => new DropDownDto()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

                resultDto.SuccessDto.Response = customergroupList;
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

        public ResultDto GetCustomerListByCustomerGroupIdsAndBDOsForDropdown(DropDownInputDto inputDto)
        {
            _methodName = "GetCustomerListByCustomerGroupIdsAndBDOsForDropdown";
            var resultDto = new ResultDto();
            var outputDto = new List<DropDownDto>();
            try
            {
                if (inputDto.CustomerGroupIds.IsAny() && inputDto.UserIds.IsAny())
                {
                    var mappedCustomers = _emamiContext.CustomerGroupDetails.AsNoTracking()
                        .Where(_ => inputDto.CustomerGroupIds.Contains(_.CustomerGroupId) && _.CustomerGroup.IsActive).Select(_ => _.Customer);

                    var customerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(w => inputDto.UserIds.Contains(w.UserId))
                        .Select(s => s.CustomerId).ToList();

                    //Filter By StateTrader
                    outputDto = mappedCustomers.Where(_ => customerIds.Contains(_.Id) && _.IsActive)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }
                else if (inputDto.CustomerGroupIds.IsAny())
                {
                    outputDto = _emamiContext.CustomerGroupDetails.AsNoTracking()
                        .Where(_ => inputDto.CustomerGroupIds.Contains(_.CustomerGroupId) && _.CustomerGroup.IsActive).Select(_ => _.Customer)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }
                else if (inputDto.UserIds.IsAny())
                {
                    var customerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(w => inputDto.UserIds.Contains(w.UserId))
                        .Select(s => s.CustomerId).ToList();

                    outputDto = _emamiContext.Users.AsNoTracking().Where(_ => customerIds.Contains(_.Id) && _.IsActive)
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }

                resultDto.SuccessDto.Response = outputDto;
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

        public ResultDto GetCustomerListByCustomerGroupIdsCityIdsForDropdown(DropDownInputDto inputDto)
        {
            _methodName = "GetCustomerListByCustomerGroupIdsCityIdsForDropdown";
            var resultDto = new ResultDto();
            var outputDto = new List<DropDownDto>();
            try
            {
                if (inputDto.CityIds.IsAny())
                {
                    var cityBasedCustomers = _emamiContext.Users.AsNoTracking()
                        .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer
                        //&& w.Users.VerticalId == inputDto.VerticalId
                        && inputDto.CityIds.Contains(w.Users.CityId) && w.Users.IsActive).Select(_ => _.Users).ToList();

                    outputDto = cityBasedCustomers
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }
                else
                {
                    var cityBasedCustomers = _emamiContext.Users.AsNoTracking()
                        .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                        .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer
                        //&& w.Users.VerticalId == inputDto.VerticalId
                       /* && inputDto.CityIds.Contains(w.Users.CityId)*/ && w.Users.IsActive).Select(_ => _.Users).ToList();

                    outputDto = cityBasedCustomers
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Name = _.Name,
                        }).ToList();
                }

                resultDto.SuccessDto.Response = outputDto;
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

    }
}
