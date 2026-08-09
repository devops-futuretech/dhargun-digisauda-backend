using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConversionUnitAndDifferenceRateDetail : Auditable
    {
        [Required]
        public long SaudaConversionUnitAndDifferenceRateId { get; set; }
        [Required]
        public long ToPackGroupId { get; set; }
        [Required]
        public long ToSkuId { get; set; }
        [Required]
        [DecimalPrecision(18, 3)]
        public decimal ToUnit { get; set; }
        [Required]
        public decimal BasicRate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public virtual SaudaConversionUnitAndDifferenceRate SaudaConversionUnitAndDifferenceRate { get; set; }
    }
}
