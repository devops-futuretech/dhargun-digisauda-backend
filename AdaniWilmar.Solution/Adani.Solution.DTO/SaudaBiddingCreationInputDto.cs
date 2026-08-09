using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaBiddingCreationInputDto
    {
        public long BiddingWindowId { get; set; }
        public DateTime BiddingDateAndTime { get; set; }
        public long DealerId { get; set; }
        public long LoginUserId { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaAllocationTime { get; set; }
        public string Message { get; set; }
        public List<SaudaBiddingDetailsDto> SaudaBiddingDetails { get; set; }
        public SaudaBiddingCreationInputDto()
        {
            SaudaBiddingDetails = new List<SaudaBiddingDetailsDto>();
        }
    }
    public class SaudaBiddingDetailsDto
    {
        public long SaudaBiddingCartHeaderId { get; set; }
        public long SaudaBiddingCartId { get; set; }
        public long PricingId { get; set; }
        public long OilTypeId { get; set; }
        public long IncotermId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long SkuId { get; set; }
        public decimal GuarateedPricePerCase { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal BidQuantityInMT { get; set; }
        public decimal BidQuantityInCase { get; set; }
        public decimal SkuDiscountUsers { get; set; }
        public decimal SkuDiscountGeography { get; set; }
        public decimal VolumeDiscountUsers { get; set; }
        public decimal VolumeDiscountGeography { get; set; }
        public decimal SchemeDiscountUsers { get; set; }
        public decimal SchemeDiscountGeography { get; set; }
        public long ChanceNumber { get; set; }
        public long TotalChance { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public decimal BaseRate { get; set; }
        public string SkuName { get; set; }

        public decimal SkuDiscount { get; set; }
        public decimal SchemeDiscount { get; set; }
        public decimal VolumeDiscountCal { get; set; }

        public int SkuDiscountType { get; set; }
        public int SchemeDiscountType { get; set; }
        public int VolumeDiscountType { get; set; }

        public int GPBenefitType { get; set; }
        public long GPBenefitOrCategoryId { get; set; }
        public string GPBenefitOrCategory { get; set; }
        public long GPBenefitAppliedTypeId { get; set; }
        public decimal GPBenefitDiscountOrDay { get; set; }
        public long TodayPricingId { get; set; }
    }
}
