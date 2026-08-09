using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaQuantityConfiguration : Auditable
    {
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public decimal MaximumPercentageQtyIncrease { get; set; }
        public bool IsActive { get; set; }

        public virtual OilType OilType { get; set; }
        public virtual PackGroup PackGroup { get; set; }
    }
}
