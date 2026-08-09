using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class ConversionFormulaDetails : Auditable
    {
        public long ConversionFormulaId { get; set; }

        public long SkuId { get; set; }

        public string Formula { get; set; }

        public virtual Sku Sku { get; set; }

        public virtual ConversionFormula ConversionFormula { get; set; }
    }
}
