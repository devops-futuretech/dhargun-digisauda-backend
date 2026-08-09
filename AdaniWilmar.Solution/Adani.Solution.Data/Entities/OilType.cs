using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class OilType : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        [Required]
        public long DivisionId { get; set; }
      //  public decimal LitreConversion { get; set; }
        public bool IsActive { get; set; }
        public string SAPCode { get; set; }
      //  public bool IsRasoi { get; set; }

        public virtual Division Division { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }

    }
}
