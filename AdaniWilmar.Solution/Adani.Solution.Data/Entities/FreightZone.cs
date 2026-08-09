using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class FreightZone : Auditable
    {
        //public long DepotId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? StateId { get; set; }
        public long? ZoneId { get; set; }

        //public virtual Depot Depot { get; set; }
        public virtual State State { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
