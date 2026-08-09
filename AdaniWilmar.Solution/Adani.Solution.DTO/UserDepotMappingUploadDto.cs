using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserDepotMappingUploadDto : CommonResultDto
    {
        public string UserCode { get; set; }
        public string DepotCode { get; set; }
        public string IsDealer { get; set; }
        public string DivisionCode { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
    }
}
