using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class OutstandingSaudaDto
    {
        public long SaudaId { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal BiddingPrice { get; set; }
    }
}
