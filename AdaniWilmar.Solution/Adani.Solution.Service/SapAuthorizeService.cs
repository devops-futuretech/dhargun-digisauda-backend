using System;
using System.Collections.Generic;
using System.Linq;
using GMCore.Authenticate;
using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using System.Data.Entity;
using Newtonsoft.Json;

namespace Adani.Solution.Service
{
    public interface ISapAuthorizeService
    {
      ResultDto AuthorizeUserSap(AuthorizeInputDto authorizeInputDto);
    }

    public class SapAuthorizeService : ISapAuthorizeService
    {
        private readonly IAdaniContext _emamiContext;
        private const string ServiceName = "Authorize Service";
        private string _methodName;
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly INotificationService _notificationService;
        private readonly IResultService _resultService;

        public SapAuthorizeService(IAdaniContext emamiContext, INotificationService notificationService, IResultService resultService)
        {
            try
            {
                _methodName = "Constructor";
                _emamiContext = emamiContext;
                _notificationService = notificationService;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        public ResultDto AuthorizeUserSap(AuthorizeInputDto authorizeInputDto)
        {
            _methodName = "AuthorizeUserSap";
            var resultDto = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Service-Method {_methodName} " +
                    $"Mobilenumber: {authorizeInputDto.MobileNumber} " +
                    $"Email: {authorizeInputDto.Email}" + $"VerticalId: {authorizeInputDto.VerticalId}");

                if (string.IsNullOrEmpty(authorizeInputDto.Password))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto = new ErrorDto
                    {
                        ErrorCode = Constants.InvalidRequest,
                        Message = Constants.InvalidRequest
                    };
                    _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                }
                else if (string.IsNullOrEmpty(authorizeInputDto.MobileNumber) &&
                        string.IsNullOrEmpty(authorizeInputDto.Email))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto = new ErrorDto
                    {
                        ErrorCode = Constants.InvalidRequest,
                        Message = Constants.InvalidRequest
                    };
                    _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                }
                else if (!authorizeInputDto.IsRequestFromWeb && authorizeInputDto.VerticalId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto = new ErrorDto
                    {
                        ErrorCode = Constants.VerticalCodeIsEmpty,
                        Message = Constants.VerticalCodeIsEmpty
                    };
                    _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                }
                else
                {
                    var selectedUser = new User();
                    var userRole = new UserRole();
                    if (!string.IsNullOrEmpty(authorizeInputDto.Email))
                    {
                        //if (authorizeInputDto.VerticalId > 0)
                        //{
                        //    selectedUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Email == authorizeInputDto.Email && u.DivisionId == authorizeInputDto.VerticalId && u.IsActive);
                        //}
                        //else
                        //{

                            selectedUser = _emamiContext.Users.AsNoTracking()
                           .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                           .Where(_ => _.User.Email == authorizeInputDto.Email //&& _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
                           && _.User.IsActive).ToList()
                           .Select(u => new User
                           {
                               Id = u.User.Id,
                               Name = u.User.Name,
                               Password = u.User.Password,
                               ImageUrl = u.User.ImageUrl,
                               IsActive = u.User.IsActive,
                               LastLoggedInDate = u.User.LastLoggedInDate,
                               //DivisionId = u.User.DivisionId,
                               HeadquartersId = u.User.HeadquartersId ?? 0
                           }
                           ).FirstOrDefault();
                        //}


                    }
                    else if (!string.IsNullOrEmpty(authorizeInputDto.MobileNumber))
                    {
                        //if (authorizeInputDto.VerticalId > 0)
                        //{
                        //    selectedUser = _emamiContext.Users.AsNoTracking()
                        //        .FirstOrDefault(u => u.MobileNumber == authorizeInputDto.MobileNumber 
                        //        && u.DivisionId == authorizeInputDto.VerticalId && u.IsActive);
                        //}
                        //else
                        //{
                            selectedUser = _emamiContext.Users.AsNoTracking()
                           .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                           .Where(_ => _.User.MobileNumber == authorizeInputDto.MobileNumber 
                           //&& _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
                           && _.User.IsActive).ToList()
                           .Select(u => new User
                           {
                               Id = u.User.Id,
                               Name = u.User.Name,
                               Password = u.User.Password,
                               ImageUrl = u.User.ImageUrl,
                               IsActive = u.User.IsActive,
                               LastLoggedInDate = u.User.LastLoggedInDate,
                               //DivisionId = u.User.DivisionId,
                               HeadquartersId = u.User.HeadquartersId ?? 0
                           }
                           ).FirstOrDefault();
                        //}
                    }


                    if (selectedUser == null || selectedUser.Id <= 0)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto = new ErrorDto
                        {
                            Message = Constants.InvalidLoginCredential
                        };
                        _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                    }
                    else if (string.IsNullOrEmpty(selectedUser.Password))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto = new ErrorDto
                        {
                            Message = Constants.InvalidLoginCredential
                        };
                        _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                    }
                    else
                    {
                        userRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == selectedUser.Id);
                        if (userRole.RoleId == 0)
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto = new ErrorDto
                            {
                                //ErrorCode = Constants.Unauthorised,
                                Message = Constants.Unauthorised
                            };
                            _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                        }
                        else if (!selectedUser.IsActive)
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto = new ErrorDto
                            {
                                //ErrorCode = Constants.InActiveUser,
                                Message = Constants.InActiveUser
                            };
                            _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                        }
                        else
                        {
                            //var password=UtilityHelper.ConvertMd5ToString(selectedUser.Password, SecurityConstants.EncryptionKey);
                            if (UtilityHelper.ConvertMd5ToString(selectedUser.Password, SecurityConstants.EncryptionKey) != authorizeInputDto.Password)
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto = new ErrorDto
                                {
                                    Message = Constants.InvalidLoginCredential
                                };
                                _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                            }
                            //else if (!authorizeInputDto.IsRequestFromWeb && (userRole.RoleId != (int)DTO.Enums.Role.StateTrader &&
                            //    userRole.RoleId != (int)DTO.Enums.Role.Dealer && userRole.RoleId != (int)DTO.Enums.Role.Broker))
                            //{
                            //    resultDto.IsSuccess = false;
                            //    resultDto.ErrorDto = new ErrorDto
                            //    {
                            //        ErrorCode = Constants.UserDontHavePermission,
                            //        Message = Constants.UserDontHavePermission
                            //    };
                            //    _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                            //}
                            else
                            {
                                var configurationContext = _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.PasswordExpiryEnabled);
                                if (configurationContext != null)
                                {
                                    if (configurationContext.Value == "True")
                                    {
                                        if (userRole.RoleId != (int)DTO.Enums.Role.Admin)
                                        {
                                            var configContext = _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.PasswordExpiryDays);
                                            if (configContext != null)
                                            {
                                                DateTime PasswordExpiredDate = selectedUser.PasswordModifiedDate.AddDays(Convert.ToInt32(configContext.Value));
                                                if (PasswordExpiredDate < DateHelper.UtcToIndia(DateTime.UtcNow))
                                                {
                                                    resultDto.IsSuccess = false;
                                                    resultDto.ErrorDto = new ErrorDto
                                                    {
                                                        Message = Constants.PasswordExpired
                                                    };
                                                    _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                                                    return resultDto;
                                                }
                                            }
                                        }
                                    }
                                }

                                //Update the login date-last and previous
                                if (null != selectedUser.LastLoggedInDate && selectedUser.LastLoggedInDate != DateTime.MinValue)
                                {
                                    selectedUser.PreviousLoggedInDate = selectedUser.LastLoggedInDate;
                                }
                                else
                                {
                                    selectedUser.PreviousLoggedInDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                }
                                selectedUser.LastLoggedInDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SaveChanges();

                                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                var currentTime = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);
                                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == selectedUser.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                                if (userRoleContext != null)
                                {
                                    var userAttendance = new UserAttendance();
                                    var userAttendanceContext = _emamiContext.UserAttendance.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate) && _.UserId == selectedUser.Id);
                                    var configContext = _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.LoginBaseHour);
                                    if (userAttendanceContext == null && configContext != null)
                                    {
                                        TimeSpan loginBaseHour = TimeSpan.FromHours(Convert.ToDouble(configContext.Value));
                                        if (currentTime >= loginBaseHour)
                                        {
                                            userAttendance = new UserAttendance()
                                            {
                                                UserId = selectedUser.Id,
                                                LoginTime = currentDate,
                                                CreatedBy = selectedUser.Id,
                                                CreatedDate = currentDate,
                                            };
                                            _emamiContext.UserAttendance.Add(userAttendance);
                                            _emamiContext.SaveChanges();
                                        }
                                    }
                                }

                                var newSystemToken = TokenManager.CreateJwtToken(new List<System.Security.Claims.Claim>
                                {
                                    new System.Security.Claims.Claim("System",EncryptDecryptHelper.Encrypt(SecurityConstants.LoginApiTokenKey
                                        ,SecurityConstants.EncryptionKey
                                        ,SecurityConstants.VectorKey))
                                });

                                resultDto.IsSuccess = true;
                                //Complaint management system Users - Claims list and assigned forms list
                                var ClaimIds = new List<int>();
                                if (userRole.Role != null && userRole.Role.RoleClaims != null)
                                {
                                    ClaimIds = userRole.Role.RoleClaims.Select(_ => _.ClaimId).ToList();
                                }
                                //var userFormContext = _emamiContext.FormUsers.AsNoTracking().Where(_ => _.UserId == selectedUser.Id && _.IsActive)
                                //                                                            .Select(_ => new FormDto()
                                //                                                            {
                                //                                                                FormId = _.FormId,
                                //                                                                FormName = _.Form != null ? _.Form.Name : string.Empty
                                //                                                            }).ToList();
                               
                                resultDto.SuccessDto = new SuccessDto
                                {
                                    Response = newSystemToken
                                };
                            }
                        }
                    }
                }
                _logger.Info($"Json Output : {JsonConvert.SerializeObject(resultDto.SuccessDto)}");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto = new ErrorDto
                {
                    ErrorCode = Constants.Exception,
                    Message = Constants.Exception
                };
                _logger.Error(message);
            }
            return resultDto;
        }

 
    }
}
