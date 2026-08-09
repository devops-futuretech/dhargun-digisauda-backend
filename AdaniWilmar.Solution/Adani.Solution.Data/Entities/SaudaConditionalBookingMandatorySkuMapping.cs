using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConditionalBookingMandatorySkuMapping : Auditable
    {
        [Required]
        public long ConditionalBookingEssentialSkuMappingId { get; set; }
        [Required]
        public long MandatorySkuId { get; set; }
        [Required]
        public string MandatorySkuCode { get; set; }
        [Required]
        public decimal MandatorySkuPercentage { get; set; }
        [Required]
        public long OilTypeId { get; set; }
        [Required]
        public long PackGroupId { get; set; }
        public virtual SaudaConditionalBookingEssentialSkuMapping ConditionalBookingEssentialSkuMapping { get; set; }
    }
}
