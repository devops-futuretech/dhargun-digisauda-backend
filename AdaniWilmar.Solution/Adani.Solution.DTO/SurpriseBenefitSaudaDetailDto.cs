using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitSaudaDetailDtoOld
    {
        public long SaudaOrderId { get; set; }
        public long SaudaId { get; set; }
        public long CustomerId { get; set; }
        public long OilTypeId { get; set; }
        public long PackTypeId { get; set; }
        public long SkuId { get; set; }
        public long CityId { get; set; }
        public long StateId { get; set; }
        public long StatusId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long FreightZoneId { get; set; }
        public long FreightRouteId { get; set; }
        public long TransportModeId { get; set; }
        public long PricingId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long BiddingwindowId { get; set; }
        public long CustomerGroupId { get; set; }

        public string BiddingWindow { get; set; }
        public string CustomerGroup { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string BidRate { get; set; }
        public string BidRatePerCase { get; set; }
        public string BidQuantityInCase { get; set; }
        public string BidQuantityInMT { get; set; }
        public string Status { get; set; }
        public string MarginPerCase { get; set; }
        public string BDOName { get; set; }
        public string SchemeDiscount { get; set; }
        public string VolumeDiscount { get; set; }
        public string SkuDiscount { get; set; }
        public string GPBenefitType { get; set; }
        public string GPBenefitDiscountOrDays { get; set; }
        public string OilTypeName { get; set; }
        public string OilPackingType { get; set; }
        public string SaudaBookingType { get; set; }
        public string StateName { get; set; }
        public string FrieghtZone { get; set; }
        public string FrieghtRoute { get; set; }
        public string LoadQuantity { get; set; }
        public string TransportMode { get; set; }
        public string PlantName { get; set; }
        public string DepotName { get; set; }
        public string BiddingDate { get; set; }
        public string MaterialCost { get; set; }
        public string PackingCost { get; set; }
        public string PrimaryFrieght { get; set; }
        public string SecondaryFrieght { get; set; }
        public string PlantSecondaryFrieght { get; set; }
        public string DepotCost { get; set; }
        public string DetentionCost { get; set; }
        public string HoneycombCost { get; set; }
        public string SchemeCostRecovery { get; set; }
        public string RaMargin { get; set; }
        public string CushionMargin { get; set; }
        public string CustomerGroupMargin { get; set; }
        public string SumOfIngredientCost { get; set; }
        public string ExPlantSGST { get; set; }
        public string ExPlantCGST { get; set; }
        public string ForPlantSGST { get; set; }
        public string ForPlantCGST { get; set; }
        public string ExPlantIGST { get; set; }
        public string ForPlantIGST { get; set; }
        public string ExDepotSGST { get; set; }
        public string ExDepotCGST { get; set; }
        public string ForDepotSGST { get; set; }
        public string ForDepotCGST { get; set; }
        public string ExDepotIGST { get; set; }
        public string ForDepotIGST { get; set; }
        public string ExPlantPrice { get; set; }
        public string ForDepotPrice { get; set; }
        public string ForPlantPrice { get; set; }
        public string ExDepotPrice { get; set; }
        public string ExRakePrice { get; set; }
        public string ForRakePrice { get; set; }
        public string ExPlantGuaranteePrice { get; set; }
        public string ForPlantGuaranteePrice { get; set; }
        public string ExDepotGuaranteePrice { get; set; }
        public string ForDepotGuaranteePrice { get; set; }
    }

    public class SurpriseBenefitSaudaDetailDto
    {
        public string BiddingWindow { get; set; }
        public string CustomerGroup { get; set; }
        public string BdoName { get; set; }
        public string DealerName { get; set; }
        public string SaudaBookingType { get; set; }
        public string OilTypeName { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilPackingType { get; set; }
        public string BiddingDate { get; set; }

        public decimal BidPriceTotal { get; set; }
        public decimal BidQuantityInMT { get; set; }
        public decimal BidPriceAfterDiscount { get; set; }
        public decimal BidQuanityInCase { get; set; }
        public decimal BidRatePerCase { get; set; }
        public decimal BidPriceAfterDiscountPerCase { get; set; }
        public decimal BaseRate { get; set; }
        public decimal MarginPerCase { get; set; }

        public string Status { get; set; }
        public string PlantName { get; set; }
        public string DepotName { get; set; }
        public string StateName { get; set; }
        public string FrieghtZone { get; set; }
        public string FrieghtRoute { get; set; }
        public string LoadQuantity { get; set; }
        public string TransportMode { get; set; }

        public decimal SchemeDiscount { get; set; }
        public decimal VolumeDiscount { get; set; }
        public decimal SkuDiscount { get; set; }

        public string GPBenefitType { get; set; }
        public string GPBenefitAppliedType { get; set; }
        public string GPBenefitCategory { get; set; }
        public decimal GPBenefitDiscountOrDay { get; set; }

        public string SurpriseBenefitType { get; set; }
        public string SurpriseBenefitAppliedType { get; set; }
        public string SurpriseBenefitBenefitCategory { get; set; }
        public decimal SurpriseBenefitDiscountOrDay { get; set; }

        public string SaudaValidFrom { get; set; }
        public string SaudaValidTo { get; set; }
        public decimal SaudaValidityDays { get; set; }

        public decimal MaterialCost { get; set; }
        public decimal PackingCost { get; set; }
        public decimal PrimaryFrieght { get; set; }
        public decimal SecondaryFrieght { get; set; }
        public decimal PlantSecondaryFrieght { get; set; }
        public decimal DepotCost { get; set; }
        public decimal DetentionCost { get; set; }
        public decimal HoneycombCost { get; set; }
        public decimal SchemeCostRecovery { get; set; }
        public decimal RaMargin { get; set; }
        public decimal CustomerGroupMargin { get; set; }
        public decimal SumOfIngredientCost { get; set; }

        public decimal ExPlantSGST { get; set; }
        public decimal ExPlantCGST { get; set; }
        public decimal ForPlantSGST { get; set; }
        public decimal ForPlantCGST { get; set; }
        public decimal ExPlantIGST { get; set; }
        public decimal ForPlantIGST { get; set; }
        public decimal ExDepotSGST { get; set; }
        public decimal ExDepotCGST { get; set; }
        public decimal ForDepotSGST { get; set; }
        public decimal ForDepotCGST { get; set; }
        public decimal ExDepotIGST { get; set; }
        public decimal ForDepotIGST { get; set; }

        public decimal ExPlantPrice { get; set; }
        public decimal ForDepotPrice { get; set; }
        public decimal ForPlantPrice { get; set; }
        public decimal ExDepotPrice { get; set; }

        public decimal ExPlantGuaranteePrice { get; set; }
        public decimal ForPlantGuaranteePrice { get; set; }
        public decimal ExDepotGuaranteePrice { get; set; }
        public decimal ForDepotGuaranteePrice { get; set; }

    }
}
