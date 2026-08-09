using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class MonthlyTourPlans : Auditable
    {
        public string MTPNumber { get; set; }
        [Required]
        public int MonthlyTourPlanStatusId { get; set; }
        public string Remarks { get; set; }
        public long PJPId { get; set; }
        public long MonthId { get; set; }

        public virtual MonthlyTourPlanStatus MonthlyTourPlanStatus { get; set; }
        public virtual ICollection<MonthlyTourPlanDetails> MTPDetails { get; set; }
        public virtual ICollection<MonthlyTourPlanApprovalInformation> MTPApprovalInformation { get; set; }
    }
}
