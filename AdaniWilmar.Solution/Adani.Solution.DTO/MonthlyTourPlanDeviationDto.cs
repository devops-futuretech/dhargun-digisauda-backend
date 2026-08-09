using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyTourPlanDeviationDto
    {
        public long MTPId { get; set; }
        public long MTPDetailId { get; set; }
        public string DealerId { get; set; }
        public string Dealer { get; set; }
        public long ToDealerId { get; set; }
        public string ToDealer { get; set; }
        public int InHQNoVisitId { get; set; }
        public string InHQNoVisitName { get; set; }
        public string ActualDate { get; set; }
        public string RevisedDate { get; set; }
        public DateTime DeviationActualDate { get; set; }
        public DateTime DeviationRevisedDate { get; set; }
        public string Remarks { get; set; }
        public string Approval { get; set; }
        public bool IsChecked { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public long Id { get; set; }
        public string Area { get; set; }
        public long ReasonId { get; set; }
        public string Reason { get; set; }
        public string ApproverRemarks { get; set; }
        public string Town { get; set; }
        public string Reasons { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime PCPValidFrom { get; set; }
        public DateTime PCPValidTo { get; set; }
        public string PCPValidFromString { get; set; }
        public string PCPValidToString { get; set; }


        public long ApprovedBy { get; set; }
        public bool IsApprove { get; set; }

    }
}
