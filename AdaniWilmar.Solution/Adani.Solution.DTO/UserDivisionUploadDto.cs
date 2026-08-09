using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserDivisionUploadDto: CommonResultDto
    {
        public string UserCode { get; set; }
        public string RoleName { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public decimal SaudaLimit { get; set; }
        public long CreatedBy { get; set; }
        public int ContractValidityPeriodDays { get; set; }
        public string PlantDepot { get; set; }

    }
}
