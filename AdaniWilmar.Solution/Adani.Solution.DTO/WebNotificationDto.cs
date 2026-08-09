using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class WebNotificationDto
    {
        public long ToUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int PushNotificationTypeId { get; set; }
        public int WebNotificationTypeId { get; set; }
    }
}
