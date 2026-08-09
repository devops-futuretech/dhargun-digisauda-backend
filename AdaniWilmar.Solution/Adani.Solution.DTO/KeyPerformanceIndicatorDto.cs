using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class KeyPerformanceIndicatorDto
    {
        public long RoleId { get; set; }
        public string Content { get; set; }
    }

    public class KeyPerformanceDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
