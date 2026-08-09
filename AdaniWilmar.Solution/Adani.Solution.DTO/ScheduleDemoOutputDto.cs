using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class ScheduleDemoOutputDto : LoginUserIdDto
    {
        public long DemoId { get; set; }
        public long ComplaintFormId { get; set; }
        public string ComplaintFormName { get; set; }
        public long? UnderstandingFormId { get; set; }
        public string UnderstandingFormName { get; set; }        
        public long DemonstratorId { get; set; }
        public string DemonstratorName { get; set; }
        public long DemoInchargeId { get; set; }
        public string DemoInchargeName { get; set; }
        public DateTime DemoDateTime { get; set; }       
        public long SalesExecutiveId { get; set; }
        public string SalesExecutiveName { get; set; }
        public string FormStatus { get; set; }
        public string DemoCreatedBy { get; set; }
        public bool IsActive { get; set; }
        public List<long> EALUserId { get; set; }
        public string EALUserName { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }  
        public List<long> SubmittedUnderstandingForms { get; set; }
        public string ComplaintRemarks { get; set; }
        public ScheduleDemoOutputDto()
        {
            SubmittedUnderstandingForms = new List<long>();
            EALUserId = new List<long>();
        }
    }
}
