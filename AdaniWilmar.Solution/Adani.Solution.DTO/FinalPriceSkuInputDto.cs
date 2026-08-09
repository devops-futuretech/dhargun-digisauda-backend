using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FinalPriceSkuInputDto
    {
        public long LoginUserId { get; set; }
        public bool IsZonalHead { get; set; }
        public long DealerId { get; set; }
        public long BDOId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long OilTypeId { get; set; }
        //public long PlantDepotId { get; set; }
        public long BiddingWindowId { get; set; }
        public long PlantId { get; set; }
        public long SkuId { get; set; }
    }
}
