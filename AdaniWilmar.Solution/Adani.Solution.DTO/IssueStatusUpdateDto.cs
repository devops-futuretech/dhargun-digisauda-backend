using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IssueStatusUpdateDto : IAPIInputDTO
    {
        public long SupportId { get; set; }
        public int StatusId { get; set; }
        public string IssueComments { get; set; }
        public long ModifiedBy { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
