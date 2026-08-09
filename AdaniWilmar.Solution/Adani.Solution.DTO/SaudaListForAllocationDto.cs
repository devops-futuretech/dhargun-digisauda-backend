using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaListForAllocationDto
    {
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime BiddingDate { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long BiddingWindowId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }

        public decimal QuotedPrice { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }

        public string BiddingWindowName { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StartEndTime { get; set; }
        public string WindowStatus { get; set; }
        public long WindowStatusId { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime SkuAllocationTimeLimit { get; set; }
        public DateTime SaudaAllocationStartTime { get; set; }
        public DateTime SaudaAllocationEndTime { get; set; }

        public int SaudaAllocationStatusId { get; set; }
        public long BiddingCartHeaderId { get; set; }

        public SKUDetail SKUDetail { get; set; }

    }
}
