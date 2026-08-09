using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingSaudaReportInput
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> PlantIds { get; set; }
    }
}
