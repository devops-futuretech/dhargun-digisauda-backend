using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ScheduleDemoInputDto : LoginUserIdDto
    {
        public ScheduleDemoInputDto()
        {
            EALUserId = new List<long>();
        }
        public DateTime DemoDateTime { get; set; }
        public long SalesExecutiveId { get; set; }
        public long DemoInchargeId { get; set; }
        public long DemonstratorId { get; set; }
        public long ComplaintFormId { get; set; }
        public long UnderstandingFormId { get; set; }
        public long DemoId { get; set; }
        public bool IsActive { get; set; }
        public long DemoUserRoleId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public List<long> EALUserId { get; set; }
        public bool IsEALUser { get; set; }
    }
}
