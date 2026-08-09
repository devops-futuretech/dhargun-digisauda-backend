using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class QuestionRemoveDto
    {
        public IList<long> QuestionIds { get; set; }
        public QuestionRemoveDto()
        {
            QuestionIds = new List<long>();
        }
    }
}
