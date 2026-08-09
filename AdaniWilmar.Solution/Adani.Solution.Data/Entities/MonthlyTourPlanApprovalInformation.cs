using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class MonthlyTourPlanApprovalInformation : Auditable
    {
        [Required]
        public long MonthlyTourPlanId { get; set; }
        [Required]
        public int MonthlyTourPlanStatusId { get; set; }
        [Required]
        public long UserId { get; set; }
        public string Remarks { get; set; }
        public string ReasonId { get; set; }

        public virtual MonthlyTourPlans MonthlyTourPlan { get; set; }
        //public virtual MonthlyTourPlanStatus MonthlyTourPlanStatus { get; set; }
    }
}
