using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealerHubDetailDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public decimal BGSGGiven { get; set; }
        public decimal CurrentLimit { get; set; }
        public decimal Sales { get; set; }
        public decimal SaudaOutStatnding { get; set; }
        public decimal SaudaOutStandingMT { get; set; }
        public long VisitsDone { get; set; }
        public decimal CollectionDue { get; set; }
    }
}
