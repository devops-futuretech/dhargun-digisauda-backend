using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class QuestionMasterDto :LoginUserIdDto
    {
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public long QuestionId { get; set; }
        public bool HasChildren { get; set; }
        public string Query { get; set; }
        public string Textlength { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsMandatory { get; set; }
        public IList<AnswerOptionDto> AnswerOptions { get; set; }
        public QuestionMasterDto()
        {
            AnswerOptions = new List<AnswerOptionDto>();
        }
    }
}
