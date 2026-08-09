using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConditionalBookingSkuMapping : Auditable
    {
        public long SaudaConditionalConfigurationId { get; set; }
        [Required]
        public string EssentialSku { get; set; }
        [Required]
        public string MandatorySku { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public virtual SaudaConditionalBookingConfiguration SaudaConditionalConfiguration { get; set; }
    }
}
