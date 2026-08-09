using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaModification : Auditable
    {
        public string SaudaNumber { get; set; }
        public int StatusId { get; set; }
        public bool IsSentToSAP { get; set; }
        public string Remarks { get; set; }
    }
}
