using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class PermanentJourneyPlanDetails : Auditable
    {
        [Required]
        public long PermanentJourneyPlanId { get; set; }
        [Required]
        public string RetailerId { get; set; }
        public long MonthId { get; set; }
        public long StateId { get; set; }
        public long TerritoryId { get; set; }
        public long DistrictId { get; set; }
        public long TownId { get; set; }
        public string NoOfDirectDealer { get; set; }
        public string NoofSubDealer { get; set; }
        public string NoOfWholeSeller { get; set; }
        public decimal NoOfVisit { get; set; }
        public int InHQNoVisit { get; set; }
        public string Remarks { get; set; }

        public virtual PermanentJourneyPlans PermanentJourneyPlan { get; set; }
    }
}
