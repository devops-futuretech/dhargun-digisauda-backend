using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaModificationLine : Auditable
    {
        public long SaudaModificationId { get; set; }
        public long OilTypeId { get; set; }
        public long OilPackGroupTypeId { get; set; }
        public virtual SaudaModification SaudaModification { get; set; }
        public virtual OilType OilType { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal TotalOriginalPendingQty { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal TotalModifiedQty { get; set; }
    }
}
