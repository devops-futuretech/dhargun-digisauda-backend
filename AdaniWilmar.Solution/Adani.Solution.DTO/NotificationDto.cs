using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class NotificationDto
    {
        public string Request { get; set; }
        public long RequestId { get; set; }
        public string Notification { get; set; }
        public DateTime? BiddingDate { get; set; }
        public TimeSpan? FromHour { get; set; }
        public TimeSpan? ToHour { get; set; }
        public DateTime? NotificationDateTime { get; set; }
        public long StatusId { get; set; }
        public long ReferenceId { get; set; }
        public long SaudaId { get; set; }
    }

    public class RaNotificationTypeDto
    {
        public bool SMS { get; set; }
        public bool Email { get; set; }
        public bool InAppNotification { get; set; }
    }

    public class EmailTemplateDto
    {
        public string Name { get; set; }
        public string PlainTemplate { get; set; }
        public string Template { get; set; }
    }

    public class PushNotificationsDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class RaNotificationSendDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSMS { get; set; }
        public bool IsEmail { get; set; }
        public bool IsInAppNotification { get; set; }
        public long DealerId { get; set; }
        public string MobileNumber { get; set; }
        public int RegistrationTypeId { get; set; }
        public string Email { get; set; }
        public string PushTokenKey { get; set; }
        public long BdoId { get; set; }
    }

    public class NotificationsStatusDto
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public string Name { get; set; }
        public string PlainTemplate { get; set; }
        public string Template { get; set; }
    }

    public class RaSaudaConfigurationDto
    {
        public TimeSpan SaudaAllocationTime { get; set; }
    }

    public class NotificationsSmsSendDto
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
    }

    public class NotificationsSmsSendInputDto
    {
        public string SmsContent { get; set; }
        public int RoleId { get; set; }
    }

    public class WindowCompleteNotificationDto
    {
        public bool IsEmail { get; set; }
        public bool IsSMS { get; set; }
        public bool IsInAppNotification { get; set; }
        public long DealerId { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int RegistrationTypeId { get; set; }
        public string PushTokenKey { get; set; }
        public long BdoId { get; set; }
        public string BdoEmail { get; set; }
        public string BdoMobileNumber { get; set; }
        public int BdoRegistrationTypeId { get; set; }
        public string BdoPushTokenKey { get; set; }
        public bool IsBooked { get; set; }
    }

    public class BdoNotificationDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int RegistrationTypeId { get; set; }
        public string PushTokenKey { get; set; }
    }    

    public class AboutWindowEndDto
    {
        public long BiddingWindowId { get; set; }
        public long NotificationTypeId { get; set; }
        public DateTime NotificationTime { get; set; }
        public long CustomerGroupId { get; set; }
    }

    public class SaudaExpiredNotificationAwlDto
    {     
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; } 
        public string SaudaNumber { get; set; }
        public string ExpiredDate { get; set; }
        public string CreatedDate { get; set; }
        public string PushTokenKey { get; set; }
       
    }

    public class OverDueNotificationAwlDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public decimal DueAmount { get; set; }
        public string DueDate { get; set; }      
        public string PushTokenKey { get; set; }

    }
}
