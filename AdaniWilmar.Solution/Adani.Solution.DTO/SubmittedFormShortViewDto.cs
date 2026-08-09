using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SubmittedFormShortViewDto
    {
        public long SubmittedFormId { get; set; }
        public long SubmittedDependentFormId { get; set; }
        public string DemonstratedBy { get; set; }
        public string DemoInchargeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long FormId { get; set; }
        public string FormName { get; set; }
        public string FormStatus { get; set; }        
        public string FormApprovalStatusName { get; set; }        
        public string Remarks { get; set; }
        public string RaisedFor { get; set; }
        public string RaisedBy { get; set; }
        public string ParentFormName { get; set; }
        public string PlantName { get; set; }
        public string SkuName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string DealerName { get; set; }
        public string EALUserName { get; set; }
        public List<long> RoleIds { get; set; }

        public IList<SubmittedFormReportQuestionsViewDto> Questions { get; set; }

        public SubmittedFormShortViewDto()
        {
            Questions = new List<SubmittedFormReportQuestionsViewDto>();
        }

    }
}
