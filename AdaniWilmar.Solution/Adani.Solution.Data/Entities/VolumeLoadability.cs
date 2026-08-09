using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class VolumeLoadability : Auditable
    {
        public long SkuId { get; set; }
        public long PlantId { get; set; }
        public decimal MaxAllowableMultiplesku { get; set; }
        public decimal MaxAllowableSinglesku { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public decimal VehicleSize { get; set; }
        public virtual Depot Plant { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
