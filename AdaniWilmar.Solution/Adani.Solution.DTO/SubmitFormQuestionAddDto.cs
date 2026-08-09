using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SubmitFormQuestionAddDto
    {
        public long SubmittedFormQuestionId { get; set; }
        public long QuestionId { get; set; }
        public long QuestionTypeId { get; set; }
        public SubmitFormAnswerOptionAddDto Answers { get; set; }

        public SubmitFormQuestionAddDto()
        {
            Answers = new SubmitFormAnswerOptionAddDto();
        }
    }
}
