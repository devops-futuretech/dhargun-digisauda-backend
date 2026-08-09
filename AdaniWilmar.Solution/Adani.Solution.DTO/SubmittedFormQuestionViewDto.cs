using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SubmittedFormQuestionViewDto
    {
        public long SubmittedFormQuestionId { get; set; }
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public long QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public string Answer { get; set; }
        public string Description { get; set; }
        public bool IsMandatory { get; set; }
        public SubmittedYesNoAnswerViewDto YesNo { get; set; }
        public SubmittedTextAnswerViewDto TextAnswer { get; set; }
        public List<SubmittedFormMultiAnswerViewDto> AnswerOptions { get; set; }
        public List<SubmittedAttachmentViewDto> Attachments { get; set; }

        public SubmittedFormQuestionViewDto()
        {
            AnswerOptions = new List<SubmittedFormMultiAnswerViewDto>();
            Attachments = new List<SubmittedAttachmentViewDto>();
        }
    }
}
