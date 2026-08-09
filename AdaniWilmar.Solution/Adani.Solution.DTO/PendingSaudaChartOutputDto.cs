using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingSaudaChartOutputDto
    {
        public long UserId { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal BidQuantity { get; set; }
        public String SaudaNumber { get; set; }
    }
    public class PendingSaudaChartSPDto
    {
        public long UserId { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal BidQuantity { get; set; }
        public String SaudaNumber { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
    }
}
