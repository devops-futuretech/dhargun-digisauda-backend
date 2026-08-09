using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BiddingCartSkuOutputDto
    {
        public long PricingId { get; set; }
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
        public decimal CaseToMTValue { get; set; }

        public decimal SkuDiscount { get; set; }
        public decimal SkuDiscountType { get; set; }

        public decimal SchemeDiscount { get; set; }
        public decimal SchemeDiscountType { get; set; }

        public decimal AppliedVolumeDiscount { get; set; }
        public int AppliedVolumeDiscountType { get; set; }

        public BiddingCartVolumeDiscount VolumeDiscount { get; set; }
        public int VolumeDiscountType { get; set; }

        public long ChancesLeft { get; set; }
        public long TotalChances { get; set; }
        public long FreightRouteId { get; set; }
        public string FreightRouteName { get; set; }
        public decimal BaseRate { get; set; }

        //public string BenefitSap { get; set; }
        //public string BenefitNonSap { get; set; }
        //public long BenefitDays { get; set; }
        //public decimal BenefitDiscount { get; set; }

        public decimal startVolumeSlabInMT { get; set; }
        public decimal endVolumeSlabInMT { get; set; }

        public long GPBenefitType { get; set; }
        public long GPBenefitOrCategoryId { get; set; }
        public string GPBenefitOrCategory { get; set; }
        public long GPBenefitAppliedTypeId { get; set; } 
        public decimal GPBenefitDiscountOrDay { get; set; }
        public decimal SkuWeightPerCase { get; set; }
        public long TodayPricingId { get; set; }

    }

    public class BiddingCartSkuInputDto
    {
        public List<long> OilTypeIds { get; set; }
        public long IncotermId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long BiddingWindowId { get; set; }
        public long DealerId { get; set; }
        public DateTime BiddingDate { get; set; }
        public long BaseSkuId { get; set; }
        public long SaudaOrderId { get; set; }
    }

    public class BiddingCartVolumeDiscount
    {
        public List<RAVolumeDiscountDto> VolumeDiscount { get; set; }
        public int VolumeDiscountType { get; set; }
        public BiddingCartVolumeDiscount()
        {
            VolumeDiscount = new List<RAVolumeDiscountDto>();
        }
    }
}
