using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BiddingWindowNotificationTiming : Auditable
    {
        public long BiddingWindowId { get; set; }
        public long NotificationTypeId { get; set; }
        public DateTime NotificationTime{ get; set; }
        public long StatusId { get; set; }
        public long CustomerGroupId { get; set; }
    }
}
