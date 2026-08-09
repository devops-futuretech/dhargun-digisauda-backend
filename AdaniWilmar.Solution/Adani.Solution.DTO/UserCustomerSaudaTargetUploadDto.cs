using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserCustomerSaudaTargetUploadDto : CommonResultDto
    {
        public string OilTypeName { get; set; }
        public string DivisionCode { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string AssignedFromUserCode { get; set; }
        public string AssignedToUserCode { get; set; }
        public int Quarter { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FinancialYear { get; set; }
        public string DealerName { get; set; }
        public decimal Target { get; set; }
        public long UserId { get; set; }
        public long CreatedBy { get; set; }
    }
}

