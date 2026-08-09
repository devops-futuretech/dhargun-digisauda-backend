using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class CustomerLedgerDetails : Auditable
    {
        public decimal Balance { get; set; }
        public long UserId { get; set; }
    }
}
