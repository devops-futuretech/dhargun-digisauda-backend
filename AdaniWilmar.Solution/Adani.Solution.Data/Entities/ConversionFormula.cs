using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class ConversionFormula : Auditable
    {
        public ConversionFormula()
        {
            this.ConversionFormulaDetails = new HashSet<ConversionFormulaDetails>();
        }

        public long OilTypeId { get; set; }

        public long PackGroupId { get; set; }

        public long SkuId { get; set; }

        public bool IsActive { get; set; }

        public virtual OilType OilType { get; set; }

        public virtual PackGroup PackGroup { get; set; }

        public virtual Sku Sku { get; set; }

        public virtual ICollection<ConversionFormulaDetails> ConversionFormulaDetails { get; set; }
    }
}
