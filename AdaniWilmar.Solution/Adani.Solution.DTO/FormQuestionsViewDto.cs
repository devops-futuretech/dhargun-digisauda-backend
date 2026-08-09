using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class FormQuestionsViewDto
    {
        public long FormId { get; set; }
        public long? DependentFormId { get; set; }
        public string FormName { get; set; }
        public List<long>RoleIds { get; set; }       
        public bool IsFormStatus { get; set; }       
        public bool IsActive { get; set; }
        public string QuestionTypeName { get; set; }
        public string Query { get; set; }
        public IList<SectionQuestionsViewDto> SectionQuestions { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IList<long> FormUsers { get; set; }
        public IList<QuestionsViewDto> Questions { get; set; }
        public FormQuestionsViewDto()
        {
            SectionQuestions = new List<SectionQuestionsViewDto>();
            FormUsers = new List<long>();
            Questions = new List<QuestionsViewDto>();
            RoleIds = new List<long>();
        }
    }
}
