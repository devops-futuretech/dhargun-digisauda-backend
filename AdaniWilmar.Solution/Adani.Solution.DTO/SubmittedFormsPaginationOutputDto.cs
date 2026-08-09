using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SubmittedFormsPaginationOutputDto
    {
        public long TotalRecords { get; set; }
        public List<SubmittedFormViewDto> SubmittedFormViewList { get; set; }
        public List<ScheduleDemoOutputDto> ScheduledDemoList { get; set; }

        public SubmittedFormsPaginationOutputDto()
        {
            SubmittedFormViewList = new List<SubmittedFormViewDto>();
            ScheduledDemoList = new List<ScheduleDemoOutputDto>();
        }
    }
}
