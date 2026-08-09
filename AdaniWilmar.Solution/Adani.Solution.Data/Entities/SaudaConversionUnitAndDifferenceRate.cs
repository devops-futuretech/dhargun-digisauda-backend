using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class SaudaConversionUnitAndDifferenceRate : Auditable
    {
        [Required]
        public long FromPackGroupId { get; set; }
        [Required]
        public long FromSkuId { get; set; }        
        [Required]
        [DecimalPrecision(18, 3)]
        public decimal FromUnit { get; set; }        
        [Column(TypeName = "datetime2")]
        public DateTime FromDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ToDate { get; set; }
        [Required]
        public long SourceId { get; set; }
        [Required]
        public long StateId { get; set; }
    }
}
