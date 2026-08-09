using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public  class BiddingWindowListDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan FromHours { get; set; }
        public TimeSpan ToHours { get; set; }
    }
}
