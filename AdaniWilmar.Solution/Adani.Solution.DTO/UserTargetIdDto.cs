using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserTargetIdDto : LoginUserIdDto
    {
        public long AssignedToUserId { get; set; }
        public long FinancialYearId { get; set; }
        public long OilTypeId { get; set; }
        public long VerticalId { get; set; }
        public long RoleId { get; set; }
        public long RoleTypeId { get; set; }
    }
}
