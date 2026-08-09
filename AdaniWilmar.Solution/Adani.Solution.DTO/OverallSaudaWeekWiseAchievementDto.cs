using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class OverallSaudaWeekWiseAchievementDto
    {
        public int WeekId { get; set; }
        public string Week { get; set; }
        public decimal Achievement { get; set; }
        public decimal Target { get; set; }
    }
}
