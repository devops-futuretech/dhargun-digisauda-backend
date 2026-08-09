using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SubmittedFormsListViewDto
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public IList<SubmittedFormShortViewDto> SubmittedFormsShortView { get; set; }
        public IList<SubmittedFormReportViewDto> SubmittedFormsReportsView { get; set; }

        public SubmittedFormsListViewDto()
        {
            SubmittedFormsShortView = new List<SubmittedFormShortViewDto>();
            SubmittedFormsReportsView = new List<SubmittedFormReportViewDto>();
        }
    }
}
