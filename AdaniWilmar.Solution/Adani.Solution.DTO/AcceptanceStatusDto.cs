using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AcceptanceStatusDto
    {
        public long UserId { get; set; }
        public decimal TotalCount { get; set; }
        public decimal PendingCount { get; set; }
        public decimal AcceptedCount { get; set; }
        public decimal RejectedCount { get; set; }
    }
}
