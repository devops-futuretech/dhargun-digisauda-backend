using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class QuestionAddDto : LoginUserIdDto
    {
        public long QuestionId { get; set; }
        public long SectionId { get; set; }
        public int QuestionTypeId { get; set; }
        public int QuestionTypeEdit { get; set; }
        public string Query { get; set; }
        public string Description { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsActive { get; set; }
        public string Option { get; set; }
        public string Options { get; set; }
        public string Textlength { get; set; }
        public string RemovedOptionIds { get; set; }
        public string AnswerOptionsDto { get; set; }
        public bool CreateAnother { get; set; }
        public IList<AnswerOptionDto> AnswerOptions { get; set; }
        public IList<long> RemovedAnswerIds { get; set; }

        public QuestionAddDto()
        {
            AnswerOptions = new List<AnswerOptionDto>();
            RemovedAnswerIds = new List<long>();
        }
    }
}
