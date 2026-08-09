using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FreightZoneDto
    {
        public long Id { get; set; }
        //public long DepotId { get; set; }
        //public string DepotName { get; set; }
        //public string DepotCode { get; set; }

        public int? StateId { get; set; }
        public long? ZoneId { get; set; }
        public string StateName { get; set; }
        public string ZoneName { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }

        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
