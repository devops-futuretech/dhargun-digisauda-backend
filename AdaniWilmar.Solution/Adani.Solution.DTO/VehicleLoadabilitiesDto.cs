using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VehicleLoadabilitiesDto : IAPIInputDTO
    {
        public long ZoneId { get; set; }
        public List<long> ZoneIds { get; set; }
        public long CreatedBy { get; set; }
        public long LoginUserId { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public string FreightZoneName { get; set; }
        public int StateId { get; set; }
        public List<int> StateIds { get; set; }
        public long FreightZoneId { get; set; }
        public List<long> FreightZoneIds { get; set; }
        public decimal VehicleSize { get; set; }
        public bool IsActiveBool { get; set; }
        public long IsActive { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string Message { get; set; }
    }
}
