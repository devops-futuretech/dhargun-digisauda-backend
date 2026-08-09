using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ChatBotPendingSaudaAndDueDto
    {
        public int DealersCount { get; set; }
        public decimal PendingSaudaQuantity { get; set; }
        public decimal ExpiredSaudaQuantity { get; set; }
        public decimal NearExpiredSaudaQuantity { get; set; }
        public decimal TotalOverDue { get; set; }
        public decimal TotalDueForTomorrow { get; set; }
    }
}
