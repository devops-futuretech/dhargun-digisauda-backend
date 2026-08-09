using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ReportingUsersInputDto : LoginUserIdDto
    {
        public long RoleId { get; set; }
        public int ProcessId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long UserId { get; set; }
        public long CityId { get; set; }
        public long StateId { get; set; }
        public bool IsAdmin { get; set; }
        public List<long> DivisionIds { get; set; }
        public List<long> SalesOrganizationIds { get; set; }
        public List<long> DistributionChannelIds { get; set; }

    }
}
