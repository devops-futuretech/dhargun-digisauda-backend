using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AutoAllocationInputDto
    {
        public string RoleIds { get; set; }
        public long VerticalId { get; set; }
        public long AverageDays { get; set; }
        public long UserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
