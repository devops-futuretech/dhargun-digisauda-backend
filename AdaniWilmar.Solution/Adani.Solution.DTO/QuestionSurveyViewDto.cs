using System;

namespace Adani.Solution.DTO
{
    public class QuestionSurveyViewDto
    {
        public long QuestionSurveyId { get; set; }
        public string Comments { get; set; }
        public long CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedDate { get; set; }
    }
}
