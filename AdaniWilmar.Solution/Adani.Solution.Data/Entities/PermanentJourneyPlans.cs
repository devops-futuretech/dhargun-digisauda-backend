using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class PermanentJourneyPlans : Auditable
    {
        public string PJPNumber { get; set; }
        [Required]
        public long PermanentJourneyPlanStatusId { get; set; }
        [Required]
        public long FinancialYearId { get; set; }
        public string Remarks { get; set; }
        public bool Isactive { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EffectiveFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EffectiveTo { get; set; }

        public virtual PermanentJourneyPlanStatus PJPStatusName { get; set; }
        public virtual FinancialYear Year { get; set; }
        public virtual ICollection<PermanentJourneyPlanDetails> PJPDetails { get; set; }
        public virtual ICollection<PermanentJourneyPlanApprovalInformation> PJPApprovalInformation { get; set; }
    }
}
