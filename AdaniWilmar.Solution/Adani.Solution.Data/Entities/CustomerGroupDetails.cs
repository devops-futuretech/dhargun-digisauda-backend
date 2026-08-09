using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class CustomerGroupDetails : Auditable
    {
        public long CustomerGroupId { get; set; }
        public long CustomerId { get; set; }

        public virtual CustomerGroups CustomerGroup { get; set; }
        public virtual User Customer { get; set; }
    }
}
