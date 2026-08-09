using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IssueCommentsDto : LoginUserIdDto
    {
        public long CommentId { get; set; }
        public long SupportId { get; set; }
        public string Comments { get; set; }
        public DateTime CommentedDate{ get; set; }
        public long UserId { get; set; }
        public string CommentedBy { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

    }
}
