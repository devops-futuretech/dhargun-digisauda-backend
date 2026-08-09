using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class CounterBidJump : Auditable
    {
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidTo { get; set; }
        public long OilTypeId { get; set; }
        public long DivisionId { get; set; }
        public long PackGroupId { get; set; }
        public decimal CounterbidJump { get; set; }
        public bool IsActive { get; set; }

        public virtual OilType OilType { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
        public virtual PackGroup PackGroup { get; set; }
    }
}
