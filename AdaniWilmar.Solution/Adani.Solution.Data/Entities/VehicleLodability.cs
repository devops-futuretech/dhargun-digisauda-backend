using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class VehicleLodability : Auditable
    {
        public long ZoneId { get; set; }
        public int StateId { get; set; }
        //public long FreightZoneId { get; set; }
        public decimal VehicleSize { get; set; }
        public bool IsActive { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual State State { get; set; }
        //public virtual FreightZone FreightZone { get; set; }
    }
}
