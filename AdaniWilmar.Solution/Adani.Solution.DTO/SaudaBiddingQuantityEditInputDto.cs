using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaBiddingQuantityEditInputDto
    {
        public List<SaudaBiddingQuantity> SaudaBiddingQuantity { get; set; }
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public long BiddingWindowId { get; set; }

        public SaudaBiddingQuantityEditInputDto()
        {
            SaudaBiddingQuantity = new List<SaudaBiddingQuantity>();
        }
    }

    public class SaudaBiddingQuantity
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long Quantity { get; set; }
        public decimal VolumeDiscountCal { get; set; }
    }
}
