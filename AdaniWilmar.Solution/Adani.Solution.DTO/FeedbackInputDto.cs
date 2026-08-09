using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FeedbackInputDto
    {
        public int FeedbackTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Feedback { get; set; }
        public long LoginUserId { get; set; }
    }
}
