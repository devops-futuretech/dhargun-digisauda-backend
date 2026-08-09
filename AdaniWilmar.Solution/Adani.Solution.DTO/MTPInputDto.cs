using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class MTPInputDto
    {
        public long LoginUserId { get; set; }
        public long PJPId { get; set; }
        public List<long> DealerIds { get; set; }
        public int InHQNoVisit { get; set; }
        public DateTime Date { get; set; }
        public int TownId { get; set; }
        public string Area { get; set; }
        public int MonthId { get; set; }
        public string Remarks { get; set; }
    }
}
