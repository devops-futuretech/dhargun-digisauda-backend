using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class PermanentJourneyPlanApprovalInformation : Auditable
    {
        [Required]
        public long PermanentJourneyPlanId { get; set; }
        [Required]
        public long StatusId { get; set; }
        [Required]
        public long UserId { get; set; }
        public string Remarks { get; set; }
        public string ReasonId { get; set; }

        public virtual PermanentJourneyPlans PermanentJourneyPlan { get; set; }
    }
}
