using System;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class MonthlyPlanDeviations : Auditable
    {
        [Required]
        public long MonthlyTourPlanDetailsId { get; set; }
        [Required]
        public DateTime RevisedDate { get; set; }
        public string Remarks { get; set; }
        [Required]
        public long ApproverId { get; set; }
        [Required]
        public long StatusId { get; set; }
        public string ApproverRemarks { get; set; }
        public long ReasonId { get; set; }
        [Required]
        public long ToDealerId { get; set; }
        public string ToDealer { get; set; }

        public virtual MonthlyTourPlanDetails MonthlyTourPlanDetails { get; set; }
    }
}
