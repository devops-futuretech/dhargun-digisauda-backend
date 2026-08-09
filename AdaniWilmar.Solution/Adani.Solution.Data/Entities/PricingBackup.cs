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
    public class PricingBackup : Auditable
    {
        public string SAPPricingCode { get; set; }
        [Required]
        public long SkuId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public decimal Price { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DivisionId { get; set; }
        public long DistributionChannelId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        //[Required]
        //public long OilTypeId { get; set; }

        //[Required]
        //public long SaudaBookingTypeId { get; set; }
        //public long OilPackingTypeId { get; set; }
        //public int StateId { get; set; }
        //public int CityId { get; set; }
        //public long TransportModeId { get; set; }
        //public long FrieghtZoneId { get; set; }
        //public long FrieghtRouteId { get; set; }
        //public long BiddingWindowId { get; set; }

        //[Column(TypeName = "datetime2")]
        //public DateTime BiddingDate { get; set; }

        //public decimal MaterialCost { get; set; }
        //public decimal PackingCost { get; set; }
        //public decimal PrimaryFrieght { get; set; }
        //public decimal SecondaryFrieght { get; set; }
        //public decimal DepotCost { get; set; }
        //public decimal DetentionCost { get; set; }
        //public decimal HoneycombCost { get; set; }
        //public decimal Margin { get; set; }
        //public decimal CushionMargin { get; set; }
        //public decimal SchemeCostRecovery { get; set; }
        //public decimal Discount { get; set; }
        //public decimal Premium { get; set; }

        //public decimal ProcessCost { get; set; }
        //public decimal SumOfIngredientCost { get; set; }

        //public decimal TpPrice { get; set; }
        //public decimal RaMargin { get; set; }
        //public decimal BaseRate { get; set; }
        //public decimal XMargin { get; set; }
        //public decimal FinalRate { get; set; }

        //public decimal ExPlantPriceWithoutGst { get; set; }
        //public decimal ForPlantPriceWithoutGst { get; set; }
        //public decimal ExDepotPriceWithoutGst { get; set; }
        //public decimal ForDepotPriceWithoutGst { get; set; }

        //public decimal ExPlantGst { get; set; }
        //public decimal ForPlantGst { get; set; }
        //public decimal ExDepotGst { get; set; }
        //public decimal ForDepotGst { get; set; }

        //public decimal ExPlantPrice { get; set; }
        //public decimal ForDepotPrice { get; set; }
        //public decimal ForPlantPrice { get; set; }
        //public decimal ExDepotPrice { get; set; }
        //public decimal ExRakePrice { get; set; }
        //public decimal ForRakePrice { get; set; }

        //public decimal ExPlantGuaranteePrice { get; set; }
        //public decimal ForPlantGuaranteePrice { get; set; }
        //public decimal ExDepotGuaranteePrice { get; set; }
        //public decimal ForDepotGuaranteePrice { get; set; }
        //public decimal ExRakeGuaranteePrice { get; set; }
        //public decimal ForRakeGuaranteePrice { get; set; }

        //public decimal ClearanceRate { get; set; }
        //public decimal CounterBidOffer { get; set; }

        //public decimal CounterBidLimit { get; set; }
        //public decimal BpCpJumb { get; set; }

        //public bool IsActive { get; set; }
        //public decimal PlantSecondaryFrieght { get; set; }

        //[DecimalPrecision(18, 4)]
        //public decimal LoadQuantity { get; set; }

        public long? PublishId { get; set; }
        public bool IsPublish { get; set; }
        //public long MaterialCostId { get; set; }
        //public string IngredientCostId { get; set; }
        //public long PackingCostId { get; set; }
        //public long DepotCostId { get; set; }
        //public long DetentionCostId { get; set; }
        //public long ProfitMarginId { get; set; }
        //public long CushionMarginId { get; set; }
        //public long SchemeCostId { get; set; }
        //public long PrimaryFrieghtId { get; set; }
        //public long SecondaryFrieghtId { get; set; }
        //public long SecondaryFrieghtForPlantId { get; set; }
        //public long HoneycombCostId { get; set; }
        //public long RaMarginId { get; set; }
        //public long LoadCapacityId { get; set; }
        //public long SkuIngrediantPlantId { get; set; }
        //public long CustomerGroupId { get; set; }
        //public decimal GPjump { get; set; }

        //public decimal ExPlantSGST { get; set; }
        //public decimal ExPlantCGST { get; set; }
        //public decimal ExPlantIGST { get; set; }

        //public decimal ForPlantSGST { get; set; }
        //public decimal ForPlantCGST { get; set; }
        //public decimal ForPlantIGST { get; set; }

        //public decimal ExDepotSGST { get; set; }
        //public decimal ExDepotCGST { get; set; }
        //public decimal ExDepotIGST { get; set; }

        //public decimal ForDepotSGST { get; set; }
        //public decimal ForDepotCGST { get; set; }
        //public decimal ForDepotIGST { get; set; }

        //public long GstId { get; set; }
        //public long CustomerGroupMarginId { get; set; }
        //public decimal CustomerGroupMargin { get; set; }

        //public decimal PlantGSTPercentage { get; set; }
        //public decimal DepotGSTPercentage { get; set; }

        //public long AdditionalCostId { get; set; }
        //public decimal AdditionalCost { get; set; }
        //public long OilTransferCosForPlantId { get; set; }
        //public decimal OilTransferCostForPlant { get; set; }
        //public long OilTransferCosForDepotId { get; set; }
        //public decimal OilTransferCostForDepot { get; set; }
        //public long PricingReferneceId { get; set; }
    }
}
