using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserSalesSaudaTargetUploadDto : CommonResultDto
    {
        public string AssignedFrom { get; set; }
        public string AssignedTo { get; set; }
        public string Quarter { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public decimal SaudaTarget { get; set; }
        public decimal SalesTarget { get; set; }
        public long UserId { get; set; }
        public long CreatedBy { get; set; }
    }
}
