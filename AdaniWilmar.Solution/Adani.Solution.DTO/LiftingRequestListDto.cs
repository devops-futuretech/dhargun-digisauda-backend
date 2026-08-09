using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestListDto
    {
        public long Id { get; set; }
        public DateTime LiftingDate { get; set; }
        public double TotalBidPrice { get; set; }
        public long TotalBidQuantity { get; set; }
        public long PendingliftQuantity { get; set; }
        public string TradeTicketNumber { get; set; }
        public string SaudaNumber { get; set; }
    }
}
