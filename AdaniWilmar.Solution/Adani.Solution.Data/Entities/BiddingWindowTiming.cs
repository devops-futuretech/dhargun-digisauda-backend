using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BiddingWindowTiming : Auditable
    {
        [Column(TypeName = "datetime2")]
        public DateTime BiddingDate { get; set; }
        public TimeSpan FromHours { get; set; }
        public TimeSpan ToHours { get; set; }
        public bool Isactive { get; set; }
        public bool IsLastWindowPerDay { get; set; }
    }
}
