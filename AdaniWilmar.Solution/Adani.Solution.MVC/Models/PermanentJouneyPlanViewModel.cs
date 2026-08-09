using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class PermanentJouneyPlanViewModel
    {
        public List<MonthViewModel> MonthList { get; set; }
        public List<PermanentJourneyPlanListViewModel> PermanentJourneyPlanList { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public long CustomerId { get; set; }
        public string Customer { get; set; }
        public long LoginUserId { get; set; }
        public DateTime EffectiveFrom { get; set; } = DateTime.Now;
        public DateTime EffectiveTo { get; set; } = DateTime.Now;

        public long MonthId { get; set; }
        public string EncryptedId { get; set; }
        public long DistrictId { get; set; }

        public long CityId { get; set; }
        public string NoOfDirectDealer { get; set; }
        public string NoOfSubDealer { get; set; }
        public string NoOfWholeSeller { get; set; }
        public string NoOfVisit { get; set; }
        public long PJPId { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public long CreatedBy { get; set; }
        public long IsEditableForCreatedUser { get; set; }
        public long IsApprover { get; set; }
        public string Remarks { get; set; }
        public long IsEditableForAdmin { get; set; }
        public string ReasonIds { get; set; }

        public long StateId { get; set; }
        public long TerritoryId { get; set; }
        public string District { get; set; }
        public string City { get; set; }

        public List<PermanentJourneyPlanDetailsDto> PermanentJourneyPlanDetailList { get; set; }

        public List<PJPApprovalFlowViewModel> PJPApprovalFlowList { get; set; }
        public PermanentJouneyPlanViewModel()
        {
            PJPApprovalFlowList = new List<PJPApprovalFlowViewModel>();
            PermanentJourneyPlanDetailList = new List<PermanentJourneyPlanDetailsDto>();
        }
    }
}