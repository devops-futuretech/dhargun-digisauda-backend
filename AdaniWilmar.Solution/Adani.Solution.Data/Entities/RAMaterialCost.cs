using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Adani.Solution.Data.Entities
{
    public class RAMaterialCost : Auditable
    {
        public long PlantId { get; set; }
        public long? DivisionId { get; set; }
        public long OilTypeId { get; set; }
        public decimal RatePerMt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }

        public virtual Depot Plant { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Division Division { get; set; }
    }
}
