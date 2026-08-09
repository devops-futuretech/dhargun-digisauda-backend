using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class PermanentJourneyPlanUpdateDto:LoginDealerIdDto
    {
        public long PJPId { get; set; }
        public string PJPNumber { get; set; }
        public long StatusId { get; set; }
        public long FinancialYearId { get; set; }
        public string Remarks { get; set; }
        public long ModifiedBy { get; set; }
        public List<PermanentJourneyPlanDetailsDto> PermanentJourneyPlanDetails { get; set; }
        public List<PJPApprovalInformationDto> PJPApprovalInformation { get; set; }
        public long IsEditedByAdmin { get; set; }
        public string ReasonIds { get; set; }
    }

    public class SalesTourPlanPcpHistoryDto : IAPIInputDTO
    {
        public string DealerName { get; set; }
        public string FinancialYear { get; set; }
        public string State { get; set; }
        public string Territory { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string NoOfDirectDealer { get; set; }
        public string NoofSubDealer { get; set; }
        public string NoOfWholeSeller { get; set; }
        public long NoOfVisit { get; set; }
        public long PermanentJourneyPlanDetailId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public int InHQNoVisitId { get; set; }
        public string InHQNoVisitName { get; set; }

        public string Remarks { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        
    }

    public class SalesTourPlanMtpHistoryDto : IAPIInputDTO
    {
        public string DealerName { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Headquarters { get; set; }
        public string Remarks { get; set; }
        public DateTime TourDate { get; set; }
        public int InHQNoVisitId { get; set; }
        public string InHQNoVisitName { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
