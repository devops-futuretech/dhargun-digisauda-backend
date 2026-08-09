using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SAPDataSyncInputDto : IAPIInputDTO
    {
        public string DataSyncInputId { get; set; }
        public long LoginUserId { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long TradeTicketWithOrWithoutId { get; set; }
        public long VerticalId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
