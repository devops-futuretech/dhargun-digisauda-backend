using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VehicleLoadabilitiesGridDataDto
    {
        public long ZoneId { get; set; }
        public long Id { get; set; }
        public int StateId { get; set; }
        public long FreightZoneId { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public string FreightZoneName { get; set; }
        public decimal VehicleSize { get; set; }
        public bool IsActive { get; set; }
    }

}
