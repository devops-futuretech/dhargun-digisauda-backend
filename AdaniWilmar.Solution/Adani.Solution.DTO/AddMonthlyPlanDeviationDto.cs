using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddMonthlyPlanDeviationDto
    {
        public List<MonthlyPlanDeviationListDto> monthlyPlanDeviationListDto { get; set; }
        public long CreatedBy { get; set; }
    }
}
