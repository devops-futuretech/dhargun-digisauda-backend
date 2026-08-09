using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyPlanDeviationListDto
    {
        public long MonthlyTourPlanDetailsId { get; set; }
        public string PlannedDate { get; set; }
        public string RevisedDate { get; set; }
        public string Remarks { get; set; }
        public long ApproverId { get; set; }
        public long StatusId { get; set; }
        public long Id { get; set; }
        public string Approval { get; set; }
        public long ReasonId { get; set; }
        public string Reasons { get; set; }
        public long CreatedBy { get; set; }
        public string Dealer { get; set; }
        public long ToDealerId { get; set; }
        public string ToDealer { get; set; }
        public string InHQNoVisitName { get; set; }
    }
}
