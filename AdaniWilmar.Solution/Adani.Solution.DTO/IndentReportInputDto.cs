using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IndentReportInputDto : RoleIdDto
    {
        public int StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<long> StateIds { get; set; }
        public bool IsAfterDeliverOrderNumber { get; set; }
        public long verticalIds { get; set; }
    }

    public class MonthlyReportInputDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ReportId { get; set; }
        public long VerticalId { get; set; }
        public long LoginUserId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public int RoleId { get; set; }

    }

}
