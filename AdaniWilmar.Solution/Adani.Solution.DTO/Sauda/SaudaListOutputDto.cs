using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emami.Solution.DTO.Sauda
{
    public class SaudaListOutputDto
    {
        public int SaudaNumber { get; set; }
        public DateTime BiddingDate { get; set; }
        public string DealerName { get; set; }
        public string TotalQty { get; set; }
        public string TotalAmt { get; set; }
    }
}
