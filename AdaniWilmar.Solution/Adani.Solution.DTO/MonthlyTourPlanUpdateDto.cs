using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyTourPlanUpdateDto:LoginUserIdDto
    {
        public long MTPId { get; set; }
        public string MTPNumber { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long ModifiedBy { get; set; }
        public long IsEditedByAdmin { get; set; }
        public string ReasonIds { get; set; }
        public List<MonthlyTourPlanDetailsDto> MonthlyTourPlanDetails { get; set; }
    }
}
