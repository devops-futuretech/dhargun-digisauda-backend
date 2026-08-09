using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaLimitHistory : Auditable
    {
        public long UserId { get; set; }

        [DecimalPrecision(10, 4)]
        public decimal OldSaudaLimit { get; set; }

        [DecimalPrecision(10, 4)]
        public decimal NewSaudaLimit { get; set; }
        public string Remarks { get; set; }

        public virtual User User { get; set; }
    }
}
