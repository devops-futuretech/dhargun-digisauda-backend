using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConditionalBookingReportInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<long> StateIds { get; set; }
        public int VerticalId { get; set; }
        public List<long> StatusIds { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long RoleId { get; set; }
        public long LoginUserId { get; set; }
    }
}
