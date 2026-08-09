using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FreightZoneInputDto
    {
        public int StateId { get; set; }
        public long ZoneId { get; set; }

        public List<int?> StateIds { get; set; }

        public List<long?> ZoneIds { get; set; }
    }

    public class FreightZoneAndRouteDropDownInputDto
    {
        public long FreightZoneId { get; set; }
        public long FreightRouteId { get; set; }
        public long LoginUserId { get; set; }
    }
    }
