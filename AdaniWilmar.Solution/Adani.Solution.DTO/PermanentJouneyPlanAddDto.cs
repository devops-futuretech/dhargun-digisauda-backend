using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PermanentJouneyPlanAddDto
    {
        public string PJPNumber { get; set; }
        public long StatusId { get; set; }
        public long FinancialYearId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public List<PermanentJourneyPlanDetailsDto> PermanentJourneyPlanDetails { get; set; }
        public List<PJPApprovalInformationDto> PJPApprovalInformation { get; set; }
    }
}
