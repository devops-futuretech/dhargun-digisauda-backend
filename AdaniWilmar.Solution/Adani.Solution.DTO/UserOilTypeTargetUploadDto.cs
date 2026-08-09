using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserOilTypeTargetUploadDto : CommonResultDto
    {
        public string AssignedFrom { get; set; }
        public string AssignedTo { get; set; }
        public string Quarter { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FinancialYear { get; set; }
        public string OilTypeName { get; set; }
        public decimal Target { get; set; }
        public long UserId { get; set; }
        public long CreatedBy { get; set; }
    }
}
