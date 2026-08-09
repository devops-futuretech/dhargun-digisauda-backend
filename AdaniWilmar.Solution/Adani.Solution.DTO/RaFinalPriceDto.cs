using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RaFinalPriceDto
    {
        public long PriceGenerateDetailId { get; set; }
        public int SaudaBookingTypeId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> PackGroup { get; set; }
        public int Plant { get; set; }
        public List<long> BiddingWindowId { get; set; }
        public List<long> CustomerGroupId { get; set; }
    }
}
