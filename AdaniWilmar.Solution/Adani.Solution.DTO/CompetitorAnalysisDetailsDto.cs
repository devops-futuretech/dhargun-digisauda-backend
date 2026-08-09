using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CompetitorAnalysisDetailsDto
    {
        public long CompetitorAnalysisId { get; set; }
        public long CompetitorId { get; set; }
        public string CompetitorName { get; set; }
        public decimal SaudaRate { get; set; }
        public decimal MarketOperatingPrice { get; set; }
        //public long WorkableQuantity { get; set; }
        //public decimal WorkablePrice { get; set; }
    }
}
