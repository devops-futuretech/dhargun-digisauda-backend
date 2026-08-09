using Adani.Solution.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class SpecialRate : Auditable
    {
        [Required]
        public long UserId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long PricingId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal Quantity { get; set; }

        public decimal QuantityCase { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal SpecialPrice { get; set; }
        public long StatusId { get; set; }
        public string Remarks { get; set; }
        public string Incoterms1 { get; set; }
        public long Incoterms2 { get; set; }
        public long DepotId { get; set; }
        //public long FreightRouteId { get; set; }//Freight Route
        public bool IsLTD { get; set; }
        public long BrokerId { get; set; }
        public string SaudaLimitExceedRemarks { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public virtual User User { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual Status Status { get; set; }
        public virtual Depot Depot { get; set; }
        //public virtual FreightRoute FreightRoute { get; set; }
    }
}
