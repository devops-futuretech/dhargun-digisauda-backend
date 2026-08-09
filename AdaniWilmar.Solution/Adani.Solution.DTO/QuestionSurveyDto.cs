using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class QuestionSurveyDto
    {
        public IList<QuestionSurveyViewDto> QuestionSurveys { get; set; }
        public QuestionSurveyDto()
        {
            QuestionSurveys = new List<QuestionSurveyViewDto>();
        }
    }
}
