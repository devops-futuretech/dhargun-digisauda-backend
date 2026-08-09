using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class PendingSaudaRemarks : Auditable
    {
        public long DealerId { get; set; }
        public long SaudaId { get; set; }
        public string Remarks { get; set; }
    }
}
