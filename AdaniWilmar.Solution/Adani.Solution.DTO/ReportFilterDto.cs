using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ReportFilterDto : LoginUserIdDto
    {
        public DateTime FilterDate { get; set; }
        public List<long> StateIds { get; set; }
        public long DivisionId { get; set; }
        //public long salesOrganizationId { get; set; }
        //public long distributionChannelId { get; set; }
        public string dealerCode { get; set; }
        public List<long> zhId { get; set; }
        public List<long> bdoId { get; set; }
    }
}
