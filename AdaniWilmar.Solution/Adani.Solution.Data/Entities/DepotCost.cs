using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class DepotCost : Auditable
    {
        public long DepotId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long? SkuId { get; set; }
        public long? OilTypeId { get; set; }
        public long? OilPackingTypeId { get; set; }
        public decimal RatePerMt { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public virtual Depot Depot { get; set; }
        public virtual Division Division { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual PackGroup OilPackingType { get; set; }
    }
}
