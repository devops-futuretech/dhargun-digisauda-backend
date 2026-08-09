using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DailyBookedSaudaInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<long> NationalHeadIds { get; set; }
        public List<long> Dealers { get; set; }
        public List<long> BDOs { get; set; }
        public List<long> ZHs { get; set; }
        public List<long> OilTypes { get; set; }
        public List<long> PackTypes { get; set; }
        public long OilPackGroupTypes { get; set; }
        public List<long> StateIds { get; set; }
        public long LoginUserId { get; set; }
        public long PlantId { get; set; }
    }
}
