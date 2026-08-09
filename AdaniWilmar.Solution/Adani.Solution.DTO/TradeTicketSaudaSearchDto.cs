using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketSaudaSearchDto
    {
        public long LoginUserId { get; set; }
        public bool IsToReturnInactiveData { get; set; }
        public long TradeTicketId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<long> DealerId { get; set; }
        public List<long> StateId { get; set; }
        public List<long> DealerIds { get; set; }
    }
}
