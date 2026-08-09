using System;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class MonthlyTourPlanDetails : Auditable
    {
        [Required]
        public long MonthlyTourPlanId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int TownId { get; set; }
        public string Area { get; set; }
        //[Required]
        public string DealerId { get; set; }
        public long HeadquartersId { get; set; }
        public string Remarks { get; set; }
        public int InHQNoVisit { get; set; }
        public string VisitRemarks { get; set; }

        public virtual MonthlyTourPlans MonthlyTourPlan { get; set; }
        //public virtual Headquarters Headquarters { get; set; }
        //public virtual City Town { get; set; }
    }
}
