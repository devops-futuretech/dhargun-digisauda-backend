using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class CounterBidNotification : Auditable
    {
        public long BiddingWindowId { get; set; }
        public long SaudaBiddingCartId { get; set; }
        public decimal CounterBidOffer { get; set; }
        public long StatusId { get; set; }
        public long SkuId { get; set; }
        public long DealerId { get; set; }
    }
}
