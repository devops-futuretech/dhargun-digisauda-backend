using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserTargetDetailDto
    {
        public long Id { get; set; }
        public string MonthAndYear { get; set; }
        public long MonthId { get; set; }
        public string Month { get; set; }
        public long YearId { get; set; }
        public long Year { get; set; }
        public decimal SalesTarget { get; set; }
        public decimal SaudaTarget { get; set; }
    }
}
