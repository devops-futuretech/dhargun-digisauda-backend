using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FinalPriceInputDto
    {
        public long DealerId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long SkuId { get; set; }
        public long PlantDepotId { get; set; }
        public long IncoTermsId { get; set; }
        public long BiddingWindowId { get; set; }
        public long LoginUserId { get; set; }
    }
}
