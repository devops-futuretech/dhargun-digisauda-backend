using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyPlanDeviationUpdateDto
    {
        public List<MonthlyPlanDeviationListDto> monthlyPlanDeviationListDto { get; set; }
        public long ModifiedBy { get; set; }
    }

    public class MonthlyPlanDeviationDto : LoginUserIdDto
    {
        public long MTPDeviationId { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
    }
}
