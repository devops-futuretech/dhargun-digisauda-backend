using GMCore.Authenticate;
using GMCore.Helper;
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
using System.Data.SqlClient;
using Dapper;

namespace Adani.Solution.Service
{
    public interface IUserService
    {
        //Mobile
        ResultDto GetDealerListByUserid(LoginUserIdDto inputDto);
        ResultDto GetBrokerList(LoginUserIdDto inputDto);
        ResultDto AddDevicePushToken(PushTokenInputDto pushTokenInputDto);
        ResultDto SaveUserLoginTime(IdInputDto inputDto);
        ResultDto GetShipToPartyListByCustomerId(LoginUserIdDto inputDto);

        ResultDto GetBDOListByStates(List<long> stateIds);
        ResultDto GetBDOList(LoginUserIdDto inputDto);
        ResultDto CheckDevicePushTokenExists(PushTokenInputDto pushTokenInputDto);
        ResultDto GetDealerListAll(DealerListAllFilterDto inputDto);
        ResultDto GetUserLoginHistory(UserLoginHistoryDto inputDto);
        ResultDto GetDealerListByPendingContractsAndUserid(LoginUserIdDto inputDto);
    }

    public class UserService : IUserService
    {

        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("User Service");
        private const string ServiceName = "User Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public UserService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
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
                _logger.Error("Error instantiating dependencies for User Service", exception);
            }
        }

        #region Mobile
        public ResultDto GetDealerListByUserid(LoginUserIdDto inputDto)
        {
            _methodName = "GetDealerListByUserid";
            var userMasterDto = new List<UserMasterDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            try
            {
                var UserRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId==inputDto.LoginUserId);
                //var userDivisionMappingsContext = _emamiContext.UserDivisionMappings.AsNoTracking();
                if(UserRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                {
                    connection.Open();
                    userMasterDto = connection.Query<UserMasterDto>("GetDealerListByUserId",
                    new
                    {
                        inputDto.DivisionId,
                        inputDto.SalesOrganizationId,
                        inputDto.DistributionChannelId,
                        inputDto.LoginUserId,
                        UserRoleContext.RoleId,
                        inputDto.IsToReturnInactiveData
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

                #region Old Code
                //if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                //{
                //    userDivisionMappingsContext = userDivisionMappingsContext.Where(udm => udm.SalesOrganizationId == inputDto.SalesOrganizationId
                //                          && udm.DistributionChannelId == inputDto.DistributionChannelId
                //                          && udm.DivisionId == inputDto.DivisionId);

                //    userDivisionMappingsContext = (from ud in userDivisionMappingsContext
                //                         join lud in _emamiContext.UserDivisionMappings.AsNoTracking() on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                         equals new { SalesOrganizationId = lud.SalesOrganizationId, DistributionChannelId = lud.DistributionChannelId, DivisionId = lud.DivisionId }
                //                         where lud.UserId == inputDto.LoginUserId
                //                         select ud
                //                   );
                //}
                //var cityContext = _emamiContext.City.AsNoTracking();
                //var stateContext = _emamiContext.State.AsNoTracking();
                //var LoginuserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                //if (LoginuserContext != null && userDivisionMappingsContext!=null)
                //{
                //    var UserRole = UserRoleContext.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId).RoleId;
                //    if (inputDto.IsToReturnInactiveData && UserRole == (int)DTO.Enums.Role.StateTrader)
                //    {
                //        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                         join udm in userDivisionMappingsContext on u.Id equals udm.UserId
                //                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                         orderby u.Name
                //                         where ucm.UserId == inputDto.LoginUserId
                //                         && ur.RoleId == (int)DTO.Enums.Role.Dealer
                //                         select new UserMasterDto
                //                         {
                //                             Id = u.Id,
                //                             StateId = u.StateId,
                //                             EmployeeName = u.Name + "-" + string.Concat((cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName.TrimEnd() : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code),
                //                             EmployeeCode = u.Code,
                //                             SaudaBookingTypeId = u.SaudaBookingTypeId != null ? u.SaudaBookingTypeId.Value : 0
                //                         }).ToList();
                //    }
                //    else if (!inputDto.IsToReturnInactiveData && UserRole == (int)DTO.Enums.Role.StateTrader)
                //    {
                //        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                         join udm in userDivisionMappingsContext on u.Id equals udm.UserId
                //                         orderby u.Name
                //                         where ucm.UserId == inputDto.LoginUserId && u.IsActive && ur.RoleId == (int)DTO.Enums.Role.Dealer
                //                         select new UserMasterDto
                //                         {
                //                             Id = u.Id,
                //                             StateId = u.StateId,
                //                             EmployeeName = u.Name + "-" + string.Concat((cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName.TrimEnd() : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code),
                //                             EmployeeCode = u.Code,
                //                             SaudaBookingTypeId = u.SaudaBookingTypeId != null ? u.SaudaBookingTypeId.Value : 0
                //                         }).ToList();
                //    }
                //    else if (inputDto.IsToReturnInactiveData && UserRole == (int)DTO.Enums.Role.ZonalTrader)
                //    {
                //        //New Reporting to table change
                //        var bdos = (from u in _emamiContext.Users.AsNoTracking()                                     
                //                     join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                //                     where urm.ReportingToUserId == inputDto.LoginUserId 
                //                     select new 
                //                     {
                //                         Id = u.Id,
                //                         SaudaBookingTypeId = u.SaudaBookingTypeId
                //                     }).ToList();



                //        var bdoIds = bdos.Select(a => a.Id).ToList();
                //        var bdoSaudaBookingTypeIds = bdos.Select(a => a.SaudaBookingTypeId).ToList();
                //        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                         join udm in userDivisionMappingsContext on u.Id equals udm.UserId
                //                         orderby u.Name
                //                         where bdoIds.Contains(ucm.UserId)
                //                         && ur.RoleId == (int)DTO.Enums.Role.Dealer  select new UserMasterDto
                //                         {
                //                             Id = u.Id,
                //                             StateId = u.StateId,
                //                             EmployeeName = u.Name + "-" + string.Concat((cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName.TrimEnd() : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code),
                //                             EmployeeCode = u.Code,
                //                             SaudaBookingTypeId = u.SaudaBookingTypeId != null ? u.SaudaBookingTypeId.Value : 0
                //                         }).ToList();
                //    }
                //    else if (!inputDto.IsToReturnInactiveData && UserRole == (int)DTO.Enums.Role.ZonalTrader)
                //    {
                //        //New Reporting to table change
                //        var bdos = (from u in _emamiContext.Users.AsNoTracking()
                //                    join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                //                    where urm.ReportingToUserId == inputDto.LoginUserId 
                //                    select new UserDto
                //                    {
                //                        Id = u.Id,
                //                        SaudaBookingTypeId = u.SaudaBookingTypeId
                //                    }).ToList();

                //        var bdoIds = bdos.Select(a => a.Id).ToList();
                //        var bdoVerticalIds = bdos.Select(a => a.VerticalId).ToList();
                //        var bdoSaudaBookingTypeIds = bdos.Select(a => a.SaudaBookingTypeId).ToList();
                //        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                         join udm in userDivisionMappingsContext on u.Id equals udm.UserId
                //                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                         orderby u.Name
                //                         where bdoIds.Contains(ucm.UserId) && u.IsActive
                //                         && ur.RoleId == (int)DTO.Enums.Role.Dealer
                //                         select new UserMasterDto
                //                         {
                //                             Id = u.Id,
                //                             StateId = u.StateId,
                //                             EmployeeName = u.Name + "-" + string.Concat((cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName.TrimEnd() : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code),
                //                             EmployeeCode = u.Code,
                //                             SaudaBookingTypeId = u.SaudaBookingTypeId != null ? u.SaudaBookingTypeId.Value : 0
                //                         }).ToList();
                //    }
                //    else if (inputDto.IsToReturnInactiveData && UserRole == (int)DTO.Enums.Role.NationalTrader)
                //    {
                //        //New Reporting to table change
                //        var zonalHeadIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();

                //        var bdos = (from u in _emamiContext.Users.AsNoTracking()
                //                    join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                //                    where zonalHeadIds.Contains(urm.ReportingToUserId)
                //                    select new UserDto
                //                    {
                //                        Id = u.Id,
                //                        SaudaBookingTypeId = u.SaudaBookingTypeId
                //                    }).ToList();


                //        var bdoIds = bdos.Select(a => a.Id).ToList();
                //        var bdoVerticalIds = bdos.Select(a => a.VerticalId).ToList();
                //        var bdoSaudaBookingTypeIds = bdos.Select(a => a.SaudaBookingTypeId).ToList();
                //        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                         join udm in userDivisionMappingsContext on u.Id equals udm.UserId
                //                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                         orderby u.Name
                //                         where bdoIds.Contains(ucm.UserId) && ur.RoleId == (int)DTO.Enums.Role.Dealer 
                //                         select new UserMasterDto
                //                         {
                //                             Id = u.Id,
                //                             StateId = u.StateId,
                //                             EmployeeName = u.Name + "-" + string.Concat((cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName.TrimEnd() : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code),
                //                             EmployeeCode = u.Code,
                //                             SaudaBookingTypeId = u.SaudaBookingTypeId != null ? u.SaudaBookingTypeId.Value : 0
                //                         }).ToList();
                //    }
                //    else if (!inputDto.IsToReturnInactiveData && UserRole == (int)DTO.Enums.Role.NationalTrader)
                //    {
                //        //New Reporting to table change
                //        var zonalHeadIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();

                //        var bdos = (from u in _emamiContext.Users.AsNoTracking()
                //                    join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                //                    where zonalHeadIds.Contains(urm.ReportingToUserId)
                //                    select new UserDto
                //                    {
                //                        Id = u.Id,
                //                        SaudaBookingTypeId = u.SaudaBookingTypeId
                //                    }).ToList();

                //        var bdoIds = bdos.Select(a => a.Id).ToList();
                //        var bdoVerticalIds = bdos.Select(a => a.VerticalId).ToList();
                //        var bdoSaudaBookingTypeIds = bdos.Select(a => a.SaudaBookingTypeId).ToList();
                //        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                         join udm in userDivisionMappingsContext on u.Id equals udm.UserId
                //                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                         orderby u.Name
                //                         where bdoIds.Contains(ucm.UserId) && u.IsActive && ur.RoleId == (int)DTO.Enums.Role.Dealer
                //                         select new UserMasterDto
                //                         {
                //                             Id = u.Id,
                //                             StateId = u.StateId,
                //                             EmployeeName = u.Name + "-" + string.Concat((cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName.TrimEnd() : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code),
                //                             EmployeeCode = u.Code,
                //                             SaudaBookingTypeId = u.SaudaBookingTypeId != null ? u.SaudaBookingTypeId.Value : 0
                //                         }).ToList();
                //    }
                //    foreach (var user in userMasterDto)
                //    {
                //        user.IsBroker = UserRoleContext.Any(_ => _.UserId == user.Id && _.RoleId == (int)DTO.Enums.Role.Broker) ? true : false;

                //    }
                //}

                //userMasterDto = userMasterDto.GroupBy(s => s.Id).Select(g => g.First()).ToList();
                #endregion

                return _resultService.SuccessObject(userMasterDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDealerListByPendingContractsAndUserid(LoginUserIdDto inputDto)
        {
            _methodName = "GetDealerListByPendingContractsAndUserid";
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            try
            {
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var UserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (UserContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var UserRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (UserRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if(UserRoleContext.RoleId != (int)DTO.Enums.Role.ZonalTrader && UserRoleContext.RoleId != (int)DTO.Enums.Role.StateTrader)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow).Date;

                var userDivisions = _emamiContext.UserDivisionMappings
                .Where(x => x.UserId == inputDto.LoginUserId)
                .Select(x => new
                {
                    x.SalesOrganizationId,
                    x.DistributionChannelId,
                    x.DivisionId
                });

                IQueryable<long> dealerIds = Enumerable.Empty<long>().AsQueryable();

                if(UserRoleContext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    dealerIds =
                        from bdo in _emamiContext.UserReportingToMappings
                        join ucm in _emamiContext.UserCustomerMapping
                            on bdo.UserId equals ucm.UserId
                        join cus in _emamiContext.Users
                            on ucm.CustomerId equals cus.Id
                        where bdo.ReportingToUserId == inputDto.LoginUserId
                        select cus.Id;
                }
                else
                {
                    dealerIds =
                        from ucm in _emamiContext.UserCustomerMapping
                        join cus in _emamiContext.Users
                            on ucm.CustomerId equals cus.Id
                        where ucm.UserId == inputDto.LoginUserId
                        select cus.Id;
                }

                //var dealerIds = _emamiContext.UserCustomerMapping
                //.Where(x => x.UserId == inputDto.LoginUserId)
                //.Select(x => x.CustomerId);

                var dealers = (
                from pc in _emamiContext.PendingContracts
                join u in _emamiContext.Users on pc.UserId equals u.Id

                join sku in _emamiContext.Skus on new
                {
                    MaterialCode = pc.MaterialCode,
                    SalesOrgId = pc.SalesOrgId,
                    DistChnlId = pc.DistChnlId,
                    DivisionId = pc.DivisionId
                }
                equals new
                {
                    MaterialCode = sku.SkuCode,
                    SalesOrgId = sku.SalesOrganizationId,
                    DistChnlId = sku.DistributionChannelId,
                    DivisionId = sku.DivisionId
                }

                join ud in userDivisions on new
                {
                    SalesOrganizationId = pc.SalesOrgId,
                    DistributionChannelId = pc.DistChnlId,
                    DivisionId = pc.DivisionId
                }
                equals new
                {
                    SalesOrganizationId = ud.SalesOrganizationId,
                    DistributionChannelId = ud.DistributionChannelId,
                    DivisionId = ud.DivisionId
                }

                where dealerIds.Contains(u.Id)
                      && pc.PendingQuantityInCase >= 1
                      && (pc.ContractValidFrom == null || DbFunctions.TruncateTime(pc.ContractValidFrom) <= currentDate) 
                      && (pc.ContractValidTo == null || DbFunctions.TruncateTime(pc.ContractValidTo) >= currentDate)

                select new UserDto
                {
                    Id = u.Id,
                    Code = u.Code,
                    Name = u.Name +" - "+ (u.Code ?? "")
                }

            ).Distinct().ToListAsync();

                return _resultService.SuccessObject(dealers);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetShipToPartyListByCustomerId(LoginUserIdDto inputDto)
        {
            _methodName = "GetShipToPartyListByCustomerId";
            var outputDto = new List<UserMasterDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.LoginUserId == 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
            if (userContext == null)
            {
                return _resultService.ErrorMessage(Constants.UserNotFound);
            }
            try
            {
                outputDto = _emamiContext.CustomerShipToPartyMappings.AsNoTracking().Where(_ => _.CustomerId == inputDto.LoginUserId)
                    .Join(_emamiContext.Users.AsNoTracking().Where(_ => inputDto.IsToReturnInactiveData ? inputDto.IsToReturnInactiveData : _.IsActive), x => x.ShipToPartyId, u => u.Id, (x, u) => new { u })
                    .Select(_ => new UserMasterDto()
                    {
                        Id = _.u.Id,
                        EmployeeName = _.u.Name,
                        EmployeeCode = _.u.Code,
                        //FrieghtRoute = _.u.FreightRoute != null ? _.u.FreightRoute.Name : string.Empty,
                        //FrieghtZone = _.u.FreightZone != null ? _.u.FreightZone.Name : string.Empty,
                        Loadability = _.u.Loadability,
                        DepotLoadability = _.u.DepotLoadability,
                        //VerticalId = _.u.DivisionId != null ? _.u.DivisionId.Value : 0,
                        //Vertical = _.u.Division.Name,
                        SaudaBookingTypeId = _.u.SaudaBookingTypeId != null ? _.u.SaudaBookingTypeId.Value : 0,
                       // SaudaBookingType = _.u.SaudaBookingType.Name
                    }).ToList();

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetBrokerList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBrokerList";
            var userMasterDto = new List<UserMasterDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    userMasterDto = (from ucm in _emamiContext.UserRoles.AsNoTracking()
                                     join u in _emamiContext.Users.AsNoTracking() on ucm.UserId equals u.Id
                                     where ucm.RoleId == (int)DTO.Enums.Role.Broker
                                     select new UserMasterDto
                                     {
                                         Id = u.Id,
                                         EmployeeName = u.Name,
                                         EmployeeCode = u.Code,
                                         //FrieghtRoute = u.FreightZone.Name,
                                         //FrieghtZone = u.FreightRoute.Name

                                     }).ToList();
                }
                else
                {
                    userMasterDto = (from ucm in _emamiContext.UserRoles.AsNoTracking()
                                     join u in _emamiContext.Users.AsNoTracking() on ucm.UserId equals u.Id
                                     where ucm.RoleId == (int)DTO.Enums.Role.Broker && u.IsActive
                                     select new UserMasterDto
                                     {
                                         Id = u.Id,
                                         EmployeeName = u.Name,
                                         EmployeeCode = u.Code,
                                         //FrieghtRoute = u.FreightZone.Name,
                                         //FrieghtZone = u.FreightRoute.Name

                                     }).ToList();
                }

                return _resultService.SuccessObject(userMasterDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AddDevicePushToken(PushTokenInputDto pushTokenInputDto)
        {
            var resultDto = new ResultDto();

            _methodName = "AddUserPushTokenKey";
            try
            {
                string OldPushTokenKey = string.Empty;
                int? OldRegistrationTypeId = 0;
                if (pushTokenInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);

                }
                if (pushTokenInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);

                }
                if (string.IsNullOrEmpty(pushTokenInputDto.PushToken))
                {
                    return _resultService.ErrorMessage(Constants.PushTokenEmpty);

                }
                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == pushTokenInputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                OldPushTokenKey = userContext.PushTokenKey;
                OldRegistrationTypeId = userContext.RegistrationTypeId;

                userContext.PushTokenKey = pushTokenInputDto.PushToken;
                userContext.RegistrationTypeId = pushTokenInputDto.RegistrationTypeId;
                _emamiContext.SaveChanges();

                if (OldPushTokenKey != pushTokenInputDto.PushToken)
                {
                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                    {
                        PushTokenKey = OldPushTokenKey,
                        Title = "Logout",
                        Message = "You are logging into another device",
                        RegistrationTypeId = OldRegistrationTypeId != null ? (int)OldRegistrationTypeId : 0,
                        IsLogOut = true
                    };
                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto CheckDevicePushTokenExists(PushTokenInputDto pushTokenInputDto)
        {
            var resultDto = new ResultDto();
            _methodName = "CheckDevicePushTokenExists";
            try
            {
                if (pushTokenInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);

                }
                if (pushTokenInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);

                }
                if (string.IsNullOrEmpty(pushTokenInputDto.PushToken))
                {
                    return _resultService.ErrorMessage(Constants.PushTokenEmpty);

                }
                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == pushTokenInputDto.LoginUserId && _.PushTokenKey == pushTokenInputDto.PushToken);
                if (userContext == null)
                {
                    resultDto.SuccessDto.Response = false;
                }
                else
                {
                    resultDto.SuccessDto.Response = true;
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

        public ResultDto SaveUserLoginTime(IdInputDto inputDto)
        {
            var resultDto = new ResultDto();
            _methodName = "SaveUserLogin";
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                bool isSaved = false;
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var currentTime = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext != null)
                {
                    var userAttendance = new UserAttendance();
                    var userAttendanceContext = _emamiContext.UserAttendance.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate) && _.UserId == inputDto.LoginUserId);
                    var configContext = _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.LoginBaseHour);
                    if (userAttendanceContext == null && configContext != null)
                    {
                        TimeSpan loginBaseHour = TimeSpan.FromHours(Convert.ToDouble(configContext.Value));
                        if (currentTime >= loginBaseHour)
                        {
                            userAttendance = new UserAttendance()
                            {
                                UserId = inputDto.LoginUserId,
                                LoginTime = currentDate,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = currentDate,
                            };
                            _emamiContext.UserAttendance.Add(userAttendance);
                            _emamiContext.SaveChanges();
                            isSaved = true;
                        }
                    }
                    else
                    {
                        isSaved = true;
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = isSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetUserLoginHistory(UserLoginHistoryDto inputDto)
        {
            _methodName = "GetUserLoginHistory";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var userLoginHistory = new UserLoginHistory
                {
                    LoginUserId = inputDto.LoginUserId,
                    LoginDate = DateTime.Now,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateTime.Now
                };

                _emamiContext.UserLoginHistory.Add(userLoginHistory);
                _emamiContext.SaveChanges();
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

        public ResultDto GetBDOListByStates(List<long> stateIds)
        {
            _methodName = "GetBDOListByStates";
            var outputDto = new List<DropDownDto>();
            try
            {
                outputDto = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), x => x.Id, ur => ur.UserId, (x, ur) =>
                           new { Id = x.Id, Name = x.Name, StateId = x.StateId, RoleId = ur.RoleId }).Where(_ => stateIds.Contains(_.StateId) && _.RoleId == (int)DTO.Enums.Role.StateTrader)
                    .Select(_ => new DropDownDto()
                    {
                        Id = _.Id,
                        Name = _.Name
                    }).ToList();

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetBDOList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBrokerList";
            var userMasterDto = new List<DropDownDto>();
            try
            {
                userMasterDto = (from ucm in _emamiContext.UserRoles.AsNoTracking()
                                 join u in _emamiContext.Users.AsNoTracking() on ucm.UserId equals u.Id
                                 where ucm.RoleId == (int)DTO.Enums.Role.StateTrader
                                 select new DropDownDto
                                 {
                                     Id = u.Id,
                                     Name = u.Name

                                 }).ToList();

                return _resultService.SuccessObject(userMasterDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDealerListAll(DealerListAllFilterDto inputDto)
        {
            _methodName = "GetDealerListAll";
            var userMasterDto = new List<UserMasterDetailDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.ZHId <= 0)
            {
                return _resultService.ErrorMessage(Constants.ZonalHeadIsMissing);
            }
            try
            {

                var ZonalTrader = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.ZHId);
                if (ZonalTrader == null)
                {
                    return _resultService.ErrorMessage(Constants.ZonalHeadIsMissing);
                }
                var UserRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (UserRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                {
                    connection.Open();
                    userMasterDto = connection.Query<UserMasterDetailDto>("GetDealerListAll",
                    new
                    {
                        inputDto.LoginUserId,
                        UserRoleContext.RoleId,
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

                //IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                //divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //  .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                //if (inputDto.BdoIds == null || !inputDto.BdoIds.Any())
                //{
                //New Reporting to table change
                //inputDto.BdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.ZHId).Select(_ => _.UserId).ToList();
                //inputDto.BdoIds = (from u in _emamiContext.UserReportingToMappings.AsNoTracking()
                //                   join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on u.UserId equals ud.UserId
                //                   join dm in divisionslogieduser on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                      equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                   where u.ReportingToUserId == inputDto.ZHId
                //                   select u.UserId
                //                 ).ToList();
                //inputDto.BdoIds = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId && _.IsActive == true).Select(_ => _.Id).ToList();
                //}
                //var cityContext = _emamiContext.City.AsNoTracking();
                //var stateContext = _emamiContext.State.AsNoTracking();
                //userMasterDto = (from u in _emamiContext.Users.AsNoTracking()
                //                 join uc in _emamiContext.UserCustomerMapping.AsNoTracking() on u.Id equals uc.CustomerId
                //                 join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on uc.CustomerId equals ud.UserId
                //                 join dm in divisionslogieduser on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                          equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                where inputDto.BdoIds.Contains(uc.UserId)
                //                orderby u.Name
                //                select new UserMasterDetailDto()
                //                {
                //                    Id=u.Id,
                //                    EmployeeName = u.Name + "-" + string.Concat((cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == u.CityId).CityName.TrimEnd() : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code),
                //                    EmployeeCode =u.Code
                //                }
                //               ).Distinct().ToList();

                //userMasterDto = _emamiContext.Users.AsNoTracking()
                //    .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.Id, uc => uc.CustomerId, (u, uc) => new { user = u, UserCustomer = uc })
                //    .Where(_ => inputDto.BdoIds.Contains(_.UserCustomer.UserId))
                //    .Select(_ => new UserMasterDetailDto()
                //    {
                //        Id = _.user.Id,
                //        EmployeeName = _.user.Name,
                //        EmployeeCode = _.user.Code,
                //        Loadability = _.user.Loadability,
                //        DepotLoadability = _.user.DepotLoadability,
                //        SaudaBookingTypeId = _.user.SaudaBookingTypeId != null ? _.user.SaudaBookingTypeId.Value : 0
                //    }).ToList();

                return _resultService.SuccessObject(userMasterDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
    }
}
