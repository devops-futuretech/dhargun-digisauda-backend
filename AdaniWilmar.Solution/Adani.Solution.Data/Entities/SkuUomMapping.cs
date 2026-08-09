using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SkuUomMapping : Auditable
    {
        [Required]
        public long SkuId { get; set; }

        public long UomId { get; set; }
        public long RelationUomId { get; set; }
        public decimal ConversionFactor { get; set; }
        public decimal ConversionFactor1 { get; set; }
        public decimal ConversionFactor2 { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
