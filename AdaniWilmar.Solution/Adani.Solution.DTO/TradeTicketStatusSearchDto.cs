using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketStatusSearchDto
    {
        public long LoginUserId { get; set; }
        public bool IsToReturnInactiveData { get; set; }
        public DateTime SearchDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long VerticalId { get; set; }
    }
}
