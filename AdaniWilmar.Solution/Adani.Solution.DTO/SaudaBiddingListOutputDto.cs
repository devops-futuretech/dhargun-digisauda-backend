using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaBiddingListOutputDto
    {
        public BiddingWindowDetails biddingWindowDetails { get; set; }
        public SaudaDetail SaudaDetail { get; set; }
        public List<SKUDetail> SKUDetail { get; set; }

        public SaudaBiddingListOutputDto()
        {
            SKUDetail = new List<SKUDetail>();
            biddingWindowDetails = new BiddingWindowDetails();
            SaudaDetail = new SaudaDetail();
        }
    }
    public class BiddingWindowDetails
    {
        public long BiddingWindowId { get; set; }
        public string BiddingWindowName { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StartEndTime { get; set; }
        public string WindowStatus { get; set; }
        public long WindowStatusId { get; set; }
        public DateTime ServerDateTime { get; set; }
    }
    public class SaudaDetail
    {
        public decimal TotalSaudaLimit { get; set; }
        public decimal AvailableSaudaLimit { get; set; }
        public long TotalChances { get; set; }
        public long ChancesLeft { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
    }
    public class SKUDetail
    {
        public long BiddingCartId { get; set; }
        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long IncotermId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public string SkuName { get; set; }
        public decimal GuaranteePrice { get; set; }
        public string IncotermName { get; set; }
        public string PlantName { get; set; }
        public string DepotName { get; set; }
        public decimal BidQuantityInCase { get; set; }
        public decimal BidQuantityMT { get; set; }
        public decimal AvailableBidQuantityForOilType { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal GuarateedPricePerCase { get; set; }
        public decimal CaseToMTValue { get; set; }
        public decimal SkuDiscount{ get; set; }
        public decimal AppliedVolumeDiscount { get; set; }
        public BiddingCartVolumeDiscount VolumeDiscount { get; set; }
        public decimal SchemeDiscount { get; set; }
        public long ChancesLeft { get; set; }
        public long TotalChances { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public long FreightRouteId { get; set; }
        public string FreightRouteName { get; set; }
        public decimal SurpriseDiscount { get; set; }

        public bool IsCounterBidSku { get; set; }
        public bool IsSaudaAllocated { get; set; }
        public decimal MaximumQuantity { get; set; }
        public decimal MinimumQuantity { get; set; }

        public decimal SkuWeightPerCase { get; set; }
        public decimal BidPricePerCaseWithoutTax { get; set; }
    }
}
