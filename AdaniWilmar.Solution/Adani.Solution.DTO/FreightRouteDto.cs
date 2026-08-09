using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
     public class FreightRouteDto
    {
        public long Id { get; set; }
        public long FreightZoneId { get; set; }
        public string FreightZoneName { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
