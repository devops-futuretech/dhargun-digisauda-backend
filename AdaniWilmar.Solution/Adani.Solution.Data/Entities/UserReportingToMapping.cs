using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserReportingToMapping:Auditable
    {
        public long UserId { get; set; }
        public long ReportingToUserId { get; set; }
        public long RoleId { get; set; }
    }
}
