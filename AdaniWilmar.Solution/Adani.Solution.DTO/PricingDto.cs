using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class PricingDto
    {
        public long Id { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilTypeName { get; set; }
        //public long SaudaBookingTypeId { get; set; }
        //public string SaudaBookingType { get; set; }
        public string OilPackingType { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        //public string TransportMode { get; set; }
        public string Plant { get; set; }
        public decimal Price { get; set; }
        //public string Depot { get; set; }
        //public string FrieghtZone { get; set; }
        //public string FrieghtRoute { get; set; }
        //public DateTime BiddingDate { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal PackingCost { get; set; }
        public decimal PrimaryFrieght { get; set; }
        public decimal SecondaryFrieght { get; set; }
        public decimal PlantSecondaryFrieght { get; set; }
        public decimal DepotCost { get; set; }
        public decimal DetentionCost { get; set; }
        public decimal HoneycombCost { get; set; }
        public decimal Margin { get; set; }
        public decimal CushionMargin { get; set; }
        public decimal SchemeCostRecovery { get; set; }
        public decimal Discount { get; set; }
        public decimal Premium { get; set; }
        public decimal ProcessCost { get; set; }
        public decimal SumOfIngredientCost { get; set; }
        public decimal TpPrice { get; set; }
        public decimal RaMargin { get; set; }
        public decimal BaseRate { get; set; }
        public decimal XMargin { get; set; }
        public decimal FinalRate { get; set; }
        public decimal ExPlantPrice { get; set; }
        public decimal ForDepotPrice { get; set; }
        public decimal ForPlantPrice { get; set; }
        public decimal ExDepotPrice { get; set; }
        public decimal ClearanceRate { get; set; }
        public decimal CounterBidOffer { get; set; }
        public decimal CounterBidLimit { get; set; }
        public decimal BpCpJumb { get; set; }
        public decimal ExRakePrice { get; set; }
        public decimal ForRakePrice { get; set; }
        public decimal Loadability { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long BiddingWindowId { get; set; }
        public string BiddingWindowTiming { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string ErrorMessage { get; set; }

        public decimal AdditionalCost { get; set; }
        public decimal OilTransferCost { get; set; }

        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public long OilPackTypeId { get; set; }
        public long TransPortModeId { get; set; }
    }

    public class PricingTPandRAInputDto : LoginUserIdDto
    {
        public DateTime CreatedDate { get; set; }
        public DateTime BiddingDate { get; set; }
        public int SaudaBookingTypeId { get; set; }
        public long BiddingWindowId { get; set; }        
    }   

    public class PricingMailDto
    {
        public List<long> CustomerGroupIds { get; set; }
        public long BiddingWindowId { get; set; }
        public int NotificationActionId { get; set; }
    }


    public class BiddingWindowDashboardReportDto
    {
        public string BiddingWindowName { get; set; }
        public string CustomerGroup { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string DealerState { get; set; }
        public string DealerCity { get; set; }

        public string BdoName { get; set; }
        public string BdoState { get; set; }
        public string BdoCity { get; set; }

        public string SaudaBookingType { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilName { get; set; }
        public string PackGroupName { get; set; }
        public int Incotermid { get; set; }
        public decimal QuotedPrice { get; set; }
        public decimal BidQuantityInMT { get; set; }
        public decimal BidQuantityInCase { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal GuarateedPricePerCase { get; set; }
        public int GPBenefitAppliedTypeId { get; set; }

        public decimal SchemeDiscount { get; set; }
        public decimal VolumeDiscount { get; set; }
        public decimal SkuDiscount { get; set; }
        public decimal SchemeDiscountCase { get; set; }
        public decimal VolumeDiscountCase { get; set; }
        public decimal SkuDiscountCase { get; set; }

        public string GPBenefitType { get; set; }
        public string GPBenefitAppliedType { get; set; }
        public string GPBenefitCategory { get; set; }
        public decimal GPBenefitDiscountOrDay { get; set; }

        public string SaudaValidFrom { get; set; }
        public string SaudaValidTo { get; set; }
        public decimal SaudaValidityDays { get; set; }

        public string Status { get; set; }
        public decimal CounterBidOffer { get; set; }
        public string CounterBidStatus { get; set; }

        public string State { get; set; }
        public string TransportMode { get; set; }
        public decimal LoadQuantity { get; set; }
        public string Plant { get; set; }
        public string Depot { get; set; }
        public string FreightZone { get; set; }
        public string FreightRoute { get; set; }
        public string BiddingDate { get; set; }

        public decimal MaterialCost { get; set; }
        public decimal PackingCost { get; set; }
        public decimal PrimaryFrieght { get; set; }
        public decimal SecondaryFrieght { get; set; }
        public decimal DepotCost { get; set; }
        public decimal DetentionCost { get; set; }
        public decimal HoneycombCost { get; set; }
        public decimal CustomerGroupMargin { get; set; }
        public decimal RAMargin { get; set; }
        public decimal SchemeCostRecovery { get; set; }
        public decimal SumOfIngredientCost { get; set; }

        public decimal ExPlantCGST { get; set; }
        public decimal ExPlantSGST { get; set; }
        public decimal ExPlantIGST { get; set; }
        public decimal ForPlantSGST { get; set; }
        public decimal ForPlantCGST { get; set; }
        public decimal ForPlantIGST { get; set; }
        public decimal ExDepotSGST { get; set; }
        public decimal ExDepotCGST { get; set; }
        public decimal ExDepotIGST { get; set; }
        public decimal ForDepotSGST { get; set; }
        public decimal ForDepotCGST { get; set; }
        public decimal ForDepotIGST { get; set; }
        public decimal PlantGSTPercentage { get; set; }
        public decimal DepotGSTPercentage { get; set; }

        public decimal ExPlantPrice { get; set; }
        public decimal ForDepotPrice { get; set; }
        public decimal ForPlantPrice { get; set; }
        public decimal ExDepotPrice { get; set; }

        public decimal ExPlantGuaranteePrice { get; set; }
        public decimal ForPlantGuaranteePrice { get; set; }
        public decimal ExDepotGuaranteePrice { get; set; }
        public decimal ForDepotGuaranteePrice { get; set; }

    }
    public class TPPricingExportDto
    {
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilTypeName { get; set; }
        public string SaudaBookingType { get; set; }
        public string OilPackingType { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string TransportMode { get; set; }
        public decimal Loadability { get; set; }
        public string Plant { get; set; }
        public string Depot { get; set; }
        public string FrieghtZone { get; set; }
        public string FrieghtRoute { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal PackingCost { get; set; }
        public decimal PrimaryFrieght { get; set; }
        public decimal SecondaryFrieght { get; set; }
        public decimal PlantSecondaryFrieght { get; set; }
        public decimal DepotCost { get; set; }
        public decimal DetentionCost { get; set; }
        public decimal HoneycombCost { get; set; }
        public decimal Margin { get; set; }
        public decimal CushionMargin { get; set; }
        public decimal SchemeCostRecovery { get; set; }
        public decimal ExPlantPrice { get; set; }
        public decimal ForPlantPrice { get; set; }
        public decimal ExDepotPrice { get; set; }
        public decimal ForDepotPrice { get; set; }
        public decimal ExRakePrice { get; set; }
        public decimal ForRakePrice { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal OilTransferCost { get; set; }
    }
}
