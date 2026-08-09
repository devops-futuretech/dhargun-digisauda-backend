using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DashboardWeekwiseOverallSaudaDto
    {
        public decimal TotalTarget { get; set; }
        public decimal OverallSauda { get; set; }
        public List<OverallSaudaWeekWiseAchievementDto> OverallWeekWiseAchievements { get; set; }
        public DashboardWeekwiseOverallSaudaDto()
        {
            OverallWeekWiseAchievements = new List<OverallSaudaWeekWiseAchievementDto>();
        }
    }

    public class DashboardWeekwiseOverallSalesDto
    {
        public decimal TotalTarget { get; set; }
        public decimal OverallSales { get; set; }
        public List<OverallSaudaWeekWiseAchievementDto> OverallWeekWiseAchievements { get; set; }
        public DashboardWeekwiseOverallSalesDto()
        {
            OverallWeekWiseAchievements = new List<OverallSaudaWeekWiseAchievementDto>();
        }
    }
}
