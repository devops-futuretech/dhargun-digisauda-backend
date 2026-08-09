using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class PrimaryFreight : Auditable
    {
        public long? PlantId { get; set; }
        public long DepotId { get; set; }
        public long VerticalId { get; set; }
        public long TransportModeId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal LoadCapacity { get; set; }

        public decimal HireCost { get; set; }
        public decimal ActualFreight { get; set; }
        public decimal SalesFreight { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public virtual Depot Plant { get; set; }
        public virtual Depot Depot { get; set; }
        public virtual Division Vertical { get; set; }
        public virtual TransportMode TransportMode { get; set; }
    }
}
