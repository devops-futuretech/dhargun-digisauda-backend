using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using System.Linq.Expressions;
using GMCore.Helper;
using System.Data.Entity.Core.Objects;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Windows.Media.Imaging;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Adani.Solution.MVC.Common;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Services;
using System.Threading.Tasks;
//using Google.Apis.AnalyticsReporting.v4.Data;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Web.Mvc;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
using System.Net.Http;
using User = Adani.Solution.Data.Entities.User;
//using System.Threading.Tasks;
//using Adani.Solution.MVC.Common;
//using Amazon.Runtime;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Services;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using System.Data.Entity.Core.Common.CommandTrees;
//using System.Net.Http;
//using System.Web.Mvc;
using System.Web.Hosting;

namespace Adani.Solution.Service
{
    public interface IEmployeeService
    {
        ResultDto SaveUser(EmployeeDto employeeDto);
        ResultDto GetUserMasterList(LoginUserIdDto inputDto);
        ResultDto GetUserDetailsById(string userId);
        ResultDto GetUserDetails(LoginUserIdDto inputDto);
        ResultDto UpdateUser(EmployeeDto employeeDto);
        ResultDto ProfileUpload(EmployeeDto employeeDto);

        ResultDto GetDealerList(LoginUserIdDto inputDto);
        ResultDto GetDealerDetailsById(string dealerId);
        ResultDto GetDealerListWithPaging(KendoGridResult inputDto);

        ResultDto GetBrokerList(LoginUserIdDto inputDto);
        ResultDto GetBrokerDetailsById(string brokerId);
        ResultDto GetBrokerListddl(DealerBrokerParamDto inputDto);


        ResultDto GetUserRoleClaims(UserIdDto userIdDto);
        ResultDto GetBDOStatistics(SaudaFilterDto saudaFilterDto);
        ResultDto GetKeyPerformanceIndicator(IdInputDto inputDto);

        ResultDto GetUserAssignedTo(IdInputDto inputDto);
        ResultDto AddUserTarget(AddUserTargetDto addUserTargetDto);
        ResultDto UpdateUserTarget(UpdateUserTargetDto updateUserTargetDto);
        ResultDto GetUserTargetList(LoginUserIdDto inputDto);
        ResultDto GetUserTargetById(IdInputDto inputDto);


        ResultDto GetUsersByRole(IdInputDto inputDto);
        ResultDto ChartSaudaAndSalesDetailsByOilTypes(ChartSaudaSalesByOilTypeInputDto inputDto);
        ResultDto ChartSaudaApprovalDetailsByOilTypes(ChartSaudaSalesByOilTypeInputDto inputDto);
        ResultDto GetBDOListByZonalHead(IdInputDto inputDto);

        ResultDto GetUserExcelExportList(LoginUserIdDto inputDto);
        ResultDto DeleteConsentImage(BulletinInputDto inputDto);
        ResultDto UploadConsentImage(List<DealerConsentImageUploadDto> inputDto);

        #region ShipToParty
        ResultDto GetShipToPartyListExcelExport(LoginUserIdDto inputDto);
        ResultDto GetShipToPartyListWithPaging(KendoGridResult inputDto);
        ResultDto GetShipToPartyDetailsById(string shiptoparty);
        ResultDto GetShipToPartyBrokerList(LoginUserIdDto inputDto);
        #endregion

        ResultDto GetDealerListWithPagination(DealerListInputDto inputDto);

        ResultDto GetDealerListWithPaginationAdminApp(DealerListInputDto inputDto);
        ResultDto GetBrokerListWithPaginationAdminApp(DealerListInputDto inputDto);
        ResultDto GetShipToPartyListWithPaginationAdminApp(DealerListInputDto inputDto);
        ResultDto GetUserListWithPaginationAdminApp(LoginUserIdDto inputDto);

        ResultDto GetDealerAndBrokerList(DealerBrokerParamDto inputDto);
        ResultDto GetDealerListByVertical(DealerBrokerParamDto inputDto);
        ResultDto GetShipToPartyListBasedOnVertical(DealerBrokerParamDto inputDto);
        ResultDto GetDashboardDetails(LoginUserIdDto LoginUserId);
        ResultDto GetDashboardSalesUserInfo(LoginUserIdDto inputKey);
        ResultDto GetDashboardUserInfo(LoginUserIdDto inputKey);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Employee Service");
        private const string ServiceName = "Employee Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public EmployeeService(IAdaniContext salesContext, IResultService resultService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Employee Service", exception);
            }
        }

        #region User

        /// <summary>
        /// Method to Save User
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public ResultDto SaveUser(EmployeeDto employeeDto)
        {
            _methodName = "SaveUser";
            var resultDto = new ResultDto();
            try
            {
                if (employeeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                if (string.IsNullOrEmpty(employeeDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                              
                var userDatas = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                           .Where(_ => //_.User.DivisionId == employeeDto.VerticalId
                            _.User.IsActive == employeeDto.IsActive
                           && _.UserRole.RoleId == employeeDto.RoleId)
                           .Select(s => new
                           {
                               Name = s.User.Name,
                               Code = s.User.Code,
                               ShiptoPartyCode = s.User.ShipToPartyCode,
                               Email = s.User.Email,
                               MobileNumber = s.User.MobileNumber
                           });
                if (userDatas == null && !userDatas.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                //If the user has been ShipToParty, [Code] property only validate for the duplicates.
                if (employeeDto.RoleId == (int)DTO.Enums.Role.ShipToParty)
                {
                    var isCodeExist = userDatas.Any(_ => _.ShiptoPartyCode == employeeDto.Code && !string.IsNullOrEmpty(employeeDto.Code));
                    if (isCodeExist)
                        return _resultService.ErrorMessage(Constants.CodeExist);
                }
                else if (employeeDto.RoleId != (int)DTO.Enums.Role.Dealer && employeeDto.RoleId != (int)DTO.Enums.Role.ShipToParty && employeeDto.RoleId != (int)DTO.Enums.Role.Broker)
                {
                    //ReportingTo UserCheck
                    if (!employeeDto.SelectedReportingToIds.IsAny())
                        return _resultService.ErrorMessage(Constants.ReportingToUserNotSelected);
                }
                else
                {
                    //User Name Validation
                    //var isNameExist = userDatas.Any(_ => _.Name == employeeDto.Name && !string.IsNullOrEmpty(employeeDto.Name));
                    //if (isNameExist)
                    //    return _resultService.ErrorMessage(Constants.NameExist);

                    

                    //User Code Validation
                    var isCodeExist = userDatas.Any(_ => _.Code == employeeDto.Code && !string.IsNullOrEmpty(employeeDto.Code));
                    if (isCodeExist)
                        return _resultService.ErrorMessage(Constants.CodeExist);

                    //User Email Validation
                    //var isEmailExist = userDatas.Any(_ => _.Email == employeeDto.Email && !string.IsNullOrEmpty(employeeDto.Email));
                    //if (isEmailExist)
                    //    return _resultService.ErrorMessage(Constants.EmailExists);

                    //User MobileNumber Validation
                    //var isMobileNumberExist = userDatas.Any(_ => _.MobileNumber == employeeDto.MobileNumber && !string.IsNullOrEmpty(employeeDto.MobileNumber));
                    //if (isMobileNumberExist)
                    //    return _resultService.ErrorMessage(Constants.MobileNumberExist);
                }

                var customerGroup = string.Empty;
                if (employeeDto.RoleId == (int)DTO.Enums.Role.Dealer)
                {
                    customerGroup = "01";
                }
                else if (employeeDto.RoleId == (int)DTO.Enums.Role.Broker)
                {
                    customerGroup = "02";
                }

                if (employeeDto.RoleId == (int)DTO.Enums.Role.Dealer)
                {
                    if (!string.IsNullOrWhiteSpace(employeeDto.TANNumber))
                    {
                        var existingUser = _emamiContext.Users.AsNoTracking().Where(c => c.TANNumber == employeeDto.TANNumber && c.Code != employeeDto.Code).ToList();

                        if (existingUser != null && existingUser.Any())
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = string.Format(Constants.TannumberExist, string.Join(",", existingUser.Select(c => c.Name)));
                            return resultDto;
                        }
                    }
                }

                var user = new User
                {
                    Name = employeeDto.Name,
                    //Code = employeeDto.Code,
                    Email = employeeDto.Email,
                    MobileNumber = employeeDto.MobileNumber,
                    Password = string.IsNullOrEmpty(employeeDto.Password) ? UtilityHelper.ConvertToMd5(UtilityHelper.GenerateRandomAlphaUpper(8), SecurityConstants.EncryptionKey) : UtilityHelper.ConvertToMd5(employeeDto.Password, SecurityConstants.EncryptionKey),
                    //Password = "JXJK14rJK/nCUGdsaZIc2w==",
                    OtpNumber = employeeDto.OtpNumber,
                    //RoleId = employeeDto.RoleId,
                    //UserCode = employeeDto.UserCode,
                    PushTokenKey = employeeDto.Name,
                    //ReportingToId = employeeDto.ReportingToId,
                    //ReportingToId = employeeDto.ReportingToId,
                    //SpecialityFatReportingToId = employeeDto.SpecialityFatReportingToId,
                    //FreightRouteId = employeeDto.FreightRouteId,
                    //FreightZoneId = employeeDto.FreightZoneId,
                    Remarks = employeeDto.Remarks,
                    LastLoggedInDate = employeeDto.LastLoggedInDate,
                    PreviousLoggedInDate = employeeDto.PreviousLoggedInDate,
                    ApprovedBy = employeeDto.ApprovedBy,
                    ApprovedDate = employeeDto.ApprovedDate,
                    IsActive = employeeDto.IsActive,
                    IsBlacklisted = employeeDto.IsBlacklisted,
                    ImageUrl = employeeDto.ImageUrl,
                    ParentUserId = employeeDto.ParentUserId,
                    RegistrationTypeId = employeeDto.RegistrationTypeId,
                    Region = employeeDto.Region,
                    Pincode = employeeDto.Pincode,
                    Street = employeeDto.Street,
                    DistrictId = employeeDto.DistrictId,
                    District = employeeDto.District,
                    CityId = employeeDto.CityId,
                    City = employeeDto.City,
                    //TerritoryId = employeeDto.TerritoryId,
                    //Territory = employeeDto.Territory,
                    StateId = employeeDto.StateId,
                    State = employeeDto.State,
                    ExecutivePassword = employeeDto.ExecutivePassword,
                    McsNo = employeeDto.McsNo,
                    //MobileNumber1 = employeeDto.MobileNumber1,
                    MobileNumber2 = employeeDto.MobileNumber2,
                    //AddressLine1 = employeeDto.AddressLine1,
                    //AddressLine2 = employeeDto.AddressLine2,
                    //AddressLine3 = employeeDto.AddressLine3,
                    GSTN = employeeDto.GSTN,
                    TANNumber = employeeDto.TANNumber,
                    VisitDay = employeeDto.VisitDay,
                    //SaudaValidityPeriod = employeeDto.SaudaValidityPeriod,
                    WeeklyClosingDay = employeeDto.WeeklyClosingDay,
                    MonthlyPotential = employeeDto.MonthlyPotential,
                    //  Loadability = employeeDto.PlantTruckCapacity,
                    Address1 = employeeDto.Address1,
                    Address2 = employeeDto.Address2,
                    //Address1 = employeeDto.Address,
                    CustClass = employeeDto.CustClass,
                    Branch = employeeDto.Branch,
                    //ReportingTo = employeeDto.ReportingTo,
                    SalesAccess = employeeDto.SalesAccess,
                    Designation = employeeDto.Designation,
                    //HeadquartersId = employeeDto.HeadquartersId,
                    ZoneId = employeeDto.ZoneId,
                    Acedns = employeeDto.Acedns,
                    SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                    //IncoTermsId = employeeDto.IncoTermsId,
                    //TransportModeId = employeeDto.TransportModeId,
                    //DivisionId = employeeDto.VerticalId,
                    CreatedBy = employeeDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    //IsBroker = employeeDto.IsBroker,
                    //IsDealer = employeeDto.IsDealer,
                    //IsUser = employeeDto.IsUser,
                    //SaudaLimit = employeeDto.SaudaLimit,
                    IsSelf = employeeDto.IsSelf,
                    IsBroker = employeeDto.IsBroker,
                    CustomerGroup = customerGroup,
                    // DepotLoadability = employeeDto.DepotTruckCapacity,
                    PasswordModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    Latitude = employeeDto.Latitude,
                    Longitude = employeeDto.Longitude,
                    //CustomerGroupOneId = employeeDto.CustomerGroupOneId,
                    //CustomerGroupTwoId = employeeDto.CustomerGroupTwoId,
                    CustomerGroupFiveId = employeeDto.CustomerGroupFiveId,
                    AdditionalMobileNumber = employeeDto.AdditionalMobileNumber,
                    ContactPersonName = employeeDto.ContactPersonName,
                    IsActiveForCall = employeeDto.Attachments.IsAny() ? employeeDto.IsActiveForCall : false,
                    CompanyCode = employeeDto.CompanyCode == null ? String.Empty : employeeDto.CompanyCode,
                    LineId = employeeDto.LineId!=null ? string.Join(",", employeeDto.LineId) : string.Empty
                };

                if (employeeDto.RoleId == (int)DTO.Enums.Role.ShipToParty)
                {
                    user.ShipToPartyCode = employeeDto.Code;
                    user.Code = String.Empty;
                }
                else
                {
                    user.Code = employeeDto.Code;
                }
                _emamiContext.Users.Add(user);
                _emamiContext.SaveChanges();

                //User Division Mapping
                foreach (var division in employeeDto.DivisionList)
                {
                    var userDivMap = new UserDivisionMapping
                    {
                        UserId = user.Id,
                        SalesOrganizationId = division.SalesOrganizationId,
                        DistributionChannelId = division.DistributionChannelId,
                        DivisionId = division.DivisionId,
                        SaudaLimit = division.SaudaLimit,
                        SaudaValidityPeriod = division.SaudaValidityPeriod,
                        CreatedBy = employeeDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    };
                    _emamiContext.UserDivisionMappings.Add(userDivMap);
                    _emamiContext.SaveChanges();

                    AddOrReplaceUserDivisionPlantMappings(userDivMap.Id, division.UserDivisionPlantIds, user.Id);

                }


                //uploaded consent images
                if (employeeDto.Attachments.IsAny())
                {
                    string folderName = DTO.Enums.PageType.ConsentImages.ToString();
                    var mediaFileItemList = new List<SupportAttachmentDto>();
                    foreach (var attachment in employeeDto.Attachments)
                    {
                        string ImagePath = string.Empty;
                        if (attachment != null)
                        {
                            var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }
                            var ext = Path.GetExtension(attachment.FileName);
                            attachment.FileName = Guid.NewGuid() + ext;
                            var filename = Path.Combine(directory, attachment.FileName);
                            string mediaPath = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName);
                            attachment.MediaPath = Path.Combine(mediaPath, attachment.FileName);

                            //Deletion exists file  
                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }
                            File.WriteAllBytes(filename, attachment.FileByteArray);
                        }
                    }

                    foreach (var attachment in employeeDto.Attachments)
                    {
                        var consentImageContext = new ConsentImageDetailsForCustomers()
                        {
                            UserId = user.Id,
                            FileName = attachment.FileName,
                            MediaPath = attachment.MediaPath,
                            MediaTypeId = attachment.MediaTypeId,
                            CreatedBy = employeeDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.ConsentImageDetailsForCustomers.Add(consentImageContext);
                    }
                    _emamiContext.SaveChanges();
                }


                ////AddOrUpdateDealerLocation(employeeDto);

                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = employeeDto.RoleId
                };
                _emamiContext.UserRoles.Add(userRole);

                AddUserIncoTerms(employeeDto.IncoTermsId, user.Id, employeeDto.LoginUserId);

                _emamiContext.SaveChanges();

                #region User Reporting To mapping
                var reporterrormsg = "";
                if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.StateTrader || employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.ZonalTrader || employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.NationalTrader)
                {


                    if (employeeDto.SelectedReportingToIds.IsAny())
                    {

                        var existIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == user.Id).Select(s => s.ReportingToUserId).ToList();

                        var removeIds = existIds.Except(employeeDto.SelectedReportingToIds);

                        if (removeIds.IsAny())
                        {
                            foreach (var id in removeIds)
                            {
                                var report = _emamiContext.UserReportingToMappings.FirstOrDefault(_ => _.ReportingToUserId == id);
                                _emamiContext.UserReportingToMappings.Remove(report);
                            }

                            _emamiContext.SaveChanges();
                        }


                        foreach (var userId in employeeDto.SelectedReportingToIds)
                        {
                            var reportuseralreadyExists = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.ReportingToUserId == userId && _.UserId == user.Id);

                            if (reportuseralreadyExists == null)
                            {

                                //var reportUserComb = (from ud in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                      where ud.UserId == userId
                                //                      select ud
                                //               );
                                //var reportuserData = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userId);

                                //var reportuserExists = (from urm in _emamiContext.UserReportingToMappings.AsNoTracking()
                                //                        where urm.UserId == user.Id
                                //                        select urm.ReportingToUserId
                                //                    ).ToList();

                                //if (reportuserExists.IsAny())
                                //{
                                //    var isExistBdoCombination = (from div in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                                 join bdiv in reportUserComb on new { SalesOrganizationId = div.SalesOrganizationId, DistributionChannelId = div.DistributionChannelId, DivisionId = div.DivisionId }
                                //             equals new { SalesOrganizationId = bdiv.SalesOrganizationId, DistributionChannelId = bdiv.DistributionChannelId, DivisionId = bdiv.DivisionId }
                                //                                 where reportuserExists.Contains(div.UserId)
                                //                                 select div.UserId
                                //                          );
                                //    if (isExistBdoCombination.IsAny())
                                //    {
                                //        reporterrormsg = reporterrormsg + reportuserData.Name + ",";
                                //    }
                                //    else
                                //    {
                                _emamiContext.UserReportingToMappings.Add(new UserReportingToMapping() { UserId = user.Id, ReportingToUserId = userId, CreatedBy = employeeDto.LoginUserId, RoleId = employeeDto.RoleId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                                _emamiContext.SaveChanges();
                                //        }
                                //    }
                                //    else
                                //    {
                                //        var isCombMatched = (from ud in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                             join rud in reportUserComb on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                //                             equals new { SalesOrganizationId = rud.SalesOrganizationId, DistributionChannelId = rud.DistributionChannelId, DivisionId = rud.DivisionId }
                                //                             where ud.UserId == user.Id
                                //                             select ud
                                //                       ).FirstOrDefault();

                                //        if (isCombMatched != null)
                                //        {
                                //            _emamiContext.UserReportingToMappings.Add(new UserReportingToMapping() { UserId = user.Id, ReportingToUserId = userId, CreatedBy = employeeDto.LoginUserId, RoleId = employeeDto.RoleId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                                //            _emamiContext.SaveChanges();
                                //        }
                                //        else
                                //        {
                                //            reporterrormsg = reporterrormsg + reportuserData.Name + ",";
                                //        }
                                //    }


                            }

                        }



                    }
                }


                #endregion

                #region Dealer Depot Mapping

                if (employeeDto.RoleId == (int)DTO.Enums.Role.Dealer)
                {
                    //Depot mapping
                    //CreateUserDepotMapping(employeeDto.SelectedDepotIds, user.Id, employeeDto.LoginUserId);
                    //Plant mapping
                    //CreateUserDepotMapping(employeeDto.SelectedPlantIds, user.Id, employeeDto.LoginUserId);
                    //Ship to party mapping
                    CreateCustomerShipToPartyMapping(employeeDto.SelectedDealerIds, user.Id, employeeDto.LoginUserId);
                    //TruckCapacityMappings
                    CreateCustomerTruckCapacityMapping(employeeDto.PlantTruckCapacities, employeeDto.DepotTruckCapacities, user.Id, employeeDto.LoginUserId);

                    _emamiContext.SaveChanges();

                    if (employeeDto.BrokerId > 0)
                    {
                        var userCustomerContext = new UserCustomerMapping
                        {
                            UserId = employeeDto.BrokerId,
                            CustomerId = user.Id,
                            CreatedBy = employeeDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                        _emamiContext.SaveChanges();
                    }

                    //if it is dealer 
                    if (employeeDto.BrokerIds.IsAny())
                    {
                        foreach (var data in employeeDto.BrokerIds)
                        {
                            var userCustomerContext = new UserCustomerMapping
                            {
                                UserId = data,
                                CustomerId = user.Id,
                                CreatedBy = employeeDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                        }
                        _emamiContext.SaveChanges();
                    }

                    //}
                }
                #endregion

                #region BrokerDepotMapping & BrokerDealerMapping

                if (employeeDto.RoleId == (int)DTO.Enums.Role.Broker)
                {

                    //Depot mapping
                    //CreateUserDepotMapping(employeeDto.SelectedDepotIds, user.Id, employeeDto.LoginUserId);
                    //Plant mapping
                    //CreateUserDepotMapping(employeeDto.SelectedPlantIds, user.Id, employeeDto.LoginUserId);
                    //Customer Truck capacity mapping
                    CreateCustomerTruckCapacityMapping(employeeDto.PlantTruckCapacities, employeeDto.DepotTruckCapacities, user.Id, employeeDto.LoginUserId);
                    _emamiContext.SaveChanges();

                    if (employeeDto.SelectedDealerIds != null && employeeDto.SelectedDealerIds.Any())
                    {
                        foreach (var customerId in employeeDto.SelectedDealerIds)
                        {
                            var userCustomerContext = new UserCustomerMapping
                            {
                                UserId = user.Id,
                                CustomerId = customerId,
                                CreatedBy = employeeDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                        }
                        _emamiContext.SaveChanges();
                    }
                }

                #endregion

                #region DealerBrokerMapping / UserCustomerMapping
                var customermaperrormsg = "";
                if (employeeDto.RoleId == (int)DTO.Enums.Role.StateTrader || employeeDto.RoleId == (int)DTO.Enums.Role.KAM)
                {
                    if (employeeDto.SelectedDealerBrokerIds != null && employeeDto.SelectedDealerBrokerIds.Any())
                    {
                        // var bdoComb = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == user.Id);
                        foreach (var customerId in employeeDto.SelectedDealerBrokerIds)
                        {
                            //var divcomb = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == customerId).ToList();
                            //var bdoIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == customerId).Select(s => s.UserId).ToList();
                            //var dealerData = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == customerId);

                            var isExistCustomer = _emamiContext.UserCustomerMapping.FirstOrDefault(f => f.UserId == user.Id && f.CustomerId == customerId);
                            if (isExistCustomer == null)
                            {
                                //if (bdoIds.IsAny())
                                //{
                                //var isExistBdoCombination=(from div in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                           join ur in _emamiContext.UserRoles.AsNoTracking() on div.UserId equals ur.UserId
                                //                           join bdiv in bdoComb on new { SalesOrganizationId = div.SalesOrganizationId, DistributionChannelId = div.DistributionChannelId, DivisionId = div.DivisionId }
                                //       equals new { SalesOrganizationId = bdiv.SalesOrganizationId, DistributionChannelId = bdiv.DistributionChannelId, DivisionId = bdiv.DivisionId }
                                //                           where bdoIds.Contains(div.UserId)
                                //                           && ur.RoleId==7
                                //                           && div.UserId !=user.Id
                                //                           select div.UserId
                                //                           );

                                //if (isExistBdoCombination.IsAny())
                                //{
                                //    customermaperrormsg = customermaperrormsg + dealerData.Name + ",";
                                //}
                                //else
                                //{
                                var userCustomerContext = new UserCustomerMapping
                                {
                                    UserId = user.Id,
                                    CustomerId = customerId,
                                    CreatedBy = employeeDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                };
                                _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                            }
                            //    }
                            //}
                            //else
                            //{
                            //var isExistBdoCombination= (from div in divcomb
                            //                            join bdiv in bdoComb on new { SalesOrganizationId = div.SalesOrganizationId, DistributionChannelId = div.DistributionChannelId, DivisionId = div.DivisionId }
                            //       equals new { SalesOrganizationId = bdiv.SalesOrganizationId, DistributionChannelId = bdiv.DistributionChannelId, DivisionId = bdiv.DivisionId }
                            //       select div.UserId

                            //                            ).FirstOrDefault();
                            //if (isExistBdoCombination!=null)
                            //{
                            //    var userCustomerContext = new UserCustomerMapping
                            //    {
                            //        UserId = user.Id,
                            //        CustomerId = customerId,
                            //        CreatedBy = employeeDto.LoginUserId,
                            //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            //    };
                            //    _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                            //}
                            //else
                            //{
                            //    customermaperrormsg = customermaperrormsg + dealerData.Name + ",";
                            //}
                        }

                    }
                    _emamiContext.SaveChanges();
                }



                //if (employeeDto.RoleId == (int)DTO.Enums.Role.StateTrader || employeeDto.RoleId == (int)DTO.Enums.Role.KAM)
                //{
                //    if (employeeDto.SelectedDealerBrokerIds != null && employeeDto.SelectedDealerBrokerIds.Any())
                //    {
                //        foreach (var customerId in employeeDto.SelectedDealerBrokerIds)
                //        {
                //            var userCustomerContext = new UserCustomerMapping
                //            {
                //                UserId = user.Id,
                //                CustomerId = customerId,
                //                CreatedBy = employeeDto.LoginUserId,
                //                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                //            };
                //            _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                //        }
                //        _emamiContext.SaveChanges();
                //    }
                //}

                #endregion

                #region ShipToParty Depot,Plant, Broker Mapping

                if (employeeDto.RoleId == (int)DTO.Enums.Role.ShipToParty)
                {
                    //Depot mapping
                    //CreateUserDepotMapping(employeeDto.SelectedDepotIds, user.Id, employeeDto.LoginUserId);

                    //Plant mapping
                    CreateUserDepotMapping(employeeDto.SelectedPlantIds, user.Id, employeeDto.LoginUserId);
                    CreateCustomerTruckCapacityMapping(employeeDto.PlantTruckCapacities, employeeDto.DepotTruckCapacities, user.Id, employeeDto.LoginUserId);
                    _emamiContext.SaveChanges();

                    if (employeeDto.BrokerId > 0)
                    {
                        var userCustomerContext = new UserCustomerMapping
                        {
                            UserId = employeeDto.BrokerId,
                            CustomerId = user.Id,
                            CreatedBy = employeeDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                        _emamiContext.SaveChanges();
                    }
                }
                #endregion


                var finalerrormsg = "";
                if (customermaperrormsg != "")
                {
                    finalerrormsg = finalerrormsg + Constants.BDOCompExist.Replace("Distributor", customermaperrormsg);
                }


                if (reporterrormsg != "")
                {
                    finalerrormsg = finalerrormsg + Constants.UserCompExist.Replace("Users", reporterrormsg);
                }

                if (finalerrormsg != "")
                {
                    return _resultService.ErrorMessage(finalerrormsg);
                }


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

        public void CreateUserDepotMapping(List<long> Ids, long userId, long loginUserId)
        {
            try
            {
                if (Ids != null && Ids.Any())
                {
                    foreach (var depotId in Ids)
                    {
                        var userCustomerContext = new UserDepotMapping
                        {
                            UserId = userId,
                            DepotId = depotId,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = loginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.UserDepotMapping.Add(userCustomerContext);
                    }
                    _emamiContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddOrReplaceUserDivisionPlantMappings(long userDivisionId, List<long> plantIds, long userId)
        {
            // 1. Delete existing plant mappings
            var existing = _emamiContext.UserDivisionDepotMappings
                .Where(p => p.UserDivisionId == userDivisionId)
                .ToList();

            foreach(var plant in existing)
            {
                _emamiContext.UserDivisionDepotMappings.Remove(plant);
            }

            // 2. Add new mappings
            if (plantIds != null)
            {
                foreach (var plantId in plantIds)
                {
                    _emamiContext.UserDivisionDepotMappings.Add(new UserDivisionDepotMapping
                    {
                        UserDivisionId = userDivisionId,
                        DepotId = plantId,
                        CreatedBy = userId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        ModifiedBy = userId,
                        ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    });
                }
            }
        }

        public string GetUserDivisionPlantCodes(long userDivisionId)
        {
            // Step 1: Get all plant IDs from mapping table
            var plantIds = _emamiContext.UserDivisionDepotMappings
                .Where(p => p.UserDivisionId == userDivisionId)
                .Select(p => p.DepotId)
                .ToList();

            if (!plantIds.Any())
                return string.Empty;

            // Step 2: Fetch PlantCodes from Plants table
            var plantCodes = _emamiContext.Depots
                .Where(pl => plantIds.Contains(pl.Id))
                .Select(pl => pl.Code)
                .ToList();

            // Step 3: Return CSV string
            return string.Join(",", plantCodes);
        }



        public void UpdateUserDepotMapping(List<long> Ids, long userId, long loginUserId)
        {
            try
            {
                if (Ids != null && Ids.Any())
                {
                    var depotExistList = _emamiContext.UserDepotMapping.Where(f => f.UserId == userId);
                    var depotIds = depotExistList.Select(s => s.DepotId).ToList();

                    var newIds = depotExistList.Where(w => !Ids.Contains(w.DepotId));
                    var removedIds = Ids.Where(w => !depotIds.Contains(w)).ToList();

                    if (removedIds != null)
                    {
                        foreach (var depotId in removedIds)
                        {
                            var userDepotMapping = _emamiContext.UserDepotMapping.FirstOrDefault(f => f.UserId == userId && f.DepotId == depotId);
                            _emamiContext.UserDepotMapping.Remove(userDepotMapping);
                        }
                    }

                    if (newIds != null && newIds.Any())
                    {
                        foreach (var depot in newIds)
                        {
                            var userCustomerContext = new UserDepotMapping
                            {
                                UserId = userId,
                                DepotId = depot.DepotId,
                                CreatedBy = loginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.UserDepotMapping.Add(userCustomerContext);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateCustomerShipToPartyMapping(List<long> shipToPartyIds, long customerId, long loginUserId)
        {
            try
            {
                if (shipToPartyIds != null && shipToPartyIds.Any())
                {
                    foreach (var shipToPartyId in shipToPartyIds)
                    {
                        var customerShipToPartyContext = new CustomerShipToPartyMapping
                        {
                            CustomerId = customerId,
                            ShipToPartyId = shipToPartyId,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = loginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CustomerShipToPartyMappings.Add(customerShipToPartyContext);
                    }
                    _emamiContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerShipToPartyMapping(List<long> shipToPartyIds, long customerId, long loginUserId)
        {
            try
            {
                var existingShipToPartyContext = _emamiContext.CustomerShipToPartyMappings.Where(_ => _.CustomerId == customerId);
                if (existingShipToPartyContext != null && existingShipToPartyContext.Any())
                {
                    foreach (var existingShipToParty in existingShipToPartyContext)
                    {
                        _emamiContext.CustomerShipToPartyMappings.Remove(existingShipToParty);
                    }
                    _emamiContext.SaveChanges();
                }
                if (shipToPartyIds != null && shipToPartyIds.Any())
                {
                    foreach (var shipToPartyId in shipToPartyIds)
                    {
                        var customerShipToPartyContext = new CustomerShipToPartyMapping
                        {
                            CustomerId = customerId,
                            ShipToPartyId = shipToPartyId,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = loginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CustomerShipToPartyMappings.Add(customerShipToPartyContext);
                    }
                    _emamiContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateCustomerTruckCapacityMapping(string plantTruckCapacity, string depotTruckCapacity, long customerId, long loginUserId)
        {
            try
            {
                if (plantTruckCapacity != null && depotTruckCapacity != null)
                {
                    List<decimal> plantcapacity = plantTruckCapacity.Split(',').Select(s => Convert.ToDecimal(s)).ToList();
                    List<decimal> depotcapacity = depotTruckCapacity.Split(',').Select(s => Convert.ToDecimal(s)).ToList();
                    foreach (var capacity in plantcapacity)
                    {
                        var customerTruckCapacityContext = new CustomerTruckCapacityMapping
                        {
                            UserId = customerId,
                            TruckCapacity = capacity,
                            StorageTypeId = (int)DTO.Enums.StorageType.Plant,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = loginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CustomerTruckCapacityMapping.Add(customerTruckCapacityContext);
                    }
                    _emamiContext.SaveChanges();
                    foreach (var capacity in depotcapacity)
                    {
                        var customerTruckCapacityContext = new CustomerTruckCapacityMapping
                        {
                            UserId = customerId,
                            TruckCapacity = capacity,
                            StorageTypeId = (int)DTO.Enums.StorageType.Depot,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = loginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CustomerTruckCapacityMapping.Add(customerTruckCapacityContext);
                    }
                    _emamiContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerTruckCapacityMapping(string plantTruckCapacity, string depotTruckCapacity, long customerId, long loginUserId)
        {
            try
            {
                var existingTruckCapacityContext = _emamiContext.CustomerTruckCapacityMapping.Where(_ => _.UserId == customerId);
                if (existingTruckCapacityContext != null && existingTruckCapacityContext.Any())
                {
                    foreach (var existingTruckCapacity in existingTruckCapacityContext)
                    {
                        _emamiContext.CustomerTruckCapacityMapping.Remove(existingTruckCapacity);
                    }
                    _emamiContext.SaveChanges();
                }
                if (plantTruckCapacity != null && depotTruckCapacity.Any())
                {
                    List<decimal> plantcapacity = plantTruckCapacity.Split(',').Select(s => Convert.ToDecimal(s)).ToList();
                    List<decimal> depotcapacity = depotTruckCapacity.Split(',').Select(s => Convert.ToDecimal(s)).ToList();
                    foreach (var capacity in plantcapacity)
                    {
                        var customerTruckCapacityContext = new CustomerTruckCapacityMapping
                        {
                            UserId = customerId,
                            TruckCapacity = capacity,
                            StorageTypeId = (int)DTO.Enums.StorageType.Plant,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = loginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CustomerTruckCapacityMapping.Add(customerTruckCapacityContext);
                    }
                    _emamiContext.SaveChanges();
                    foreach (var capacity in depotcapacity)
                    {
                        var customerTruckCapacityContext = new CustomerTruckCapacityMapping
                        {
                            UserId = customerId,
                            TruckCapacity = capacity,
                            StorageTypeId = (int)DTO.Enums.StorageType.Depot,
                            CreatedBy = loginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = loginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CustomerTruckCapacityMapping.Add(customerTruckCapacityContext);
                    }
                    _emamiContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method to Get User Master List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetUserMasterList(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserMasterList";
            var resultDto = new ResultDto();
            var userMasterDto = new List<UserMasterDto>();
            try
            {
                var userList =
                    _emamiContext.UserRoles.AsNoTracking().Where(_ =>
                      _.RoleId != (int)DTO.Enums.Role.Dealer && _.RoleId != (int)DTO.Enums.Role.Broker && _.RoleId != (int)DTO.Enums.Role.ShipToParty && _.RoleId != (int)DTO.Enums.Role_CMS.Demonstrator && _.RoleId != (int)DTO.Enums.Role.Admin)
                    .Select(_ => _.User);

                List<User> entity;
                var divmap = _emamiContext.UserDivisionMappings.Select(_ => _.UserId).ToList();
                if (inputDto.IsToReturnInactiveData)
                {

                    //entity = userList.AsNoTracking().Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0));
                    entity = userList.AsNoTracking().ToList();
                }
                else
                {
                    //entity = userList.AsNoTracking().Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0) && _.IsActive);
                    entity = userList.AsNoTracking().Where(_ => _.IsActive).ToList();
                }
                var zoneList = _emamiContext.Zones.AsNoTracking().ToList();
                userMasterDto = entity.AsEnumerable().Select(c => new UserMasterDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = c.Id,
                    EmployeeCode = c.Code,
                    EmployeeName = c.Name,
                    Branch = c.Branch,
                    //VerticalId = c.DivisionId,
                    //Vertical = c.Division != null ? c.Division.Name : string.Empty,
                    //ReportingTo = c.ReportingTo,
                    OrganizationReportingToId = c.ReportingToId,
                    SalesReportingToId = c.ReportingToId,
                    //SpecialityFatReportingToId = c.SpecialityFatReportingToId,
                    Email = c.Email,
                    MobileNumber = c.MobileNumber,
                    SalesAccess = c.SalesAccess,
                    Designation = c.Designation,
                    //HeadquartersId = c.HeadquartersId,
                    // Headquarters = c.Headquarters?.Name,
                    State = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == c.StateId)?.StateName,
                    //Territory = c.Territory,
                    Zone = zoneList.FirstOrDefault(_ => _.Id==c.ZoneId)!=null ? zoneList.FirstOrDefault(_ => _.Id == c.ZoneId).Name:String.Empty,
                    Acedns = c.Acedns,
                    District = _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == c.DistrictId)?.DistrictName,
                    IsActive = c.IsActive,
                    Pincode = c.Pincode,
                    RoleName = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == c.Id).Role.Name,
                    SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                    // SaudaBookingType = c.SaudaBookingType != null ? c.SaudaBookingType.Name : string.Empty,
                    CompanyCode = c.CompanyCode == null ? String.Empty : c.CompanyCode
                }).ToList();
                var divisionContext = _emamiContext.UserDivisionMappings.AsQueryable();
                userMasterDto.ForEach(item =>
                {
                    var div = divisionContext.Where(_ => _.UserId == item.Id);
                    item.Vertical = div != null ? String.Join(",", div.Select(s => s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code)) : String.Empty;
                });

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userMasterDto != null ? userMasterDto.OrderByDescending(_ => _.Id).ToList() : userMasterDto;
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
        /// Method to get Get User Details By Id
        /// </summary>
        /// <param name="brokerId"></param>
        /// <returns></returns>
        public ResultDto GetUserDetailsById(string userId)
        {
            _methodName = "GetUserDetailsById";
            var resultDto = new ResultDto();
            var employeeDto = new EmployeeDto();
            try
            {
                userId = userId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(userId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    var userDivisionMapping = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == Id).Select(_ => new DivisionDetailsDto
                    {

                        DistributionChannel = _.DistributionChannel.Name,
                        DistributionChannelId = _.DistributionChannelId,
                        Division = _.Division.Name,
                        DivisionId = _.DivisionId,
                        SalesOrganization = _.SalesOrganization.Name,
                        SalesOrganizationId = _.SalesOrganizationId,
                        SaudaLimit = (decimal?)_.SaudaLimit ?? 0
                    });
                    var userReportingToIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == Id).Select(s => s.ReportingToUserId).ToList();

                    employeeDto.SelectedReportingToIds = userReportingToIds;

                    employeeDto.EncryptedId = userId;
                    employeeDto.DivisionList = userDivisionMapping.ToList();
                    employeeDto.Id = resultContext.Id;
                    employeeDto.Code = resultContext.Code;
                    employeeDto.Name = resultContext.Name;
                    employeeDto.MobileNumber = resultContext.MobileNumber;
                    employeeDto.Email = resultContext.Email;
                    employeeDto.IsActive = resultContext.IsActive;
                    employeeDto.DistrictId = resultContext.DistrictId;
                    //  employeeDto.Zone = resultContext.Zone?.Name;
                    employeeDto.ZoneId = resultContext.ZoneId;
                    employeeDto.District = resultContext.District;
                    employeeDto.StateId = resultContext.StateId;
                    employeeDto.State = resultContext.State;
                    employeeDto.TerritoryId = resultContext.TerritoryId;
                    employeeDto.Territory = resultContext.Territory;
                    employeeDto.City = resultContext.City;
                    employeeDto.CityId = resultContext.CityId;
                    //employeeDto.Address = resultContext.Address1;
                    employeeDto.Address1 = resultContext.Address1;
                    employeeDto.Address2 = resultContext.Address2;
                    //  employeeDto.Zone = resultContext.Zone?.Name;
                    employeeDto.Acedns = resultContext.Acedns;
                    employeeDto.Branch = resultContext.Branch;
                    employeeDto.SalesAccess = resultContext.SalesAccess;
                    employeeDto.Designation = resultContext.Designation;
                    employeeDto.HeadquartersId = resultContext.HeadquartersId;
                    //  employeeDto.Headquarters = resultContext.Headquarters?.Name;
                    //employeeDto.VerticalId = resultContext.DivisionId;
                    employeeDto.Pincode = resultContext.Pincode;
                    employeeDto.AdditionalMobileNumber = resultContext.AdditionalMobileNumber;
                    employeeDto.CompanyCode = resultContext.CompanyCode == null ? string.Empty : resultContext.CompanyCode;

                    //employeeDto.CustomerGroupOneId = resultContext.CustomerGroupOneId;
                    //employeeDto.CustomerGroupTwoId = resultContext.CustomerGroupTwoId;
                    if (!string.IsNullOrEmpty(resultContext.Password))
                    {
                        employeeDto.Password = UtilityHelper.ConvertMd5ToString(resultContext.Password, SecurityConstants.EncryptionKey);
                    }
                    employeeDto.SaudaBookingTypeId = resultContext.SaudaBookingTypeId;
                    var userroleContext = _emamiContext.UserRoles.AsNoTracking();

                    employeeDto.Role = userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id) != null && userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role != null ? userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role.Name : string.Empty;
                    employeeDto.RoleId = userroleContext.FirstOrDefault(f => f.UserId == Id).RoleId;
                    employeeDto.SelectedDealerBrokerIds = GetUserCustomerIds(Id);
                    employeeDto.SelectedDealerBrokerIdsCount = 0;
                    if (employeeDto.SelectedDealerBrokerIds != null && employeeDto.SelectedDealerBrokerIds.Any())
                    {
                        var userIds = _emamiContext.Users.Where(_ => employeeDto.SelectedDealerBrokerIds.Contains(_.Id)).Select(_ => _.Id).ToList();

                        employeeDto.SelectedDealerBrokerIdsCount = userIds.Any() ? userIds.Count : 0;
                    }

                    //employeeDto.ReportingToId = resultContext.ReportingToId;
                    employeeDto.ReportingToId = resultContext.ReportingToId;
                    //employeeDto.SpecialityFatReportingToId = resultContext.SpecialityFatReportingToId;

                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = employeeDto;
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

        public List<long> GetUserCustomerIds(long userId)
        {
            var customerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                .Where(w => w.UserId == userId)
                .Select(s => s.CustomerId).ToList();
            return customerIds;
        }

        public List<long> GetUserDepotIds(long userId)
        {
            var customerIds = _emamiContext.UserDepotMapping.AsNoTracking()
                .Where(w => w.UserId == userId)
                .Select(s => s.DepotId).ToList();
            return customerIds;
        }

        public List<long> GetCustomerShipToParyIds(long customerId)
        {
            var shipToPartyIds = _emamiContext.CustomerShipToPartyMappings.AsNoTracking()
                .Where(w => w.CustomerId == customerId)
                .Select(s => s.ShipToPartyId).ToList();
            return shipToPartyIds;
        }

        /// <summary>
        /// Method to get Get User Details By Id
        /// </summary>
        /// <param name="brokerId"></param>
        /// <returns></returns>
        public ResultDto GetUserDetails(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserDetails";
            var resultDto = new ResultDto();
            var employeeDto = new EmployeeDto();
            try
            {
                var resultContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (resultContext != null)
                {
                    var userroleContext = _emamiContext.UserRoles.AsNoTracking();

                    employeeDto.Id = resultContext.Id;
                    employeeDto.Code = resultContext.Code;
                    employeeDto.Name = resultContext.Name;
                    employeeDto.MobileNumber = resultContext.MobileNumber;
                    employeeDto.Email = resultContext.Email;
                    employeeDto.IsActive = resultContext.IsActive;
                    employeeDto.District = resultContext.District;
                    employeeDto.State = resultContext.State;
                    employeeDto.Address1 = resultContext.Address1;
                    employeeDto.Address2 = resultContext.Address2;
                    //  employeeDto.Zone = resultContext.Zone?.Name;
                    employeeDto.Acedns = resultContext.Acedns;
                    employeeDto.Branch = resultContext.Branch;
                    //employeeDto.ReportingTo = resultContext.ReportingTo;
                    employeeDto.ReportingToId = resultContext.ReportingToId;
                    employeeDto.ReportingToId = resultContext.ReportingToId;
                    //employeeDto.SpecialityFatReportingToId = resultContext.SpecialityFatReportingToId;
                    employeeDto.SalesAccess = resultContext.SalesAccess;
                    employeeDto.Designation = resultContext.Designation;
                    employeeDto.HeadquartersId = resultContext.HeadquartersId;
                    // employeeDto.Headquarters = resultContext.Headquarters?.Name;
                    //employeeDto.VerticalId = resultContext.DivisionId;
                    employeeDto.Pincode = resultContext.Pincode;
                    //employeeDto.FreightZoneId = resultContext.FreightZoneId;
                    //employeeDto.FreightRouteId = resultContext.FreightRouteId;
                    ////employeeDto.FreightZone = resultContext.FreightZone != null ? resultContext.FreightZone.Name : string.Empty;
                    //employeeDto.FreightRoute = resultContext.FreightRoute != null ? resultContext.FreightRoute.Name : string.Empty;
                    employeeDto.SaudaBookingTypeId = resultContext.SaudaBookingTypeId;
                    //employeeDto.PlantTruckCapacity = resultContext.Loadability;
                    //employeeDto.DepotTruckCapacity = resultContext.DepotLoadability;
                    employeeDto.Role = userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id) != null && userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role != null ? userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role.Name : string.Empty;
                    employeeDto.RoleId = userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id) != null ? userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).RoleId : 0;
                    employeeDto.AdditionalMobileNumber = resultContext.AdditionalMobileNumber;
                    employeeDto.CompanyCode = resultContext.CompanyCode == null ? string.Empty : resultContext.CompanyCode;
                    //employeeDto.FormUsers = _emamiContext.FormUsers.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && _.IsActive)
                    //                                                                        .Select(_ => new FormDto()
                    //                                                                        {
                    //                                                                            FormId = _.FormId,
                    //                                                                            FormName = _.Form != null ? _.Form.Name : string.Empty
                    //                                                                        }).ToList();

                    employeeDto.PlantTruckCapacityList = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(_ => _.StorageTypeId == (int)DTO.Enums.StorageType.Plant && _.UserId == inputDto.LoginUserId).Select(s => s.TruckCapacity).ToList();
                    employeeDto.DepotTruckCapacityList = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(_ => _.StorageTypeId == (int)DTO.Enums.StorageType.Depot && _.UserId == inputDto.LoginUserId).Select(s => s.TruckCapacity).ToList();
                    employeeDto.LineId = resultContext.LineId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList();
                }

                //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.RoleId == (int)DTO.Enums.Role.Dealer);
                //if (userRoleContext != null)
                //{
                //    var saudaLimitContext = _emamiContext.SaudaLimit.AsNoTracking().OrderByDescending(_ => _.CreatedDate).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                //    if (saudaLimitContext != null)
                //    {
                //        var available = (decimal)0;
                //        var saudaOrderContext = (from sauda in _emamiContext.Sauda
                //                                 join saudaOrder in _emamiContext.SaudaOrders on sauda.Id equals saudaOrder.SaudaId
                //                                 where sauda.UserId == inputDto.LoginUserId
                //                                 select saudaOrder);
                //        if (saudaOrderContext != null)
                //        {
                //            available = saudaOrderContext.ToList().Sum(_ => _.BidQuantity);
                //        }
                //        employeeDto.AvailableSaudaLimit = saudaLimitContext.ActualLimit - available;
                //    }
                //}

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = employeeDto;
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
        /// Method to Update User
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public ResultDto UpdateUser(EmployeeDto employeeDto)
        {
            _methodName = "UpdateUser";
            var resultDto = new ResultDto();
            long oldSaudaBookingTypeId = 0;
            string LineIds = null;
            try
            {
                if (employeeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (!String.IsNullOrEmpty(employeeDto.EncryptedId))
                {
                    employeeDto.EncryptedId = employeeDto.EncryptedId.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(employeeDto.EncryptedId, SecurityConstants.EncryptionKey);

                    employeeDto.Id = UtilityHelper.LongTryToParse(decryptedId);
                }

                if (string.IsNullOrEmpty(employeeDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }


                if (!employeeDto.IsFromMobile)
                {
                    if (!employeeDto.IsActive && (employeeDto.RoleId == (int)DTO.Enums.Role.Dealer || employeeDto.RoleId == (int)DTO.Enums.Role.ShipToParty))
                    {
                        if (employeeDto.InActiveRemarkId == 0 || employeeDto.InActiveRemarkId == null)
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = Constants.InActiveRemarksIsEmpty;
                            return resultDto;
                        }
                    }
                }


                //Get Common user datas for validation properties only
                var userDatas = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                           .Where(_ => //_.User.DivisionId == employeeDto.VerticalId
                            _.User.IsActive == employeeDto.IsActive
                           && _.UserRole.RoleId == employeeDto.RoleId)
                           .Select(s => new
                           {
                               Id = s.User.Id,
                               Name = s.User.Name,
                               Code = s.User.Code,
                               ShipToPartyCode = s.User.ShipToPartyCode,
                               Email = s.User.Email,
                               MobileNumber = s.User.MobileNumber,
                               RoleId = s.UserRole.RoleId
                           });
                if (userDatas == null && !userDatas.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                //If the user role has been ShipToParty [Code] property only validates for a duplicate.
                if (employeeDto.RoleId == (int)DTO.Enums.Role.ShipToParty)
                {
                    //User Code Validation
                    var codeExist = userDatas.Any(_ => _.ShipToPartyCode == employeeDto.Code && _.Id != employeeDto.Id && _.RoleId == employeeDto.RoleId && !string.IsNullOrEmpty(employeeDto.Code));
                    if (codeExist)
                        return _resultService.ErrorMessage(Constants.CodeExist);
                }
                else if (employeeDto.RoleId != (int)DTO.Enums.Role.Dealer && employeeDto.RoleId != (int)DTO.Enums.Role.ShipToParty && employeeDto.RoleId != (int)DTO.Enums.Role.Broker)
                {
                    //ReportingTo UserCheck
                    if (!employeeDto.SelectedReportingToIds.IsAny())
                        return _resultService.ErrorMessage(Constants.ReportingToUserNotSelected);
                }
                else
                {
                    //User Name Validation
                    //var isNameExist = userDatas.Any(_ => _.Name == employeeDto.Name && _.Id != employeeDto.Id && _.RoleId == employeeDto.RoleId && !string.IsNullOrEmpty(employeeDto.Name));
                    //if (isNameExist)
                    //    return _resultService.ErrorMessage(Constants.NameExist);
                    

                    //User Code Validation
                    var isCodeExist = userDatas.Any(_ => _.Code == employeeDto.Code && _.Id != employeeDto.Id && _.RoleId == employeeDto.RoleId && !string.IsNullOrEmpty(employeeDto.Code));
                    if (isCodeExist)
                        return _resultService.ErrorMessage(Constants.CodeExist);


                    //User Email Validation
                    //var isEmailExist = userDatas.Any(_ => _.Email == employeeDto.Email && _.Id != employeeDto.Id && _.RoleId == employeeDto.RoleId && !string.IsNullOrEmpty(employeeDto.Email));
                    //if (isEmailExist)
                    //    return _resultService.ErrorMessage(Constants.EmailExists);


                    //User MobileNumber Validation
                    //var isMobileNumberExist = userDatas.Any(_ => _.MobileNumber == employeeDto.MobileNumber && _.Id != employeeDto.Id && _.RoleId == employeeDto.RoleId && !string.IsNullOrEmpty(employeeDto.MobileNumber));
                    //if (isMobileNumberExist)
                    //    return _resultService.ErrorMessage(Constants.MobileNumberExist);

                }

                var user = _emamiContext.Users.FirstOrDefault(s => s.Id == employeeDto.Id);
                if (user.Password != null)
                {
                    var decryptedPassword = UtilityHelper.ConvertMd5ToString(user.Password, SecurityConstants.EncryptionKey);
                    //checking whether password changed
                    if (employeeDto.Password != decryptedPassword)
                    {
                        user.PasswordModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    }
                }

                oldSaudaBookingTypeId = user.SaudaBookingTypeId ?? 0;
                if (!string.IsNullOrEmpty(employeeDto.Password))
                {
                    user.Password = UtilityHelper.ConvertToMd5(employeeDto.Password, SecurityConstants.EncryptionKey);
                }

                user.Name = employeeDto.Name;

                if (employeeDto.RoleId == (int)DTO.Enums.Role.ShipToParty)
                {
                    user.ShipToPartyCode = employeeDto.Code;
                    user.Code = String.Empty;
                }
                else
                {
                    user.Code = employeeDto.Code;
                }

                if(employeeDto.LineId!=null && employeeDto.LineId.Any())
                {
                    LineIds = string.Join(",", employeeDto.LineId);
                }

                if (employeeDto.RoleId == (int)DTO.Enums.Role.Dealer)
                {
                    if (!string.IsNullOrWhiteSpace(employeeDto.TANNumber))
                    {
                        var existingUser = _emamiContext.Users.AsNoTracking().Where(c => c.TANNumber == employeeDto.TANNumber && c.Code != employeeDto.Code).ToList();

                        if (existingUser != null && existingUser.Any())
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = string.Format(Constants.TannumberExist, string.Join(",", existingUser.Select(c => c.Name)));
                            return resultDto;
                        }
                    }
                }

                user.Email = employeeDto.Email;
                user.MobileNumber = employeeDto.MobileNumber;
                user.OtpNumber = employeeDto.OtpNumber;
                //RoleId = employeeDto.RoleId;
                //user.UserCode = employeeDto.UserCode;
                user.PushTokenKey = employeeDto.Name;
                //user.ReportingToId = employeeDto.ReportingToId;
                //user.ReportingToId = employeeDto.ReportingToId;
                //user.SpecialityFatReportingToId = employeeDto.SpecialityFatReportingToId;
                //user.FreightRouteId = employeeDto.FreightRouteId;
                //user.FreightZoneId = employeeDto.FreightZoneId;
                user.Remarks = employeeDto.Remarks;
                user.LastLoggedInDate = employeeDto.LastLoggedInDate;
                user.PreviousLoggedInDate = employeeDto.PreviousLoggedInDate;
                user.ApprovedBy = employeeDto.ApprovedBy;
                user.ApprovedDate = employeeDto.ApprovedDate;
                user.IsActive = employeeDto.IsActive;
                user.IsBlacklisted = employeeDto.IsBlacklisted;
                user.ImageUrl = employeeDto.ImageUrl;
                user.ParentUserId = employeeDto.ParentUserId;
                user.RegistrationTypeId = employeeDto.RegistrationTypeId;
                user.Region = employeeDto.Region;
                user.Pincode = employeeDto.Pincode;
                user.Street = employeeDto.Street;
                user.DistrictId = employeeDto.DistrictId;
                user.District = employeeDto.District;
                user.CityId = employeeDto.CityId;
                user.City = employeeDto.City;
                user.StateId = employeeDto.StateId;
                user.State = employeeDto.State;
                //user.TerritoryId = employeeDto.TerritoryId;
                //user.Territory = employeeDto.Territory;
                user.ExecutivePassword = employeeDto.ExecutivePassword;
                user.McsNo = employeeDto.McsNo;
                //user.HeadquartersId = employeeDto.HeadquartersId;
                //user.MobileNumber1 = employeeDto.MobileNumber1;
                user.MobileNumber2 = employeeDto.MobileNumber2;
                //user.AddressLine1 = employeeDto.AddressLine1;
                //user.AddressLine2 = employeeDto.AddressLine2;
                //user.AddressLine3 = employeeDto.AddressLine3;
                user.GSTN = employeeDto.GSTN;
                user.TANNumber = employeeDto.TANNumber;
                user.VisitDay = employeeDto.VisitDay;
                //user.SaudaValidityPeriod = employeeDto.SaudaValidityPeriod;
                user.WeeklyClosingDay = employeeDto.WeeklyClosingDay;
                user.MonthlyPotential = employeeDto.MonthlyPotential;
                //user.Loadability = employeeDto.PlantTruckCapacity;
                user.Address1 = employeeDto.Address1;
                user.Address2 = employeeDto.Address2;
                user.CustClass = employeeDto.CustClass;
                user.Branch = employeeDto.Branch;
                //user.ReportingTo = employeeDto.ReportingTo;
                user.SalesAccess = employeeDto.SalesAccess;
                user.Designation = employeeDto.Designation;
                //user.HeadquartersId = employeeDto.HeadquartersId;
                //user.Address1 = employeeDto.Address;
                user.ZoneId = employeeDto.ZoneId;
                user.Acedns = employeeDto.Acedns;
                user.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                //user.IncoTermsId = employeeDto.IncoTermsId;
                user.TransportModeId = employeeDto.TransportModeId;
                //user.DivisionId = employeeDto.VerticalId;
                //user.SaudaLimit = employeeDto.SaudaLimit;
                user.IsSelf = employeeDto.IsSelf;
                user.IsBroker = employeeDto.IsBroker;
                user.ModifiedBy = employeeDto.LoginUserId;
                user.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //user.DepotLoadability = employeeDto.DepotTruckCapacity;
                user.Latitude = employeeDto.Latitude;
                user.Longitude = employeeDto.Longitude;
                user.CustomerGroupFiveId = employeeDto.CustomerGroupFiveId;
                user.CompanyCode = employeeDto.CompanyCode == null ? string.Empty : employeeDto.CompanyCode;
                //user.Password = employeeDto.Password;
                user.LineId = LineIds;

                if (employeeDto.IsActive)
                {
                    user.InActiveRemarkId = null;
                }
                else
                {
                    user.InActiveRemarkId = employeeDto.InActiveRemarkId;
                }
                //user.CustomerGroupOneId = employeeDto.CustomerGroupOneId;
                //user.CustomerGroupTwoId = employeeDto.CustomerGroupTwoId;
                user.CustomerGroupFiveId = employeeDto.CustomerGroupFiveId;
                user.AdditionalMobileNumber = employeeDto.AdditionalMobileNumber;
                user.ContactPersonName = employeeDto.ContactPersonName;
                //if (employeeDto.Attachments.IsAny())
                //{
                user.IsActiveForCall = employeeDto.IsActiveForCall;
                //}
                var divisionMappings = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == employeeDto.Id).ToList();
                if (divisionMappings.Count == 0)
                {
                    foreach (var division in employeeDto.DivisionList)
                    {
                        var userDivMap = new UserDivisionMapping
                        {
                            UserId = user.Id,
                            SalesOrganizationId = division.SalesOrganizationId,
                            DistributionChannelId = division.DistributionChannelId,
                            DivisionId = division.DivisionId,
                            CreatedBy = employeeDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = employeeDto.LoginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            SaudaLimit = division.SaudaLimit,
                            SaudaValidityPeriod = division.SaudaValidityPeriod,
                        };
                        _emamiContext.UserDivisionMappings.Add(userDivMap);
                        _emamiContext.SaveChanges();  // Need SaveChanges to get userDivMap.Id

                        //DELETE + INSERT PLANT MAPPINGS
                        AddOrReplaceUserDivisionPlantMappings(userDivMap.Id, division.UserDivisionPlantIds, user.Id);
                    }
                }
                else
                {
                    var userDivisions = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == employeeDto.Id).ToList();
                    var userDivisionsID = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == employeeDto.Id).Select(_ => _.DivisionId).ToList();
                    var inputDivisions = employeeDto.DivisionList.Select(_ => _.DivisionId).ToList();
                    foreach (var division in userDivisions)
                    {
                        if (!inputDivisions.Contains(division.DivisionId))
                        {
                            var existingPlants = _emamiContext.UserDivisionDepotMappings
                                                .Where(p => p.UserDivisionId == division.Id)
                                                .ToList();

                            foreach (var plant in existingPlants)
                            {
                                _emamiContext.UserDivisionDepotMappings.Remove(plant);
                            }

                            _emamiContext.UserDivisionMappings.Remove(division);
                        }
                    }
                    foreach (var division in employeeDto.DivisionList)
                    {
                        if (!userDivisionsID.Contains(division.DivisionId))
                        {
                            var divisionMap = new UserDivisionMapping
                            {
                                DivisionId = division.DivisionId,
                                UserId = employeeDto.Id,
                                DistributionChannelId = division.DistributionChannelId,
                                SalesOrganizationId = division.SalesOrganizationId,
                                CreatedBy = employeeDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ModifiedBy = employeeDto.LoginUserId,
                                ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                SaudaLimit = division.SaudaLimit,
                                SaudaValidityPeriod = division.SaudaValidityPeriod,

                            };
                            _emamiContext.UserDivisionMappings.Add(divisionMap);
                            _emamiContext.SaveChanges(); // Need SaveChanges to get Id

                            //DELETE + INSERT PLANT MAPPINGS
                            AddOrReplaceUserDivisionPlantMappings(divisionMap.Id, division.UserDivisionPlantIds, employeeDto.Id);
                        }
                        else
                        {
                            var divmap = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == employeeDto.Id && _.DivisionId == division.DivisionId).FirstOrDefault();
                            divmap.DivisionId = division.DivisionId;
                            divmap.DistributionChannelId = division.DistributionChannelId;
                            divmap.SalesOrganizationId = division.SalesOrganizationId;
                            divmap.ModifiedBy = employeeDto.LoginUserId;
                            divmap.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            divmap.SaudaLimit = division.SaudaLimit;
                            divmap.SaudaValidityPeriod = division.SaudaValidityPeriod;
                            divmap.UserId = employeeDto.Id;

                            //DELETE + INSERT PLANT MAPPINGS
                            AddOrReplaceUserDivisionPlantMappings(divmap.Id, division.UserDivisionPlantIds, employeeDto.Id);

                        }
                    }
                }



                _emamiContext.SaveChanges();


                //uploaded consent images
                if (employeeDto.Attachments.IsAny())
                {
                    string folderName = DTO.Enums.PageType.ConsentImages.ToString();
                    var mediaFileItemList = new List<SupportAttachmentDto>();
                    foreach (var attachment in employeeDto.Attachments)
                    {
                        string ImagePath = string.Empty;
                        if (attachment != null)
                        {
                            var directory = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigurationManager.AppSettings["UploadAttachments"]), folderName);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }
                            var ext = Path.GetExtension(attachment.FileName);
                            attachment.FileName = Guid.NewGuid() + ext;
                            var filename = Path.Combine(directory, attachment.FileName);
                            string mediaPath = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName);
                            attachment.MediaPath = Path.Combine(mediaPath, attachment.FileName);

                            //Deletion exists file  
                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                            }
                            File.WriteAllBytes(filename, attachment.FileByteArray);
                        }
                    }

                    foreach (var attachment in employeeDto.Attachments)
                    {
                        var consentImageContext = new ConsentImageDetailsForCustomers()
                        {
                            UserId = user.Id,
                            FileName = attachment.FileName,
                            MediaPath = attachment.MediaPath,
                            MediaTypeId = attachment.MediaTypeId,
                            CreatedBy = employeeDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.ConsentImageDetailsForCustomers.Add(consentImageContext);
                    }
                    _emamiContext.SaveChanges();
                }


                //if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.Dealer || employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.ShipToParty)
                //{
                //    if (oldSaudaBookingTypeId != employeeDto.SaudaBookingTypeId)
                //    {
                //        //var CustomerMappingDetails = _emamiContext.UserCustomerMapping.Where(f => f.CustomerId == user.Id).ToList();
                //        //foreach (var item in CustomerMappingDetails)
                //        //{
                //        //    _emamiContext.UserCustomerMapping.Remove(item);
                //        //    _emamiContext.SaveChanges();
                //        //}
                //    }
                //}
                //else
                //{
                //    if (oldSaudaBookingTypeId != employeeDto.SaudaBookingTypeId)
                //    {
                //        var CustomerMappingDetails = _emamiContext.UserCustomerMapping.Where(f => f.UserId == user.Id).ToList();
                //        foreach (var item in CustomerMappingDetails)
                //        {
                //            _emamiContext.UserCustomerMapping.Remove(item);
                //            _emamiContext.SaveChanges();
                //        }
                //    }
                //}

                #region User Reporting To mapping
                var reporterrormsg = "";
                if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.StateTrader || employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.ZonalTrader || employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.NationalTrader)
                {


                    if (employeeDto.SelectedReportingToIds.IsAny())
                    {

                        var existIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == user.Id).Select(s => s.ReportingToUserId).ToList();

                        var removeIds = existIds.Except(employeeDto.SelectedReportingToIds);

                        if (removeIds.IsAny())
                        {
                            foreach (var id in removeIds)
                            {
                                var report = _emamiContext.UserReportingToMappings.FirstOrDefault(_ => _.UserId == user.Id && _.ReportingToUserId == id);
                                _emamiContext.UserReportingToMappings.Remove(report);
                            }

                            _emamiContext.SaveChanges();
                        }


                        foreach (var userId in employeeDto.SelectedReportingToIds)
                        {
                            var reportuseralreadyExists = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.ReportingToUserId == userId && _.UserId == user.Id);

                            if (reportuseralreadyExists == null)
                            {

                                //    var reportUserComb = (from ud in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                          where ud.UserId == userId
                                //                          select ud
                                //                   );
                                //    var reportuserData = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userId);

                                //    var reportuserExists = (from urm in _emamiContext.UserReportingToMappings.AsNoTracking()
                                //                            where urm.UserId == user.Id
                                //                            select urm.ReportingToUserId
                                //                        ).ToList();

                                //    if (reportuserExists.IsAny())
                                //    {
                                //        var isExistBdoCombination = (from div in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                                     join bdiv in reportUserComb on new { SalesOrganizationId = div.SalesOrganizationId, DistributionChannelId = div.DistributionChannelId, DivisionId = div.DivisionId }
                                //                 equals new { SalesOrganizationId = bdiv.SalesOrganizationId, DistributionChannelId = bdiv.DistributionChannelId, DivisionId = bdiv.DivisionId }
                                //                                     where reportuserExists.Contains(div.UserId)
                                //                                     select div.UserId
                                //                              );
                                //        if (isExistBdoCombination.IsAny())
                                //        {
                                //            reporterrormsg = reporterrormsg + reportuserData.Name + ",";
                                //        }
                                //        else
                                //        {
                                _emamiContext.UserReportingToMappings.Add(new UserReportingToMapping() { UserId = user.Id, ReportingToUserId = userId, CreatedBy = employeeDto.LoginUserId, RoleId = employeeDto.RoleId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                                _emamiContext.SaveChanges();
                                //        }
                                //    }
                                //    else
                                //    {
                                //        var isCombMatched = (from ud in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                             join rud in reportUserComb on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                //                             equals new { SalesOrganizationId = rud.SalesOrganizationId, DistributionChannelId = rud.DistributionChannelId, DivisionId = rud.DivisionId }
                                //                             where ud.UserId == user.Id
                                //                             select ud
                                //                       ).FirstOrDefault();

                                //        if (isCombMatched != null)
                                //        {
                                //            _emamiContext.UserReportingToMappings.Add(new UserReportingToMapping() { UserId = user.Id, ReportingToUserId = userId, CreatedBy = employeeDto.LoginUserId, RoleId = employeeDto.RoleId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                                //            _emamiContext.SaveChanges();
                                //        }
                                //        else
                                //        {
                                //            reporterrormsg = reporterrormsg + reportuserData.Name + ",";
                                //        }
                                //    }


                            }

                        }



                    }
                }


                #endregion

                #region Dealer,Broker,Plant,Depot Mapping
                var customermaperrormsg = "";
                if (employeeDto.RoleId == (int)DTO.Enums.Role.StateTrader || employeeDto.RoleId == (int)DTO.Enums.Role.KAM)
                {
                    if (employeeDto.SelectedDealerBrokerIds != null && employeeDto.SelectedDealerBrokerIds.Any())
                    {
                        // var bdoComb = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == user.Id);

                        foreach (var customerId in employeeDto.SelectedDealerBrokerIds)
                        {
                            //var divcomb = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == customerId).ToList();
                            //var bdoIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == customerId).Select(s => s.UserId).ToList();
                            //var dealerData = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == customerId);

                            var isExistCustomer = _emamiContext.UserCustomerMapping.FirstOrDefault(f => f.UserId == user.Id && f.CustomerId == customerId);
                            if (isExistCustomer == null)
                            {
                                //if (bdoIds.IsAny())
                                //{
                                //var isExistBdoCombination = (from div in _emamiContext.UserDivisionMappings.AsNoTracking()
                                //                             join ur in _emamiContext.UserRoles.AsNoTracking() on div.UserId equals ur.UserId
                                //                             join bdiv in bdoComb on new { SalesOrganizationId = div.SalesOrganizationId, DistributionChannelId = div.DistributionChannelId, DivisionId = div.DivisionId }
                                //         equals new { SalesOrganizationId = bdiv.SalesOrganizationId, DistributionChannelId = bdiv.DistributionChannelId, DivisionId = bdiv.DivisionId }
                                //                             where bdoIds.Contains(div.UserId)
                                //                             && ur.RoleId == 7
                                //                             && div.UserId != user.Id
                                //                             select div.UserId
                                //                           );

                                //if (isExistBdoCombination.IsAny())
                                //{
                                //    customermaperrormsg = customermaperrormsg + dealerData.Name + ",";
                                //}
                                //else
                                //{
                                var userCustomerContext = new UserCustomerMapping
                                {
                                    UserId = user.Id,
                                    CustomerId = customerId,
                                    CreatedBy = employeeDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                };
                                _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                                // }
                                //}
                                //else
                                //{
                                //    //var isExistBdoCombination = (from div in divcomb
                                //    //                             join bdiv in bdoComb on new { SalesOrganizationId = div.SalesOrganizationId, DistributionChannelId = div.DistributionChannelId, DivisionId = div.DivisionId }
                                //    //        equals new { SalesOrganizationId = bdiv.SalesOrganizationId, DistributionChannelId = bdiv.DistributionChannelId, DivisionId = bdiv.DivisionId }
                                //    //                             select div.UserId

                                //    //                            ).FirstOrDefault();
                                //    //if (isExistBdoCombination != null)
                                //    //{
                                //        var userCustomerContext = new UserCustomerMapping
                                //        {
                                //            UserId = user.Id,
                                //            CustomerId = customerId,
                                //            CreatedBy = employeeDto.LoginUserId,
                                //            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                //        };
                                //        _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                                //    //}
                                //    //else
                                //    //{
                                //    //    customermaperrormsg = customermaperrormsg + dealerData.Name + ",";
                                //    //}
                                //}
                            }
                        }
                        _emamiContext.SaveChanges();

                    }

                    if (employeeDto.RemovedDealerBrokerIds != null && employeeDto.RemovedDealerBrokerIds.Any())
                    {
                        foreach (var customerId in employeeDto.RemovedDealerBrokerIds)
                        {
                            var isExistCustomer = _emamiContext.UserCustomerMapping.FirstOrDefault(f => f.UserId == user.Id && f.CustomerId == customerId);
                            _emamiContext.UserCustomerMapping.Remove(isExistCustomer);
                        }
                        _emamiContext.SaveChanges();
                    }
                }

                //if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.StateTrader || employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.KAM)
                //{
                //    if (employeeDto.SelectedDealerBrokerIds != null && employeeDto.SelectedDealerBrokerIds.Any())
                //    {
                //        foreach (var customerId in employeeDto.SelectedDealerBrokerIds)
                //        {
                //            var isExistCustomer = _emamiContext.UserCustomerMapping.FirstOrDefault(f => f.UserId == user.Id && f.CustomerId == customerId);
                //            if (isExistCustomer == null)
                //            {
                //                _emamiContext.UserCustomerMapping.Add(new UserCustomerMapping() { UserId = user.Id, CustomerId = customerId, CreatedBy = employeeDto.LoginUserId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                //            }
                //        }
                //        _emamiContext.SaveChanges();
                //    }

                //    //Remove
                //    if (employeeDto.RemovedDealerBrokerIds != null && employeeDto.RemovedDealerBrokerIds.Any())
                //    {
                //        foreach (var customerId in employeeDto.RemovedDealerBrokerIds)
                //        {
                //            var isExistCustomer = _emamiContext.UserCustomerMapping.FirstOrDefault(f => f.UserId == user.Id && f.CustomerId == customerId);
                //            _emamiContext.UserCustomerMapping.Remove(isExistCustomer);
                //        }
                //        _emamiContext.SaveChanges();
                //    }
                //}

                if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.Dealer || employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.Broker)
                {
                    //if (employeeDto.SelectedDepotIds != null && employeeDto.SelectedDepotIds.Any())
                    // {
                    //var depotExistList = _emamiContext.UserDepotMapping.AsNoTracking().Where(f => f.UserId == user.Id);
                    //if (depotExistList != null && depotExistList.Any())
                    //{
                    //    foreach (var depotDelete in depotExistList)
                    //    {
                    //        _emamiContext.UserDepotMapping.Attach(depotDelete);
                    //        _emamiContext.UserDepotMapping.Remove(depotDelete);
                    //    }
                    //    _emamiContext.SaveChanges();
                    //}

                    //Depot mapping
                    //CreateUserDepotMapping(employeeDto.SelectedDepotIds, user.Id, employeeDto.LoginUserId);
                    //Plant mapping
                    //CreateUserDepotMapping(employeeDto.SelectedPlantIds, user.Id, employeeDto.LoginUserId);

                    //foreach (var depotId in employeeDto.SelectedDepotIds)
                    //{
                    //    var userCustomerContext = new UserDepotMapping
                    //    {
                    //        UserId = user.Id,
                    //        DepotId = depotId,
                    //        CreatedBy = employeeDto.LoginUserId,
                    //        CreatedDate = DateTime.UtcNow
                    //    };
                    //    _emamiContext.UserDepotMapping.Add(userCustomerContext);
                    //}
                    //_emamiContext.SaveChanges();

                    //if (employeeDto.PlantId > 0)
                    //{
                    //    var userCustomerContext1 = new UserDepotMapping
                    //    {
                    //        UserId = user.Id,
                    //        DepotId = employeeDto.PlantId,
                    //        CreatedBy = employeeDto.LoginUserId,
                    //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    //    };
                    //    _emamiContext.UserDepotMapping.Add(userCustomerContext1);
                    //}

                    //if (employeeDto.DepotId > 0)
                    //{
                    //    var userCustomerContext = new UserDepotMapping
                    //    {
                    //        UserId = user.Id,
                    //        DepotId = employeeDto.DepotId,
                    //        CreatedBy = employeeDto.LoginUserId,
                    //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    //    };
                    //    _emamiContext.UserDepotMapping.Add(userCustomerContext);
                    //}

                    //}
                }

                if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.Dealer)
                {
                    //if (employeeDto.IsBroker)
                    //{
                    //    if (employeeDto.SelectedBrokerIds != null && employeeDto.SelectedBrokerIds.Any())
                    //    {
                    //        var dealerExistList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(f => f.UserId == user.Id);
                    //        if (dealerExistList != null && dealerExistList.Any())
                    //        {
                    //            foreach (var dealerDelete in dealerExistList)
                    //            {
                    //                _emamiContext.UserCustomerMapping.Attach(dealerDelete);
                    //                _emamiContext.UserCustomerMapping.Remove(dealerDelete);
                    //            }
                    //            _emamiContext.SaveChanges();
                    //        }

                    //        foreach (var customerId in employeeDto.SelectedBrokerIds)
                    //        {
                    //            _emamiContext.UserCustomerMapping.Add(new UserCustomerMapping()
                    //            { UserId = user.Id, CustomerId = customerId, CreatedBy = employeeDto.LoginUserId, CreatedDate = DateTime.UtcNow });
                    //        }
                    //        _emamiContext.SaveChanges();
                    //    }
                    //}

                    //if (employeeDto.IsBroker)
                    //{

                    var dealerExistList = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                  .Join(_emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == user.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                  .Select(_ => _.UserCustomerMapping).ToList();


                    //var dealerExistList = _emamiContext.UserCustomerMapping.Where(f => f.CustomerId == user.Id);
                    if (dealerExistList != null && dealerExistList.Any())
                    {
                        foreach (var dealerDelete in dealerExistList)
                        {
                            _emamiContext.UserCustomerMapping.Attach(dealerDelete);
                            _emamiContext.UserCustomerMapping.Remove(dealerDelete);
                        }
                        _emamiContext.SaveChanges();
                    }


                    if (employeeDto.BrokerIds != null && employeeDto.BrokerIds.Any())
                    {
                        foreach (var data in employeeDto.BrokerIds)
                        {
                            var userCustomerContext = new UserCustomerMapping
                            {
                                UserId = data,
                                CustomerId = user.Id,
                                CreatedBy = employeeDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                        }
                        _emamiContext.SaveChanges();
                    }
                    UpdateCustomerShipToPartyMapping(employeeDto.SelectedDealerIds, user.Id, employeeDto.LoginUserId);
                    UpdateCustomerTruckCapacityMapping(employeeDto.PlantTruckCapacities, employeeDto.DepotTruckCapacities, user.Id, employeeDto.LoginUserId);
                    //}
                }

                if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.Broker)
                {
                    if (employeeDto.SelectedDealerIds != null && employeeDto.SelectedDealerIds.Any())
                    {
                        var dealerExistList = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer)
                      .Join(_emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == user.Id), u => u.UserId, ur => ur.CustomerId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                      .Select(_ => _.UserCustomerMapping).ToList();

                        //var dealerExistList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(f => f.UserId == user.Id);
                        if (dealerExistList != null && dealerExistList.Any())
                        {
                            foreach (var dealerDelete in dealerExistList)
                            {
                                _emamiContext.UserCustomerMapping.Attach(dealerDelete);
                                _emamiContext.UserCustomerMapping.Remove(dealerDelete);
                            }
                            _emamiContext.SaveChanges();
                        }

                        foreach (var customerId in employeeDto.SelectedDealerIds)
                        {
                            _emamiContext.UserCustomerMapping.Add(new UserCustomerMapping()
                            { UserId = user.Id, CustomerId = customerId, CreatedBy = employeeDto.LoginUserId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                        }
                        _emamiContext.SaveChanges();
                    }
                    UpdateCustomerTruckCapacityMapping(employeeDto.PlantTruckCapacities, employeeDto.DepotTruckCapacities, user.Id, employeeDto.LoginUserId);
                }

                if (employeeDto.RoleId == (int)Adani.Solution.DTO.Enums.Role.ShipToParty)
                {
                    var depotExistList = _emamiContext.UserDepotMapping.AsNoTracking().Where(f => f.UserId == user.Id);
                    if (depotExistList != null && depotExistList.Any())
                    {
                        foreach (var depotDelete in depotExistList)
                        {
                            _emamiContext.UserDepotMapping.Attach(depotDelete);
                            _emamiContext.UserDepotMapping.Remove(depotDelete);
                        }
                        _emamiContext.SaveChanges();
                    }

                    //Depot mapping
                    //CreateUserDepotMapping(employeeDto.SelectedDepotIds, user.Id, employeeDto.LoginUserId);
                    //Plant mapping
                    CreateUserDepotMapping(employeeDto.SelectedPlantIds, user.Id, employeeDto.LoginUserId);
                    //Customer Truck capacity mapping
                    UpdateCustomerTruckCapacityMapping(employeeDto.PlantTruckCapacities, employeeDto.DepotTruckCapacities, user.Id, employeeDto.LoginUserId);

                    var dealerExistList = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == user.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                        .Select(_ => _.UserCustomerMapping).ToList();

                    if (dealerExistList != null && dealerExistList.Any())
                    {
                        foreach (var dealerDelete in dealerExistList)
                        {
                            _emamiContext.UserCustomerMapping.Attach(dealerDelete);
                            _emamiContext.UserCustomerMapping.Remove(dealerDelete);
                        }
                        _emamiContext.SaveChanges();
                    }

                    if (employeeDto.BrokerId > 0)
                    {
                        var userCustomerContext = new UserCustomerMapping
                        {
                            UserId = employeeDto.BrokerId,
                            CustomerId = user.Id,
                            CreatedBy = employeeDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.UserCustomerMapping.Add(userCustomerContext);
                        _emamiContext.SaveChanges();
                    }
                }

                #endregion

                var oldIncoTermsIds = _emamiContext.UserIncoTerms.Where(w => w.UserId == employeeDto.Id).Select(_ => _.IncoTermsId);
                UpdateUserIncoTerms(employeeDto.IncoTermsId, employeeDto.Id, employeeDto.LoginUserId, employeeDto.Email, employeeDto.Name, employeeDto.RoleId);

                if (_resultService.IsEmail())
                {
                    var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == employeeDto.Id);
                    if (userRoleContext != null && (userRoleContext.RoleId == (int)DTO.Enums.Role.Dealer || userRoleContext.RoleId == (int)DTO.Enums.Role.Broker))
                    {
                        string msgContent = string.Empty;
                        if ((employeeDto.IncoTermsId != null && employeeDto.IncoTermsId.Any()))
                        {
                            List<long> newIncoTermsIds = new List<long>();
                            if (oldIncoTermsIds != null && oldIncoTermsIds.Any())
                            {
                                newIncoTermsIds = employeeDto.IncoTermsId.Except(oldIncoTermsIds).ToList();
                            }
                            else
                            {
                                newIncoTermsIds = employeeDto.IncoTermsId;
                            }
                            if (newIncoTermsIds != null && newIncoTermsIds.Any())
                            {
                                var newIncoTermsNames = _emamiContext.IncoTerms.Where(_ => newIncoTermsIds.Contains(_.Id) && _.Name != null && _.Name != "").Select(_ => _.Name);
                                foreach (var newIncoTermsName in newIncoTermsNames)
                                {
                                    if (string.IsNullOrEmpty(msgContent))
                                    {
                                        msgContent = newIncoTermsName;
                                    }
                                    else
                                    {
                                        msgContent += ", " + newIncoTermsName;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(msgContent) && user.MobileNumber != employeeDto.MobileNumber)
                        {
                            msgContent = " and " + employeeDto.MobileNumber;
                        }
                        else if (user.MobileNumber != employeeDto.MobileNumber)
                        {
                            msgContent = employeeDto.MobileNumber;
                        }
                        if (!string.IsNullOrEmpty(msgContent))
                        {
                            try
                            {
                                List<User> usersContext = new List<User>();
                                List<string> toUsers = new List<string>();
                                User createdBy = new User();
                                User dealer = new User();
                                var configContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.NotificationEmail);
                                if (configContext != null)
                                {
                                    var emailIds = UtilityHelper.ConvertStringToStringArray(configContext.Value);
                                    toUsers = emailIds.ToList();
                                }


                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                if (toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var emailSubject = string.Empty;
                                    var plainText = string.Empty;
                                    EmailTemplate emailTemplate = new EmailTemplate();

                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.CustomerDetailsChangeNotificationEmail);
                                    emailSubject = Constants.CustomerDetailsChangeSubject;

                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.IncoTerms_MobileNo, msgContent).Replace(Constants.Name, employeeDto.Name);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                //if (_resultService.IsSMS())
                //{
                //    var smsPlainTemplate = string.Empty;
                //    var smsMessage = string.Empty;
                //    EmailTemplate smsTemplate = new EmailTemplate();
                //    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.CustomerDetailsChangeNotificationSMS);

                //    if (smsTemplate != null)
                //    {
                //        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.IncoTerms_MobileNo, msgContent).Replace(Constants.Name, employeeDto.Name);
                //    smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                //    }
                //    if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                //    {
                //        amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                //    }
                //    if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                //    {
                //        amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                //    }
                //}

                var finalerrormsg = "";
                if (customermaperrormsg != "")
                {
                    finalerrormsg = finalerrormsg + Constants.BDOCompExist.Replace("Distributor", customermaperrormsg);
                }


                if (reporterrormsg != "")
                {
                    finalerrormsg = finalerrormsg + Constants.UserCompExist.Replace("Users", reporterrormsg);
                }

                if (finalerrormsg != "")
                {
                    return _resultService.ErrorMessage(finalerrormsg);
                }

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

        /// <summary>
        /// Get secure upload directory outside web root for profile photos.
        /// Priority:
        ///  1) appSettings["UploadMediaSecurePath"] — should be absolute physical path OUTSIDE web root (recommended).
        ///  2) ~/App_Data/Uploads/ProfilePhotos — safe fallback inside site but not served by IIS static handler.
        /// The method creates the folder if missing and returns the physical path.
        /// </summary>
        private string GetSecureProfileUploadDirectory()
        {
            try
            {
                var configured = ConfigurationManager.AppSettings["UploadMediaSecurePath"];
                string basePath;

                // Use configured absolute path if present and rooted
                if (!string.IsNullOrWhiteSpace(configured) && Path.IsPathRooted(configured))
                {
                    basePath = configured;
                }
                else
                {
                    // App_Data is not served by IIS static files by default => safer than site root
                    basePath = HostingEnvironment.MapPath("~/App_Data/Uploads");
                    if (string.IsNullOrWhiteSpace(basePath))
                    {
                        // last resort: use application base
                        basePath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }

                var folderName = "ProfilePhotos";
                var fullFolderPath = Path.Combine(basePath, folderName);

                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                return fullFolderPath;
            }
            catch (Exception ex)
            {
                _logger.Warn($"{ServiceName} GetSecureProfileUploadDirectory fallback: {ex}");
                // fallback to App_Data
                var fallback = HostingEnvironment.MapPath("~/App_Data/Uploads") ?? AppDomain.CurrentDomain.BaseDirectory;
                var ff = Path.Combine(fallback, "ProfilePhotos");
                if (!Directory.Exists(ff)) Directory.CreateDirectory(ff);
                return ff;
            }
        }

        public ResultDto ProfileUpload(EmployeeDto employeeDto)
        {
            _methodName = "ProfileUpload";
            var resultDto = new ResultDto();
            try
            {
                if (employeeDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                var usercontext = _emamiContext.Users.FirstOrDefault(_ => _.Id == employeeDto.LoginUserId);
                if (usercontext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = "User not found.";
                    return resultDto;
                }

                //uploaded profile images
                if (employeeDto.Attachments.IsAny())
                {
                    foreach (var attachment in employeeDto.Attachments)
                    {
                        if (attachment == null || attachment.FileByteArray == null || attachment.FileByteArray.Length == 0)
                            continue;

                        try
                        {
                            // Security: Get secure directory outside web root
                            var secureDirectory = GetSecureProfileUploadDirectory();

                            // Security: Extract and validate extension
                            var ext = Path.GetExtension(attachment.FileName)?.ToLowerInvariant() ?? string.Empty;
                            
                            // Security: Validate extension is safe (should already be validated by controller, but double-check)
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                            if (!allowedExtensions.Contains(ext))
                            {
                                _logger.Warn($"{ServiceName} ProfileUpload: Invalid extension {ext} for file {attachment.FileName}");
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto.Message = "Invalid file format. Only JPG, JPEG and PNG images are allowed.";
                                return resultDto;
                            }

                            // Security: Generate GUID filename instead of predictable user-based name
                            // This prevents filename enumeration and makes files harder to guess
                            var safeFileName = Guid.NewGuid().ToString("N") + ext;
                            var filePath = Path.Combine(secureDirectory, safeFileName);

                            // Security: Delete old profile photo if exists (find by user ID pattern or store mapping)
                            // For now, we'll store the safe filename in the database for later retrieval
                            // Delete any existing files for this user (if we stored the mapping)
                            var oldProfilePath = usercontext.ProfilePath;
                            if (!string.IsNullOrEmpty(oldProfilePath))
                            {
                                // Extract filename from old path if it was a direct URL
                                // If it's a handler URL, we need to track it differently
                                // For now, we'll just overwrite the reference
                            }

                            // Security: Save file to secure location outside web root
                            File.WriteAllBytes(filePath, attachment.FileByteArray);

                            // Security: Store handler URL instead of direct URL
                            // Files are served through /Lookup/DownloadProfilePhoto handler, not directly
                            var apiUrl = ConfigurationManager.AppSettings["WebsiteUrl"];
                            // Use handler URL pattern instead of direct file access
                            var handlerUrl = $"{apiUrl}/Lookup/DownloadProfilePhoto?file={System.Web.HttpUtility.UrlEncode(safeFileName)}";
                            attachment.MediaPath = handlerUrl;
                            attachment.FileName = safeFileName; // Store the safe filename

                            // Update user profile path with handler URL
                            usercontext.ProfilePath = handlerUrl;
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"{ServiceName} ProfileUpload: Error saving file {attachment.FileName}: {ex}");
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = "Error saving profile photo. Please try again.";
                            return resultDto;
                        }
                    }
                    
                    _emamiContext.SaveChanges();
                }

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
        private ResultDto AddOrUpdateDealerLocation(EmployeeDto dto)
        {
            _methodName = "UpdateUser";
            var resultDto = new ResultDto();
            try
            {
                var dealerLocation = _emamiContext.DealerLocation.Where(s => s.UserId == dto.Id).ToList();
                var newLocation = dto.PickupLocation.Where(s => s.PickupLocationId == 0).Select(st => new DealerLocation() { UserId = dto.LoginUserId, StateId = st.StateId, CityId = st.CityId, DistrictId = st.DistrictId, Address = st.Address, CreatedBy = dto.LoginUserId, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow), ModifiedBy = dto.LoginUserId, ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                var deletedStates = dealerLocation.Select(s => s.Id).Except(dto.PickupLocation.Select(s => s.PickupLocationId).Select(s => s));
                var deletedrow = dealerLocation.Where(s => deletedStates.Contains(s.Id));

                newLocation.ToList().ForEach(s => _emamiContext.DealerLocation.Add(s));
                deletedrow.ToList().ForEach(s => _emamiContext.DealerLocation.Remove(s));
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dto;
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

        public ResultDto GetUserRoleClaims(UserIdDto userIdDto)
        {
            _methodName = "GetUserClaims";
            var resultDto = new ResultDto();
            var userClaimListDto = new List<UserClaimsDto>();
            try
            {
                if (userIdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (userIdDto.UserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Utility.MessageLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userIdDto.UserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Utility.MessageLanguage);
                    return resultDto;
                }
                if (!userContext.IsActive)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InActiveUser;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InActiveUser, Utility.MessageLanguage);
                    return resultDto;
                }

                var userRoleContext = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == userIdDto.UserId);

                if (null == userRoleContext)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserRoleMappingNotExists;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserRoleMappingNotExists, Utility.MessageLanguage);
                    return resultDto;
                }
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
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userClaimListDto;
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

        #region Dealer

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetDealerList(LoginUserIdDto inputDto)
        {
            _methodName = "GetDealerList";
            var resultDto = new ResultDto();
            var dealerDto = new List<DealerDto>();
            try
            {
                IQueryable<User> entity;
                if (inputDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User);
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0));
                }
                else
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User);
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0) && _.IsActive);
                    entity = entity.Where(_ => _.IsActive);
                }

                dealerDto = entity.ToList().OrderBy(_ => _.Name).Select(c => new DealerDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    SaudaValidityPeriod = Convert.ToInt32(c.SaudaValidityPeriod),
                    //SaudaLimit = c.SaudaLimit,
                    SaudaBookingTypeId = c.SaudaBookingTypeId,
                    // SaudaBookingType = c.SaudaBookingType?.Name,
                    //FreightRouteName = c.FreightRoute?.Name,
                    //FreightZoneName = c.FreightZone?.Name,
                    City = c.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.CityId)?.CityName : string.Empty,
                    ZoneId = c.ZoneId,
                    // Zone = c.Zone?.Name,
                    District = c.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == c.DistrictId)?.DistrictName : string.Empty,
                    State = c.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == c.StateId)?.StateName : string.Empty,
                    Territory = c.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(_ => _.Id == c.TerritoryId)?.Name : string.Empty,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    GSTN = c.GSTN,
                    TANNumber = c.TANNumber,
                    CustClass = c.CustClass,
                    VisitDay = c.VisitDay,
                    WeeklyClosingDay = c.WeeklyClosingDay,
                    MonthlyPotential = c.MonthlyPotential,
                    //PlantTruckCapacity = c.Loadability,
                    //DepotTruckCapacity = c.DepotLoadability,
                    //TransportMode = c.TransportMode?.Name,
                    //  SaudaType = c.SaudaBookingType?.Name,
                    Pincode = c.Pincode,
                    //VerticalCode = c.Division?.Code,
                    //VerticalName = c.Division?.Name,
                    FSSAINumber = c.FSSAINumber,
                    Password = !string.IsNullOrEmpty(c.Password) ? UtilityHelper.ConvertMd5ToString(c.Password, SecurityConstants.EncryptionKey) : string.Empty,
                    InActiveRemarks = c.InActiveRemarkId != null ? _emamiContext.DeleteListCreations.AsNoTracking().FirstOrDefault(_ => _.Id == c.InActiveRemarkId).Remarks : string.Empty,
                    //CustomerGroupOne = _emamiContext.CustomerGroupOne.AsNoTracking().FirstOrDefault(f => f.Id == c.CustomerGroupOneId)?.GroupName,
                    //CustomerGroupTwo = _emamiContext.CustomerGroupTwo.AsNoTracking().FirstOrDefault(f => f.Id == c.CustomerGroupTwoId)?.GroupName,
                    CustomerGroupFiveId = c.CustomerGroupFiveId,
                    AdditionalMobileNumber = c.AdditionalMobileNumber,
                    IsActiveForCall = c.IsActiveForCall,
                    CompanyCode = c.CompanyCode == null ? string.Empty : c.CompanyCode
                }).ToList();

                foreach (var item in dealerDto)
                {
                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    item.PlantTruckCapacities = string.Join(",", plantcapacities);

                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    item.DepotTruckCapacities = string.Join(",", depotcapacities);
                    var userIncoTerms = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == item.Id);
                    var incoterms = userIncoTerms
                        .Join(_emamiContext.IncoTerms, uic => uic.IncoTermsId, ic => ic.Id, (UserIncoTerms, IncoTerms) => new { IncoTerms })
                        .Select(_ => _.IncoTerms).ToList();
                    if (incoterms != null && incoterms.Any())
                    {
                        item.Incoterms = string.Join(",", incoterms.Select(_ => _.Name.Trim()).Distinct().ToList());
                    }

                    var brokerContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                    .Select(_ => _.UserCustomerMapping.User.Code).ToList();
                    if (brokerContext.IsAny())
                    {

                        item.BrokerCode = UtilityHelper.ConvertStringListToCommaSeparatedString(brokerContext);
                    }


                    var depotPlantListContext = _emamiContext.UserDepotMapping.AsNoTracking().Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.DepotId, d => d.Id, (x, d) => new { d.StorageTypeId, Depot = d.Name, DepotCode = d.Code });

                    var depotListContext = depotPlantListContext.Where(_ => (_.StorageTypeId == (int)DTO.Enums.StorageType.Depot || _.StorageTypeId == (int)DTO.Enums.StorageType.Rake)).Select(_ => _.DepotCode).ToList();
                    var plantListContext = depotPlantListContext.Where(_ => _.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(_ => _.DepotCode).ToList();
                    if (depotListContext != null && depotListContext.Any())
                    {
                        item.Depots = UtilityHelper.ConvertStringListToCommaSeparatedString(depotListContext);
                    }
                    if (plantListContext != null && plantListContext.Any())
                    {
                        item.Plants = UtilityHelper.ConvertStringListToCommaSeparatedString(plantListContext);
                    }

                    var bdoContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader)
                   .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                   .Select(_ => _.UserCustomerMapping.User).FirstOrDefault();
                    if (bdoContext != null)
                    {
                        item.StateTrader = bdoContext.Name;
                        item.BDOCode = bdoContext.Code;
                    }

                    var shipToPartyList = _emamiContext.CustomerShipToPartyMappings.AsNoTracking().Where(_ => _.CustomerId == item.Id).Select(_ => _.ShipToParty.ShipToPartyCode).ToList();
                    if (shipToPartyList != null && shipToPartyList.Any())
                    {
                        item.ShipToParty = UtilityHelper.ConvertStringListToCommaSeparatedString(shipToPartyList);
                    }


                    #region Check Is Empty & Need to Update

                    var incoTermsIds = userIncoTerms.Select(s => s.IncoTermsId).ToList();
                    if (incoTermsIds == null || !incoTermsIds.Any())
                    {
                        item.NewlyAdded = "Yes, incoTerms Missing";
                    }
                    if (item.SaudaValidityPeriod == 0)
                    {
                        item.NewlyAdded = "Yes, SaudaValidityPeriod Missing";
                    }
                    if (item.TransportModeId == 0)
                    {
                        item.NewlyAdded = "Yes, TransportModeId Missing";
                    }
                    if (item.SaudaBookingTypeId == 0)
                    {
                        item.NewlyAdded = "Yes, SaudaBookingTypeId Missing";
                    }
                    if (item.SaudaLimit == 0)
                    {
                        item.NewlyAdded = "Yes, SaudaLimit Missing";
                    }
                    if (string.IsNullOrEmpty(item.Plants))
                    {
                        item.NewlyAdded = "Yes, PlantName Missing";
                    }
                    if (string.IsNullOrEmpty(item.Depots))
                    {
                        item.NewlyAdded = "Yes, DepotName Missing";
                    }
                    //if (string.IsNullOrEmpty(item.FreightRouteName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightRouteName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.FreightZoneName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightZoneName Missing";
                    //}
                    if (string.IsNullOrEmpty(item.Zone))
                    {
                        item.NewlyAdded = "Yes, Zone Missing";
                    }
                    if (string.IsNullOrEmpty(item.State))
                    {
                        item.NewlyAdded = "Yes, State Missing";
                    }
                    if (string.IsNullOrEmpty(item.Territory))
                    {
                        item.NewlyAdded = "Yes, Territory Missing";
                    }
                    if (string.IsNullOrEmpty(item.District))
                    {
                        item.NewlyAdded = "Yes, District Missing";
                    }
                    if (string.IsNullOrEmpty(item.City))
                    {
                        item.NewlyAdded = "Yes, City Missing";
                    }

                    #endregion
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dealerDto != null ? dealerDto.OrderByDescending(_ => _.Id).ToList() : dealerDto;
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
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetDealerListWithPaging(KendoGridResult inputDto)
        {
            _methodName = "GetDealerListWithPaging";
            var resultDto = new ResultDto();
            var dealerDto = new List<DealerDto>();
            DataSourceResult result = new DataSourceResult();
            List<User> entity;

            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User).ToList();
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0));
                }
                else
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User).ToList();
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0) && _.IsActive);
                    entity = entity.Where(_ => _.IsActive).ToList();
                }

                var deleteListContext = _emamiContext.DeleteListCreations.AsNoTracking();
                result = entity.AsEnumerable().Select(c => new DealerDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    SaudaValidityPeriod = c.SaudaValidityPeriod,
                    //SaudaLimit = c.SaudaLimit,
                    SaudaBookingTypeId = c.SaudaBookingTypeId,
                    //  SaudaBookingType = c.SaudaBookingType != null ? c.SaudaBookingType.Name : string.Empty,
                    //FreightRouteName = c.FreightRoute != null ? c.FreightRoute.Name : string.Empty,
                    //FreightZoneName = c.FreightZone != null ? c.FreightZone.Name : string.Empty,
                    ZoneId = c.ZoneId,
                    //  Zone = c.Zone != null ? c.Zone.Name : string.Empty,
                    CityId = c.CityId,
                    DistrictId = c.DistrictId,
                    StateId = c.StateId,
                    TerritoryId = c.TerritoryId,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    GSTN = c.GSTN,
                    TANNumber = c.TANNumber,
                    CustClass = c.CustClass,
                    VisitDay = c.VisitDay,
                    WeeklyClosingDay = c.WeeklyClosingDay,
                    MonthlyPotential = c.MonthlyPotential,
                    //PlantTruckCapacity = c.Loadability,
                    //DepotTruckCapacity = c.DepotLoadability,
                    //TransportMode = c.TransportMode != null ? c.TransportMode.Name : string.Empty,
                    //  SaudaType = c.SaudaBookingType != null ? c.SaudaBookingType.Name : string.Empty,
                    CustomerGroupFiveId = c.CustomerGroupFiveId,
                    Pincode = c.Pincode,
                    //VerticalCode = c.Division != null ? c.Division.Code : string.Empty,
                    //VerticalName = c.Division != null ? c.Division.Name : string.Empty,
                    FSSAINumber = c.FSSAINumber,
                    CompanyCode = c.CompanyCode == null ? string.Empty : c.CompanyCode,
                    Password = c.Password != null ? c.Password : string.Empty,
                    InActiveRemarks = c.InActiveRemarkId != null ? deleteListContext.FirstOrDefault(_ => _.Id == c.InActiveRemarkId).Remarks : string.Empty
                }).ToDataSourceResult(inputDto.DataSourceRequest);

                dealerDto = (result != null && result.Data != null) ? result.Data as List<DealerDto> : new List<DealerDto>();

                foreach (var item in dealerDto)
                {

                    var cg5 = _emamiContext.CustomerGroupFive.Where(_ => _.IsActive);
                    if (item.CustomerGroupFiveId > 0)
                    {
                        item.CustomerGroupFive = cg5.Where(_ => _.Id == item.CustomerGroupFiveId).Select(_ => _.GroupName).FirstOrDefault();
                    }

                    var divisions = _emamiContext.UserDivisionMappings.AsNoTracking()
                        .Where(w => w.UserId == item.Id);
                    if (divisions != null && divisions.Any())
                    {
                        item.VerticalName = string.Join(",", divisions.Select(_ => _.SalesOrganization.Code+"/"+_.DistributionChannel.Code+"/"+_.Division.Code+"-"+_.SaudaLimit).Distinct().ToList());
                    }

                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    item.PlantTruckCapacities = string.Join(",", plantcapacities);

                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    item.DepotTruckCapacities = string.Join(",", depotcapacities);
                    var userIncoTerms = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == item.Id);
                    var incoterms = userIncoTerms.Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.IncoTerms, uic => uic.IncoTermsId, ic => ic.Id, (UserIncoTerms, IncoTerms) => new { IncoTerms }).Select(_ => _.IncoTerms).ToList();
                    if (incoterms != null && incoterms.Any())
                    {
                        item.Incoterms = string.Join(",", incoterms.Select(_ => _.Name.Trim()).Distinct().ToList());
                    }

                    var brokerContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                    .Select(_ => _.UserCustomerMapping.User.Code).ToList();
                    if (brokerContext.IsAny())
                    {
                        item.BrokerCode = UtilityHelper.ConvertStringListToCommaSeparatedString(brokerContext);
                    }


                    var depotPlantListContext = _emamiContext.UserDepotMapping.AsNoTracking().Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.DepotId, d => d.Id, (x, d) => new { StorageTypeId = d.StorageTypeId, Depot = d.Name, DepotCode = d.Code });
                    var depotListContext = depotPlantListContext.Where(_ => (_.StorageTypeId == (int)DTO.Enums.StorageType.Depot || _.StorageTypeId == (int)DTO.Enums.StorageType.Rake)).Select(_ => _.DepotCode).ToList();
                    var plantListContext = depotPlantListContext.Where(_ => _.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(_ => _.DepotCode).ToList();
                    if (depotListContext != null && depotListContext.Any())
                    {
                        item.Depots = UtilityHelper.ConvertStringListToCommaSeparatedString(depotListContext);
                    }
                    if (plantListContext != null && plantListContext.Any())
                    {
                        item.Plants = UtilityHelper.ConvertStringListToCommaSeparatedString(plantListContext);
                    }

                    var bdoContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader)
                   .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                   .Select(_ => _.UserCustomerMapping.User).FirstOrDefault();
                    if (bdoContext != null)
                    {
                        item.StateTrader = bdoContext.Name;
                        item.BDOCode = bdoContext.Code;
                    }

                    var name = item.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == item.CityId)?.CityName : string.Empty;
                    item.City = Utility.TrimAndReduce(name);

                    name = item.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == item.DistrictId)?.DistrictName : string.Empty;
                    item.District = Utility.TrimAndReduce(name);

                    name = item.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == item.StateId)?.StateName : string.Empty;
                    item.State = Utility.TrimAndReduce(name);

                    name = item.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(_ => _.Id == item.TerritoryId)?.Name : string.Empty;
                    item.Territory = Utility.TrimAndReduce(name);

                    #region Check Is Empty & Need to Update

                    var incoTermsIds = userIncoTerms.Where(w => w.UserId == item.Id).Select(s => s.IncoTermsId).ToList();
                    if (incoTermsIds == null || !incoTermsIds.Any())
                    {
                        item.NewlyAdded = "Yes, incoTerms Missing";
                    }
                    var plantIds = _emamiContext.UserDepotMapping.Where(_ => _.UserId == item.Id && _.Depot.IsPlant).Select(_ => _.Id).ToList();
                    if (item.SaudaValidityPeriod == 0)
                    {
                        item.NewlyAdded = "Yes, SaudaValidityPeriod Missing";
                    }

                    //if (item.TransportModeId == 0)
                    //{
                    //    item.NewlyAdded = "Yes, TransportModeId Missing";
                    //}

                    //if (item.SaudaBookingTypeId == 0)
                    //{
                    //    item.NewlyAdded = "Yes, SaudaBookingTypeId Missing";
                    //}

                    //if (item.SaudaLimit == 0)
                    //{
                    //    item.NewlyAdded = "Yes, SaudaLimit Missing";
                    //}
                    if (plantIds.Count == 0)
                    {
                        item.NewlyAdded = "Yes, PlantName Missing";
                    }
                    //if (string.IsNullOrEmpty(item.Depots))
                    //{
                    //    item.NewlyAdded = "Yes, DepotName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.FreightRouteName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightRouteName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.FreightZoneName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightZoneName Missing";
                    //}
                    if (string.IsNullOrEmpty(item.Zone))
                    {
                        item.NewlyAdded = "Yes, Zone Missing";
                    }

                    if (string.IsNullOrEmpty(item.State))
                    {
                        item.NewlyAdded = "Yes, State Missing";
                    }
                    //if (string.IsNullOrEmpty(item.Territory))
                    //{
                    //    item.NewlyAdded = "Yes, Territory Missing";
                    //}
                    if (string.IsNullOrEmpty(item.District))
                    {
                        item.NewlyAdded = "Yes, District Missing";
                    }
                    if (string.IsNullOrEmpty(item.City))
                    {
                        item.NewlyAdded = "Yes, City Missing";
                    }

                    #endregion


                    item.Password = item.Password != null ? UtilityHelper.ConvertMd5ToString(item.Password, SecurityConstants.EncryptionKey) : string.Empty;
                }

                //resultDto.SuccessDto.Response = dealerDto != null ? dealerDto.OrderByDescending(_ => _.Id).ToList() : dealerDto;
                result.Data = dealerDto;
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
        /// Method to get Get Dealer Details By Id
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public ResultDto GetDealerDetailsById(string dealerId)
        {
            _methodName = "GetDealerDetailsById";
            var resultDto = new ResultDto();
            var employeeDto = new EmployeeDto();
            try
            {
                dealerId = dealerId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(dealerId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var userContext = _emamiContext.Users.AsNoTracking();
                var resultContext = userContext.FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    var stateContext = _emamiContext.State.AsNoTracking();
                    var cityContext = _emamiContext.City.AsNoTracking();
                    var districtContext = _emamiContext.District.AsNoTracking();
                    var customerGroupFiveContext = _emamiContext.CustomerGroupFive.AsNoTracking();
                    var userroleContext = _emamiContext.UserRoles.AsNoTracking();                   
                    employeeDto.EncryptedId = dealerId;
                    employeeDto.DivisionList = GetUserDivisionInfo(Id);
                    employeeDto.Id = resultContext.Id;
                    employeeDto.Code = resultContext.Code;
                    employeeDto.Name = resultContext.Name;
                    employeeDto.MobileNumber = resultContext.MobileNumber;
                    employeeDto.Email = resultContext.Email;
                    employeeDto.IsActive = resultContext.IsActive;
                    employeeDto.SaudaValidityPeriod = Convert.ToInt32(resultContext.SaudaValidityPeriod);
                    employeeDto.DistrictId = resultContext.DistrictId;
                    employeeDto.District = districtContext.FirstOrDefault(s => s.Id == resultContext.DistrictId) != null ? districtContext.FirstOrDefault(s => s.Id == resultContext.DistrictId).DistrictName.Trim() : string.Empty;
                    employeeDto.ZoneId = resultContext.ZoneId;
                    employeeDto.StateId = resultContext.StateId;
                    employeeDto.State = stateContext.FirstOrDefault(s => s.Id == resultContext.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == resultContext.StateId).StateName.Trim() : string.Empty;
                    employeeDto.CityId = resultContext.CityId;
                    employeeDto.City = cityContext.FirstOrDefault(c => c.Id == resultContext.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == resultContext.CityId).CityName.Trim() : string.Empty;
                    employeeDto.Territory = resultContext.Territory;
                    employeeDto.TerritoryId = resultContext.TerritoryId;
                    employeeDto.Address1 = resultContext.Address1;
                    employeeDto.Address2 = resultContext.Address2;
                    employeeDto.GSTN = resultContext.GSTN;
                    employeeDto.TANNumber = resultContext.TANNumber;
                    employeeDto.CustClass = resultContext.CustClass;
                    employeeDto.VisitDay = resultContext.VisitDay;
                    employeeDto.WeeklyClosingDay = resultContext.WeeklyClosingDay;
                    employeeDto.MonthlyPotential = resultContext.MonthlyPotential;
                    employeeDto.TransportModeId = resultContext.TransportModeId;
                    employeeDto.SaudaBookingTypeId = resultContext.SaudaBookingTypeId;
                    employeeDto.Pincode = resultContext.Pincode;
                    employeeDto.IsSelf = resultContext.IsSelf;
                    employeeDto.IsBroker = resultContext.IsBroker;
                    employeeDto.FSSAINumber = resultContext.FSSAINumber;
                    employeeDto.InActiveRemarks = resultContext.InActiveRemarks;
                    employeeDto.CustomerGroupFiveId = resultContext.CustomerGroupFiveId;
                    employeeDto.InActiveRemarkId = resultContext.InActiveRemarkId;
                    employeeDto.AdditionalMobileNumber = resultContext.AdditionalMobileNumber;
                    employeeDto.ContactPersonName = resultContext.ContactPersonName;
                    employeeDto.IsActiveForCall = resultContext.IsActiveForCall;
                    employeeDto.CompanyCode = resultContext.CompanyCode == null ? string.Empty : resultContext.CompanyCode;
                    employeeDto.Role = userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id) != null && userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role != null ? userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role.Name : string.Empty;
                    employeeDto.RoleId = userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id) != null ? userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).RoleId : 0;
                    employeeDto.Password = !string.IsNullOrEmpty(resultContext.Password) ? UtilityHelper.ConvertMd5ToString(resultContext.Password, SecurityConstants.EncryptionKey) : string.Empty;
                    employeeDto.SelectedBrokerIds = GetUserCustomerIds(Id);
                    employeeDto.LineId = resultContext.LineId != null ? resultContext.LineId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(_ => Convert.ToInt64(_)).ToList() : null;
                    employeeDto.SelectedDealerIds = GetCustomerShipToParyIds(Id);                                                                                
                    employeeDto.ShipToPartyList = userContext.Where(_ => employeeDto.SelectedDealerIds.Contains(_.Id)).ToList().Select(_ => new ShipToPartyMappingDto()
                    {
                        Id = _.Id,
                        Name = _.Name,
                        City = cityContext.FirstOrDefault(c => c.Id == _.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == _.CityId).CityName.Trim() : string.Empty,
                        State = stateContext.FirstOrDefault(s => s.Id == _.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == _.StateId).StateName.Trim() : string.Empty,
                        District = districtContext.FirstOrDefault(s => s.Id == _.DistrictId) != null ? districtContext.FirstOrDefault(s => s.Id == _.DistrictId).DistrictName.Trim() : string.Empty
                    }).ToList();
                    employeeDto.SelectedDealerIdsCount = (employeeDto.SelectedDealerIds != null && employeeDto.SelectedDealerIds.Any()) ? employeeDto.SelectedDealerIds.Count : 0;
                    employeeDto.IncoTermsId = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == resultContext.Id).Select(s => s.IncoTermsId).ToList();
                    employeeDto.IncoTerms = string.Join(",", _emamiContext.IncoTerms.Where(w => employeeDto.IncoTermsId.Contains(w.Id)).Select(s => s.Name).ToList());
                    var plantContext = _emamiContext.Depots.Where(_ => _.IsPlant);
                    var plantIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => w.UserId == Id)
                        .Join(plantContext, a => a.DepotId, d => d.Id, (a, d) => new { userdepot = a, depot = d });
                    if (plantIds != null && plantIds.Any())
                    {
                        employeeDto.SelectedPlantIds = plantIds.Select(s => s.userdepot.DepotId).ToList();
                        employeeDto.PlantNames = string.Join(",", plantContext.Where(_ => employeeDto.SelectedPlantIds.Contains(_.Id)).Select(s => s.Name).ToList());
                    }
                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    employeeDto.PlantTruckCapacities = string.Join(",", plantcapacities);
                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    employeeDto.DepotTruckCapacities = string.Join(",", depotcapacities);
                    var dealerExistList = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                      .Join(_emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                      .Select(_ => _.UserCustomerMapping.User).ToList();
                    if (dealerExistList != null && dealerExistList.Any())
                    {
                        employeeDto.BrokerIds = dealerExistList.Select(_ => _.Id).ToList();
                        employeeDto.BrokerNames = string.Join(",", dealerExistList.Select(_ => _.Name).ToList());
                    }
                    employeeDto.Attachments = _emamiContext.ConsentImageDetailsForCustomers.AsNoTracking().Where(image => image.UserId == Id).Select(s => new SupportAttachmentDto()
                    {
                        FileName = s.FileName,
                        MediaPath = s.MediaPath,
                        MediaType = s.MediaType.Name,
                        ConsentImageId = s.Id
                    }).ToList();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = employeeDto;
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

        private List<DivisionDetailsDto> GetUserDivisionInfo(long UserId)
        {
            List<DivisionDetailsDto> divisionDetails = new List<DivisionDetailsDto>();

            try
            {
                if (UserId > 0)
                {
                    divisionDetails = _emamiContext.UserDivisionMappings.Where(mapping => mapping.UserId == UserId)
                        .Select(mapping => new DivisionDetailsDto
                        {
                            Id = mapping.Id,
                            DistributionChannel = mapping.DistributionChannel.Name,
                            DistributionChannelId = mapping.DistributionChannelId,
                            Division = mapping.Division.Name,
                            DivisionId = mapping.DivisionId,
                            SalesOrganization = mapping.SalesOrganization.Name,
                            SalesOrganizationId = mapping.SalesOrganizationId,
                            SaudaLimit = mapping.SaudaLimit ?? 0,
                            SaudaValidityPeriod = mapping.SaudaValidityPeriod ?? 0,
                        }).ToList();

                    foreach (var userdivision in divisionDetails)
                    {
                        userdivision.UserDivisionPlantCodes = GetUserDivisionPlantCodes(userdivision.Id);
                        userdivision.UserDivisionPlantIds = _emamiContext.UserDivisionDepotMappings
                        .Where(p => p.UserDivisionId == userdivision.Id)
                        .Select(p => p.DepotId)
                        .ToList();
                    }

                }

                return divisionDetails;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";             
                _logger.Error(message);
                return divisionDetails;
            }
        }

        public ResultDto DeleteConsentImage(BulletinInputDto inputDto)
        {
            _methodName = "DeleteConsentImage";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.LoginUserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }


                var deleteConsentImage = _emamiContext.ConsentImageDetailsForCustomers.FirstOrDefault(_ => _.Id == inputDto.BulletinMediaId);

                if (deleteConsentImage == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                _emamiContext.ConsentImageDetailsForCustomers.Remove(deleteConsentImage);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordDeleted, Utility.MessageLanguage);
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


        public ResultDto UploadConsentImage(List<DealerConsentImageUploadDto> inputDto)
        {
            _methodName = "UploadConsentImage";
            var resultDto = new ResultDto();
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var data in inputDto)
                {
                    long verticalId = 0;
                    long saudaBookingTypeId = 0;
                    bool errorflag = false;
                    if (string.IsNullOrEmpty(data.DivisionCode))
                    {
                        data.Message = Constants.VerticalCodeEmpty;
                        errorflag = true;
                    }
                    else
                    {
                        var verticalContext = _emamiContext.Divisions.AsNoTracking().FirstOrDefault(_ => _.Code == data.DivisionCode);
                        if (verticalContext == null)
                        {
                            data.Message = Constants.VerticalNotExists;
                            errorflag = true;
                        }
                        else
                        {
                            verticalId = verticalContext.Id;
                        }

                    }

                    if (string.IsNullOrEmpty(data.SaudaBookingType))
                    {
                        data.Message = Constants.SaudaBookingTypeEmpty;
                        errorflag = true;
                    }
                    else
                    {
                        var bookingTypeContext = _emamiContext.SaudaBookingTypes.AsNoTracking().FirstOrDefault(_ => _.Name == data.SaudaBookingType);
                        if (bookingTypeContext == null)
                        {
                            data.Message = Constants.SaudaBookingTypeNotExists;
                            errorflag = true;
                        }
                        else
                        {
                            saudaBookingTypeId = bookingTypeContext.Id;
                        }
                    }
                    var IsactiveForCallToCustomer = (data.ActiveForCallToCustomers == "1") ? true : false;
                    var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Code == data.DealerCode
                    //&& _.DivisionId == verticalId 
                    && _.SaudaBookingTypeId == saudaBookingTypeId);
                    if (userContext == null)
                    {
                        data.Message = Constants.UserNotFound;
                        errorflag = true;
                    }

                    if (!errorflag)
                    {
                        userContext.IsActiveForCall = IsactiveForCallToCustomer;
                        userContext.ModifiedBy = data.ModifiedBy;
                        userContext.AdditionalMobileNumber = data.AdditionalMobileNumber;
                        userContext.ContactPersonName = data.ContactPersonName;
                        userContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                        data.Message = "Data uploaded successfully";
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = inputDto;
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
        /// Method to Get Dealer Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetDealerBrokerList(LoginUserIdDto inputDto)
        {
            _methodName = "GetDealerBrokerList";
            var resultDto = new ResultDto();
            var dealerDto = new List<DealerDto>();
            try
            {
                IQueryable<User> entity;
                if (inputDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer && _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Select(_ => _.User);
                }
                else
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer && _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Select(_ => _.User);
                    entity = entity.Where(_ => _.IsActive);
                }
                dealerDto = entity.ToList().OrderBy(_ => _.Name).Select(c => new DealerDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    IsActive = c.IsActive,
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dealerDto;
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

        #region Broker

        /// <summary>
        /// Method to Get Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetBrokerList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBrokerList";
            var resultDto = new ResultDto();
            var brokerDto = new List<BrokerDto>();
            try
            {
                List<User> entity;
                if (inputDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker).Select(_ => _.User).ToList();
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0));
                }
                else
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker).Select(_ => _.User).ToList();
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0) && _.IsActive);
                    entity = entity.Where(_ => _.IsActive).ToList();
                }

                brokerDto = entity.AsEnumerable().OrderBy(_ => _.Name).Select(c => new BrokerDto
                {
                    Id = c.Id,
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    BrokerCode = c.Code,
                    BrokerName = c.Name,
                    MobileNumber = c.MobileNumber,
                    MobileNumber2 = c.MobileNumber2,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    ZoneId = c.ZoneId,
                    //  Zone = c.Zone?.Name,
                    City = c.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.CityId).CityName : string.Empty,
                    District = c.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == c.DistrictId).DistrictName : string.Empty,
                    State = c.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == c.StateId).StateName : string.Empty,
                    Territory = c.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(_ => _.Id == c.TerritoryId).Name : string.Empty,
                    //Address1 = c.Address1,
                    //Address2 = c.Address2,
                    Address = c.Address1,
                    GSTN = c.GSTN,
                    VisitDay = c.VisitDay,
                    //  SaudaBookingType = c.SaudaBookingType != null ? c.SaudaBookingType.Name : string.Empty,
                    //FreightZoneName = c.FreightZone != null ? c.FreightZone.Name : string.Empty,
                    //FreightRouteName = c.FreightRoute != null ? c.FreightRoute.Name : string.Empty,
                    SaudaValidityPeriod = Convert.ToInt32(c.SaudaValidityPeriod),
                    WeeklyClosingDay = c.WeeklyClosingDay,
                    MonthlyPotential = c.MonthlyPotential,
                    //Incoterms = c.IncoTerms != null ? c.IncoTerms.Name : string.Empty,
                    // Loadability = c.Loadability,
                    //SaudaLimit = c.SaudaLimit,
                    //TransportMode = c.TransportMode != null ? c.TransportMode.Name : string.Empty,
                    //   SaudaType = c.SaudaBookingType != null ? c.SaudaBookingType.Name : string.Empty,
                    Pincode = c.Pincode,
                    TransportModeId = c.TransportModeId,
                    SaudaBookingTypeId = c.SaudaBookingTypeId,
                    //VerticalCode = c.Division != null ? c.Division.Code : string.Empty,
                    //VerticalName = c.Division != null ? c.Division.Name : string.Empty,
                    // PlantTruckCapacity = c.Loadability,
                    //DepotTruckCapacity = c.DepotLoadability,
                    FSSAINumber = c.FSSAINumber,
                    Password = c.Password != null ? UtilityHelper.ConvertMd5ToString(c.Password, SecurityConstants.EncryptionKey) : string.Empty,
                    AdditionalMobileNumber = c.AdditionalMobileNumber,
                    ContactPersonName = c.ContactPersonName,
                    IsActiveForCall = c.IsActiveForCall,
                    CompanyCode = c.CompanyCode == null ? string.Empty : c.CompanyCode
                }).ToList();

                foreach (var item in brokerDto)
                {

                    //if (item.SaudaValidityPeriod == 0)
                    //{
                    //    item.NewlyAdded = "Yes, SaudaValidityPeriod Missing";
                    //}

                    //if (item.TransportModeId == 0)
                    //{
                    //    item.NewlyAdded = "Yes, TransportModeId Missing";
                    //}

                    //if (item.SaudaBookingTypeId == 0)
                    //{
                    //    item.NewlyAdded = "Yes, SaudaBookingTypeId Missing";
                    //}

                    //if (item.SaudaLimit == 0)
                    //{
                    //    item.NewlyAdded = "Yes, SaudaLimit Missing";
                    //}

                    var divisions = _emamiContext.UserDivisionMappings.AsNoTracking().Where(w => w.UserId == item.Id).Select(x => x.Division).ToList();
                    if (divisions != null && divisions.Any())
                    {
                        item.VerticalName = string.Join(",", divisions.Select(_ => _.SalesOrganization.Code+"/"+_.DistributionChannel.Code+"/"+_.Code).Distinct().ToList());
                    }
                    var incoTermsIds = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == item.Id).Select(s => s.IncoTermsId).ToList();
                    //if (incoTermsIds == null || !incoTermsIds.Any())
                    //{
                    //    item.NewlyAdded = "Yes, incoTerms Missing";
                    //}

                    var incoterms = _emamiContext.UserIncoTerms.Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.IncoTerms, uic => uic.IncoTermsId, ic => ic.Id, (UserIncoTerms, IncoTerms) => new { IncoTerms }).Select(_ => _.IncoTerms).ToList();
                    if (incoterms != null && incoterms.Any())
                    {
                        item.Incoterms = string.Join(",", incoterms.Select(_ => _.Name.Trim()).Distinct().ToList());
                    }

                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    item.PlantTruckCapacities = string.Join(",", plantcapacities);

                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    item.DepotTruckCapacities = string.Join(",", depotcapacities);
                    //var depotContext = from users in _emamiContext.Users
                    //                   join userDepotMapping in _emamiContext.UserDepotMapping on users.Id equals userDepotMapping.UserId
                    //                   join depot in _emamiContext.Depots on userDepotMapping.DepotId equals depot.Id
                    //                   select new { Depot = depot };
                    //if (depotContext != null)
                    //{
                    //    item.PlantName = depotContext.FirstOrDefault(_ => _.Depot.IsPlant)?.Depot?.Name;
                    //    item.DepotName = depotContext.FirstOrDefault(_ => !_.Depot.IsPlant)?.Depot?.Name;
                    //}


                    //if (string.IsNullOrEmpty(item.FreightRouteName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightRouteName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.FreightZoneName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightZoneName Missing";
                    //}
                    if (string.IsNullOrEmpty(item.Zone))
                    {
                        item.NewlyAdded = "Yes, Zone Missing";
                    }
                    if (string.IsNullOrEmpty(item.State))
                    {
                        item.NewlyAdded = "Yes, State Missing";
                    }
                    if (string.IsNullOrEmpty(item.Territory))
                    {
                        item.NewlyAdded = "Yes, Territory Missing";
                    }
                    if (string.IsNullOrEmpty(item.District))
                    {
                        item.NewlyAdded = "Yes, District Missing";
                    }
                    if (string.IsNullOrEmpty(item.City))
                    {
                        item.NewlyAdded = "Yes, City Missing";
                    }

                    var depotPlantListContext = _emamiContext.UserDepotMapping.AsNoTracking().Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.DepotId, d => d.Id, (x, d) => new { StorageTypeId = d.StorageTypeId, Depot = d.Name, DepotCode = d.Code });

                    var depotListContext = depotPlantListContext.Where(_ => (_.StorageTypeId == (int)DTO.Enums.StorageType.Depot || _.StorageTypeId == (int)DTO.Enums.StorageType.Rake))
                        .Select(_ => _.DepotCode).ToList();

                    var plantListContext = depotPlantListContext.Where(_ => _.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(_ => _.DepotCode).ToList();

                    if (depotListContext != null && depotListContext.Any())
                    {
                        item.Depots = UtilityHelper.ConvertStringListToCommaSeparatedString(depotListContext);
                    }
                    if (plantListContext != null && plantListContext.Any())
                    {
                        item.Plants = UtilityHelper.ConvertStringListToCommaSeparatedString(plantListContext);
                    }

                    //if (string.IsNullOrEmpty(item.Plants))
                    //{
                    //    item.NewlyAdded = "Yes, PlantName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.Depots))
                    //{
                    //    item.NewlyAdded = "Yes, DepotName Missing";
                    //}

                    var bdoContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == item.Id)
                        .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader), x => x.UserId, ur => ur.UserId, (ucm, ur) => new { ucm, ur })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.ucm.UserId, u => u.Id, (x, u) => new { u })
                        .Where(_ => _.u != null).Select(_ => new { _.u.Name, _.u.Code }).FirstOrDefault();
                    if (bdoContext != null)
                    {
                        item.StateTrader = bdoContext.Name;
                        item.BDOCode = bdoContext.Code;
                    }

                    var dealerCodeList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.CustomerId, u => u.Id, (x, u) => new { u })
                        .Where(_ => _.u != null).Select(_ => _.u.Code).ToList();
                    if (dealerCodeList != null && dealerCodeList.Any())
                    {
                        item.DealerCodeList = UtilityHelper.ConvertStringListToCommaSeparatedString(dealerCodeList);
                    }

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = brokerDto != null ? brokerDto.OrderByDescending(_ => _.Id).ToList() : brokerDto;
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
        /// Method to get Get Broker Details By Id
        /// </summary>
        /// <param name="brokerId"></param>
        /// <returns></returns>
        public ResultDto GetBrokerDetailsById(string brokerId)
        {
            _methodName = "GetBrokerDetailsById";
            var resultDto = new ResultDto();
            var employeeDto = new EmployeeDto();
            try
            {
                brokerId = brokerId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(brokerId, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    var userroleContext = _emamiContext.UserRoles.AsNoTracking();
                    var userDivisionMapping = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == Id).Select(_ => new DivisionDetailsDto
                    {
                        DistributionChannel = _.DistributionChannel.Name,
                        DistributionChannelId = _.DistributionChannelId,
                        Division = _.Division.Name,
                        DivisionId = _.DivisionId,
                        SalesOrganization = _.SalesOrganization.Name,
                        SalesOrganizationId = _.SalesOrganizationId,
                    });
                    employeeDto.EncryptedId = brokerId;
                    employeeDto.DivisionList = userDivisionMapping.ToList();
                    employeeDto.Id = resultContext.Id;
                    employeeDto.Code = resultContext.Code;
                    employeeDto.Name = resultContext.Name;
                    employeeDto.MobileNumber = resultContext.MobileNumber;
                    employeeDto.MobileNumber2 = resultContext.MobileNumber2;
                    employeeDto.Email = resultContext.Email;
                    employeeDto.IsActive = resultContext.IsActive;
                    employeeDto.DistrictId = resultContext.DistrictId;
                    employeeDto.District = resultContext.District;
                    employeeDto.ZoneId = resultContext.ZoneId;
                    //  employeeDto.Zone = resultContext.Zone?.Name;
                    employeeDto.StateId = resultContext.StateId;
                    employeeDto.State = resultContext.State;
                    employeeDto.CityId = resultContext.CityId;
                    employeeDto.City = resultContext.City;
                    employeeDto.TerritoryId = resultContext.TerritoryId;
                    employeeDto.Territory = resultContext.Territory;
                    //employeeDto.Address = resultContext.Address;
                    employeeDto.Address1 = resultContext.Address1;
                    employeeDto.Address2 = resultContext.Address2;
                    employeeDto.GSTN = resultContext.GSTN;
                    employeeDto.VisitDay = resultContext.VisitDay;
                    employeeDto.SaudaValidityPeriod = Convert.ToInt32(resultContext.SaudaValidityPeriod);
                    employeeDto.WeeklyClosingDay = resultContext.WeeklyClosingDay;
                    employeeDto.MonthlyPotential = resultContext.MonthlyPotential;
                    //employeeDto.IncoTermsId = resultContext.IncoTermsId;
                    employeeDto.TransportModeId = resultContext.TransportModeId;
                    employeeDto.SaudaBookingTypeId = resultContext.SaudaBookingTypeId;
                    //employeeDto.PlantTruckCapacity = resultContext.Loadability;
                    employeeDto.Pincode = resultContext.Pincode;
                    //employeeDto.SaudaLimit = resultContext.SaudaLimit;
                    //employeeDto.FreightRouteId = resultContext.FreightRouteId;
                    //employeeDto.FreightZoneId = resultContext.FreightZoneId;
                    //employeeDto.VerticalId = resultContext.DivisionId;
                    employeeDto.FSSAINumber = resultContext.FSSAINumber;
                    // employeeDto.DepotTruckCapacity = resultContext.DepotLoadability;
                    //employeeDto.CustomerGroupOneId = resultContext.CustomerGroupOneId;
                    //employeeDto.CustomerGroupTwoId = resultContext.CustomerGroupTwoId;
                    employeeDto.AdditionalMobileNumber = resultContext.AdditionalMobileNumber;
                    employeeDto.ContactPersonName = resultContext.ContactPersonName;
                    employeeDto.IsActiveForCall = resultContext.IsActiveForCall;
                    employeeDto.CompanyCode = resultContext.CompanyCode == null ? string.Empty : resultContext.CompanyCode;
                    employeeDto.Role = userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id) != null && userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role != null ? userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id).Role.Name : string.Empty;
                    employeeDto.RoleId = userroleContext.FirstOrDefault(_ => _.UserId == resultContext.Id) != null ? _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == resultContext.Id).RoleId : 0;
                    if (!string.IsNullOrEmpty(resultContext.Password))
                    {
                        employeeDto.Password = UtilityHelper.ConvertMd5ToString(resultContext.Password, SecurityConstants.EncryptionKey);
                    }
                    employeeDto.SelectedDealerIds = GetUserCustomerIds(Id);
                    //employeeDto.SelectedDepotIds = GetUserDepotIds(brokerId);
                    employeeDto.SelectedDealerIdsCount = (employeeDto.SelectedDealerIds != null && employeeDto.SelectedDealerIds.Any()) ? employeeDto.SelectedDealerIds.Count : 0;
                    employeeDto.IncoTermsId = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == resultContext.Id).Select(s => s.IncoTermsId).ToList();

                    var plantIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => w.UserId == Id)
                        .Join(_emamiContext.Depots.AsNoTracking().Where(_ => _.IsPlant), a => a.DepotId, d => d.Id, (a, d) => new { userdepot = a, depot = d });
                    if (plantIds != null && plantIds.Any())
                    {
                        //employeeDto.PlantId = plantIds.FirstOrDefault().userdepot.DepotId;
                        employeeDto.SelectedPlantIds = plantIds.Select(s => s.userdepot.DepotId).ToList();
                    }

                    //var depotIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => w.UserId == brokerId)
                    //    .Join(_emamiContext.Depots.AsNoTracking().Where(_ => !_.IsPlant), a => a.DepotId, d => d.Id, (a, d) => new { userdepot = a, depot = d });
                    //if (depotIds != null && depotIds.Any())
                    //{
                    //    //employeeDto.DepotId = depotIds.FirstOrDefault().userdepot.DepotId;
                    //    employeeDto.SelectedDepotIds = depotIds.Select(s => s.userdepot.DepotId).ToList();
                    //}

                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    employeeDto.PlantTruckCapacities = string.Join(",", plantcapacities);

                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    employeeDto.DepotTruckCapacities = string.Join(",", depotcapacities);

                    employeeDto.Attachments = _emamiContext.ConsentImageDetailsForCustomers.AsNoTracking().Where(image => image.UserId == Id).Select(s => new SupportAttachmentDto()
                    {
                        FileName = s.FileName,
                        MediaPath = s.MediaPath,
                        MediaType = s.MediaType.Name,
                        ConsentImageId = s.Id
                    }).ToList();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = employeeDto;
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
        /// Method to Get Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetBrokerListddl(DealerBrokerParamDto inputDto)
        {
            _methodName = "GetBrokerListddl";
            var resultDto = new ResultDto();
            var brokerDto = new List<DropDownDto>();
            try
            {

                //brokerDto = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                //    .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.Broker && w.Users.VerticalId == inputDto.VerticalId && w.Users.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                //    .Select(s => new DropDownDto
                //    {
                //        Id = s.Users.Id,
                //        Name = s.Users.Name
                //    }).ToList();
                brokerDto = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Where(w => w.UserRoles.RoleId == (int)DTO.Enums.Role.Broker && w.Users.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                    .Select(s => new DropDownDto
                    {
                        Id = s.Users.Id,
                        Name = s.Users.Name
                    }).ToList();
                var userIds = _emamiContext.UserDivisionMappings.Where(_ => inputDto.DivisionIds.Contains(_.DivisionId)).Select(_ => _.UserId);
                brokerDto = brokerDto.Where(_ => userIds.Contains(_.Id)).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = brokerDto;
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

        /// <summary>
        /// Method to get get StateTrader statistics
        /// </summary>
        /// <param name="BDOId"></param>
        /// <returns></returns>
        public ResultDto GetBDOStatistics(SaudaFilterDto inputDto)
        {
            _methodName = "GetBDOStatistics";
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var BDOStatistics = new UserStatisticsOutputDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.UserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (roleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RoleNotFound);
                }
                if (inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }


                if (roleContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                    BDOStatistics.DealersCount = _emamiContext.UserCustomerMapping.AsNoTracking().Count(_ => _.UserId == inputDto.UserId && _.User.IsActive);

                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.UserId);

                var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                
                if (dealersList != null && dealersList.Any())
                {

                    var PendingContractContext = new List<PendingContractStatistics>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                            Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                            insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                            insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings where UserId=@UserId
                                            select
                                            pc.SaudaQuantity as PendingQuantityInMT,
                                            pc.ContractValidTo
                                            from PendingContracts pc with(NOLOCK)
                                            join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId
                                            and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                                            join #UserDivision ud on pc.SalesOrgId=ud.SalesOrganizationId
                                            and pc.DistChnlId=ud.DistributionChannelId and pc.DivisionId=ud.DivisionId
                                            where pc.UserId in (select DealerId from #DealerTemp)

                                              drop table #DealerTemp
                                              drop table #UserDivision";

                        PendingContractContext = conn.Query<PendingContractStatistics>(sqlQuery, new
                        {
                            UserId = inputDto.UserId
                        }).ToList();

                    }
                    if (PendingContractContext != null && PendingContractContext.Any())
                    {
                        BDOStatistics.PendingSaudaQuantity = PendingContractContext.ToList().Select(_ => _.PendingQuantityInMT).DefaultIfEmpty(0).Sum();
                    }
                    if (PendingContractContext != null && PendingContractContext.Any())
                    {
                        var ExpiredContextList = PendingContractContext.Where(_ => _.ContractValidTo.Date < currentDate.Date).ToList();
                        
                        var NearExpiredContextList = PendingContractContext.Where(_ =>( _.ContractValidTo.Date- currentDate.Date).Days < 5 && ( _.ContractValidTo.Date- currentDate.Date).Days >= 1).ToList();

                        if (ExpiredContextList != null && ExpiredContextList.Any())
                        {
                            BDOStatistics.AboveOutstandingSaudaQuantity = ExpiredContextList.Select(_ => _.PendingQuantityInMT).DefaultIfEmpty(0).Sum();
                        }
                        if (NearExpiredContextList != null && NearExpiredContextList.Any())
                        {
                            BDOStatistics.BelowOutstandingSaudaQuantity = NearExpiredContextList.Select(_ => _.PendingQuantityInMT).DefaultIfEmpty(0).Sum();
                        }
                    }

                    //var PendingContractContext = _emamiContext.PendingContracts.AsNoTracking()
                    //                        .Join(_emamiContext.Users.AsNoTracking(), pc => pc.UserId, us => us.Id, (sr, us) => new { PendingContract = sr, User = us })
                    //                        .Join(_emamiContext.Skus.AsNoTracking(), c => c.PendingContract.MaterialCode, sku => sku.SkuCode, (c, sku) => new { PendingContract = c.PendingContract, User = c.User, Sku = sku })
                    //                        .Join(_emamiContext.Sauda.AsNoTracking(), c => c.PendingContract.SaudaNumber, sauda => sauda.SaudaNumber, (c, sauda) => new { PendingContract = c.PendingContract, User = c.User, Sku = c.Sku , Sauda = sauda })
                    //                        .Where(_ => _.PendingContract != null && dealersList.Any(a => a.CustomerId == _.User.Id) && _.PendingContract.SalesOrgId == _.Sku.SalesOrganizationId && _.PendingContract.DistChnlId == _.Sku.DistributionChannelId
                    //                        && _.PendingContract.DivisionId == _.Sku.DivisionId
                    //                        ).Select(_ => new { _.PendingContract }).ToList();

                    //var PendingContractContext = (from p in _emamiContext.PendingContracts.AsNoTracking()
                    //                              join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                             // join s in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals s.SaudaNumber
                    //                              join dm in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId }
                    //                              equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                              join dl in dealersList on p.UserId equals dl.CustomerId
                    //                              where p.SalesOrgId == sku.SalesOrganizationId
                    //                               //&& DbFunctions.TruncateTime(p.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                    //                              && p.DistChnlId == sku.DistributionChannelId
                    //                              && p.DivisionId == sku.DivisionId
                    //                              select new { SaudaQuantity = p.SaudaQuantity, ContractValidTo = p.ContractValidTo }
                    //         );

                    //if (PendingContractContext != null && PendingContractContext.Any())
                    //{
                    //    BDOStatistics.PendingSaudaQuantity = PendingContractContext.ToList().Select(_ => _.SaudaQuantity).DefaultIfEmpty(0).Sum();
                    //}



                    //var outStandingContextList = _emamiContext.PendingContracts.AsNoTracking()
                    //                     .Join(_emamiContext.Users.AsNoTracking(), pc => pc.UserId, us => us.Id, (sr, us) => new { PendingContract = sr, User = us })
                    //                     .Join(_emamiContext.Skus.AsNoTracking(), c => c.PendingContract.MaterialCode, sku => sku.SkuCode, (c, sku) => new { PendingContract = c.PendingContract, User = c.User, Sku = sku })
                    //                     .Join(_emamiContext.Sauda.AsNoTracking(), c => c.PendingContract.SaudaNumber, sauda => sauda.SaudaNumber, (c, sauda) => new { PendingContract = c.PendingContract, User = c.User, Sku = c.Sku, Sauda = sauda })
                    //                     .Where(_ => _.PendingContract != null && dealersList.Any(a => a.CustomerId == _.User.Id)
                    //                     && _.PendingContract.SalesOrgId == _.Sku.SalesOrganizationId && _.PendingContract.DistChnlId == _.Sku.DistributionChannelId
                    //                        && _.PendingContract.DivisionId == _.Sku.DivisionId)
                    //                     .Select(_ => new { _.PendingContract });

                    //var outStandingContextList = (from p in _emamiContext.PendingContracts.AsNoTracking()
                    //                              join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                              // join s in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals s.SaudaNumber
                    //                              join dm in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId }
                    //                              equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                              join dl in dealersList on p.UserId equals dl.CustomerId
                    //                              where p.SalesOrgId == sku.SalesOrganizationId
                    //                              && p.DistChnlId == sku.DistributionChannelId
                    //                              && p.DivisionId == sku.DivisionId
                    //                              select new { SaudaQuantity = p.SaudaQuantity, ContractValidTo = p.ContractValidTo }
                    //        );

                    //if (outStandingContextList != null && outStandingContextList.Any())
                    //{
                    //    var ExpiredContextList = outStandingContextList.Where(_ => DbFunctions.TruncateTime(_.ContractValidTo) < DbFunctions.TruncateTime(currentDate)).ToList();
                    //    var NearExpiredContextList = outStandingContextList.Where(_ => DbFunctions.DiffDays(currentDate, _.ContractValidTo) < 5 && DbFunctions.DiffDays(currentDate, _.ContractValidTo) >= 1).ToList();

                    //    if (ExpiredContextList != null && ExpiredContextList.Any())
                    //    {
                    //        BDOStatistics.AboveOutstandingSaudaQuantity = ExpiredContextList.Select(_ => _.SaudaQuantity).DefaultIfEmpty(0).Sum();
                    //    }
                    //    if (NearExpiredContextList != null && NearExpiredContextList.Any())
                    //    {
                    //        BDOStatistics.BelowOutstandingSaudaQuantity = NearExpiredContextList.Select(_ => _.SaudaQuantity).DefaultIfEmpty(0).Sum();
                    //    }
                    //}

                    var dealerIds = dealersList.Select(_ => _.CustomerId).ToList();


                    var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId));
                    if (overduePaymentContext != null && overduePaymentContext.Any())
                    {
                        var tomDate = currentDate.AddDays(1);
                        decimal TotalDueForTomorrow = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        decimal TotalOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        BDOStatistics.TotalDueForTomorrow = TotalDueForTomorrow;
                        BDOStatistics.TotalOverDue = TotalOverDue;
                    }


                    //var specialRatesContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId) && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(currentDate)
                    //    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(currentDate) && _.StatusId == (int)DTO.Enums.Status.Pending).ToList();

                    BDOStatistics.TotalSpecialRateApproval = (from s in _emamiContext.Sauda.AsNoTracking()
                                                              join sr in _emamiContext.SpecialRate.AsNoTracking() on s.SpecialRateRequestIdInParentTable equals sr.Id
                                                              join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                                              equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                                                              join d in dealersList on sr.UserId equals d.CustomerId
                                                              where DbFunctions.TruncateTime(sr.CreatedDate) >= DbFunctions.TruncateTime(currentDate)
                                                         && DbFunctions.TruncateTime(sr.CreatedDate) <= DbFunctions.TruncateTime(currentDate) && sr.StatusId == (int)DTO.Enums.Status.Pending
                                                              select sr).Count();
                    //if (specialRatesContext != null && specialRatesContext.Any())
                    //{
                    //    BDOStatistics.TotalSpecialRateApproval = specialRatesContext.Count();
                    //}

                }

                //var dashboardOverallsaudaOutpuDto = new List<OverallPerformanceByUserOutputDto>();
                //var OrderOutpuDto = new List<OverallPerformanceByUserOutputDto>();
                //DateTime FromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                //DateTime ToDate = FromDate.AddMonths(1).AddDays(-1);
                //var userRoleContext = (from user in _emamiContext.Users.AsNoTracking()
                //                       join role in _emamiContext.UserRoles.AsNoTracking() on user.Id equals role.UserId
                //                       where role.RoleId == (int)DTO.Enums.Role.StateTrader && user.IsActive
                //                       //&& user.DivisionId == userContext.DivisionId
                //                       select new UserMasterDto
                //                       {
                //                           Id = user.Id,
                //                           EmployeeName = user.Name,
                //                           EmployeeCode = user.Code
                //                       }).ToList();

                //var InvoiceDetailContext = _emamiContext.InvoiceDetails.AsNoTracking().ToList();
                //var InvoiceDetailContext = (from i in _emamiContext.Invoices.AsNoTracking()
                //         join inv in _emamiContext.InvoiceDetails.AsNoTracking() on i.Id equals inv.InvoiceId
                //         join dm in divisionslogieduser on new { SalesOrganizationId = inv.SalesOrganizationId, DistributionChannelId = inv.DistributionChannelId, DivisionId = inv.DivisionId }
                //         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //         where DbFunctions.TruncateTime(i.InvoiceDate) >= DbFunctions.TruncateTime(FromDate) &&
                //        DbFunctions.TruncateTime(i.InvoiceDate) <= DbFunctions.TruncateTime(ToDate)
                //        select new { UserId = i.UserId , InvoiceId = i.Id , Quantity = inv.ActualBilledQuantity}).ToList();
                ////var InvoiceContext = _emamiContext.Invoices.AsNoTracking().Where(_ => ).ToList();
                //var UserContext = _emamiContext.Users.AsNoTracking().ToList();
                //var CustomerSalesTargetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ =>  _.MonthId == FromDate.Month && _.Year == FromDate.Year).ToList();
                //var UserCustomerMapping = _emamiContext.UserCustomerMapping.AsNoTracking().ToList();
                //foreach (var user in userRoleContext)
                //{
                //    var targetContext = CustomerSalesTargetContext.Where(_ => _.AssignedToId == user.Id).ToList();
                //    var dealerlist = (from ucm in UserCustomerMapping
                //                      join u in UserContext on ucm.CustomerId equals u.Id
                //                      where ucm.UserId == user.Id
                //                      select ucm.CustomerId).ToList();
                //    if (dealerlist != null && dealerlist.Any())
                //    {
                //        var salesContext = InvoiceDetailContext.Where(_ => dealerlist.Contains(_.UserId)).ToList();

                //        //  if (salesContext != null && salesContext.Any())
                //        //{
                //        decimal Achievement = salesContext.Select(_ => _.Quantity).DefaultIfEmpty(0).Sum();

                //        //foreach (var item in salesContext)
                //        //{
                //        //    var invoiceDetailContext = InvoiceDetailContext.Where(_ => _.InvoiceId == item.Id);
                //        //    Achievement = Achievement + invoiceDetailContext.Sum(_ => (decimal?)_.ActualBilledQuantity) ?? 0;
                //        //}
                //        var usercontext = UserContext.FirstOrDefault(_ => _.Id == user.Id);
                //        //if (targetContext.IsAny())
                //        //{
                //        var acheivment = new OverallPerformanceByUserOutputDto
                //        {
                //            UserId = usercontext.Id,
                //            Usercode = usercontext.Code,
                //            Username = usercontext.Name,
                //            UserTarget = (targetContext.Sum(_ => _.Target) > 0) ? targetContext.Sum(_ => _.Target) : 0,
                //            UserAchievment = Achievement,
                //            AchievmentPercentage = (targetContext.Sum(_ => _.Target) > 0 && Achievement > 0) ? (Achievement / targetContext.Sum(_ => _.Target)) * 100 : 0
                //        };
                //        dashboardOverallsaudaOutpuDto.Add(acheivment);
                //        // }
                //        // }
                //    }
                //}
                //if (dashboardOverallsaudaOutpuDto != null && dashboardOverallsaudaOutpuDto.Any())
                //{
                //    OrderOutpuDto.AddRange(dashboardOverallsaudaOutpuDto.OrderBy(_ => _.AchievmentPercentage));

                //}

                BDOStatistics.CurrentDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                //bool isApplySpecialityFatDiscount = false;
                //var applySpecialityFatDiscount = Utility.GetEnumDescription(DTO.Enums.Configuration.IsApplySpecialityFatDiscount);
                //var configurationSpecialityFatDiscountContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == applySpecialityFatDiscount);
                //if (configurationSpecialityFatDiscountContext != null)
                //{
                //    isApplySpecialityFatDiscount = Convert.ToBoolean(configurationSpecialityFatDiscountContext.Value);
                //}
                //BDOStatistics.IsApplySpecialityFatDiscount = isApplySpecialityFatDiscount;
                if (BDOStatistics != null)
                {
                    return _resultService.SuccessObject(BDOStatistics);
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

        public ResultDto GetKeyPerformanceIndicator(IdInputDto inputDto)
        {
            _methodName = "GetKeyPerformanceIndicator";
            var resultDto = new ResultDto();
            var kpiDto = new List<KeyPerformanceIndicatorDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.Id <= 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            try
            {
                kpiDto = _emamiContext.KeyPerformanceIndicator.AsNoTracking().Where(_ => _.RoleId == inputDto.Id && _.IsActive).AsNoTracking().Select(c => new KeyPerformanceIndicatorDto
                {
                    RoleId = c.RoleId,
                    Content = c.Content,
                }).ToList();

                return _resultService.SuccessMessageWitObject(kpiDto, string.Empty);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        #region User Target
        public ResultDto GetUserAssignedTo(IdInputDto inputDto)
        {
            _methodName = "GetUserAssignedTo";
            var resultDto = new ResultDto();
            var userDto = new List<UserMasterDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.LoginUserId <= 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            try
            {
                if (inputDto.Id == (int)DTO.Enums.Role.StateTrader)
                {


                    userDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                               join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                               join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals ud.UserId
                               where ucm.UserId == inputDto.LoginUserId 
                               && (inputDto.SalesOrganizationId > 0 ? inputDto.SalesOrganizationId == ud.SalesOrganizationId : ud.SalesOrganizationId > 0)
                               && (inputDto.DistributionChannelId > 0 ? inputDto.DistributionChannelId == ud.DistributionChannelId : ud.DistributionChannelId > 0)
                               && (inputDto.DivisionId > 0 ? inputDto.DivisionId == ud.DivisionId : ud.DivisionId > 0)
                               select new UserMasterDto
                               {
                                   Id = u.Id,
                                   EmployeeName = u.Name,
                                   EmployeeCode = u.Code
                               }).Distinct().ToList();
                    //userDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                    //           join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                    //           where ucm.UserId == inputDto.LoginUserId
                    //           select new UserMasterDto
                    //           {
                    //               Id = u.Id,
                    //               EmployeeName = u.Name,
                    //               EmployeeCode = u.Code
                    //           }).ToList();
                }
                else
                {

                    userDto=(from urm in _emamiContext.UserReportingToMappings.AsNoTracking()
                             join u in _emamiContext.Users.AsNoTracking() on urm.UserId equals u.Id
                             join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on urm.UserId equals ud.UserId
                             where urm.ReportingToUserId==inputDto.LoginUserId
                             && u.IsActive
                             && (inputDto.SalesOrganizationId > 0 ? inputDto.SalesOrganizationId == ud.SalesOrganizationId : ud.SalesOrganizationId > 0)
                               && (inputDto.DistributionChannelId > 0 ? inputDto.DistributionChannelId == ud.DistributionChannelId : ud.DistributionChannelId > 0)
                               && (inputDto.DivisionId > 0 ? inputDto.DivisionId == ud.DivisionId : ud.DivisionId > 0)
                               //&& (inputDto.StateId > 0 ? inputDto.StateId == u.StateId : u.StateId > 0)
                               select new UserMasterDto()
                               {
                                   Id = u.Id,
                                   EmployeeName = u.Name,
                                   EmployeeCode = u.Code
                               }
                             ).Distinct().ToList();

                    //userDto = _emamiContext.Users.AsNoTracking()
                    //    .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), u => u.Id, ud => ud.UserId, (u, ud) => new { u, ud })
                    //    .Where(_ => _.u.ReportingToId == inputDto.LoginUserId && _.u.IsActive
                    //    && (inputDto.SalesOrganizationId > 0 ? inputDto.SalesOrganizationId == _.ud.SalesOrganizationId : _.ud.SalesOrganizationId > 0)
                    //           && (inputDto.DistributionChannelId > 0 ? inputDto.DistributionChannelId == _.ud.DistributionChannelId : _.ud.DistributionChannelId > 0)
                    //           && (inputDto.DivisionId > 0 ? inputDto.DivisionId == _.ud.DivisionId : _.ud.DivisionId > 0)
                    //    ).AsNoTracking().Select(c => new UserMasterDto
                    //    {
                    //        Id = c.u.Id,
                    //        EmployeeName = c.u.Name,
                    //        EmployeeCode = c.u.Code
                    //    }).Distinct().ToList();
                }

                userDto = userDto.OrderBy(s => s.EmployeeName).ToList();

                return _resultService.SuccessMessageWitObject(userDto, string.Empty);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        public ResultDto AddUserTarget(AddUserTargetDto addUserTargetDto)
        {
            _methodName = "AddUserTarget";
            var resultDto = new ResultDto();
            try
            {
                if (addUserTargetDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (addUserTargetDto.CreatedBy == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == addUserTargetDto.CreatedBy);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var userTargetContext = new UserSkuTarget
                {
                    AssignedFromId = addUserTargetDto.AssignedFromId,
                    AssignedToId = addUserTargetDto.AssignedToId,
                    FromDate = addUserTargetDto.FromDate,
                    ToDate = addUserTargetDto.ToDate,
                    IsActive = addUserTargetDto.IsActive,
                    OilTypeId = addUserTargetDto.OilTypeId,
                    SkuId = addUserTargetDto.SkuId,
                    TargetQuanity = addUserTargetDto.TargetQuanity,
                    //SchemeQuanity = addUserTargetDto.SchemeQuanity,
                    CreatedBy = addUserTargetDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };
                _emamiContext.UserTarget.Add(userTargetContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userTargetContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        public ResultDto UpdateUserTarget(UpdateUserTargetDto updateUserTargetDto)
        {
            _methodName = "UpdateUserTarget";
            var resultDto = new ResultDto();
            try
            {
                if (updateUserTargetDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (updateUserTargetDto.ModifiedBy == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == updateUserTargetDto.ModifiedBy);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var userTargetContext = _emamiContext.UserTarget.FirstOrDefault(_ => _.Id == updateUserTargetDto.Id);
                if (userTargetContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                userTargetContext.AssignedToId = updateUserTargetDto.AssignedToId;
                userTargetContext.FromDate = updateUserTargetDto.FromDate;
                userTargetContext.ToDate = updateUserTargetDto.ToDate;
                userTargetContext.IsActive = updateUserTargetDto.IsActive;
                userTargetContext.OilTypeId = updateUserTargetDto.OilTypeId;
                userTargetContext.SkuId = updateUserTargetDto.SkuId;
                userTargetContext.TargetQuanity = updateUserTargetDto.TargetQuanity;
                //userTargetContext.SchemeQuanity = updateUserTargetDto.SchemeQuanity;
                userTargetContext.ModifiedBy = updateUserTargetDto.ModifiedBy;
                userTargetContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userTargetContext.Id;
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

        public ResultDto GetUserTargetList(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserTargetList";
            var resultDto = new ResultDto();
            var userTargetDto = new List<UserTargetDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.LoginUserId <= 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            try
            {
                userTargetDto = _emamiContext.UserTarget.AsNoTracking().Where(_ => _.AssignedFromId == inputDto.LoginUserId && _.IsActive)
                    .Select(c => new UserTargetDto
                    {
                        Id = c.Id,
                        AssignedFromId = c.AssignedFromId,
                        AssignedFrom = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.AssignedFromId).Name,
                        AssignedToId = c.AssignedToId,
                        AssignedTo = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.AssignedToId).Name,
                        FromDate = c.FromDate,
                        ToDate = c.ToDate,
                        IsActive = c.IsActive,
                        OilTypeId = c.OilTypeId,
                        OilType = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == c.OilTypeId).Name,
                        SkuId = c.SkuId,
                        Sku = _emamiContext.Skus.FirstOrDefault(_ => _.Id == c.SkuId).SkuName,
                        TargetQuanity = c.TargetQuanity,
                        //SchemeQuanity = c.SchemeQuanity,
                    }).ToList();

                return _resultService.SuccessMessageWitObject(userTargetDto != null ? userTargetDto.OrderByDescending(_ => _.Id).ToList() : userTargetDto, string.Empty);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        public ResultDto GetUserTargetById(IdInputDto inputDto)
        {
            _methodName = "GetUserTargetById";
            var resultDto = new ResultDto();
            var userTargetDto = new UserTargetDto();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.LoginUserId <= 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            try
            {
                var userTargetContext = _emamiContext.UserTarget.FirstOrDefault(_ => _.Id == inputDto.Id);
                if (userTargetContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                userTargetDto.Id = userTargetContext.Id;
                userTargetDto.AssignedFromId = userTargetContext.AssignedFromId;
                userTargetDto.AssignedFrom = _emamiContext.Users.FirstOrDefault(_ => _.Id == userTargetContext.AssignedFromId).Name;
                userTargetDto.AssignedToId = userTargetContext.AssignedToId;
                userTargetDto.AssignedTo = _emamiContext.Users.FirstOrDefault(_ => _.Id == userTargetContext.AssignedToId).Name;
                userTargetDto.FromDate = userTargetContext.FromDate;
                userTargetDto.ToDate = userTargetContext.ToDate;
                userTargetDto.IsActive = userTargetContext.IsActive;
                userTargetDto.OilTypeId = userTargetContext.OilTypeId;
                userTargetDto.OilType = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == userTargetContext.OilTypeId).Name;
                userTargetDto.SkuId = userTargetContext.SkuId;
                userTargetDto.Sku = _emamiContext.Skus.FirstOrDefault(_ => _.Id == userTargetContext.SkuId).SkuName;
                userTargetDto.TargetQuanity = userTargetContext.TargetQuanity;
                //userTargetDto.SchemeQuanity = userTargetContext.SchemeQuanity;
                return _resultService.SuccessMessageWitObject(userTargetDto, string.Empty);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }
        #endregion      

        #region User & Sauda

        /// <summary>
        /// User Incoterms Mapping
        /// </summary>
        /// <param name="incoTermsIds"></param>
        /// <param name="userId"></param>
        /// <param name="createdBy"></param>
        public void AddUserIncoTerms(List<long> incoTermsIds, long userId, long createdBy)
        {
            try
            {
                if (incoTermsIds != null && incoTermsIds.Any())
                {
                    foreach (var incoTermsId in incoTermsIds)
                    {
                        _emamiContext.UserIncoTerms.Add(new UserIncoTerms() { UserId = userId, IncoTermsId = incoTermsId, CreatedBy = createdBy, CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow) });
                    }
                    _emamiContext.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Update Incoterms : Add and Remove
        /// </summary>
        /// <param name="incoTermsIds"></param>
        /// <param name="userId"></param>
        /// <param name="createdBy"></param>
        public void UpdateUserIncoTerms(List<long> incoTermsIds, long userId, long createdBy, string userEmail, string userName, long roleId)
        {
            try
            {
                if (incoTermsIds != null && incoTermsIds.Any())
                {
                    var userData = _emamiContext.UserIncoTerms.Where(w => w.UserId == userId).Select(s => s);
                    var newIncoTerms = incoTermsIds.Where(w => !userData.Any(a => a.IncoTermsId == w)).ToList();
                    var removeIncoTerms = userData.Where(w => !incoTermsIds.Any(a => a == w.IncoTermsId)).ToList();
                    newIncoTerms.ForEach(incoid => _emamiContext.UserIncoTerms.Add(new UserIncoTerms()
                    {
                        UserId = userId,
                        IncoTermsId = incoid,
                        CreatedBy = createdBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    }));
                    removeIncoTerms.ForEach(incoterms => _emamiContext.UserIncoTerms.Remove(incoterms));
                    _emamiContext.SaveChanges();

                    //#region Notification
                    //if (_resultService.IsEmail())
                    //{
                    //    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                    //    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.UserIncotermsEmail);  //Get email templte
                    //    var notificationEmail = _emamiContext.Configurations.FirstOrDefault(f => f.Key == Constants.NotificationEmail);        //Get notification emailids

                    //    if (notificationEmail != null)
                    //    {
                    //        var toUser = notificationEmail.Value.Split(',').ToList();       //string to list convert            

                    //        var emailSubject = Constants.SaudaApprovalSubject;
                    //        var fromEmail = Constants.FromEmail;
                    //        var plainText = string.Empty;

                    //        var userIncotermDetails = _emamiContext.IncoTerms.AsNoTracking().Join(_emamiContext.UserIncoTerms.AsNoTracking(), i => i.Id, ui => ui.IncoTermsId, (i, ui) => new { i, ui })
                    //            .Where(w => w.ui.UserId == userId).Select(s => s.i);        //Get all incoterms

                    //        if (userIncotermDetails != null && userIncotermDetails.Any())
                    //        {
                    //            var removedIncoTerm = removeIncoTerms.Select(s => s.IncoTermsId);  //Get removed incoterms name
                    //            var newIncoterms = userIncotermDetails.Where(w => newIncoTerms.Contains(w.Id)).Select(s => s.Name).ToList();        //Get newly added incoterms name
                    //            var removedIncoterms = _emamiContext.IncoTerms.Where(w => removedIncoTerm.Contains(w.Id)).Select(s => s.Name).ToList();
                    //            var roleNmae = Enum.GetName(typeof(Emami.Solution.DTO.Enums.Role), roleId);
                    //            //List string to string
                    //            var incoterms = string.Join(",", userIncotermDetails.Select(s => s.Name));
                    //            var newIncotermsName = (newIncoterms != null && newIncoterms.Any() ? string.Join(",", newIncoterms.Select(s => s)) : string.Empty);
                    //            var removedIncotermsName = (removedIncoterms != null && removedIncoterms.Any() ? string.Join(",", removedIncoterms.Select(s => s)) : string.Empty);

                    //            if (emailTemplate != null && (!string.IsNullOrEmpty(newIncotermsName) || !string.IsNullOrEmpty(removedIncotermsName)))
                    //            {
                    //                //Replace the values for plaintemplate
                    //                var htmlPlainTemplate = emailTemplate.PlainTemplate
                    //                    .Replace(Constants.Name, userName)
                    //                    .Replace(Constants.RoleName, roleNmae)
                    //                    .Replace(Constants.IncoTerms, incoterms)
                    //                    .Replace(Constants.NewIncoTerms, newIncotermsName)
                    //                    .Replace(Constants.RemovedIncoTerms, removedIncotermsName);
                    //                //Replace plaintemplate to maincontent
                    //                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, htmlPlainTemplate);
                    //                amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                    //            }
                    //        }
                    //    }
                    //}
                    //#endregion
                }
            }
            catch (Exception)
            {
            }
        }

        public ResultDto GetUsersByRole(IdInputDto inputDto)
        {
            _methodName = "GetUserAssignedTo";
            var resultDto = new ResultDto();
            var userDto = new List<UserMasterDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.LoginUserId <= 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            try
            {
                userDto = (from user in _emamiContext.Users.AsNoTracking()
                           join rolemap in _emamiContext.UserRoles.AsNoTracking() on user.Id equals rolemap.UserId
                           where rolemap.RoleId == inputDto.Id
                           select new UserMasterDto
                           {
                               Id = user.Id,
                               EmployeeName = user.Name,
                               EmployeeCode = user.Code
                           }).ToList();

                return _resultService.SuccessMessageWitObject(userDto, string.Empty);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        public ResultDto ChartSaudaAndSalesDetailsByOilTypes(ChartSaudaSalesByOilTypeInputDto inputDto)
        {
            _methodName = "GetUserAssignedTo";
            var resultDto = new ResultDto();
            var outputDto = new List<ChartSaudaSalesByOilTypeOutputDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }

            var OilTypeContext = (from saudaorder in _emamiContext.SaudaOrders
                                  join oiltype in _emamiContext.OilTypes on saudaorder.OilTypeId equals oiltype.Id
                                  where oiltype.DivisionId == inputDto.VerticalId
                                  && (DbFunctions.TruncateTime(saudaorder.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(saudaorder.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                  select new
                                  {
                                      OilTypeId = oiltype.Id,
                                      OilType = oiltype.Name
                                  }).Distinct().ToList();

            foreach (var item in OilTypeContext)
            {
                var saudaContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.OilTypeId == item.OilTypeId).ToList();
                var salesContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.OilTypeId == item.OilTypeId).ToList();

                var dto = new ChartSaudaSalesByOilTypeOutputDto
                {
                    OilTypeId = item.OilTypeId,
                    OilType = item.OilType,
                    SaudaCount = saudaContext != null ? saudaContext.Sum(_ => _.BidQuantity) : 0,
                    SalesCount = salesContext != null ? salesContext.Sum(_ => _resultService.ConvertCasetoMetricTon(_.ActualBilledQuantity, _.SkuId)) : 0
                };
                outputDto.Add(dto);
            }
            return _resultService.SuccessMessageWitObject(outputDto, string.Empty);
        }

        public ResultDto ChartSaudaApprovalDetailsByOilTypes(ChartSaudaSalesByOilTypeInputDto inputDto)
        {
            _methodName = "GetUserAssignedTo";
            var resultDto = new ResultDto();
            var outputDto = new List<ChartApprovalsByOilTypeOutputDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }

            var OilTypeContext = (from saudaorder in _emamiContext.SaudaOrders
                                  join oiltype in _emamiContext.OilTypes on saudaorder.OilTypeId equals oiltype.Id
                                  where oiltype.DivisionId == inputDto.VerticalId
                                  && (DbFunctions.TruncateTime(saudaorder.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(saudaorder.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                  select new
                                  {
                                      OilTypeId = oiltype.Id,
                                      OilType = oiltype.Name
                                  }).Distinct().ToList();

            foreach (var item in OilTypeContext)
            {
                var saudaPendingContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.OilTypeId == item.OilTypeId && _.StatusId == (int)DTO.Enums.Status.Pending && (DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))).ToList();
                var saudaApprovedContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.OilTypeId == item.OilTypeId && _.StatusId == (int)DTO.Enums.Status.Approved && (DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))).ToList();
                var saudaRejectedContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.OilTypeId == item.OilTypeId && _.StatusId == (int)DTO.Enums.Status.Rejected && (DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))).ToList();
                var saudaHoldContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.OilTypeId == item.OilTypeId && _.StatusId == (int)DTO.Enums.Status.Hold && (DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))).ToList();

                var dto = new ChartApprovalsByOilTypeOutputDto
                {
                    OilTypeId = item.OilTypeId,
                    OilType = item.OilType,
                    PendingCount = saudaPendingContext != null ? saudaPendingContext.Count() : 0,
                    ApprovedCount = saudaApprovedContext != null ? saudaApprovedContext.Count() : 0,
                    RejectedCount = saudaRejectedContext != null ? saudaRejectedContext.Count() : 0,
                    HoldCount = saudaHoldContext != null ? saudaHoldContext.Count() : 0
                };
                outputDto.Add(dto);
            }
            return _resultService.SuccessMessageWitObject(outputDto, string.Empty);
        }

        public ResultDto GetBDOListByZonalHead(IdInputDto inputDto)
        {
            _methodName = "GetBDOListByZonalHead";
            var resultDto = new ResultDto();
            var userDto = new List<UserMasterDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            if (inputDto.LoginUserId <= 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            try
            {
                userDto = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive).AsNoTracking().Select(c => new UserMasterDto
                {
                    Id = c.Id,
                    EmployeeName = c.Name,
                    EmployeeCode = c.Code,
                    BdoCount = _emamiContext.UserCustomerMapping.Where(_ => _.Id == c.Id).Count()
                }).ToList();

                return _resultService.SuccessMessageWitObject(userDto, string.Empty);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        public ResultDto GetUserExcelExportList(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserExcelExportList";
            var resultDto = new ResultDto();
            var userMasterDto = new List<UserMasterDto>();
            try
            {
                var userList =
                    _emamiContext.UserRoles.AsNoTracking().Where(_ =>
                      _.RoleId != (int)DTO.Enums.Role.Dealer && _.RoleId != (int)DTO.Enums.Role.Broker
                    )
                    .Select(_ => _.User);

                if (inputDto.VerticalId > 0)
                {
                    userMasterDto = (from user in _emamiContext.Users.AsNoTracking()
                                     join userRole in _emamiContext.UserRoles.AsNoTracking() on user.Id equals userRole.UserId
                                     join role in _emamiContext.Roles.AsNoTracking() on userRole.RoleId equals role.Id
                                     join state in _emamiContext.State.AsNoTracking() on user.StateId equals state.Id into stateTemp
                                     from state in stateTemp.DefaultIfEmpty()
                                     join district in _emamiContext.District.AsNoTracking() on user.DistrictId equals district.Id into districtTemp
                                     from district in districtTemp.DefaultIfEmpty()
                                     join territory in _emamiContext.Territory.AsNoTracking() on user.TerritoryId equals territory.Id into territoryTemp
                                     from territory in territoryTemp.DefaultIfEmpty()
                                     join zone in _emamiContext.Zones.AsNoTracking() on user.ZoneId equals zone.Id into zoneTemp
                                     from zone in zoneTemp.DefaultIfEmpty()
                                     join city in _emamiContext.City.AsNoTracking() on user.CityId equals city.Id into cityTemp
                                     from city in cityTemp.DefaultIfEmpty()
                                     join orgReport in _emamiContext.Users.AsNoTracking() on user.ReportingToId equals orgReport.Id into orgTemp
                                     from orgReport in orgTemp.DefaultIfEmpty()
                                     join salesReport in _emamiContext.Users.AsNoTracking() on user.ReportingToId equals salesReport.Id into salesTemp
                                     from salesReport in salesTemp.DefaultIfEmpty()
                                     where role.Id != (int)DTO.Enums.Role.Dealer && role.Id != (int)DTO.Enums.Role.Broker && role.Id != (int)DTO.Enums.Role.ShipToParty && role.Id != (int)DTO.Enums.Role.Admin //&& user.DivisionId == inputDto.VerticalId
                                     select new UserMasterDto()
                                     {
                                         //ReportingTo = c.ReportingTo,
                                         //SpecialityFatReportingToId = c.SpecialityFatReportingToId,
                                         Id = user.Id,
                                         EmployeeCode = user.Code,
                                         EmployeeName = user.Name,
                                         Branch = user.Branch,
                                         CompanyCode = user.CompanyCode,
                                         //VerticalId = user.DivisionId,
                                         //Vertical = user.Division != null ? user.Division.Name : string.Empty,
                                         //OrganizationReportingToId = user.ReportingToId,
                                         //OrganizationReportingToName = orgReport == null ? "" : orgReport.Code,
                                         //SalesReportingToId = user.ReportingToId,
                                         //SalesReportingToName = salesReport == null ? "" : salesReport.Code,
                                         Email = user.Email,
                                         MobileNumber = user.MobileNumber,
                                         SalesAccess = user.SalesAccess,
                                         Designation = user.Designation,
                                         HeadquartersId = user.HeadquartersId,
                                         //  Headquarters = user.Headquarters == null ? "" : user.Headquarters.Name,
                                         State = state == null ? "" : state.StateName,
                                         Territory = territory == null ? "" : territory.Name,
                                         Zone = zone == null ? "" : zone.Name,
                                         Acedns = user.Acedns,
                                         District = district == null ? "" : district.DistrictName,
                                         City = city == null ? "" : city.CityName,
                                         IsActive = user.IsActive,
                                         Pincode = user.Pincode,
                                         Address1 = user.Address1,
                                         Address2 = user.Address2,
                                         Password = user.Password,
                                         RoleName = role.Name,
                                         //   SaudaBookingType = user.SaudaBookingType == null ? "" : user.SaudaBookingType.Name,
                                         AdditionalMobileNumber = user.AdditionalMobileNumber
                                     }).ToList();

                }
                else
                {
                    userMasterDto = (from user in _emamiContext.Users.AsNoTracking()
                                     join userRole in _emamiContext.UserRoles.AsNoTracking() on user.Id equals userRole.UserId
                                     join role in _emamiContext.Roles.AsNoTracking() on userRole.RoleId equals role.Id
                                     join state in _emamiContext.State.AsNoTracking() on user.StateId equals state.Id into stateTemp
                                     from state in stateTemp.DefaultIfEmpty()
                                     join district in _emamiContext.District.AsNoTracking() on user.DistrictId equals district.Id into districtTemp
                                     from district in districtTemp.DefaultIfEmpty()
                                     join territory in _emamiContext.Territory.AsNoTracking() on user.TerritoryId equals territory.Id into territoryTemp
                                     from territory in territoryTemp.DefaultIfEmpty()
                                     join zone in _emamiContext.Zones.AsNoTracking() on user.ZoneId equals zone.Id into zoneTemp
                                     from zone in zoneTemp.DefaultIfEmpty()
                                     join city in _emamiContext.City.AsNoTracking() on user.CityId equals city.Id into cityTemp
                                     from city in cityTemp.DefaultIfEmpty()
                                     join orgReport in _emamiContext.Users.AsNoTracking() on user.ReportingToId equals orgReport.Id into orgTemp
                                     from orgReport in orgTemp.DefaultIfEmpty()
                                     join salesReport in _emamiContext.Users.AsNoTracking() on user.ReportingToId equals salesReport.Id into salesTemp
                                     from salesReport in salesTemp.DefaultIfEmpty()
                                     where role.Id != (int)DTO.Enums.Role.Dealer && role.Id != (int)DTO.Enums.Role.Broker && role.Id != (int)DTO.Enums.Role.ShipToParty && role.Id != (int)DTO.Enums.Role.Admin
                                     select new UserMasterDto()
                                     {
                                         //ReportingTo = c.ReportingTo,
                                         //SpecialityFatReportingToId = c.SpecialityFatReportingToId,
                                         Id = user.Id,
                                         EmployeeCode = user.Code,
                                         EmployeeName = user.Name,
                                         Branch = user.Branch,
                                         CompanyCode = user.CompanyCode,
                                         //VerticalId = user.DivisionId,
                                         //Vertical = user.Division != null ? user.Division.Name : string.Empty,
                                         OrganizationReportingToId = user.ReportingToId,
                                         OrganizationReportingToName = orgReport == null ? "" : orgReport.Code,
                                         SalesReportingToId = user.ReportingToId,
                                         SalesReportingToName = salesReport == null ? "" : salesReport.Code,
                                         Email = user.Email,
                                         MobileNumber = user.MobileNumber,
                                         SalesAccess = user.SalesAccess,
                                         Designation = user.Designation,
                                         HeadquartersId = user.HeadquartersId,
                                         //   Headquarters = user.Headquarters == null ? "" : user.Headquarters.Name,
                                         State = state == null ? "" : state.StateName,
                                         Territory = territory == null ? "" : territory.Name,
                                         Zone = zone == null ? "" : zone.Name,
                                         Acedns = user.Acedns,
                                         District = district == null ? "" : district.DistrictName,
                                         City = city == null ? "" : city.CityName,
                                         IsActive = user.IsActive,
                                         Pincode = user.Pincode,
                                         Address1 = user.Address1,
                                         Address2 = user.Address2,
                                         Password = user.Password,
                                         RoleName = role.Name,
                                         //  SaudaBookingType = user.SaudaBookingType == null ? "" : user.SaudaBookingType.Name,
                                         AdditionalMobileNumber = user.AdditionalMobileNumber
                                     }).ToList();

                }

                if (userMasterDto != null && userMasterDto.Any())
                {
                    var userdivisiondata = _emamiContext.UserDivisionMappings.ToList();
                    var userReportingTo = _emamiContext.UserReportingToMappings.AsQueryable()
                        .Join(_emamiContext.Users.AsNoTracking(),ur=>ur.ReportingToUserId,u=>u.Id,(ur,u)=> new { ur, u })
                        .Select(s => new { 
                            UserId=s.ur.UserId,
                            ReportingToId=s.ur.ReportingToUserId,
                            ReportingToCode=s.u.Code
                        }).ToList();
                    userMasterDto.ForEach(f =>
                    {
                        f.OrganizationReportingToName = String.Join(",", userReportingTo.Where(_ => _.UserId == f.Id).Select(s => s.ReportingToCode).ToList());
                        f.Vertical = String.Join(",", userdivisiondata.Where(_ => _.UserId == f.Id).Select(s => s.SalesOrganization.Code+"/"+s.DistributionChannel.Code+"/"+s.Division.Code).ToList());
                        f.CustomerCode = string.Join(",", GetUserCustomerMappingUserCode(f.Id));
                        f.Password = f.Password != null ? UtilityHelper.ConvertMd5ToString(f.Password, SecurityConstants.EncryptionKey) : string.Empty;
                    });
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userMasterDto != null ? userMasterDto.OrderByDescending(_ => _.Id).ToList() : userMasterDto;
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

        public List<string> GetUserCustomerMappingUserCode(long userId)
        {
            var result = new List<string>();
            result = _emamiContext.UserCustomerMapping.AsNoTracking().Join(_emamiContext.Users.AsNoTracking(), ucm => ucm.CustomerId, u => u.Id, (ucm, u) => new { UserCustomer = ucm, User = u })
                 .Where(w => w.UserCustomer.UserId == userId).Select(s => s.User.Code).Distinct().ToList();
            return result;
        }

        #endregion

        #region ShipToParty

        /// <summary>
        /// Method to Get ShipToParty List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetShipToPartyListExcelExport(LoginUserIdDto inputDto)
        {
            _methodName = "GetShipToPartyList";
            var resultDto = new ResultDto();
            var dealerDto = new List<ShipToPartyDto>();
            try
            {
                IQueryable<User> entity;
                if (inputDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty).Select(_ => _.User);
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0));
                }
                else
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty).Select(_ => _.User);
                    entity = entity.Where(_ => _.IsActive);
                }
                //var userDivisionMapping = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == userId).Select(_ => new DivisionDetailsDto
                //{
                //    DistributionChannel = _.DistributionChannel.Name,
                //    DistributionChannelId = _.DistributionChannelId,
                //    Division = _.Division.Name,
                //    DivisionId = _.DivisionId,
                //    SalesOrganization = _.SalesOrganization.Name,
                //    SalesOrganizationId = _.SalesOrganizationId,
                //});
                //employeeDto.DivisionList = userDivisionMapping.ToList();
                dealerDto = entity.ToList().OrderBy(_ => _.Name).Select(c => new ShipToPartyDto
                {

                    Id = c.Id,
                    Code = c.ShipToPartyCode,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    SaudaValidityPeriod = Convert.ToInt32(c.SaudaValidityPeriod),
                    //SaudaLimit = c.SaudaLimit,
                    SaudaBookingTypeId = c.SaudaBookingTypeId,
                    // SaudaBookingType = c.SaudaBookingType?.Name,
                    //FreightRouteName = c.FreightRoute?.Name,
                    //FreightZoneName = c.FreightZone?.Name,
                    City = c.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.CityId)?.CityName : string.Empty,
                    ZoneId = c.ZoneId,
                    //Zone = c.Zone?.Name,
                    District = c.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == c.DistrictId)?.DistrictName : string.Empty,
                    State = c.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == c.StateId)?.StateName : string.Empty,
                    Territory = c.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(_ => _.Id == c.TerritoryId)?.Name : string.Empty,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    GSTN = c.GSTN,
                    CustClass = c.CustClass,
                    VisitDay = c.VisitDay,
                    WeeklyClosingDay = c.WeeklyClosingDay,
                    MonthlyPotential = c.MonthlyPotential,
                    // PlantTruckCapacity = c.Loadability,
                    //DepotTruckCapacity = c.DepotLoadability,
                    //TransportMode = c.TransportMode?.Name,
                    // SaudaType = c.SaudaBookingType?.Name,
                    Pincode = c.Pincode,
                    //VerticalCode = c.Division?.Code,
                    //VerticalName = c.Division?.Name,
                    FSSAINumber = c.FSSAINumber,
                    Password = !string.IsNullOrEmpty(c.Password) ? UtilityHelper.ConvertMd5ToString(c.Password, SecurityConstants.EncryptionKey) : string.Empty,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    CustomerGroupFiveId = c.CustomerGroupFiveId,
                    CompanyCode = c.CompanyCode == null ? string.Empty : c.CompanyCode
                }).ToList();

                foreach (var item in dealerDto)
                {
                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    item.PlantTruckCapacities = string.Join(",", plantcapacities);
                    var divisions = _emamiContext.UserDivisionMappings.AsNoTracking().Where(w => w.UserId == item.Id).Select(x => x.Division).ToList();
                    if (divisions != null && divisions.Any())
                    {
                        item.VerticalName = string.Join(",", divisions.Select(_ => _.SalesOrganization.Code+"/"+_.DistributionChannel.Code+"/"+_.Code).Distinct().ToList());
                    }
                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    item.DepotTruckCapacities = string.Join(",", depotcapacities);

                    var userIncoTerms = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == item.Id);
                    var incoterms = userIncoTerms
                        .Join(_emamiContext.IncoTerms, uic => uic.IncoTermsId, ic => ic.Id, (UserIncoTerms, IncoTerms) => new { IncoTerms })
                        .Select(_ => _.IncoTerms).ToList();
                    if (incoterms != null && incoterms.Any())
                    {
                        item.Incoterms = string.Join(",", incoterms.Select(_ => _.Name.Trim()).Distinct().ToList());
                    }

                    var brokerContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                    .Select(_ => _.UserCustomerMapping.User).FirstOrDefault();
                    if (brokerContext != null)
                    {
                        item.BrokerCode = brokerContext.Code != null ? brokerContext.Code : string.Empty;
                    }

                    #region Check Is Empty & Need to Update

                    var incoTermsIds = userIncoTerms.Select(s => s.IncoTermsId).ToList();
                    if (incoTermsIds == null || !incoTermsIds.Any())
                    {
                        item.NewlyAdded = "Yes, incoTerms Missing";
                    }
                    if (item.SaudaValidityPeriod == 0)
                    {
                        item.NewlyAdded = "Yes, SaudaValidityPeriod Missing";
                    }
                    if (item.TransportModeId == 0)
                    {
                        item.NewlyAdded = "Yes, TransportModeId Missing";
                    }
                    if (item.SaudaBookingTypeId == 0)
                    {
                        item.NewlyAdded = "Yes, SaudaBookingTypeId Missing";
                    }
                    if (item.SaudaLimit == 0)
                    {
                        item.NewlyAdded = "Yes, SaudaLimit Missing";
                    }
                    if (string.IsNullOrEmpty(item.PlantName))
                    {
                        item.NewlyAdded = "Yes, PlantName Missing";
                    }
                    if (string.IsNullOrEmpty(item.DepotName))
                    {
                        item.NewlyAdded = "Yes, DepotName Missing";
                    }
                    //if (string.IsNullOrEmpty(item.FreightRouteName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightRouteName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.FreightZoneName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightZoneName Missing";
                    //}
                    if (string.IsNullOrEmpty(item.Zone))
                    {
                        item.NewlyAdded = "Yes, Zone Missing";
                    }
                    if (string.IsNullOrEmpty(item.State))
                    {
                        item.NewlyAdded = "Yes, State Missing";
                    }
                    if (string.IsNullOrEmpty(item.Territory))
                    {
                        item.NewlyAdded = "Yes, Territory Missing";
                    }
                    if (string.IsNullOrEmpty(item.District))
                    {
                        item.NewlyAdded = "Yes, District Missing";
                    }
                    if (string.IsNullOrEmpty(item.City))
                    {
                        item.NewlyAdded = "Yes, City Missing";
                    }

                    #endregion

                    var depotPlantListContext = _emamiContext.UserDepotMapping.AsNoTracking().Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.DepotId, d => d.Id, (x, d) => new { d.StorageTypeId, Depot = d.Name, DepotCode = d.Code });

                    var depotListContext = depotPlantListContext.Where(_ => (_.StorageTypeId == (int)DTO.Enums.StorageType.Depot || _.StorageTypeId == (int)DTO.Enums.StorageType.Rake)).Select(_ => _.DepotCode).ToList();
                    var plantListContext = depotPlantListContext.Where(_ => _.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(_ => _.DepotCode).ToList();
                    if (depotListContext != null && depotListContext.Any())
                    {
                        item.Depots = UtilityHelper.ConvertStringListToCommaSeparatedString(depotListContext);
                    }
                    if (plantListContext != null && plantListContext.Any())
                    {
                        item.Plants = UtilityHelper.ConvertStringListToCommaSeparatedString(plantListContext);
                    }

                    var bdoContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader)
                   .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                   .Select(_ => _.UserCustomerMapping.User).FirstOrDefault();
                    if (bdoContext != null)
                    {
                        item.StateTrader = bdoContext.Name;
                        item.BDOCode = bdoContext.Code;
                    }

                    var shipToPartyList = _emamiContext.CustomerShipToPartyMappings.AsNoTracking().Where(_ => _.CustomerId == item.Id).Select(_ => _.ShipToParty.Code).ToList();
                    if (shipToPartyList != null && shipToPartyList.Any())
                    {
                        item.ShipToParty = UtilityHelper.ConvertStringListToCommaSeparatedString(shipToPartyList);
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dealerDto != null ? dealerDto.OrderByDescending(_ => _.Id).ToList() : dealerDto;
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
        /// Method to Get ShipToParty List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetShipToPartyListWithPaging(KendoGridResult inputDto)
        {
            _methodName = "GetShipToPartyListWithPaging";
            var resultDto = new ResultDto();
            var outputDto = new List<ShipToPartyDto>();
            List<User> entity;
            DataSourceResult result = new DataSourceResult();

            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty).Select(_ => _.User).ToList();
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0));
                }
                else
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty).Select(_ => _.User).ToList();
                    //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0) && _.IsActive);
                    entity = entity.Where(_ => _.IsActive).ToList();
                }

                result = entity.AsEnumerable().Select(c => new ShipToPartyDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = c.Id,
                    Code = c.ShipToPartyCode,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    SaudaValidityPeriod = c.SaudaValidityPeriod,
                    //SaudaLimit = c.SaudaLimit,
                    SaudaBookingTypeId = c.SaudaBookingTypeId,
                    // SaudaBookingType = c.SaudaBookingType != null ? c.SaudaBookingType.Name : string.Empty,
                    //FreightRouteName = c.FreightRoute != null ? c.FreightRoute.Name : string.Empty,
                    //FreightZoneName = c.FreightZone != null ? c.FreightZone.Name : string.Empty,
                    ZoneId = c.ZoneId,
                    //  Zone = c.Zone != null ? c.Zone.Name : string.Empty,
                    CityId = c.CityId,
                    DistrictId = c.DistrictId,
                    StateId = c.StateId,
                    TerritoryId = c.TerritoryId,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    GSTN = c.GSTN,
                    CustClass = c.CustClass,
                    VisitDay = c.VisitDay,
                    WeeklyClosingDay = c.WeeklyClosingDay,
                    MonthlyPotential = c.MonthlyPotential,
                    //PlantTruckCapacity = c.Loadability,
                    //DepotTruckCapacity = c.DepotLoadability,
                    //TransportMode = c.TransportMode != null ? c.TransportMode.Name : string.Empty,
                    // SaudaType = c.SaudaBookingType != null ? c.SaudaBookingType.Name : string.Empty,
                    Pincode = c.Pincode,
                    //VerticalCode = c.Division != null ? c.Division.Code : string.Empty,
                    //VerticalName = c.Division != null ? c.Division.Name : string.Empty,
                    FSSAINumber = c.FSSAINumber,
                    Password = c.Password != null ? c.Password : string.Empty,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    CustomerGroupFiveId = c.CustomerGroupFiveId,
                    CompanyCode = c.CompanyCode == null ? string.Empty : c.CompanyCode

                }).ToDataSourceResult(inputDto.DataSourceRequest);

                outputDto = (result != null && result.Data != null) ? result.Data as List<ShipToPartyDto> : new List<ShipToPartyDto>();

                foreach (var item in outputDto)
                {
                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    item.PlantTruckCapacities = string.Join(",", plantcapacities);
                    var divisions = _emamiContext.UserDivisionMappings.AsNoTracking().Where(w => w.UserId == item.Id).Select(x => x.Division).ToList();
                    if (divisions != null && divisions.Any())
                    {
                        item.VerticalName = string.Join(",", divisions.Select(_ => _.SalesOrganization.Code+"/"+_.DistributionChannel.Code+"/"+_.Code).Distinct().ToList());
                    }
                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == item.Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    item.DepotTruckCapacities = string.Join(",", depotcapacities);
                    var userIncoTerms = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == item.Id);
                    var incoterms = userIncoTerms.Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.IncoTerms, uic => uic.IncoTermsId, ic => ic.Id, (UserIncoTerms, IncoTerms) => new { IncoTerms }).Select(_ => _.IncoTerms).ToList();
                    if (incoterms != null && incoterms.Any())
                    {
                        item.Incoterms = string.Join(",", incoterms.Select(_ => _.Name.Trim()).Distinct().ToList());
                    }

                    var brokerContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                    .Select(_ => _.UserCustomerMapping.User).FirstOrDefault();
                    if (brokerContext != null)
                    {
                        item.BrokerCode = brokerContext.Code != null ? brokerContext.Code : string.Empty;
                    }

                    var depotPlantListContext = _emamiContext.UserDepotMapping.AsNoTracking().Where(_ => _.UserId == item.Id)
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.DepotId, d => d.Id, (x, d) => new { StorageTypeId = d.StorageTypeId, Depot = d.Name, DepotCode = d.Code });
                    var depotListContext = depotPlantListContext.Where(_ => (_.StorageTypeId == (int)DTO.Enums.StorageType.Depot || _.StorageTypeId == (int)DTO.Enums.StorageType.Rake)).Select(_ => _.DepotCode).ToList();
                    var plantListContext = depotPlantListContext.Where(_ => _.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(_ => _.DepotCode).ToList();
                    if (depotListContext != null && depotListContext.Any())
                    {
                        item.Depots = UtilityHelper.ConvertStringListToCommaSeparatedString(depotListContext);
                    }
                    if (plantListContext != null && plantListContext.Any())
                    {
                        item.Plants = UtilityHelper.ConvertStringListToCommaSeparatedString(plantListContext);
                    }

                    var bdoContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader)
                   .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == item.Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                   .Select(_ => _.UserCustomerMapping.User).FirstOrDefault();
                    if (bdoContext != null)
                    {
                        item.StateTrader = bdoContext.Name;
                        item.BDOCode = bdoContext.Code;
                    }

                    var name = item.CityId > 0 ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == item.CityId)?.CityName : string.Empty;
                    item.City = Utility.TrimAndReduce(name);

                    name = item.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == item.DistrictId)?.DistrictName : string.Empty;
                    item.District = Utility.TrimAndReduce(name);

                    name = item.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == item.StateId)?.StateName : string.Empty;
                    item.State = Utility.TrimAndReduce(name);

                    name = item.TerritoryId > 0 ? _emamiContext.Territory.AsNoTracking().FirstOrDefault(_ => _.Id == item.TerritoryId)?.Name : string.Empty;
                    item.Territory = Utility.TrimAndReduce(name);

                    #region Check Is Empty & Need to Update

                    var incoTermsIds = userIncoTerms.Where(w => w.UserId == item.Id).Select(s => s.IncoTermsId).ToList();
                    if (incoTermsIds == null || !incoTermsIds.Any())
                    {
                        item.NewlyAdded = "Yes, incoTerms Missing";
                    }

                    //if (item.SaudaValidityPeriod == 0)
                    //{
                    //    item.NewlyAdded = "Yes, SaudaValidityPeriod Missing";
                    //}



                    //if (string.IsNullOrEmpty(item.Depots))
                    //{
                    //    item.NewlyAdded = "Yes, DepotName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.FreightRouteName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightRouteName Missing";
                    //}
                    //if (string.IsNullOrEmpty(item.FreightZoneName))
                    //{
                    //    item.NewlyAdded = "Yes, FreightZoneName Missing";
                    //}
                    if (string.IsNullOrEmpty(item.Zone))
                    {
                        item.NewlyAdded = "Yes, Zone Missing";
                    }
                    if (string.IsNullOrEmpty(item.State))
                    {
                        item.NewlyAdded = "Yes, State Missing";
                    }

                    if (string.IsNullOrEmpty(item.District))
                    {
                        item.NewlyAdded = "Yes, District Missing";
                    }
                    if (string.IsNullOrEmpty(item.City))
                    {
                        item.NewlyAdded = "Yes, City Missing";
                    }

                    #endregion

                    item.Password = item.Password != null ? UtilityHelper.ConvertMd5ToString(item.Password, SecurityConstants.EncryptionKey) : string.Empty;
                }
                result.Data = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToList() : outputDto;
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
        /// Method to get Get ShipToParty Details By Id
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public ResultDto GetShipToPartyDetailsById(string shiptoparty)
        {
            _methodName = "GetShipToPartyDetailsById";
            var resultDto = new ResultDto();
            var employeeDto = new EmployeeDto();
            try
            {
                shiptoparty = shiptoparty.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(shiptoparty, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);

                var resultContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    var userDivisionMapping = _emamiContext.UserDivisionMappings.Where(_ => _.UserId == Id).Select(_ => new DivisionDetailsDto
                    {
                        DistributionChannel = _.DistributionChannel.Name,
                        DistributionChannelId = _.DistributionChannelId,
                        Division = _.Division.Name,
                        DivisionId = _.DivisionId,
                        SalesOrganization = _.SalesOrganization.Name,
                        SalesOrganizationId = _.SalesOrganizationId,
                    });
                    employeeDto.EncryptedId = shiptoparty;
                    employeeDto.DivisionList = userDivisionMapping.ToList();
                    employeeDto.Id = resultContext.Id;
                    employeeDto.Code = resultContext.ShipToPartyCode;
                    employeeDto.Name = resultContext.Name;
                    employeeDto.MobileNumber = resultContext.MobileNumber;
                    employeeDto.Email = resultContext.Email;
                    employeeDto.IsActive = resultContext.IsActive;
                    employeeDto.SaudaValidityPeriod = Convert.ToInt32(resultContext.SaudaValidityPeriod);
                    //employeeDto.SaudaLimit = resultContext.SaudaLimit;
                    employeeDto.DistrictId = resultContext.DistrictId;
                    employeeDto.District = resultContext.District;
                    employeeDto.ZoneId = resultContext.ZoneId;
                    // employeeDto.Zone = resultContext.Zone?.Name;
                    employeeDto.StateId = resultContext.StateId;
                    employeeDto.State = resultContext.State;
                    employeeDto.City = resultContext.City;
                    employeeDto.CityId = resultContext.CityId;
                    employeeDto.Territory = resultContext.Territory;
                    employeeDto.TerritoryId = resultContext.TerritoryId;
                    employeeDto.Address1 = resultContext.Address1;
                    employeeDto.Address2 = resultContext.Address2;
                    employeeDto.GSTN = resultContext.GSTN;
                    employeeDto.CustClass = resultContext.CustClass;
                    employeeDto.VisitDay = resultContext.VisitDay;
                    employeeDto.WeeklyClosingDay = resultContext.WeeklyClosingDay;
                    employeeDto.MonthlyPotential = resultContext.MonthlyPotential;
                    employeeDto.TransportModeId = resultContext.TransportModeId;
                    employeeDto.SaudaBookingTypeId = resultContext.SaudaBookingTypeId;
                    employeeDto.Pincode = resultContext.Pincode;
                    //employeeDto.FreightRouteId = resultContext.FreightRouteId;
                    //employeeDto.FreightZoneId = resultContext.FreightZoneId;
                    employeeDto.IsSelf = resultContext.IsSelf;
                    employeeDto.IsBroker = resultContext.IsBroker;
                    //employeeDto.VerticalId = resultContext.DivisionId;
                    employeeDto.FSSAINumber = resultContext.FSSAINumber;
                    //employeeDto.PlantTruckCapacity = resultContext.Loadability;
                    //employeeDto.DepotTruckCapacity = resultContext.DepotLoadability;
                    //employeeDto.CustomerGroupOneId = resultContext.CustomerGroupOneId;
                    //employeeDto.CustomerGroupTwoId = resultContext.CustomerGroupTwoId;
                    employeeDto.Latitude = resultContext.Latitude;
                    employeeDto.Longitude = resultContext.Longitude;
                    employeeDto.InActiveRemarkId = resultContext.InActiveRemarkId;
                    employeeDto.CustomerGroupFiveId = resultContext.CustomerGroupFiveId;
                    employeeDto.CompanyCode = resultContext.CompanyCode == null ? string.Empty : resultContext.CompanyCode;
                    if (!string.IsNullOrEmpty(resultContext.Password))
                    {
                        employeeDto.Password = UtilityHelper.ConvertMd5ToString(resultContext.Password, SecurityConstants.EncryptionKey);
                    }

                    //employeeDto.SelectedDepotIds = GetUserDepotIds(dealerId);
                    employeeDto.SelectedBrokerIds = GetUserCustomerIds(Id);
                    employeeDto.SelectedDealerIds = GetCustomerShipToParyIds(Id);
                    employeeDto.SelectedDealerIdsCount = (employeeDto.SelectedDealerIds != null && employeeDto.SelectedDealerIds.Any()) ? employeeDto.SelectedDealerIds.Count : 0;
                    employeeDto.IncoTermsId = _emamiContext.UserIncoTerms.AsNoTracking().Where(w => w.UserId == resultContext.Id).Select(s => s.IncoTermsId).ToList();

                    var plantcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
                    employeeDto.PlantTruckCapacities = string.Join(",", plantcapacities);

                    var depotcapacities = _emamiContext.CustomerTruckCapacityMapping.AsNoTracking().Where(w => w.UserId == Id && w.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
                    employeeDto.DepotTruckCapacities = string.Join(",", depotcapacities);

                    var plantIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => w.UserId == Id)
                        .Join(_emamiContext.Depots.Where(_ => _.IsPlant), a => a.DepotId, d => d.Id, (a, d) => new { userdepot = a, depot = d });
                    if (plantIds != null && plantIds.Any())
                    {
                        //employeeDto.PlantId = plantIds.FirstOrDefault().userdepot.DepotId;
                        employeeDto.SelectedPlantIds = plantIds.Select(s => s.userdepot.DepotId).ToList();
                    }

                    //var depotIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => w.UserId == dealerId)
                    //    .Join(_emamiContext.Depots.Where(_ => !_.IsPlant), a => a.DepotId, d => d.Id, (a, d) => new { userdepot = a, depot = d });
                    //if (depotIds != null && depotIds.Any())
                    //{
                    //    //employeeDto.DepotId = depotIds.FirstOrDefault().userdepot.DepotId;
                    //    employeeDto.SelectedDepotIds = depotIds.Select(s => s.userdepot.DepotId).ToList();
                    //}

                    //if (resultContext.IsBroker)
                    // {

                    var dealerExistList = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                      .Join(_emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == Id), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                      .Select(_ => _.UserCustomerMapping).ToList();


                    //var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                    //    .Where(w => w.CustomerId == dealerId);
                    if (dealerExistList != null && dealerExistList.Any())
                    {
                        employeeDto.BrokerId = dealerExistList.FirstOrDefault().UserId;
                    }
                    //}
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = employeeDto;
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
        /// Method to Get ShipToParty Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetShipToPartyBrokerList(LoginUserIdDto inputDto)
        {
            _methodName = "GetShipToPartyBrokerList";
            var resultDto = new ResultDto();
            var dealerDto = new List<ShipToPartyDto>();
            try
            {
                IQueryable<User> entity;
                if (inputDto.IsToReturnInactiveData)
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty && _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Select(_ => _.User);
                }
                else
                {
                    entity = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty && _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Select(_ => _.User);
                    entity = entity.Where(_ => _.IsActive);
                }
                dealerDto = entity.ToList().OrderBy(_ => _.Name).Select(c => new ShipToPartyDto
                {
                    Id = c.Id,
                    Code = c.ShipToPartyCode,
                    Name = c.Name,
                    IsActive = c.IsActive,
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dealerDto;
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

        #region Get Dealer List With Pagination

        public ResultDto GetDealerListWithPagination(DealerListInputDto inputDto)
        {
            _methodName = "GetDealerListWithPagination";
            var resultDto = new ResultDto();
            var outputDto = new DealerListOutputDto();
            outputDto.DealerList = new List<DealerListDto>();
            try
            {
                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                List<User> userContext = _emamiContext.Users.ToList();

                if (inputDto.DealerId > 0)
                {
                    var dealerIds = new List<long> { inputDto.DealerId };
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }
                else if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && inputDto.ZonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }
                else if (inputDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = userContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }
                else
                {
                    var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
                    var zonalHeadIds = userContext.Where(_ => _.ReportingToId != null && nationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }

                if (userContext != null && userContext.Any())
                {
                    outputDto.ListCount = userContext.Count();
                    outputDto.DealerList = userContext.Select(c => new DealerListDto
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Name = c.Name,
                        MobileNumber = c.MobileNumber,
                        //SaudaLimit = c.SaudaLimit,
                    }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).OrderBy(_ => _.Name).ToList();
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

        #endregion

        #region Dealer, Broker, ShipToParty - Admin App

        public ResultDto GetDealerListWithPaginationAdminApp(DealerListInputDto inputDto)
        {
            _methodName = "GetDealerListWithPaginationAdminApp";
            var resultDto = new ResultDto();
            var outputDto = new DealerListOutputDto();
            outputDto.DealerList = new List<DealerListDto>();
            IQueryable<User> userContext;

            try
            {
                var usersContext = _emamiContext.Users;
                userContext = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).Select(_ => _.User);
                //userContext = userContext.Where(_ => (inputDto.VerticalId > 0 ? _.VerticalId == inputDto.VerticalId : _.VerticalId > 0));

                if (inputDto.DistrictId > 0)
                {
                    userContext = userContext.Where(_ => _.DistrictId == inputDto.DistrictId);
                }
                if (!string.IsNullOrEmpty(inputDto.Name))
                {
                    userContext = userContext.Where(_ => _.Name.ToLower().Contains(inputDto.Name.ToLower()));
                }
                if (inputDto.StateId > 0)
                {
                    userContext = userContext.Where(_ => _.StateId == inputDto.StateId);
                }
                if (inputDto.CityId > 0)
                {
                    userContext = userContext.Where(_ => _.CityId == inputDto.CityId);
                }
                if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id));
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && inputDto.ZonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id));
                }
                else if (inputDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = usersContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id));
                }
                else
                {
                    var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
                    var zonalHeadIds = usersContext.Where(_ => _.ReportingToId != null && nationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id));
                }

                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                var deleteListContext = _emamiContext.DeleteListCreations.AsNoTracking();
                outputDto.ListCount = userContext.Count();
                outputDto.DealerList = userContext.Select(c => new DealerListDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    //SaudaLimit = c.SaudaLimit,
                }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).ToList();

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

        public ResultDto GetBrokerListWithPaginationAdminApp(DealerListInputDto inputDto)
        {
            _methodName = "GetBrokerList";
            var resultDto = new ResultDto();
            var outputDto = new DealerListOutputDto();
            outputDto.DealerList = new List<DealerListDto>();
            try
            {
                var usersContext = _emamiContext.Users;
                IQueryable<User> userContext = _emamiContext.UserRoles
                    .Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker)
                    .Select(_ => _.User);
                //userContext = userContext.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0));

                if (inputDto.DistrictId > 0)
                {
                    userContext = userContext.Where(_ => _.DistrictId == inputDto.DistrictId);
                }
                if (!string.IsNullOrEmpty(inputDto.Name))
                {
                    userContext = userContext.Where(_ => _.Name.ToLower().Contains(inputDto.Name.ToLower()));
                }
                if (inputDto.StateId > 0)
                {
                    userContext = userContext.Where(_ => _.StateId == inputDto.StateId);
                }
                if (inputDto.CityId > 0)
                {
                    userContext = userContext.Where(_ => _.CityId == inputDto.CityId);
                }
                if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.UserId).ToList();
                    userContext = userContext.Where(_ => brokerIds.Contains(_.Id));
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && inputDto.ZonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.UserId).ToList();
                    userContext = userContext.Where(_ => brokerIds.Contains(_.Id));
                }
                else if (inputDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = usersContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.UserId).ToList();
                    userContext = userContext.Where(_ => brokerIds.Contains(_.Id));
                }
                else
                {
                    var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
                    var zonalHeadIds = usersContext.Where(_ => _.ReportingToId != null && nationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.UserId).ToList();
                    userContext = userContext.Where(_ => brokerIds.Contains(_.Id));
                }

                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                outputDto.ListCount = userContext.Count();
                outputDto.DealerList = userContext.Select(c => new DealerListDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    MobileNumber2 = c.MobileNumber2,
                    Email = c.Email,
                    //SaudaLimit = c.SaudaLimit,
                }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).ToList();


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

        public ResultDto GetShipToPartyListWithPaginationAdminApp(DealerListInputDto inputDto)
        {
            _methodName = "GetShipToPartyListWithPaginationAdminApp";
            var resultDto = new ResultDto();
            var outputDto = new DealerListOutputDto();
            outputDto.DealerList = new List<DealerListDto>();
            IQueryable<User> userContext;

            try
            {
                userContext = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty).Select(_ => _.User);
                var usersContext = _emamiContext.Users;
                //userContext = userContext.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0));

                if (inputDto.DistrictId > 0)
                {
                    userContext = userContext.Where(_ => _.DistrictId == inputDto.DistrictId);
                }
                if (!string.IsNullOrEmpty(inputDto.Name))
                {
                    userContext = userContext.Where(_ => _.Name.ToLower().Contains(inputDto.Name.ToLower()));
                }

                if (inputDto.StateId > 0)
                {
                    userContext = userContext.Where(_ => _.StateId == inputDto.StateId);
                }
                if (inputDto.CityId > 0)
                {
                    userContext = userContext.Where(_ => _.CityId == inputDto.CityId);
                }
                if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    //var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var shipToPartyIds = _emamiContext.CustomerShipToPartyMappings.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.ShipToPartyId).ToList();
                    userContext = userContext.Where(_ => shipToPartyIds.Contains(_.Id));
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && inputDto.ZonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    //var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var shipToPartyIds = _emamiContext.CustomerShipToPartyMappings.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.ShipToPartyId).ToList();
                    userContext = userContext.Where(_ => shipToPartyIds.Contains(_.Id));
                }
                else if (inputDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = usersContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    // var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var shipToPartyIds = _emamiContext.CustomerShipToPartyMappings.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.ShipToPartyId).ToList();
                    userContext = userContext.Where(_ => shipToPartyIds.Contains(_.Id));
                }
                else
                {
                    var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
                    var zonalHeadIds = usersContext.Where(_ => _.ReportingToId != null && nationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = usersContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    //var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var shipToPartyIds = _emamiContext.CustomerShipToPartyMappings.AsNoTracking().Where(_ => dealerIds.Contains(_.CustomerId)).Select(_ => _.ShipToPartyId).ToList();
                    userContext = userContext.Where(_ => shipToPartyIds.Contains(_.Id));
                }

                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                outputDto.ListCount = userContext.Count();
                outputDto.DealerList = userContext.Select(c => new DealerListDto
                {
                    Id = c.Id,
                    Code = c.ShipToPartyCode,
                    Name = c.Name,
                    MobileNumber = c.MobileNumber,
                    Email = c.Email,
                    //SaudaLimit = c.SaudaLimit,
                }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).ToList();

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

        public ResultDto GetUserListWithPaginationAdminApp(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserListWithPaginationAdminApp";
            var resultDto = new ResultDto();
            var outputDto = new DealerListOutputDto();
            outputDto.DealerList = new List<DealerListDto>();
            try
            {
                IQueryable<User> entity = _emamiContext.UserRoles.Where(_ => _.RoleId != (int)DTO.Enums.Role.Dealer
                && _.RoleId != (int)DTO.Enums.Role.Broker && _.RoleId != (int)DTO.Enums.Role.ShipToParty && _.RoleId != (int)DTO.Enums.Role_CMS.Demonstrator)
                .Select(_ => _.User);

                //entity = entity.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0));

                if (!string.IsNullOrEmpty(inputDto.Name))
                {
                    entity = entity.Where(_ => _.Name.Contains(inputDto.Name));
                }

                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                outputDto.ListCount = entity.Count();
                outputDto.DealerList = entity.Select(c => new DealerListDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Email = c.Email,
                    MobileNumber = c.MobileNumber,
                }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).ToList();

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

        #region Dealer, Broker, ShipToParty - Mapping List

        public ResultDto GetDealerAndBrokerList(DealerBrokerParamDto inputDto)
        {
            _methodName = "GetDealerAndBrokerList";
            var resultDto = new ResultDto();
            try
            {
                var userList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                    .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer || w.UserRoles.RoleId == (int)DTO.Enums.Role.Broker)
                    && w.Users.SaudaBookingTypeId == inputDto.SaudaBookingTypeId
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
                        Address1 = s.Users.Address1,
                        Address2 = s.Users.Address2,
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

        public ResultDto GetDealerListByVertical(DealerBrokerParamDto inputDto)
        {
            _methodName = "GetDealerListByVertical";
            var resultDto = new ResultDto();
            try
            {
                var userList = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                    .Where(w => w.ur.RoleId == (int)DTO.Enums.Role.Dealer)
                    //&& w.u.DivisionId == inputDto.VerticalId
                    //&& w.u.SaudaBookingTypeId == inputDto.SaudaBookingTypeId)
                    .Select(s => new DealerBrokerDto()
                    {
                        Id = s.u.Id,
                        Code = s.u.Code,
                        Name = s.u.Name,
                        MobileNumber = s.u.MobileNumber,
                        Email = s.u.Email,
                        //State = s.u.State,
                        Address1 = s.u.Address1,
                        Address2 = s.u.Address2,
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

        public ResultDto GetShipToPartyListBasedOnVertical(DealerBrokerParamDto inputDto)
        {
            _methodName = "GetShipToPartyListBasedOnVertical";
            var resultDto = new ResultDto();
            try
            {
                var userContext = _emamiContext.Users.AsNoTracking()//.Where(_ => _.DivisionId == inputDto.VerticalId)
                    .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.ShipToParty), u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur });

                var userList = userContext.ToList().Select(s => new DealerBrokerDto()
                {
                    Id = s.Users.Id,
                    Code = s.Users.Code,
                    Name = s.Users.Name,
                    MobileNumber = s.Users.MobileNumber,
                    Email = s.Users.Email,
                    State = s.Users.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == s.Users.StateId).StateName : string.Empty,
                    District = s.Users.DistrictId > 0 ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == s.Users.DistrictId).DistrictName : string.Empty,
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

        #region Dashboard Portal

        public ResultDto GetDashboardDetails(LoginUserIdDto inputDto)
        {
            _methodName = "GetDashboardDetails";
            var resultDto = new ResultDto();
            var data = new DashboardDetailsDto();
            if (inputDto.LoginUserId == 0)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }

            try
            {
                _logger.Info("Dashboard Cards menthod started");
                var roleId = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur }).FirstOrDefault(_ => _.u.Id == inputDto.LoginUserId).ur.RoleId;
                var dealerList = new List<long>();
                if (roleId == (int)DTO.Enums.Role.NationalTrader)
                {

                    dealerList = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Where(_ => _.ReportingToUserId == inputDto.LoginUserId)
                        .Join(_emamiContext.UserReportingToMappings.AsNoTracking(), zh => zh.UserId, bdo => bdo.ReportingToUserId, (zh, bdo) => new { zh, bdo })
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.bdo.UserId, uc => uc.UserId, (x, uc) => new { x, uc })
                        .Select(s => s.uc.CustomerId).ToList();

                    //dealerList = _emamiContext.Users.AsNoTracking()
                    //    .Join(_emamiContext.Users.AsNoTracking(), zh => zh.Id, bdo => bdo.ReportingToId, (zh, bdo) => new { zh, bdo }).Where(_ => _.zh.ReportingToId == inputDto.LoginUserId)
                    //    .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.bdo.Id, ucm => ucm.UserId, (u, ucm) => new { u, ucm })
                    //    .Select(a => a.ucm.CustomerId).ToList();
                }
                else if (roleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    dealerList = _emamiContext.UserReportingToMappings.AsNoTracking()
                            .Where(_ => _.ReportingToUserId == inputDto.LoginUserId)
                            .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), ur => ur.UserId, uc => uc.UserId, (ur, uc) => new { ur, uc })
                            .Select(_ => _.uc.CustomerId).ToList();

                    //dealerList =_emamiContext.Users.AsNoTracking()
                    //   .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.Id, ucm => ucm.UserId, (u, ucm) => new { u, ucm })
                    //   .Where(_ => _.u.ReportingToId == inputDto.LoginUserId).Select(a => a.ucm.CustomerId).ToList();
                }
                else if (roleId == (int)DTO.Enums.Role.StateTrader)
                {
                    dealerList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(ucm => ucm.UserId == inputDto.LoginUserId)
                       .Select(a => a.CustomerId).ToList();
                }
                else
                {
                    dealerList = _emamiContext.Users.AsNoTracking()
                       .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                       .Where(_ => _.ur.RoleId == (int)DTO.Enums.Role.Dealer).Select(a => a.u.Id).ToList();
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (roleId == (int)DTO.Enums.Role.Admin)
                {
                    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.Id
                    });
                }
                else
                {
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }
                _logger.Info("Dashboard Cards menthod master data taken completed");
                if (inputDto.IntercomId == (int)DTO.Enums.DashboardOption.TodayContract)
                {
                    data.TodayContract = (from s in _emamiContext.Sauda.AsNoTracking()
                                          join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                          join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                          where DbFunctions.TruncateTime(s.BiddingDate) == DbFunctions.TruncateTime(currentDate)
                      && (so.StatusId != (int)DTO.Enums.Status.Rejected
                      && s.StatusId != (int)DTO.Enums.Status.Rejected)
                      && dealerList.Contains(s.UserId)
                                          select so.BidQuantity
                                        ).DefaultIfEmpty().Sum();
                    _logger.Info("Dashboard Cards menthod master Contract");
                    //data.TodayContract = _emamiContext.Sauda.AsNoTracking()
                    //.Join(_emamiContext.SaudaOrders.AsNoTracking(), sauda => sauda.Id, saudaOrder => saudaOrder.SaudaId,
                    //(sauda, saudaOrder) => new { sauda, saudaOrder })
                    //.Where(_ => DbFunctions.TruncateTime(_.sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate)
                    //&& (_.saudaOrder.StatusId != (int)DTO.Enums.Status.Rejected
                    //&& _.sauda.StatusId != (int)DTO.Enums.Status.Rejected)
                    //&& dealerList.Contains(_.sauda.UserId))
                    //.DefaultIfEmpty().Sum(s => s.saudaOrder != null ? s.saudaOrder.BidQuantity : 0);
                }
                else if (inputDto.IntercomId == (int)DTO.Enums.DashboardOption.TodaySalesOrder)
                {
                    data.TodaySalesOrder = 
                    (from l in _emamiContext.LiftingRequest.AsNoTracking()
                     join lr in _emamiContext.LiftingRequestDetails.AsNoTracking() on l.Id equals lr.LiftingRequestId
                     join s in _emamiContext.Sauda.AsNoTracking() on lr.SaudaNumber equals s.SaudaNumber
                     join ud in divisionslogieduser on new { SalesOrganizationId = lr.SalesOrganizationId, DistributionChannelId = lr.DistributionhannelId, DivisionId = lr.DivisionId }
                       equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                     where DbFunctions.TruncateTime(l.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                     && l.StatusId != (int)DTO.Enums.Status.Rejected
                     && dealerList.Contains(l.UserId)
                     select lr.LiftingQuantity
                                      ).DefaultIfEmpty().Sum();

                    _logger.Info("Dashboard Cards menthod master Sales order");
                    //  data.TodaySalesOrder = _emamiContext.LiftingRequest.AsNoTracking()
                    //.Join(_emamiContext.LiftingRequestDetails.AsNoTracking(), lift => lift.Id, liftdetails => liftdetails.LiftingRequestId,
                    //(lift, liftdetails) => new { lift, liftdetails })
                    //.Where(_ => DbFunctions.TruncateTime(_.lift.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                    //&& _.lift.StatusId != (int)DTO.Enums.Status.Rejected
                    //&& dealerList.Contains(_.lift.UserId))
                    //.DefaultIfEmpty().Sum(s => s.liftdetails != null ? s.liftdetails.LiftingQuantity : 0);
                }
                else if (inputDto.IntercomId == (int)DTO.Enums.DashboardOption.TodayInvoice)
                {
                    data.TodayInvoice=(from s in _emamiContext.SalesRegister.AsNoTracking()
                                       join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                       where DbFunctions.TruncateTime(s.InvoiceDate) == DbFunctions.TruncateTime(currentDate)
                                                && dealerList.Contains(s.UserId)
                                                select s.QuantityCase
                                              ).DefaultIfEmpty().Sum();
                    _logger.Info("Dashboard Cards menthod master Invoice");
                    //data.TodayInvoice = _emamiContext.SalesRegister.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.InvoiceDate) == DbFunctions.TruncateTime(currentDate)
                    //&& dealerList.Contains(_.UserId))
                    //.DefaultIfEmpty().Sum(_ => _.QuantityCase > 0 ? _.QuantityCase : 0);
                }
                else if (inputDto.IntercomId == (int)DTO.Enums.DashboardOption.Due)
                {
                    var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ => dealerList.Contains(_.UserId));
                    if (overduePaymentContext != null && overduePaymentContext.Any())
                    {
                        var tomDate = currentDate.AddDays(1);
                        decimal TotalDueForTomorrow = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        decimal TotalOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        data.TomorrowDue = TotalDueForTomorrow != 0 ? Math.Round((TotalDueForTomorrow / 100000), 2) : 0;
                        data.OverDue = TotalOverDue != 0 ? Math.Round((TotalOverDue / 100000), 2) : 0;
                    }

                    _logger.Info("Dashboard Cards menthod master Due");
                }
                _logger.Info("Dashboard Cards menthod complted");
                return _resultService.SuccessMessageWitObject(data, string.Empty);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        #endregion

        //#region GetGoogle Analytics

        public ResultDto GetDashboardUserInfo(LoginUserIdDto inputKey)
        {
            _methodName = "GetDashboardUserInfo";
            var resultDto = new ResultDto();
            GoogleAnalyticsDataDto result = new GoogleAnalyticsDataDto();
            if (inputKey.LoginUserId == 0)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            try
            {
                //var specificDate = new DateTime(2024, 5, 15);
                var loginDistributorCount = (from ul in _emamiContext.UserLoginHistory
                                             join ur in _emamiContext.UserRoles on ul.LoginUserId equals ur.UserId
                                             where ur.RoleId ==(int)DTO.Enums.RoleType.Dealer && DbFunctions.TruncateTime(ul.LoginDate) == DbFunctions.TruncateTime(DateTime.Now)
                                             //where ur.RoleId ==(int)DTO.Enums.RoleType.Dealer && DbFunctions.TruncateTime(ul.LoginDate) == specificDate
                                             select ul.LoginUserId).Distinct().Count();
                result.TotalLoginsByDistributor = loginDistributorCount;
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
            return resultDto;

        }

        public ResultDto GetDashboardSalesUserInfo(LoginUserIdDto inputKey)
        {
            _methodName = "GetDashboardSalesUserInfo";
            var resultDto = new ResultDto();
            GoogleAnalyticsDataDto result = new GoogleAnalyticsDataDto();
            if (inputKey.LoginUserId == 0)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            try
            {
                //var targetDate = new DateTime(2024, 5, 15);
                var loginSalesCount = (from ul in _emamiContext.UserLoginHistory
                                       join ur in _emamiContext.UserRoles on ul.LoginUserId equals ur.UserId
                                       where ur.RoleId == (int)DTO.Enums.RoleType.StateTrader && DbFunctions.TruncateTime(ul.LoginDate) == DbFunctions.TruncateTime(DateTime.Now)
                                       //where ur.RoleId == (int)DTO.Enums.RoleType.StateTrader && DbFunctions.TruncateTime(ul.LoginDate) == targetDate
                                       select ul.LoginUserId).Distinct().Count();
                result.TotalLoginsBySales = (loginSalesCount);
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
                return _resultService.ErrorMessage(exception.Message);
            }
        }
        //#Endregion GetGoogle Analytics
    }
}
