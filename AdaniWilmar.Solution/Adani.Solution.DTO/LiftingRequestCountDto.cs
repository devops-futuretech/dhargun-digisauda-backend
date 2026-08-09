using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestCountDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long? ShipToPartyId { get; set; }
        public string ShipToParty { get; set; }
        public long TotalLiftingCount { get; set; }
        public bool IsCreatedBy { get; set; }
    }
}
