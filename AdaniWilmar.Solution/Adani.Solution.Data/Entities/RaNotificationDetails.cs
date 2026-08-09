using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class RaNotificationDetails : Auditable
    {
        public long RaNotificationId { get; set; }
        public long CustomerGroupId { get; set; }
        public long DealerId { get; set; }
        public long NotificationActionId { get; set; }
        public string WindowVolumeCapacity { get; set; }
        public bool IsActive { get; set; }
        public virtual RaNotification RaNotification { get; set; }
        public virtual CustomerGroups CustomerGroup { get; set; }
        public virtual User Dealer { get; set; }
    }
}
