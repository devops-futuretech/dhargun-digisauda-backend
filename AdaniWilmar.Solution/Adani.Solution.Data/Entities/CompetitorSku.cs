using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class CompetitorSku : Auditable
    {
        public long CompetitorId { get; set; }

        public long SkuId { get; set; }

        public virtual Competitor Competitor { get; set; }

        public virtual Sku Sku { get; set; }
    }
}
