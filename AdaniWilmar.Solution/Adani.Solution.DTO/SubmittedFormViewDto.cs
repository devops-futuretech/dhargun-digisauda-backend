using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SubmittedFormViewDto
    {
        public long CustomerId { get; set; }
        public string RaisedFor { get; set; }
        public bool IsLatLonUpdated { get; set; }
        public string RaisedBy { get; set; }
        public long SubmittedFormId { get; set; }
        public DateTime CreatedDate { get; set; }
        public long FormId { get; set; }
        public string FormName { get; set; }        
        public string FormStatusName { get; set; }
        public long FormApprovalStatusId { get; set; }
        public long FormStatusId { get; set; }
        public string DemonstratedBy { get; set; }
        public string DemoIncharge { get; set; }
        public string Remarks { get; set; }
        public IList<SectionDto> Sections { get; set; }
        public long? ParentFormId { get; set; }
        public string ParentFormName { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long PlantId { get; set; }
        public string PlantName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long EALUserId { get; set; }
        public string EALUserName { get; set; }

        public List<FormRemarksDto> Comments { get; set; }
        public List<DropDownDto> DependentFormDetails { get; set; }
        public List<ScheduleDemoOutputDto> DemoDetails { get; set; }
        public List<SubmittedDependentFormDto> DependentForms { get; set; }
        public List<FormQuestionsViewDto> DependentFormsMaster { get; set; }
        public List<FormTab> SubmittedFormTabs { get; set; }
        public SubmittedFormViewDto()
        {
            Sections = new List<SectionDto>();
            Comments = new List<FormRemarksDto>();
            DependentFormDetails = new List<DropDownDto>();
            DependentForms = new List<SubmittedDependentFormDto>();
            DemoDetails = new List<ScheduleDemoOutputDto>();
            DependentFormsMaster = new List<FormQuestionsViewDto>();
            SubmittedFormTabs = new List<FormTab>();
        }
    }

    public class FormTab
    {
        public long FormId { get; set; }
        public string Header { get; set; }
        public bool IsSelected { get; set; }
        public string LoadUrl { get; set; }
    }
}
