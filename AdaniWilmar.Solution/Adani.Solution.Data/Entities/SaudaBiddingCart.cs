using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaBiddingCart : Auditable
    {
        public long BiddingWindowId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime BiddingDateAndTime { get; set; }
        public long DealerId { get; set; }
        public long IncotermId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal GuarateedPricePerCase { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal BidPricePerCase { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal BidQuantityInCase { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal BidQuantityInMT { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal TotalPrice { get; set; }
        public long ChanceNumber { get; set; }
        public long TotalChance { get; set; }
        public long StatusId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal SchemeDiscount { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal SchemeDiscountCase { get; set; }
        public int SchemeDiscountType { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal VolumeDiscount { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal VolumeDiscountCase { get; set; }
        public int VolumeDiscountType { get; set; }
        //[DecimalPrecision(18, 4)]
        //public decimal GeographyVolumeDiscount { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal SkuDiscount { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal SkuDiscountCase { get; set; }
        public int SkuDiscountType { get; set; }

        public long SaudaBiddingCartHeaderId { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal BaseRate { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal CounterBidOffer { get; set; }

        public long CounterBidStatusId { get; set; }
        public bool IsSaudaAllocated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidFromDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidToDate { get; set; }


        public int GPBenefitType { get; set; }
        public long GPBenefitOrCategoryId { get; set; }
        public long GPBenefitAppliedTypeId { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal GPBenefitDiscountInCase { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal GPBenefitDiscountOrDay { get; set; }
        public long PricingId { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal BidPrice { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal BaseBidQuantityInCase { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal CounterBidPrice { get; set; }

        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual User Dealer { get; set; }
        public virtual BiddingWindow BiddingWindow { get; set; }
        public virtual IncoTerms Incoterm { get; set; }
        public virtual SaudaBiddingCartHeader SaudaBiddingCartHeader { get; set; }
    }
}
