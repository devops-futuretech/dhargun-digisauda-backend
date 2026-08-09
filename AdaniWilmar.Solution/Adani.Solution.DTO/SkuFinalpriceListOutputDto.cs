using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SkuFinalpriceListOutputDto
    {
        public long Id { get; set; }
        public long SkuId { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal PackingCost { get; set; }
        public decimal PrimaryFrieght { get; set; }
        public decimal SecondaryFrieght { get; set; }
        public decimal SecondaryFrieghtForPlant { get; set; }
        public decimal MarginCost { get; set; }
        public decimal HoneycombCost { get; set; }
        public decimal DepoCost { get; set; }
        public decimal DetentionCost { get; set; }
        public decimal CushionMarginCost { get; set; }
        public decimal SchemeCost { get; set; }
        public decimal Discount { get; set; }
        public decimal Premium { get; set; }
        public decimal RaMarginCost { get; set; }

        public decimal ExPlantPrice { get; set; }
        public decimal ForPlantPrice { get; set; }
        public decimal ExDepotPrice { get; set; }
        public decimal ForDepotPrice { get; set; }
        public decimal ExRakePrice { get; set; }
        public decimal ForRakePrice { get; set; }

        public decimal ClearanceRate { get; set; }
        public decimal CounterbidOffer { get; set; }
        [UIHint("XMarginCostEditorFor")]
        public decimal XMarginCost { get; set; }

        public decimal TpPrice { get; set; }
        public decimal BaseRate { get; set; }
        public decimal FinalPrice { get; set; }

        public decimal IngredientCost { get; set; }

        public long StateId { get; set; }
        public long CityId { get; set; }
        public long DistrictId { get; set; }
        public long TerritoryId { get; set; }
        public long TransportModeId { get; set; }
        public string TransportMode { get; set; }
        public long OilPackingTypeId { get; set; }
        public bool IsAddedForPricing { get; set; }
        public decimal LoadQuantity { get; set; }

        public string CityName { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string TerritoryName { get; set; }
        public string FreightRouteName { get; set; }

        public long FreightZoneId { get; set; }
        public long FreightRouteId { get; set; }

        public bool IsChecked { get; set; }

    }

    public class FinalpriceListOutputDto
    {
        public IList<SkuFinalpriceListOutputDto> SkuFinalpriceList { get; set; }
        public List<string> ErrorMessage { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
