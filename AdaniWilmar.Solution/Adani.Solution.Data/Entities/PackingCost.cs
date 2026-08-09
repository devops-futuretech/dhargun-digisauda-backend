using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class PackingCost : Auditable
    {
        public long DivisionId { get; set; }
        public long OilTypeId { get; set; }
        public long? SkuId { get; set; }
        public long PlantId { get; set; }
        public decimal ActualPackingCost { get; set; }
        public decimal SalesPackingCost { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }

        public virtual Depot Plant { get; set; }
        public virtual Division Division { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
