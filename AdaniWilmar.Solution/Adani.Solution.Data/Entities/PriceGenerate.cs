using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class PriceGenerate : Auditable
    {
        public long SaudaBookingTypeId { get; set; }
        public long VerticalId { get; set; }

        public int ExeStatusId { get; set; }

        public virtual SaudaBookingType SaudaBookingType { get; set; }
        public virtual Division Vertical { get; set; }

        public ICollection<PriceGenerateDetail> PriceGenerateDetail { get; set; }
    }
}
