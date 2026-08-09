using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class HoneycombCost : Auditable
    {
        public long? PlantId { get; set; }
        public long? DivisionId { get; set; }
        public long? OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long TransportModeId { get; set; }
        public long ZoneId { get; set; }
        public int? StateId { get; set; }
        public decimal RatePerMt { get; set; }

        public decimal RatePerCase { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public virtual Depot Plant { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Division Division { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual TransportMode TransportMode { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual State State { get; set; }
    }
}
