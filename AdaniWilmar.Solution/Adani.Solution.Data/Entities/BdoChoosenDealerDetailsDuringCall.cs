using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BdoChoosenDealerDetailsDuringCall : Auditable
    {
        public long DealerId { get; set; }
        public string DealerMobileNumber { get; set; }
        public long BDOId { get; set; }
        public string BDOMobileNumber { get; set; }
    }
}
