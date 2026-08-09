using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SectionQuestionsViewDto
    {
        public long SectionId { get; set; }
        public string SectionName { get; set; }   
        public int QuestionsCount { get; set; }
        public string SelectedQuestionsString { get; set; }
        public string UnselectedQuestionsString { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string QuestionTypeName { get; set; }
        public long QuestionId { get; set; }
        public string Query { get; set; }
        public IList<QuestionsViewDto> Questions { get; set; }
        public SectionQuestionsViewDto()
        {
            Questions = new List<QuestionsViewDto>();
        }
    }
}
