using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PermanentJourneyPlanDto
    {
        public long PJPId { get; set; }
        public string PJPNumber { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public long CustomerId { get; set; }
        public string Customer { get; set; }
        public long LoginUserId { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Remarks { get; set; }
        public string ReasonIds { get; set; }
        public DateTime EffectiveFrom { get; set; } 
        public DateTime EffectiveTo { get; set; } 
        public List<PJPApprovalInformationDto> PJPApprovalInformationList { get; set; }
        public List<PermanentJourneyPlanDetailsDto> PermanentJourneyPlanDetails { get; set; }
        public PermanentJourneyPlanDto()
        {
            PermanentJourneyPlanDetails = new List<PermanentJourneyPlanDetailsDto>();
        }
    }
}
