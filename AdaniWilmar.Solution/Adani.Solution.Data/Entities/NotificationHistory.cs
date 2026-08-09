using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class NotificationHistory : Auditable
    {
        public long NotificationActionId { get; set; }
        public long BiddingWindowId { get; set; }
        public long CustomerGroupId { get; set; }
        public long CustomerId { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSms { get; set; }
        public bool IsPushNotification { get; set; }
    }
}
