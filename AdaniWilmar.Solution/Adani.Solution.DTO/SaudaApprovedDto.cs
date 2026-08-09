using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaApprovedDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long BiddingWindowId { get; set; }
        public DateTime BiddingDateAndTime { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long IncotermId { get; set; }
        public string Incoterm { get; set; }
        public long PlantId { get; set; }
        public string Plant { get; set; }
        public long DepotId { get; set; }
        public string Depot { get; set; }
        public long CityId { get; set; }
        public string City { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SkuId { get; set; }
        public string Sku { get; set; }
        public decimal BidQuantityInCase { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal CounterBidRatePerCase { get; set; }
        public int StatusId { get; set; }
    }
}
