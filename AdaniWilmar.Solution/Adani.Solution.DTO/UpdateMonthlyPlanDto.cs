using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UpdateMonthlyPlanDto
    {
        public long Id { get; set; }
        public long MonthlyTourPlanDetailsId { get; set; }
        public string PlannedDate { get; set; }
        public string RevisedDate { get; set; }
        public string Remarks { get; set; }
        public long ApproverId { get; set; }
        public long StatusId { get; set; }
        public long ModifiedBy { get; set; }
    }
}
