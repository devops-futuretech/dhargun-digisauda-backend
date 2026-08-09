using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FeedbackRequestInputDto:LoginUserIdDto
    {
        public long FeedbackTypeId { get; set; }
        public string Details { get; set; }
    }
}
