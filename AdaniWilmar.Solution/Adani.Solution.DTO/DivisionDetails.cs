using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DivisionDetailsDto
    {
        public long Id { get; set; }
        public long DivisionId { get; set; }
        public string Division { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public decimal SaudaLimit { get; set; }
        public int SaudaValidityPeriod { get; set; }
        public List<long> UserDivisionPlantIds { get; set; }
        public string UserDivisionPlantCodes { get; set; }

    }
}
