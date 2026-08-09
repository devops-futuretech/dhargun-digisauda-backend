using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaAllocationDetailsDto
    {
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime BiddingDate { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public decimal QuotedPrice { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }

        public List<SKUDetail> SKUDetail { get; set; }

        public SaudaAllocationDetailsDto()
        {
            SKUDetail = new List<SKUDetail>();
        }

    }
}
