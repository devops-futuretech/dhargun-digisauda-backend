using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserSalesSaudaTargetDetailDto
    {
        public long Id { get; set; }
        public int MonthId { get; set; }
        public string Month { get; set; }
        public decimal SalesTarget { get; set; }
        public decimal SaudaTarget { get; set; }
    }
}
