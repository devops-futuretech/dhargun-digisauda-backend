using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.Service.Common;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO.Common;
using GMCore.Helper;
using System;
using System.Text;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Adani.Solution.MVC.Common;
using Newtonsoft.Json;

namespace Adani.Solution.Service
{
    public interface INotificationService
    {
        ResultDto SendMessageOld(string message, string mobileNumber);
        ResultDto PricePublishNotificationAsync(PricingMailDto pricingMailDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        void ReverseAuctionTwoNotification(PricingMailDto pricingMaildto, CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        #region PushNotification

        ResultDto SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto);

        #endregion

        string GenerateLiftingRequestEmailTemplate(LiftingRequestNotificationDto liftingRequest);
        string GenerateLiftingRequestSmsTemplate(LiftingRequestNotificationDto liftingRequest);

        #region Surprise Benefit Notification

        void UserBasedSurpriseBenefitNotificationAsync(SurpriseBenefitMailDto surpriseBenefitMailDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        void GeographyBasedSurpriseBenefitNotificationAsync(SurpriseBenefitMailDto surpriseBenefitMailDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        #endregion
        Task<ResultDto> SendPushNotificationThroughFirebaseNew(PushNotificationInputDto pushNotificationInputDto);
        string SaudaCreateEmailTemplate(List<SaudaCreateNotificationDto> inputDto, string userName, int notificationType);
        string SaudaCreateSmsTemplate(List<SaudaCreateNotificationDto> inputDto, string userName, int notificationType);
        Task ReverseAuctionWindowNotificationAsync(PricingMailDto notificationDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        Task SendPushnotificationTemplateCreation(List<RaNotificationSendDto> raNotificationData, List<EmailTemplateDto> emailTemplateData, BidWindowListDto biddingWindow, RaSaudaConfigurationDto saudaAllocation, List<PushNotificationsDto> pushNotificationData, long noticationActionId);
    }

    public class NotificationService : INotificationService
    {
        private readonly IAdaniContext _emamiContext;
        private const string ServiceName = "Notification Service";
        private string _methodName;
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IResultService _resultService;

        public NotificationService(IAdaniContext emamiContext, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Notification Service", exception);
            }
        }

        public ResultDto SendMessageOld(string smsMessage, string sendToMobileNumber)
        {
            _methodName = "SendMessage";
            var resultDto = new ResultDto();
            try
            {
                //var query = Constants.SmsUsername + Constants.SmsPassword + Constants.SmsNumbers +
                //    sendToMobileNumber + Constants.SmsRoute + Constants.SmsRouteValue +
                //    Constants.SmsStaticMessage + smsMessage + Constants.SmsSender + Constants.SmsSenderId;

                //var apiUrl = Constants.SmsApiUrl;
                //var client = new RestClient(string.Concat(apiUrl, query));


                //var request = new RestRequest(Method.GET);
                //IRestResponse response = client.Execute(request);
                //if (response != null && response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
                //{
                //    resultDto.IsSuccess = true;
                //}
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = string.Empty;
                _logger.Error(message);
            }
            return resultDto;
        }

        #region PushNotification

        public static void SendPushNotificationToFirebase()
        {                    
            try
            {
                string applicationID = SecurityConstants.PushNotifyServerkey;
                string senderId = SecurityConstants.PushNotifySenderId;
                string deviceId = "ch_G60NPga4:APA9............T_LH8up40Ghi-J";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    data = new
                    {
                        notification_title = "test not title",
                        notification_description = "test not desc"
                    },
                    notification = new
                    {
                        body = "test body",
                        title = "test title",
                        sound = "default",
                        //click_action = ".home.activities.VisitorListActivity"
                    },
                    priority = "high"
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
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
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public ResultDto SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
        {
            _methodName = "SendPushNotificationThroughFirebase";
            var resultDto = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Service-Method {_methodName}");
                if (pushNotificationInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (string.IsNullOrEmpty(pushNotificationInputDto.Title))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PushNotificationTitleMissing;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(pushNotificationInputDto.Message))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PushNotifcationMessageMissing;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(pushNotificationInputDto.PushTokenKey))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PushTokenEmpty;
                    return resultDto;
                }
                if (pushNotificationInputDto.RegistrationTypeId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                var firebaseSenderId = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                var pushNotifyServerkey = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                var pushNotifyUrl = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

                if (string.IsNullOrEmpty(pushNotifyServerkey) || string.IsNullOrEmpty(firebaseSenderId) || string.IsNullOrEmpty(pushNotifyUrl))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

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
                            IsLogOut = pushNotificationInputDto.IsLogOut,
                            NotificationTypeId = pushNotificationInputDto.NotificationTypeId,
                            IsCMSNotification = pushNotificationInputDto.IsCMSNotification,
                            SubmittedFormId = pushNotificationInputDto.SubmittedFormId,
                            NotificationDetail = JsonHelper.ConvertObjectToJson(pushNotificationInputDto.NotificationObject)
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
                            IsLogOut = pushNotificationInputDto.IsLogOut,
                            NotificationTypeId = pushNotificationInputDto.NotificationTypeId,
                            IsCMSNotification = pushNotificationInputDto.IsCMSNotification,
                            SubmittedFormId = pushNotificationInputDto.SubmittedFormId,
                            NotificationDetail = JsonHelper.ConvertObjectToJson(pushNotificationInputDto.NotificationObject)
                        },
                        notification = new
                        {
                            title = pushNotificationInputDto.Title,
                            body = pushNotificationInputDto.Message,
                            id = pushNotificationInputDto.Id,
                            sound = "default",
                            IsLogOut = pushNotificationInputDto.IsLogOut,
                            NotificationTypeId = pushNotificationInputDto.NotificationTypeId,
                            IsCMSNotification = pushNotificationInputDto.IsCMSNotification,
                            SubmittedFormId = pushNotificationInputDto.SubmittedFormId,
                            NotificationDetail = JsonHelper.ConvertObjectToJson(pushNotificationInputDto.NotificationObject)
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
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);

            }
            return resultDto;
        }

        #endregion

        #region Window Price Publish Notification

        public ResultDto PricePublishNotificationAsync(PricingMailDto pricingMaildto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var resultdto = new ResultDto();
            try
            {
                using (AdaniContext _context = new AdaniContext())
                {
                    var BiddingWindow = _context.BiddingWindow.FirstOrDefault(f => f.Id == pricingMaildto.BiddingWindowId).Name;
                    foreach (var customerGroupId in pricingMaildto.CustomerGroupIds)
                    {
                        var today = DateHelper.UtcToIndia(DateTime.Now);
                        string NotificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(pricingMaildto.NotificationActionId);

                        var notificationDetail = _context.RaNotification.AsNoTracking()
                            .Join(_context.RaNotificationDetails.AsNoTracking(), n => n.Id, nd => nd.RaNotificationId, (n, nd) => new { Notification = n, NotificationDetail = nd })
                            .Where(f => f.NotificationDetail.IsActive
                            && f.NotificationDetail.CustomerGroupId == customerGroupId
                            && f.NotificationDetail.NotificationActionId == pricingMaildto.NotificationActionId
                            && DbFunctions.TruncateTime(today) >= DbFunctions.TruncateTime(f.Notification.ValidFrom)
                            && DbFunctions.TruncateTime(today) <= DbFunctions.TruncateTime(f.Notification.ValidTo))
                            .Select(s => new { Email = s.Notification.Email, SMS = s.Notification.SMS, InAppNotification = s.Notification.InAppNotification }).FirstOrDefault();

                        if (notificationDetail != null)
                        {
                            //var customerDetails = _context.CustomerGroupDetails.AsNoTracking().Where(w => w.CustomerGroupId == customerGroupId)
                            //.Select(s => new { Email = s.Customer.Email, MobileNumber = s.Customer.MobileNumber, PushTokenKey = s.Customer.PushTokenKey }).ToList();

                            var dealerlist = _context.RaNotificationDetails.AsNoTracking().Where(w => w.CustomerGroupId == customerGroupId && w.NotificationActionId == pricingMaildto.NotificationActionId).Select(s => s.DealerId).Distinct().ToList();

                            var customerDetails = _context.CustomerGroupDetails.AsNoTracking().Where(w => w.CustomerGroupId == customerGroupId && dealerlist.Contains(w.CustomerId))
                            .Select(s => new { CustomerId = s.CustomerId, Email = s.Customer.Email, MobileNumber = s.Customer.MobileNumber, PushTokenKey = s.Customer.PushTokenKey, RegistrationTypeId = s.Customer.RegistrationTypeId }).ToList();

                            if (customerDetails.IsAny())
                            {
                                List<string> toUsersEmail = customerDetails.Select(s => s.Email).ToList();
                                List<string> toUsersMobileNumber = customerDetails.Select(s => s.MobileNumber).ToList();
                                List<string> pushtokenkey = customerDetails.Select(s => s.PushTokenKey).ToList();

                                List<string> notificationActions = new List<string>()
                                {
                                    NotificationAction + "Email",
                                    NotificationAction + "SMS"
                                };
                                var emailTemplateData = _context.EmailTemplate.AsNoTracking().Where(email => notificationActions.Contains(email.Name))
                                    .Select(s => new { Name = s.Name, PlainTemplate = s.PlainTemplate, Template = s.Template }).ToList();


                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                string emailSubject = string.Empty;

                                if (emailTemplateData.IsAny())
                                {
                                    if (notificationDetail.Email && toUsersEmail.IsAny())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var plainText = string.Empty;
                                        emailSubject = NotificationAction;
                                        var emailTemplate = emailTemplateData.FirstOrDefault(f => f.Name == NotificationAction + "Email");
                                        if (emailTemplate != null)
                                        {
                                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.BiddingWindowName, BiddingWindow);
                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toUsersEmail, emailSubject, plainText, htmlTemplate, true);
                                        }
                                    }

                                    var smsPlainTemplate = string.Empty;

                                    if (notificationDetail.SMS && toUsersMobileNumber.IsAny())
                                    {
                                        var smsMessage = string.Empty;
                                        var smsTemplate = emailTemplateData.FirstOrDefault(email => email.Name == NotificationAction + "SMS");
                                        if (smsTemplate != null)
                                        {
                                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.BiddingWindowName, BiddingWindow);
                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            foreach (var mobilenumber in toUsersMobileNumber)
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, mobilenumber);
                                            }
                                        }
                                    }

                                    if (notificationDetail.InAppNotification && pushtokenkey.IsAny())
                                    {
                                        foreach (var userpushkey in pushtokenkey)
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = userpushkey,
                                                RegistrationTypeId = 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplate
                                            };
                                            //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                            SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }
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
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return resultdto;
        }

        #endregion

        #region Reverse Auction Window Notification

        public void ReverseAuctionTwoNotification(PricingMailDto pricingMaildto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                _methodName = "ReverseAuctionWindowNotificationAsync";
                if (pricingMaildto != null && pricingMaildto.BiddingWindowId > 0 && pricingMaildto.CustomerGroupIds.IsAny())
                {
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        string saudaAllocationTime = "";
                        StringBuilder sb = new StringBuilder();
                        //AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var today = DateHelper.UtcToIndia(DateTime.Now);

                        sb.Append("Select Name,StartTime,EndTime,SaudaAllocationStartTime,SaudaAllocationEndTime From BiddingWindows Where Id = @Id");
                        var biddingWindow = conn.QueryFirstOrDefault<BidWindowListDto>(sb.ToString(),
                            new { Id = pricingMaildto.BiddingWindowId });

                        foreach (var customerGroupId in pricingMaildto.CustomerGroupIds)
                        {
                            string NotificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(pricingMaildto.NotificationActionId);
                            string subject = NotificationAction;
                            sb.Clear();
                            sb.Append(" Select n.Id,n.Email as IsEmail,n.SMS as IsSMS,n.InAppNotification as IsInAppNotification,nd.DealerId,");
                            sb.Append(" u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey From RaNotifications n");
                            sb.Append(" Join RaNotificationDetails nd on n.Id = nd.RaNotificationId");
                            sb.Append(" Join Users u on u.Id = nd.DealerId");
                            sb.Append(" Where nd.IsActive = @IsActive");
                            sb.Append(" and nd.CustomerGroupId = @CustomerGroupId");
                            sb.Append(" and nd.NotificationActionId = @NotificationActionId");
                            sb.Append(" and Convert(varchar,@TodayDate, 111) >= Convert(varchar, n.ValidFrom, 111)");
                            sb.Append(" and Convert(varchar,@TodayDate, 111) <= Convert(varchar, n.ValidTo, 111)");
                            var notificationUserDetails = conn.Query<RaNotificationSendDto>(sb.ToString(),
                            new
                            {
                                IsActive = true,
                                CustomerGroupId = customerGroupId,
                                NotificationActionId = pricingMaildto.NotificationActionId,
                                TodayDate = today
                            }).ToList();

                            if (notificationUserDetails.IsAny())
                            {

                                #region Add StateTrader Notification Details
                                var dealerIds = notificationUserDetails.Select(s => s.DealerId).ToList();
                                sb.Clear();
                                sb.Append(" Select Distinct u.Name,u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey");
                                sb.Append(" From Users u Join UserCustomerMappings uc on u.Id = uc.UserId");
                                sb.Append(" Join UserRoles ur on u.Id = ur.UserId");
                                sb.Append(" Where uc.CustomerId in @DealerIds");
                                sb.Append(" and ur.RoleId = @RoleId");
                                sb.Append(" and u.SaudaBookingTypeId = @SaudaBookingTypeId");
                                var notificationBdoDetails = conn.Query<RaNotificationSendDto>(sb.ToString(),
                                new
                                {
                                    DealerIds = dealerIds,
                                    RoleId = (int)DTO.Enums.Role.StateTrader,
                                    //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
                                }).ToList();

                                if (notificationBdoDetails.IsAny())
                                {
                                    var isEmail = notificationUserDetails.All(a => a.IsEmail);
                                    var isSms = notificationUserDetails.All(a => a.IsSMS);
                                    var isPushnotification = notificationUserDetails.All(a => a.IsInAppNotification);
                                    foreach (var bdoNotification in notificationBdoDetails)
                                    {   
                                        if (isEmail)
                                            bdoNotification.IsEmail = true;
                                        if (isSms)
                                            bdoNotification.IsSMS = true;
                                        if (isPushnotification)
                                            bdoNotification.IsInAppNotification = true;

                                        notificationUserDetails.Add(bdoNotification);
                                    }
                                }
                                #endregion


                                if (notificationUserDetails.IsAny())
                                {
                                    sb.Clear();
                                    sb.Append(" Select Name,PlainTemplate,Template From EmailTemplates");
                                    sb.Append(" Where Name = @Name1 or Name = @Name2");
                                    var emailTemplateData = conn.Query<EmailTemplateDto>(sb.ToString(),
                                    new
                                    {
                                        Name1 = NotificationAction + "Email",
                                        Name2 = NotificationAction + "SMS"
                                    }).ToList();


                                    sb.Clear();
                                    sb.Append(" Select SaudaAllocationTime From RaSaudaConfigurations");
                                    sb.Append(" Where IsActive = @IsActive");
                                    var saudaConfiguration = conn.QueryFirstOrDefault<RaSaudaConfigurationDto>(sb.ToString(),
                                    new
                                    {
                                        IsActive = 1
                                    });
                                    //var saudaConfiguration = _emamiContext.RaSaudaConfiguration.AsNoTracking().FirstOrDefault(f => f.IsActive);

                                    var saudaAllocationEndTime = "";
                                    var saudaAllocationStartTime = "";
                                    if (saudaConfiguration != null)
                                    {
                                        //var saudaAllocationTime = saudaConfiguration.SaudaAllocationTime;
                                        //var saudaAllocationDateTime = new DateTime(today.Year, today.Month, today.Day, saudaAllocationTime.Hours, saudaAllocationTime.Minutes, saudaAllocationTime.Seconds, saudaAllocationTime.Milliseconds);
                                        //saudaAllocationEndTime = string.Format(Constants.SaudaAllocationTimeFormat, saudaAllocationDateTime);

                                        var saudaAllocationStartDateTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0);
                                        //saudaAllocationStartTime = string.Format(Constants.SaudaAllocationTimeFormat, saudaAllocationStartDateTime);
                                    }

                                    var windowStartTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.StartTime);
                                    var windowEndTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.EndTime);
                                    //saudaAllocationStartTime = String.Format("{0:HH:mm tt}", biddingWindow.SaudaAllocationStartTime);
                                    //saudaAllocationEndTime = String.Format("{0:HH:mm tt}", biddingWindow.SaudaAllocationEndTime);

                                    #region Email
                                    if (notificationUserDetails.IsAny())
                                    {
                                        _logger.Error("Email is fiered");

                                        var plainTemplate = string.Empty;
                                        var htmlTemplate = string.Empty;
                                        var toEmails = notificationUserDetails.Where(w => w.IsEmail).Select(s => s.Email).Distinct().ToList();
                                        if (toEmails.IsAny())
                                        {
                                            var fromEmail = Constants.FromEmail;
                                            var plainText = string.Empty;
                                            subject = NotificationAction;
                                            var emailTemplate = emailTemplateData.FirstOrDefault(f => f.Name == NotificationAction + "Email");

                                            if (emailTemplate != null)
                                            {
                                                //plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.BiddingWindowName, biddingWindow.Name);
                                                //htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                //amazonNotificationService.SendEmail(toEmails, subject, plainText, htmlTemplate, true);

                                                sb.Clear();
                                                sb.Append(" Select SaudaAllocationTime From RaSaudaConfigurations Where IsActive = @IsActive");
                                                var saudaAllocationData = conn.QueryFirstOrDefault<RaSaudaConfigurationDto>(sb.ToString(),
                                                new
                                                {
                                                    IsActive = 1
                                                });
                                                sb.Clear();

                                                if (saudaAllocationData != null)
                                                {
                                                    //var saudaAllocationTime = saudaAllocationData.SaudaAllocationTime;
                                                    var saudaAllocationDateTime = new DateTime(today.Year, today.Month, today.Day, saudaAllocationData.SaudaAllocationTime.Hours, saudaAllocationData.SaudaAllocationTime.Minutes, saudaAllocationData.SaudaAllocationTime.Seconds, saudaAllocationData.SaudaAllocationTime.Milliseconds);
                                                    saudaAllocationTime = string.Format(Constants.SaudaAllocationTimeFormat, saudaAllocationDateTime);
                                                }

                                                switch (pricingMaildto.NotificationActionId)
                                                {
                                                    case (int)DTO.Enums.NotificationActions.WindowCreation:
                                                        plainTemplate = emailTemplate.PlainTemplate
                                                            .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                            .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                            .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                                            .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                                                        htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                        SendEmail(toEmails, subject, plainText, htmlTemplate, true);
                                                        break;
                                                    case (int)DTO.Enums.NotificationActions.WindowPricePublish:
                                                        plainTemplate = emailTemplate.PlainTemplate
                                                            .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                            .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                            .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                                            .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                                                        htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                        SendEmail(toEmails, subject, plainText, htmlTemplate, true);
                                                        break;
                                                    case (int)DTO.Enums.NotificationActions.WindowStopped:
                                                        plainTemplate = emailTemplate.PlainTemplate
                                                            .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                            .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                            .Replace(Constants.BiddingWindowEndTime, windowEndTime);
                                                        htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                        SendEmail(toEmails, subject, plainText, htmlTemplate, true);
                                                        break;
                                                    case (int)DTO.Enums.NotificationActions.WindowCompleted:
                                                        plainTemplate = emailTemplate.PlainTemplate
                                                            .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                           .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                            .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                                            .Replace(Constants.BiddingWindowSaudaAllocationStartTime, saudaAllocationStartTime)
                                                            .Replace(Constants.BiddingWindowSaudaAllocationEndTime, saudaAllocationEndTime);
                                                        htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                        SendEmail(toEmails, subject, plainText, htmlTemplate, true);
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    var smsMessage = string.Empty;
                                    var smsPlainTemplate = string.Empty;

                                    var smsTemplate = emailTemplateData.FirstOrDefault(email => email.Name == NotificationAction + "SMS");
                                    if (smsTemplate != null)
                                    {
                                        switch (pricingMaildto.NotificationActionId)
                                        {
                                            case (int)DTO.Enums.NotificationActions.WindowCreation:
                                                smsPlainTemplate = smsTemplate.PlainTemplate
                                                    .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                    .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                    .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                                    .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                                break;
                                            case (int)DTO.Enums.NotificationActions.WindowPricePublish:
                                                smsPlainTemplate = smsTemplate.PlainTemplate
                                                   .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                    .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                    .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                                    .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                                break;
                                            case (int)DTO.Enums.NotificationActions.WindowStopped:
                                                smsPlainTemplate = smsTemplate.PlainTemplate
                                                    .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                    .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                    .Replace(Constants.BiddingWindowEndTime, windowEndTime);
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                                break;
                                            case (int)DTO.Enums.NotificationActions.WindowCompleted:
                                                smsPlainTemplate = smsTemplate.PlainTemplate
                                                   .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                                   .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                                   .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                                   .Replace(Constants.BiddingWindowSaudaAllocationStartTime, saudaAllocationStartTime)
                                                   .Replace(Constants.BiddingWindowSaudaAllocationEndTime, saudaAllocationEndTime);
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    foreach (var notification in notificationUserDetails)
                                    {
                                        if (notification.IsSMS)
                                        {
                                            _logger.Error("SMS  is fiered");
                                            SendMessage(smsMessage, notification.MobileNumber);
                                        }

                                        if (notification.IsInAppNotification)
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = notification.PushTokenKey,
                                                RegistrationTypeId = notification.RegistrationTypeId,
                                                Title = subject,
                                                Message = smsMessage
                                            };
                                            SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }
                            }
                            #region Push Notification Nested Method
                            void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
                            {
                                _logger.Error("push notification  is fiered");
                                try
                                {
                                    sb.Clear();
                                    sb.Append(" Select [Key],Value From Configurations");
                                    sb.Append(" Where [Key] = @FirebaseSenderId or [Key] = @PushNotifyServerkey or [Key] = @PushNotifyUrl");
                                    var pushNotificationData = conn.Query<PushNotificationsDto>(sb.ToString(),
                                    new
                                    {
                                        FirebaseSenderId = Constants.FirebaseSenderId,
                                        PushNotifyServerkey = Constants.PushNotifyServerkey,
                                        PushNotifyUrl = Constants.PushNotifyUrl
                                    }).ToList();

                                    if (pushNotificationData.IsAny())
                                    {
                                        var firebaseSenderId = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                                        var pushNotifyServerkey = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                                        var pushNotifyUrl = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

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
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            //return resultdto;
        }

        public async Task ReverseAuctionWindowNotificationAsync(PricingMailDto notificationDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            _methodName = "ReverseAuctionWindowNotificationAsync";
            try
            {

                var isEmail = false;
                var isSms = false;
                var isPushnotification = false;

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    if (notificationDto.CustomerGroupIds.IsAny())
                    {
                        foreach (var customerGroupId in notificationDto.CustomerGroupIds)
                        {
                            string notificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(notificationDto.NotificationActionId);
                            var resultSet = conn.QueryMultiple("RABiddingWindowNotification", new
                            {
                                BiddingWindowId = notificationDto.BiddingWindowId,
                                CustomerGroupId = customerGroupId,
                                NotificationActionId = notificationDto.NotificationActionId,
                                TemplateName = notificationAction
                            }, null, commandType: CommandType.StoredProcedure);

                            if (resultSet != null)
                            {
                                var dealerList = resultSet.Read<RaNotificationSendDto>().ToList();
                                var bdoList = resultSet.Read<BdoNotificationDto>().ToList();
                                var emailTemplates = resultSet.Read<EmailTemplateDto>().ToList();
                                var biddingWindow = resultSet.Read<BidWindowListDto>().FirstOrDefault();
                                var saudaAllocationTime = resultSet.Read<RaSaudaConfigurationDto>().FirstOrDefault();
                                var pushNotificationData = resultSet.Read<PushNotificationsDto>().ToList();

                                #region StateTrader Adding
                                if (dealerList.IsAny() && bdoList.IsAny())
                                {
                                    bdoList.ForEach(f =>
                                    {
                                        isEmail = dealerList.Any(a => a.BdoId == f.Id && a.IsEmail);
                                        isSms = dealerList.Any(a => a.BdoId == f.Id && a.IsSMS);
                                        isPushnotification = dealerList.Any(a => a.BdoId == f.Id && a.IsInAppNotification);

                                        dealerList.Add(new RaNotificationSendDto()
                                        {
                                            DealerId = f.Id,
                                            Name = f.Name,
                                            MobileNumber = f.MobileNumber,
                                            Email = f.Email,
                                            RegistrationTypeId = f.RegistrationTypeId,
                                            PushTokenKey = f.PushTokenKey,
                                            IsSMS = isSms,
                                            IsEmail = isEmail,
                                            IsInAppNotification = isPushnotification,
                                        });
                                    });
                                }
                                #endregion

                                if (dealerList.IsAny())
                                {
                                    isEmail = dealerList.Any(a => a.IsEmail);
                                    isSms = dealerList.Any(a => a.IsSMS);
                                    isPushnotification = dealerList.Any(a => a.IsInAppNotification);
                                    var toEmailIds = dealerList.Where(w => w.IsEmail).Select(s => s.Email).Distinct().ToList();
                                    var toMobileNumbers = dealerList.Where(w => w.IsSMS).Select(s => s.MobileNumber).Distinct().ToList();
                                    if (isEmail)
                                    {
                                        await EmailSendTemplateCreation(emailTemplates, toEmailIds, biddingWindow, saudaAllocationTime, notificationDto.NotificationActionId);
                                    }

                                    if (isSms)
                                    {
                                        await SmsSendTemplateCreation(emailTemplates, toMobileNumbers, biddingWindow, saudaAllocationTime, notificationDto.NotificationActionId);
                                    }

                                    if (isPushnotification)
                                    {
                                        await SendPushnotificationTemplateCreation(dealerList, emailTemplates, biddingWindow, saudaAllocationTime, pushNotificationData, notificationDto.NotificationActionId);
                                    }
                                }
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

        public async Task EmailSendTemplateCreation(List<EmailTemplateDto> emailTemplateData, List<string> toEmails, BidWindowListDto biddingWindow, RaSaudaConfigurationDto saudaAllocation, long noticationActionId)
        {
            _methodName = "EmailSendTemplateCreation";
            try
            {
                string plainTemplate = string.Empty;
                string htmlTemplate = string.Empty;
                string saudaAllocationTime = string.Empty;
                string notificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(noticationActionId);
                string subject = notificationAction;
                var emailTemplate = emailTemplateData.FirstOrDefault(f => f.Name == notificationAction + "Email");
                var toDayDate = DateHelper.UtcToIndia(DateTime.Now);
                var windowStartTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.StartTime);
                var windowEndTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.EndTime);
                if (saudaAllocation != null)
                {
                    var saudaAllocationDateTime = new DateTime(toDayDate.Year, toDayDate.Month, toDayDate.Day, saudaAllocation.SaudaAllocationTime.Hours, saudaAllocation.SaudaAllocationTime.Minutes, saudaAllocation.SaudaAllocationTime.Seconds, saudaAllocation.SaudaAllocationTime.Milliseconds);
                    saudaAllocationTime = string.Format(Constants.SaudaAllocationTimeFormat, saudaAllocationDateTime);
                }

                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                if (emailTemplate != null)
                {
                    switch (noticationActionId)
                    {
                        case (int)DTO.Enums.NotificationActions.WindowCreation:
                            plainTemplate = emailTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                            htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                            //amazonNotificationService.SendEmail(toEmails, subject, string.Empty, htmlTemplate, true);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowPricePublish:
                            plainTemplate = emailTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                            htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                            //amazonNotificationService.SendEmail(toEmails, subject, string.Empty, htmlTemplate, true);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowStopped:
                            plainTemplate = emailTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime);
                            htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                            //amazonNotificationService.SendEmail(toEmails, subject, string.Empty, htmlTemplate, true);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowCompleted:
                            plainTemplate = emailTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                               .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                //.Replace(Constants.BiddingWindowSaudaAllocationStartTime, saudaAllocationStartTime)
                                .Replace(Constants.BiddingWindowSaudaAllocationEndTime, saudaAllocationTime);
                            htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                            //amazonNotificationService.SendEmail(toEmails, subject, string.Empty, htmlTemplate, true);
                            break;
                        default:
                            break;
                    }

                    int messageCount = 0;
                    int skipCount = 0;
                    int takeCount = Config.MaximumEmailCount;
                    decimal divider = Convert.ToDecimal(string.Format("{0:0.0}", takeCount));
                    decimal count = 0;

                    if (toEmails.Count < takeCount)
                        count = 1;
                    else
                        count = Math.Round(toEmails.Count / divider);

                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            var emailResult = toEmails.Skip(skipCount).Take(takeCount).ToList();
                            var result = await amazonNotificationService.SendEmailAsync(emailResult, subject, string.Empty, htmlTemplate, true);
                            if (result.IsSuccess)
                            {
                                messageCount += emailResult.Count;
                            }
                            skipCount += takeCount;
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

        public async Task SmsSendTemplateCreation(List<EmailTemplateDto> emailTemplateData, List<string> toMobileNumbers, BidWindowListDto biddingWindow, RaSaudaConfigurationDto saudaAllocation, long noticationActionId)
        {
            _methodName = "SmsSendTemplateCreation";
            try
            {
                string saudaAllocationTime = string.Empty;
                string smsMessage = string.Empty;
                string smsPlainTemplate = string.Empty;
                string notificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(noticationActionId);
                var windowStartTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.StartTime);
                var windowEndTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.EndTime);
                var toDayDate = DateHelper.UtcToIndia(DateTime.Now);

                if (saudaAllocation != null)
                {
                    var saudaAllocationDateTime = new DateTime(toDayDate.Year, toDayDate.Month, toDayDate.Day, saudaAllocation.SaudaAllocationTime.Hours, saudaAllocation.SaudaAllocationTime.Minutes, saudaAllocation.SaudaAllocationTime.Seconds, saudaAllocation.SaudaAllocationTime.Milliseconds);
                    saudaAllocationTime = string.Format(Constants.SaudaAllocationTimeFormat, saudaAllocationDateTime);
                }

                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                var smsTemplate = emailTemplateData.FirstOrDefault(email => email.Name == notificationAction + "SMS");
                if (smsTemplate != null)
                {
                    switch (noticationActionId)
                    {
                        case (int)DTO.Enums.NotificationActions.WindowCreation:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowPricePublish:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                               .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowStopped:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowCompleted:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                               .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                               .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                               .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                               //.Replace(Constants.BiddingWindowSaudaAllocationStartTime, saudaAllocationStartTime)
                               .Replace(Constants.BiddingWindowSaudaAllocationEndTime, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;
                        default:
                            break;
                    }

                    if (toMobileNumbers.IsAny())
                    {
                        int messageCount = 0;
                        int skipCount = 0;
                        int takeCount = Config.MaximumSmsCount;
                        decimal divider = Convert.ToDecimal(string.Format("{0:0.0}", takeCount));
                        decimal count = 0;

                        if (toMobileNumbers.Count < takeCount)
                            count = 1;
                        else
                            count = Math.Round(toMobileNumbers.Count / divider);

                        if (count > 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var numberResult = toMobileNumbers.Skip(skipCount).Take(takeCount).ToList();
                                var result = await amazonNotificationService.SendMessageAsync(smsMessage, string.Join(",", numberResult));
                                if (result.IsSuccess)
                                {
                                    messageCount += numberResult.Count;
                                }
                                skipCount += takeCount;
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

        public async Task SendPushnotificationTemplateCreation(List<RaNotificationSendDto> raNotificationData, List<EmailTemplateDto> emailTemplateData, BidWindowListDto biddingWindow, RaSaudaConfigurationDto saudaAllocation, List<PushNotificationsDto> pushNotificationData, long noticationActionId)
        {
            _methodName = "SendPushnotificationTemplateCreation";
            try
            {
                string saudaAllocationTime = string.Empty;
                string smsMessage = string.Empty;
                string smsPlainTemplate = string.Empty;
                string notificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>(noticationActionId);
                var windowStartTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.StartTime);
                var windowEndTime = String.Format(Constants.SaudaAllocationTimeFormat, biddingWindow.EndTime);
                var toDayDate = DateHelper.UtcToIndia(DateTime.Now);

                if (saudaAllocation != null)
                {
                    var saudaAllocationDateTime = new DateTime(toDayDate.Year, toDayDate.Month, toDayDate.Day, saudaAllocation.SaudaAllocationTime.Hours, saudaAllocation.SaudaAllocationTime.Minutes, saudaAllocation.SaudaAllocationTime.Seconds, saudaAllocation.SaudaAllocationTime.Milliseconds);
                    saudaAllocationTime = string.Format(Constants.SaudaAllocationTimeFormat, saudaAllocationDateTime);
                }

                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                var smsTemplate = emailTemplateData.FirstOrDefault(email => email.Name == notificationAction + "SMS");
                if (smsTemplate != null)
                {
                    switch (noticationActionId)
                    {
                        case (int)DTO.Enums.NotificationActions.WindowCreation:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowPricePublish:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                               .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                                .Replace(Constants.SAUDAALLOCATIONTIME, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowStopped:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                                .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                                .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                                .Replace(Constants.BiddingWindowEndTime, windowEndTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;
                        case (int)DTO.Enums.NotificationActions.WindowCompleted:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                               .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                               .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                               .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                               .Replace(Constants.BiddingWindowSaudaAllocationEndTime, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                            break;

                        case (int)DTO.Enums.NotificationActions.AboutWindowEnd:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                               .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                               .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                               .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                               .Replace(Constants.BiddingWindowSaudaAllocationEndTime, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, "Window End Caution Notification");
                            break;
                        case (int)DTO.Enums.NotificationActions.CustomerCounterBidoffer:
                            smsPlainTemplate = smsTemplate.PlainTemplate
                               .Replace(Constants.BiddingWindowName, biddingWindow.Name)
                               .Replace(Constants.BiddingWindowStartTime, windowStartTime)
                               .Replace(Constants.BiddingWindowEndTime, windowEndTime)
                               .Replace(Constants.BiddingWindowSaudaAllocationEndTime, saudaAllocationTime);
                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, "Counter Bid offer Caution Notification");
                            break;
                        default:
                            break;
                    }

                    if (raNotificationData.IsAny() && !string.IsNullOrEmpty(smsMessage))
                    {
                        var firebaseSenderId = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                        var pushNotifyServerkey = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                        var pushNotifyUrl = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

                        foreach (var notification in raNotificationData)
                        {
                            if (notification.RegistrationTypeId > 0 && !string.IsNullOrEmpty(notification.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = notification.PushTokenKey,
                                    RegistrationTypeId = notification.RegistrationTypeId,
                                    Title = notificationAction,
                                    Message = smsMessage,
                                    FirebaseSenderId = firebaseSenderId,
                                    PushNotifyServerkey = pushNotifyServerkey,
                                    PushNotifyUrl = pushNotifyUrl
                                };
                                await SendPushNotificationThroughFirebaseNew(pushNotificationInputDto);
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

        public async Task<ResultDto> SendPushNotificationThroughFirebaseNew(PushNotificationInputDto pushNotificationInputDto)
        {
            object fcmMessage = null;
            var resultDto = new ResultDto();

            try
            {
                if (pushNotificationInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                if (string.IsNullOrEmpty(pushNotificationInputDto.Title))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PushNotificationTitleMissing;
                    return resultDto;
                }

                if (string.IsNullOrEmpty(pushNotificationInputDto.Message))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PushNotifcationMessageMissing;
                    return resultDto;
                }

                if (string.IsNullOrEmpty(pushNotificationInputDto.PushTokenKey))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.PushTokenEmpty;
                    return resultDto;
                }

                if (pushNotificationInputDto.RegistrationTypeId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }

                if (pushNotificationInputDto.PushTokenKey != null)
                {
                    var DirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
                    //var DirectoryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), "Credentials");
                    //var DirectoryPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/adani-1ec98-firebase-adminsdk-krt2v-71bc79cb9b.json"));

                    DirectoryPath = Path.Combine(DirectoryPath, "Credential");


                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }

                    var filepath = Path.Combine(DirectoryPath, ConfigHelper.PushNotificationConfigFileName);
                    GoogleCredential credential;
                    using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        credential = GoogleCredential.FromStream(stream).CreateScoped(scopes: ConfigHelper.Scopes);
                    }

                    var accessToken = credential.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        if (pushNotificationInputDto.RegistrationTypeId == (int)(int)DTO.Enums.RegistrationType.Android)
                        {
                            fcmMessage = new
                            {
                                message = new
                                {
                                    token = pushNotificationInputDto.PushTokenKey,
                                    notification = new
                                    {
                                        title = pushNotificationInputDto.Title,
                                        body = pushNotificationInputDto.Message
                                    },
                                    android = new
                                    {
                                        priority = "high",
                                        notification = new
                                        {
                                            click_action = "FLUTTER_NOTIFICATION_CLICK",
                                            sound = "default"
                                        }
                                    }
                                }
                            };
                        }
                        else if (pushNotificationInputDto.RegistrationTypeId == (int)(int)DTO.Enums.RegistrationType.IOS)
                        {
                            fcmMessage = new
                            {
                                message = new
                                {
                                    token = pushNotificationInputDto.PushTokenKey,
                                    notification = new
                                    {
                                        title = pushNotificationInputDto.Title,
                                        body = pushNotificationInputDto.Message
                                    },
                                    apns = new
                                    {
                                        payload = new
                                        {
                                            aps = new
                                            {
                                                alert = new
                                                {
                                                    title = pushNotificationInputDto.Title,
                                                    body = pushNotificationInputDto.Message
                                                },
                                                sound = "default",
                                                badge = 1
                                            }
                                        }
                                    }
                                }
                            };
                        }

                        var jsonMessage = JsonConvert.SerializeObject(fcmMessage);
                        var body = new StringContent(jsonMessage, System.Text.Encoding.UTF8, "application/json");

                        var response = client.PostAsync(new Uri(ConfigHelper.FCMUrl), body).Result;
                    }
                }

                resultDto.IsSuccess = true;
                return await Task.FromResult(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return await Task.FromResult(resultDto);
            }
        }

        #endregion

        #region Surprise Benefit Notification

        public void UserBasedSurpriseBenefitNotificationAsync(SurpriseBenefitMailDto surpriseBenefitMailDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                if (surpriseBenefitMailDto != null && surpriseBenefitMailDto.CustomerGroupId > 0 && surpriseBenefitMailDto.CustomerIds.IsAny())
                {
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        StringBuilder sb = new StringBuilder();
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var today = DateHelper.UtcToIndia(DateTime.Now);

                        string NotificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>((int)DTO.Enums.NotificationActions.SurpriseDiscount);

                        string subject = NotificationAction;
                        sb.Clear();
                        sb.Append(" Select n.Id,n.Email as IsEmail,n.SMS as IsSMS,n.InAppNotification as IsInAppNotification,nd.DealerId,");
                        sb.Append(" u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey From RaNotifications n");
                        sb.Append(" Join RaNotificationDetails nd on n.Id = nd.RaNotificationId");
                        sb.Append(" Join Users u on u.Id = nd.DealerId");
                        sb.Append(" Where nd.IsActive = @IsActive");
                        sb.Append(" and nd.CustomerGroupId = @CustomerGroupId");
                        sb.Append(" and nd.DealerId in @CustomerIds");
                        sb.Append(" and nd.NotificationActionId = @NotificationActionId");
                        sb.Append(" and Convert(varchar,@TodayDate, 111) >= Convert(varchar, n.ValidFrom, 111)");
                        sb.Append(" and Convert(varchar,@TodayDate, 111) <= Convert(varchar, n.ValidTo, 111)");
                        var notificationUserDetails = conn.Query<RaNotificationSendDto>(sb.ToString(),
                        new
                        {
                            IsActive = true,
                            CustomerGroupId = surpriseBenefitMailDto.CustomerGroupId,
                            CustomerIds = surpriseBenefitMailDto.CustomerIds,
                            NotificationActionId = (int)DTO.Enums.NotificationActions.SurpriseDiscount,
                            TodayDate = today
                        }).ToList();

                        if (notificationUserDetails.IsAny())
                        {

                            #region Add StateTrader Notification Details
                            var dealerIds = notificationUserDetails.Select(s => s.DealerId).ToList();
                            sb.Clear();
                            sb.Append(" Select Distinct u.Name,u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey");
                            sb.Append(" From Users u Join UserCustomerMappings uc on u.Id = uc.UserId");
                            sb.Append(" Join UserRoles ur on u.Id = ur.UserId");
                            sb.Append(" Where uc.CustomerId in @DealerIds");
                            sb.Append(" and ur.RoleId = @RoleId");
                            sb.Append(" and u.SaudaBookingTypeId = @SaudaBookingTypeId");
                            var notificationBdoDetails = conn.Query<RaNotificationSendDto>(sb.ToString(),
                            new
                            {
                                DealerIds = dealerIds,
                                RoleId = (int)DTO.Enums.Role.StateTrader,
                                //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
                            }).ToList();

                            if (notificationBdoDetails.IsAny())
                            {
                                foreach (var bdoNotification in notificationBdoDetails)
                                {
                                    bdoNotification.IsEmail = true;
                                    bdoNotification.IsSMS = true;
                                    bdoNotification.IsInAppNotification = true;
                                    notificationUserDetails.Add(bdoNotification);
                                }
                            }
                            #endregion

                            if (notificationUserDetails.IsAny())
                            {
                                sb.Clear();
                                sb.Append(" Select Name,PlainTemplate,Template From EmailTemplates");
                                sb.Append(" Where Name = @Name1 or Name = @Name2");
                                var emailTemplateData = conn.Query<EmailTemplateDto>(sb.ToString(),
                                new
                                {
                                    Name1 = NotificationAction + "Email",
                                    Name2 = NotificationAction + "SMS"
                                }).ToList();

                                sb.Clear();
                                sb.Append(" Select SkuName From Skus");
                                sb.Append(" Where Id in @SkuIds");
                                var skuNames = conn.Query<string>(sb.ToString(),
                                new
                                {
                                    SkuIds = surpriseBenefitMailDto.SkuIds
                                }).ToList();

                                string SkuName = UtilityHelper.ConvertStringListToCommaSeparatedString(skuNames);

                                string PERCASE_OR_DAYS = string.Empty;
                                string DiscountOrDays = string.Empty;
                                if (surpriseBenefitMailDto.BenefitTypeId == (int)DTO.Enums.BenefitType.SAP)
                                {
                                    DiscountOrDays = (Math.Round(surpriseBenefitMailDto.BenefitDiscountOrDays, 0)).ToString();
                                    PERCASE_OR_DAYS = Constants.DAYS;
                                }
                                else
                                {
                                    DiscountOrDays = surpriseBenefitMailDto.BenefitDiscountOrDays.ToString();
                                    PERCASE_OR_DAYS = Constants.PERCASE;
                                }

                                #region Email

                                if (notificationUserDetails.IsAny())
                                {
                                    var plainTemplate = string.Empty;
                                    var htmlTemplate = string.Empty;
                                    var toEmails = notificationUserDetails.Where(w => w.IsEmail).Select(s => s.Email).Distinct().ToList();


                                    if (toEmails.IsAny())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var plainText = string.Empty;
                                        subject = NotificationAction;
                                        var emailTemplate = emailTemplateData.FirstOrDefault(f => f.Name == NotificationAction + "Email");

                                        if (emailTemplate != null)
                                        {
                                            plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, SkuName)
                                                .Replace(Constants.BENEFIT_TYPE, surpriseBenefitMailDto.BenefitType.ToString())
                                                .Replace(Constants.BENEFIT, surpriseBenefitMailDto.BenefitOrCategory.ToString())
                                                .Replace(Constants.DISCOUNTORDAYS, surpriseBenefitMailDto.DiscountOrDays)
                                                .Replace(Constants.PERCASE_OR_DAYS, PERCASE_OR_DAYS);

                                            htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toEmails, subject, plainText, htmlTemplate, true);
                                        }
                                    }
                                }
                                #endregion

                                foreach (var notification in notificationUserDetails)
                                {
                                    #region SMS

                                    var smsPlainTemplate = string.Empty;
                                    if (notification.IsSMS)
                                    {
                                        var smsMessage = string.Empty;
                                        var smsTemplate = emailTemplateData.FirstOrDefault(email => email.Name == NotificationAction + "SMS");
                                        if (smsTemplate != null)
                                        {

                                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, SkuName)
                                                .Replace(Constants.BENEFIT_TYPE, surpriseBenefitMailDto.BenefitType.ToString())
                                                .Replace(Constants.BENEFIT, surpriseBenefitMailDto.BenefitOrCategory.ToString())
                                                 .Replace(Constants.DISCOUNTORDAYS, surpriseBenefitMailDto.DiscountOrDays)
                                                .Replace(Constants.PERCASE_OR_DAYS, PERCASE_OR_DAYS);

                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            amazonNotificationService.SendMessage(smsMessage, notification.MobileNumber);

                                        }
                                    }

                                    #endregion

                                    #region Push Notification

                                    if (notification.IsInAppNotification)
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = notification.PushTokenKey,
                                            RegistrationTypeId = notification.RegistrationTypeId,
                                            Title = subject,
                                            Message = smsPlainTemplate
                                        };
                                        SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }

                                    #endregion
                                }
                            }
                        }
                        #region Push Notification Nested Method
                        void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
                        {
                            try
                            {
                                sb.Clear();
                                sb.Append(" Select [Key],Value From Configurations");
                                sb.Append(" Where [Key] = @FirebaseSenderId or [Key] = @PushNotifyServerkey or [Key] = @PushNotifyUrl");
                                var pushNotificationData = conn.Query<PushNotificationsDto>(sb.ToString(),
                                new
                                {
                                    FirebaseSenderId = Constants.FirebaseSenderId,
                                    PushNotifyServerkey = Constants.PushNotifyServerkey,
                                    PushNotifyUrl = Constants.PushNotifyUrl
                                }).ToList();

                                if (pushNotificationData.IsAny())
                                {
                                    var firebaseSenderId = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                                    var pushNotifyServerkey = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                                    var pushNotifyUrl = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

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
            //return resultdto;
        }

        public void GeographyBasedSurpriseBenefitNotificationAsync(SurpriseBenefitMailDto surpriseBenefitMailDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                if (surpriseBenefitMailDto != null && surpriseBenefitMailDto.CustomerIds.IsAny())
                {
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        StringBuilder sb = new StringBuilder();
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var today = DateHelper.UtcToIndia(DateTime.Now);

                        string NotificationAction = Utility.GetEnumFromString<DTO.Enums.NotificationActions>((int)DTO.Enums.NotificationActions.SurpriseDiscount);

                        string subject = NotificationAction;
                        sb.Clear();
                        sb.Append(" Select n.Id,n.Email as IsEmail,n.SMS as IsSMS,n.InAppNotification as IsInAppNotification,nd.DealerId,");
                        sb.Append(" u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey From RaNotifications n");
                        sb.Append(" Join RaNotificationDetails nd on n.Id = nd.RaNotificationId");
                        sb.Append(" Join Users u on u.Id = nd.DealerId");
                        sb.Append(" Where nd.IsActive = @IsActive");
                        sb.Append(" and nd.DealerId in @CustomerIds");
                        sb.Append(" and nd.NotificationActionId = @NotificationActionId");
                        sb.Append(" and Convert(varchar,@TodayDate, 111) >= Convert(varchar, n.ValidFrom, 111)");
                        sb.Append(" and Convert(varchar,@TodayDate, 111) <= Convert(varchar, n.ValidTo, 111)");
                        var notificationUserDetails = conn.Query<RaNotificationSendDto>(sb.ToString(),
                        new
                        {
                            IsActive = true,
                            CustomerIds = surpriseBenefitMailDto.CustomerIds,
                            NotificationActionId = (int)DTO.Enums.NotificationActions.SurpriseDiscount,
                            TodayDate = today
                        }).ToList();

                        if (notificationUserDetails.IsAny())
                        {

                            #region Add StateTrader Notification Details
                            var dealerIds = notificationUserDetails.Select(s => s.DealerId).ToList();
                            sb.Clear();
                            sb.Append(" Select Distinct u.Name,u.Email,u.MobileNumber,u.RegistrationTypeId,u.PushTokenKey");
                            sb.Append(" From Users u Join UserCustomerMappings uc on u.Id = uc.UserId");
                            sb.Append(" Join UserRoles ur on u.Id = ur.UserId");
                            sb.Append(" Where uc.CustomerId in @DealerIds");
                            sb.Append(" and ur.RoleId = @RoleId");
                            sb.Append(" and u.SaudaBookingTypeId = @SaudaBookingTypeId");
                            var notificationBdoDetails = conn.Query<RaNotificationSendDto>(sb.ToString(),
                            new
                            {
                                DealerIds = dealerIds,
                                RoleId = (int)DTO.Enums.Role.StateTrader,
                                //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
                            }).ToList();

                            if (notificationBdoDetails.IsAny())
                            {
                                foreach (var bdoNotification in notificationBdoDetails)
                                {
                                    bdoNotification.IsEmail = true;
                                    bdoNotification.IsSMS = true;
                                    bdoNotification.IsInAppNotification = true;
                                    notificationUserDetails.Add(bdoNotification);
                                }
                            }
                            #endregion


                            if (notificationUserDetails.IsAny())
                            {
                                sb.Clear();
                                sb.Append(" Select Name,PlainTemplate,Template From EmailTemplates");
                                sb.Append(" Where Name = @Name1 or Name = @Name2");
                                var emailTemplateData = conn.Query<EmailTemplateDto>(sb.ToString(),
                                new
                                {
                                    Name1 = NotificationAction + "Email",
                                    Name2 = NotificationAction + "SMS"
                                }).ToList();

                                sb.Clear();
                                sb.Append(" Select SkuName From Skus");
                                sb.Append(" Where Id in @SkuIds");
                                var skuNames = conn.Query<string>(sb.ToString(),
                                new
                                {
                                    SkuIds = surpriseBenefitMailDto.SkuIds
                                }).ToList();

                                string SkuName = UtilityHelper.ConvertStringListToCommaSeparatedString(skuNames);

                                string PERCASE_OR_DAYS = string.Empty;
                                string DiscountOrDays = string.Empty;
                                if (surpriseBenefitMailDto.BenefitTypeId == (int)DTO.Enums.BenefitType.SAP)
                                {
                                    DiscountOrDays = (Math.Round(surpriseBenefitMailDto.BenefitDiscountOrDays, 0)).ToString();
                                    PERCASE_OR_DAYS = Constants.DAYS;
                                }
                                else
                                {
                                    DiscountOrDays = surpriseBenefitMailDto.BenefitDiscountOrDays.ToString();
                                    PERCASE_OR_DAYS = Constants.PERCASE;
                                }

                                #region Email

                                if (notificationUserDetails.IsAny())
                                {
                                    var plainTemplate = string.Empty;
                                    var htmlTemplate = string.Empty;
                                    var toEmails = notificationUserDetails.Where(w => w.IsEmail).Select(s => s.Email).Distinct().ToList();


                                    if (toEmails.IsAny())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var plainText = string.Empty;
                                        subject = NotificationAction;
                                        var emailTemplate = emailTemplateData.FirstOrDefault(f => f.Name == NotificationAction + "Email");

                                        if (emailTemplate != null)
                                        {
                                            plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, SkuName)
                                                .Replace(Constants.BENEFIT_TYPE, surpriseBenefitMailDto.BenefitType.ToString())
                                                .Replace(Constants.BENEFIT, surpriseBenefitMailDto.BenefitOrCategory.ToString())
                                                .Replace(Constants.DISCOUNTORDAYS, surpriseBenefitMailDto.DiscountOrDays)
                                                .Replace(Constants.PERCASE_OR_DAYS, PERCASE_OR_DAYS);

                                            htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toEmails, subject, plainText, htmlTemplate, true);
                                        }
                                    }
                                }
                                #endregion

                                foreach (var notification in notificationUserDetails)
                                {
                                    #region SMS

                                    var smsPlainTemplate = string.Empty;
                                    if (notification.IsSMS)
                                    {
                                        var smsMessage = string.Empty;
                                        var smsTemplate = emailTemplateData.FirstOrDefault(email => email.Name == NotificationAction + "SMS");
                                        if (smsTemplate != null)
                                        {
                                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, SkuName)
                                                .Replace(Constants.BENEFIT_TYPE, surpriseBenefitMailDto.BenefitType.ToString())
                                                .Replace(Constants.BENEFIT, surpriseBenefitMailDto.BenefitOrCategory.ToString())
                                                .Replace(Constants.DISCOUNTORDAYS, surpriseBenefitMailDto.DiscountOrDays)
                                                .Replace(Constants.PERCASE_OR_DAYS, PERCASE_OR_DAYS);

                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            amazonNotificationService.SendMessage(smsMessage, notification.MobileNumber);

                                        }
                                    }

                                    #endregion

                                    #region Push Notification

                                    if (notification.IsInAppNotification)
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = notification.PushTokenKey,
                                            RegistrationTypeId = notification.RegistrationTypeId,
                                            Title = subject,
                                            Message = smsPlainTemplate
                                        };
                                        SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }

                                    #endregion
                                }
                            }
                        }

                        #region Push Notification Nested Method
                        void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
                        {
                            try
                            {
                                sb.Clear();
                                sb.Append(" Select [Key],Value From Configurations");
                                sb.Append(" Where [Key] = @FirebaseSenderId or [Key] = @PushNotifyServerkey or [Key] = @PushNotifyUrl");
                                var pushNotificationData = conn.Query<PushNotificationsDto>(sb.ToString(),
                                new
                                {
                                    FirebaseSenderId = Constants.FirebaseSenderId,
                                    PushNotifyServerkey = Constants.PushNotifyServerkey,
                                    PushNotifyUrl = Constants.PushNotifyUrl
                                }).ToList();

                                if (pushNotificationData.IsAny())
                                {
                                    var firebaseSenderId = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                                    var pushNotifyServerkey = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                                    var pushNotifyUrl = pushNotificationData.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

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
            //return resultdto;
        }

        #endregion

        #region RA2.0 Sauda Create Mail Template

        public string SaudaCreateEmailTemplate(List<SaudaCreateNotificationDto> inputDto, string userName, int notificationType)
        {
            string windowName = inputDto.FirstOrDefault().WindowName;
            StringBuilder mainTemplate = new StringBuilder();
            mainTemplate.Append(" <table style='border-collapse: collapse;table-layout: fixed;width:100%;' id='tbl1' border='1'> ");
            mainTemplate.Append("	<tr>");
            mainTemplate.Append("       <td colspan='2' style='text-align: center;background-color: #c4c4c4;font-weight:bold;padding: 5px;'>BID ACCEPTED, CONGRATULATIONS!</td>");
            mainTemplate.Append("</tr>");
            if (notificationType == (int)DTO.Enums.NotificationType.SaudaCreation)
            {
                mainTemplate.Append("<tr>");
                mainTemplate.Append($"<td colspan='2' style='border-collapse: collapse;padding-top:25px;padding-bottom:25px;padding-left: 8px;'>Dear Customer, Thank you for participating in the Emami bidding process. Bid by {userName} through window {windowName} has been accepted.<br></td>");
                mainTemplate.Append("</tr>");
            }
            else if (notificationType == (int)DTO.Enums.NotificationType.CounterBidoffer)
            {
                mainTemplate.Append("<tr>");
                mainTemplate.Append($"<td colspan='2' style='border-collapse: collapse;padding-top:25px;padding-bottom:25px;padding-left: 8px;'>CONGRATULATIONS! Dear Customer, Your counter bid has been accepted. Your bid by {userName}. Thank you for participating in the Emami Bidding Process. Your order below listed.</td>");
                mainTemplate.Append("</tr>");
            }
            mainTemplate.Append("<tr>");
            mainTemplate.Append("<td style='padding: 5px;' colspan='2'>");
            mainTemplate.Append("     <table style='border-collapse: collapse;table-layout: fixed;width:100%;padding: 5px;' id='tbl3' border='1' align='center'>                                                                                  ");
            mainTemplate.Append("        <tr>                                                                                                                      ");
            mainTemplate.Append("          <td colspan='6' style='text-align: center;font-weight:bold;padding: 5px;'>Booked Sauda Details</td>");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("        <tr>                                                                                                                      ");
            mainTemplate.Append("          <td style='font-weight:bold;width: 80px !important;padding: 5px;text-align: center;'>Item Line</td>                                                                             ");
            mainTemplate.Append("          <td style='font-weight:bold;padding: 5px;text-align: center;' colspan='3'>SKU Description</td>                                                           ");
            mainTemplate.Append("		   <td style='font-weight:bold;width: 75px !important;padding: 5px;text-align: center;'>Qty</td>                                                                                      ");
            mainTemplate.Append("          <td style='font-weight:bold;width: 70px !important;padding: 5px;text-align: center;'>Bid Price</td>		                                                                             ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("		##SKUDETAILCONTENT##                                                                                                         ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("<td style='height:19px;padding:5px;' colspan='6'></td>");
            mainTemplate.Append("</tr>");
            mainTemplate.Append("<tr>");
            mainTemplate.Append("<td style='height:19px;padding:5px;' colspan='6'></td>");
            mainTemplate.Append("</tr>");
            mainTemplate.Append("</table>");
            mainTemplate.Append("</td>");
            mainTemplate.Append("</tr>");
            mainTemplate.Append("</table>");

            StringBuilder details = new StringBuilder();
            if (inputDto.IsAny())
            {
                int itemLine = 1;
                foreach (var sauda in inputDto)
                {
                    details.Append($"<tr><td style='text-align: center;'>{itemLine}</td><td colspan='3'>{sauda.SkuName}</td><td style='text-align: center;'>{sauda.BidQuantityInCase}</td><td style='text-align: center;'>{Utility.DecimalFormatTwo(sauda.BidPrice)}</td></tr>");
                    itemLine++;
                }
            }
            var result = mainTemplate.ToString().Replace("##SKUDETAILCONTENT##", details.ToString());
            return result;
        }

        public string SaudaCreateSmsTemplate(List<SaudaCreateNotificationDto> inputDto, string userName, int notificationType)
        {
            string windowName = inputDto.FirstOrDefault().WindowName;
            StringBuilder sb = new StringBuilder();

            if (notificationType == (int)DTO.Enums.NotificationType.SaudaCreation)
            {
                sb.AppendLine($"BID ACCEPTED: Congratulations! Dear Customer, thank you for participating in the Emami bidding process. Your bid by {userName} through window {windowName}  has been accepted.");
            }
            else if (notificationType == (int)DTO.Enums.NotificationType.CounterBidoffer)
            {
                sb.AppendLine($"CONGRATULATIONS! Dear Customer, your counter bid has been accepted. Your bid by {userName}. Thank you for participating in the Emami Bidding Process. Your order below listed.");
            }

            sb.AppendLine();
            int itemLine = 1;
            foreach (var sauda in inputDto)
            {
                sb.AppendLine();
                sb.AppendLine($"Item - {itemLine} : {sauda.SkuName}");
                sb.AppendLine($"Qty : {sauda.BidQuantityInCase} case");
                sb.AppendLine($"Bid Price : Rs. {Utility.DecimalFormatTwo(sauda.BidPrice)}");
                itemLine++;
            }
            sb.AppendLine();
            return sb.ToString();
        }

        #endregion

        public string GenerateLiftingRequestEmailTemplate(LiftingRequestNotificationDto liftingRequest)
        {
            StringBuilder mainTemplate = new StringBuilder();
            mainTemplate.Append(" <table style='border-collapse: collapse;table-layout: fixed;width:100%;' id='tbl1' border='1'> ");
            mainTemplate.Append("	<tr>");
            mainTemplate.Append("       <td colspan='2' style='text-align: center;background-color: #c4c4c4;font-weight:bold;padding: 5px;'>Your Sales Order Successfully Created</td>");
            mainTemplate.Append("    </tr>                                                                                                                         ");
            mainTemplate.Append("  <tr>                                                                                                                            ");
            mainTemplate.Append("    <td style='padding: 5px;' colspan='2'>                                                                                                              ");
            mainTemplate.Append("     <table style='border-collapse: collapse;table-layout: fixed;width:100%;' id='tbl2' border='0' align='center'>                                                                                  ");
            //mainTemplate.Append("        <tr>                                                                                                                      ");
            //mainTemplate.Append("          <td style='font-weight: bold;' colspan='3'>SAP Inquiry No : ##SAPINQUIRYNO##</td>                                       ");
            //mainTemplate.Append("          <td colspan='3'>Date & Time : ##SAPDATETIME##</td>                                                                      ");
            //mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("        <tr>                                                                                                                      ");
            mainTemplate.Append("          <td style='font-weight: bold;padding: 5px;' colspan='3'>APP Sales Order No : ##APPINDENTNO##</td>                                    ");
            mainTemplate.Append("		   <td style='padding: 5px;' colspan='3'>Date & Time : ##APPDATETIME##</td>                                                                         ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("		  <td style='height:20px;padding: 5px;' colspan='4'></td>                                                                                  ");
            mainTemplate.Append("		</tr>                                                                                                                        ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("          <td style='padding: 5px;' colspan='6'>Bill to Party name : ##BILLTOPARTYNAME##</td>                                                 ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("        <tr>                                                                                                                      ");
            mainTemplate.Append("          <td style='padding: 5px;' colspan='6'>Bill to Party place : ##BILLTOPARTYPLACE##</td>		                                         ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("          <td style='padding: 5px;' colspan='6'>Ship to Party name : ##SHIPTOPARTYNAME##</td>                                                           ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("          <td style='padding: 5px;' colspan='6'>Ship to Party place : ##SHIPTOPARTYPLACE##</td>                                                         ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("      </table>                                                                                                                    ");
            mainTemplate.Append("    </td>                                                                                                                         ");
            mainTemplate.Append("  </tr>                                                                                                                           ");
            mainTemplate.Append("  <tr>                                                                                                                            ");
            mainTemplate.Append("  <td style='padding: 5px;' colspan='2'>                                                                                                                ");
            mainTemplate.Append("     <table style='border-collapse: collapse;table-layout: fixed;width:100%;padding: 5px;' id='tbl3' border='1' align='center'>                                                                                  ");
            mainTemplate.Append("        <tr>                                                                                                                      ");
            mainTemplate.Append("          <td colspan='6' style='text-align: center;font-weight:bold;padding: 5px;'>Sales Order detail created in the SAP 	</td>                    ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("        <tr>                                                                                                                      ");
            mainTemplate.Append("          <td style='font-weight:bold;width: 80px !important;padding: 5px;text-align: center;'>Item Line</td>                                                                             ");
            mainTemplate.Append("          <td style='font-weight:bold;padding: 5px;text-align: center;' colspan='3'>Material Description</td>                                                           ");
            mainTemplate.Append("		   <td style='font-weight:bold;width: 75px !important;padding: 5px;text-align: center;'>Qty</td>                                                                                      ");
            mainTemplate.Append("          <td style='font-weight:bold;width: 70px !important;padding: 5px;text-align: center;'>UOM</td>		                                                                             ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("		##SKUDETAILCONTENT##                                                                                                         ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("          <td style='height:19px;padding:5px;' colspan='6'></td>		  	                                                                     ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("          <td style='height:19px;padding:5px;' colspan='6'></td>		  	                                                                     ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("		<tr>                                                                                                                         ");
            mainTemplate.Append("          <td style='padding:5px;' colspan='6'><span style='font-weight:bold'>Remarks from APP :</span> ##REMARKSFROMAPP##</td>                        ");
            mainTemplate.Append("        </tr>                                                                                                                     ");
            mainTemplate.Append("      </table>                                                                                                                    ");
            mainTemplate.Append("</td>                                                                                                                             ");
            mainTemplate.Append("</tr>                                                                                                                             ");
            mainTemplate.Append("</table>");

            StringBuilder details = new StringBuilder();
            if (liftingRequest.LiftingRequestSkuDetails != null && liftingRequest.LiftingRequestSkuDetails.Any())
            {
                foreach (var lifting in liftingRequest.LiftingRequestSkuDetails)
                {
                    details.Append($"<tr><td style='text-align: center;'>{lifting.ItemLine}</td><td colspan='3'>{lifting.Sku}</td><td style='text-align: center;'>{lifting.QtyInCase}</td><td style='text-align: center;'>{lifting.UOM}</td></tr>");
                }
            }

            var result = mainTemplate.ToString()
                .Replace("##APPINDENTNO##", liftingRequest.LiftingRequestNumber)
                .Replace("##APPDATETIME##", string.Format(Constants.LiftingRequestMailDatetimeFormat, liftingRequest.APPIndentNoCreatedDateTime))
                .Replace("##BILLTOPARTYNAME##", liftingRequest.BillToPartyName)
                .Replace("##BILLTOPARTYPLACE##", liftingRequest.BillToPartyPlace)
                .Replace("##SHIPTOPARTYNAME##", liftingRequest.ShipToPartyName)
                .Replace("##SHIPTOPARTYPLACE##", liftingRequest.ShipToPartyPlace)
                .Replace("##REMARKSFROMAPP##", liftingRequest.RemarksFromApp)
                .Replace("##SKUDETAILCONTENT##", details.ToString());

            return result;
        }

        public string GenerateLiftingRequestSmsTemplate(LiftingRequestNotificationDto liftingRequest)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"APP Indent no : {liftingRequest.LiftingRequestNumber}");
            sb.AppendLine($"Indent date time : {string.Format(Constants.LiftingRequestMailDatetimeFormat, liftingRequest.APPIndentNoCreatedDateTime)}"); sb.AppendLine($"Bill to Party name : {liftingRequest.BillToPartyName}");
            sb.AppendLine($"Ship to Party name : {liftingRequest.ShipToPartyName}");
            foreach (var lifting in liftingRequest.LiftingRequestSkuDetails)
            {
                sb.AppendLine();
                sb.AppendLine($"Item -{lifting.ItemLine} : { lifting.Sku}");
                sb.AppendLine($"Qty : { lifting.QtyInCase} case");
            }
            sb.AppendLine();
            sb.AppendLine($"Remarks from APP : {liftingRequest.RemarksFromApp}");
            return sb.ToString();
        }

        public ResultDto SendEmail(List<string> toEmailIds, string subject, string plainBody = "", string htmlContent = "", bool isHtml = false, string qrCode = "", bool isAttachment = false, string filePath = "", bool isCc = false, List<string> ccEmailId = null)
        {
            _methodName = "SendEmail";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + " Email Start");
            var resultDto = new ResultDto();
            try
            {
                if (ConsoleSettings.IsEmail)
                {
                    htmlContent = htmlContent.Replace("cid:footer", ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                    if (Constants.AwsEmail)
                    {
                        var fromEmail = Constants.FromEmail;
                        //var fromDisplayName = Constants.FromDisplayName;
                        var amazonS3Config = new AmazonSimpleEmailServiceConfig();
                        var newRegion = RegionEndpoint.GetBySystemName(Constants.AWSRegionName);
                        amazonS3Config.RegionEndpoint = newRegion;

                        using (var client = new AmazonSimpleEmailServiceClient(Constants.AWSEmailAccessKey, Constants.AWSEmailSecretKey, amazonS3Config))
                        {

                            var emailRequest = new SendEmailRequest()
                            {
                                Source = fromEmail,
                                Destination = new Destination(),
                                Message = new Message(),
                            };

                            var body = new Body()
                            {
                                Html = new Content(htmlContent),
                                Text = new Content(plainBody),

                            };

                            foreach (var toMailId in toEmailIds)
                            {
                                emailRequest.Destination.ToAddresses.Add(toMailId);
                            }

                            if (isCc && ccEmailId != null)
                            {
                                foreach (var cc in ccEmailId)
                                {
                                    emailRequest.Destination.CcAddresses.Add(cc);
                                }
                            }
                            if (!string.IsNullOrEmpty(Constants.CCEmail))
                            {
                                emailRequest.Destination.CcAddresses.Add(Constants.CCEmail);
                            }
                            emailRequest.Message.Subject = new Content(subject);
                            emailRequest.Message.Body = body;
                            client.SendEmail(emailRequest);
                            _logger.Info("Email Fired " + emailRequest);

                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = Constants.EmailSendSuccessfully;
                        }
                    }
                    else
                    {
                        if (toEmailIds != null && toEmailIds.Count > 0)
                        {
                            string tomails = string.Join(",", toEmailIds);

                            using (MailMessage mailMessage = new MailMessage())
                            {
                                MailAddress mailFrom = new MailAddress(Constants.SmtpFromMailAddress);
                                mailMessage.From = mailFrom;
                                mailMessage.Subject = subject;
                                mailMessage.Body = htmlContent;
                                mailMessage.IsBodyHtml = true;
                                mailMessage.To.Add(tomails);
                                if (!string.IsNullOrEmpty(Constants.CCEmail))
                                {
                                    mailMessage.CC.Add(Constants.CCEmail);
                                }

                                ////create Alrternative HTML view
                                //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, "text/html");
                                ////Add Image
                                //LinkedResource theEmailImage = new LinkedResource(ConfigurationManager.AppSettings["EmailFooterImageUrl"]);
                                //theEmailImage.ContentId = "footer";

                                ////Add the Image to the Alternate view
                                //htmlView.LinkedResources.Add(theEmailImage);

                                ////Add view to the Email Message
                                //mailMessage.AlternateViews.Add(htmlView);

                                SmtpClient smtp = new SmtpClient()
                                {
                                    Host = Constants.SmtpHostServerName,
                                    Port = Convert.ToInt32(Constants.SmtpNetworkCredentialPort),
                                    EnableSsl = Constants.SmtpEnableSsl,
                                    UseDefaultCredentials = true
                                };

                                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential()
                                {
                                    UserName = Constants.SmtpNetworkCredentialUserName,
                                    Password = Constants.SmtpNetworkCredentialPassword
                                };

                                smtp.Credentials = NetworkCred;
                                smtp.Send(mailMessage);
                                //Task.Run(() => smtp.Send(mailMessage));
                                _logger.Info("Email Fired " + mailMessage);
                            }

                            resultDto.IsSuccess = true;
                            resultDto.SuccessDto.Response = Constants.EmailSendSuccessfully;
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto.Message = Constants.EmailSendError;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;

            }
            return resultDto;
        }

        public string SendMessage(string smsMessage, string contactNumber, string TemplateId = "")
        {
            _methodName = "SendMessage";
            var message = $"{ServiceName} Service-Method {_methodName}";
            _logger.Info(message + " Send SMS Start");
            try
            {
                if (ConsoleSettings.IsSMS)
                {
                    if (!string.IsNullOrEmpty(TemplateId))
                    {
                        var httpClient = new HttpClient();
                        var SmsUrl = $"http://msg.cellapps.com/API/WebSMS/Http/v1.0a/index.php?username={Constants.SmsCodeZUserName}&password={Constants.SmsCodeZPassword}&sender=AWLSMS&to={contactNumber}&message={smsMessage}&reqid=1&format=TXT&route_id=&Template_ID={TemplateId}&PE_ID={Constants.PEID}";
                        var response = httpClient.GetAsync(SmsUrl).Result;
                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Send SMS exception");
                 message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);

            }
            _logger.Info("Send SMS End");
            return string.Empty;
        }

   
    }
}
