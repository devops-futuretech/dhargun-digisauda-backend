using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Ticker : Auditable
    {
        public string Content { get; set; }
        public DateTime TickerDate { get; set; }
        public TimeSpan FromHours { get; set; }
        public TimeSpan ToHours { get; set; }
        public string ColorCode { get; set; }
        public bool IsActive { get; set; }
    }
}
