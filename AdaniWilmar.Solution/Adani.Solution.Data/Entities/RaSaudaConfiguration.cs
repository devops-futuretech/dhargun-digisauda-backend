using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class RaSaudaConfiguration : Auditable
    {
        public decimal GuaranteePricePercentage { get; set; }

        public TimeSpan SaudaAllocationTime { get; set; }

        public bool IsActive { get; set; }
    }
}
