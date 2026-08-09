using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BiddingWindowVolumeCapacity : Auditable
    {
        public long BiddingWindowId { get; set; }

        public long OilTypeId { get; set; }

        public decimal VolumeCapacity { get; set; }

        public int Status { get; set; }

        public virtual OilType OilType { get; set; }

        public virtual BiddingWindow BiddingWindow { get; set; }
    }
}
