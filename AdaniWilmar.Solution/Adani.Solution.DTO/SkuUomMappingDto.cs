using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SkuUomMappingDto
    {
        public long Id { get; set; }
        public decimal ConversionFactor { get; set; }
        public decimal ConversionFactor1 { get; set; }
        public decimal ConversionFactor2 { get; set; }
        public long SkuId { get; set; }
        public long UomId { get; set; }
        
    }
}
