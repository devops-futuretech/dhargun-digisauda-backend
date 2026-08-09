using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class QuestionDto
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public IList<QuestionSurveyViewDto> Comments { get; set; }

        public QuestionDto()
        {
            Comments = new List<QuestionSurveyViewDto>();
        }
    }
}
