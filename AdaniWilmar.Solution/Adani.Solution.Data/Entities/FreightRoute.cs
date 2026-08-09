using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class FreightRoute : Auditable
    {
        public long FreightZoneId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual FreightZone FreightZone { get; set; }
    }
}
