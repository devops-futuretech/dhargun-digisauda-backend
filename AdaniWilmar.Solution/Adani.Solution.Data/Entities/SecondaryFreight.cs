using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class SecondaryFreight : Auditable
    {
        public long DepotId { get; set; }
        public int? StateId { get; set; }
        public long? ZoneId { get; set; }
        public long? FreightZoneId { get; set; }
        public long? FreightRouteId { get; set; }
        public long TransportModeId { get; set; }
        public long VerticalId { get; set; }
        public decimal ActualFreight { get; set; }
        public decimal SalesFreight { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal Capacity { get; set; }

        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        //public virtual FreightZone FreightZone { get; set; }
        //public virtual FreightRoute FreightRoute { get; set; }
        public virtual Depot Depot { get; set; }
        public virtual TransportMode TransportMode { get; set; }
        public virtual Division Vertical { get; set; }
        public virtual State State { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
