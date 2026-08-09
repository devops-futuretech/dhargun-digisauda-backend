using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class DistributionChannel: Auditable
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public long SalesOrganizationId { get; set; }

        public virtual SalesOrganization SalesOrganization { get; set; }
    }
}
