using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AnswerOptionDto
    {
        public long QuestionId { get; set; }
        public long AnswerOptionId { get; set; }
        public string Option { get; set; }
    }
}
