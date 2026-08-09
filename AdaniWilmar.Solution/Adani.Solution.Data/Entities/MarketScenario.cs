using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class MarketScenario : Auditable
    {
        public long DealerId { get; set; }
        public string Title { get; set; }
        public string Remarks { get; set; }
    }
}
