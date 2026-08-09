using System;

namespace Adani.Solution.DTO
{
    public class SaudaReportOutputDto
    {
        public long SaudaOrderId { get; set; }
        public string OilTypeName { get; set; }
        public string SkuName { get; set; }
        public string DealerName { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal BookingQuantity { get; set; }
        public decimal BookingQuantityCase { get; set; }
        public decimal BookingPrice { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string LiftingStatus { get; set; }
        public DateTime LiftingRequestDate { get; set; }
        public decimal LiftedQuantity { get; set; }
        public decimal LiftedQuantityCase { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal PendingQuantityCase { get; set; }
        public string LiftingRemarks { get; set; }
        public decimal CounterBidOffer { get; set; }
    }

    public class RaSaudaOrederReportDto
    {
        public string BiddingWindow { get; set; }
        public string CustomerGroup { get; set; }

        public string BdoName { get; set; }
        public string BdoState { get; set; }
        public string BdoCity { get; set; }

        public string DealerName { get; set; }
        public string DealerState { get; set; }
        public string DealerCity { get; set; }

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

        public decimal CounterBidOffer { get; set; }
        public string CounterBidStatus { get; set; }

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
    public class NewSaudaReportOutputDto
    {
        public string SaudaNumber { get; set; }
        public long BookedNumber { get; set; }
        public string Plant { get; set; }
        public string OilTypeName { get; set; }
        public string SkuName { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal QuantityInMT { get; set; }
        public string SkuCode { get; set; }
        public DateTime BiddingDate { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public decimal SaudaBidPrice { get; set; }
        public string Incoterms { get; set; }
        public string FreightRoute { get; set; }
        public string Status { get; set; }
        public string BookingType { get; set; }
        public string CreatedBy { get; set; }
        public string State { get; set; }
        public string BdoName { get; set; }
        public string BdoCode { get; set; }
        public string PackGroup { get; set; }
        public string ContractValidFrom { get; set; }
        public string CustomerGroupOne { get; set; }
        public string CustomerGroupTwo { get; set; }
        public string Uom { get; set; }
        public string Depot { get; set; }
        public string BrokerName { get; set; }
        public decimal SaleRate { get; set; }
        public long SaudaOrderId { get; set; }
    }

}
