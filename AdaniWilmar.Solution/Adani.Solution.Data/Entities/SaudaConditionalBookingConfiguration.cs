using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConditionalBookingConfiguration : Auditable
    {
        [Required]
        public long SalesOrganizationId { get; set; }
        [Required]
        public long DistributionChannelId { get; set; }
        [Required]
        public long DivisionId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
    }
}
