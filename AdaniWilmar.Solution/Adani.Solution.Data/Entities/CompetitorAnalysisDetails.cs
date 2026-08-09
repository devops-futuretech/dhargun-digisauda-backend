using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class CompetitorAnalysisDetails : Auditable
    {
        public long CompetitorAnalysisId { get; set; }
        public long CompetitorId { get; set; }
        public decimal SaudaRate { get; set; }
        public decimal MarketOperatingPrice { get; set; }

        public virtual Competitor Competitor { get; set; }
        public virtual CompetitorAnalysis CompetitorAnalysis { get; set; }
    }
}
