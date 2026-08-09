using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class QuestionsViewDto
    {
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public long QuestionId { get; set; }
        public int OrderNo { get; set; }
        public string Query { get; set; }
        public string Textlength { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsMandatory { get; set; }
        public int OrderId { get; set; }
        public string SubmittedAnswer { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IList<AnswerOptionDto> AnswerOptions { get; set; }
        public IList<QuestionsViewDto> Questions { get; set; }
        public QuestionsViewDto()
        {
            Questions = new List<QuestionsViewDto>();
            AnswerOptions = new List<AnswerOptionDto>();
        }
    }

    public class QuestionrOrderDto
    {
        public long QuestionId { get; set; }
        public long SectionId { get; set; }
        public int OrderId { get; set; }
    }
}
