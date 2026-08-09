using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaModificationOldItem : Auditable
    {
        public long SaudaModificationLineId { get; set; }
        public long skuId { get; set; }
        public virtual SaudaModificationLine SaudaModificationLine { get; set; }
        public virtual Sku Sku { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal QuantityInCase { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal SaudaQuantity { get; set; }
    }
}
