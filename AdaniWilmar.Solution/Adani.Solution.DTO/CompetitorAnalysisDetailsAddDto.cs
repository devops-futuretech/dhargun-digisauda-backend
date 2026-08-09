using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class CompetitorAnalysisDetailsAddDto
    {
        public long CompetitorId { get; set; }
        public decimal SaudaRate { get; set; }
        public decimal MarketOperatingPrice { get; set; }
    }
}
