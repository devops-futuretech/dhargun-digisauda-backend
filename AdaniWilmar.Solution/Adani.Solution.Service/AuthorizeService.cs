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
using Newtonsoft.Json;

namespace Adani.Solution.Service
{
    public interface IAuthorizeService
    {
        ResultDto AuthorizeUser(AuthorizeInputDto authorizeInputDto);
        ResultDto ResetPassword(ResetPasswordDto resetPasswordDto);
        ResultDto ForgotPasswordOtpSend(ForgotPasswordDto forgotPasswordDto);
        ResultDto OtpReSend(UserIdDto userIdDto);
        ResultDto GetVerticalListBasedonUsername(AuthorizeInputDto authorizeInputDto);

        //Counter Bid
        //ResultDto GetSaudaCounterBidDetails(SaudaDetailInputDto inputDto);
        //ResultDto ApproveCounterBid(CounterBidInputDto inputDto);
        ResultDto UpdateLogOut(UserIdDto userIdDto);

        ResultDto AuthorizeUserSap(AuthorizeInputDto authorizeInputDto);
        ResultDto AuthorizeUserSapNew(AuthorizeInputDto authorizeInputDto);
    }

    public class AuthorizeService : IAuthorizeService
    {
        private readonly IAdaniContext _emamiContext;
        private const string ServiceName = "Authorize Service";
        private string _methodName;
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly INotificationService _notificationService;
        private readonly IResultService _resultService;

        public AuthorizeService(IAdaniContext emamiContext, INotificationService notificationService, IResultService resultService)
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

        public ResultDto AuthorizeUser(AuthorizeInputDto authorizeInputDto)
        {
            _methodName = "AuthorizeUser";
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
                        selectedUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Email == authorizeInputDto.Email && u.IsActive && !string.IsNullOrEmpty(u.Code));
                        //}
                        //else
                        //{

                        //    selectedUser = _emamiContext.Users.AsNoTracking()
                        //   .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                        //   .Where(_ => _.User.Email == authorizeInputDto.Email && _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
                        //   && _.User.IsActive).ToList()
                        //   .Select(u => new User
                        //   {
                        //       Id = u.User.Id,
                        //       Name = u.User.Name,
                        //       Password = u.User.Password,
                        //       ImageUrl = u.User.ImageUrl,
                        //       IsActive = u.User.IsActive,
                        //       LastLoggedInDate = u.User.LastLoggedInDate,
                        //       DivisionId = u.User.DivisionId,
                        //       HeadquartersId = u.User.HeadquartersId ?? 0,
                        //       OrganizationReportingToId = u.User.OrganizationReportingToId ?? 0
                        //   }
                        //   ).FirstOrDefault();
                        //}
                    }
                    else if (!string.IsNullOrEmpty(authorizeInputDto.MobileNumber))
                    {
                        //if (authorizeInputDto.VerticalId > 0)
                        //{
                        selectedUser = _emamiContext.Users
                            .FirstOrDefault(u => u.MobileNumber == authorizeInputDto.MobileNumber && u.IsActive && !string.IsNullOrEmpty(u.Code));
                        //}
                        //else
                        //{
                        //    selectedUser = _emamiContext.Users.AsNoTracking()
                        //   .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                        //   .Where(_ => _.User.MobileNumber == authorizeInputDto.MobileNumber && _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
                        //   && _.User.IsActive).ToList()
                        //   .Select(u => new User
                        //   {
                        //       Id = u.User.Id,
                        //       Name = u.User.Name,
                        //       Password = u.User.Password,
                        //       ImageUrl = u.User.ImageUrl,
                        //       IsActive = u.User.IsActive,
                        //       LastLoggedInDate = u.User.LastLoggedInDate,
                        //       DivisionId = u.User.DivisionId,
                        //       HeadquartersId = u.User.HeadquartersId ?? 0,
                        //       OrganizationReportingToId = u.User.OrganizationReportingToId ?? 0
                        //   }
                        //   ).FirstOrDefault();
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
                            else if (!authorizeInputDto.IsRequestFromWeb && (userRole.RoleId != (int)DTO.Enums.Role.StateTrader &&
                                userRole.RoleId != (int)DTO.Enums.Role.Dealer && userRole.RoleId != (int)DTO.Enums.Role.NationalTrader && userRole.RoleId != (int)DTO.Enums.Role.ZonalTrader))
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto = new ErrorDto
                                {
                                    ErrorCode = Constants.UserDontHavePermission,
                                    Message = Constants.UserDontHavePermission
                                };
                                _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                            }else if(authorizeInputDto.IsRequestFromWeb && (userRole.RoleId == (int)DTO.Enums.Role.Dealer ||
                                userRole.RoleId == (int)DTO.Enums.Role.ShipToParty || userRole.RoleId == (int)DTO.Enums.Role.Broker )){
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto = new ErrorDto
                                {
                                    ErrorCode = Constants.UserDontHavePermission,
                                    Message = Constants.UserDontHavePermission
                                };
                                _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                            }
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

                                bool isApplySpecialityFatDiscount = false;
                                var applySpecialityFatDiscount = Utility.GetEnumDescription(DTO.Enums.Configuration.IsApplySpecialityFatDiscount);
                                var configurationSpecialityFatDiscountContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == applySpecialityFatDiscount);
                                if (configurationSpecialityFatDiscountContext != null)
                                {
                                    isApplySpecialityFatDiscount = Convert.ToBoolean(configurationSpecialityFatDiscountContext.Value);
                                }
                                var authorizeOutputDto = new AuthorizeOutputDto
                                {
                                    LoginToken = newSystemToken,
                                    Name = selectedUser.Name,
                                    Code = selectedUser.Code,
                                    RoleId = userRole.RoleId.ToString(),
                                    RoleTypeId = userRole.Role != null ? userRole.Role.RoleTypeId : 0,
                                    UserId = selectedUser.Id,
                                    ProfileName = selectedUser.ImageUrl,
                                    //VerticalId = selectedUser.DivisionId ?? 0,
                                    HeadquartersId = selectedUser.HeadquartersId ?? 0,
                                    OrganizationReportingToId = selectedUser.ReportingToId ?? 0,
                                    UserClaimIds = ClaimIds,
                                    //FormUsers = userFormContext,
                                    IsApplySpecialityFatDiscount = isApplySpecialityFatDiscount,
                                    //Code = selectedUser.Code,
                                    StateId = selectedUser.StateId,
                                    ProfilePath=selectedUser.ProfilePath
                                };
                                resultDto.SuccessDto = new SuccessDto
                                {
                                    Response = authorizeOutputDto
                                };
                            }
                        }
                    }
                }

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

        public ResultDto ForgotPasswordOtpSend(ForgotPasswordDto forgotPasswordDto)
        {
            _methodName = "ForgotPasswordOtpSend";
            var resultDto = new ResultDto();
            try
            {
                if (string.IsNullOrEmpty(forgotPasswordDto?.UserName))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }


                ///TO-DO
                var otpNumber = UtilityHelper.GenerateRandomId(6);
                //var otpNumber = "123456";

                User userContext;
                if (UtilityHelper.IsDigitsOnly(forgotPasswordDto.UserName))
                {
                    userContext = _emamiContext.Users.FirstOrDefault(_ => _.MobileNumber == forgotPasswordDto.UserName 
                    && _.ShipToPartyCode==null
                       //&& _.DivisionId == forgotPasswordDto.VerticalId
                       );
                    

                       // userContext = _emamiContext.Users.AsNoTracking()
                       //.Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                       //.Where(_ => _.User.MobileNumber == forgotPasswordDto.UserName && _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
                       //&& _.User.IsActive).ToList()
                       //.Select(u => new User
                       //{
                       //    Id = u.User.Id,
                       //    Name = u.User.Name,
                       //    Password = u.User.Password,
                       //    ImageUrl = u.User.ImageUrl,
                       //    IsActive = u.User.IsActive,
                       //    LastLoggedInDate = u.User.LastLoggedInDate,
                       //    MobileNumber = u.User.MobileNumber,
                       //    Email = u.User.Email
                       //}
                       //).FirstOrDefault();
                    
                    if (userContext == null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.InvalidUser;
                        resultDto.ErrorDto.Message = Constants.InvalidUser;
                        return resultDto;
                    }
                    if (!userContext.IsActive)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.InActiveUser;
                        resultDto.ErrorDto.Message = Constants.InActiveUser;
                        return resultDto;
                    }
                    userContext.OtpNumber = otpNumber;
                }
                else
                {
                    userContext = _emamiContext.Users.FirstOrDefault(_ => _.Email == forgotPasswordDto.UserName //&& _.DivisionId == forgotPasswordDto.VerticalId
                        );
                    //else
                    //{

                    //    userContext = _emamiContext.Users.AsNoTracking()
                    //   .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                    //   .Where(_ => _.User.Email == forgotPasswordDto.UserName && _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
                    //   && _.User.IsActive).ToList()
                    //   .Select(u => new User
                    //   {
                    //       Id = u.User.Id,
                    //       Name = u.User.Name,
                    //       Password = u.User.Password,
                    //       ImageUrl = u.User.ImageUrl,
                    //       IsActive = u.User.IsActive,
                    //       LastLoggedInDate = u.User.LastLoggedInDate,
                    //       MobileNumber = u.User.MobileNumber,
                    //       Email = u.User.Email
                    //   }
                    //   ).FirstOrDefault();
                    //}
                    if (userContext == null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.InvalidUser;
                        resultDto.ErrorDto.Message = Constants.InvalidUser;
                        return resultDto;
                    }
                    userContext.OtpNumber = otpNumber;
                }

                var amazonNotificationService = new AmazonNotificationService();

                if (_resultService.IsSMS())
                {
                    var mobileTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name == Constants.OtpSms);
                    if (_resultService.IsSMS())
                    {
                        if (mobileTemplate != null && !string.IsNullOrEmpty(userContext.MobileNumber))
                        {
                            var replaceMobileTemplates = mobileTemplate.PlainTemplate.Replace(Constants.OtpValue, otpNumber);
                            //_notificationService.SendMessage(replaceMobileTemplates, userContext.MobileNumber.Trim());
                            amazonNotificationService.SendMessage(replaceMobileTemplates, userContext.MobileNumber.Trim(), mobileTemplate.SMSTemplateID);

                            //var pushNotificationInputDto = new PushNotificationInputDto
                            //{
                            //    PushTokenKey = "cGGnYT2TMEs:APA91bHCswbPLB32x88LUZh7qpcIlv2qIkV4Oa8B92iwL6DvAcHfbq8NcncjYYB09TF1GOPs_D-WyEGqtfghmC8ouJHEnF-3Ckj9azdBU1ocaH61XIxH_6-qRlm2RAk7ZL5kraJDNfq-",
                            //    RegistrationTypeId = 1,
                            //    Title = Constants.ResendOtpEmailSubject,
                            //    Message = replaceMobileTemplates
                            //};

                            //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                        }
                    }
                }

                var toUser = new List<string>();


                if (_resultService.IsEmail())
                {
                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.OtpEmail);
                    if (_resultService.IsEmail())
                    {
                        if (emailTemplate != null && !string.IsNullOrEmpty(userContext.Email))
                        {
                            var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.OtpValue, otpNumber);
                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                            toUser.Add(userContext.Email.Trim());
                            //_notificationService.SendEmail(userContext.Email.Trim(), Constants.ResendOtpEmailSubject, htmlTemplate);
                            amazonNotificationService.SendEmail(toUser, Constants.ResendOtpEmailSubject, string.Empty, htmlTemplate, true);
                        }
                    }
                }




                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userContext.Id;
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

        public ResultDto OtpReSend(UserIdDto userIdDto)
        {
            _methodName = "OtpReSend";
            var resultDto = new ResultDto();
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
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                ///TO-DO
                var otpNumber = UtilityHelper.GenerateRandomId(6);
                //var otpNumber = "123456";

                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == userIdDto.UserId);
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
                userContext.OtpNumber = otpNumber;

                var amazonNotificationService = new AmazonNotificationService();
                if (_resultService.IsSMS())
                {
                    var mobileTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(_ => _.Name == Constants.OtpSms);
                    if (mobileTemplate != null && !string.IsNullOrEmpty(userContext.MobileNumber))
                    {
                        var replaceMobileTemplates = mobileTemplate.PlainTemplate.Replace(Constants.OtpValue, otpNumber);
                        //_notificationService.SendMessage(replaceMobileTemplates, userContext.MobileNumber.Trim());
                        amazonNotificationService.SendMessage(replaceMobileTemplates, userContext.MobileNumber.Trim(), mobileTemplate.SMSTemplateID);
                    }
                }

                if (_resultService.IsEmail())
                {
                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.OtpEmail);
                    if (emailTemplate != null && !string.IsNullOrEmpty(userContext.Email))
                    {
                        var toUser = new List<string>();
                        var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.OtpValue, otpNumber);
                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                        toUser.Add(userContext.Email.Trim());
                        //_notificationService.SendEmail(userContext.Email.Trim(), Constants.ResendOtpEmailSubject, htmlTemplate);
                        amazonNotificationService.SendEmail(toUser, Constants.ResendOtpEmailSubject, string.Empty, htmlTemplate, true);
                    }
                }


                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userContext.Id;
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

        public ResultDto ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            _methodName = "ResetPassword";
            var resultDto = new ResultDto();
            try
            {
                if (resetPasswordDto.UserId == 0 || string.IsNullOrEmpty(resetPasswordDto.OtpNumber) || string.IsNullOrEmpty(resetPasswordDto.NewPassword))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var userId = resetPasswordDto.UserId;
                var user = _emamiContext.Users.FirstOrDefault(_ => _.Id == userId);
                if (user == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                if (!user.IsActive)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InActiveUser;
                    resultDto.ErrorDto.Message = Constants.InActiveUser;
                    return resultDto;
                }
                if (user.OtpNumber != resetPasswordDto.OtpNumber)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InValidOtpNumber;
                    resultDto.ErrorDto.Message = Constants.InValidOtpNumber;
                    return resultDto;
                }
                user.Password = UtilityHelper.ConvertToMd5(resetPasswordDto.NewPassword, SecurityConstants.EncryptionKey);
                user.ModifiedBy = user.Id;
                user.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                user.PasswordModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = user.Id;
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

        public ResultDto GetVerticalListBasedonUsername(AuthorizeInputDto authorizeInputDto)
        {
            _methodName = "GetVerticalListBasedonUsername";
            var resultDto = new ResultDto();
            var verticalList = new List<Division>();
            try
            {
                _logger.Info($"{ServiceName} Service-Method {_methodName} " +
                    $"Mobilenumber: {authorizeInputDto.MobileNumber} " +
                    $"Email: {authorizeInputDto.Email}");


                if (string.IsNullOrEmpty(authorizeInputDto.MobileNumber) &&
                        string.IsNullOrEmpty(authorizeInputDto.Email))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto = new ErrorDto
                    {
                        ErrorCode = Constants.InvalidRequest,
                        Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage)
                    };
                    _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                }
                else
                {
                    var selectedUser = new User();
                    var userRole = new UserRole();
                    if (!string.IsNullOrEmpty(authorizeInputDto.Email))
                    {
                        selectedUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Email == authorizeInputDto.Email && u.IsActive);
                    }
                    else if (!string.IsNullOrEmpty(authorizeInputDto.MobileNumber))
                    {
                        selectedUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.MobileNumber == authorizeInputDto.MobileNumber && u.IsActive);
                    }

                    if (selectedUser == null || selectedUser.Id <= 0)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto = new ErrorDto
                        {
                            ErrorCode = Constants.UserNotFound,
                            Message = Constants.GetMessage(Constants.UserNotFound, Utility.MessageLanguage)
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
                                ErrorCode = Constants.Unauthorised,
                                Message = Constants.GetMessage(Constants.Unauthorised, Utility.MessageLanguage)
                            };
                            _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                        }
                        else if (!selectedUser.IsActive)
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto = new ErrorDto
                            {
                                ErrorCode = Constants.InActiveUser,
                                Message = Constants.GetMessage(Constants.InActiveUser, Utility.MessageLanguage)
                            };
                            _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                        }
                        else
                        {

                            //if (userRole.RoleId == (int)DTO.Enums.Role.Admin)
                            //{
                            //    resultDto.IsSuccess = true;
                            //    resultDto.SuccessDto.Response = string.Empty;
                            //    return resultDto;
                            //}

                            if (!string.IsNullOrEmpty(authorizeInputDto.MobileNumber))
                            {
                                verticalList = _emamiContext.Divisions.AsNoTracking()
                                .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), v => v.Id, u => u.DivisionId, (v, u) => new { Vertical = v, User = u })
                                .Where(_ => _.User.User.MobileNumber == authorizeInputDto.MobileNumber
                                && _.Vertical.IsActive && _.User.User.IsActive).ToList()
                                .Select(ve => new Division
                                {
                                    Id = ve.Vertical.Id,
                                    Name = ve.Vertical.Name
                                }).Distinct().ToList();
                            }
                            else if (!string.IsNullOrEmpty(authorizeInputDto.Email))
                            {
                                verticalList = _emamiContext.Divisions.AsNoTracking()
                                .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), v => v.Id, u => u.DivisionId, (v, u) => new { Vertical = v, User = u })
                                .Where(_ => _.User.User.Email == authorizeInputDto.Email
                                && _.Vertical.IsActive && _.User.User.IsActive).ToList()
                                .Select(ve => new Division
                                {
                                    Id = ve.Vertical.Id,
                                    Name = ve.Vertical.Name
                                }).Distinct().ToList();
                            }
                        }
                    }
                }

                if (verticalList == null || !verticalList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto = new ErrorDto
                    {
                        ErrorCode = Constants.RecordNotFound,
                        Message = Constants.GetMessage(Constants.RecordNotFound, Utility.MessageLanguage)
                    };
                    return resultDto;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = verticalList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto = new ErrorDto
                {
                    ErrorCode = Constants.Exception,
                    Message = Constants.Exception,
                };
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto UpdateLogOut(UserIdDto userIdDto)
        {
            _methodName = "UpdateLogOut";
            var resultDto = new ResultDto();
            try
            {
                if (userIdDto.UserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var userAttendance = _emamiContext.UserAttendance.FirstOrDefault(_ => _.UserId == userIdDto.UserId /*&& DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate)*/);
                //if (userAttendance == null)
                //{
                //    resultDto.IsSuccess = false;
                //    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                //    resultDto.ErrorDto.Message = Constants.UserNotFound;
                //    return resultDto;
                //}

                if (userAttendance != null)
                {
                    userAttendance.LogoutTime = currentDate;
                    userAttendance.ModifiedBy = userIdDto.UserId;
                    userAttendance.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.SuccessMessage;
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

        #region Counter Bid

        //public ResultDto GetSaudaCounterBidDetails(SaudaDetailInputDto inputDto)
        //{
        //    _methodName = "GetSaudaCounterBidDetails";
        //    try
        //    {
        //        var saudaOrderDetails = new SaudaOrderDetails();
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId);
        //        if (saudaOrderContext != null)
        //        {
        //            if (saudaOrderContext.Sauda != null)
        //            {
        //                var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
        //                if (dealerContext != null)
        //                {
        //                    saudaOrderDetails.DealerId = dealerContext.Id;
        //                    saudaOrderDetails.DealerName = dealerContext.Name;
        //                }
        //                saudaOrderDetails.BookedDate = saudaOrderContext.Sauda.BiddingDate;
        //            }
        //            saudaOrderDetails.SaudaId = saudaOrderContext.Id;
        //            saudaOrderDetails.SaudaOrderId = saudaOrderContext.Id;
        //            saudaOrderDetails.SaudaNumber = saudaOrderContext.SaudaNumber;
        //            saudaOrderDetails.ValidToDate = saudaOrderContext.ValidToDate;
        //            saudaOrderDetails.OilTypeId = saudaOrderContext.OilTypeId;
        //            saudaOrderDetails.OilTypeName = saudaOrderContext.OilType != null ? saudaOrderContext.OilType.Name : string.Empty;
        //            saudaOrderDetails.SkuId = saudaOrderContext.SkuId;
        //            saudaOrderDetails.SkuName = saudaOrderContext.Sku != null ? saudaOrderContext.Sku.SkuName : string.Empty;
        //            saudaOrderDetails.StatusId = saudaOrderContext.StatusId;
        //            var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.StatusId);
        //            if (statusContext != null)
        //            {
        //                saudaOrderDetails.Status = statusContext.Name;
        //            }
        //            IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderContext.Id
        //                && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);
        //            if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
        //            {
        //                saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity - liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
        //                saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase);
        //            }
        //            else
        //            {
        //                saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity;
        //                saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase;
        //            }
        //            saudaOrderDetails.BidPrice = saudaOrderContext.BidPrice;
        //            saudaOrderDetails.BidPricePerCase = Math.Round((saudaOrderContext.BidPrice != 0 && saudaOrderContext.BidQuantityCase != 0 ? (saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase) : 0), 2);
        //            saudaOrderDetails.IncoTerms = saudaOrderContext.Incoterms1;
        //            var plantContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.PlantId);
        //            if (plantContext != null)
        //            {
        //                saudaOrderDetails.PlantDepot = plantContext.Name;
        //            }
        //            //var freightRouteContext = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.DealerLocationId);
        //            //if (freightRouteContext != null)
        //            //{
        //            //    saudaOrderDetails.FrieghtRoute = freightRouteContext.Name;
        //            //}
        //            saudaOrderDetails.CounterBidOffer = saudaOrderContext.CounterBidOffer;
        //            saudaOrderDetails.CounterBidOfferDate = saudaOrderContext.CounterBidOfferDate != null ? saudaOrderContext.CounterBidOfferDate.Value : DateTime.MinValue;
        //        }
        //        return _resultService.SuccessObject(saudaOrderDetails);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto ApproveCounterBid(CounterBidInputDto inputDto)
        //{
        //    _methodName = "ApproveCounterBid";
        //    try
        //    {
        //        string responseMessage = string.Empty;
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.Sauda != null
        //            && DbFunctions.TruncateTime(_.Sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate) && _.CounterBidOffer != 0 && _.CounterBidOfferDate != null);
        //        if (saudaOrderContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.SaudaNotFound);
        //        }
        //        else
        //        {
        //            decimal couterBidOffer = 0;
        //            if (inputDto.IsAccept)
        //            {
        //                var configContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.CounterBidBufferTime);
        //                if (configContext != null)
        //                {
        //                    var bufferTime = TimeSpan.FromMinutes(Convert.ToInt32(configContext.Value));
        //                    var timeLimit = saudaOrderContext.CounterBidOfferDate.Value.TimeOfDay + bufferTime;
        //                    if (timeLimit < currentDate.TimeOfDay)
        //                    {
        //                        return _resultService.ErrorMessage(Constants.CounterBidOfferTimeLimitExceeds);
        //                    }
        //                }
        //                else
        //                {
        //                    return _resultService.ErrorMessage(Constants.RecordNotFound);
        //                }
        //                saudaOrderContext.StatusId = (int)DTO.Enums.Status.Pending;
        //                couterBidOffer = saudaOrderContext.CounterBidOffer;
        //                saudaOrderContext.CounterBidOffer = saudaOrderContext.BidPrice;
        //                saudaOrderContext.BidPrice = couterBidOffer;
        //                saudaOrderContext.ModifiedBy = saudaOrderContext.CreatedBy;
        //                saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                _emamiContext.SaveChanges();
        //                responseMessage = Constants.CounterBidSuccess;
        //            }
        //            else
        //            {
        //                couterBidOffer = saudaOrderContext.CounterBidOffer;
        //                saudaOrderContext.StatusId = (int)DTO.Enums.Status.Rejected;
        //                saudaOrderContext.ModifiedBy = saudaOrderContext.CreatedBy;
        //                saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                _emamiContext.SaveChanges();
        //                responseMessage = Constants.CounterBidReject;
        //            }
        //            try
        //            {
        //                List<User> usersContext = new List<User>();
        //                List<string> toUsers = new List<string>();
        //                User createdBy = new User();
        //                User dealer = new User();
        //                if (saudaOrderContext.CreatedBy == saudaOrderContext.Sauda.UserId)
        //                {
        //                    createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.CreatedBy);
        //                    if (createdBy != null)
        //                    {
        //                        toUsers.Add(createdBy.Email);
        //                    }
        //                }
        //                else
        //                {
        //                    usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaOrderContext.CreatedBy || _.Id == saudaOrderContext.Sauda.UserId).ToList();
        //                    if (usersContext != null && usersContext.Any())
        //                    {
        //                        createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.CreatedBy);
        //                        dealer = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
        //                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
        //                        {
        //                            toUsers.Add(createdBy.Email);
        //                        }
        //                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
        //                        {
        //                            toUsers.Add(dealer.Email);
        //                        }
        //                    }
        //                }
        //                if ((usersContext != null && usersContext.Any()) || createdBy != null)
        //                {
        //                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                    string emailSubject = string.Empty;
        //                    if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                    {
        //                        var fromEmail = Constants.FromEmail;
        //                        var plainText = string.Empty;
        //                        EmailTemplate emailTemplate = new EmailTemplate();
        //                        if (inputDto.IsAccept)
        //                        {
        //                            emailSubject = Constants.CounterBidAcceptSubject;
        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowEmail);
        //                        }
        //                        else
        //                        {
        //                            emailSubject = Constants.CounterBidRejectSubject;
        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
        //                        }

        //                        if (emailTemplate != null)
        //                        {
        //                            string plainTemplate = string.Empty;
        //                            string htmlTemplate = string.Empty;
        //                            if (toUsers.Count > 1)
        //                            {
        //                                plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealer.Name);
        //                            }
        //                            else
        //                            {
        //                                plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, createdBy.Name);
        //                            }
        //                            htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                        }

        //                    }
        //                    var smsPlainTemplate = string.Empty;
        //                    if (_resultService.IsSMS())
        //                    {
        //                        var smsMessage = string.Empty;
        //                        EmailTemplate smsTemplate = new EmailTemplate();
        //                        if (inputDto.IsAccept)
        //                        {
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowSMS);
        //                        }
        //                        else
        //                        {
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
        //                        }
        //                        if (smsTemplate != null)
        //                        {
        //                            if (toUsers.Count > 1)
        //                            {
        //                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealer.Name);
        //                            }
        //                            else
        //                            {
        //                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, createdBy.Name);
        //                            }
        //                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
        //                            {
        //                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
        //                            }
        //                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
        //                            {
        //                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
        //                            }
        //                        }
        //                    }
        //                    if (_resultService.IsPushNotification())
        //                    {
        //                        if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
        //                        {
        //                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                            {
        //                                PushTokenKey = createdBy.PushTokenKey,
        //                                RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
        //                                Title = emailSubject,
        //                                Message = smsPlainTemplate,
        //                                //Id = saudaOrderContext.Id,
        //                            };
        //                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                        }
        //                        if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
        //                        {
        //                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                            {
        //                                PushTokenKey = dealer.PushTokenKey,
        //                                RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
        //                                Title = emailSubject,
        //                                Message = smsPlainTemplate,
        //                                //Id = saudaOrderContext.Id,
        //                            };
        //                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //            return _resultService.SuccessMessage(responseMessage);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}
        #endregion

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
                        if (authorizeInputDto.VerticalId > 0)
                        {
                            selectedUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Email == authorizeInputDto.Email
                            //&& u.DivisionId == authorizeInputDto.VerticalId 
                            && u.IsActive);
                        }
                        else
                        {

                            selectedUser = _emamiContext.Users.AsNoTracking()
                           .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                           .Where(_ => _.User.Email == authorizeInputDto.Email && _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
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
                        }


                    }
                    else if (!string.IsNullOrEmpty(authorizeInputDto.MobileNumber))
                    {
                        if (authorizeInputDto.VerticalId > 0)
                        {
                            selectedUser = _emamiContext.Users.AsNoTracking()
                                .FirstOrDefault(u => u.MobileNumber == authorizeInputDto.MobileNumber
                                //&& u.DivisionId == authorizeInputDto.VerticalId 
                                && u.IsActive);
                        }
                        else
                        {
                            selectedUser = _emamiContext.Users.AsNoTracking()
                           .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                           .Where(_ => _.User.MobileNumber == authorizeInputDto.MobileNumber && _.UserRole.RoleId == (int)DTO.Enums.Role.Admin
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
                        }
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

        public ResultDto AuthorizeUserSapNew(AuthorizeInputDto authorizeInputDto)
        {
            _methodName = "AuthorizeUserSapNew";
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
                       .Where(_ => _.User.Email == authorizeInputDto.Email /*&& _.UserRole.RoleId == (int)DTO.Enums.Role.Admin*/
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
                        //        .FirstOrDefault(u => u.MobileNumber == authorizeInputDto.MobileNumber && u.DivisionId == authorizeInputDto.VerticalId && u.IsActive);
                        //}
                        //else
                        //{
                        selectedUser = _emamiContext.Users.AsNoTracking()
                       .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { UserRole = ur, User = u })
                       .Where(_ => _.User.MobileNumber == authorizeInputDto.MobileNumber /*&& _.UserRole.RoleId == (int)DTO.Enums.Role.Admin*/
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
