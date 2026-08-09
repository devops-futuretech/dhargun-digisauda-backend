using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PushNotificationInputDto
    {
        public long ToUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PushTokenKey { get; set; }   
        public int RegistrationTypeId { get; set; }
        public int NotificationTypeId { get; set; }
        public string Id { get; set; }
        public bool IsLogOut { get; set; }

        public string FirebaseSenderId { get; set; }
        public string PushNotifyServerkey { get; set; }
        public string PushNotifyUrl { get; set; }
        public object NotificationObject { get; set; }
        public bool IsCMSNotification { get; set; }
        public long SubmittedFormId { get; set; }
    }
}
