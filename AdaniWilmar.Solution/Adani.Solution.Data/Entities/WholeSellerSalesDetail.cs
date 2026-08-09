using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class WholeSellerSalesDetail : Auditable
    {
        public long WholesellerBdoId { get; set; }
        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public decimal QuantityPerMt { get; set; }
        public decimal Price { get; set; }

        public virtual WholesellerBdo WholesellerBdo { get; set; }

        public virtual Sku Sku { get; set; }
    }
}
