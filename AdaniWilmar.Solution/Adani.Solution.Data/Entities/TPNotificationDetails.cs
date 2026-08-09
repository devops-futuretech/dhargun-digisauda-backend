using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class TPNotificationDetails : Auditable
    {
        public long TPNotificationId { get; set; }
        public long DealerId { get; set; }
        public long NotificationActionId { get; set; }
        public bool IsActive { get; set; }
        public virtual TPNotification TPNotification { get; set; }
        public virtual User Dealer { get; set; }
    }

}
