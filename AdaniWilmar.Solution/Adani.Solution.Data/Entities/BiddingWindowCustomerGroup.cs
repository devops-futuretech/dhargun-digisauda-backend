using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BiddingWindowCustomerGroups : Auditable
    {
        public long BiddingWindowId { get; set; }

        public long CustomerGroupId { get; set; }

        public virtual CustomerGroups CustomerGroup { get; set; }
    }
}
