using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FinalPriceOutputDto
    {
        public long PricingId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        //public decimal MaterialCost { get; set; }
        //public decimal PackingCost { get; set; }
        //public decimal PrimaryFrieght { get; set; }
        //public decimal SecondaryFrieght { get; set; }
        //public decimal MarginCost { get; set; }
        //public decimal HoneycombCost { get; set; }
        //public decimal DepoCost { get; set; }
        //public decimal DetentionCost { get; set; }
        //public decimal CushionMarginCost { get; set; }
        //public decimal SchemeCost { get; set; }
        //public decimal Discount { get; set; }
        //public decimal Premium { get; set; }
        //public decimal RaMarginCost { get; set; }

        public decimal ExPlantPrice { get; set; }
        public decimal ForPlantPrice { get; set; }
        public decimal ExDepotPrice { get; set; }
        public decimal ForDepotPrice { get; set; }
        public long OilTypeId { get; set; }

        //public decimal ClearanceRate { get; set; }
        //public decimal CounterbidOffer { get; set; }
        //public decimal XMarginCost { get; set; }

        public decimal FinalPrice { get; set; }
        public decimal BdoDiscount { get; set; }
        public decimal BdoPremium { get; set; }

        //public long StateId { get; set; }
        //public long CityId { get; set; }
        //public long TransportModeId { get; set; }
        //public long OilPackingTypeId { get; set; }
    }
}
