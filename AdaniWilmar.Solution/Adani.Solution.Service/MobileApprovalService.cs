using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMCore.Helper;
using System.Web.Hosting;
using System.Globalization;
using System.IO;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Adani.Solution.Service
{
    public interface IMobileApprovalService
    {
        //Special Rate
        //ResultDto GetSpecialRateApprovalList(SpecialRateAddInputDto inputDto);
        //ResultDto GetSpecialRateRequestDetails(SpecialRateDetailInputDto specialRateDetailInputDto);
        ResultDto SpecialRateApproval(SpecialRateApprovalDto inputDto);
        ResultDto GetSpecialRateApprovals(SpecialRateAddInputDto inputDto);
        ResultDto GetSpecialRateApprovalDetail(IdInputDto inputDto);
        ResultDto GetSpecialRateRequestList(SpecialRateInputDto inputDto);
        ResultDto GetSpecialRateRequestDetails(SpecialRateDetailInputDto specialRateDetailInputDto);
        ResultDto SaudaCreationFromSpecialRate(SpecialRateSaudaDto inputDto);

        ////CompetitorAnalysis 
        ResultDto GetCompetitorAnalysisList(LoginUserIdDto inputDto);
        ResultDto GetCompetitorAnalysisById(IdInputDto inputDto);
        ResultDto GetCompetitorAnalysisDetailsListById(IdInputDto inputDto);
        ResultDto SaveCompetitorAnalysisApproval(CompetitorAnalysisApprovalDto inputDto);

        //Permanent Coverage Plan
        ResultDto GetPendingPermanentJourneyPlanList(LoginUserIdDto inputDto);
        ResultDto GetPermanentJourneyPlanList(IdInputDto inputDto);
        ResultDto GetPermanentJourneyPlanDetails(PJPIdDto inputDto);
        ResultDto PermanentJourneyPlanApproval(PermanentJourneyPlanUpdateDto inputDto);
        ResultDto GetTotalPCPByUsers(IdInputDto inputDto);

        //Monthly Tour Plan
        ResultDto GetPendingMonthlyTourPlanList(LoginUserIdDto inputDto);
        ResultDto GetMonthlyTourPlanList(MTPDateWiseDetailsInputDto inputDto);
        ResultDto GetMonthlyTourPlanDetails(MTPIdDto inputDto);
        ResultDto MonthlyTourPlanApproval(MonthlyTourPlanUpdateDto inputDto);

        //Monthly Tour Plan Deviation
        ResultDto MonthlyPlanDeviationList(LoginUserIdDto inputDto);
        ResultDto MonthlyPlanDeviationApproval(MonthlyPlanDeviationDto inputDto);

        //Sauda
        ResultDto SaudaApproval(SaudaApproveInputDto inputDto);
        ResultDto GetPendingSaudaChartForMobile(LoginZHId inputDto);
        ResultDto GetPendingSaudaChartDetailForMobile(LoginZHId inputDto);
        ResultDto GetBookedSauda(LoginZHId inputDto);
        ResultDto SaudaCreation(SaudaInputDto inputDto);
        ResultDto GetSaudaorderdetails(SaudaDetailInputDto inputDto);

        //Lifting
        ResultDto GetLiftingRequestCountList(LiftingRequestListInputDto inputDto);
        ResultDto GetDealersLiftingRequestList(DealersLiftingRequestInputDto inputDto);
        ResultDto LiftingRequestApproval(LiftingRequestStatusChangeDto inputDto);
        ResultDto GetLiftingRequestListByBDO(LiftingRequestListInputDto inputDto);

        //Discount
        ResultDto GetEmployeeAndUserDiscountList(LoginUserIdDto inputDto);
        ResultDto AddEmployeeAndUserDiscount(EmployeeUserDiscountDto inputDto);
        ResultDto UpdateDiscountUsers(DiscountUserDto inputDto);
        ResultDto GetDiscountUserList(LoginUserIdDto inputDto);

        ResultDto GetMultiselectDiscountList(LoginUserIdDto inputDto);
        ResultDto AssignMultiselectDiscount(DiscountUserDto inputDto);
        ResultDto AssignedMultiselectDiscountList(LoginUserIdDto inputDto);
        ResultDto UpdateMultiselectDiscountUsers(DiscountUserDto inputDto);

        //Sauda Limit
        ResultDto SaudaLimitApproval(SaudaLimitRequestInputDto inputDto);

        //Secondary Sales
        ResultDto GetSecondarySalesFortheDay(LoginZHId inputDto);

        //Sauda Conversion
        ResultDto GetSaudaConversionList(SaudaFilterDto inputDto);
        ResultDto SaudaConversionApproval(SaudaConversionApprovalInputDto inputDto);

        //Sauda Extension
        ResultDto GetSaudaExtensionList(SaudaFilterDto inputDto);
        ResultDto SaudaExtensionApproval(SaudaConversionApprovalInputDto inputDto);

        //Premium
        ResultDto GetPremiumList(LoginUserIdDto inputDto);
        ResultDto AssignPremium(EmployeeUserPremiumDto inputDto);
        ResultDto UpdatePremium(PremiumUserDto inputDto);
        ResultDto GetAssignedPremiumList(LoginUserIdDto inputDto);

        ResultDto GetMultiselectPremiumList(LoginUserIdDto inputDto);
        ResultDto AssignMultiselectPremium(PremiumUserDto inputDto);

        //Speciality Fat quantity allocation
        ResultDto GetSpecialityFatQuantityLimitList(LoginUserIdDto inputDto);
        ResultDto AssignSpecialityFatQuantityLimit(SpecialityFatEmployeeDiscountDto inputDto);
        ResultDto UpdateAssignedSpecialityFatQuantityLimit(SpecialityFatDiscountUserDto inputDto);
        ResultDto GetAssignedSpecialityFatQuantityLimitList(LoginUserIdDto inputDto);

        //Speciality Fat quantity request
        ResultDto AddSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto);
        ResultDto GetSpecialtyFatQuantityRequestsList(SpecialtyFatQuantityRequestSearchDto inputDto);
        ResultDto GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(SpecialtyFatQuantityRequestSearchDto inputDto);
        ResultDto UpdateSpecialtyFatQuantityLimit(SpecialtyFatQuantityRequestDto inputDto);
        ResultDto GetPendingContractChartMobile(LoginZHId loginUserIdDto);

        ResultDto GetCreditLimitAndCreditExposureList(CreditLimitAndCreditExposureInputDto inputDto);
        ResultDto GetContactListForActiveCallToCustomers(ContactListForActiveCallInputDto inputDto);
        ResultDto SaveCallRecordingOfCustomers(ContactListForActiveCallInputDto inputDto);
        ResultDto GetAudioFilesListAgainstCustomers(ContactListForActiveCallInputDto inputDto);
        ResultDto SaveSaudadetailsMappedAgainstAudiofiles(ContactListForActiveCallInputDto inputDto);
        ResultDto GetSpecialRateRequestNewList(SpecialRateInputDto inputDto);
        ResultDto GetCreditLimitAndCreditExposureListAPP(CreditLimitAndCreditExposureInputDto inputDto);
        ResultDto GetDateRangeList();
        ResultDto SaveDealerDetails(SaveDealerDetails inputDto);
    }
    public class MobileApprovalService : IMobileApprovalService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Sauda Service");
        private const string ServiceName = "Sauda Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;
        private readonly ISaudaServices _saudaService;
        private readonly ISAPIntegrationService _sapIntegrationService;

        public MobileApprovalService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService, ISaudaServices saudaService, ISAPIntegrationService sapIntegrationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
                _saudaService = saudaService;
                _sapIntegrationService = sapIntegrationService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for sauda Service", exception);
            }
        }

        private ResultDto NotFoundResult()
        {
            var resultDto = new ResultDto();
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
            return resultDto;
        }
        private ResultDto ExceptionResult(Exception exception)
        {
            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
            var resultDto = new ResultDto();
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.ErrorCode = Constants.Exception;
            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
            _logger.Error(message);
            return resultDto;
        }
        private ResultDto SucessResult(Object obj)
        {
            var resultDto = new ResultDto();
            resultDto.IsSuccess = true;
            resultDto.SuccessDto.Response = obj;
            return resultDto;
        }

        #region Special Rate

        public ResultDto SpecialRateApproval(SpecialRateApprovalDto inputDto)
        {
            _methodName = "SpecialRateApproval";
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var result = _emamiContext.SpecialRate.FirstOrDefault(_ => _.Id == inputDto.Id);
                if (result == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                {
                    inputDto.RequestedTo = 0;
                }
                else
                {
                    var users = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId);
                    if (users != null && users.Any())
                    {
                        inputDto.RequestedTo = (long)users.FirstOrDefault().ReportingToId;
                    }
                }
                if (result != null && (result.StatusId == (int)DTO.Enums.Status.Pending || result.StatusId == (int)DTO.Enums.Status.RequestForApproval))
                {
                    var input = new SpecialRateApproval
                    {
                        SpecialRateId = inputDto.Id,
                        RequestedBy = inputDto.LoginUserId,
                        RequestedTo = inputDto.RequestedTo,
                        ApprovedBy = inputDto.LoginUserId,
                        StatusId = inputDto.StatusId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.SpecialRateApproval.Add(input);
                    _emamiContext.SaveChanges();

                    result.StatusId = inputDto.StatusId;
                    result.Remarks = inputDto.Remarks;
                    _emamiContext.SaveChanges();

                    #region Send Email and SMS

                    try
                    {
                        bool isEmail = false;
                        var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                        Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                        .Where(_ => _.TPND.DealerId == result.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SpecialRateApproval && _.TPND.IsActive).ToList();

                        var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                        if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                            isEmail = true;
                        else
                            isEmail = false;

                        List<User> usersContext = new List<User>();
                        User createdBy = new User(); createdBy = null;
                        User dealer = new User(); dealer = null;
                        if (result.CreatedBy == result.UserId)
                        {
                            createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == result.UserId);
                        }
                        else
                        {
                            usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == result.CreatedBy || _.Id == result.UserId).ToList();
                            if (usersContext != null && usersContext.Any())
                            {
                                createdBy = usersContext.FirstOrDefault(_ => _.Id == result.CreatedBy);
                                dealer = usersContext.FirstOrDefault(_ => _.Id == result.UserId);
                            }
                        }

                        if ((usersContext != null && usersContext.Any()) || createdBy != null)
                        {
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            var emailSubject = string.Empty;
                            if (isEmail)
                            {
                                var fromEmail = Constants.FromEmail;
                                var plainText = string.Empty;
                                EmailTemplate emailTemplate = new EmailTemplate();
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    emailSubject = Constants.SpecialRateApprovalSubject;
                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateApprovalEmail);
                                }
                                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    emailSubject = Constants.SpecialRateRejectSubject;
                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateRejectEmail);
                                }
                                if (emailTemplate != null)
                                {
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email) && dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                    {
                                        List<string> toUsers = new List<string>();
                                        toUsers.Add(createdBy.Email);
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, dealer.Name)
                                            .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                            .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                        toUsers = new List<string>();
                                        toUsers.Add(dealer.Email);
                                        plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, dealer.Name).Replace(Constants.CustomerName, dealer.Name)
                                            .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                            .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                        htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email) && dealer == null)
                                    {
                                        List<string> toUsers = new List<string>();
                                        toUsers.Add(createdBy.Email);
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, createdBy.Name)
                                            .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                            .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                            }
                            bool isSms = false;
                            var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                            if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                isSms = true;
                            else
                                isSms = false;
                            var smsPlainTemplateCreatedBy = string.Empty;
                            var smsPlainTemplateDealer = string.Empty;
                            if (isSms)
                            {
                                var smsMessage = string.Empty;
                                EmailTemplate smsTemplate = new EmailTemplate();
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateApprovalSMS);
                                }
                                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateRejectSMS);
                                }
                                if (smsTemplate != null)
                                {
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber) && dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                    {
                                        smsPlainTemplateCreatedBy = smsTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, dealer.Name)
                                            .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                            .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplateCreatedBy);
                                        amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);

                                        smsPlainTemplateDealer = smsTemplate.PlainTemplate.Replace(Constants.UserName, dealer.Name).Replace(Constants.CustomerName, dealer.Name)
                                            .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                            .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplateDealer);
                                        amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                    }
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber) && dealer == null)
                                    {
                                        smsPlainTemplateCreatedBy = smsTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, createdBy.Name)
                                            .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                            .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplateCreatedBy);
                                        amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                    }

                                }
                            }

                            if (inputDto.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                            {
                                var reportingToContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.RequestedTo);
                                if (_resultService.IsPushNotification())
                                {
                                    if (reportingToContext != null && reportingToContext.RegistrationTypeId != null && reportingToContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(reportingToContext.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = reportingToContext.PushTokenKey,
                                            RegistrationTypeId = reportingToContext.RegistrationTypeId != null ? (int)reportingToContext.RegistrationTypeId : 0,
                                            Title = Constants.SpecialRateRequestForApproval,
                                            Message = Constants.SpecialRateRequestCreationNotification.Replace(Constants.SkuName, result.Sku.SkuName).Replace(Constants.Quantity, Math.Round(result.Quantity, 2).ToString()).Replace(Constants.Price, Math.Round(result.SpecialPrice, 2).ToString()),
                                            //Id = result.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }

                                }
                            }

                            if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                            {
                                if (_resultService.IsPushNotification())
                                {
                                    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = createdBy.PushTokenKey,
                                            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                            Title = Constants.SpecialRateRejectSubject,
                                            Message = Constants.SpecialRateRejectNotification.Replace(Constants.SkuName, result.Sku.SkuName).Replace(Constants.Quantity, Math.Round(result.Quantity, 2).ToString()).Replace(Constants.Price, Math.Round(result.SpecialPrice, 2).ToString()),
                                            //Id = result.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = dealer.PushTokenKey,
                                            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                            Title = Constants.SpecialRateRejectSubject,
                                            Message = Constants.SpecialRateRejectNotification.Replace(Constants.SkuName, result.Sku.SkuName).Replace(Constants.Quantity, Math.Round(result.Quantity, 2).ToString()).Replace(Constants.Price, Math.Round(result.SpecialPrice, 2).ToString()),
                                            //Id = result.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }

                                }
                            }

                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                if (_resultService.IsPushNotification())
                                {
                                    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = createdBy.PushTokenKey,
                                            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                            Title = Constants.SpecialRateApprovalSubject,
                                            Message = Constants.SpecialRateApproveNotification.Replace(Constants.SkuName, result.Sku.SkuName).Replace(Constants.Quantity, Math.Round(result.Quantity, 2).ToString()).Replace(Constants.Price, Math.Round(result.SpecialPrice, 2).ToString()),
                                            //Id = result.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = dealer.PushTokenKey,
                                            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                            Title = Constants.SpecialRateApprovalSubject,
                                            Message = Constants.SpecialRateApproveNotification.Replace(Constants.SkuName, result.Sku.SkuName).Replace(Constants.Quantity, Math.Round(result.Quantity, 2).ToString()).Replace(Constants.Price, Math.Round(result.SpecialPrice, 2).ToString()),
                                            //Id = result.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                }
                            }

                            if (_resultService.IsPushNotification())
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SpecialRateStatusAlreadyUpdated);
                }

                return _resultService.SuccessObject(1);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialRateApprovals(SpecialRateAddInputDto inputDto)
        {
            var specialRateApprovalList = new List<SpecialRateApprovalOutputDto>();
            _methodName = "GetSpecialRateApprovalList";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var specialRateApprovalListContext = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => (_.RequestedTo == inputDto.LoginUserId || _.RequestedBy == inputDto.LoginUserId)
                    && _.SpecialRate != null && ((inputDto.VerticalId > 0 && _.SpecialRate.OilType != null) ? _.SpecialRate.OilType.DivisionId == inputDto.VerticalId : _.SpecialRate.OilType.DivisionId > 0))
                    .GroupBy(_ => _.SpecialRateId).Select(group =>
                          new
                          {
                              SpecialRateId = group.Key,
                              SpecialRateApprovals = group.OrderByDescending(_ => _.Id)
                          })
                    .Select(_ => _.SpecialRateApprovals.FirstOrDefault());
                if (specialRateApprovalListContext != null && specialRateApprovalListContext.Any())
                {
                    specialRateApprovalList = specialRateApprovalListContext.ToList().Select(_ => new SpecialRateApprovalOutputDto()
                    {
                        Id = _.Id,
                        HasAccessToProceed = _.RequestedTo == inputDto.LoginUserId ? true : false,
                        RequestedTo = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == _.RequestedTo)?.Name,
                        RequestedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == _.RequestedBy)?.Name,
                        //ApprovedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == _.ApprovedBy)?.Name,
                        FinalPrice = _.SpecialRate.FinalPrice,
                        OilTypeId = _.SpecialRate.OilTypeId,
                        Quantity = _.SpecialRate.Quantity,
                        QuantityCase = _.SpecialRate.QuantityCase,
                        SpecialPrice = _.SpecialRate.SpecialPrice,
                        SkuName = _.SpecialRate.Sku?.SkuName,
                        StatusId = (int)_.SpecialRate.StatusId,
                        DealerName = _.SpecialRate.User.Name,
                        Remarks = _.SpecialRate.Remarks,
                        //FreightRoute = _.SpecialRate.FreightRoute != null ? _.SpecialRate.FreightRoute.Name : string.Empty,
                        IncoTerms = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(f => f.Id == _.SpecialRate.Incoterms2)?.Name,
                        OilTypeName = _.SpecialRate.OilType?.Name,
                        Status = _.SpecialRate.Status?.Name,
                        IsLTD = _.SpecialRate.IsLTD
                    }).ToList();
                }

                return _resultService.SuccessObject(specialRateApprovalList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialRateApprovalDetail(IdInputDto inputDto)
        {
            var specialRateApproval = new SpecialRateApprovalOutputDto();
            _methodName = "GetSpecialRateApprovalDetail";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var specialRateApprovalContext = _emamiContext.SpecialRateApproval.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id && _.SpecialRate != null);

                if (specialRateApprovalContext != null)
                {
                    specialRateApproval = new SpecialRateApprovalOutputDto()
                    {
                        Id = specialRateApprovalContext.Id,
                        HasAccessToProceed = specialRateApprovalContext.RequestedTo == inputDto.LoginUserId ? true : false,
                        RequestedTo = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == specialRateApprovalContext.RequestedTo)?.Name,
                        RequestedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == specialRateApprovalContext.RequestedBy)?.Name,
                        //ApprovedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == _.ApprovedBy)?.Name,
                        FinalPrice = specialRateApprovalContext.SpecialRate.FinalPrice,
                        OilTypeId = specialRateApprovalContext.SpecialRate.OilTypeId,
                        Quantity = specialRateApprovalContext.SpecialRate.Quantity,
                        QuantityCase = specialRateApprovalContext.SpecialRate.QuantityCase,
                        SpecialPrice = specialRateApprovalContext.SpecialRate.SpecialPrice,
                        SkuName = specialRateApprovalContext.SpecialRate.Sku?.SkuName,
                        StatusId = (int)specialRateApprovalContext.SpecialRate.StatusId,
                        DealerName = specialRateApprovalContext.SpecialRate.User.Name,
                        Remarks = specialRateApprovalContext.SpecialRate.Remarks,
                        //FreightRoute = specialRateApprovalContext.SpecialRate.FreightRoute != null ? specialRateApprovalContext.SpecialRate.FreightRoute.Name : string.Empty,
                        IncoTerms = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(f => f.Id == specialRateApprovalContext.SpecialRate.Incoterms2)?.Name,
                        OilTypeName = specialRateApprovalContext.SpecialRate.OilType?.Name,
                        Status = specialRateApprovalContext.SpecialRate.Status?.Name,
                    };
                }


                return _resultService.SuccessObject(specialRateApproval);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialRateRequestList(SpecialRateInputDto inputDto)
        {
            var specialRateListDto = new List<SpecialRateOutputDto>();
            _methodName = "GetSpecialRateRequestList";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var specialRateApproval = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.RequestedTo == inputDto.LoginUserId || _.CreatedBy == inputDto.LoginUserId);
                List<long> specialRateIds = specialRateApproval.Select(_ => _.SpecialRateId).Distinct().ToList();

                IQueryable<SpecialRate> specialRateListContext = null;
                if (inputDto.DealerId != null && inputDto.OilTypeId != null && inputDto.FromDate.HasValue && inputDto.ToDate.HasValue)
                {
                    specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId
                            && _.OilTypeId == inputDto.OilTypeId && _.CreatedDate >= inputDto.FromDate && _.CreatedDate <= inputDto.ToDate && specialRateIds.Contains(_.Id));
                }
                //else if ((inputDto.DealerId != 0 && inputDto.DealerId != null) || (inputDto.OilTypeId != 0 && inputDto.OilTypeId != null)
                //    || (inputDto.FromDate.HasValue && inputDto.FromDate != DateTime.MinValue) || (inputDto.ToDate.HasValue && inputDto.ToDate != DateTime.MinValue))
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
                else
                {
                    List<long> bdoList = new List<long>();
                    if (inputDto.BDOId > 0)
                    {
                        bdoList.Add(inputDto.BDOId);
                    }
                    else
                    {
                        bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    }
                    if (bdoList != null && bdoList.Any())
                    {
                        List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                        if (dealersList != null && dealersList.Any())
                        {
                            if (dealersList != null && dealersList.Any())
                            {
                                specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && DbFunctions.TruncateTime(_.CreatedDate) >= inputDto.FromDate && DbFunctions.TruncateTime(_.CreatedDate) <= inputDto.ToDate && specialRateIds.Contains(_.Id));
                            }
                        }
                    }
                }
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null)
                        .OrderByDescending(o => o.sr.CreatedDate).ToList();
                    foreach (var specialRateContext in specialRateList)
                    {
                        var specialRateOutputDto = new SpecialRateOutputDto();
                        specialRateOutputDto.SpecialRateId = specialRateContext.sr.Id;
                        specialRateOutputDto.DealerId = specialRateContext.sr.UserId;
                        specialRateOutputDto.DealerName = specialRateContext.sr.User != null ? specialRateContext.sr.User.Name : string.Empty;
                        specialRateOutputDto.RequestDate = specialRateContext.sr.CreatedDate;
                        specialRateOutputDto.StatusId = specialRateContext.sr.StatusId;
                        specialRateOutputDto.StatusName = specialRateContext.sr.Status != null ? specialRateContext.sr.Status.Name : string.Empty;
                        specialRateOutputDto.IsBroker = specialRateContext.ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false;
                        specialRateOutputDto.IsLTD = specialRateContext.sr.IsLTD;
                        var specialRateOilTypeListDto = new List<SpecialRateOilTypeDto>();

                        var specialRateOilTypeDto = new SpecialRateOilTypeDto();
                        specialRateOilTypeDto.OilTypeId = specialRateContext.sr.OilTypeId;
                        specialRateOilTypeDto.OilTypeName = specialRateContext.sr.OilType != null ? specialRateContext.sr.OilType.Name : string.Empty;
                        specialRateOilTypeDto.SkuCount = 1;
                        specialRateOilTypeDto.SkuId = specialRateContext.sr.SkuId;
                        specialRateOilTypeDto.SkuName = specialRateContext.sr.Sku != null ? specialRateContext.sr.Sku.SkuCode + "-" + specialRateContext.sr.Sku.SkuName : string.Empty;

                        specialRateOilTypeListDto.Add(specialRateOilTypeDto);
                        specialRateOutputDto.OilTypeList = specialRateOilTypeListDto;
                        specialRateListDto.Add(specialRateOutputDto);
                    }
                }
                if (specialRateListDto != null && specialRateListDto.Any())
                {
                    return _resultService.SuccessObject(specialRateListDto);
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

        public ResultDto GetSpecialRateRequestDetails(SpecialRateDetailInputDto specialRateDetailInputDto)
        {
            _methodName = "GetSpecialRateRequestDetails";
            try
            {
                var specialRateDetailsDto = new SpecialRateDetailsDto();
                if (specialRateDetailInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (specialRateDetailInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateDetailInputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (specialRateDetailInputDto.SpecialRateId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SpecialRateRequestIdMissing);
                }

                var specialRateContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.Id == specialRateDetailInputDto.SpecialRateId);

                if (specialRateContext != null && specialRateContext.Any())
                {
                    var cityContext = _emamiContext.City.AsNoTracking();
                    var stateContext = _emamiContext.State.AsNoTracking();

                    SpecialRate specialRateDetailContext = specialRateContext.FirstOrDefault();
                    specialRateDetailsDto.DealerId = specialRateDetailContext.UserId;
                    specialRateDetailsDto.DealerName = string.Concat((specialRateDetailContext.User != null ? specialRateDetailContext.User.Name : string.Empty) + "-" + (cityContext.FirstOrDefault(c => c.Id == specialRateDetailContext.User.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == specialRateDetailContext.User.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == specialRateDetailContext.User.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == specialRateDetailContext.User.StateId).StateName : string.Empty) + "-" + (specialRateDetailContext.User != null ? specialRateDetailContext.User.Code : string.Empty));
                    specialRateDetailsDto.RequestDate = specialRateDetailContext.CreatedDate;
                    specialRateDetailsDto.Remarks = specialRateDetailContext.Remarks;
                    var specialRateApprovalContext = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.SpecialRateId == specialRateDetailInputDto.SpecialRateId && _.SpecialRate != null).OrderByDescending(_ => _.Id).FirstOrDefault();
                    specialRateDetailsDto.IsAccessToApprove = (specialRateApprovalContext.RequestedTo == specialRateDetailInputDto.LoginUserId) ? true : false;
                    var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateDetailContext.StatusId);
                    if (statusContext != null)
                    {
                        specialRateDetailsDto.Status = statusContext.Name;
                        specialRateDetailsDto.StatusId = statusContext.Id;
                    }
                    foreach (var _ in specialRateContext.ToList())
                    {
                        decimal SkuConversionFactor = 0;
                        decimal ProfitMarginperCase = 0;
                        DateTime currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        var skuUomMapping = _emamiContext.SkuUomMapping.Where(s => s.SkuId == _.SkuId).ToList();
                        if (skuUomMapping != null && skuUomMapping.Any())
                        {
                            var skuUomMapping3 = skuUomMapping.FirstOrDefault(su => su.UomId == (int)DTO.Enums.Uom.MT && su.RelationUomId == (int)DTO.Enums.Uom.Nos);
                            SkuConversionFactor = skuUomMapping3.ConversionFactor;
                        }
                        var DealerContext = _emamiContext.Users.FirstOrDefault(u => u.Id == _.UserId);
                        var ProfitMarginContext = _emamiContext.ProfitMargins.AsNoTracking().FirstOrDefault(s => s.SkuId == _.SkuId && s.ZoneId == DealerContext.ZoneId && s.StateId == DealerContext.StateId
                        && s.IsActive
                        //&& s.DistrictId == DealerContext.DistrictId && s.CityId == DealerContext.CityId
                        && (DbFunctions.TruncateTime(s.ValidFrom) <= DbFunctions.TruncateTime(currentDate) && DbFunctions.TruncateTime(s.ValidTo) >= DbFunctions.TruncateTime(currentDate)));
                        if (ProfitMarginContext != null)
                        {
                            ProfitMarginperCase = (ProfitMarginContext.RatePerMt / SkuConversionFactor);
                        }

                        var outputdto = new SkuShortViewOutputDto()
                        {
                            SpecialRateId = _.Id,
                            SkuId = _.SkuId,
                            SkuName = _.Sku != null ? _.Sku.SkuName + "-" + _.Sku.SkuCode : string.Empty,
                            Quantity = _.Quantity,
                            QuantityCase = _.QuantityCase,
                            FinalPrice = _.FinalPrice,
                            SpecialPrice = _.SpecialPrice,
                            IncotermsName = _.Incoterms1,
                            IsRake = (_.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || _.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? true : false,
                            //DealerLocationName = _.FreightRoute != null ? _.FreightRoute.Name : string.Empty,
                            PlantName = _.Depot != null ? _.Depot.Name : string.Empty,
                            CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, _.SkuId),
                            IsLTD = _.IsLTD,
                            ImpactOnMarginCase = ProfitMarginperCase - (_.FinalPrice - _.SpecialPrice),
                            ImpactOnMarginMT = (ProfitMarginperCase - (_.FinalPrice - _.SpecialPrice)) * SkuConversionFactor
                        };
                        specialRateDetailsDto.SkuList.Add(outputdto);
                    }
                }

                if (specialRateDetailsDto != null && specialRateDetailsDto.SkuList != null && specialRateDetailsDto.SkuList.Any())
                {
                    return _resultService.SuccessObject(specialRateDetailsDto);
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

        public ResultDto SaudaCreationFromSpecialRate(SpecialRateSaudaDto inputDto)
        {
            _methodName = "SaudaCreationFromSpecialRate";
            var resultDto = new ResultDto();
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

                if (inputDto.SalesOrganizationId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SalesOrganisationIsEmpty);
                }
                if (inputDto.DistributionChannelId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DistributionChannelIsEmpty);
                }
                if (inputDto.DivisionId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DivisionIsEmpty);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                //if (userRoleContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}

                var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.DealerId);

                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                   .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                   && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                   && _.DivisionId == inputDto.DivisionId);

                if (inputDto.SpecialRateIdInfo == null || !inputDto.SpecialRateIdInfo.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var SplrateIdInfo = inputDto.SpecialRateIdInfo.ToList();
                var SpecialRatelist = _emamiContext.SpecialRate.Where(_ => _.StatusId == (int)DTO.Enums.Status.Approved).ToList();
                var specialRateListContext = SpecialRatelist
                                             .Join(SplrateIdInfo, sr => sr.Id, srId => srId.SpecialRateIds, (sr, srId) => new { sr, srId })
                                            .ToList();

                if (specialRateListContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                decimal overallSaudaLimit = 0;
                decimal orderedQuantity = 0;
                decimal liftingQuantity = 0;
                decimal availableQuantity = 0;

                overallSaudaLimit = userdivContext.SaudaLimit ?? 0;

                //IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                //    && (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)DTO.Enums.Status.Approved ));
                //IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                //    && (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)DTO.Enums.Status.Approved || _.StatusId == (int)DTO.Enums.Status.Completed));

                IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                    && _.SaudaNumber == null && _.StatusId == (int)DTO.Enums.Status.Pending);

                var QuantityLimitForBookingSaudaName = Utility.GetEnumDescription(DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == QuantityLimitForBookingSaudaName);
                bool IsQuantityLimitForBookingSauda = Convert.ToBoolean(configurationContext.Value);

                if (saudaOrderListContext != null && saudaOrderListContext.Any())
                {
                    var overallSaudaStatuses = Constants.OverallSaudaStatus;
                    foreach (var item in specialRateListContext)
                    {
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.sr.SkuId);
                        if (skuContext != null && (skuContext.DivisionId == (int)DTO.Enums.Division.SpecialityFat || skuContext.DivisionId == (int)DTO.Enums.Division.Hbc))
                        {
                            //bool geoErrorFlag = false;
                            //bool bdoErrorFlag = false;
                            //decimal availableQuantityGeo = 0;
                            decimal availableQuantityBdo = 0;
                            //var geographicalLimitContext = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking().FirstOrDefault(_ => _.SkuId == item.SkuId && _.CityId == dealerContext.CityId
                            //&& DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate) && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                            //if (geographicalLimitContext != null)
                            //{
                            //    IQueryable<SaudaOrder> saudaOrdersGeoContext = null;
                            //    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                            //        .Where(_ => _.u.CityId == dealerContext.CityId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.u.Id).ToList();
                            //    if (dealerList != null && dealerList.Any())
                            //    {
                            //        saudaOrdersGeoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
                            //              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(geographicalLimitContext.ValidFrom) &&
                            //              DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(geographicalLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId));
                            //    }
                            //    decimal requestedQuantityGeo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                            //    decimal orderedQuantityGeo = 0;
                            //    decimal totalQuantityGeo = requestedQuantityGeo;
                            //    if (saudaOrdersGeoContext != null && saudaOrdersGeoContext.Any())
                            //    {
                            //        orderedQuantityGeo = saudaOrdersGeoContext.Sum(_ => _.BidQuantity);
                            //        totalQuantityGeo = requestedQuantityGeo + orderedQuantityGeo;
                            //    }
                            //    if (totalQuantityGeo > geographicalLimitContext.ActualDiscount)
                            //    {
                            //        geoErrorFlag = true;
                            //        //return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                            //        availableQuantityGeo = geographicalLimitContext.ActualDiscount - orderedQuantityGeo;
                            //        if (availableQuantityGeo < 0)
                            //        {
                            //            availableQuantityGeo = 0;
                            //        }
                            //        //else
                            //        //{
                            //        //    return _resultService.ErrorMessage(Constants.SkuGeographicalLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
                            //        //}
                            //    }
                            //}
                            if (configurationContext != null && IsQuantityLimitForBookingSauda)
                            {
                                long bdoId = 0;
                                var bdoContext = _emamiContext.UserCustomerMapping.AsNoTracking().FirstOrDefault(_ => _.CustomerId == inputDto.DealerId);
                                if (bdoContext != null)
                                {
                                    bdoId = bdoContext.UserId;
                                }
                                var bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == bdoId && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                               && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                if (bdoLimitContext != null)
                                {
                                    IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
                                    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == bdoId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    if (dealerList != null && dealerList.Any())
                                    {
                                        saudaOrdersBdoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.sr.SkuId && dealerList.Contains(_.Sauda.UserId)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda);
                                    }
                                    decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.srId.QuantityInCases, item.sr.SkuId);
                                    decimal orderedQuantityBdo = 0;
                                    decimal totalQuantityBdo = requestedQuantityBdo;
                                    if (saudaOrdersBdoContext != null && saudaOrdersBdoContext.Any())
                                    {
                                        orderedQuantityBdo = saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
                                        totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                    }
                                    if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
                                    {
                                        //bdoErrorFlag = true;
                                        //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                        if (availableQuantityBdo < 0)
                                        {
                                            availableQuantityBdo = 0;
                                            //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        }
                                        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                                        //if (availableQuantityBdo >= 0)
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
                                        //}
                                        //else
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
                                        //}
                                    }
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
                                }
                            }

                            //if (geographicalLimitContext != null && bdoLimitContext != null && geoErrorFlag && bdoErrorFlag)
                            //{
                            //    return _resultService.ErrorMessage(Constants.SkuGeographicalBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.GeoLimitQuantity, Math.Round(availableQuantityGeo, 2).ToString())
                            //        .Replace(Constants.BdoLimitQuantity, Math.Round(availableQuantityBdo, 2).ToString()));
                            //}
                            //else if (((geographicalLimitContext != null) != (bdoLimitContext != null)) && (geoErrorFlag || bdoErrorFlag))
                            //{
                            //    if (geographicalLimitContext != null)
                            //    {
                            //        return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityGeo, 2).ToString()));
                            //    }
                            //    else
                            //    {
                            //        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                            //    }
                            //}
                        }
                    }
                    //orderedQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                    //IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => saudaOrderListContext.Any(a => a.Id == _.SaudaOrderId));
                    //if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
                    //{
                    //    liftingQuantity = liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                    //}
                    //availableQuantity = overallSaudaLimit - (orderedQuantity - liftingQuantity);

                    //decimal invoiceQuantity = 0;
                    var existingSaudaQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                    //var skuIds = saudaOrderListContext.Select(_ => _.SkuId).Distinct().ToList();
                    //var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                    //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                    //                      where inv.UserId == dealerContext.Id
                    //                      && skuIds.Contains(invDet.SkuId)
                    //                      select invDet
                    //                          ).ToList();

                    //if (invoiceContext != null && invoiceContext.Any())
                    //{
                    //    invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
                    //}
                    //availableQuantity = overallSaudaLimit + invoiceQuantity - existingSaudaQuantity;

                    var saudaLimitTableValue = _emamiContext.SaudaLimit.FirstOrDefault(_ => _.UserId == dealerContext.Id);
                    var saudaLimitTableValueTotal = saudaLimitTableValue != null ? (saudaLimitTableValue.PendingContract + saudaLimitTableValue.PendingDO + saudaLimitTableValue.PendingOBD) : 0;
                    availableQuantity = overallSaudaLimit - saudaLimitTableValueTotal - existingSaudaQuantity;
                    if (availableQuantity < specialRateListContext.Sum(_ => _resultService.ConvertCasetoMetricTon(_.srId.QuantityInCases, _.sr.SkuId)))
                    {
                        return _resultService.ErrorMessage(Constants.SaudaLimitExceeds);
                    }
                }

                long BrokerId = 0;
                var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
                if (dealerRole != null)
                {
                    if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
                    {
                        BrokerId = inputDto.DealerId;
                    }
                    else
                    {
                        var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
                                             join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
                                             where ur.RoleId == (int)DTO.Enums.Role.Broker
                                             && ucm.CustomerId == inputDto.DealerId
                                             select new
                                             {
                                                 BrokerId = ucm.UserId
                                             }).FirstOrDefault();

                        if (BrokerContext != null)
                        {
                            BrokerId = BrokerContext.BrokerId;
                        }
                    }
                }

                var saudaContext = new Sauda();
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    saudaContext = new Sauda
                    {

                        BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        UserId = inputDto.DealerId,

                        SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,

                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        IsSAPDataSync = false,
                        IsSAPDataSyncApproval = false

                    };

                    _emamiContext.Sauda.Add(saudaContext);
                    _emamiContext.SaveChanges();

                    int i = 0;
                    List<long> saudaOrderIds = new List<long>();
                    foreach (var item in specialRateListContext)
                    {
                        DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        long? depotIdForRake = 0;
                        if (item.sr.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || item.sr.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake)
                        {
                            depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.sr.DepotId && !_.IsPlant)?.DepotId;
                            if (item.srId.SaudaValidFromDate != null)
                                saudaValidFromDate = item.srId.SaudaValidFromDate;

                        }

                        #region PricingLive to Pricing DataInsert and Rearrange the Pricing Data
                        ///Pricing Live is contain Current day Pricing
                        ///So, we insert the Pricing Live data into Pricing table for Sauda booked records only
                        /// Daily we cleanup and fresh data insert into the pricing live table
                        var pricingLiveContext = _emamiContext.TodayPricing.FirstOrDefault(_ => _.Id == item.sr.PricingId);
                        //var pricingContext = default(Pricing);
                        long pricingId = 0;
                        if (pricingLiveContext == null)
                        {
                            return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
                        }
                        if (pricingLiveContext.PricingReferneceId == 0)
                        {
                            var pricing = new Pricing()
                            {
                                SkuId = pricingLiveContext.SkuId,
                                OilTypeId = pricingLiveContext.OilTypeId,
                                OilPackingTypeId = pricingLiveContext.OilPackingTypeId,
                                PlantId = pricingLiveContext.PlantId,
                                Price = pricingLiveContext.Price,
                                SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
                                DistributionChannelId = pricingLiveContext.DistributionChannelId,
                                DivisionId = pricingLiveContext.DivisionId,
                                SAPPricingCode = pricingLiveContext.SAPPricingCode,
                                ValidFrom = pricingLiveContext.ValidFrom,
                                ValidTo = pricingLiveContext.ValidTo,
                                CreatedBy = pricingLiveContext.CreatedBy,
                                CreatedDate = pricingLiveContext.CreatedDate,
                                ModifiedBy = pricingLiveContext.ModifiedBy,
                                ModifiedDate = pricingLiveContext.ModifiedDate,
                            };
                            _emamiContext.Pricing.Add(pricing);
                            _emamiContext.SaveChanges();
                            pricingId = pricing.Id;
                            /// Update pricingLive Record Pricing Reference Id
                            //var pricingLiveRecord = _emamiContext.TodayPricing.FirstOrDefault(s => s.Id == pricingLiveContext.Id);
                            pricingLiveContext.PricingReferneceId = pricing.Id;
                            _emamiContext.SaveChanges();
                            //pricingContext = pricing;
                        }
                        else
                        {
                            pricingId = pricingLiveContext.PricingReferneceId;
                            //pricingContext = _emamiContext.Pricing.FirstOrDefault(s => s.Id == pricingLiveContext.PricingReferneceId);
                        }
                        item.sr.PricingId = pricingId;
                        #endregion

                        i = i + 10;
                        var saudaOrder = new SaudaOrder
                        {
                            SaudaId = saudaContext.Id,
                            SaudaNumber = i.ToString(),
                            SkuId = item.sr.SkuId,
                            OilTypeId = item.sr.OilTypeId,
                            BidPrice = (item.sr.SpecialPrice * item.srId.QuantityInCases),
                            BidQuantity = _resultService.ConvertCasetoMetricTon(item.srId.QuantityInCases, item.sr.SkuId),
                            BidQuantityCase = item.srId.QuantityInCases,
                            QuotedPrice = (item.sr.FinalPrice * item.sr.QuantityCase),
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            PricingId = item.sr.PricingId,
                            //DealerTypeId = (int)DTO.Enums.DealerType.Direct,
                            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                            Incoterms1 = item.sr.Incoterms1,
                            PlantId = item.sr.DepotId,
                            //DealerLocationId = item.sr.FreightRouteId,
                            //CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = (int)DTO.Enums.Status.Pending,
                            // SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,

                            Incoterms2 = item.sr.Incoterms2,
                            BrokerId = BrokerId,
                            SpecialRateRequestId = item.sr.Id,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            // DepotIdForRake = depotIdForRake.Value,
                            IsQuantityLimitForBookingSauda = IsQuantityLimitForBookingSauda,
                            QuotedPriceBeforeSAPDiscount = item.sr.SpecialPrice
                        };
                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();

                        //if (dealerContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose)
                        //{
                        //    saudaOrderIds.Add(saudaOrder.Id);
                        //}

                        try
                        {
                            var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId || _.Id == inputDto.DealerId);
                            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.Id);
                            if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                var createdBy = usersContext.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                                var dealer = usersContext.FirstOrDefault(_ => _.Id == inputDto.DealerId);
                                string dealerName = string.Empty;
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    dealerName = dealer.Name;
                                    toUsers.Add(dealer.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var emailSubject = Constants.SaudaBookedSubject;
                                    var plainText = string.Empty;
                                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationEmail);
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }

                                }
                                var smsPlainTemplate = string.Empty;
                                if (_resultService.IsSMS())
                                {
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString());
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                    }

                                }
                                //if (_resultService.IsPushNotification())
                                //{
                                //    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                //    {
                                //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                //        {
                                //            PushTokenKey = createdBy.PushTokenKey,
                                //            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                //            Title = Constants.SaudaCreationSubject,
                                //            Message = smsPlainTemplate,
                                //            //Id = saudaOrderContext.Id,
                                //        };
                                //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                //    }
                                //    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                //    {
                                //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                //        {
                                //            PushTokenKey = dealer.PushTokenKey,
                                //            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                //            Title = Constants.SaudaCreationSubject,
                                //            Message = smsPlainTemplate,
                                //            //Id = saudaOrderContext.Id,
                                //        };
                                //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                //    }
                                //}
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    specialRateListContext.ForEach(_ => _.sr.StatusId = (int)DTO.Enums.Status.Completed);
                    _emamiContext.SaveChanges();

                    //if (dealerContext.VerticalId == (int)DTO.Enums.LooseVertical.Loose)
                    //{
                    //    //method to sync Loose sauda from APP to SAP 
                    //    _sapIntegrationService.GetSaudaDetailsForLooseVertical(saudaOrderIds);
                    //}
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SpecialRateNotFoundWithApproval);
                }



                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaContext.Id;
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

        public ResultDto GetSpecialRateRequestNewList(SpecialRateInputDto inputDto)
        {
            var specialRateListDto = new List<SpecialRateOutputDto>();
            _methodName = "GetSpecialRateRequestNewList";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (roleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (roleContext.RoleId == (int)DTO.Enums.Role.Admin)
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


                //var specialRateApprovalContext = _emamiContext.SpecialRateApproval.AsNoTracking();

                var specialRateApprovalContext = (from sra in _emamiContext.SpecialRateApproval.AsNoTracking()
                                                  join sr in _emamiContext.SpecialRate.AsNoTracking() on sra.SpecialRateId equals sr.Id
                                                  join ud in divisionslogieduser on new { SalesOrganizationId = sr.SalesOrganizationId, DistributionChannelId = sr.DistributionChannelId, DivisionId = sr.DivisionId }
                                            equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                  select sra
                                                  );

                var specialRatesContext = (from sr in _emamiContext.SpecialRate.AsNoTracking()
                                           join ud in divisionslogieduser on new { SalesOrganizationId = sr.SalesOrganizationId, DistributionChannelId = sr.DistributionChannelId, DivisionId = sr.DivisionId }
                                            equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                           where DbFunctions.TruncateTime(sr.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                           && DbFunctions.TruncateTime(sr.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                           && sr.StatusId != (int)DTO.Enums.Status.Approved
                                           select sr
                                            );

                //var specialRatesContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.StatusId != (int)DTO.Enums.Status.Approved);

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                }

                // var specialRateApproval = specialRateApprovalContext.Where(_ => _.RequestedTo == inputDto.LoginUserId /*|| _.CreatedBy == inputDto.LoginUserId*/);
                List<long> specialRateIds = specialRateApprovalContext.Where(_ => bdoList.Contains(_.CreatedBy)).Select(_ => _.SpecialRateId).Distinct().ToList();
                List<long> specialRateIdsCreatedByLoginUser = specialRateApprovalContext.Where(_ => _.CreatedBy == inputDto.LoginUserId).Select(_ => _.SpecialRateId).Distinct().ToList();

                var key = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.DiscountAmountforSpecialRateApproval));
                var discountAmountForSpecialRateApproval = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == key)?.Value ?? "0";
                var amountInDecimal = Convert.ToDecimal(discountAmountForSpecialRateApproval);

                List<SpecialRate> specialRateListContext = null;
                if (inputDto.DealerId != null && inputDto.OilTypeId != null && inputDto.FromDate.HasValue && inputDto.ToDate.HasValue)
                {

                    specialRateListContext = specialRatesContext.Where(_ => _.UserId == inputDto.DealerId
                            && _.OilTypeId == inputDto.OilTypeId && specialRateIds.Contains(_.Id)).ToList();
                    specialRateListContext.AddRange(specialRatesContext.Where(_ => _.UserId == inputDto.DealerId
                            && _.OilTypeId == inputDto.OilTypeId && specialRateIdsCreatedByLoginUser.Contains(_.Id)));
                }
                //else if ((inputDto.DealerId != 0 && inputDto.DealerId != null) || (inputDto.OilTypeId != 0 && inputDto.OilTypeId != null)
                //    || (inputDto.FromDate.HasValue && inputDto.FromDate != DateTime.MinValue) || (inputDto.ToDate.HasValue && inputDto.ToDate != DateTime.MinValue))
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
                else
                {

                    if (bdoList != null && bdoList.Any())
                    {
                        List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                        if (dealersList != null && dealersList.Any())
                        {
                            if (dealersList != null && dealersList.Any())
                            {
                                specialRateListContext = specialRatesContext.Where(_ => dealersList.Contains(_.UserId) && specialRateIds.Contains(_.Id)).ToList();
                                specialRateListContext.AddRange(specialRatesContext.Where(_ => dealersList.Contains(_.UserId) && specialRateIdsCreatedByLoginUser.Contains(_.Id)));
                            }
                        }
                    }
                }
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null)
                        .OrderByDescending(o => o.sr.CreatedDate).ToList();

                    var cityContext = _emamiContext.City.AsNoTracking();
                    var stateContext = _emamiContext.State.AsNoTracking();

                    foreach (var specialRateContext in specialRateList)
                    {
                        var specialRateOutputDto = new SpecialRateOutputDto();
                        specialRateOutputDto.SpecialRateId = specialRateContext.sr.Id;
                        specialRateOutputDto.DealerId = specialRateContext.sr.UserId;
                        specialRateOutputDto.DealerName = string.Concat((specialRateContext.sr.User != null ? specialRateContext.sr.User.Name : string.Empty) + "-" + (cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName : string.Empty) + "-" + (specialRateContext.sr.User != null ? specialRateContext.sr.User.Code : string.Empty));
                        //specialRateOutputDto.DealerCode = cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName : string.Empty + "-" + stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName : string.Empty + "-" + specialRateContext.sr.User != null ? specialRateContext.sr.User.Code : string.Empty;
                        specialRateOutputDto.RequestDate = specialRateContext.sr.CreatedDate;
                        specialRateOutputDto.StatusId = (((specialRateContext.sr.FinalPrice - specialRateContext.sr.SpecialPrice) > amountInDecimal) && specialRateContext.sr.StatusId == (int)DTO.Enums.Status.Pending && specialRateContext.sr.CreatedBy != inputDto.LoginUserId) ? (int)DTO.Enums.Status.WaitingForRequestApproval : specialRateContext.sr.StatusId;
                        specialRateOutputDto.StatusName = (((specialRateContext.sr.FinalPrice - specialRateContext.sr.SpecialPrice) > amountInDecimal) && specialRateContext.sr.StatusId == (int)DTO.Enums.Status.Pending && specialRateContext.sr.CreatedBy != inputDto.LoginUserId) ? UtilityHelper.GetEnumDescription(DTO.Enums.Status.WaitingForRequestApproval) : (specialRateContext.sr.Status != null ? specialRateContext.sr.Status.Name : string.Empty);
                        specialRateOutputDto.IsBroker = specialRateContext.ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false;
                        specialRateOutputDto.IsLTD = specialRateContext.sr.IsLTD;
                        specialRateOutputDto.SkuId = specialRateContext.sr.SkuId;
                        specialRateOutputDto.SkuName = specialRateContext.sr.Sku != null ? specialRateContext.sr.Sku.SkuName : string.Empty;
                        specialRateOutputDto.SpecialPrice = specialRateContext.sr.SpecialPrice;
                        specialRateOutputDto.Quantity = specialRateContext.sr.QuantityCase;
                        specialRateOutputDto.DiscountOrPremium = specialRateContext.sr.FinalPrice - specialRateContext.sr.SpecialPrice;
                        specialRateOutputDto.CreatedBy = specialRateContext.sr.CreatedBy;
                        specialRateOutputDto.DiscountAmountInConfiguration = amountInDecimal;
                        specialRateListDto.Add(specialRateOutputDto);
                    }
                }
                if (specialRateListDto != null && specialRateListDto.Any())
                {
                    specialRateListDto = specialRateListDto.Where(_ => _.StatusId == inputDto.StatusId).ToList();
                    return _resultService.SuccessObject(specialRateListDto);
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

        #region Permanent Coverage Plan

        public ResultDto GetPendingPermanentJourneyPlanList(LoginUserIdDto inputDto)
        {
            _methodName = "GetPendingPermanentJourneyPlanList";
            var resultDto = new ResultDto();
            var permanentJourneyPlans = new List<PermanentJourneyPlansDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var pcpListContext = _emamiContext.PermanentJourneyPlans.AsNoTracking().Where(_ => _.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending)
                    .Join(_emamiContext.PJPApprovalInformation.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && _.StatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending)
                        , pjp => pjp.Id, pjpai => pjpai.PermanentJourneyPlanId, (pjp, pjpai) => new { pjp, CreatedBy = pjpai.CreatedBy })
                    .Join(_emamiContext.FinancialYears.AsNoTracking(), x => x.pjp.FinancialYearId, fy => fy.Id, (x, fy) => new { x.pjp, x.CreatedBy, FinalYear = fy.Year })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.CreatedBy, u => u.Id, (x, u) => new { x.pjp, x.CreatedBy, x.FinalYear, CreatedUser = u.Name })
                    .Join(_emamiContext.PJPStatus.AsNoTracking(), x => x.pjp.PermanentJourneyPlanStatusId, ps => ps.Id, (x, ps) => new { x.pjp, x.CreatedBy, x.FinalYear, x.CreatedUser, Status = ps.Status })
                    .Distinct();

                if (pcpListContext != null && pcpListContext.Any())
                {
                    permanentJourneyPlans = pcpListContext.Select(_ => new PermanentJourneyPlansDto()
                    {
                        PJPId = _.pjp.Id,
                        PJPNumber = _.pjp.PJPNumber,
                        FinancialYearId = _.pjp.FinancialYearId,
                        FinancialYear = _.FinalYear,
                        Remarks = _.pjp.Remarks,
                        CreatedBy = _.CreatedBy,
                        CreatedUser = _.CreatedUser,
                        StatusId = _.pjp.PermanentJourneyPlanStatusId,
                        Status = _.Status
                    }).ToList();
                }

                return _resultService.SuccessObject(permanentJourneyPlans);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPermanentJourneyPlanList(IdInputDto inputDto)
        {
            _methodName = "GetPermanentJourneyPlanList";
            var resultDto = new ResultDto();
            var permanentJourneyPlans = new List<PermanentJourneyPlansDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                List<long> bdoList = new List<long>();
                bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                var pcpListContext = _emamiContext.PermanentJourneyPlans.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.CreatedBy, u => u.Id, (x, u) => new { pjp = x, CreatedUser = u.Name })
                    .Where(_ => bdoList.Contains(_.pjp.CreatedBy) && _.pjp.FinancialYearId == inputDto.Id);


                if (pcpListContext != null && pcpListContext.Any())
                {
                    permanentJourneyPlans = pcpListContext.Select(_ => new PermanentJourneyPlansDto()
                    {
                        PJPId = _.pjp.Id,
                        PJPNumber = _.pjp.PJPNumber,
                        FinancialYearId = _.pjp.FinancialYearId,
                        FinancialYear = _.pjp.Year != null ? _.pjp.Year.Year.ToString() : string.Empty,
                        Remarks = _.pjp.Remarks,
                        CreatedBy = _.pjp.CreatedBy,
                        CreatedUser = _.CreatedUser,
                        StatusId = _.pjp.PermanentJourneyPlanStatusId,
                        Status = _.pjp.PJPStatusName != null ? _.pjp.PJPStatusName.Status : string.Empty,
                        EffectiveFrom = _.pjp.EffectiveFrom,
                        EffectiveTo = _.pjp.EffectiveTo,
                    }).ToList();
                }

                return _resultService.SuccessObject(permanentJourneyPlans);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPermanentJourneyPlanDetails(PJPIdDto inputDto)
        {
            _methodName = "GetPermanentJourneyPlanDetails";
            var resultDto = new ResultDto();
            var outputDto = new PermanentJourneyPlanDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                if (inputDto.PJPId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PJPIdMissing);
                }

                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == inputDto.PJPId);
                var approverContext = _emamiContext.PJPApprovalInformation.Where(_ => _.PermanentJourneyPlanId == inputDto.PJPId && _.StatusId != (int)DTO.Enums.PermanentJourneyPlanStatus.Pending).OrderByDescending(_ => _.Id).FirstOrDefault();
                if (pjpContext != null)
                {
                    outputDto.PJPId = pjpContext.Id;
                    outputDto.PJPNumber = pjpContext.PJPNumber;
                    outputDto.StatusId = pjpContext.PermanentJourneyPlanStatusId;
                    outputDto.Status = pjpContext?.PJPStatusName.Status;
                    outputDto.FinancialYearId = pjpContext.FinancialYearId;
                    outputDto.FinancialYear = pjpContext.Year?.Year.ToString();
                    outputDto.CreatedBy = pjpContext.CreatedBy;
                    outputDto.CreatedByName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == pjpContext.CreatedBy)?.Name;
                    outputDto.EffectiveFrom = pjpContext.EffectiveFrom;
                    outputDto.EffectiveTo = pjpContext.EffectiveTo;
                    if (approverContext != null)
                    {
                        outputDto.Remarks = approverContext.Remarks;
                        outputDto.ReasonIds = approverContext.ReasonId;
                    }
                    var pjpDetailsList = new List<PermanentJourneyPlanDetailsDto>();
                    if (pjpContext.PJPDetails.Any())
                    {
                        pjpDetailsList = pjpContext.PJPDetails
                            .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { PCPDetail = x, StateName = s.StateName })
                            .Join(_emamiContext.City.AsNoTracking(), x => x.PCPDetail.TownId, c => c.Id, (x, c) => new { x.PCPDetail, x.StateName, CityName = c.CityName })
                            .ToList().Select(s => new { s.PCPDetail, s.StateName, s.CityName, DealerId = Convert.ToInt64(s.PCPDetail.RetailerId) }).ToList()
                            .GroupJoin(_emamiContext.Users.AsNoTracking(), x => x.DealerId, u => u.Id, (x, u) => new
                            {
                                x.PCPDetail,
                                x.StateName,
                                x.CityName,
                                DealerName = u.Any() ? u.FirstOrDefault().Name : string.Empty,
                                InHQNoVisitName = x.PCPDetail.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(x.PCPDetail.InHQNoVisit) : string.Empty
                            })
                            .Select(_ => new PermanentJourneyPlanDetailsDto()
                            {
                                PJPId = pjpContext.Id,
                                Id = _.PCPDetail.Id,
                                StateId = _.PCPDetail.StateId,
                                State = _.StateName,
                                CityId = _.PCPDetail.TownId,
                                City = _.CityName,
                                RetailerId = _.PCPDetail.RetailerId,
                                Retailer = _.DealerName,
                                NoOfDirectDealer = _.PCPDetail.NoOfDirectDealer,
                                NoOfSubDealer = _.PCPDetail.NoofSubDealer,
                                NoOfWholeSeller = _.PCPDetail.NoOfWholeSeller,
                                NoOfVisit = _.PCPDetail.NoOfVisit.ToString(),
                                InHQNoVisitId = _.PCPDetail.InHQNoVisit,
                                InHQNoVisitName = _.InHQNoVisitName,
                                Remarks = _.PCPDetail.Remarks,
                            }).ToList();
                        outputDto.PermanentJourneyPlanDetails = pjpDetailsList;
                    }

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

        public ResultDto GetTotalPCPByUsers(IdInputDto inputDto)
        {
            _methodName = "GetTotalPCPByUsers";
            var outputDto = new List<TotalPCPDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                List<long> bdoList = new List<long>();
                bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();

                outputDto = _emamiContext.PermanentJourneyPlanDetails.AsNoTracking().ToList().Select(s => new { pjp = s, DealerId = Convert.ToInt64(s.RetailerId) })
                    .Join(_emamiContext.City.AsNoTracking(), x => x.pjp.TownId, c => c.Id, (x, c) => new { x.pjp, x.DealerId, CityName = c.CityName })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.pjp.CreatedBy, u => u.Id, (x, u) => new { x.pjp, x.DealerId, x.CityName, BDOName = u.Name })
                    .GroupJoin(_emamiContext.Users.AsNoTracking(), x => x.DealerId, u => u.Id, (x, u) => new { x.pjp, x.DealerId, x.CityName, x.BDOName, DealerName = u.Any() ? u.FirstOrDefault().Name : string.Empty })
                    .Where(_ => _.pjp.PermanentJourneyPlan != null && _.pjp.PermanentJourneyPlan.FinancialYearId == inputDto.Id && bdoList.Contains(_.pjp.PermanentJourneyPlan.CreatedBy))
                    .GroupBy(g => g.pjp.TownId).ToList()
                    .Select(_ => new TotalPCPDto()
                    {
                        CityId = _.FirstOrDefault().pjp.TownId,
                        City = _.FirstOrDefault().CityName,
                        Dealers = string.Join(", ", _.Where(w => w.DealerName != string.Empty).Select(s => s.DealerName).Distinct().ToArray()),
                        NoOfDealers = _.Where(w => w.DealerName != string.Empty).Select(s => s.DealerName).Distinct().Count(),
                        NoOfVisit = _.Where(w => w.pjp.InHQNoVisit == 0).Sum(s => s.pjp.NoOfVisit),
                        HQVisitCount = _.Where(w => w.pjp.InHQNoVisit != 0).Sum(s => s.pjp.NoOfVisit),
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

        public ResultDto PermanentJourneyPlanApproval(PermanentJourneyPlanUpdateDto inputDto)
        {
            _methodName = "PermanentJourneyPlanApproval";
            var resultDto = new ResultDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == inputDto.PJPId);
                if (pjpContext != null)
                {
                    if (pjpContext.PermanentJourneyPlanStatusId == (int)DTO.Enums.Status.Pending || pjpContext.PermanentJourneyPlanStatusId == (int)DTO.Enums.Status.RequestForApproval)
                    {
                        var approverContext = _emamiContext.PJPApprovalInformation.FirstOrDefault(_ => _.PermanentJourneyPlanId == pjpContext.Id);
                        if (pjpContext == null || approverContext == null)
                        {
                            return _resultService.ErrorMessage(Constants.PJPNotFound);
                        }

                        pjpContext.PermanentJourneyPlanStatusId = inputDto.StatusId;
                        pjpContext.ModifiedBy = inputDto.LoginUserId;
                        pjpContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        approverContext.StatusId = inputDto.StatusId;
                        approverContext.ModifiedBy = inputDto.LoginUserId;
                        approverContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        approverContext.Remarks = inputDto.Remarks;

                        _emamiContext.SaveChanges();

                        #region Notification

                        try
                        {
                            if (inputDto.StatusId != (int)DTO.Enums.PermanentJourneyPlanStatus.Pending)
                            {
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                var CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == pjpContext.CreatedBy);
                                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PCPApproval);
                                string PCPStatus = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.StatusId).Name;
                                List<string> toUser = new List<string>();
                                toUser.Add(CreatedByUser.Email);
                                var emailSubject = Constants.PCPApprovalSubject;
                                var fromEmail = Constants.FromEmail;
                                var plainText = string.Empty;
                                if (emailTemplate != null)
                                {
                                    var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, PCPStatus);
                                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                                    amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                                }
                                var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PCPApprovalSMS);
                                if (smsTemplate != null)
                                {
                                    var replaceSmsTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, PCPStatus);
                                    amazonNotificationService.SendMessage(replaceSmsTemplate, CreatedByUser.MobileNumber);

                                    if (CreatedByUser != null && CreatedByUser.RegistrationTypeId != null && CreatedByUser.RegistrationTypeId > 0 && !string.IsNullOrEmpty(CreatedByUser.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = CreatedByUser.PushTokenKey,
                                            RegistrationTypeId = CreatedByUser.RegistrationTypeId != null ? (int)CreatedByUser.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = replaceSmsTemplate,
                                            //Id = pjpContext.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        #endregion
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.PermanentJourneyPlanAlreadyUpdated);
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = pjpContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Monthly Tour Plan

        public ResultDto GetPendingMonthlyTourPlanList(LoginUserIdDto inputDto)
        {
            _methodName = "GetPendingMonthlyTourPlanList";
            var resultDto = new ResultDto();
            var monthlyTourPlans = new List<MonthlyTourPlanDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var mtpListContext = _emamiContext.MonthlyTourPlans.AsNoTracking().Where(_ => _.MonthlyTourPlanStatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Pending)
                    .Join(_emamiContext.MonthlyTourPlanApprovalInformation.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && _.MonthlyTourPlanStatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Pending)
                        , mtp => mtp.Id, mtpai => mtpai.MonthlyTourPlanId, (mtp, mtpai) => new { mtp, CreatedBy = mtpai.CreatedBy })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.CreatedBy, u => u.Id, (x, u) => new { x.mtp, x.CreatedBy, CreatedUser = u.Name })
                    .Join(_emamiContext.MonthlyTourPlanStatus.AsNoTracking(), x => x.mtp.MonthlyTourPlanStatusId, ms => ms.Id, (x, ms) => new { x.mtp, x.CreatedBy, x.CreatedUser, Status = ms.Status })
                    .Distinct();

                if (mtpListContext != null && mtpListContext.Any())
                {
                    monthlyTourPlans = mtpListContext.Select(_ => new MonthlyTourPlanDto()
                    {
                        MTPId = _.mtp.Id,
                        MTPNumber = _.mtp.MTPNumber,
                        Remarks = _.mtp.Remarks,
                        CreatedBy = _.CreatedBy,
                        CreatedUser = _.CreatedUser,
                        StatusId = _.mtp.MonthlyTourPlanStatusId,
                        Status = _.Status
                    }).ToList();
                }

                return _resultService.SuccessObject(monthlyTourPlans);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetMonthlyTourPlanList(MTPDateWiseDetailsInputDto inputDto)
        {
            _methodName = "GetMonthlyTourPlanList";
            var resultDto = new ResultDto();
            var outputDto = new List<MonthlyTourPlanDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                DateTime resultMonthDate = new DateTime();
                if (inputDto.IsUpcoming)
                {
                    resultMonthDate = currentDate.AddMonths(1);
                }
                else
                {
                    resultMonthDate = currentDate;
                }
                List<long> bdoList = new List<long>();
                bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                var mtpListContext = _emamiContext.MonthlyTourPlans.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.CreatedBy, u => u.Id, (x, u) => new { mtp = x, CreatedUser = u.Name })
                    .Where(_ => bdoList.Contains(_.mtp.CreatedBy) && _.mtp.MTPDetails.Any(a => a.Date.Month == resultMonthDate.Month && a.Date.Year == resultMonthDate.Year));

                if (mtpListContext != null && mtpListContext.Any())
                {
                    outputDto = mtpListContext.Select(_ => new MonthlyTourPlanDto()
                    {
                        MTPId = _.mtp.Id,
                        MTPNumber = _.mtp.MTPNumber,
                        Remarks = _.mtp.Remarks,
                        CreatedBy = _.mtp.CreatedBy,
                        CreatedUser = _.CreatedUser,
                        StatusId = _.mtp.MonthlyTourPlanStatusId,
                        Status = _.mtp.MonthlyTourPlanStatus != null ? _.mtp.MonthlyTourPlanStatus.Status : string.Empty
                    }).ToList();
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

        public ResultDto GetMonthlyTourPlanDetails(MTPIdDto inputDto)
        {
            _methodName = "GetMonthlyTourPlanDetails";
            var outputDto = new MonthlyTourPlanDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                if (inputDto.MTPId == 0)
                {
                    return _resultService.ErrorMessage(Constants.MTPIdMissing);
                }

                var mtpContext = _emamiContext.MonthlyTourPlans.FirstOrDefault(_ => _.Id == inputDto.MTPId);
                var approverContext = _emamiContext.MonthlyTourPlanApprovalInformation.Where(_ => _.MonthlyTourPlanId == inputDto.MTPId && _.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Pending).OrderByDescending(_ => _.Id).FirstOrDefault();

                if (mtpContext != null)
                {
                    outputDto.MTPId = mtpContext.Id;
                    outputDto.MTPNumber = mtpContext.MTPNumber;
                    outputDto.StatusId = mtpContext.MonthlyTourPlanStatusId;
                    outputDto.Status = mtpContext.MonthlyTourPlanStatus?.Status;
                    outputDto.CreatedBy = mtpContext.CreatedBy;
                    outputDto.CreatedUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == mtpContext.CreatedBy)?.Name;
                    if (approverContext != null)
                    {
                        outputDto.Remarks = approverContext.Remarks;
                        outputDto.ReasonIds = approverContext.ReasonId;
                    }
                    var mtpDetailsList = new List<MonthlyTourPlanDetailsDto>();
                    if (mtpContext.MTPDetails.Any())
                    {
                        mtpDetailsList = mtpContext.MTPDetails.Where(_ => _.MonthlyTourPlan != null)
                            .ToList().Select(s => new { MTPDetail = s, DealerId = Convert.ToInt64(s.DealerId) }).ToList()
                            .GroupJoin(_emamiContext.Users.AsNoTracking(), x => x.DealerId, u => u.Id, (x, u) => new
                            {
                                MTPDetail = x.MTPDetail,
                                DealerName = u.Any() ? u.FirstOrDefault().Name : string.Empty,
                                InHQNoVisitName = x.MTPDetail.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(x.MTPDetail.InHQNoVisit) : string.Empty
                            })
                            .Select(_ => new MonthlyTourPlanDetailsDto()
                            {
                                MTPId = _.MTPDetail.MonthlyTourPlan.Id,
                                Id = _.MTPDetail.Id,
                                Date = _.MTPDetail.Date.ToString(),
                                MTPDate = _.MTPDetail.Date,
                                Day = _.MTPDetail.Date.ToString("dddd"),
                                TownId = _.MTPDetail.TownId,
                                Town = _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.MTPDetail.TownId) != null ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.MTPDetail.TownId).CityName : String.Empty,
                                Area = _.MTPDetail.Area,
                                DealerId = _.MTPDetail.DealerId,
                                Dealer = _.DealerName,
                                //HeadquartersId = _.MTPDetail.HeadquartersId,
                                //Headquarters = _.MTPDetail.Headquarters?.Name,
                                Remarks = _.MTPDetail.Remarks,
                                InHQNoVisitId = _.MTPDetail.InHQNoVisit,
                                InHQNoVisitName = _.InHQNoVisitName,
                            }).ToList();
                        outputDto.MonthlyTourPlanDetailList = mtpDetailsList;
                    }
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

        public ResultDto MonthlyTourPlanApproval(MonthlyTourPlanUpdateDto inputDto)
        {
            _methodName = "MonthlyTourPlanApproval";
            var resultDto = new ResultDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var mtpContext = _emamiContext.MonthlyTourPlans.FirstOrDefault(_ => _.Id == inputDto.MTPId);
                if (mtpContext == null)
                {
                    return _resultService.ErrorMessage(Constants.MTPNotFound);
                }
                if (mtpContext.MonthlyTourPlanStatusId == (int)DTO.Enums.Status.Pending)
                {
                    mtpContext.MonthlyTourPlanStatusId = inputDto.StatusId;
                    mtpContext.ModifiedBy = inputDto.LoginUserId;
                    mtpContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();

                    var approverContext = _emamiContext.MonthlyTourPlanApprovalInformation.FirstOrDefault(_ => _.MonthlyTourPlanId == mtpContext.Id);
                    approverContext.MonthlyTourPlanStatusId = inputDto.StatusId;
                    approverContext.ModifiedBy = inputDto.LoginUserId;
                    approverContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    approverContext.Remarks = inputDto.Remarks;
                    _emamiContext.SaveChanges();

                    #region Notification

                    try
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == mtpContext.CreatedBy);
                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPApproval);
                        string MTPStatus = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.StatusId).Name;
                        List<string> toUser = new List<string>();
                        toUser.Add(CreatedByUser.Email);
                        var emailSubject = Constants.MTPApprovalSubject;
                        var fromEmail = Constants.FromEmail;
                        var plainText = string.Empty;
                        if (emailTemplate != null)
                        {
                            var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, MTPStatus);
                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                            amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                        }
                        var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPApprovalSMS);
                        if (smsTemplate != null)
                        {
                            var replaceSmsTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, MTPStatus);
                            amazonNotificationService.SendMessage(replaceSmsTemplate, CreatedByUser.MobileNumber);

                            if (CreatedByUser != null && CreatedByUser.RegistrationTypeId != null && CreatedByUser.RegistrationTypeId > 0 && !string.IsNullOrEmpty(CreatedByUser.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = CreatedByUser.PushTokenKey,
                                    RegistrationTypeId = CreatedByUser.RegistrationTypeId != null ? (int)CreatedByUser.RegistrationTypeId : 0,
                                    Title = emailSubject,
                                    Message = replaceSmsTemplate,
                                    //Id = mtpContext.Id,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.MonthlyTourPlanAlreadyUpdated);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = mtpContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Monthly Tour Plan Deviation

        public ResultDto MonthlyPlanDeviationList(LoginUserIdDto inputDto)
        {
            _methodName = "MonthlyPlanDeviationList";
            var outputDto = new List<MonthlyTourPlanDeviationDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                List<long> bdoList = new List<long>();
                bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();

                outputDto = _emamiContext.MonthlyPlanDeviation.AsNoTracking().Where(_ => _.MonthlyTourPlanDetails != null)
                     .Join(_emamiContext.MonthlyPlanDeviationStatus.AsNoTracking(), x => x.StatusId, mtpds => mtpds.Id, (x, mtpds) => new { Deviation = x, Status = mtpds.Status })
                     .Join(_emamiContext.Users.AsNoTracking(), x => x.Deviation.CreatedBy, u => u.Id, (x, u) => new { x.Deviation, x.Status, BDOName = u.Name }).ToList()
                     .Select(s => new { s.Deviation, s.Status, s.BDOName, DealerId = Convert.ToInt64(s.Deviation.MonthlyTourPlanDetails.DealerId) })
                     .GroupJoin(_emamiContext.Users.AsNoTracking(), x => x.DealerId, u => u.Id, (x, u) => new { x.Deviation, x.Status, x.BDOName, DealerName = u.Any() ? u.FirstOrDefault().Name : string.Empty })
                     //.GroupJoin(_emamiContext.Reasons.AsNoTracking(), x => x.Deviation.ReasonId, r => r.Id, (x, r) => new { x.Deviation, x.Status, x.BDOName, x.DealerName, Reason = r.Any() ? r.FirstOrDefault().Reason : string.Empty })
                     .Where(_ => bdoList.Contains(_.Deviation.CreatedBy)).ToList()
                     .Select(_ => new MonthlyTourPlanDeviationDto()
                     {
                         MTPDetailId = _.Deviation.MonthlyTourPlanDetailsId,
                         DealerId = _.Deviation.MonthlyTourPlanDetails.DealerId,
                         Dealer = _.DealerName,
                         ActualDate = _.Deviation.MonthlyTourPlanDetails.Date.ToString(),
                         Remarks = _.Deviation.Remarks,
                         ToDealerId = _.Deviation.ToDealerId,
                         ToDealer = _.Deviation.ToDealerId != 0 ? _emamiContext.Users.FirstOrDefault(f => f.Id == _.Deviation.ToDealerId).Name : "",
                         RevisedDate = _.Deviation.RevisedDate.ToString(),
                         DeviationActualDate = _.Deviation.MonthlyTourPlanDetails.Date,
                         DeviationRevisedDate = _.Deviation.RevisedDate,
                         Id = _.Deviation.Id,
                         CreatedBy = _.Deviation.CreatedBy,
                         CreatedByUser = _.BDOName,
                         InHQNoVisitId = _.Deviation.MonthlyTourPlanDetails.InHQNoVisit,
                         InHQNoVisitName = _.Deviation.MonthlyTourPlanDetails.InHQNoVisit != 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.STPVisitType)_.Deviation.MonthlyTourPlanDetails.InHQNoVisit) : string.Empty,
                         StatusId = _.Deviation.StatusId,
                         Status = _.Status,
                         Town = _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.Deviation.MonthlyTourPlanDetails.TownId) != null ? _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.Deviation.MonthlyTourPlanDetails.TownId).CityName : String.Empty,
                         ReasonId = _.Deviation.ReasonId,
                         //Reason = _.Reason,
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

        public ResultDto MonthlyPlanDeviationApproval(MonthlyPlanDeviationDto inputDto)
        {
            _methodName = "MonthlyPlanDeviationApproval";
            var resultDto = new ResultDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var mpdContext = _emamiContext.MonthlyPlanDeviation.FirstOrDefault(_ => _.Id == inputDto.MTPDeviationId);
                if (mpdContext == null)
                {
                    return _resultService.ErrorMessage(Constants.MTPDeviationNotFound);
                }
                if (mpdContext.StatusId == (int)DTO.Enums.Status.Pending)
                {
                    mpdContext.StatusId = inputDto.StatusId;
                    mpdContext.Remarks = inputDto.Remarks;
                    mpdContext.ModifiedBy = inputDto.LoginUserId;
                    mpdContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                    _emamiContext.SaveChanges();

                    #region Notification

                    try
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == mpdContext.CreatedBy);
                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPDeviationApproval);
                        string MTPStatus = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.StatusId).Name;
                        List<string> toUser = new List<string>();
                        toUser.Add(CreatedByUser.Email);
                        var emailSubject = Constants.MTPDeviationApprovalSubject;
                        var fromEmail = Constants.FromEmail;
                        var plainText = string.Empty;
                        if (emailTemplate != null)
                        {
                            var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.CustomerName, mpdContext.ToDealer).Replace(Constants.ApproveOrReject, MTPStatus);
                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                            amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                        }
                        var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPDeviationApprovalSMS);
                        if (smsTemplate != null)
                        {
                            var replaceSmsTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.CustomerName, mpdContext.ToDealer).Replace(Constants.ApproveOrReject, MTPStatus);
                            amazonNotificationService.SendMessage(replaceSmsTemplate, CreatedByUser.MobileNumber);

                            if (CreatedByUser != null && CreatedByUser.RegistrationTypeId != null && CreatedByUser.RegistrationTypeId > 0 && !string.IsNullOrEmpty(CreatedByUser.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = CreatedByUser.PushTokenKey,
                                    RegistrationTypeId = CreatedByUser.RegistrationTypeId != null ? (int)CreatedByUser.RegistrationTypeId : 0,
                                    Title = emailSubject,
                                    Message = replaceSmsTemplate,
                                    //Id = mpdContext.Id,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.MTPDeviationAlreadyUpdated);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = mpdContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Secondary Sales

        public ResultDto GetSecondarySalesFortheDay(LoginZHId inputDto)
        {
            _methodName = "GetSecondarySalesFortheDay";
            var resultDto = new ResultDto();
            var outputDto = new List<WholesellerSecondarySaleslistDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                }

                outputDto = _emamiContext.WholeSellerSalesDetail.AsNoTracking().Join(_emamiContext.Users.AsNoTracking(), ws => ws.WholesellerBdo.DealerId, u => u.Id, (ws, u) => new { Sales = ws, DealerName = u.Name })
                    .Where(_ => _.Sales.WholesellerBdo != null && bdoList.Contains(_.Sales.CreatedBy) && _.Sales.CreatedDate.Month == currentDate.Month
                     && _.Sales.CreatedDate.Year == currentDate.Year).Select(s => new { s.Sales, s.DealerName, Date = DbFunctions.TruncateTime(s.Sales.CreatedDate) }).GroupBy(g => g.Date)
                     .Select(_ => new WholesellerSecondarySaleslistDto()
                     {
                         VisitDate = _.FirstOrDefault().Date,
                         WholesellerSecondarySales = _.GroupBy(g => g.Sales.WholesellerBdoId).Select(s => new WholesellerSecondarySalesDto()
                         {
                             DealerId = s.FirstOrDefault().Sales.WholesellerBdo.DealerId,
                             Dealer = s.FirstOrDefault().DealerName,
                             WholesellerId = s.FirstOrDefault().Sales.WholesellerBdoId,
                             Name = s.FirstOrDefault().Sales.WholesellerBdo.Name,
                             TotalPrice = s.Sum(t => t.Sales.Price),
                             TotalQuantity = s.Sum(t => t.Sales.QuantityPerMt),
                             VisitDate = _.FirstOrDefault().Date,
                         }).ToList(),
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

        #endregion

        #region Sauda
        public ResultDto SaudaApproval(SaudaApproveInputDto inputDto)
        {
            _methodName = "SaudaApproval";
            var resultDto = new ResultDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var saudaOrderContext = _emamiContext.SaudaOrders.FirstOrDefault(f => f.Id == inputDto.SaudaOrderId);
                if (saudaOrderContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaNotFound);
                }

                if (saudaOrderContext.StatusId == (int)DTO.Enums.Status.Pending || saudaOrderContext.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                {
                    saudaOrderContext.StatusId = inputDto.StatusId;
                    saudaOrderContext.ModifiedBy = inputDto.LoginUserId;
                    saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    saudaOrderContext.Remarks = inputDto.Remarks;

                    //if (!string.IsNullOrEmpty(inputDto.Remarks))
                    //{
                    //    var entity = new Remarks()
                    //    {
                    //        TableId = inputDto.SaudaOrderId,
                    //        TableName = "SaudaOrders",
                    //        ReasonTypeId = inputDto.StatusId,
                    //        Description = inputDto.Remarks,
                    //        CreatedBy = inputDto.LoginUserId,
                    //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    //    };
                    //    _emamiContext.Remarks.Add(entity);
                    //}
                    //_emamiContext.SaveChanges();

                    //Email and SMS Schedule Background Jobs
                    SaudaUpdateDto saudaUpdate = new SaudaUpdateDto();
                    saudaUpdate.SaudaOrderIds.Add(inputDto.SaudaOrderId);
                    saudaUpdate.StatusId = inputDto.StatusId;
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _saudaService.SaudaApproveRejectEmailSmsQueueWorkItem(cancellationToken, saudaUpdate);
                    });
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SaudaStatusAlreadyUpdated);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaOrderContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPendingSaudaChartForMobile(LoginZHId inputDto)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<PendingSaudaChartOutputDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                #region NewCode

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #BdoTemp(BdoId BIGINT)
                                    CREATE TABLE #DealerTemp(DealerId BIGINT)
                                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                    select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId
                                    insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
                                     insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoTemp)
                                    select 
                                    (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate,
                                    u.Id as UserId,
                                    pc.SaudaQuantity as BidQuantity
                                    from PendingContracts pc with(NOLOCK)
                                    join Users u on pc.UserId=u.Id
                                    join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId and pc.DistChnlId=sku.DistributionChannelId
                                    and pc.DivisionId=sku.DivisionId
                                    join #UserDivision udiv on udiv.SalesOrganizationId=pc.SalesOrgId and udiv.DistributionChannelId=pc.DistChnlId
                                    and pc.DivisionId=udiv.DivisionId
                                    where  pc.UserId in (select DealerId from #DealerTemp)
                                    and pc.PendingQuantityInCase > 0.99
                                    drop table #UserDivision
                                    drop table #BdoTemp
                                    drop table #DealerTemp";
                    saudaListDto = conn.Query<PendingSaudaChartOutputDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                    }).ToList();

                }
                #endregion


                #region OldCode
                //List<long> bdoList = new List<long>();

                ////New Reporting to table change
                //bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();

                ////bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                ////Multiple User Changes 
                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //   .Select(_ => new DivisionDetailsDto
                //   {
                //       SalesOrganizationId = _.SalesOrganizationId,
                //       DistributionChannelId = _.DistributionChannelId,
                //       DivisionId = _.DivisionId
                //   });
                //var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //var saudaContext = _emamiContext.Sauda.AsQueryable();
                //if (bdoList != null && bdoList.Any())
                //{
                //    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                //    if (dealersList != null && dealersList.Any())
                //    {
                //        var saudaStatus = Constants.OutstandingSaudaStatus;

                //        saudaListDto = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                //                        where pct.PendingQuantityInCase !=0 select pct into pc
                //                        join u in _emamiContext.Users.AsNoTracking() on pc.UserId equals u.Id
                //                        join dm in divisionslogieduser on new { SalesOrganizationId = pc.SalesOrgId, DistributionChannelId = pc.DistChnlId, DivisionId = pc.DivisionId }
                //                        equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                                  // join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals ud.UserId
                //                        join skus in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals skus.SkuCode
                //                        where pc.SalesOrgId == skus.SalesOrganizationId && pc.DistChnlId == skus.DistributionChannelId && pc.DivisionId == skus.DivisionId
                //                        //join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                        where dealersList.Contains(u.Id)
                //                         //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                        //&& bdoList.Contains(sauda.BdoId)
                //                        select new PendingSaudaChartOutputDto() 
                //                        { 
                //                            UserId = u.Id, 
                //                            BidQuantity = pc.SaudaQuantity , 
                //                            BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).BiddingDate : DateTime.MinValue 
                //                        }).ToList();

                //Old Query
                //var saudaListDto1 = (from pc in _emamiContext.PendingContracts.AsNoTracking()
                //                          join u in _emamiContext.Users.AsNoTracking() on pc.UserId equals u.Id
                //                         // join ud in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals ud.UserId
                //                          join skus in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals skus.SkuCode where pc.SalesOrgId == skus.SalesOrganizationId && pc.DistChnlId == skus.DistributionChannelId && pc.DivisionId == skus.DivisionId
                //                          join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                          where pc.PendingQuantityInCase != 0 && dealersList.Contains(u.Id)
                //                          select new PendingSaudaChartOutputDto() { UserId = u.Id, BidQuantity = pc.SaudaQuantity , BiddingDate = sauda.BiddingDate }).ToList();
                //    }
                //}
                #endregion

                if (saudaListDto != null && saudaListDto.Any())
                {
                    return _resultService.SuccessObject(saudaListDto);
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

        public ResultDto GetPendingSaudaChartDetailForMobile(LoginZHId inputDto)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaPendinglistOutputDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    //New Reporting to table change
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                #region NewCode

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #BdoTemp(BdoId BIGINT)
                                    CREATE TABLE #DealerTemp(DealerId BIGINT)
                                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                    select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId
                                    if(@BdoId=0)
                                    begin
                                     insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
                                     insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                     where UserId in (select BdoId from #BdoTemp)
                                    end
                                    else
                                    begin
	                                    insert into #BdoTemp(BdoId) select @BdoId
	                                     insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                     where UserId in (select BdoId from #BdoTemp)
                                    end
                                    select 
                                    pc.Id,
                                    (Case when s.SaudaNumber is null then 0 else s.Id end) as SaudaOrderId,
                                    (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate,
                                    u.Id as UserId,
                                    u.Name as [User],
                                    (Case when c.CityName is null then '' else c.CityName end) as City,
                                    pc.BasicRate as TotalBidPrice,
                                    pc.SaudaQuantity as TotalBidQuantity,
                                    (o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OilTypename,
                                    o.Id as OilTypeId,
                                    pc.SaudaNumber
                                    from PendingContracts pc with(NOLOCK)
                                    join Users u on pc.UserId=u.Id
                                    left join Saudas s on pc.SaudaNumber=s.SaudaNumber
                                    left join Cities c on u.CityId=c.Id
                                    join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId and pc.DistChnlId=sku.DistributionChannelId
                                    and pc.DivisionId=sku.DivisionId
                                    join OilTypes o on sku.OilTypeId=o.Id
                                    join SalesOrganizations sorg on o.SalesOrganizationId=sorg.Id
                                    join DistributionChannels dist on o.DistributionChannelId=dist.Id
                                    join Divisions div on o.DivisionId=div.Id
                                    join #UserDivision udiv on udiv.SalesOrganizationId=pc.SalesOrgId and udiv.DistributionChannelId=pc.DistChnlId
                                    and pc.DivisionId=udiv.DivisionId
                                    where 
                                    ((@SalesOrganizationId>0 and pc.SalesOrgId=@SalesOrganizationId) or @SalesOrganizationId=0)
                                    and ((@DistributionChannelId>0 and pc.DistChnlId=@DistributionChannelId) or @DistributionChannelId=0)
                                    and ((@DivisionId>0 and pc.DivisionId=@DivisionId) or @DivisionId=0)
                                    and pc.UserId in (select DealerId from #DealerTemp)
                                    and pc.PendingQuantityInCase > 0.99
                                    order by pc.Id desc
                                    drop table #UserDivision
                                    drop table #BdoTemp
                                    drop table #DealerTemp";
                    saudaListDto = conn.Query<SaudaPendinglistOutputDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        BdoId = inputDto.BDOId,
                        SalesOrganizationId = inputDto.SalesOrganizationId,
                        DistributionChannelId = inputDto.DistributionChannelId,
                        DivisionId = inputDto.DivisionId
                    }).ToList();

                }

                #endregion

                #region OldCode
                //if (bdoList != null && bdoList.Any())
                //{
                //    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();

                //    var divisionsloginWiseuser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //        .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                //    if (dealersList != null && dealersList.Any())
                //    {
                //        var pendingContractContext = _emamiContext.PendingContracts.AsNoTracking();
                //        if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                //        {
                //            pendingContractContext = pendingContractContext.Where(_ => _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);
                //        }
                //        var cityContext = _emamiContext.City.AsQueryable();

                //saudaListDto = pendingContractContext.Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaNumber, s => s.SaudaNumber, (so, s) => new { so, s })
                //     .Join(_emamiContext.Users.AsNoTracking(), x => x.so.UserId, u => u.Id, (x, u) => new { x, u })
                //    //.Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, DealerName = x.u.Name, CityName = c.CityName, DealerId = x.u.Id/*, VerticalId = x.u.DivisionId*/ })
                //    .Join(_emamiContext.Skus.AsNoTracking(), s => s.x.so.MaterialCode, ss => ss.SkuCode, (s, ss) => new { s.x, ss, DealerName=s.u.Name,CityId=s.u.CityId,  DealerId=s.u.Id})
                //    .Join(_emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => (_.SalesOrganizationId == inputDto.SalesOrganizationId || _.SalesOrganizationId > 0) &&(_.DistributionChannelId == inputDto.DistributionChannelId ||  _.DistributionChannelId > 0) && (_.DivisionId == inputDto.DivisionId || _.DivisionId > 0)) , t => t.DealerId , udm => udm.UserId , (t,udm) => new { t , udm})
                //    .Where(_ => _.t.x.so.PendingQuantityInCase != 0 && dealersList.Contains(_.t.DealerId) && (_.t.ss.SalesOrganizationId == inputDto.SalesOrganizationId || _.t.ss.SalesOrganizationId > 0) && (_.t.ss.DistributionChannelId == inputDto.DistributionChannelId || _.t.ss.DistributionChannelId > 0) && (_.t.ss.DivisionId == inputDto.DivisionId || _.t.ss.DivisionId > 0)     
                //     ).Select(a => new SaudaListDto {
                //        Id = a.t.x.so.Id,
                //        SaudaOrderId = a.t.x.s.Id,  //Sauda Table Id sent
                //        UserId = a.t.DealerId,
                //        User = a.t.DealerName,
                //        City = cityContext.FirstOrDefault(_ => _.Id==a.t.CityId)!=null? cityContext.FirstOrDefault(_ => _.Id == a.t.CityId).CityName : String.Empty,
                //        BiddingDate =  a.t.x.s.BiddingDate,
                //        TotalBidPrice = a.t.x.so.BasicRate,
                //        TotalBidQuantity = a.t.x.so.SaudaQuantity,
                //        OiltypeName = a.t.ss.OilType.Name,
                //        OilTypeId = a.t.ss.OilType.Id,
                //        StatusId = 0,
                //        Status = string.Empty,
                //    }).ToList();          
                //        var saudaContext = _emamiContext.Sauda.AsQueryable();
                //        saudaListDto = (from pct in pendingContractContext
                //                        where pct.PendingQuantityInCase!=0 select pct into pc
                //                        join ud in _emamiContext.Users.AsNoTracking() on pc.UserId equals ud.Id
                //                        join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                //                        where pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId && pc.DivisionId == sku.DivisionId
                //                        join o in _emamiContext.OilTypes.AsNoTracking() on sku.OilTypeId equals o.Id
                //                        join sorg in _emamiContext.SalesOrganization.AsNoTracking() on o.SalesOrganizationId equals sorg.Id
                //                        join dist in _emamiContext.DistributionChannel.AsNoTracking() on o.DistributionChannelId equals dist.Id
                //                        join div in _emamiContext.Divisions.AsNoTracking() on o.DivisionId equals div.Id
                //                        //join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                        join dm in divisionsloginWiseuser on new { SalesOrganizationId = pc.SalesOrgId, DistributionChannelId = pc.DistChnlId, DivisionId = pc.DivisionId }
                //                         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId } /*into saudadb from sd in saudadb.DefaultIfEmpty()*/
                //                        where  dealersList.Contains(pc.UserId)
                //                         //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                        //&& bdoList.Contains(sauda.BdoId)
                //                        select new SaudaListDto()
                //                        {
                //                            Id = pc.Id,
                //                            SaudaOrderId = saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).Id : 0,//sauda table Id
                //                            UserId = ud.Id,
                //                            User = ud.Name,
                //                            City = cityContext.FirstOrDefault(_ => _.Id == ud.CityId) != null ? cityContext.FirstOrDefault(_ => _.Id == ud.CityId).CityName : String.Empty,
                //                            BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).BiddingDate : DateTime.Today,
                //                            TotalBidPrice = pc.BasicRate,
                //                            TotalBidQuantity = pc.SaudaQuantity,
                //                            OiltypeName = o.Name + "-" + sorg.Code + "/" + dist.Code + "/" + div.Code,
                //                            OilTypeId = o.Id,
                //                            StatusId = 0,
                //                            Status = string.Empty,
                //                        }).ToList();

                //    }
                //}

                #endregion


                if (saudaListDto != null && saudaListDto.Any())
                {

                    if (inputDto.IsPendingSauda)
                    {
                        var data = saudaListDto.OrderByDescending(s => s.BiddingDate).GroupBy(s => s.BiddingDate.Date).Select(a => new SaudaListGroupedOutputDto()
                        {
                            BiddingDate = a.Key,
                            saudaListOutputs = a.Select(sauda => new SaudaListOutputDto()
                            {
                                SaudaId = sauda.Id,
                                SaudaNo = sauda.Id.ToString(),
                                SaudaOrderId = sauda.SaudaOrderId,
                                BiddingDate = sauda.BiddingDate,
                                TotalQty = sauda.TotalBidQuantity,
                                SaudaNumber = sauda.SaudaNumber != null ? sauda.SaudaNumber : string.Empty,
                                DealerName = sauda.User,
                                DealerId = sauda.UserId
                            }).ToList()
                        }).ToList();
                        return _resultService.SuccessObject(data);
                    }
                    else
                    {
                        saudaListDto = saudaListDto.OrderBy(a => a.User).ToList();
                        return _resultService.SuccessObject(saudaListDto);
                    }
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
    
        public ResultDto GetBookedSauda(LoginZHId inputDto)
        {
            _methodName = "GetBookedSauda";
            var resultDto = new ResultDto();
            var saudaListDto = new List<BookedSaudaDto>();
            var saudaListDtos = new List<BookedSaudaDto>();
            var outputDto = new List<BookedSaudaDealerGroupDto>();

            try
            {
                if (inputDto == null)
                    return _resultService.ErrorMessage(Constants.InvalidRequest);

                if (inputDto.LoginUserId == 0)
                    return _resultService.ErrorMessage(Constants.UserIdMissing);

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);

                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);

                var usersContext = _emamiContext.Users
                    .AsNoTracking()
                    .FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);

                if (usersContext == null)
                    return _resultService.ErrorMessage(Constants.UserNotFound);

                List<long> bdoList = new List<long>();
                List<long> dealerList = new List<long>();

                if (inputDto.BdoIds != null && inputDto.BdoIds.Any())
                    bdoList.AddRange(inputDto.BdoIds);
                else if (inputDto.BDOId > 0)
                    bdoList.Add(inputDto.BDOId);
                else
                    bdoList = _emamiContext.UserReportingToMappings
                        .AsNoTracking()
                        .Where(_ => _.ReportingToUserId == inputDto.LoginUserId)
                        .Select(_ => _.UserId)
                        .ToList();

                if (inputDto.DealerId > 0)
                    dealerList.Add(inputDto.DealerId);
                else if (inputDto.DealerIds != null && inputDto.DealerIds.Any())
                    dealerList.AddRange(inputDto.DealerIds);
                else
                    dealerList = _emamiContext.UserCustomerMapping
                        .AsNoTracking()
                        .Where(_ => bdoList.Contains(_.UserId))
                        .Select(_ => _.CustomerId)
                        .Distinct()
                        .ToList();

                if (dealerList != null && dealerList.Any())
                {
                    var oilTypesContext = _emamiContext.OilTypes
                        .AsNoTracking()
                        .Select(s => new
                        {
                            Id = s.Id,
                            Name = s.Name + "-" +
                                   s.SalesOrganization.Code + "/" +
                                   s.DistributionChannel.Code + "/" +
                                   s.Division.Code
                        }).ToList();



                    //saudaListDto = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null).Join(_emamiContext.Users.AsNoTracking(), so => so.Sauda.UserId, u => u.Id, (so, u) =>
                    //            new { Sauda = so.Sauda, OilTypeId = so.OilTypeId, OilTypeName = so.OilType.Name, StatusId = so.StatusId, SaudaOrderId = so.Id, DealerId = u.Id, DealerName = u.Name, u.CityId, u.StateId, u.Code })
                    //        .Join(_emamiContext.UserRoles.AsNoTracking(), x => x.DealerId, ur => ur.UserId, (x, ur) =>
                    //        new { x.Sauda, x.OilTypeId, x.OilTypeName, x.DealerId, x.DealerName, x.StatusId, x.SaudaOrderId, IsBroker = (ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false), x.CityId, x.StateId, x.Code })
                    //        .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.StatusId, ss => ss.Id, (x, ss) => new { x.Sauda, x.OilTypeId, x.OilTypeName, x.DealerId, x.DealerName, x.StatusId, x.SaudaOrderId, x.IsBroker, StatusName = (x.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : ss.Name), x.CityId, x.StateId, x.Code })
                    //        .Where(_ => dealerList.Contains(_.Sauda.UserId) && (DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)))
                    //        .Select(_ => new BookedSaudaDto()
                    //        {
                    //            SaudaOrderId = _.SaudaOrderId,
                    //            DealerId = _.DealerId,
                    //            Dealer = string.Concat((_.DealerName) + "-" + (cityContext.FirstOrDefault(c => c.Id == _.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == _.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == _.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == _.StateId).StateName : string.Empty) + "-" + (_.Code)),
                    //            //DealerCode = cityContext.FirstOrDefault(c => c.Id == _.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == _.CityId).CityName : string.Empty + "-" + stateContext.FirstOrDefault(s => s.Id == _.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == _.StateId).StateName : string.Empty + "-" + _.Code,
                    //            SaudaBookedDate = _.Sauda.BiddingDate,
                    //            IsBroker = _.IsBroker,
                    //            SaudaNumber = _.Sauda.Id.ToString(),
                    //            OilTypeId = _.OilTypeId,
                    //            OilType = _.OilTypeName,
                    //            StatusId = _.StatusId,
                    //            Status = _.StatusName,
                    //        }).ToList();
                    var userRoleContext = _emamiContext.UserRoles.AsNoTracking().ToList();
                    var cityContext = _emamiContext.City.AsNoTracking().ToList();
                    var stateContext = _emamiContext.State.AsNoTracking().ToList();

                    //var saudatableContext = _emamiContext.Sauda.AsNoTracking()
                    //                   .Where(w => dealerList.Contains(w.UserId)
                    //                    && (DbFunctions.TruncateTime(w.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //       && DbFunctions.TruncateTime(w.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)))
                    //       .Select(s => new
                    //                   {
                    //                       Id = s.Id,
                    //                       UserId = s.UserId,
                    //                       SaudaBookingTypeId = s.SaudaBookingTypeId,
                    //                       BiddingDate = s.BiddingDate
                    //                   }).ToList();
                    //var saudaId = saudatableContext.Select(s => s.Id).Distinct().ToList();
                    //var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking()
                    //    .Where(w => saudaId.Contains(w.SaudaId))
                    //    .Select(s => new
                    //    {
                    //        Id = s.Id,
                    //        SaudaId = s.SaudaId,
                    //        OilTypeId = s.OilTypeId,
                    //        SkuId = s.SkuId
                    //    }).ToList();
                    //var userContext = _emamiContext.Users.AsNoTracking()
                    //    .Where(w => dealerList.Contains(w.Id))
                    //    .Select(s => new
                    //    {
                    //        Id = s.Id,
                    //        Name = s.Name,
                    //        Code = s.Code,
                    //        s.CityId,
                    //        s.StateId
                    //    }).ToList();

                    var divisionslogieduser = _emamiContext.UserDivisionMappings
    .AsNoTracking()
    .Where(_ => _.UserId == inputDto.LoginUserId)
    .Select(_ => new DivisionDetailsDto
    {
        SalesOrganizationId = _.SalesOrganizationId,
        DistributionChannelId = _.DistributionChannelId,
        DivisionId = _.DivisionId
    }).ToList();

                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var sqlQuery = @"
                    Create Table #DealerIdsTemp(DealerId bigint)
                    Create Table #BdoId(BdoId bigint)
                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

                    insert into #UserDivision
                    select SalesOrganizationId,DistributionChannelId,DivisionId 
                    from UserDivisionMappings where UserId=@UserId

                    if(@BdoId > 0)
                    begin
                        insert into #BdoId select @BdoId
                    end
                    if(@BdoIds != null or @BdoIds !='')
                    begin
                        insert into #BdoId select Data from dbo.Split(@BdoIds,',')
                    end
                    else
                    begin
                        insert into #BdoId 
                        select UserId from UserReportingToMappings where ReportingToUserId=@UserId
                    end
                    if(@CustomerId > 0)
                    begin
                        insert into #DealerIdsTemp select @CustomerId
                    end
                    if(@CustomerIds != null or @CustomerIds !='')
                    begin
                        insert into #DealerIdsTemp select Data from dbo.Split(@CustomerIds,',')
                    end
                    else
                    begin
                        insert into #DealerIdsTemp 
                        select CustomerId from UserCustomerMappings 
                        where UserId in (select BdoId from #BdoId)
                    end
                    select 
                        s.Id as SaudaId,
                        so.Id as SaudaOrderId,
                        u.Id as DealerId,
                        (u.Name+'-'+isnull(c.CityName,'')+'-'+isnull(stat.StateName,'')+'-'+u.Code) as Dealer,
                        s.BiddingDate as SaudaBookedDate,
                        (case when ur.RoleId=6 then 1 else 0 end) as IsBroker,
                        s.Id as SaudaNumber,
                        o.Id as OilTypeId,
                        o.Name as OilType,
                        so.StatusId,
                        (case when so.StatusId=1 then 'Accepted' else st.Name end) as Status
                    from Saudas s
                    join SaudaOrders so on s.Id = so.SaudaId
                    join Skus sku on sku.Id = so.SkuId
                    join OilTypes o on o.Id = sku.OilTypeId
                    join Users u on s.UserId = u.Id
                    left join Cities c on u.CityId = c.Id
                    left join States stat on u.StateId = stat.Id
                    join #UserDivision ud on
                        ud.SalesOrganizationId = s.SalesOrganizationId
                        and ud.DistributionChannelId = s.DistributionChannelId
                        and ud.DivisionId = s.DivisionId
                    join UserRoles ur on u.Id = ur.UserId
                    join Status st on so.StatusId = st.Id
                    where u.Id in (select DealerId from #DealerIdsTemp)
                    and cast(s.BiddingDate as date) between cast(@FromDate as date) and cast(@ToDate as date)

                    drop table #BdoId
                    drop table #DealerIdsTemp
                    drop table #UserDivision";

                        saudaListDto = conn.Query<BookedSaudaDto>(sqlQuery, new
                        {
                            inputDto.FromDate,
                            inputDto.ToDate,
                            CustomerId = inputDto.DealerId,
                            CustomerIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.DealerIds),
                            BdoId = inputDto.BDOId,
                            BdoIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.BdoIds),
                            UserId = inputDto.LoginUserId
                        }).ToList();
                    }


                    //var saudaOrder = (from so in _emamiContext.SaudaOrders.AsNoTracking()
                    //                         join u in _emamiContext.Users.AsNoTracking() on so.Sauda.UserId equals u.Id
                    //                         join dm in divisionslogieduser on new { SalesOrganizationId = so.Sauda.SalesOrganizationId, DistributionChannelId = so.Sauda.DistributionChannelId, DivisionId = so.Sauda.DivisionId }
                    //                         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                         join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                    //                         join ss in _emamiContext.ApprovalStatus.AsNoTracking() on so.StatusId equals ss.Id
                    //                         where so.Sauda != null 
                    //                         && dealerList.Contains(so.Sauda.UserId) 
                    //                         //&& (bdoList.Contains(so.Sauda.BdoId) || so.Sauda.BdoId==0)
                    //                          && (DbFunctions.TruncateTime(so.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //                          && DbFunctions.TruncateTime(so.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    //                         select new
                    //                         {
                    //                             Sauda = so.Sauda,
                    //                             OilTypeId = so.OilTypeId,
                    //                             OilTypeName = so.OilType.Name,
                    //                             StatusId = so.StatusId,
                    //                             SaudaOrderId = so.Id,
                    //                             DealerId = u.Id,
                    //                             DealerName = u.Name,
                    //                             u.CityId,
                    //                             u.StateId,
                    //                             u.Code,                                                
                    //                             IsBroker = (ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false),
                    //                             StatusName = (so.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : ss.Name)
                    //                         }).AsEnumerable(); 

                    //old query
                    //var saudaOrder1 = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null)
                    //   .Join(_emamiContext.Users.AsNoTracking(), so => so.Sauda.UserId, u => u.Id, (so, u) =>
                    //           new { Sauda = so.Sauda, OilTypeId = so.OilTypeId, OilTypeName = so.OilType.Name, StatusId = so.StatusId, SaudaOrderId = so.Id, DealerId = u.Id, DealerName = u.Name, u.CityId, u.StateId, u.Code })
                    //       .Join(_emamiContext.UserRoles.AsNoTracking(), x => x.DealerId, ur => ur.UserId, (x, ur) =>
                    //       new { x.Sauda, x.OilTypeId, x.OilTypeName, x.DealerId, x.DealerName, x.StatusId, x.SaudaOrderId, IsBroker = (ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false), x.CityId, x.StateId, x.Code })
                    //       .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.StatusId, ss => ss.Id, (x, ss) => new { x.Sauda, x.OilTypeId, x.OilTypeName, x.DealerId, x.DealerName, x.StatusId, x.SaudaOrderId, x.IsBroker, StatusName = (x.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : ss.Name), x.CityId, x.StateId, x.Code })
                    //       .Where(_ => dealerList.Contains(_.Sauda.UserId)
                    //       && (DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //       && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate))).AsEnumerable();

                    //saudaListDto = saudaOrder.ToList().Select(_ => new BookedSaudaDto()
                    //{
                    //    SaudaId = _.Sauda.Id,
                    //    SaudaOrderId = _.SaudaOrderId,
                    //    DealerId = _.DealerId,
                    //    Dealer = string.Concat((_.DealerName) + "-" + (cityContext.FirstOrDefault(c => c.Id == _.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == _.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == _.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == _.StateId).StateName : string.Empty) + "-" + (_.Code)),
                    //    //DealerCode = cityContext.FirstOrDefault(c => c.Id == _.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == _.CityId).CityName : string.Empty + "-" + stateContext.FirstOrDefault(s => s.Id == _.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == _.StateId).StateName : string.Empty + "-" + _.Code,
                    //    SaudaBookedDate = _.Sauda.BiddingDate,
                    //    IsBroker = _.IsBroker,
                    //    SaudaNumber = _.Sauda.Id.ToString(),
                    //    OilTypeId = _.OilTypeId,
                    //    OilType = _.OilTypeName,
                    //    StatusId = _.StatusId,
                    //    Status = _.StatusName,
                    //}).ToList();

                    // --- NEW: populate ApprovalUser and IsApprovalView using latest SaudaApproval for each Sauda ---
                    if (saudaListDto != null && saudaListDto.Any())
                    {
                        var saudaIds = saudaListDto.Select(s => s.SaudaId).Distinct().ToList();
                        var latestApprovals = _emamiContext.SaudaApproval.AsNoTracking()
                            .Where(a => saudaIds.Contains(a.SaudaId))
                            .GroupBy(a => a.SaudaId)
                            .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault())
                            .ToList();

                        var approvalDict = latestApprovals
                            .Where(a => a != null)
                            .ToDictionary(a => a.SaudaId, a => a);

                        var approvalUserIds = latestApprovals.Where(a => a != null && a.RequestedTo > 0).Select(a => a.RequestedTo).Distinct().ToList();
                        var approvalUsers = _emamiContext.Users.AsNoTracking().Where(u => approvalUserIds.Contains(u.Id))
                            .ToDictionary(u => u.Id, u => u.Name);

                        foreach (var sauda in saudaListDto)
                        {
                            if (approvalDict.TryGetValue(sauda.SaudaId, out var approval) && approval != null)
                            {
                                sauda.ApprovalUser = approval.RequestedTo > 0 && approvalUsers.ContainsKey(approval.RequestedTo) ? approvalUsers[approval.RequestedTo] : string.Empty;
                            }
                            else
                            {
                                sauda.ApprovalUser = string.Empty;
                              
                            }
                        }
                    }
                    // --- END NEW ---

                    saudaListDto = saudaListDto
                        .GroupBy(x => x.SaudaId)
                        .Select(g => g.First())
                        .ToList();

                    var saudaId = saudaListDto.Select(s => s.SaudaId).Distinct().ToList();

                    var saudaOrderContext = _emamiContext.SaudaOrders
                        .AsNoTracking()
                        .Where(w => saudaId.Contains(w.SaudaId))
                        .Select(s => new
                        {
                            s.Id,
                            s.SaudaId,
                            OilTypeId = s.Sku.OilTypeId,
                            s.SkuId,
                            s.BidQuantity
                        }).ToList();
                    foreach (var dealer in dealerList)
                    {
                        var saudaContext = saudaListDto.Where(_ => _.DealerId == dealer).ToList();
                        if (saudaContext != null)
                        {
                            foreach (var sauda in saudaContext)
                            {
                                var SaudaDetailContext = saudaOrderContext.Where(_ => _.SaudaId == sauda.SaudaId);
                                var saudaDto = new BookedSaudaDto()
                                {
                                    SaudaId = sauda.SaudaId,
                                    SaudaOrderId = sauda.SaudaOrderId,
                                    DealerId = sauda.DealerId,
                                    Dealer = sauda.Dealer,
                                    SaudaBookedDate = sauda.SaudaBookedDate,
                                    IsBroker = sauda.IsBroker,
                                    SaudaNumber = sauda.SaudaNumber,
                                    OilTypeId = sauda.OilTypeId,
                                    OilType = sauda.OilType,
                                    StatusId = sauda.StatusId,
                                    Status = sauda.Status,
                                    TotalQuantity = SaudaDetailContext.IsAny() ? SaudaDetailContext.Sum(_ => _.BidQuantity) : 0,
                                    // preserve approval info retrieved earlier (if any)
                                    ApprovalUser = sauda.ApprovalUser,
                                };

                                var results = SaudaDetailContext.GroupBy(
                                    p => p.OilTypeId,
                                    p => p.SkuId,
                                    (key, g) => new { OilTypeId = key, Skus = g.ToList() }).ToList();

                                foreach (var detail in results)
                                {
                                    var OilType = oilTypesContext.FirstOrDefault(_ => _.Id == detail.OilTypeId);
                                    var DetailDto = new BookedSaudaDetailDto
                                    {
                                        OilTypeId = (long)detail.OilTypeId,
                                        OilType = OilType != null ? OilType.Name : string.Empty,
                                        SkuCount = detail.Skus.Count
                                    };
                                    saudaDto.BookedSaudaDetailDto.Add(DetailDto);
                                }

                                saudaListDtos.Add(saudaDto);
                            }
                        }
                    }

                    outputDto = saudaListDtos
                        .GroupBy(_ => _.DealerId)
                        .Select(g => new BookedSaudaDealerGroupDto
                        {
                            DealerId = g.Key,
                            Dealer = g.First().Dealer,
                            BookedSaudaList = g.ToList()
                        }).ToList();
                }

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {exception}");
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }








        public ResultDto SaudaCreation(SaudaInputDto inputDto)
        {
            _methodName = "SaudaCreation";
            var resultDto = new ResultDto();
            try
            {

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.SalesOrganizationId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SalesOrganisationIsEmpty);
                }
                if (inputDto.DistributionChannelId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DistributionChannelIsEmpty);
                }
                if (inputDto.DivisionId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DivisionIsEmpty);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                   .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                   && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                   && _.DivisionId == inputDto.DivisionId);

                decimal TotalQtyInMT = 0;
                var bookedSkuQuantity = inputDto.SaudaOrders.Select(s => new ConvertCasetoMetricTon()
                {
                    SkuId = s.SkuId,
                    Quantity = s.BidQuantity
                }).ToList();
                TotalQtyInMT = _resultService.ConvertCasetoMetricTonSaudaBooking(bookedSkuQuantity);
                //foreach (var item in inputDto.SaudaOrders)
                //{
                //    TotalQtyInMT = TotalQtyInMT + _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                //}

                var statuses = Constants.OverallSaudaStatus;
                //var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
                //                               join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //                               where s.UserId == inputDto.DealerId
                //                               && s.SaudaNumber == null && s.StatusId == (int)DTO.Enums.Status.Pending
                //                               select new { BidQuantity = so.BidQuantity, SkuId = so.SkuId }
                //                               ).ToList();

                //if (SaudaOutstandingContext != null && SaudaOutstandingContext.Any())
                //{
                //decimal invoiceQuantity = 0;
                //var existingSaudaQuantity = SaudaOutstandingContext.Sum(_ => _.BidQuantity);
                //var skuIds = SaudaOutstandingContext.Select(_ => _.SkuId).Distinct().ToList();
                //var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                //                      where inv.UserId == inputDto.DealerId
                //                      && skuIds.Contains(invDet.SkuId)
                //                      select new
                //                      {
                //                          ActualBilledQuantity = invDet.ActualBilledQuantity
                //                      }).ToList();

                //if (invoiceContext != null && invoiceContext.Any())
                //{
                //    invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
                //}

                // var pendingContracttablevalue = _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId).ToList().IsAny() ? _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId).Select(_ => _.SaudaQuantity).Sum() : 0;
                var SaudaOutstanding = TotalQtyInMT;
                var usersaudalimit = userdivContext.SaudaLimit ?? 0;
                var SaudaLimit = _resultService.AvailableSaudaLimit(inputDto.DealerId, usersaudalimit, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId);
                if (SaudaLimit < SaudaOutstanding)
                {
                    return _resultService.ErrorMessage(Constants.SaudaLimitIsExceeds);
                }
                //}

                bool isDiscountAppliedZero = false;
                var QuantityLimitForBookingSaudaName = Utility.GetEnumDescription(DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == QuantityLimitForBookingSaudaName);
                bool IsQuantityLimitForBookingSauda = Convert.ToBoolean(configurationContext.Value);
                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {

                    var oiltypesContext = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.IsActive);
                    var overallSaudaStatuses = Constants.OverallSaudaStatus;
                    foreach (var item in inputDto.SaudaOrders)
                    {
                        decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                        var skusContext = _emamiContext.Skus.AsNoTracking().Where(_ => _.IsActive);
                        var skuContext = skusContext.FirstOrDefault(_ => _.Id == item.SkuId);

                        if (skuContext != null)
                        {
                            decimal availableQuantityBdo = 0;
                            long bdoId = 0;

                            if (configurationContext != null && IsQuantityLimitForBookingSauda)
                            {
                                var bdoContext = _emamiContext.UserCustomerMapping.AsNoTracking().FirstOrDefault(_ => _.CustomerId == inputDto.DealerId);
                                if (bdoContext != null)
                                {
                                    bdoId = bdoContext.UserId;
                                }

                                var bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == bdoId && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                              && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                if (bdoLimitContext != null)
                                {
                                    decimal saudaBidQuantity = 0;
                                    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == bdoId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    if (dealerList != null && dealerList.Any())
                                    {
                                        saudaBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda)
                                              .Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
                                    }
                                    //decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                                    decimal orderedQuantityBdo = 0;
                                    decimal totalQuantityBdo = requestedQuantityBdo;
                                    if (saudaBidQuantity != 0)
                                    {
                                        orderedQuantityBdo = saudaBidQuantity;
                                        totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                    }
                                    if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
                                    {
                                        availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                        if (availableQuantityBdo < 0)
                                        {
                                            availableQuantityBdo = 0;
                                        }
                                        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                                    }
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
                                }
                            }
                        }

                        //var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(currentDate) && _.IsActive);
                        //if (pricingContext == null)
                        //{
                        //    return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
                        //}

                        //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                        //{
                        //    //var TodayBiddingWindowIds = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.BiddingDate == DbFunctions.TruncateTime(currentDate) && _.Id == pricingContext.BiddingWindowId && _.Isactive).ToList();
                        //    //if (TodayBiddingWindowIds == null)
                        //    //{
                        //    //    return _resultService.ErrorMessage(Constants.BiddingWindowisnotValid);
                        //    //}

                        //    int CounterBidAllowCount = 0;
                        //    var CounterBidAllowContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.CounterBidCount);
                        //    if (CounterBidAllowContext != null)
                        //    {
                        //        CounterBidAllowCount = Convert.ToInt32(CounterBidAllowContext.Value);
                        //    }
                        //    //var isSKuExistsContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.BiddingwindowId == pricingContext.BiddingWindowId && _.SkuId == item.SkuId
                        //    //    && _.OilTypeId == item.OilTypeId && _.Incoterms2 == item.IncotermsId && _.PlantId == item.PlantId).ToList();
                        //    //if (isSKuExistsContext != null && isSKuExistsContext.Count >= CounterBidAllowCount)
                        //    //{
                        //    //    return _resultService.ErrorMessage(Constants.SkuAlreadyBookedinBidding);
                        //    //}

                        //    var TodayBiddingIds = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.BiddingDate == DbFunctions.TruncateTime(currentDate) && _.Isactive);
                        //    if (TodayBiddingIds != null)
                        //    {
                        //        var SaudaContext = (from sauda in _emamiContext.Sauda
                        //                            join saudaorder in _emamiContext.SaudaOrders on sauda.Id equals saudaorder.SaudaId
                        //                            join biddings in TodayBiddingIds on saudaorder.BiddingwindowId equals biddings.Id
                        //                            where sauda.UserId == inputDto.DealerId && saudaorder.StatusId == (int)DTO.Enums.Status.Hold
                        //                            && saudaorder.SkuId == item.SkuId
                        //                            && saudaorder.OilTypeId == item.OilTypeId && saudaorder.Incoterms2 == item.IncotermsId && saudaorder.PlantId == item.PlantId
                        //                            select saudaorder
                        //                        ).ToList();

                        //        if (SaudaContext.Count > 1)
                        //        {
                        //            return _resultService.ErrorMessage(Constants.SaudaHoldMessage);
                        //        }
                        //    }
                        //}
                        //Id ReAssigned from PricingLive Table to SaudaOrder table
                        //item.PricingRecordId = pricingContext.Id;

                        decimal calculatedDiscount = (decimal)0;

                        if (item.DiscountTypeId == (int)DTO.Enums.SaudaDiscountType.Discount)
                        {
                            var userdiscount = _emamiContext.DiscountUsers.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId && a.ParentId != 0 && a.UserId == inputDto.LoginUserId && a.StateId == dealerContext.StateId &&
                            currentDate >= a.ValidFrom && currentDate <= a.ValidTo);
                            if (userdiscount != null)
                            {
                                if (item.DiscountAmountPerCase > userdiscount.ActualDiscount)
                                {

                                    //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                    // currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == userContext.CityId);

                                    //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                    // currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == userContext.CityId);

                                    //if(geodiscount == null)
                                    //{
                                    //    geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.OilTypeId == item.OilTypeId &&
                                    // currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == userContext.CityId);
                                    //}

                                    //if (geodiscount != null)
                                    //{
                                    //    if(skuContext.OilPackGroupTypeId != null)
                                    //    {
                                    //        if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                    //        {
                                    //            calculatedDiscount = geodiscount.ActualDiscount;
                                    //        }
                                    //        else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                    //        {
                                    //            calculatedDiscount = _resultService.CalculateAutomatedDiscount(geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                    //        }
                                    //    }

                                    //    if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                    //    {
                                    //        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    //    }
                                    //}
                                    //else if (item.DiscountAmountPerCase > 0)
                                    //{
                                    //    return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    //}

                                    // Direct SkuId match (unchanged query)
                                    var geodiscount = _emamiContext.DiscountGeography.AsNoTracking()
                                        .OrderByDescending(s => s.Id)
                                        .FirstOrDefault(a => a.SkuId == item.SkuId &&
                                            currentDate >= a.ValidFrom && currentDate <= a.ValidTo &&
                                            a.CityId == userContext.CityId);

                                    if (geodiscount != null)
                                    {
                                        // Direct sku match → no conversion
                                        calculatedDiscount = geodiscount.ActualDiscount;
                                    }
                                    else
                                    {
                                        // Fallback: same OilType AND same OilPackGroupType (join Skus to access OilPackGroupTypeId)
                                        geodiscount = (from a in _emamiContext.DiscountGeography.AsNoTracking()
                                                       join s in _emamiContext.Skus.AsNoTracking() on a.SkuId equals s.Id
                                                       where a.OilTypeId == item.OilTypeId
                                                          && currentDate >= a.ValidFrom && currentDate <= a.ValidTo
                                                          && a.CityId == userContext.CityId
                                                          && s.OilPackGroupTypeId == skuContext.OilPackGroupTypeId
                                                       orderby a.Id descending
                                                       select a).FirstOrDefault();

                                        if (geodiscount != null && skuContext.OilPackGroupTypeId != null)
                                        {
                                            if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                            {
                                                calculatedDiscount = geodiscount.ActualDiscount;
                                            }
                                            else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                            {
                                                calculatedDiscount = _resultService.CalculateAutomatedDiscount(
                                                    geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                            }
                                        }
                                    }

                                    // Existing post-lookup validation stays as-is:
                                    if (geodiscount != null)
                                    {
                                        if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                        {
                                            return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                        }
                                    }
                                    else if (item.DiscountAmountPerCase > 0)
                                    {
                                        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    }
                                }
                            }
                            else
                            {
                                //    var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                //currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == userContext.CityId);

                                //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                //      currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == userContext.CityId);

                                //if(geodiscount == null)
                                //{
                                //    geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.OilTypeId == item.OilTypeId &&
                                //      currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == userContext.CityId);
                                //}

                                //if (geodiscount != null)
                                //{
                                //    if (skuContext.OilPackGroupTypeId != null)
                                //    {
                                //        if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                //        {
                                //            calculatedDiscount = geodiscount.ActualDiscount;
                                //        }
                                //        else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                //        {
                                //            calculatedDiscount = _resultService.CalculateAutomatedDiscount(geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                //        }
                                //    }

                                //    if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                //    {
                                //        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                //    }
                                //}
                                //else if (item.DiscountAmountPerCase > 0)
                                //{
                                //    return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                //}

                                // Direct SkuId match (unchanged query)
                                var geodiscount = _emamiContext.DiscountGeography.AsNoTracking()
                                    .OrderByDescending(s => s.Id)
                                    .FirstOrDefault(a => a.SkuId == item.SkuId &&
                                        currentDate >= a.ValidFrom && currentDate <= a.ValidTo &&
                                        a.CityId == userContext.CityId);

                                if (geodiscount != null)
                                {
                                    // Direct sku match → no conversion
                                    calculatedDiscount = geodiscount.ActualDiscount;
                                }
                                else
                                {
                                    // Fallback: same OilType AND same OilPackGroupType (join Skus to access OilPackGroupTypeId)
                                    geodiscount = (from a in _emamiContext.DiscountGeography.AsNoTracking()
                                                   join s in _emamiContext.Skus.AsNoTracking() on a.SkuId equals s.Id
                                                   where a.OilTypeId == item.OilTypeId
                                                      && currentDate >= a.ValidFrom && currentDate <= a.ValidTo
                                                      && a.CityId == userContext.CityId
                                                      && s.OilPackGroupTypeId == skuContext.OilPackGroupTypeId
                                                   orderby a.Id descending
                                                   select a).FirstOrDefault();

                                    if (geodiscount != null && skuContext.OilPackGroupTypeId != null)
                                    {
                                        if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                        {
                                            calculatedDiscount = geodiscount.ActualDiscount;
                                        }
                                        else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                        {
                                            calculatedDiscount = _resultService.CalculateAutomatedDiscount(
                                                geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                        }
                                    }
                                }

                                // Existing post-lookup validation stays as-is:
                                if (geodiscount != null)
                                {
                                    if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                    {
                                        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    }
                                }
                                else if (item.DiscountAmountPerCase > 0)
                                {
                                    return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                }
                            }
                        }
                    }

                }

                var statusId = (int)DTO.Enums.Status.Pending;
                /*  if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                  {
                      foreach (var item in inputDto.SaudaOrders)
                      {
                          var status = 0;
                          var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId);
                          if (pricingContext != null)
                          {
                              var cleranceRate = (decimal)0;
                              var baseRate = (decimal)0;
                              if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExDepot)
                              {
                                  cleranceRate = pricingContext.ExDepotPrice * pricingContext.CounterBidLimit;
                                  baseRate = pricingContext.ExDepotPrice;
                              }
                              else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExPlant)
                              {
                                  cleranceRate = pricingContext.ExPlantPrice * pricingContext.CounterBidLimit;
                                  baseRate = pricingContext.ExPlantPrice;
                              }
                              else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForDepot)
                              {
                                  cleranceRate = pricingContext.ForDepotPrice * pricingContext.CounterBidLimit;
                                  baseRate = pricingContext.ForDepotPrice;
                              }
                              else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForPlant)
                              {
                                  cleranceRate = pricingContext.ForPlantPrice * pricingContext.CounterBidLimit;
                                  baseRate = pricingContext.ForPlantPrice;
                              }
                              else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake)
                              {
                                  cleranceRate = pricingContext.ExRakePrice * pricingContext.CounterBidLimit;
                                  baseRate = pricingContext.ExRakePrice;
                              }
                              else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForRake)
                              {
                                  cleranceRate = pricingContext.ForRakePrice * pricingContext.CounterBidLimit;
                                  baseRate = pricingContext.ForRakePrice;
                              }

                              if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                              {
                                  if (item.BidPrice < cleranceRate)
                                      status = (int)DTO.Enums.Status.Rejected;
                                  else if (item.BidPrice >= cleranceRate && item.BidPrice <= baseRate)
                                      status = (int)DTO.Enums.Status.Hold;
                                  else if (item.BidPrice > baseRate)
                                      status = (int)DTO.Enums.Status.Pending;
                              }
                              else
                              {
                                  status = (int)DTO.Enums.Status.Pending;
                              }
                              item.StatusId = status;
                          }
                      }

                      if (inputDto.SaudaOrders.Any(_ => _.StatusId == (int)DTO.Enums.Status.Hold))
                          statusId = (int)DTO.Enums.Status.Hold;
                      else if (inputDto.SaudaOrders.Any(_ => _.StatusId == (int)DTO.Enums.Status.Rejected))
                          statusId = (int)DTO.Enums.Status.Rejected;

                  }
                  */

                long DealerTypeId = 0;
                string IncotermsType = string.Empty;
                long BrokerId = 0;
                var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
                if (dealerRole != null)
                {
                    DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
                    if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
                    {
                        BrokerId = inputDto.DealerId;
                    }
                    else
                    {
                        //var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
                        //                     join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
                        //                     where ur.RoleId == (int)DTO.Enums.Role.Broker
                        //                     && ucm.CustomerId == inputDto.DealerId
                        //                     select new
                        //                     {
                        //                         BrokerId = ucm.UserId
                        //                     }).FirstOrDefault();

                        //if (BrokerContext != null)
                        //{
                        BrokerId = inputDto.BrokerId;
                        //}
                    }
                }

                var saudaContext = new Sauda
                {
                    BdoId = inputDto.BDOId,
                    BiddingDate = currentDate,
                    UserId = inputDto.DealerId,
                    SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = currentDate,
                    IsSAPDataSync = false,
                    IsSAPDataSyncApproval = false,
                    SalesOrganizationId = inputDto.SalesOrganizationId,
                    DistributionChannelId = inputDto.DistributionChannelId,
                    DivisionId = inputDto.DivisionId,
                    SaudaType=inputDto.SaudaType
                };
                _emamiContext.Sauda.Add(saudaContext);
                _emamiContext.SaveChanges();

                //Sauda approval save
                //_emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId).ReportingToId ?? 0
                var saudaapprovalContext = new SaudaApproval
                {
                    RequestedBy = inputDto.LoginUserId,
                    RequestedTo = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId).ReportingToUserId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = currentDate,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    SaudaId = saudaContext.Id
                };
                _emamiContext.SaudaApproval.Add(saudaapprovalContext);
                _emamiContext.SaveChanges();

                List<long> saudaOrderIds = new List<long>();
                List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>();
                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {
                    int i = 0;
                    var skuIds = inputDto.SaudaOrders.Select(s => s.SkuId).ToList();
                    var skuUomMappingData = _emamiContext.SkuUomMapping.AsNoTracking()
                                         .Where(_ => skuIds.Contains(_.SkuId))
                                         .Select(s => new SkuUomMappingDto
                                         {
                                             Id = s.Id,
                                             ConversionFactor = s.ConversionFactor,
                                             ConversionFactor1 = s.ConversionFactor1,
                                             ConversionFactor2 = s.ConversionFactor2,
                                             SkuId = s.SkuId,
                                             UomId = s.UomId
                                         }).ToList();

                    var tpIds = inputDto.SaudaOrders.Select(s => s.PricingId).ToList();
                    var todayPricingContext = _emamiContext.TodayPricing.AsNoTracking().Where(tp => tpIds.Contains(tp.Id)).ToList();
                    foreach (var item in inputDto.SaudaOrders)
                    {

                        #region PricingLive to Pricing DataInsert and Rearrange the Pricing Data
                        ///Pricing Live is contain Current day Pricing
                        ///So, we insert the Pricing Live data into Pricing table for Sauda booked records only
                        /// Daily we cleanup and fresh data insert into the pricing live table
                        var pricingLiveContext = todayPricingContext.FirstOrDefault(_ => _.Id == item.PricingId);
                        //var pricingContext = default(Pricing);
                        long pricingId = 0;
                        if (pricingLiveContext == null)
                        {
                            return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
                        }
                        if (pricingLiveContext.PricingReferneceId == 0)
                        {
                            var pricing = new Pricing()
                            {
                                SkuId = pricingLiveContext.SkuId,
                                OilTypeId = pricingLiveContext.OilTypeId,
                                OilPackingTypeId = pricingLiveContext.OilPackingTypeId,
                                PlantId = pricingLiveContext.PlantId,
                                Price = pricingLiveContext.Price,
                                SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
                                DistributionChannelId = pricingLiveContext.DistributionChannelId,
                                DivisionId = pricingLiveContext.DivisionId,
                                SAPPricingCode = pricingLiveContext.SAPPricingCode,
                                ValidFrom = pricingLiveContext.ValidFrom,
                                ValidTo = pricingLiveContext.ValidTo,
                                CreatedBy = pricingLiveContext.CreatedBy,
                                CreatedDate = pricingLiveContext.CreatedDate,
                                ModifiedBy = pricingLiveContext.ModifiedBy,
                                ModifiedDate = pricingLiveContext.ModifiedDate,
                            };
                            _emamiContext.Pricing.Add(pricing);
                            _emamiContext.SaveChanges();
                            pricingId = pricing.Id;
                            /// Update pricingLive Record Pricing Reference Id
                            //var pricingLiveRecord = _emamiContext.TodayPricing.FirstOrDefault(s => s.Id == pricingLiveContext.Id);
                            pricingLiveContext.PricingReferneceId = pricing.Id;
                            _emamiContext.SaveChanges();
                            //pricingContext = pricing;
                        }
                        else
                        {
                            pricingId = pricingLiveContext.PricingReferneceId;
                            //pricingContext = _emamiContext.Pricing.FirstOrDefault(s => s.Id == pricingLiveContext.PricingReferneceId);
                        }

                        #endregion

                        DateTime? saudaValidFromDate = currentDate;
                       // long? depotIdForRake = 0;
                        if (item.SaudaValidFromDate != null)
                            saudaValidFromDate = item.SaudaValidFromDate;
                        //if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake || item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake)
                        //{
                        //    depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.PlantId && !_.IsPlant)?.DepotId;
                        //}

                        var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == item.IncotermsId).Name;
                        IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";
                        //if (!isDiscountAppliedZero)
                        //{
                        //    item.DiscountAmount = 0;
                        //}
                        //else
                        //{
                        //    item.DiscountAmount = item.BidQuantity * item.DiscountAmount;
                        //}
                        //item.DiscountAmount = item.BidQuantity * item.DiscountAmount;
                        decimal itemquotedprice = item.BidQuantity * item.QuotedPrice; // Here QuotedPrice is with Discount or Premium applied for BasePrice so only below formulas for discount and premium
                        item.QuotedPrice = itemquotedprice;
                        item.BidPrice = itemquotedprice;

                        if (item.DiscountTypeId == (int)DTO.Enums.SaudaDiscountType.Discount)
                        {
                            item.QuotedPrice = item.QuotedPrice + item.DiscountAmount;  // Discount
                        }
                        else
                        {
                            item.QuotedPrice = item.QuotedPrice - item.DiscountAmount;  // Premium
                        }

                        i = i + 10;
                        var saudaOrder = new SaudaOrder
                        {
                            SaudaId = saudaContext.Id,
                            SaudaNumber = i.ToString(),
                            SkuId = item.SkuId,
                            OilTypeId = item.OilTypeId,
                            BidPrice = item.BidPrice,
                            DiscountTypeId = item.DiscountTypeId,
                            DiscountAmount = item.DiscountAmount,
                            QPSDiscount = item.QPSDiscount,
                            BidQuantity = _resultService.ConvertCasetoMetricTonWithoutDB(item.BidQuantity, item.SkuId, skuUomMappingData),
                            BidQuantityCase = item.BidQuantity,
                            QuotedPrice = item.QuotedPrice,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = currentDate,
                            //BiddingwindowId = item.BiddingwindowId,
                            SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
                            PricingId = pricingId,
                            // DealerTypeId = DealerTypeId,
                            Incoterms1 = IncotermsType,
                            PlantId = item.PlantId,
                            //DealerLocationId = Convert.ToInt64(dealerContext.FreightRouteId),
                            // CustomerPONumber = dealerContext.Code + currentDate.ToShortDateString(),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = item.SaudaValidToDate != null ? item.SaudaValidToDate.Value : saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = statusId,
                            // SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = item.IncotermsId,
                            BrokerId = BrokerId,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            // DepotIdForRake = depotIdForRake.Value,
                            IsQuantityLimitForBookingSauda = IsQuantityLimitForBookingSauda,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            DivisionId = inputDto.DivisionId,
                            QuotedPriceBeforeSAPDiscount = item.BidQuantity == 0? 0m: item.BidPrice / item.BidQuantity
                        };
                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();

                        //if (dealerContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose)
                        //{
                        //    saudaOrderIds.Add(saudaOrder.Id);
                        //}

                        saudaCreateEmailList.Add(new SaudaCreateNotificationDto()
                        {
                            StatusId = item.StatusId,
                            SaudaOrderId = saudaOrder.Id,
                            SaudaBookingTypeId = saudaOrder.SaudaBookingTypeId,
                            SaudaOrderStatusId = saudaOrder.StatusId,
                            LoginUserId = inputDto.LoginUserId,
                            DealerId = inputDto.DealerId
                        });
                    }

                    //if (dealerContext.VerticalId == (int)DTO.Enums.LooseVertical.Loose)
                    //{
                    //    //method to sync Loose sauda from APP to SAP 
                    //    _sapIntegrationService.GetSaudaDetailsForLooseVertical(saudaOrderIds);
                    //}

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => SaudaCreateNotificationAsync(saudaCreateEmailList, cancellationToken));
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaContext.Id;
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

        public void SaudaCreateNotificationAsync(List<SaudaCreateNotificationDto> inputDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                using (AdaniContext _context = new AdaniContext())
                {
                    if (inputDto != null && inputDto.Any())
                    {
                        foreach (var saudaData in inputDto)
                        {
                            bool isEmail = false;

                            var DealerNotificationContext = _context.TPNotification.AsNoTracking().
                                                            Join(_context.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                            .Where(_ => _.TPND.DealerId == saudaData.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaCreation && _.TPND.IsActive).ToList();

                            var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                            if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                isEmail = true;
                            else
                                isEmail = false;

                            var usersContext = _context.Users.AsNoTracking().Where(_ => _.Id == saudaData.LoginUserId || _.Id == saudaData.DealerId);
                            var saudaOrderContext = _context.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.SaudaOrderId);
                            if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                var createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaData.LoginUserId);
                                var dealer = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.DealerId);
                                var reportingId = _context.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == saudaData.LoginUserId).ReportingToUserId;
                                var reportingTo = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == reportingId);
                                string dealerName = string.Empty;
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    dealerName = dealer.Name;
                                    toUsers.Add(dealer.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                string emailSubject = string.Empty;

                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var plainText = string.Empty;
                                    EmailTemplate emailTemplate = new EmailTemplate();
                                    if (saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                    {
                                        emailSubject = Constants.SaudaBookedSubject;
                                        emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationEmail);
                                    }
                                    else
                                    {
                                        if (saudaData.StatusId == (int)DTO.Enums.Status.Pending)
                                        {
                                            emailSubject = Constants.SaudaCreationRAFlowSubject;
                                            emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowEmail);
                                        }
                                        else if (saudaData.StatusId == (int)DTO.Enums.Status.Hold)
                                        {
                                            emailSubject = Constants.SaudaOnHoldSubject;
                                            emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationEmail);
                                        }
                                        else if (saudaData.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            emailSubject = Constants.SaudaRejectedSubject;
                                            emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
                                        }
                                    }
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                                var smsPlainTemplate = string.Empty;

                                bool isSms = false;
                                //var IsSMS = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsSMS).Select(_ => _.Value).Single();
                                var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                    isSms = true;
                                else
                                    isSms = false;

                                if (isSms)
                                {
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    if (saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                    {
                                        smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                                    }
                                    else
                                    {
                                        if (saudaData.SaudaOrderStatusId == (int)DTO.Enums.Status.Pending)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowSMS);


                                        }
                                        else if (saudaData.SaudaOrderStatusId == (int)DTO.Enums.Status.Hold)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationSMS);
                                        }
                                        else if (saudaData.SaudaOrderStatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
                                        }

                                        var statusContext = _context.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.SaudaOrderStatusId);
                                        var notificationContext = new Notifications
                                        {
                                            Request = DTO.Enums.NotificationRequest.Sauda.ToString(),
                                            RequestId = (int)DTO.Enums.NotificationRequest.Sauda,
                                            ReferenceId = saudaData.SaudaOrderId,
                                            Notification = statusContext != null ? statusContext.Name : string.Empty,
                                            StatusId = saudaData.SaudaOrderStatusId,
                                            CreatedBy = saudaData.SaudaOrderCreatedBy,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        };
                                        _context.Notifications.Add(notificationContext);
                                        _context.SaveChanges();
                                    }
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        try
                                        {
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }

                                bool isPushNotification = false;
                                //var IsPushNotification = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsPushNotification).Select(_ => _.Value).Single();
                                var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                    isPushNotification = true;
                                else
                                    isPushNotification = false;


                                //if (isPushNotification && saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                //{
                                if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = createdBy.PushTokenKey,
                                        RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                        Title = emailSubject,
                                        Message = smsPlainTemplate,
                                        //Id = saudaOrderContext.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                                if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = dealer.PushTokenKey,
                                        RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                        Title = emailSubject,
                                        Message = smsPlainTemplate,
                                        //Id = saudaOrderContext.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                                if (reportingTo != null && reportingTo.RegistrationTypeId != null && reportingTo.RegistrationTypeId > 0 && !string.IsNullOrEmpty(reportingTo.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = reportingTo.PushTokenKey,
                                        RegistrationTypeId = reportingTo.RegistrationTypeId != null ? (int)reportingTo.RegistrationTypeId : 0,
                                        Title = Constants.ApprovalRequest,
                                        Message = Constants.ApprovalRequestMessage,
                                        //Id = saudaOrderContext.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                                //}
                            }
                        }

                        #region Push Notification Nested Method
                        void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
                        {
                            try
                            {
                                var firebaseSenderId = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                                var pushNotifyServerkey = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                                var pushNotifyUrl = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

                                WebRequest tRequest = WebRequest.Create(pushNotifyUrl);
                                tRequest.Method = "post";
                                tRequest.ContentType = "application/json";
                                var json = new JavaScriptSerializer().Serialize(string.Empty);
                                if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.Android)
                                {
                                    var data = new
                                    {
                                        to = pushNotificationInputDto.PushTokenKey,
                                        data = new
                                        {
                                            sound = "default",
                                            message = pushNotificationInputDto.Message,
                                            title = pushNotificationInputDto.Title,
                                            id = pushNotificationInputDto.Id,
                                        },
                                        priority = "high"
                                    };
                                    json = new JavaScriptSerializer().Serialize(data);
                                }
                                else if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.IOS)
                                {
                                    var data = new
                                    {
                                        to = pushNotificationInputDto.PushTokenKey,
                                        data = new
                                        {
                                            sound = "default",
                                            message = pushNotificationInputDto.Message,
                                            title = pushNotificationInputDto.Title,
                                            id = pushNotificationInputDto.Id,
                                        },
                                        notification = new
                                        {
                                            title = pushNotificationInputDto.Title,
                                            body = pushNotificationInputDto.Message,
                                            id = pushNotificationInputDto.Id,
                                            sound = "default",
                                        },
                                        priority = "high"
                                    };
                                    json = new JavaScriptSerializer().Serialize(data);
                                }

                                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                                tRequest.Headers.Add(string.Format("Authorization: key={0}", pushNotifyServerkey));
                                tRequest.Headers.Add(string.Format("Sender: id={0}", firebaseSenderId));
                                tRequest.ContentLength = byteArray.Length;
                                using (Stream dataStream = tRequest.GetRequestStream())
                                {
                                    dataStream.Write(byteArray, 0, byteArray.Length);
                                    using (WebResponse tResponse = tRequest.GetResponse())
                                    {
                                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                                        {
                                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                            {
                                                String sResponseFromServer = tReader.ReadToEnd();
                                                string str = sResponseFromServer;
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
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public ResultDto GetSaudaorderdetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaorderdetails";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId);
                if (saudaOrderListContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.SaudaId);
                var totalBidAmount = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0;
                var totalBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidQuantity) ?? 0;
                var BrokerContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

                saudaDetails.SaudaNumber = saudaContext.Id.ToString();
                saudaDetails.SaudaDate = saudaContext.BiddingDate;
                saudaDetails.DealerId = saudaContext.UserId;
                saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
                saudaDetails.TotalAmount = totalBidAmount;
                saudaDetails.TotalQuantity = totalBidQuantity;
                saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
                saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);
                saudaDetails.BrokerId = BrokerContext.BrokerId;
                if (BrokerContext != null)
                {
                    saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
                }

                var saudaOrders = new List<SaudaOrderDetails>();

                var saudaOrderItem = new SaudaOrderDetails
                {
                    SkuId = saudaOrderListContext.SkuId,
                    SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.SkuId)?.SkuName,
                    BidPrice = saudaOrderListContext.BidPrice,
                    BidQuantity = saudaOrderListContext.BidQuantity,
                    BidQuantityCases = saudaOrderListContext.BidQuantityCase,
                    IncoTerms = saudaOrderListContext.Incoterms1,
                    Discount = saudaOrderListContext.DiscountAmount,
                    PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.PlantId)?.Name,
                    //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.DealerLocationId)?.Name,
                    DiscountTypeId = saudaOrderListContext.DiscountTypeId,
                    StatusId = saudaOrderListContext.StatusId,
                    Status = saudaOrderListContext.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.StatusId)?.Name,
                    SaudaConversionId = _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(_ => _.SaudaOrderId == saudaOrderListContext.Id) != null ? _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(_ => _.SaudaOrderId == saudaOrderListContext.Id).Id : 0,
                    Remarks = saudaOrderListContext.Remarks,
                };
                saudaOrders.Add(saudaOrderItem);

                saudaDetails.SaudaOrders = saudaOrders;


                if (saudaOrderListContext != null)
                {
                    //Dispatch status

                    IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderListContext.Id
                        && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);
                    if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
                    {
                        var liftingDetailView = new LiftingDetailViewDto();
                        liftingDetailView.CompletedQuantity = liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                        liftingDetailView.PendingQuantity = saudaOrderListContext.BidQuantity - liftingDetailView.CompletedQuantity;
                        liftingDetailView.PendingQuantityCase = saudaOrderListContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase);
                        liftingDetailView.LiftedSkus = liftingReqOrderContextList.Join(_emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Id == inputDto.SaudaOrderId),
                            lr => lr.SaudaOrderId, so => so.Id, (lr, so) => new { lr, so }).Select(_ => new SaudaOrderDetails
                            {
                                SkuId = _.so != null ? _.so.SkuId : 0,
                                SkuName = _.so != null && _.so.Sku != null ? _.so.Sku.SkuName : string.Empty,
                                BidQuantity = _.lr != null ? _.lr.LiftingQuantity : 0,
                                BidQuantityCases = _.lr != null ? _.lr.LiftingQuantityCase : 0,
                                LiftedDate = _.lr != null ? _.lr.CreatedDate : DateTime.MinValue,
                            }).ToList();
                        if (liftingDetailView != null && liftingDetailView.LiftedSkus != null && liftingDetailView.LiftedSkus.Any())
                        {
                            saudaDetails.LiftingDetails = liftingDetailView;
                        }
                    }

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaDetails;
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

        #region Lifting

        public ResultDto GetLiftingRequestCountList(LiftingRequestListInputDto inputDto)
        {
            _methodName = "GetLiftingRequestCountList";
            var outputDto = new List<LiftingRequestCountDto>();
            try
            {
                var StatusList = new List<long>();
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

                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
               .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    //New Reporting to table change
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                }
                if (bdoList != null && bdoList.Any())
                {

                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {

                        var sqlQuery = @"Create Table #DealerIdsTemp(DealerId bigint)
Create Table #BdoId(BdoId bigint)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

if(@BdoId > 0)
begin 
 insert into #BdoId(BdoId) select @BdoId
end
else
begin
	 insert into #BdoId(BdoId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
end

insert into #DealerIdsTemp(DealerId) select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoId)

select 
u.Id as DealerId,
Max(u.Name) as Dealer,
Count(DISTINCT lr.Id) as TotalLiftingCount
from LiftingRequests lr with(NOLOCK)
left join LiftingRequestDetails ld with(NOLOCK) on lr.Id=ld.LiftingRequestId
join #UserDivision ud on ud.SalesOrganizationId=ld.SalesOrganizationId
and ud.DistributionChannelId=ld.DistributionhannelId and ud.DivisionId=ld.DivisionId
join Users u with(NOLOCK) on lr.UserId=u.Id
where 
lr.UserId in (select DealerId from #DealerIdsTemp)
and ((@StatusId=2 and lr.SAPDocumentNo is not null) or (@StatusId!=2 and lr.SAPDocumentNo is null) )
group by u.Id

drop table #UserDivision
drop table #BdoId
drop table #DealerIdsTemp";
                        outputDto = conn.Query<LiftingRequestCountDto>(sqlQuery, new
                        {
                            UserId = inputDto.LoginUserId,
                            BdoId=inputDto.BDOId,
                            StatusId=inputDto.StatusId
                        }).ToList();

                    }

                    //List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    //if (dealersList != null && dealersList.Any())
                    //{
                    //    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                    //    {
                    //        StatusList.Add(inputDto.StatusId);
                    //        StatusList.Add((int)DTO.Enums.Status.Completed);


                    //        var liftingdata = (from lr in _emamiContext.LiftingRequest.AsNoTracking()
                    //                           join lrd in _emamiContext.LiftingRequestDetails.AsNoTracking() on lr.Id equals lrd.LiftingRequestId
                    //                           //join s in _emamiContext.Sauda.AsNoTracking() on lrd.SaudaNumber equals s.SaudaNumber
                    //                           join ud in divisionslogieduser on new { SalesOrganizationId = lrd.SalesOrganizationId, DistributionChannelId = lrd.DistributionhannelId, DivisionId = lrd.DivisionId }
                    //                                equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                           join u in _emamiContext.Users.AsNoTracking() on lr.UserId equals u.Id
                    //                           where dealersList.Contains(lr.UserId)
                    //                           && !String.IsNullOrEmpty(lr.SAPDocumentNo)
                    //                           select new { DealerId = lr.UserId, Dealer = u.Name, CreatedBy = lr.CreatedBy, StatusId = lr.StatusId } into lifting
                    //                           group lifting by lifting.DealerId into lift
                    //                           select new { Key = lift.Key, Group = lift.ToList() }
                    //                         );

                    //        //var liftingdata = _emamiContext.LiftingRequest.AsNoTracking()
                    //        //    .Where(_ => dealersList.Contains(_.UserId) 
                    //        //    && /*StatusList.Contains(_.StatusId)*/ !String.IsNullOrEmpty(_.SAPDocumentNo))
                    //        //.Join(_emamiContext.Users.AsNoTracking(), lr => lr.UserId, u => u.Id, (lr, u) => new { DealerId = lr.UserId, Dealer = u.Name , CreatedBy = lr.CreatedBy , StatusId = lr.StatusId })
                    //        //.GroupBy(_ => _.DealerId).Select(s => new { Key = s.Key , group = s.ToList()}).ToList();

                    //        outputDto = liftingdata
                    //        .Select(_ => new LiftingRequestCountDto()
                    //        {
                    //            Dealer = _.Group.FirstOrDefault().Dealer,
                    //            DealerId = _.Group.FirstOrDefault().DealerId,
                    //            TotalLiftingCount = _.Group.Count(),
                    //        }).ToList();
                    //    }
                    //    else
                    //    {
                    //        StatusList.Add(inputDto.StatusId);

                    //        var liftingdata = (from lr in _emamiContext.LiftingRequest.AsNoTracking()
                    //                           join lrd in _emamiContext.LiftingRequestDetails.AsNoTracking() on lr.Id equals lrd.LiftingRequestId
                    //                           join s in _emamiContext.Sauda.AsNoTracking() on lrd.SaudaNumber equals s.SaudaNumber
                    //                           join u in _emamiContext.Users.AsNoTracking() on lr.UserId equals u.Id
                    //                           where dealersList.Contains(lr.UserId)
                    //                           && String.IsNullOrEmpty(lr.SAPDocumentNo)
                    //                           select new { DealerId = lr.UserId, Dealer = u.Name, CreatedBy = lr.CreatedBy, StatusId = lr.StatusId } into lifting
                    //                           group lifting by lifting.DealerId into lift
                    //                           select new { Key = lift.Key, Group = lift.ToList() }
                    //                        );

                    //        //var liftingdata = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && /*StatusList.Contains(_.StatusId)*/ String.IsNullOrEmpty(_.SAPDocumentNo))
                    //        //.Join(_emamiContext.Users.AsNoTracking(), lr => lr.UserId, u => u.Id, (lr, u) => new { DealerId = lr.UserId, Dealer = u.Name, CreatedBy = lr.CreatedBy, StatusId = lr.StatusId })
                    //        //.GroupBy(_ => _.DealerId).Select(s => new { Key = s.Key, group = s.ToList() }).ToList();

                    //        outputDto = liftingdata
                    //        .Select(_ => new LiftingRequestCountDto()
                    //        {
                    //            Dealer = _.Group.FirstOrDefault().Dealer,
                    //            DealerId = _.Group.FirstOrDefault().DealerId,
                    //            TotalLiftingCount = _.Group.Count(),
                    //            IsCreatedBy = _.Group.FirstOrDefault().CreatedBy != inputDto.LoginUserId && inputDto.StatusId == (int)DTO.Enums.Status.Pending ? false : true
                    //        }).ToList();
                    //    }

                    //}
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

        public ResultDto GetLiftingRequestListByBDO(LiftingRequestListInputDto inputDto)
        {
            _methodName = "GetLiftingRequestListByBDO";
            var outputDto = new List<LiftingRequestCountDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).Select(_ => _.CustomerId).ToList();
                if (dealersList != null && dealersList.Any())
                {
                    outputDto = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && _.StatusId == inputDto.StatusId)
                        .Join(_emamiContext.Users.AsNoTracking(), lr => lr.UserId, u => u.Id, (lr, u) => new { DealerId = lr.UserId, Dealer = u.Name })
                        .GroupBy(_ => _.DealerId)
                        .Select(_ => new LiftingRequestCountDto()
                        {
                            Dealer = _.FirstOrDefault().Dealer,
                            DealerId = _.FirstOrDefault().DealerId,
                            TotalLiftingCount = _.Count()
                        }).ToList();
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

        public ResultDto GetDealersLiftingRequestList(DealersLiftingRequestInputDto inputDto)
        {
            _methodName = "GetDealersLiftingRequestList";
            var outputDto = new List<DealersLiftingRequestOutputDto>();
            try
            {
                var StatusList = new List<long>();
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

                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
              .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                IEnumerable<DealersLiftingRequestOutputDto> PendingContractContext = new List<DealersLiftingRequestOutputDto>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var sqlQuery = @"Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId


select 
Max(lr.UserId) as DealerId,
Max(u.Name) as Dealer,
Max(lr.ShipToPartyId) as ShipTopartyId,
 (Case when Max(ship.Name) is null then '' else Max(ship.Name) end) as ShipToParty,
 ld.LiftingRequestId as LiftingRequestId,
max(lr.LiftingRequestNumber) as LiftingRequestNumber,
max(lr.LiftingDate) as LiftingRequestdate,
Sum(ld.LiftingQuantityCase) as RequestedQuantity,
max(lr.StatusId) as StatusID,
max(st.Name) as [Status],
'' as Remarks,
'' as CreatedUser
from LiftingRequests lr with(NOLOCK)
join LiftingRequestDetails ld with(NOLOCK) on lr.Id=ld.LiftingRequestId
left join Users ship on ship.Id=lr.ShipToPartyId
join Saudas s with(NOLOCK) on s.SaudaNumber=ld.SaudaNumber
join #UserDivision ud on ud.SalesOrganizationId=ld.SalesOrganizationId
and ud.DistributionChannelId=ld.DistributionhannelId and ud.DivisionId=ld.DivisionId
join Users u on lr.UserId=u.Id
join Status st on lr.StatusId=st.Id
where lr.UserId=@CustomerId and
((@IsFilter=1 
and (Cast(lr.CreatedDate as date) >= Cast(@FromDate as date)
and Cast(lr.CreatedDate as date) <= Cast(@ToDate as date))) OR @IsFilter = 0)
and lr.StatusId !=3 --Rejected
and lr.StatusId !=14 --Deleted
and ((@StatusId=2 and lr.SAPDocumentNo is not null) or lr.SAPDocumentNo is null )
group by ld.LiftingRequestId
order by ld.LiftingRequestId desc

drop table #UserDivision";

                    outputDto = conn.Query<DealersLiftingRequestOutputDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        CustomerId = inputDto.DealerId,
                        FromDate = inputDto.FromDate,
                        ToDate = inputDto.ToDate,
                        StatusId=inputDto.StatusId,
                        IsFilter=inputDto.IsFilter
                    }).ToList();
                }
                //    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                //{
                //    StatusList.Add(inputDto.StatusId);
                //    StatusList.Add((int)DTO.Enums.Status.Completed);

                //    outputDto = (from lr in _emamiContext.LiftingRequest.AsNoTracking()
                //                 join lrd in _emamiContext.LiftingRequestDetails.AsNoTracking() on lr.Id equals lrd.LiftingRequestId
                //                 //join s in _emamiContext.Sauda.AsNoTracking() on lrd.SaudaNumber equals s.SaudaNumber
                //                 join ud in divisionslogieduser on new { SalesOrganizationId = lrd.SalesOrganizationId, DistributionChannelId = lrd.DistributionhannelId, DivisionId = lrd.DivisionId }
                //                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                 join u in _emamiContext.Users.AsNoTracking() on lr.UserId equals u.Id
                //                 join ast in _emamiContext.ApprovalStatus.AsNoTracking() on lr.StatusId equals ast.Id
                //                 where lr.UserId == inputDto.DealerId
                //                 && (DbFunctions.TruncateTime(lr.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //                 && DbFunctions.TruncateTime(lr.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //                 && lr.StatusId != (int)DTO.Enums.Status.Rejected
                //                 && lr.StatusId != (int)DTO.Enums.Status.Deleted
                //                 && !String.IsNullOrEmpty(lr.SAPDocumentNo)
                //                 select new { lr, lrd, UserName = u.Name, StatusName = ast.Name } into lifting
                //                 group lifting by lifting.lrd.LiftingRequestId into lift
                //                 select new DealersLiftingRequestOutputDto()
                //                 {
                //                     DealerId = lift.FirstOrDefault().lr.UserId,
                //                     Dealer = lift.FirstOrDefault().UserName,
                //                     ShipToPartyId = lift.FirstOrDefault().lr.ShipToPartyId,
                //                     ShipToParty = lift.FirstOrDefault().lr.ShipToParty != null ? lift.FirstOrDefault().lr.ShipToParty.Name : string.Empty,
                //                     LiftingRequestId = lift.FirstOrDefault().lr.Id,
                //                     LiftingRequestNumber = lift.FirstOrDefault().lr.LiftingRequestNumber,
                //                     LiftingRequestdate = lift.FirstOrDefault().lr.LiftingDate,
                //                     RequestedQuantity = lift.Sum(s => s.lrd.LiftingQuantityCase),
                //                     StatusID = lift.FirstOrDefault().lr.StatusId,
                //                     Status = lift.FirstOrDefault().StatusName,
                //                     Remarks = string.Empty,
                //                     CreatedUser = string.Empty,
                //                 }
                //               ).OrderByDescending(_ => _.LiftingRequestId).ToList();

                //    // outputDto = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequest != null)
                //    //.Join(_emamiContext.Users.AsNoTracking(), lr => lr.LiftingRequest.UserId, u => u.Id, (lr, u) => new { lr, UserName = u.Name })
                //    //.Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.lr.LiftingRequest.StatusId, lrs => lrs.Id, (x, lrs) => new { x.lr, x.UserName, StatusName = lrs.Name })
                //    //.Where(_ => _.lr.LiftingRequest.UserId == inputDto.DealerId /*&& StatusList.Contains(_.lr.LiftingRequest.StatusId)*/ && (DbFunctions.TruncateTime(_.lr.LiftingRequest.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.lr.LiftingRequest.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //    //&& _.lr.StatusId != (int)DTO.Enums.Status.Rejected && _.lr.StatusId != (int)DTO.Enums.Status.Deleted && !String.IsNullOrEmpty(_.lr.LiftingRequest.SAPDocumentNo))
                //    //.GroupBy(_ => _.lr.LiftingRequestId)
                //    //.Select(_ => new DealersLiftingRequestOutputDto()
                //    //{
                //    //    DealerId = _.FirstOrDefault().lr.LiftingRequest.UserId,
                //    //    Dealer = _.FirstOrDefault().UserName,
                //    //    ShipToPartyId = _.FirstOrDefault().lr.LiftingRequest.ShipToPartyId,
                //    //    ShipToParty = _.FirstOrDefault().lr.LiftingRequest.ShipToParty != null ? _.FirstOrDefault().lr.LiftingRequest.ShipToParty.Name : string.Empty,
                //    //    LiftingRequestId = _.FirstOrDefault().lr.LiftingRequest.Id,
                //    //    LiftingRequestNumber = _.FirstOrDefault().lr.LiftingRequest.LiftingRequestNumber,
                //    //    LiftingRequestdate = _.FirstOrDefault().lr.LiftingRequest.LiftingDate,
                //    //    RequestedQuantity = _.Sum(s => s.lr.LiftingQuantityCase),
                //    //    StatusID = _.FirstOrDefault().lr.LiftingRequest.StatusId,
                //    //    Status = _.FirstOrDefault().StatusName,
                //    //    Remarks = string.Empty,
                //    //    CreatedUser = string.Empty,
                //    //}).OrderByDescending(_ => _.LiftingRequestId).ToList();
                //}
                //else
                //{
                //    StatusList.Add(inputDto.StatusId);
                //    outputDto = (from lr in _emamiContext.LiftingRequest.AsNoTracking()
                //                 join lrd in _emamiContext.LiftingRequestDetails.AsNoTracking() on lr.Id equals lrd.LiftingRequestId
                //                 join s in _emamiContext.Sauda.AsNoTracking() on lrd.SaudaNumber equals s.SaudaNumber
                //                 join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                 join u in _emamiContext.Users.AsNoTracking() on lr.UserId equals u.Id
                //                 join ast in _emamiContext.ApprovalStatus.AsNoTracking() on lr.StatusId equals ast.Id
                //                 where lrd.LiftingRequest.UserId == inputDto.DealerId
                //                   && (DbFunctions.TruncateTime(lrd.LiftingRequest.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //                   && DbFunctions.TruncateTime(lrd.LiftingRequest.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //                   && lrd.StatusId != (int)DTO.Enums.Status.Rejected
                //                   && lrd.StatusId != (int)DTO.Enums.Status.Deleted
                //                   && String.IsNullOrEmpty(lrd.LiftingRequest.SAPDocumentNo)
                //                 select new { lr, lrd, UserName = u.Name, StatusName = ast.Name } into lifting
                //                 group lifting by lifting.lrd.LiftingRequestId into lift
                //                 select new DealersLiftingRequestOutputDto()
                //                 {
                //                     DealerId = lift.FirstOrDefault().lr.UserId,
                //                     Dealer = lift.FirstOrDefault().UserName,
                //                     ShipToPartyId = lift.FirstOrDefault().lr.ShipToPartyId,
                //                     ShipToParty = lift.FirstOrDefault().lr.ShipToParty != null ? lift.FirstOrDefault().lr.ShipToParty.Name : string.Empty,
                //                     LiftingRequestId = lift.FirstOrDefault().lr.Id,
                //                     LiftingRequestNumber = lift.FirstOrDefault().lr.LiftingRequestNumber,
                //                     LiftingRequestdate = lift.FirstOrDefault().lr.LiftingDate,
                //                     RequestedQuantity = lift.Sum(s => s.lrd.LiftingQuantityCase),
                //                     StatusID = lift.FirstOrDefault().lr.StatusId,
                //                     Status = lift.FirstOrDefault().StatusName,
                //                     Remarks = string.Empty,
                //                     CreatedUser = string.Empty,
                //                     IsCreatedBy = lift.FirstOrDefault().lr.CreatedBy != inputDto.LoginUserId && lift.FirstOrDefault().lr.StatusId == (int)DTO.Enums.Status.Pending ? true : false
                //                 }
                //               ).OrderByDescending(_ => _.LiftingRequestId).ToList();



                //    // outputDto = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequest != null)
                //    //.Join(_emamiContext.Users.AsNoTracking(), lr => lr.LiftingRequest.UserId, u => u.Id, (lr, u) => new { lr, UserName = u.Name })
                //    //.Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.lr.LiftingRequest.StatusId, lrs => lrs.Id, (x, lrs) => new { x.lr, x.UserName, StatusName = lrs.Name })
                //    //.Where(_ => _.lr.LiftingRequest.UserId == inputDto.DealerId 
                //    //&& (DbFunctions.TruncateTime(_.lr.LiftingRequest.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) 
                //    //&& DbFunctions.TruncateTime(_.lr.LiftingRequest.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //    //&& _.lr.StatusId != (int)DTO.Enums.Status.Rejected 
                //    //&& _.lr.StatusId != (int)DTO.Enums.Status.Deleted 
                //    //&& String.IsNullOrEmpty(_.lr.LiftingRequest.SAPDocumentNo)
                //    //)
                //    //.GroupBy(_ => _.lr.LiftingRequestId)
                //    //.Select(_ => new DealersLiftingRequestOutputDto()
                //    //{
                //    //    DealerId = _.FirstOrDefault().lr.LiftingRequest.UserId,
                //    //    Dealer = _.FirstOrDefault().UserName,
                //    //    ShipToPartyId = _.FirstOrDefault().lr.LiftingRequest.ShipToPartyId,
                //    //    ShipToParty = _.FirstOrDefault().lr.LiftingRequest.ShipToParty != null ? _.FirstOrDefault().lr.LiftingRequest.ShipToParty.Name : string.Empty,
                //    //    LiftingRequestId = _.FirstOrDefault().lr.LiftingRequest.Id,
                //    //    LiftingRequestNumber = _.FirstOrDefault().lr.LiftingRequest.LiftingRequestNumber,
                //    //    LiftingRequestdate = _.FirstOrDefault().lr.LiftingRequest.LiftingDate,
                //    //    RequestedQuantity = _.Sum(s => s.lr.LiftingQuantityCase),
                //    //    StatusID = _.FirstOrDefault().lr.LiftingRequest.StatusId,
                //    //    Status = _.FirstOrDefault().StatusName,
                //    //    Remarks = string.Empty,
                //    //    CreatedUser = string.Empty,
                //    //    IsCreatedBy = _.FirstOrDefault().lr.LiftingRequest.CreatedBy != inputDto.LoginUserId && _.FirstOrDefault().lr.LiftingRequest.StatusId == (int)DTO.Enums.Status.Pending ? true : false
                //    //}).OrderByDescending(_ => _.LiftingRequestId).ToList();
                //}


                return _resultService.SuccessObject(outputDto);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto LiftingRequestApproval(LiftingRequestStatusChangeDto inputDto)
        {
            _methodName = "LiftingRequestApproval";
            var resultDto = new ResultDto();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                LiftingRequestNotificationDto liftingRequestNotificationDto = new LiftingRequestNotificationDto();
                List<LiftingRequestSkuDto> liftingRequestSkuList = new List<LiftingRequestSkuDto>();

                var liftingContext = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == inputDto.Id);
                if (liftingContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                liftingContext.StatusId = inputDto.StatusId;
                liftingContext.ApproverRemarks = inputDto.Remarks;
                liftingContext.ModifiedBy = inputDto.LoginUserId;
                liftingContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                var skuUOMMapping = _emamiContext.SkuUomMapping.AsNoTracking();
                var uom = _emamiContext.Uom.AsNoTracking();
                var liftingdetails = _emamiContext.LiftingRequestDetails.AsNoTracking()
                    .Where(_ => _.LiftingRequestId == inputDto.Id)
                    .Select(s => new
                    {
                        Sku = s.Sku.SkuName,
                        QtyInCase = s.LiftingQuantityCase,
                        UOM = uom.FirstOrDefault(u => u.Id == skuUOMMapping.FirstOrDefault(sku => sku.SkuId == s.SkuId).UomId).Name
                    });

                bool IsReprocess = false;
                List<long> LiftingRequestIds = new List<long>();
                LiftingRequestIds.Add(liftingContext.Id);
                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved /*&& ConsoleSettings.IsInboundDirectSyncToSapAllowed*/)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.GetLiftingRequestEnquiryNumberOutboundDetails(LiftingRequestIds, IsReprocess);
                    });
                }

                #region Notification details
                if (liftingdetails != null && liftingdetails.Any())
                {
                    long count = 1;
                    foreach (var liftingData in liftingdetails)
                    {
                        liftingRequestSkuList.Add(new LiftingRequestSkuDto()
                        {
                            ItemLine = count,
                            Sku = liftingData.Sku,
                            QtyInCase = liftingData.QtyInCase,
                            UOM = liftingData.UOM
                        });
                    }
                }

                liftingRequestNotificationDto.LiftingRequestNumber = liftingContext.Id.ToString();
                liftingRequestNotificationDto.RemarksFromApp = liftingContext.CustomerRemarks;
                liftingRequestNotificationDto.UserId = liftingContext.UserId;
                liftingRequestNotificationDto.CreatedBy = inputDto.LoginUserId;
                string cityName = _emamiContext.City.AsNoTracking().FirstOrDefault(f => f.Id == liftingContext.User.CityId)?.CityName;
                string districtName = _emamiContext.District.AsNoTracking().FirstOrDefault(f => f.Id == liftingContext.User.DistrictId)?.DistrictName;
                string stateName = _emamiContext.State.AsNoTracking().FirstOrDefault(f => f.Id == liftingContext.User.StateId)?.StateName;
                liftingRequestNotificationDto.BillToPartyName = liftingContext.User.Name;
                liftingRequestNotificationDto.BillToPartyPlace = $"{cityName},{districtName},{stateName}";
                if (liftingContext.ShipToParty != null && liftingContext.ShipToPartyId > 0)
                {
                    cityName = _emamiContext.City.AsNoTracking().FirstOrDefault(f => f.Id == liftingContext.ShipToParty.CityId)?.CityName;
                    districtName = _emamiContext.District.AsNoTracking().FirstOrDefault(f => f.Id == liftingContext.ShipToParty.DistrictId)?.DistrictName;
                    stateName = _emamiContext.State.AsNoTracking().FirstOrDefault(f => f.Id == liftingContext.ShipToParty.StateId)?.StateName;
                }
                else { cityName = ""; districtName = ""; stateName = ""; }

                liftingRequestNotificationDto.ShipToPartyName = liftingContext.ShipToParty != null ? liftingContext.ShipToParty.Name : "";
                liftingRequestNotificationDto.ShipToPartyPlace = string.IsNullOrEmpty(cityName) ? "" : $"{cityName},{districtName},{stateName}";
                liftingRequestNotificationDto.LiftingRequestNumber = liftingContext.Id.ToString();
                liftingRequestNotificationDto.LiftingRequestSkuDetails = liftingRequestSkuList;
                liftingRequestNotificationDto.APPIndentNoCreatedDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);

                #endregion

                try
                {
                    bool isEmail = false;

                    var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                    Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                    .Where(_ => _.TPND.DealerId == liftingContext.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.IndentRequestApproval && _.TPND.IsActive).ToList();

                    var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                    if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                        isEmail = true;
                    else
                        isEmail = false;

                    List<User> usersContext = new List<User>();
                    List<string> toUsers = new List<string>();
                    User createdBy = new User();
                    User dealer = new User();
                    if (liftingContext.CreatedBy == liftingContext.UserId)
                    {
                        usersContext = _emamiContext.Users.AsNoTracking().ToList();
                        if (usersContext != null && usersContext.Any())
                        {
                            createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingContext.CreatedBy);
                            var BdoForCorrespondingDealer = _emamiContext.UserCustomerMapping.AsNoTracking().FirstOrDefault(_ => _.CustomerId == liftingContext.CreatedBy).UserId;
                            var BdoContext = usersContext.FirstOrDefault(_ => _.Id == BdoForCorrespondingDealer);
                            if (createdBy != null)
                            {
                                toUsers.Add(createdBy.Email);
                            }
                            if (BdoContext != null)
                            {
                                toUsers.Add(BdoContext.Email);
                            }
                        }
                    }
                    else
                    {
                        usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == liftingContext.CreatedBy || _.Id == liftingContext.UserId).ToList();
                        if (usersContext != null && usersContext.Any())
                        {
                            createdBy = usersContext.FirstOrDefault(_ => _.Id == liftingContext.CreatedBy);
                            dealer = usersContext.FirstOrDefault(_ => _.Id == liftingContext.UserId);
                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                            {
                                toUsers.Add(createdBy.Email);
                            }
                            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                            {
                                toUsers.Add(dealer.Email);
                            }
                        }
                    }
                    if ((usersContext != null && usersContext.Any()) || createdBy != null)
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var emailSubject = string.Empty;
                        if (isEmail && toUsers != null && toUsers.Any())
                        {
                            var fromEmail = Constants.FromEmail;
                            var plainText = string.Empty;
                            EmailTemplate emailTemplate = new EmailTemplate();
                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestApprovalEmail);
                                emailSubject = Constants.LiftingRequestApprovalSubject;
                            }
                            if (emailTemplate != null)
                            {
                                var result = _notificationService.GenerateLiftingRequestEmailTemplate(liftingRequestNotificationDto);
                                var plainTemplate = emailTemplate.PlainTemplate;
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, result);
                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                            }

                        }
                        var smsPlainTemplate = string.Empty;
                        bool isSms = false;
                        var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                        if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                            isSms = true;
                        else
                            isSms = false;
                        if (isSms)
                        {
                            var smsMessage = string.Empty;
                            EmailTemplate smsTemplate = new EmailTemplate();
                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.LiftingRequestApprovalSMS);
                            }
                            if (smsTemplate != null)
                            {
                                var result = _notificationService.GenerateLiftingRequestSmsTemplate(liftingRequestNotificationDto);
                                smsPlainTemplate = smsTemplate.PlainTemplate;
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, result);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                                }
                            }
                        }
                        bool isPushNotification = false;
                        var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                        if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                            isPushNotification = true;
                        else
                            isPushNotification = false;
                        if (isPushNotification)
                        {
                            if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = createdBy.PushTokenKey,
                                    RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                    Title = emailSubject,
                                    Message = smsPlainTemplate,
                                    //Id = liftingContext.Id,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                            if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = dealer.PushTokenKey,
                                    RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                    Title = emailSubject,
                                    Message = smsPlainTemplate,
                                    //Id = liftingContext.Id,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = liftingContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Discount Allocation

        //Discount list
        public ResultDto GetEmployeeAndUserDiscountList(LoginUserIdDto inputDto)
        {
            _methodName = "GetEmployeeAndUserDiscountList";
            var discountUsers = new List<DiscountUserDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var discountcontext = _emamiContext.DiscountUsers.AsNoTracking()
                    .Where(w => w.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate);
                discountUsers = discountcontext.Where(s => s.ParentId == 0)
                    .Select(s => new DiscountUserDto()
                    {
                        Id = s.Id,
                        SkuId = s.SkuId,
                        SkuName = s.Sku != null ? s.Sku.SkuName : "",
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType != null ? s.OilType.Name : "",
                        ActualDiscount = s.ActualDiscount,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo,
                        SkuList = discountcontext.Where(a => a.ParentId == s.ParentId).Select(b => new DropDownDto() {
                            Id = b.SkuId,
                            Name = b.Sku != null ? b.Sku.SkuName : ""
                        }).ToList(),
                        UserList = discountcontext.Where(a => a.ParentId == s.ParentId).Select(b => new DropDownDto()
                        {
                            Id = b.UserId,
                            Name = b.User != null ? b.User.Name : ""
                        }).ToList(),
                      DiscountReason = s.DiscountReason
                    }).ToList();

                return _resultService.SuccessObject(discountUsers);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        //Assign discount
        public ResultDto AddEmployeeAndUserDiscount(EmployeeUserDiscountDto inputDto)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountData = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidTo.Date <= discountData.ValidTo.Date && inputDto.ValidTo.Date >= discountData.ValidFrom.Date))
                    {
                        return _resultService.ErrorMessage("Please select a Valid From and To date");
                    }

                    if (!(inputDto.ActualDiscount <= discountData.ActualDiscount))
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less than or equal to discount");
                    }

                    foreach (var userid in inputDto.CustomerId)
                    {
                        if (!isFirstRecord)
                        {
                            var parentDiscount = new DiscountUsers()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = inputDto.Id
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
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = inputDto.Id
                            };
                            _emamiContext.DiscountUsers.Add(discount);
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
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        //Update discount
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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountUserData = _emamiContext.DiscountUsers.AsNoTracking().Where(f => f.ParentDiscountId == inputDto.Id);
                if (discountUserData != null && discountUserData.Any())
                {
                    return _resultService.ErrorMessage(Constants.DiscountAlreadyProcessed);
                }

                var parentDiscountId = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id).ParentDiscountId;
                var discountData = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == parentDiscountId);

                if (inputDto.ActualDiscount > discountData.ActualDiscount)
                {
                    return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less then or equal to discount");
                }

                if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                && inputDto.ValidTo.Date <= discountData.ValidTo.Date && inputDto.ValidTo.Date >= discountData.ValidFrom.Date))
                {
                    return _resultService.ErrorMessage("Discount date range is " + discountData.ValidFrom.ToString("dd-MMM-yyyy") + " - " + discountData.ValidTo.ToString("dd-MMM-yyyy") + ". Please select dates between the range");
                }

                var discountDatas = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id).Select(s => new { SkuIds = s.SkuId, UserIds = s.UserId }).ToList();
                var dbEmployess = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.UserIds).Distinct().ToList() : null;

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
                    //foreach (var skuId in inputDto.SkuIds)
                    //{
                    foreach (var userID in newEmployees)
                    {
                        if (!isFirstRecord)
                        {
                            var entity = new DiscountUsers()
                            {
                                SkuId = inputDto.SkuId,
                                UserId = userID,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                OilTypeId = inputDto.OilTypeId,
                                ParentId = inputDto.Id,
                                ParentDiscountId = parentDiscountId
                            };
                            _emamiContext.DiscountUsers.Add(entity);
                            _emamiContext.SaveChanges();
                        }
                    }
                    //}
                }


                var discounts = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id || f.Id == inputDto.Id).ToList();
                if (discounts != null && discounts.Any())
                {
                    foreach (var discount in discounts)
                    {
                        discount.ActualDiscount = inputDto.ActualDiscount;
                        discount.ValidFrom = inputDto.ValidFrom;
                        discount.ValidTo = inputDto.ValidTo;
                        discount.ModifiedBy = inputDto.LoginUserId;
                        discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }

                resultDto.IsSuccess = true;
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        //Assigned discount list
        public ResultDto GetDiscountUserList(LoginUserIdDto inputDto)
        {
            _methodName = "GetDiscountUserList";
            var result = new List<DiscountUserParentChildDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                result = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.Status && w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId)
                .GroupJoin(_emamiContext.DiscountUsers.AsNoTracking().GroupJoin(_emamiContext.DiscountUsers.AsNoTracking(), x => x.Id, gc => gc.ParentDiscountId, (x, gc) => new { child = x, grandChildCount = gc.Count() }), x => x.Id, du => du.child.ParentId, (x, du) => new { parent = x, child = du, })
                .Join(_emamiContext.DiscountUsers.AsNoTracking(), x => x.parent.ParentDiscountId, p => p.Id, (x, p) => new { x.parent, x.child, grandparent = p })
                .OrderByDescending(o => o.parent.CreatedDate)
                .Select(s => new DiscountUserParentChildDto()
                {
                    Id = s.parent.Id,
                    SkuId = s.parent.SkuId,
                    SkuName = s.parent.Sku.SkuName,
                    SkuCode = s.parent.Sku.SkuCode,
                    OilTypeId = s.parent.OilTypeId,
                    OilTypeName = s.parent.OilType != null ? s.parent.OilType.Name : string.Empty,
                    ActualDiscount = s.grandparent.ActualDiscount,
                    ValidFrom = s.grandparent.ValidFrom,
                    ValidTo = s.grandparent.ValidTo,
                    ChildActualDiscount = s.parent.ActualDiscount,
                    ChildValidFrom = s.parent.ValidFrom,
                    ChildValidTo = s.parent.ValidTo,
                    AssignedUserDiscountList = s.child.Select(_ => new DiscountUserQuantityOutput()
                    {
                        Id = _.child.Id,
                        EmployeeId = _.child.UserId,
                        EmployeeName = _.child.User.Name,
                        Email = _.child.User.Email,
                        MobileNumber = _.child.User.MobileNumber,
                    }).ToList(),
                    IsProcessed = s.child.Where(w => w.grandChildCount > 0).Any(),
                }).ToList();

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetMultiselectDiscountList(LoginUserIdDto inputDto)
        {
            _methodName = "GetMultiselectDiscountList";
            var discountUsers = new List<DiscountUserDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                discountUsers = _emamiContext.DiscountUsers.AsNoTracking()
                    .Where(w => w.UserId == inputDto.LoginUserId && w.ParentId != 0 && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate)
                    .GroupBy(_ => _.ParentId)
                    .Select(s => new DiscountUserDto()
                    {
                        Id = s.FirstOrDefault().ParentId,
                        OilTypeId = s.FirstOrDefault().OilTypeId,
                        OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : "",
                        ActualDiscount = s.FirstOrDefault().ActualDiscount,
                        ValidFrom = s.FirstOrDefault().ValidFrom,
                        ValidTo = s.FirstOrDefault().ValidTo,
                        SkuDetails = s.Select(_ => new SkuOutputDto()
                        {
                            SkuId = _.SkuId,
                            Name = _.Sku != null ? _.Sku.SkuName : string.Empty,
                            PackGroupId = (_.Sku != null && _.Sku.PackGroupId != null) ? (long)_.Sku.PackGroupId : 0,
                            PackGroupName = (_.Sku != null && _.Sku.PackGroup != null) ? _.Sku.PackGroup.Name : string.Empty,
                            ParentId = _.Id,
                        }).ToList(),
                    }).ToList();

                return _resultService.SuccessObject(discountUsers);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignMultiselectDiscount(DiscountUserDto inputDto)
        {
            _methodName = "AssignMultiselectDiscount";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }
                if (inputDto.SkuDetails == null || !inputDto.SkuDetails.Any())
                {
                    return _resultService.ErrorMessage(Constants.SkuMissing);
                }

                var discountData = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidFrom.Date <= discountData.ValidTo.Date && inputDto.ValidFrom.Date >= discountData.ValidFrom.Date))
                    {
                        return _resultService.ErrorMessage("Please select a Valid From and To date");
                    }

                    if (!(inputDto.ActualDiscount <= discountData.ActualDiscount))
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less than or equal to discount");
                    }

                    foreach (var sku in inputDto.SkuDetails)
                    {
                        isFirstRecord = false;
                        parentId = 0;
                        if (!isFirstRecord)
                        {
                            var parentDiscount = new DiscountUsers()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = inputDto.CustomerId.FirstOrDefault(),
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = sku.ParentId
                            };
                            _emamiContext.DiscountUsers.Add(parentDiscount);
                            _emamiContext.SaveChanges();

                            parentId = parentDiscount.Id;
                            isFirstRecord = true;
                        }
                        foreach (var userid in inputDto.CustomerId)
                        {
                            var discount = new DiscountUsers()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = userid,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = sku.ParentId,
                            };
                            _emamiContext.DiscountUsers.Add(discount);
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
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto UpdateMultiselectDiscountUsers(DiscountUserDto inputDto)
        {
            _methodName = "UpdateDiscountUsers";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            var isExistsData = false;

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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }
                //if (inputDto.SkuIds == null || !inputDto.SkuIds.Any())
                //{
                //    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                //}

                var discountUserData = _emamiContext.DiscountUsers.AsNoTracking().Where(f => f.ParentDiscountId == inputDto.Id);
                if (discountUserData != null && discountUserData.Any())
                {
                    return _resultService.ErrorMessage(Constants.DiscountAlreadyProcessed);
                }

                var parentDiscountId = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id).ParentDiscountId;
                var discountData = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == parentDiscountId);

                if (inputDto.ActualDiscount > discountData.ActualDiscount)
                {
                    return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less then or equal to discount");
                }

                if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                && inputDto.ValidTo.Date <= discountData.ValidTo.Date && inputDto.ValidTo.Date >= discountData.ValidFrom.Date))
                {
                    return _resultService.ErrorMessage("Discount date range is " + discountData.ValidFrom.ToString("dd-MMM-yyyy") + " - " + discountData.ValidTo.ToString("dd-MMM-yyyy") + ". Please select dates between the range");
                }

                var discountDatas = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id).Select(s => new { SkuIds = s.SkuId, UserIds = s.UserId }).ToList();
                var dbEmployess = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.UserIds).Distinct().ToList() : null;

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
                    //foreach (var skuId in inputDto.SkuIds)
                    //{
                    foreach (var userID in newEmployees)
                    {
                        if (!isFirstRecord)
                        {
                            var entity = new DiscountUsers()
                            {
                                SkuId = inputDto.SkuId,
                                UserId = userID,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                OilTypeId = inputDto.OilTypeId,
                                ParentId = inputDto.Id,
                                ParentDiscountId = parentDiscountId
                            };
                            _emamiContext.DiscountUsers.Add(entity);
                            _emamiContext.SaveChanges();
                        }
                    }
                    //}
                }


                var discounts = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id || f.Id == inputDto.Id).ToList();
                if (discounts != null && discounts.Any())
                {
                    foreach (var discount in discounts)
                    {
                        discount.ActualDiscount = inputDto.ActualDiscount;
                        discount.ValidFrom = inputDto.ValidFrom;
                        discount.ValidTo = inputDto.ValidTo;
                        discount.ModifiedBy = inputDto.LoginUserId;
                        discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }

                resultDto.IsSuccess = true;
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignedMultiselectDiscountList(LoginUserIdDto inputDto)
        {
            _methodName = "AssignedDiscountList";
            var result = new List<DiscountUserParentChildDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                result = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.Status && w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId)
                .GroupJoin(_emamiContext.DiscountUsers.AsNoTracking().GroupJoin(_emamiContext.DiscountUsers.AsNoTracking(), x => x.Id, gc => gc.ParentDiscountId, (x, gc) => new { child = x, grandChildCount = gc.Count() }), x => x.Id, du => du.child.ParentId, (x, du) => new { parent = x, child = du, })
                .Join(_emamiContext.DiscountUsers.AsNoTracking(), x => x.parent.ParentDiscountId, p => p.Id, (x, p) => new { x.parent, x.child, grandparent = p })
                //.GroupJoin(_emamiContext.DiscountUsers.AsNoTracking().Where(_=>_.UserId==inputDto.LoginUserId), x => x.grandparent.ParentId, gps => gps.ParentId, (x, gps) => new { x.parent, x.child, x.grandparent, grandParentSiblings = gps })
                .OrderByDescending(o => o.parent.CreatedDate)
                .Select(s => new DiscountUserParentChildDto()
                {
                    Id = s.parent.Id,
                    OilTypeId = s.parent.OilTypeId,
                    OilTypeName = s.parent.OilType != null ? s.parent.OilType.Name : string.Empty,
                    ActualDiscount = s.grandparent.ActualDiscount,
                    ValidFrom = s.grandparent.ValidFrom,
                    ValidTo = s.grandparent.ValidTo,
                    //ChildActualDiscount = s.parent.ActualDiscount,
                    //ChildValidFrom = s.parent.ValidFrom,
                    //ChildValidTo = s.parent.ValidTo,
                    //ParentSkuList = s.grandParentSiblings.Select(_ => new SkuOutputDto()
                    //{
                    //    SkuId = _.SkuId,
                    //    Name = _.Sku != null ? _.Sku.SkuName : string.Empty,
                    //    PackGroupId = (_.Sku != null && _.Sku.PackGroupId != null) ? (long)_.Sku.PackGroupId : 0,
                    //    PackGroupName = (_.Sku != null && _.Sku.PackGroup != null) ? _.Sku.PackGroup.Name : string.Empty,
                    //    ParentId = _.Id,
                    //}).ToList(),
                    AssignedUserDiscountList = s.child.Select(_ => new DiscountUserQuantityOutput()
                    {
                        Id = _.child.Id,
                        EmployeeId = _.child.UserId,
                        EmployeeName = _.child.User.Name,
                        Email = _.child.User.Email,
                        MobileNumber = _.child.User.MobileNumber,
                        SkuId = _.child.SkuId,
                        SkuName = _.child.Sku != null ? _.child.Sku.SkuName : string.Empty,
                        Discount = _.child.ActualDiscount,
                        ValidFrom = _.child.ValidFrom,
                        ValidTo = _.child.ValidTo,
                    }).ToList(),
                    IsProcessed = s.child.Where(w => w.grandChildCount > 0).Any(),
                }).ToList();

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Sauda Limit

        public ResultDto SaudaLimitApproval(SaudaLimitRequestInputDto inputDto)
        {
            _methodName = "SaudaLimitApproval";
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                    .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                    && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                    && _.DivisionId == inputDto.DivisionId);

                var limitContext = _emamiContext.SaudaLimit.FirstOrDefault(_ => _.Id == inputDto.Id);
                if (limitContext != null)
                {
                    if (limitContext.StatusId == (int)DTO.Enums.Status.Pending)
                    {
                        limitContext.Remarks = inputDto.Remarks;
                        limitContext.StatusId = inputDto.StatusId;
                        limitContext.ModifiedBy = limitContext.CreatedBy;
                        limitContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        if (limitContext.StatusId == (int)DTO.Enums.Status.Approved)
                        {
                            var dealerContext = _emamiContext.Users.Where(_ => _.Id == limitContext.UserId).FirstOrDefault();
                            if (dealerContext != null)
                            {
                                limitContext.ActualLimit = userdivContext.SaudaLimit ?? 0;
                                limitContext.RequestedLimit = inputDto.RequestedLimitRequest;
                                userdivContext.SaudaLimit = userdivContext.SaudaLimit + inputDto.RequestedLimitRequest;
                            }
                            else
                            {
                                return _resultService.ErrorMessage(Constants.DealerNotFound);
                            }
                        }
                        _emamiContext.SaveChanges();

                        #region Notification

                        try
                        {
                            bool isEmail = false;
                            var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                            Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                            .Where(_ => _.TPND.DealerId == limitContext.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.LimitEnhancementRequestApproval && _.TPND.IsActive).ToList();

                            var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                            if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                isEmail = true;
                            else
                                isEmail = false;

                            List<User> usersContext = new List<User>();
                            List<string> toUsers = new List<string>();
                            User createdBy = new User();
                            User dealer = new User();
                            if (limitContext.CreatedBy == limitContext.UserId)
                            {
                                createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == limitContext.CreatedBy);
                                if (createdBy != null)
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                            }
                            else
                            {
                                usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == limitContext.CreatedBy || _.Id == limitContext.UserId).ToList();
                                if (usersContext != null && usersContext.Any())
                                {
                                    createdBy = usersContext.FirstOrDefault(_ => _.Id == limitContext.CreatedBy);
                                    dealer = usersContext.FirstOrDefault(_ => _.Id == limitContext.UserId);
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                    {
                                        toUsers.Add(createdBy.Email);
                                    }
                                    if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                    {
                                        toUsers.Add(dealer.Email);
                                    }
                                }
                            }

                            if ((usersContext != null && usersContext.Any()) || createdBy != null)
                            {
                                decimal actualLimit = limitContext.ActualLimit;
                                decimal extendedLimit = limitContext.ActualLimit + limitContext.RequestedLimit;
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                var emailSubject = string.Empty;
                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var plainText = string.Empty;

                                    EmailTemplate emailTemplate = new EmailTemplate();
                                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                    {
                                        emailSubject = Constants.SaudaLimitApprovalSubject;
                                        emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitApprovalEmail);
                                    }
                                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                    {
                                        emailSubject = Constants.SaudaLimitRejectSubject;
                                        emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitRejectEmail);
                                    }
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, actualLimit.ToString()).Replace(Constants.Quantity, extendedLimit.ToString()).Replace(Constants.CustomerName, dealer.Name);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                                var smsPlainTemplate = string.Empty;
                                bool isSms = false;
                                var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                    isSms = true;
                                else
                                    isSms = false;
                                if (isSms)
                                {

                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                    {
                                        smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitApprovalSMS);
                                    }
                                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                    {
                                        smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitRejectSMS);
                                    }
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.ContractQty, actualLimit.ToString()).Replace(Constants.Quantity, extendedLimit.ToString()).Replace(Constants.CustomerName, dealer.Name);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                    }
                                }
                                bool IsPushNotification = false;
                                var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                    IsPushNotification = true;
                                else
                                    IsPushNotification = false;
                                if (IsPushNotification)
                                {
                                    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = createdBy.PushTokenKey,
                                            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = smsPlainTemplate,
                                            //Id = limitContext.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = dealer.PushTokenKey,
                                            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = smsPlainTemplate,
                                            //Id = limitContext.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        #endregion

                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.SaudaLimitStatusAlreadyUpdated);
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                return _resultService.SuccessObject(1);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Sauda Conversion

        public ResultDto GetSaudaConversionList(SaudaFilterDto inputDto)
        {
            _methodName = "GetSaudaConversionList";
            var saudaConversionListDto = new List<SaudaShortViewDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    //New Reporting to table change
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.UserId).Select(_ => _.UserId).ToList();
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.UserId).Select(_ => _.Id).ToList();
                }
                if (bdoList != null && bdoList.Any())
                {
                    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    if (dealersList != null && dealersList.Any())
                    {
                        IQueryable<SaudaConversionOrder> saudaConvOrderListContext = _emamiContext.SaudaConversionOrder.AsNoTracking().Where(_ => _.SaudaConversion != null
                            && dealersList.Contains(_.SaudaConversion.DealerId) && _.SaudaConversion.IsConversion);
                        if (saudaConvOrderListContext != null && saudaConvOrderListContext.Any())
                        {
                            saudaConversionListDto = saudaConvOrderListContext.GroupBy(_ => _.SaudaConversionId).Select(_ => new SaudaShortViewDto
                            {
                                SaudaConversionId = _.FirstOrDefault().SaudaConversionId,
                                SaudaId = _.FirstOrDefault().SaudaId,
                                SaudaOrderId = _.FirstOrDefault().SaudaId,
                                BookedDate = _.FirstOrDefault().SaudaConversion.CreatedDate,
                                TotalQuantity = _.Sum(s => s.BidQuantityCase),
                                TotalAmount = _.Sum(s => s.BidPrice),
                                StatusId = _.FirstOrDefault().SaudaConversion.StatusId,
                                StatusName = _.FirstOrDefault().SaudaConversion.Status != null ? _.FirstOrDefault().SaudaConversion.Status.Name : string.Empty,
                                DealerId = _.FirstOrDefault().SaudaConversion.DealerId,
                                DealerName = _.FirstOrDefault().SaudaConversion.Dealer != null ? _.FirstOrDefault().SaudaConversion.Dealer.Name : string.Empty,
                                OilTypes = _.GroupBy(g => g.OilTypeId).Select(s => new SpecialRateOilTypeDto
                                {
                                    OilTypeId = s.FirstOrDefault().OilTypeId,
                                    OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : string.Empty,
                                    SkuCount = s.Count(),
                                }).ToList(),
                            }).ToList();
                        }
                    }
                }
                if (saudaConversionListDto != null && saudaConversionListDto.Any())
                {
                    return _resultService.SuccessObject(saudaConversionListDto);
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

        public ResultDto SaudaConversionApproval(SaudaConversionApprovalInputDto inputDto)
        {
            _methodName = "SaudaConversionApproval";
            var resultDto = new ResultDto();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var saudaConvertionContext = _emamiContext.SaudaConversion.FirstOrDefault(w => w.Id == inputDto.Id);
                if (saudaConvertionContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                if (saudaConvertionContext.StatusId == (int)DTO.Enums.Status.Pending || saudaConvertionContext.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                {
                    saudaConvertionContext.StatusId = inputDto.StatusId;
                    saudaConvertionContext.ModifiedBy = inputDto.LoginUserId;
                    saudaConvertionContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                    #region Reason
                    if (!string.IsNullOrEmpty(inputDto.Remarks))
                    {
                        var entity = new Remarks()
                        {
                            TableId = saudaConvertionContext.Id,
                            TableName = "SaudaConversion",
                            ReasonTypeId = inputDto.StatusId,
                            Description = inputDto.Remarks,
                            ModifiedBy = inputDto.LoginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.Remarks.Add(entity);
                    }
                    #endregion

                    _emamiContext.SaveChanges();

                    List<string> newSkuNameList = _emamiContext.SaudaConversionOrder.Where(w => w.SaudaConversionId == saudaConvertionContext.Id && w.Sku != null).Select(_ => _.Sku.SkuName).DefaultIfEmpty("").ToList();
                    string newSku = string.Empty;
                    string oldSku = string.Empty;
                    if (newSkuNameList != null && newSkuNameList.Any())
                    {
                        newSku = string.Join(", ", newSkuNameList.ToArray());
                    }
                    if (saudaConvertionContext.SaudaOrder != null && saudaConvertionContext.SaudaOrder.Sku != null)
                    {
                        oldSku = saudaConvertionContext.SaudaOrder.Sku.SkuName;
                    }

                    #region Notification

                    try
                    {
                        List<User> usersContext = new List<User>();
                        List<string> toUsers = new List<string>();
                        User createdBy = new User();
                        User dealer = new User();

                        if (saudaConvertionContext.CreatedBy == saudaConvertionContext.DealerId)
                        {
                            createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConvertionContext.CreatedBy);
                            if (createdBy != null)
                            {
                                toUsers.Add(createdBy.Email);
                            }
                        }
                        else
                        {
                            usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaConvertionContext.CreatedBy || _.Id == saudaConvertionContext.DealerId).ToList();
                            if (usersContext != null && usersContext.Any())
                            {
                                createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaConvertionContext.CreatedBy);
                                dealer = usersContext.FirstOrDefault(_ => _.Id == saudaConvertionContext.DealerId);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    toUsers.Add(dealer.Email);
                                }
                            }
                        }

                        if ((usersContext != null && usersContext.Any()) || createdBy != null)
                        {
                            bool isEmail = false;
                            var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                            Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                            .Where(_ => _.TPND.DealerId == saudaConvertionContext.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaConversionApproval && _.TPND.IsActive).ToList();

                            var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                            if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                isEmail = true;
                            else
                                isEmail = false;

                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            var emailSubject = string.Empty;
                            if (isEmail && toUsers != null && toUsers.Any())
                            {
                                var fromEmail = Constants.FromEmail;
                                var plainText = string.Empty;
                                EmailTemplate emailTemplate = new EmailTemplate();
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    emailSubject = Constants.SaudaConversionApprovalSubject;
                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionApprovalEmail);
                                }
                                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    emailSubject = Constants.SaudaConversionRejectSubject;
                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionRejectEmail);
                                }
                                if (emailTemplate != null)
                                {
                                    var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuOld, oldSku).Replace(Constants.SkuNew, newSku).Replace(Constants.CustomerName, dealer.Name);
                                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                }
                            }
                            var smsPlainTemplate = string.Empty;
                            bool isSms = false;
                            var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                            if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                isSms = true;
                            else
                                isSms = false;
                            if (isSms)
                            {
                                var smsMessage = string.Empty;
                                EmailTemplate smsTemplate = new EmailTemplate();
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionApprovalSMS);
                                }
                                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionRejectSMS);
                                }
                                if (smsTemplate != null)
                                {
                                    smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuOld, oldSku).Replace(Constants.SkuNew, newSku).Replace(Constants.CustomerName, dealer.Name);
                                    smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                    try
                                    {
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            bool IsPushNotification = false;
                            var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                            if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                IsPushNotification = true;
                            else
                                IsPushNotification = false;
                            if (IsPushNotification)
                            {
                                if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = createdBy.PushTokenKey,
                                        RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                        Title = emailSubject,
                                        Message = smsPlainTemplate,
                                        //Id = saudaOrderContext.Id,
                                    };
                                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                                if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = dealer.PushTokenKey,
                                        RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                        Title = emailSubject,
                                        Message = smsPlainTemplate,
                                        //Id = saudaOrderContext.Id,
                                    };
                                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SaudaConversionAlreadyUpdated);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = 1;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Sauda Extension

        public ResultDto GetSaudaExtensionList(SaudaFilterDto inputDto)
        {
            _methodName = "GetSaudaExtensionList";
            var saudaExtensionListDto = new List<SaudaExtensionListDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    //New Reporting to table change
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.UserId).Select(_ => _.UserId).ToList();
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.UserId).Select(_ => _.Id).ToList();
                }
                if (bdoList != null && bdoList.Any())
                {
                    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    if (dealersList != null && dealersList.Any())
                    {
                        var saudaConversionListContext = _emamiContext.SaudaConversion.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { sc, so })
                        .Where(_ => dealersList.Contains(_.sc.DealerId)
                          && _.sc.IsExtension == true);
                        if (saudaConversionListContext != null && saudaConversionListContext.Any())
                        {
                            saudaExtensionListDto = saudaConversionListContext.Select(_ => new SaudaExtensionListDto
                            {
                                SaudaConversionId = _.sc.Id,
                                SaudaId = _.sc.SaudaOrderId,
                                SaudaOrderId = _.sc.SaudaOrderId,
                                SaudaNumber = _.so.SaudaNumber,
                                ExpiryDate = _.sc.ExpiryDate,
                                ExtendToDate = _.sc.ExtendToDate,
                                StatusId = _.sc.ExtensionStatusId,
                                StatusName = _.sc.ExtensionStatusId != null ? _.sc.ExtensionStatus.Name : string.Empty,
                                DealerId = _.sc.DealerId,
                                DealerName = _.sc.Dealer != null ? _.sc.Dealer.Name : string.Empty,
                            }).Distinct().ToList();
                        }
                    }
                }
                if (saudaExtensionListDto != null && saudaExtensionListDto.Any())
                {
                    return _resultService.SuccessObject(saudaExtensionListDto);
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

        public ResultDto SaudaExtensionApproval(SaudaConversionApprovalInputDto inputDto)
        {
            _methodName = "SaudaExtensionApproval";
            var resultDto = new ResultDto();
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

                var saudaConvertion = _emamiContext.SaudaConversion.FirstOrDefault(w => w.Id == inputDto.Id);
                if (saudaConvertion == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                if (saudaConvertion.ExtensionStatusId == (int)DTO.Enums.Status.Pending || saudaConvertion.ExtensionStatusId == (int)DTO.Enums.Status.RequestForApproval)
                {
                    saudaConvertion.ExtensionStatusId = inputDto.StatusId;
                    saudaConvertion.ModifiedBy = inputDto.LoginUserId;
                    saudaConvertion.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                    #region Reason
                    if (!string.IsNullOrEmpty(inputDto.Remarks))
                    {
                        var entity = new Remarks()
                        {
                            TableId = saudaConvertion.Id,
                            TableName = "SaudaConversion",
                            ReasonTypeId = inputDto.StatusId,
                            Description = inputDto.Remarks,
                            ModifiedBy = inputDto.LoginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.Remarks.Add(entity);
                    }
                    #endregion
                    _emamiContext.SaveChanges();

                    var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConvertion.SaudaOrderId);

                    #region Notification

                    try
                    {
                        if (saudaOrderContext != null && saudaOrderContext.ValidToDate != null && saudaConvertion.ExtendToDate != null && saudaOrderContext.ValidToDate != DateTime.MinValue && saudaConvertion.ExtendToDate != DateTime.MinValue)
                        {
                            List<User> usersContext = new List<User>();
                            List<string> toUsers = new List<string>();
                            User createdBy = new User();
                            User dealer = new User();
                            if (saudaConvertion.CreatedBy == saudaConvertion.DealerId)
                            {
                                createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConvertion.CreatedBy);
                                if (createdBy != null)
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                            }
                            else
                            {
                                usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaConvertion.CreatedBy || _.Id == saudaConvertion.DealerId).ToList();
                                if (usersContext != null && usersContext.Any())
                                {
                                    createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaConvertion.CreatedBy);
                                    dealer = usersContext.FirstOrDefault(_ => _.Id == saudaConvertion.DealerId);
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                    {
                                        toUsers.Add(createdBy.Email);
                                    }
                                    if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                    {
                                        toUsers.Add(dealer.Email);
                                    }
                                }
                            }

                            if ((usersContext != null && usersContext.Any()) || createdBy != null)
                            {
                                string noOfDays = (saudaConvertion.ExtendToDate?.Date - saudaOrderContext.ValidToDate.Date).Value.TotalDays.ToString();
                                bool isEmail = false;
                                var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                                   Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                                   .Where(_ => _.TPND.DealerId == saudaConvertion.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaExtensionApproval && _.TPND.IsActive).ToList();

                                var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                                if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                    isEmail = true;
                                else
                                    isEmail = false;
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                var emailSubject = string.Empty;
                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var plainText = string.Empty;
                                    EmailTemplate emailTemplate = new EmailTemplate();
                                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                    {
                                        emailSubject = Constants.SaudaExtensionApprovalSubject;
                                        emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionApprovalNotificationEmail);
                                    }
                                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                    {
                                        emailSubject = Constants.SaudaExtensionRejectSubject;
                                        emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRequestNotificationEmail);
                                    }
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                                var smsPlainTemplate = string.Empty;
                                bool isSms = false;
                                var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                    isSms = true;
                                else
                                    isSms = false;
                                if (isSms)
                                {
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                    {
                                        smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionApprovalNotificationSMS);
                                    }
                                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                    {
                                        smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRejectNotificationSMS);
                                    }
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        try
                                        {
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                                bool IsPushNotification = false;
                                var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                    IsPushNotification = true;
                                else
                                    IsPushNotification = false;
                                if (IsPushNotification)
                                {
                                    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = createdBy.PushTokenKey,
                                            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = smsPlainTemplate,
                                            //Id = saudaOrderContext.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = dealer.PushTokenKey,
                                            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = smsPlainTemplate,
                                            //Id = saudaOrderContext.Id,
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

                    #endregion
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SaudaExtensionAlreadyUpdated);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = 1;
                return resultDto;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Premium Allocation

        public ResultDto GetPremiumList(LoginUserIdDto inputDto)
        {
            _methodName = "GetPremiumList";
            var premiumList = new List<PremiumUserDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                premiumList = _emamiContext.PremiumUser.AsNoTracking()
                    .Where(w => w.ParentId != 0 && w.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate)
                    .Select(s => new PremiumUserDto()
                    {
                        Id = s.Id,
                        SkuId = s.SkuId,
                        SkuName = s.Sku != null ? s.Sku.SkuName : "",
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType != null ? s.OilType.Name : "",
                        ActualPremium = s.ActualPremium,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList();

                return _resultService.SuccessObject(premiumList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignPremium(EmployeeUserPremiumDto inputDto)
        {
            _methodName = "AssignPremium";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountData = _emamiContext.PremiumUser.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {

                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidTo.Date <= discountData.ValidTo.Date && inputDto.ValidTo.Date >= discountData.ValidFrom.Date))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Please select a Valid From and To date";
                        return resultDto;
                    }

                    if (!(inputDto.ActualPremium <= discountData.ActualPremium))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Premium limit is " + discountData.ActualPremium + ". Please enter less than or equal to premium";
                        return resultDto;
                    }

                    foreach (var userid in inputDto.CustomerId)
                    {
                        if (!isFirstRecord)
                        {
                            var parentDiscount = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = inputDto.Id
                            };
                            _emamiContext.PremiumUser.Add(parentDiscount);
                            _emamiContext.SaveChanges();

                            parentId = parentDiscount.Id;
                            isFirstRecord = true;
                        }
                        if (isFirstRecord)
                        {
                            var discount = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = inputDto.Id
                            };
                            _emamiContext.PremiumUser.Add(discount);
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
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto UpdatePremium(PremiumUserDto inputDto)
        {
            _methodName = "UpdatePremium";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            bool isExistsData = false;
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var premiumUserData = _emamiContext.PremiumUser.AsNoTracking().Where(f => f.ParentPremiumId == inputDto.Id);
                if (premiumUserData != null && premiumUserData.Any())
                {
                    return _resultService.ErrorMessage(Constants.PremiumAlreadyProcessed);
                }

                var parentPremiumId = _emamiContext.PremiumUser.FirstOrDefault(f => f.Id == inputDto.Id).ParentPremiumId;
                var premiumData = _emamiContext.PremiumUser.FirstOrDefault(f => f.Id == parentPremiumId);

                if (!(inputDto.ValidFrom.Date >= premiumData.ValidFrom.Date && inputDto.ValidFrom.Date <= premiumData.ValidTo.Date
                && inputDto.ValidTo.Date <= premiumData.ValidTo.Date && inputDto.ValidTo.Date >= premiumData.ValidFrom.Date))
                {
                    return _resultService.ErrorMessage("Discount date range is " + premiumData.ValidFrom.ToString("dd-MMM-yyyy") + " - " + premiumData.ValidTo.ToString("dd-MMM-yyyy") + ". Please select dates between the range");
                }

                if (inputDto.ActualPremium <= premiumData.ActualPremium)
                {
                    var discountDatas = _emamiContext.PremiumUser.Where(f => f.ParentId == inputDto.Id).Select(s => new { SkuIds = s.SkuId, UserIds = s.UserId }).ToList();
                    var dbEmployess = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.UserIds).Distinct().ToList() : null;

                    //Get Removed Employees
                    var removedEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                        ? dbEmployess.Where(w => !inputDto.CustomerId.Contains(w)).Distinct().ToList() : null;

                    if (removedEmployees != null && removedEmployees.Any())
                    {
                        var removedData = _emamiContext.PremiumUser.Where(f => removedEmployees.Contains(f.UserId) && f.ParentId == inputDto.Id);
                        if (removedData != null)
                        {
                            removedData.ToList().ForEach(f => _emamiContext.PremiumUser.Remove(f));
                            _emamiContext.SaveChanges();
                        }
                    }

                    var newEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                        ? inputDto.CustomerId.Where(w => !dbEmployess.Contains(w)) : null;

                    if (newEmployees != null && newEmployees.Any())
                    {
                        foreach (var userID in newEmployees)
                        {
                            if (!isFirstRecord)
                            {
                                var entity = new PremiumUser()
                                {
                                    SkuId = inputDto.SkuId,
                                    UserId = userID,
                                    ActualPremium = inputDto.ActualPremium,
                                    ValidFrom = inputDto.ValidFrom,
                                    ValidTo = inputDto.ValidTo,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    OilTypeId = inputDto.OilTypeId,
                                    ParentId = inputDto.Id,
                                    ParentPremiumId = parentPremiumId
                                };
                                _emamiContext.PremiumUser.Add(entity);
                                _emamiContext.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    return _resultService.ErrorMessage("Premium limit is " + premiumData.ActualPremium + ". Please enter less then or equal to discount");
                }

                var premiums = _emamiContext.PremiumUser.Where(f => f.ParentId == inputDto.Id || f.Id == inputDto.Id).ToList();
                if (premiums != null && premiums.Any())
                {
                    foreach (var discount in premiums)
                    {
                        discount.ActualPremium = inputDto.ActualPremium;
                        discount.ValidFrom = inputDto.ValidFrom;
                        discount.ValidTo = inputDto.ValidTo;
                        discount.ModifiedBy = inputDto.LoginUserId;
                        discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }


                resultDto.IsSuccess = true;
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetAssignedPremiumList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAssignedPremiumList";
            var result = new List<PremiumUserParentChildDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                result = _emamiContext.PremiumUser.AsNoTracking().Where(w => w.ParentId == 0 && w.CreatedBy == inputDto.LoginUserId)
                .GroupJoin(_emamiContext.PremiumUser.AsNoTracking().GroupJoin(_emamiContext.PremiumUser.AsNoTracking(), x => x.Id, gc => gc.ParentPremiumId, (x, gc) => new { child = x, grandChildCount = gc.Count() }), x => x.Id, du => du.child.ParentId, (x, du) => new { parent = x, child = du })
                .Join(_emamiContext.PremiumUser.AsNoTracking(), x => x.parent.ParentPremiumId, p => p.Id, (x, p) => new { x.parent, x.child, grandparent = p })
                .OrderByDescending(o => o.parent.CreatedDate)
                .Select(s => new PremiumUserParentChildDto()
                {
                    Id = s.parent.Id,
                    SkuId = s.parent.SkuId,
                    SkuName = s.parent.Sku.SkuName,
                    SkuCode = s.parent.Sku.SkuCode,
                    OilTypeId = s.parent.OilTypeId,
                    OilTypeName = s.parent.OilType != null ? s.parent.OilType.Name : string.Empty,
                    ActualPremium = s.grandparent.ActualPremium,
                    ValidFrom = s.grandparent.ValidFrom,
                    ValidTo = s.grandparent.ValidTo,
                    ChildActualPremium = s.parent.ActualPremium,
                    ChildValidFrom = s.parent.ValidFrom,
                    ChildValidTo = s.parent.ValidTo,
                    AssignedUserPremiumList = s.child.Select(_ => new PremiumUserQuantityOutput()
                    {
                        Id = _.child.Id,
                        EmployeeId = _.child.UserId,
                        EmployeeName = _.child.User.Name,
                        Email = _.child.User.Email,
                        MobileNumber = _.child.User.MobileNumber,
                    }).ToList(),
                    IsProcessed = s.child.Where(w => w.grandChildCount > 0).Any(),
                }).ToList();

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetMultiselectPremiumList(LoginUserIdDto inputDto)
        {
            _methodName = "GetMultiselectPremiumList";
            var premiumUsers = new List<PremiumUserDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                premiumUsers = _emamiContext.PremiumUser.AsNoTracking()
                    .Where(w => w.UserId == inputDto.LoginUserId && w.ParentId != 0 && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate)
                    .GroupBy(_ => _.ParentId)
                    .Select(s => new PremiumUserDto()
                    {
                        Id = s.FirstOrDefault().ParentId,
                        OilTypeId = s.FirstOrDefault().OilTypeId,
                        OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : "",
                        ActualPremium = s.FirstOrDefault().ActualPremium,
                        ValidFrom = s.FirstOrDefault().ValidFrom,
                        ValidTo = s.FirstOrDefault().ValidTo,
                        SkuDetails = s.Select(_ => new SkuOutputDto()
                        {
                            SkuId = _.SkuId,
                            Name = _.Sku != null ? _.Sku.SkuName : string.Empty,
                            PackGroupId = (_.Sku != null && _.Sku.PackGroupId != null) ? (long)_.Sku.PackGroupId : 0,
                            PackGroupName = (_.Sku != null && _.Sku.PackGroup != null) ? _.Sku.PackGroup.Name : string.Empty,
                            ParentId = _.Id,
                        }).ToList(),
                    }).ToList();

                return _resultService.SuccessObject(premiumUsers);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignMultiselectPremium(PremiumUserDto inputDto)
        {
            _methodName = "AssignMultiselectPremium";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }
                if (inputDto.SkuDetails == null || !inputDto.SkuDetails.Any())
                {
                    return _resultService.ErrorMessage(Constants.SkuMissing);
                }

                var discountData = _emamiContext.PremiumUser.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidFrom.Date <= discountData.ValidTo.Date && inputDto.ValidFrom.Date >= discountData.ValidFrom.Date))
                    {
                        return _resultService.ErrorMessage("Please select a Valid From and To date");
                    }

                    if (!(inputDto.ActualPremium <= discountData.ActualPremium))
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountData.ActualPremium + ". Please enter less than or equal to discount");
                    }

                    foreach (var sku in inputDto.SkuDetails)
                    {
                        isFirstRecord = false;
                        parentId = 0;
                        if (!isFirstRecord)
                        {
                            var parentPremium = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = inputDto.CustomerId.FirstOrDefault(),
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = sku.ParentId
                            };
                            _emamiContext.PremiumUser.Add(parentPremium);
                            _emamiContext.SaveChanges();

                            parentId = parentPremium.Id;
                            isFirstRecord = true;
                        }
                        foreach (var userid in inputDto.CustomerId)
                        {
                            var premium = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = userid,
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = sku.ParentId,
                            };
                            _emamiContext.PremiumUser.Add(premium);
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
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Speciality Fat Quantity Allocation

        public ResultDto GetSpecialityFatQuantityLimitList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSpecialityFatQuantityList";
            var quantityLimitList = new List<SpecialityFatDiscountUserDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                quantityLimitList = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.UserId == inputDto.LoginUserId //&& w.ParentQuantityId != 0
                    && w.Sku != null && w.OilType != null
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate) //&& w.DivisionId == userContext.DivisionId.Value
                    ).OrderByDescending(o => o.CreatedDate).Select(s => new SpecialityFatDiscountUserDto()
                    {
                        Id = s.Id,
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType.Name,
                        QuantityLimit = s.ActualDiscount,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo,
                        RemainingQuantity = s.RemainingQuantity
                    }).ToList();
                return _resultService.SuccessObject(quantityLimitList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignSpecialityFatQuantityLimit(SpecialityFatEmployeeDiscountDto inputDto)
        {
            _methodName = "AssignSpecialityFatQuantityLimit";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountData = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    #region Validation

                    var userId = inputDto.CustomerId;
                    var details = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .Where(w => w.OilTypeId == inputDto.OilTypeId && w.SkuId == inputDto.SkuId // && (w.Id == inputDto.Id && w.ParentId == inputDto.Id)
                    && userId.Contains(w.UserId)
                    && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))
                    || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))));

                    var notWithinCurrentDiscount = details.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.UserId).ToList();
                    if (notWithinCurrentDiscount != null && notWithinCurrentDiscount.Any() && notWithinCurrentDiscount.Count > 0)
                    {
                        var userName = _emamiContext.Users.AsNoTracking().Where(w => notWithinCurrentDiscount.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                        return _resultService.ErrorMessage(Constants.QtyLimitAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)));
                    }

                    #endregion

                    if (!(inputDto.EmpValidFrom.Date >= discountData.ValidFrom.Date && inputDto.EmpValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.EmpValidTo.Date <= discountData.ValidTo.Date && inputDto.EmpValidTo.Date >= discountData.ValidFrom.Date))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Please select a Valid From and Valid To date";
                        return resultDto;
                    }
                    decimal totalQuantity = 0;
                    totalQuantity = inputDto.EmpActualDiscount * inputDto.CustomerId.Count();
                    if (!(totalQuantity <= discountData.ActualDiscount))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Actual quantity limit is " + discountData.ActualDiscount + ". Please enter less than or equal to quantity";
                        return resultDto;
                    }

                    foreach (var userid in inputDto.CustomerId)
                    {
                        var result = new SpecalityFatDiscountUser()
                        {
                            OilTypeId = inputDto.OilTypeId,
                            SkuId = inputDto.SkuId,
                            UserId = userid,
                            ActualDiscount = inputDto.EmpActualDiscount,
                            ParentId = parentId,
                            ParentQuantityId = discountData.Id,
                            RemainingQuantity = inputDto.EmpActualDiscount,
                            ValidFrom = inputDto.EmpValidFrom,
                            ValidTo = inputDto.EmpValidTo,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            //DivisionId = userContext.DivisionId ?? 0
                        };

                        _emamiContext.SpecalityFatDiscountUsers.Add(result);
                        if (!isFirstRecord)
                        {
                            isFirstRecord = true;
                            _emamiContext.SaveChanges();
                            parentId = result.Id;
                        }
                    }

                    //Update remaining quantity
                    discountData.RemainingQuantity = discountData.ActualDiscount - totalQuantity;

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
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
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

        public ResultDto UpdateAssignedSpecialityFatQuantityLimit(SpecialityFatDiscountUserDto inputDto)
        {
            _methodName = "UpdateAssignedSpecialityFatQuantityLimit";
            var resultDto = new ResultDto();

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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                decimal assignedQuantity = 0;
                assignedQuantity = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                            .Where(w => w.ParentQuantityId == inputDto.Id).Select(s => s.ActualDiscount).DefaultIfEmpty(0).Sum();

                var specalityFatData = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(w => w.Id == inputDto.Id);
                if (specalityFatData != null)
                {

                    if (specalityFatData.ParentQuantityId == 0)
                    {
                        if (inputDto.QuantityLimit >= assignedQuantity)
                        {
                            specalityFatData.ActualDiscount = inputDto.QuantityLimit;
                            specalityFatData.RemainingQuantity = inputDto.QuantityLimit - assignedQuantity;
                            specalityFatData.ModifiedBy = inputDto.LoginUserId;
                            specalityFatData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
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
                                parentAssignedQuantity.RemainingQuantity = parentAssignedQuantity.RemainingQuantity - extraQuantity;
                                parentAssignedQuantity.ModifiedBy = inputDto.LoginUserId;
                                parentAssignedQuantity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                                specalityFatData.ActualDiscount = inputDto.QuantityLimit;
                                specalityFatData.RemainingQuantity = specalityFatData.RemainingQuantity + extraQuantity;
                                specalityFatData.ModifiedBy = inputDto.LoginUserId;
                                specalityFatData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SaveChanges();
                            }
                            else
                            {
                                return _resultService.ErrorMessage(Constants.QtyLimitExceeded);
                            }
                        }
                        else
                        {
                            if (inputDto.QuantityLimit >= assignedQuantity)
                            {
                                parentAssignedQuantity.RemainingQuantity = parentAssignedQuantity.RemainingQuantity - extraQuantity;
                                parentAssignedQuantity.ModifiedBy = inputDto.LoginUserId;
                                parentAssignedQuantity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                                specalityFatData.ActualDiscount = inputDto.QuantityLimit;
                                specalityFatData.RemainingQuantity = specalityFatData.RemainingQuantity + extraQuantity;
                                specalityFatData.ModifiedBy = inputDto.LoginUserId;
                                specalityFatData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SaveChanges();
                            }
                            else
                            {
                                return _resultService.ErrorMessage(specalityFatData.User.Name + " Total quantity is " + specalityFatData.ActualDiscount + ". Used quantity is " + assignedQuantity + ". Total quantity is should be greater then or equal to assigned quantity");
                            }
                        }
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                try
                {
                    SpecialityFatLimitNotification(inputDto);
                }
                catch (Exception)
                {
                }
                resultDto.IsSuccess = true;
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetAssignedSpecialityFatQuantityLimitList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAssignedSpecialityFatQuantityLimitList";
            var result = new List<SpecialityFatQuantityLimitParentChildDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                result = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.CreatedBy == inputDto.LoginUserId //&& w.DivisionId == userContext.DivisionId.Value
                )
                .Join(_emamiContext.SpecalityFatDiscountUsers.AsNoTracking(), x => x.ParentQuantityId, p => p.Id, (x, p) => new { child = x, parent = p })
                .OrderByDescending(o => o.parent.CreatedDate)
                .Select(s => new SpecialityFatQuantityLimitParentChildDto()
                {
                    Id = s.child.Id,
                    SkuId = s.child.SkuId,
                    SkuName = s.child.Sku.SkuName,
                    SkuCode = s.child.Sku.SkuCode,
                    OilTypeId = s.child.OilTypeId,
                    OilTypeName = s.child.OilType != null ? s.child.OilType.Name : string.Empty,
                    ActualQuantity = s.parent.ActualDiscount,
                    RemainingQuantity = s.parent.RemainingQuantity,
                    ValidFrom = s.parent.ValidFrom,
                    ValidTo = s.parent.ValidTo,
                    ChildActualQuantity = s.child.ActualDiscount,
                    ChildValidFrom = s.child.ValidFrom,
                    ChildValidTo = s.child.ValidTo,
                    EmployeeId = s.child.UserId,
                    EmployeeName = s.child.User != null ? s.child.User.Name : string.Empty,
                    Email = s.child.User.Email,
                    MobileNumber = s.child.User != null ? s.child.User.MobileNumber : string.Empty,
                }).ToList();

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Speciality Fat Quantity Request

        public ResultDto AddSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto)
        {
            _methodName = "AddSpecialtyFatQuantityRequests";
            var resultDto = new ResultDto();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var entity = new SpecialtyFatQuantityRequest
                {
                    SkuId = inputDto.SkuId,
                    Quantity = inputDto.Quantity,
                    OilTypeId = inputDto.OiltypeId,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    SpecialtyFatQuantityLimitId = inputDto.SpecialtyFatQuantityLimitId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateTime.Now,
                    //DivisionId = userContext.DivisionId.Value
                };
                _emamiContext.SpecialtyFatQuantityRequests.Add(entity);
                _emamiContext.SaveChanges();

                var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                {
                    UserId = inputDto.LoginUserId,
                    SpecialtyFatQuantityRequestId = entity.Id,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialtyFatQuantityRequestsList(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsList";
            var resultDto = new ResultDto();
            List<SpecialtyFatQuantityRequestDto> outputDto = new List<SpecialtyFatQuantityRequestDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                outputDto = (from sfu in _emamiContext.SpecialtyFatQuantityRequestUserDetails
                             join sf in _emamiContext.SpecialtyFatQuantityRequests on sfu.SpecialtyFatQuantityRequestId equals sf.Id
                             join us in _emamiContext.Users on sf.CreatedBy equals us.Id
                             where sfu.UserId == inputDto.LoginUserId
                             //&& sf.DivisionId == userContext.DivisionId.Value
                             orderby sf.Id
                             select new SpecialtyFatQuantityRequestDto
                             {
                                 Id = sf.Id,
                                 UserId = sfu.UserId,
                                 UserName = sfu.User.Name,
                                 SkuId = sf.SkuId,
                                 SkuName = sf.Sku.SkuName,
                                 SkuCode = sf.Sku.SkuCode,
                                 Quantity = sf.Quantity,
                                 Status = sf.Status != null ? sf.Status.Name : string.Empty,
                                 StatusId = sf.StatusId,
                                 OiltypeId = sf.OilTypeId,
                                 OilTypeName = sf.OilType.Name,
                                 CreatedBy = us.Name,
                             }).ToList();

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }
        //Incoming requests
        public ResultDto GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId";
            var resultDto = new ResultDto();
            var specialtyFatQuantityRequestsList = new List<SpecialtyFatQuantityRequestDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                //New Reporting to table change
                //var userList = _emamiContext.Users.AsNoTracking().Where(w => w.ReportingToId == inputDto.LoginUserId).ToList();
                var userList = (from u in _emamiContext.Users.AsNoTracking()                 
                 join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                 where urm.ReportingToUserId == inputDto.LoginUserId 
                 select new User()
                 {
                     Id = u.Id,
                     Name = u.Name
                 }).ToList();

                specialtyFatQuantityRequestsList = (from us in userList
                                                    join sfu in _emamiContext.SpecialtyFatQuantityRequestUserDetails on us.Id equals sfu.UserId
                                                    join sf in _emamiContext.SpecialtyFatQuantityRequests on sfu.SpecialtyFatQuantityRequestId equals sf.Id
                                                    join createus in _emamiContext.Users on sf.CreatedBy equals createus.Id
                                                    join sfd in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking() on sf.SpecialtyFatQuantityLimitId equals sfd.Id
                                                    join parentDiscount in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking() on sfd.ParentQuantityId equals parentDiscount.Id
                                                    //where sf.DivisionId == userContext.DivisionId.Value
                                                    orderby sf.Id
                                                    select new SpecialtyFatQuantityRequestDto
                                                    {
                                                        Id = sf.Id,
                                                        UserId = sfu.UserId,
                                                        UserName = sfu.User.Name,
                                                        SkuId = sf.SkuId,
                                                        SkuName = sf.Sku.SkuName,
                                                        SkuCode = sf.Sku.SkuCode,
                                                        Quantity = sf.Quantity,
                                                        Status = sf.Status != null ? sf.Status.Name : string.Empty,
                                                        StatusId = sf.StatusId,
                                                        OiltypeId = sf.OilTypeId,
                                                        OilTypeName = sf.OilType.Name,
                                                        CreatedBy = createus.Name,
                                                        SpecialtyFatQuantityRequestId = sfu.SpecialtyFatQuantityRequestId,
                                                        IsRequestedUser = ((sf.Id == sfu.SpecialtyFatQuantityRequestId && inputDto.LoginUserId == sfu.UserId) ? true : false),
                                                        IsApprove = parentDiscount.RemainingQuantity > sf.Quantity ? true : false,
                                                        //RemainingQuantity = sfuqusub.RemainingQuantity
                                                    }).ToList();

                //if (specialtyFatQuantityRequestsList != null && specialtyFatQuantityRequestsList.Any())
                //{
                //    specialtyFatQuantityRequestsList.ForEach(f =>
                //    {
                //        f.IsRequestedUser = _emamiContext.SpecialtyFatQuantityRequestUserDetails.Any(w => w.SpecialtyFatQuantityRequestId == f.Id && w.UserId == inputDto.LoginUserId);
                //    });
                //}

                //specialtyFatQuantityRequestsList = specialtyFatQuantityRequestsList.Where(sp => sp.StatusId == inputDto.StatusId).ToList();

                return _resultService.SuccessObject(specialtyFatQuantityRequestsList);
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
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (usersContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var specialtyFatQuantityRequests = _emamiContext.SpecialtyFatQuantityRequests.FirstOrDefault(w => w.Id == inputDto.Id);
                if (specialtyFatQuantityRequests == null)
                {
                    return _resultService.ErrorMessage(Constants.SpecialtyFatQuantityRequestsNotFound);
                }
                // else
                //{
                if (specialtyFatQuantityRequests.StatusId == (int)DTO.Enums.Status.Pending || specialtyFatQuantityRequests.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                {
                    var specalityFatDiscountUsers = _emamiContext.SpecalityFatDiscountUsers
                        .FirstOrDefault(w => w.Id == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId);
                    if (specalityFatDiscountUsers != null)
                    {
                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                        {
                            var specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers
                        .FirstOrDefault(w => w.Id == specalityFatDiscountUsers.ParentQuantityId);
                            if (specalityFatRemainingQty != null)
                            {
                                remainingQuantity = specalityFatRemainingQty.RemainingQuantity;
                                if (remainingQuantity == 0 && specalityFatRemainingQty.ParentQuantityId > 0)
                                {
                                    if (inputDto.RoleId == (int)(DTO.Enums.Role.NationalTrader))
                                    {
                                        specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers
                            .FirstOrDefault(w => w.Id == specalityFatRemainingQty.ParentQuantityId);
                                    }
                                }

                                if (specialtyFatQuantityRequests.Quantity <= specalityFatRemainingQty.RemainingQuantity)
                                {
                                    if (specalityFatDiscountUsers != null && inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                    {
                                        specalityFatDiscountUsers.ActualDiscount = specalityFatDiscountUsers.ActualDiscount + specialtyFatQuantityRequests.Quantity;
                                        specalityFatDiscountUsers.RemainingQuantity = specalityFatDiscountUsers.RemainingQuantity + specialtyFatQuantityRequests.Quantity;
                                        specalityFatRemainingQty.RemainingQuantity = specalityFatRemainingQty.RemainingQuantity - specialtyFatQuantityRequests.Quantity;
                                    }

                                    specialtyFatQuantityRequests.StatusId = inputDto.StatusId;
                                    specialtyFatQuantityRequests.Remarks = inputDto.Remarks;
                                    specialtyFatQuantityRequests.ModifiedBy = inputDto.LoginUserId;
                                    specialtyFatQuantityRequests.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    //_emamiContext.SaveChanges();
                                    var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                                    {
                                        UserId = inputDto.LoginUserId,
                                        SpecialtyFatQuantityRequestId = specialtyFatQuantityRequests.Id,
                                        StatusId = inputDto.StatusId,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                    };
                                    _emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
                                    _emamiContext.SaveChanges();
                                }
                                else
                                {
                                    var userName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == specialtyFatQuantityRequests.CreatedBy).Name;
                                    errorMessage.Append("USER : " + userName + " | SKU : " + specialtyFatQuantityRequests.Sku.SkuName + "<br>");
                                }
                            }
                            else
                            {
                                resultDto = _resultService.ErrorMessage(Constants.RecordNotFound);
                            }
                        }
                        else
                        {
                            //var specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers
                            //.FirstOrDefault(w => w.Id == specalityFatDiscountUsers.ParentQuantityId);
                            if (specalityFatDiscountUsers != null && inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                specalityFatDiscountUsers.ActualDiscount = (specalityFatDiscountUsers.ActualDiscount + specialtyFatQuantityRequests.Quantity);
                                specalityFatDiscountUsers.RemainingQuantity = (specalityFatDiscountUsers.RemainingQuantity + specialtyFatQuantityRequests.Quantity);
                            }

                            specialtyFatQuantityRequests.StatusId = inputDto.StatusId;
                            specialtyFatQuantityRequests.Remarks = inputDto.Remarks;
                            specialtyFatQuantityRequests.ModifiedBy = inputDto.LoginUserId;
                            specialtyFatQuantityRequests.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            //_emamiContext.SaveChanges();
                            var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                            {
                                UserId = inputDto.LoginUserId,
                                SpecialtyFatQuantityRequestId = specialtyFatQuantityRequests.Id,
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
                    //}



                    if (!string.IsNullOrEmpty(errorMessage.ToString()))
                    {
                        errorMessage.Append("Above users not approved. Your remaining quantity is " + remainingQuantity + ".</br>");
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
                            var requestedLimitContext = _emamiContext.SpecialtyFatQuantityRequests.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.Id);
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
                        catch (Exception ex)
                        {

                        }

                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SpecialtyFatQuantityRequestAlreadyUpdated);
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

        public ResultDto GetPendingContractChartMobile(LoginZHId inputDto)
        {
            _methodName = "GetPendingContractChartMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                }
                if (bdoList != null && bdoList.Any())
                {
                    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    if (dealersList != null && dealersList.Any())
                    {
                        var saudaStatus = Constants.OutstandingSaudaStatus;
                        //var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                        //    .Join(_emamiContext.Users.AsNoTracking(), x => x.s.UserId, u => u.Id, (x, u) => new { x.so, x.s, u })
                        //    .Join(_emamiContext.PendingContracts.AsNoTracking(), x => x.so.Id, pc => pc.SaudaOrderId, (x, pc) => new { x.so, x.s, x.u, pc })
                        //    .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.so, x.s, x.pc, DealerName = x.u.Name, CityName = c.CityName })
                        //    .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.so.StatusId, ss => ss.Id, (x, ss) => new { x.so, x.s, x.pc, x.DealerName, x.CityName, StatusName = ss.Name })
                        //    .Where(_ => dealersList.Contains(_.s.UserId) && saudaStatus.Contains(_.so.StatusId) && _.s != null && _.so != null && _.so.OilType != null);

                        var saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking()
                            .Join(_emamiContext.Users.AsNoTracking(), x => x.CustomerCode, u => u.Code, (x, u) => new { x, u })
                            .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, x.u, DealerName = x.u.Name, CityName = c.CityName/*, VerticalId = x.u.DivisionId*/ })
                            .Join(_emamiContext.Skus.AsNoTracking(), x => x.x.MaterialCode, sku => sku.SkuCode, (x, sku) => new { x.x, x.u, DealerName = x.u.Name, CityName = x.CityName/*, VerticalId = x.u.DivisionId*/, sku })
                            .Where(_ => dealersList.Contains(_.u.Id) &&
                            _.x.SalesOrgId == _.sku.SalesOrganizationId && _.x.DistChnlId == _.sku.DistributionChannelId &&
                                                  _.x.DivisionId == _.sku.DivisionId

                            //&& _.u.DivisionId == userContext.DivisionId 
                            //&& _.sku.DivisionId == _.VerticalId
                            ).ToList();

                        if (saudaOrdersContext != null && saudaOrdersContext.Any())
                        {
                            saudaListDto = saudaOrdersContext.Select(_ => new SaudaListDto()
                            {
                                Id = _emamiContext.Sauda.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault() != null
                                    && _emamiContext.Sauda.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().SaudaNumber != null ? _emamiContext.Sauda.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().Id : 0,
                                //SaudaOrderId = _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == _.x.SaudaNumber).SaudaNumber != null ? _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == _.x.SaudaNumber).Id : 0,
                                //SaudaOrderId = _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == ).FirstOrDefault() != null
                                //    && _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().SaudaNumber != null ? _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().Id : 0,
                                UserId = _.u.Id,
                                User = _.DealerName,
                                City = _.CityName,
                                BiddingDate = _emamiContext.Sauda.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault() != null
                                    && _emamiContext.Sauda.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().SaudaNumber != null ? _emamiContext.Sauda.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().BiddingDate : DateTime.Now,
                                TotalBidPrice = _.x.BasicRate,
                                TotalBidQuantity = _.x.SaudaQuantity,
                                OiltypeName = _.sku.OilType.Name
                            }).ToList();
                        }
                    }
                }
                if (saudaListDto != null && saudaListDto.Any())
                {
                    return _resultService.SuccessObject(saudaListDto);
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

        public ResultDto GetCreditLimitAndCreditExposureList(CreditLimitAndCreditExposureInputDto inputDto)
        {
            _methodName = "GetCreditLimitAndCreditExposureList";
            var resultDto = new ResultDto();
            var CreditLimitAndCreditExposureListDto = new List<CreditLimitAndCreditExposureOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.IsActive);
                var dealerIds = new List<long>();
                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (roleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                //Dealer 
                if (inputDto.DealerIds.IsAny())
                {
                    dealerIds = inputDto.DealerIds;
                }
                //StateTrader
                else if (inputDto.DealerIds == null && inputDto.BdoIds.IsAny())
                {
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => inputDto.BdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                //ZonalTrader
                else if (inputDto.DealerIds == null && inputDto.BdoIds == null)
                {
                    //New Reporting to table change
                    var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    //var bdoIds = usersContext.Where(user => user.ReportingToId == inputDto.LoginUserId).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                else if (inputDto.DealerIds == null && inputDto.BdoIds == null && inputDto.ZonalHeadIds.IsAny())
                {
                    //New Reporting to table change
                    var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => inputDto.ZonalHeadIds.Contains(user.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    //var bdoIds = usersContext.Where(user => inputDto.ZonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                else if (inputDto.DealerIds == null && inputDto.BdoIds == null && inputDto.ZonalHeadIds == null && inputDto.NationalHeadIds.IsAny())
                {
                    //New Reporting to table change
                    var zonalHeadIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => inputDto.NationalHeadIds.Contains(user.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => zonalHeadIds.Contains(user.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    //var zonalHeadIds = usersContext.Where(user => inputDto.NationalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    //var bdoIds = usersContext.Where(user => zonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                else if (inputDto.DealerIds == null && inputDto.BdoIds == null && inputDto.ZonalHeadIds == null && inputDto.NationalHeadIds == null)
                {
                    var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(roles => roles.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(a => a.UserId).ToList();
                    //New Reporting to table change
                    var zonalHeadIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => nationalHeadIds.Contains(user.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => zonalHeadIds.Contains(user.ReportingToUserId)).Select(_ => _.UserId).ToList();

                    //var zonalHeadIds = usersContext.Where(user => nationalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    //var bdoIds = usersContext.Where(user => zonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (roleContext.RoleId == (int)DTO.Enums.Role.Admin)
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


                dealerIds = usersContext.Where(_ => dealerIds.Contains(_.Id) && _.IsActive).Select(_ => _.Id).ToList();
                var userDivisionMappings = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId)).ToList();
                var salesOrgs = _emamiContext.SalesOrganization.AsNoTracking().ToList();
                var distChans = _emamiContext.DistributionChannel.AsNoTracking().ToList();
                var divisions = _emamiContext.Divisions.AsNoTracking().ToList();

                var userCreditMasterContext = (from ucm in _emamiContext.UserCreditMaster.AsNoTracking()
                                               //join ud in divisionslogieduser on new { SalesOrganizationId = ucm.SalesOrgId, DistributionChannelId = ucm.DistChnlId, DivisionId = ucm.DivisionId }
                                               //      equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                               where dealerIds.Contains(ucm.UserId)
                                               group ucm by ucm.UserId into ucredit
                                               select new {Id=ucredit.Key,value=ucredit.OrderByDescending(_ => _.CreatedDate).FirstOrDefault()}
                );

                //var userCreditMasterContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId));
                //var accountNumberList = userCreditMasterContext.Where(_ => dealerIds.Contains(_.UserId) && _.Isactive && _.CreditAccountNumber != null).Select(s => s.CreditAccountNumber).Distinct().ToList();
                if (userCreditMasterContext.IsAny())
                {
                    if (inputDto.CreditId == (int)DTO.Enums.CreditId.CreditLimit)
                    {
                        //var creditLimitContext = userCreditMasterContext.Where(_ => accountNumberList.Contains(_.CreditAccountNumber) && _.Isactive).GroupBy(accountno => accountno.CreditAccountNumber).Select(group => new
                        //{
                        //    CreditAccountNumber = group.Key,
                        //    CreditLimitList = group
                        //}).ToList();

                        //foreach (var data in creditLimitContext)
                        //{
                        //var dealerVerticalId = data.CreditLimitList.FirstOrDefault(user => user.CreditAccountNumber == data.CreditAccountNumber
                        //&& dealerIds.Contains(user.UserId));

                        //var dealerCCArea = _emamiContext.DivisionDetails.AsNoTracking().FirstOrDefault(_ => _.DivisionId == dealerVerticalId).CCArea ?? "";

                        var creditLimit = userCreditMasterContext.ToList().AsEnumerable().Select(s => new CreditLimitAndCreditExposureOutputDto()
                        {
                            DealerCode = s.value.User.Code,
                            DealerName = s.value.User.Name,
                            CreditAccountNumber = string.Concat(salesOrgs.FirstOrDefault(_ => _.Id == s.value.SalesOrgId) != null ? salesOrgs.FirstOrDefault(_ => _.Id == s.value.SalesOrgId).Name : string.Empty, " / ",
                                    distChans.FirstOrDefault(_ => _.Id == s.value.DistChnlId) != null ? distChans.FirstOrDefault(_ => _.Id == s.value.DistChnlId).Name : string.Empty, " / ",
                                    divisions.FirstOrDefault(_ => _.Id == s.value.DivisionId) != null ? divisions.FirstOrDefault(_ => _.Id == s.value.DivisionId).Name : string.Empty),
                            CreditExposure = Math.Round((s.value.CreditExposure / 100000), 2),
                            CreditLimit = Math.Round((s.value.CreditLimit / 100000), 2),
                            AvailableCreditLimit = Math.Round((s.value.AvailableCreditLimit / 100000), 2)
                        });

                        CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        //foreach (var userDivision in userDivisionMappings)
                        //{
                        //    var salesOrgContext = salesOrgs.FirstOrDefault(_ => _.Id == userDivision.SalesOrganizationId);
                        //    var distChanContext = distChans.FirstOrDefault(_ => _.Id == userDivision.DistributionChannelId);
                        //    var divisionContext = divisions.FirstOrDefault(_ => _.Id == userDivision.DivisionId);
                        //    var UserDivision = string.Concat(salesOrgContext != null ? salesOrgContext.Name : string.Empty, " / ",
                        //        distChanContext != null ? distChanContext.Name : string.Empty, " / ",
                        //        divisionContext != null ? divisionContext.Name : string.Empty);
                        //    var creditLimit = userCreditMasterContext.Where(_ => _.UserId == userDivision.UserId && _.SalesOrgId == userDivision.SalesOrganizationId && _.DistChnlId == userDivision.DistributionChannelId
                        //    && _.DivisionId == userDivision.DivisionId).Select(s => new CreditLimitAndCreditExposureOutputDto
                        //    {
                        //        DealerCode = s.User.Code,
                        //        DealerName = s.User.Name,
                        //        CreditAccountNumber = UserDivision,
                        //        CreditExposure = Math.Round((s.CreditExposure / 100000), 2),
                        //        CreditLimit = Math.Round((s.CreditLimit / 100000), 2),
                        //        AvailableCreditLimit = Math.Round((s.AvailableCreditLimit / 100000), 2)
                        //    }).ToList();
                        //    CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        //}

                        //}
                    }
                    else if (inputDto.CreditId == (int)DTO.Enums.CreditId.CreditExposure)
                    {
                        //var creditLimitContext = userCreditMasterContext.Where(_ => accountNumberList.Contains(_.CreditAccountNumber) && _.Isactive).GroupBy(accountno => accountno.CreditAccountNumber).Select(group => new
                        //{
                        //    CreditAccountNumber = group.Key,
                        //    CreditLimitList = group
                        //}).ToList();

                        //foreach (var data in creditLimitContext)
                        //{
                        //var dealerVerticalId = data.CreditLimitList.FirstOrDefault(user => user.CreditAccountNumber == data.CreditAccountNumber && dealerIds.Contains(user.UserId));
                        //var dealerCCArea = _emamiContext.DivisionDetails.AsNoTracking().FirstOrDefault(_ => _.DivisionId == dealerVerticalId).CCArea ?? "";


                        var creditLimit = userCreditMasterContext.ToList().AsEnumerable().Select(s =>new CreditLimitAndCreditExposureOutputDto()
                        {
                            DealerCode = s.value.User.Code,
                            DealerName = s.value.User.Name,
                            CreditAccountNumber = string.Concat(salesOrgs.FirstOrDefault(_ => _.Id == s.value.SalesOrgId) != null ? salesOrgs.FirstOrDefault(_ => _.Id == s.value.SalesOrgId).Name : string.Empty, " / ",
                                    distChans.FirstOrDefault(_ => _.Id == s.value.DistChnlId) != null ? distChans.FirstOrDefault(_ => _.Id == s.value.DistChnlId).Name : string.Empty, " / ",
                                    divisions.FirstOrDefault(_ => _.Id == s.value.DivisionId) != null ? divisions.FirstOrDefault(_ => _.Id == s.value.DivisionId).Name : string.Empty),
                            GrossExposure = Math.Round(((s.value.OpenOrders + s.value.DeliveryValue + s.value.BillingDocumentValue) / 100000), 2),
                            OpenExposure = Math.Round(((s.value.OpenOrders + s.value.DeliveryValue) / 100000), 2),
                            TotalReceivable = Math.Round((s.value.BillingDocumentValue / 100000), 2)
                        });
                        CreditLimitAndCreditExposureListDto.AddRange(creditLimit);

                        //foreach (var userDivision in userDivisionMappings)
                        //{
                        //    var salesOrgContext = salesOrgs.FirstOrDefault(_ => _.Id == userDivision.SalesOrganizationId);
                        //    var distChanContext = distChans.FirstOrDefault(_ => _.Id == userDivision.DistributionChannelId);
                        //    var divisionContext = divisions.FirstOrDefault(_ => _.Id == userDivision.DivisionId);
                        //    var UserDivision = string.Concat(salesOrgContext != null ? salesOrgContext.Name : string.Empty, " / ",
                        //        distChanContext != null ? distChanContext.Name : string.Empty, " / ",
                        //        divisionContext != null ? divisionContext.Name : string.Empty);

                        //    var creditLimit = userCreditMasterContext.Where(_ => _.UserId == userDivision.UserId && _.SalesOrgId == userDivision.SalesOrganizationId && _.DistChnlId == userDivision.DistributionChannelId
                        //    && _.DivisionId == userDivision.DivisionId).Select(s => new CreditLimitAndCreditExposureOutputDto
                        //    {
                        //        DealerCode = s.User.Code,
                        //        DealerName = s.User.Name,
                        //        CreditAccountNumber = UserDivision,
                        //        GrossExposure = Math.Round(((s.OpenOrders + s.DeliveryValue + s.BillingDocumentValue) / 100000), 2),
                        //        OpenExposure = Math.Round(((s.OpenOrders + s.DeliveryValue) / 100000), 2),
                        //        TotalReceivable = Math.Round((s.BillingDocumentValue / 100000), 2)
                        //    }).ToList();
                        //    CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        //}
                    }
                    else
                    {
                        //var creditLimitContext = userCreditMasterContext.Where(_ => accountNumberList.Contains(_.CreditAccountNumber) && _.Isactive).GroupBy(accountno => accountno.CreditAccountNumber).Select(group => new
                        //{
                        //    CreditAccountNumber = group.Key,
                        //    CreditLimitList = group
                        //}).ToList();

                        //foreach (var data in creditLimitContext)
                        //{
                        //var dealerVerticalId = data.CreditLimitList.FirstOrDefault(user => user.CreditAccountNumber == data.CreditAccountNumber && dealerIds.Contains(user.UserId));

                        var creditLimit = userCreditMasterContext.ToList().AsEnumerable().Select(s => new CreditLimitAndCreditExposureOutputDto()
                        {
                            DealerCode = s.value.User.Code,
                            DealerName = s.value.User.Name,
                            CreditAccountNumber = salesOrgs.FirstOrDefault(_ => _.Id == s.value.SalesOrgId) != null ? salesOrgs.FirstOrDefault(_ => _.Id == s.value.SalesOrgId).Name : String.Empty + "/" +
                           distChans.FirstOrDefault(_ => _.Id == s.value.DistChnlId) != null ? distChans.FirstOrDefault(_ => _.Id == s.value.DistChnlId).Name : String.Empty + "/" +
                            divisions.FirstOrDefault(_ => _.Id == s.value.DivisionId) != null ? divisions.FirstOrDefault(_ => _.Id == s.value.DivisionId).Name : String.Empty,
                            CreditExposure = Math.Round((s.value.CreditExposure / 100000), 2),
                            CreditLimit = Math.Round((s.value.CreditLimit / 100000), 2),
                            AvailableCreditLimit = Math.Round((s.value.AvailableCreditLimit / 100000), 2),
                            GrossExposure = Math.Round(((s.value.OpenOrders + s.value.DeliveryValue + s.value.BillingDocumentValue) / 100000), 2),
                            OpenExposure = Math.Round(((s.value.OpenOrders + s.value.DeliveryValue) / 100000), 2),
                            TotalReceivable = Math.Round((s.value.BillingDocumentValue / 100000), 2)
                        });
                        CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        //foreach (var userDivision in userDivisionMappings)
                        //{
                        //    var salesOrgContext = salesOrgs.FirstOrDefault(_ => _.Id == userDivision.SalesOrganizationId);
                        //    var distChanContext = distChans.FirstOrDefault(_ => _.Id == userDivision.DistributionChannelId);
                        //    var divisionContext = divisions.FirstOrDefault(_ => _.Id == userDivision.DivisionId);
                        //    var UserDivision = string.Concat(salesOrgContext != null ? salesOrgContext.Name : string.Empty, " / ",
                        //        distChanContext != null ? distChanContext.Name : string.Empty, " / ",
                        //        divisionContext != null ? divisionContext.Name : string.Empty);
                        //    var creditLimit = userCreditMasterContext.Where(_ => _.UserId == userDivision.UserId && _.SalesOrgId == userDivision.SalesOrganizationId && _.DistChnlId == userDivision.DistributionChannelId
                        //    && _.DivisionId == userDivision.DivisionId)
                        //        .Select(s => new CreditLimitAndCreditExposureOutputDto
                        //        {
                        //            DealerCode = s.User.Code,
                        //            DealerName = s.User.Name,
                        //            CreditAccountNumber = UserDivision,
                        //            CreditExposure = Math.Round((s.CreditExposure / 100000), 2),
                        //            CreditLimit = Math.Round((s.CreditLimit / 100000), 2),
                        //            AvailableCreditLimit = Math.Round((s.AvailableCreditLimit / 100000), 2),
                        //            GrossExposure = Math.Round(((s.OpenOrders + s.DeliveryValue + s.BillingDocumentValue) / 100000), 2),
                        //            OpenExposure = Math.Round(((s.OpenOrders + s.DeliveryValue) / 100000), 2),
                        //            TotalReceivable = Math.Round((s.BillingDocumentValue / 100000), 2)
                        //        }).ToList();
                        //    CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        //}
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = CreditLimitAndCreditExposureListDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetContactListForActiveCallToCustomers(ContactListForActiveCallInputDto inputDto)
        {
            _methodName = "GetContactListForActiveCallToCustomers";
            var resultDto = new ResultDto();
            var ContactListForActiveCallOutputDto = new List<ContactListForActiveCallOutputDto>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                //Dealer 

                if (inputDto.DealerIds.IsAny())
                {
                    ContactListForActiveCallOutputDto = _emamiContext.Users.AsNoTracking()
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.Id, ucm => ucm.UserId, (u, ucm) => new { u, ucm })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), user => user.u.Id, ur => ur.UserId, (user, ur) => new { user, ur })
                        .Where(_ => inputDto.DealerIds.Contains(_.user.ucm.CustomerId) && _.user.u.IsActive && _.ur.RoleId == (int)DTO.Enums.Role.StateTrader)
                        .Select(s => new ContactListForActiveCallOutputDto()
                    {
                        BdoId = s.user.u.Id,
                        BdoName = s.user.u.Name,
                        BdoCode = s.user.u.Code,
                        MobileNumber = s.user.u.MobileNumber,
                        AdditionalMobileNumber = s.user.u.AdditionalMobileNumber != null ? s.user.u.AdditionalMobileNumber : string.Empty,
                        //ContactPersonName = s.u.ContactPersonName != null ? s.u.ContactPersonName : string.Empty
                    }).OrderBy(_ => _.BdoName).ToList();
                }
                //StateTrader
                else if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => inputDto.BdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                    var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => dealerIds.Contains(usercustomer.CustomerId)).Select(customer => customer.UserId).ToList();
                    if (brokerIds.IsAny())
                    {
                        dealerIds.AddRange(brokerIds);
                    }
                    ContactListForActiveCallOutputDto = _emamiContext.Users.AsNoTracking()
                        .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                        .Where(_ => dealerIds.Contains(_.u.Id) && _.u.IsActive && _.u.IsActiveForCall)
                        .Select(s => new ContactListForActiveCallOutputDto()
                    {
                        DealerId = s.u.Id,
                        DealerName = s.u.Name,
                        DealerCode = s.u.Code,
                        MobileNumber = s.u.MobileNumber,
                        AdditionalMobileNumber = s.u.AdditionalMobileNumber != null ? s.u.AdditionalMobileNumber : string.Empty,
                        ContactPersonName = s.u.ContactPersonName != null ? s.u.ContactPersonName : string.Empty,
                        BrokerOrDealer = s.ur.RoleId == (int)DTO.Enums.Role.Dealer ? "Dealer" : "Broker"
                    }).OrderBy(_ => _.DealerName).ToList();

                }
                //ZonalTrader
                else if ((inputDto.DealerIds == null || !inputDto.DealerIds.Any()) && (inputDto.BdoIds == null || !inputDto.BdoIds.Any()))
                {

                    var bdoIds = _emamiContext.Users.AsNoTracking()
                        .Where(user => user.ReportingToId == inputDto.ZHId).Select(a => a.Id).ToList();

                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(usercustomer => bdoIds.Contains(usercustomer.UserId))
                        .Select(customer => customer.CustomerId).ToList();

                    var brokerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(usercustomer => dealerIds.Contains(usercustomer.CustomerId))
                        .Select(customer => customer.UserId).ToList();

                    if (brokerIds.IsAny())
                    {
                        dealerIds.AddRange(brokerIds);
                    }
                    ContactListForActiveCallOutputDto = _emamiContext.Users.AsNoTracking()
                        .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                        .Where(_ => dealerIds.Contains(_.u.Id) && _.u.IsActive && _.u.IsActiveForCall)
                        .Select(s => new ContactListForActiveCallOutputDto()
                    {
                        DealerId = s.u.Id,
                        DealerName = s.u.Name,
                        DealerCode = s.u.Code,
                        MobileNumber = s.u.MobileNumber,
                        AdditionalMobileNumber = s.u.AdditionalMobileNumber != null ? s.u.AdditionalMobileNumber : string.Empty,
                        ContactPersonName = s.u.ContactPersonName != null ? s.u.ContactPersonName : string.Empty,
                        BrokerOrDealer = s.ur.RoleId == (int)DTO.Enums.Role.Dealer ? "Dealer" : "Broker"
                    }).OrderBy(_ => _.DealerName).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = ContactListForActiveCallOutputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto SaveCallRecordingOfCustomers(ContactListForActiveCallInputDto inputDto)
        {
            _methodName = "SaveCallRecordingOfCustomers";
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
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var userRoleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id).RoleId;

                //For Dealer app  - Call has made to StateTrader so saved BdoId in UserId column
                var userId = (userRoleId == (int)DTO.Enums.Role.Dealer) ? inputDto.BdoId : inputDto.DealerId;

                var audioFileContext = new AudioFileDetailsForActiveCustomers()
                {
                    UserId = userId,
                    AudioFileName = inputDto.CallRecordedFileName,
                    MediaTypeId = (int)DTO.Enums.MediaType.Audio,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    CreatedBy = userContext.Id,
                };
                _emamiContext.AudioFileDetailsForActiveCustomers.Add(audioFileContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.CallRecordedSavedSuccess;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetAudioFilesListAgainstCustomers(ContactListForActiveCallInputDto inputDto)
        {
            _methodName = "GetAudioFilesListAgainstCustomers";
            var resultDto = new ResultDto();
            var AudioFileslistAgainstCustomers = new List<ContactListForActiveCallOutputDto>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var key = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.DaysForAudioFilesShownAgainstSaudaMapping));
                var DaysAudioFilesShown = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == key)?.Value ?? "0";
                var DaysInDouble = Convert.ToDouble(DaysAudioFilesShown);
                var DateFromWhichAudioFilesTaken = DateHelper.UtcToIndia(DateTime.UtcNow).AddDays(-DaysInDouble);
                var dealerAndBrokerIds = new List<long>();
                //Dealer 
                if (inputDto.DealerId > 0)
                {
                    dealerAndBrokerIds.Add(inputDto.DealerId);
                    var bdoId = _emamiContext.UserCustomerMapping.AsNoTracking().FirstOrDefault(_ => _.CustomerId == inputDto.DealerId)?.UserId ?? 0;
                    if (inputDto.BrokerId > 0)
                    {
                        dealerAndBrokerIds.Add(inputDto.BrokerId);
                        dealerAndBrokerIds.Add(bdoId);
                    }
                    string folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.AudioFiles);
                    string mediapath = Config.WebsitePhysicalPath + Path.Combine(ConfigurationManager.AppSettings["UploadMediaPaths"], folderName);

                    AudioFileslistAgainstCustomers = _emamiContext.AudioFileDetailsForActiveCustomers.AsNoTracking().Join(_emamiContext.Users.AsNoTracking(), files => files.UserId, u => u.Id, (files, u) => new { files, u }).Where(_ => dealerAndBrokerIds.Contains(_.files.UserId) && _.files.CreatedDate >= DateFromWhichAudioFilesTaken).AsEnumerable().Select(s => new ContactListForActiveCallOutputDto()
                    {
                        DealerId = s.files.UserId,
                        DealerName = s.u.Name,
                        DealerCode = s.u.Code,
                        CallRecordedFileName = s.u.Name + "_" + s.files.CreatedDate.ToString(Constants.DateFormat) + "_" + s.files.CreatedDate.ToString(Constants.TimeFormat),
                        AudioFileDetailId = s.files.Id,
                        AudioFileNameInServerPath = s.files.AudioFileName,
                        CallDuration = s.files.CallDuation
                    }).OrderByDescending(_ => _.AudioFileDetailId).ToList();

                    foreach (var data in AudioFileslistAgainstCustomers)
                    {
                        data.CallRecordedFileName = data.CallRecordedFileName + "_" + data.CallDuration + " sec";
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = AudioFileslistAgainstCustomers;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto SaveSaudadetailsMappedAgainstAudiofiles(ContactListForActiveCallInputDto inputDto)
        {
            _methodName = "SaveSaudadetailsMappedAgainstAudiofiles";
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
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var saudaMappingcontextIsExists = _emamiContext.SaudaAudioFileMapping.Where(_ => _.SaudaId == inputDto.SaudaId).ToList();
                if (saudaMappingcontextIsExists.IsAny())
                {
                    saudaMappingcontextIsExists.ForEach(data => _emamiContext.SaudaAudioFileMapping.Remove(data));
                    _emamiContext.SaveChanges();
                }

                foreach (var item in inputDto.AudioFileDetailIds)
                {
                    var saudaMappingContext = new SaudaAudioFileMapping
                    {
                        SaudaId = inputDto.SaudaId,
                        SaudaNumber = inputDto.SaudaNumber,
                        AudioFileDetailsForActiveCustomersId = item.AudioFileDetailId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        MediaTypeId = (int)DTO.Enums.MediaType.Audio,
                        UserId = item.UserId
                    };
                    _emamiContext.SaudaAudioFileMapping.Add(saudaMappingContext);
                }
                _emamiContext.SaveChanges();

                if (inputDto.ImagePaths.IsAny())
                {
                    var ImageNames = string.Join(",", inputDto.ImagePaths);
                    var saudaMappingContext = new SaudaAudioFileMapping
                    {
                        SaudaId = inputDto.SaudaId,
                        SaudaNumber = inputDto.SaudaNumber,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        MediaTypeId = (int)DTO.Enums.MediaType.Image,
                        ImagePath = ImageNames,
                        UserId = inputDto.DealerId
                    };
                    _emamiContext.SaudaAudioFileMapping.Add(saudaMappingContext);
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.SaudaDetailsAgainstCallRecordedSavedSuccess;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetCreditLimitAndCreditExposureListAPP(CreditLimitAndCreditExposureInputDto inputDto)
        {
            _methodName = "GetCreditLimitAndCreditExposureListAPP";
            var resultDto = new ResultDto();
            var CreditLimitAndCreditExposureListDto = new List<CreditLimitAndCreditExposureOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.IsActive);
                var userDivisionsContext = _emamiContext.UserDivisionMappings.AsNoTracking();
                var dealerIds = new List<long>();
                //List<long> VerticalList = new List<long>();
                List<long> divisionIdList = new List<long>();
                List<long> userSalesOrgIdList = new List<long>();
                List<long> userDistChanIdList = new List<long>();

                //Dealer 
                if (inputDto.DealerIds.IsAny())
                {
                    dealerIds = inputDto.DealerIds;
                }
                //StateTrader
                else if (inputDto.BdoIds.IsAny())
                {
                    var userDivisionContext = userDivisionsContext.Where(_ => inputDto.NationalHeadIds.Contains(_.UserId)).ToList(); ;
                    divisionIdList = userDivisionContext.Select(_ => _.DivisionId).ToList();
                    userSalesOrgIdList = userDivisionContext.Select(_ => _.SalesOrganizationId).ToList();
                    userDistChanIdList = userDivisionContext.Select(_ => _.DistributionChannelId).ToList();
                    //VerticalList = usersContext.Where(_ => inputDto.NationalHeadIds.Contains(_.Id)).Select(_ => (long)_.DivisionId).Distinct().ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => inputDto.BdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                //ZonalTrader
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = usersContext.Where(user => inputDto.ZonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    var userDivisionContext = userDivisionsContext.Where(_ => inputDto.ZonalHeadIds.Contains(_.UserId)).ToList(); ;
                    divisionIdList = userDivisionContext.Select(_ => _.DivisionId).ToList();
                    userSalesOrgIdList = userDivisionContext.Select(_ => _.SalesOrganizationId).ToList();
                    userDistChanIdList = userDivisionContext.Select(_ => _.DistributionChannelId).ToList();
                    //VerticalList = usersContext.Where(_ => inputDto.ZonalHeadIds.Contains(_.Id)).Select(_ => (long)_.DivisionId).Distinct().ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                else
                {
                    //  var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(roles => roles.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(a => a.UserId).ToList();
                    var zonalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(roles => roles.RoleId == (int)DTO.Enums.Role.ZonalTrader).Select(a => a.UserId).ToList();
                    var bdoIds = usersContext.Where(user => zonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId))
                        .Select(customer => customer.CustomerId).ToList();
                }

                //if (userSalesOrgIdList != null && userSalesOrgIdList.Any()
                //    && userDistChanIdList != null && userDistChanIdList.Any()
                //    && divisionIdList != null && divisionIdList.Any())
                //{
                dealerIds = userDivisionsContext.Where(_ => dealerIds.Contains(_.UserId)
                //&& divisionIdList.Contains((long)_.DivisionId)
                ).Select(_ => _.Id).ToList();
                //}
                var userDivisionMappings = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId)).ToList();
                var salesOrgs = _emamiContext.SalesOrganization.AsNoTracking().ToList();
                var distChans = _emamiContext.DistributionChannel.AsNoTracking().ToList();
                var divisions = _emamiContext.Divisions.AsNoTracking().ToList();


                var userCreditMasterContext = _emamiContext.UserCreditMaster.AsNoTracking().ToList();
                //var accountNumberList = userCreditMasterContext.Where(_ => dealerIds.Contains(_.UserId) && _.Isactive && _.CreditAccountNumber != null).Select(s => s.CreditAccountNumber).Distinct().ToList();
                if (userCreditMasterContext.IsAny())
                {
                    if (inputDto.CreditId == (int)DTO.Enums.CreditId.CreditLimit)
                    {
                        //var creditLimitContext = userCreditMasterContext.Where(_ => accountNumberList.Contains(_.CreditAccountNumber) && _.Isactive).GroupBy(accountno => accountno.CreditAccountNumber).Select(group => new
                        //{
                        //    CreditAccountNumber = group.Key,
                        //    CreditLimitList = group
                        //}).ToList();

                        foreach (var userDivision in userDivisionMappings)
                        {
                            var salesOrgContext = salesOrgs.FirstOrDefault(_ => _.Id == userDivision.SalesOrganizationId);
                            var distChanContext = distChans.FirstOrDefault(_ => _.Id == userDivision.DistributionChannelId);
                            var divisionContext = divisions.FirstOrDefault(_ => _.Id == userDivision.DivisionId);
                            var UserDivision = string.Concat(salesOrgContext != null ? salesOrgContext.Name : string.Empty, " / ",
                                distChanContext != null ? distChanContext.Name : string.Empty, " / ",
                                divisionContext != null ? divisionContext.Name : string.Empty);
                            var creditLimit = userCreditMasterContext.Where(_ => _.UserId == userDivision.UserId && _.SalesOrgId == userDivision.SalesOrganizationId && _.DistChnlId == userDivision.DistributionChannelId
                            && _.DivisionId == userDivision.DivisionId).Select(s => new CreditLimitAndCreditExposureOutputDto
                            {
                                DealerCode = s.User.Code,
                                DealerName = s.User.Name,
                                CreditAccountNumber = UserDivision,
                                CreditExposure = Math.Round((s.CreditExposure / 100000), 2),
                                CreditLimit = Math.Round((s.CreditLimit / 100000), 2),
                                AvailableCreditLimit = Math.Round((s.AvailableCreditLimit / 100000), 2)
                            }).ToList();
                            CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        }
                    }
                    else if (inputDto.CreditId == (int)DTO.Enums.CreditId.CreditExposure)
                    {
                        //var creditLimitContext = userCreditMasterContext.Where(_ => accountNumberList.Contains(_.CreditAccountNumber) && _.Isactive).GroupBy(accountno => accountno.CreditAccountNumber).Select(group => new
                        //{
                        //    CreditAccountNumber = group.Key,
                        //    CreditLimitList = group
                        //}).ToList();

                        foreach (var userDivision in userDivisionMappings)
                        {
                            var salesOrgContext = salesOrgs.FirstOrDefault(_ => _.Id == userDivision.SalesOrganizationId);
                            var distChanContext = distChans.FirstOrDefault(_ => _.Id == userDivision.DistributionChannelId);
                            var divisionContext = divisions.FirstOrDefault(_ => _.Id == userDivision.DivisionId);
                            var UserDivision = string.Concat(salesOrgContext != null ? salesOrgContext.Name : string.Empty, " / ",
                                distChanContext != null ? distChanContext.Name : string.Empty, " / ",
                                divisionContext != null ? divisionContext.Name : string.Empty);

                            var creditLimit = userCreditMasterContext.Where(_ => _.UserId == userDivision.UserId && _.SalesOrgId == userDivision.SalesOrganizationId && _.DistChnlId == userDivision.DistributionChannelId
                            && _.DivisionId == userDivision.DivisionId).Select(s => new CreditLimitAndCreditExposureOutputDto
                            {
                                DealerCode = s.User.Code,
                                DealerName = s.User.Name,
                                CreditAccountNumber = UserDivision,
                                GrossExposure = Math.Round(((s.OpenOrders + s.DeliveryValue + s.BillingDocumentValue) / 100000), 2),
                                OpenExposure = Math.Round(((s.OpenOrders + s.DeliveryValue) / 100000), 2),
                                TotalReceivable = Math.Round((s.BillingDocumentValue / 100000), 2)
                            }).ToList();
                            CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        }
                    }
                    else
                    {
                        //var creditLimitContext = userCreditMasterContext.Where(_ => accountNumberList.Contains(_.CreditAccountNumber) && _.Isactive).GroupBy(accountno => accountno.CreditAccountNumber).Select(group => new
                        //{
                        //    CreditAccountNumber = group.Key,
                        //    CreditLimitList = group
                        //}).ToList();

                        foreach (var userDivision in userDivisionMappings)
                        {
                            var salesOrgContext = salesOrgs.FirstOrDefault(_ => _.Id == userDivision.SalesOrganizationId);
                            var distChanContext = distChans.FirstOrDefault(_ => _.Id == userDivision.DistributionChannelId);
                            var divisionContext = divisions.FirstOrDefault(_ => _.Id == userDivision.DivisionId);
                            var UserDivision = string.Concat(salesOrgContext != null ? salesOrgContext.Name : string.Empty, " / ",
                                distChanContext != null ? distChanContext.Name : string.Empty, " / ",
                                divisionContext != null ? divisionContext.Name : string.Empty);
                            var creditLimit = userCreditMasterContext.Where(_ => _.UserId == userDivision.UserId && _.SalesOrgId == userDivision.SalesOrganizationId && _.DistChnlId == userDivision.DistributionChannelId
                            && _.DivisionId == userDivision.DivisionId)
                                .Select(s => new CreditLimitAndCreditExposureOutputDto
                                {
                                    DealerCode = s.User.Code,
                                    DealerName = s.User.Name,
                                    CreditAccountNumber = UserDivision,
                                    CreditExposure = Math.Round((s.CreditExposure / 100000), 2),
                                    CreditLimit = Math.Round((s.CreditLimit / 100000), 2),
                                    AvailableCreditLimit = Math.Round((s.AvailableCreditLimit / 100000), 2),
                                    GrossExposure = Math.Round(((s.OpenOrders + s.DeliveryValue + s.BillingDocumentValue) / 100000), 2),
                                    OpenExposure = Math.Round(((s.OpenOrders + s.DeliveryValue) / 100000), 2),
                                    TotalReceivable = Math.Round((s.BillingDocumentValue / 100000), 2)
                                }).ToList();
                            CreditLimitAndCreditExposureListDto.AddRange(creditLimit);
                        }
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = CreditLimitAndCreditExposureListDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }


        #region CompetitorAnalysis      

        public ResultDto GetCompetitorAnalysisList(LoginUserIdDto inputDto)
        {
            _methodName = "GetCompetitorAnalysisList";
            var resultDto = new ResultDto();
            var outputDto = new List<CompetitorAnalysisViewDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var competitorAnalysisApprovalContext = _emamiContext.CompetitorAnalysisApproval.AsNoTracking().Where(_ => (_.RequestedTo == inputDto.LoginUserId || _.RequestedBy == inputDto.LoginUserId)
                    && _.CompetitorAnalysis != null && ((inputDto.VerticalId > 0 && _.CompetitorAnalysis.OilType != null) ? _.CompetitorAnalysis.OilType.DivisionId == inputDto.VerticalId : _.CompetitorAnalysis.OilType.DivisionId > 0))
                    .GroupBy(_ => _.CompetitorAnalysisId).Select(group =>
                          new
                          {
                              CompetitorAnalysisId = group.Key,
                              CompetitorAnalysisApprovals = group.OrderByDescending(_ => _.Id)
                          })
                    .Select(_ => _.CompetitorAnalysisApprovals.FirstOrDefault());

                if (competitorAnalysisApprovalContext != null && competitorAnalysisApprovalContext.Any())
                {
                    outputDto = competitorAnalysisApprovalContext.ToList().Select(_ => new CompetitorAnalysisViewDto()
                    {
                        Id = _.CompetitorAnalysisId,
                        RequestedTo = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == _.RequestedTo)?.Name,
                        RequestedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == _.RequestedBy)?.Name,
                        HasAccessToProceed = _.RequestedTo == inputDto.LoginUserId ? true : false,
                        SkuId = _.CompetitorAnalysis.SkuId,
                        SkuName = _.CompetitorAnalysis.Sku != null ? _.CompetitorAnalysis.Sku.SkuName : string.Empty,
                        SkuCode = _.CompetitorAnalysis.Sku != null ? _.CompetitorAnalysis.Sku.SkuCode : string.Empty,
                        OilTypeId = _.CompetitorAnalysis.OilTypeId,
                        OilType = _.CompetitorAnalysis.OilType != null ? _.CompetitorAnalysis.OilType.Name : string.Empty,
                        StatusId = _.CompetitorAnalysis.StatusId,
                        Status = _.CompetitorAnalysis.Status != null ? _.CompetitorAnalysis.Status.Name : string.Empty,
                        Margin = _.CompetitorAnalysis.Margin,
                        EmamiPrice = _.CompetitorAnalysis.EmamiPrice,
                        Remarks = _.CompetitorAnalysis.Remarks,
                        WorkableQuantity = _.CompetitorAnalysis.WorkableQuantity,
                        WorkablePrice = _.CompetitorAnalysis.WorkablePrice
                    }).ToList();
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

        public ResultDto GetCompetitorAnalysisById(IdInputDto inputDto)
        {
            _methodName = "GetCompetitorAnalysisDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new CompetitorAnalysisViewDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var resultContext = _emamiContext.CompetitorAnalysis.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
                if (resultContext != null)
                {
                    outputDto.Id = resultContext.Id;
                    outputDto.SkuId = resultContext.SkuId;
                    outputDto.SkuName = resultContext.Sku != null ? resultContext.Sku.SkuName : string.Empty;
                    outputDto.OilTypeId = resultContext.OilTypeId;
                    outputDto.OilType = resultContext.OilType != null ? resultContext.OilType.Name : string.Empty;
                    outputDto.StatusId = resultContext.StatusId;
                    outputDto.Status = resultContext.Status != null ? resultContext.Status.Name : string.Empty;
                    outputDto.Margin = resultContext.Margin;
                    outputDto.EmamiPrice = resultContext.EmamiPrice;
                    outputDto.WorkableQuantity = resultContext.WorkableQuantity;
                    outputDto.WorkablePrice = resultContext.WorkablePrice;
                    outputDto.Remarks = resultContext.Remarks;

                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    var userDetails = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == resultContext.CreatedBy);
                    if (userDetails != null)
                    {
                        var cityId = userDetails.CityId;
                        var stateId = userDetails.StateId;

                        //var profitMargin = _emamiContext.ProfitMargins.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId
                        //&& _.StateId == stateId
                        //&& DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                        //&& DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                        //if (profitMargin != null)
                        //{
                        //    outputDto.ProfitMargin = profitMargin.RatePerMt;
                        //}

                        //var cushionMargin = _emamiContext.CushionMargins.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId && _.CityId == cityId
                        // && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                        //&& DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                        //if (cushionMargin != null)
                        //{
                        //    outputDto.CushionMargin = cushionMargin.RatePerMt;
                        //}

                        var oilTypeId = 0L; var litreConversion = (decimal)0;
                        var oilPackingTypeId = 0L;
                        var uomId = 0L; var quantity = (decimal)0;
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == resultContext.SkuId);
                        if (skuContext != null)
                        {
                            oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                            oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                            uomId = Convert.ToInt64(skuContext.UomId);
                            quantity = skuContext.Quantity;
                            // litreConversion = skuContext.OilType.LitreConversion;
                        }

                        //var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                        //if (oilTypeContext != null)
                        //{
                        //    litreConversion = oilTypeContext.LitreConversion;
                        //}


                        var noofPiecesperCase = (decimal)0; ;
                        var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                        if (skuUomContext != null)
                        {
                            noofPiecesperCase = skuUomContext.ConversionFactor;
                        }

                        //Cushion Margin Cost
                        var cushionMarginCostContext = _emamiContext.CushionMargins.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId && _.CityId == cityId
                         && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                        if (cushionMarginCostContext != null)
                        {
                            var cushionMarginCostMT = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                            outputDto.CushionMargin = noofPiecesperCase * cushionMarginCostMT; //Case
                        }

                        //outputDto.TotalCushionProfitMargin = outputDto.CushionMargin + outputDto.ProfitMargin;
                        var priceDifference = outputDto.EmamiPrice - outputDto.WorkablePrice;
                        if (priceDifference > 0)
                        {
                            outputDto.CalculatedFinalMargin = outputDto.CushionMargin - priceDifference;
                        }
                    }

                    var competitorApprovals = _emamiContext.CompetitorAnalysisApproval.AsNoTracking().Where(_ => _.CompetitorAnalysisId == inputDto.Id).OrderByDescending(_ => _.CreatedDate);
                    if (competitorApprovals != null && competitorApprovals.Any())
                    {
                        var requestTo = competitorApprovals.FirstOrDefault().RequestedTo;
                        if (requestTo == inputDto.LoginUserId)
                        {
                            outputDto.HasAccessToProceed = true;
                            outputDto.ApprovalsCount = competitorApprovals.Count();
                        }
                    }

                    var details = _emamiContext.CompetitorAnalysisDetails.AsNoTracking().Where(_ => _.CompetitorAnalysisId == inputDto.Id);
                    if (details != null && details.Any())
                    {
                        outputDto.CompetitorAnalysisDetailsDtoList = details
                            .Select(_ => new CompetitorAnalysisDetailsViewDto
                            {
                                CompetitorAnalysisId = _.CompetitorAnalysisId,
                                CompetitorId = _.CompetitorId,
                                CompetitorName = _.Competitor != null ? _.Competitor.Name : string.Empty,
                                SaudaRate = _.SaudaRate,
                                MarketOperatingPrice = _.MarketOperatingPrice,
                            })
                            .ToList();
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetCompetitorAnalysisDetailsListById(IdInputDto inputDto)
        {
            _methodName = "GetCompetitorAnalysisDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new List<CompetitorAnalysisDetailsViewDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var resultContext = _emamiContext.CompetitorAnalysisDetails.AsNoTracking().Where(_ => _.CompetitorAnalysisId == inputDto.Id);
                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext
                    .Select(_ => new CompetitorAnalysisDetailsViewDto
                    {
                        CompetitorAnalysisId = _.CompetitorAnalysisId,
                        CompetitorId = _.CompetitorId,
                        CompetitorName = _.Competitor != null ? _.Competitor.Name : string.Empty,
                        SaudaRate = _.SaudaRate,
                        MarketOperatingPrice = _.MarketOperatingPrice,
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
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto SaveCompetitorAnalysisApproval(CompetitorAnalysisApprovalDto inputDto)
        {
            _methodName = "SaveCompetitorAnalysisApproval";
            var resultDto = new ResultDto();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                if (inputDto.CompetitorAnalysisId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PriceDiscoveryMissing);
                }

                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                {
                    inputDto.RequestedTo = 0;
                }
                else
                {
                    var users = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId);
                    if (users != null && users.Any() && users.FirstOrDefault().ReportingToId != null)
                    {
                        inputDto.RequestedTo = (long)users.FirstOrDefault().ReportingToId;
                    }
                }
                var result = _emamiContext.CompetitorAnalysis.FirstOrDefault(_ => _.Id == inputDto.CompetitorAnalysisId);
                if (result != null)
                {
                    if (result.StatusId == (int)DTO.Enums.Status.Pending)
                    {
                        var input = new CompetitorAnalysisApproval
                        {
                            CompetitorAnalysisId = inputDto.CompetitorAnalysisId,
                            RequestedBy = inputDto.LoginUserId,
                            RequestedTo = inputDto.RequestedTo,
                            ApprovedBy = inputDto.ApprovedBy,
                            StatusId = inputDto.StatusId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CompetitorAnalysisApproval.Add(input);
                        _emamiContext.SaveChanges();


                        result.StatusId = inputDto.StatusId;
                        result.Margin = inputDto.Margin;
                        _emamiContext.SaveChanges();

                        #region Send Email and SMS

                        try
                        {
                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                            {
                                var approveOrRejectStatus = string.Empty;
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    approveOrRejectStatus = DTO.Enums.Status.Approved.ToString();
                                }
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    approveOrRejectStatus = DTO.Enums.Status.Rejected.ToString();
                                }

                                var competitorAnalysisApprovalList = _emamiContext.CompetitorAnalysisApproval.AsNoTracking()
                                .Where(_ => _.CompetitorAnalysisId == inputDto.CompetitorAnalysisId);

                                //&& _.RequestedTo != inputDto.LoginUserId && _.CreatedBy != inputDto.LoginUserId
                                //competitorAnalysisApprovalList = competitorAnalysisApprovalList.Where(_ => _.CreatedBy != inputDto.LoginUserId);
                                List<long> toUserList = new List<long>();
                                foreach (var item in competitorAnalysisApprovalList)
                                {
                                    if (item.RequestedTo != inputDto.LoginUserId)
                                    {
                                        toUserList.Add(item.RequestedTo);
                                    }

                                    if (item.CreatedBy != inputDto.LoginUserId)
                                    {
                                        toUserList.Add(item.CreatedBy);
                                    }
                                }
                                //toUserList.AddRange(competitorAnalysisApprovalList.Select(_ => _.RequestedTo));
                                //toUserList.AddRange(competitorAnalysisApprovalList.Select(_ => _.CreatedBy));

                                List<string> toUserEmails = new List<string>();
                                var sendNotifyUsers = _emamiContext.Users.AsNoTracking().Where(_ => toUserList.Contains(_.Id));
                                toUserEmails.AddRange(sendNotifyUsers.Select(_ => _.Email.ToString()));

                                List<string> toUserMobileNumbers = new List<string>();
                                toUserMobileNumbers.AddRange(sendNotifyUsers.Select(_ => _.MobileNumber));

                                //if (sendNotifyUsers != null && sendNotifyUsers.Any())
                                //{
                                //    foreach (var item in sendNotifyUsers)
                                //    {
                                //        toUserEmails.Add(item.Email.ToString());
                                //    }
                                //}
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceDiscoveryEmail);
                                if (_resultService.IsEmail())
                                {
                                    var emailSubject = Constants.PriceDiscoverySubject;
                                    var fromEmail = Constants.FromEmail;
                                    var plainText = string.Empty;
                                    if (emailTemplate != null)
                                    {
                                        var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, result.Sku?.SkuName).Replace(Constants.ApproveOrReject, approveOrRejectStatus);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                                        amazonNotificationService.SendEmail(toUserEmails, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                                if (_resultService.IsSMS())
                                {
                                    var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceDiscoverySMS);
                                    if (smsTemplate != null)
                                    {
                                        var smsMessage = smsTemplate.PlainTemplate.Replace(Constants.SkuName, result.Sku?.SkuName).Replace(Constants.ApproveOrReject, approveOrRejectStatus);
                                        foreach (var mobile in toUserMobileNumbers)
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, mobile);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.PriceDiscoveryStatusAlreadyUpdated);
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
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

        #endregion

        #region Date Range List

        public ResultDto GetDateRangeList()
        {
            _methodName = "GetDateRangeList";
            var resultDto = new ResultDto();
            try
            {

                var dateranges = _emamiContext.DateRanges.AsNoTracking().Where(_ => _.IsActive).ToList();

                var resultList = new List<DataRangeDto>();
                foreach (var item in dateranges)
                {
                    var unitItem = new DataRangeDto
                    {
                        FromValue = item.FromRange1,
                        ToValue = item.ToRange1,
                    };
                    resultList.Add(unitItem);
                    var unitItemFirstRange = new DataRangeDto
                    {
                        FromValue = item.FromRange2,
                        ToValue = item.ToRange2,
                    };
                    resultList.Add(unitItemFirstRange);
                    var unitItemSecondRange = new DataRangeDto
                    {
                        FromValue = item.FromRange3,
                        ToValue = item.ToRange3,
                    };
                    resultList.Add(unitItemSecondRange);
                    var unitItemThridRange = new DataRangeDto
                    {
                        FromValue = item.FromRange4,
                    };
                    resultList.Add(unitItemThridRange);
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

        public ResultDto SaveDealerDetails(SaveDealerDetails inputDto)
        {
            _methodName = "SaveDealerDetails";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
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
                if (string.IsNullOrEmpty(inputDto.DealerMobileNumber))
                {
                    return _resultService.ErrorMessage(Constants.DealerMobileNumberMissing);
                }
                //if (string.IsNullOrEmpty(inputDto.BDOMobileNumber))
                //{
                //    return _resultService.ErrorMessage(Constants.BDOMobileNumberMissing);
                //}
                if (inputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerIdMissing);
                }
                if (inputDto.BDOId == 0)
                {
                    return _resultService.ErrorMessage(Constants.BDOIdMissing);
                }

                var bdoMobileNumber = _emamiContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == inputDto.BDOId);
                var alreadyExists = _emamiContext.BdoChoosenDealerDetailsDuringCall.FirstOrDefault(bdo => bdo.CreatedBy == inputDto.BDOId);
                if (alreadyExists == null)
                {
                    var details = new BdoChoosenDealerDetailsDuringCall()
                    {
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        CreatedBy = inputDto.BDOId,
                        DealerId = inputDto.DealerId,
                        BDOId = inputDto.BDOId,
                        DealerMobileNumber = inputDto.DealerMobileNumber,
                        BDOMobileNumber = bdoMobileNumber != null ? bdoMobileNumber.MobileNumber : string.Empty
                    };
                    _emamiContext.BdoChoosenDealerDetailsDuringCall.Add(details);
                }
                else
                {
                    alreadyExists.DealerId = inputDto.DealerId;
                    alreadyExists.DealerMobileNumber = inputDto.DealerMobileNumber;
                    alreadyExists.BDOId = inputDto.BDOId;
                    alreadyExists.BDOMobileNumber = bdoMobileNumber != null ? bdoMobileNumber.MobileNumber : string.Empty;
                    alreadyExists.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    alreadyExists.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    alreadyExists.ModifiedBy = inputDto.BDOId;
                }

                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Config.IVRNumber;

                return resultDto;
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
