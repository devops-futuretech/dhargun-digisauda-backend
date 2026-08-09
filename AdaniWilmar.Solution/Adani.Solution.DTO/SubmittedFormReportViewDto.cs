using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SubmittedFormReportViewDto
    {
        public string RaisedFor { get; set; }
        public string RaisedBy { get; set; }
        public string DealerName { get; set; }
        public long SubmittedFormId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long FormId { get; set; }
        public string FormName { get; set; }
        public string FormApprovalStatusName { get; set; }
        public string FormStatusName { get; set; }
        public string DemonstratedBy { get; set; }
        public string Remarks { get; set; }
        public IList<SubmittedFormReportQuestionsViewDto> Questions { get; set; }
        public List<SubmittedFormShortViewDto> DependentFormsList { get; set; }
        public long? ParentFormId { get; set; }
        public string ParentFormName { get; set; }
        public string PlantName { get; set; }
        public string SkuName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string Address { get; set; }
        public string BakeryOwnerName { get; set; }
        public string BakeryOwnerNumber { get; set; }
        public string BakeryMaster { get; set; }
        public string BakeryMasterNumber { get; set; }

        public SubmittedFormReportViewDto()
        {
            Questions = new List<SubmittedFormReportQuestionsViewDto>();
            DependentFormsList = new List<SubmittedFormShortViewDto>();
        }
    }
}