using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SubmitFormAnswerOptionAddDto
    {
        public IList<long> SelectedAnswerOptionIds { get; set; }
        public bool? IsYes { get; set; }
        public string TextAnswer { get; set; }
        public List<SubmittedAttachmentViewDto> Attachments { get; set; }
        public SubmitFormAnswerOptionAddDto()
        {
            SelectedAnswerOptionIds = new List<long>();
            Attachments = new List<SubmittedAttachmentViewDto>();
        }
    }
}