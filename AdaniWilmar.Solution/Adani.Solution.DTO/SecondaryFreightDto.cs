using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SecondaryFreightDto
    {
        public long Id { get; set; }
        public long? PlantId { get; set; }
        public string PlantName { get; set; }

        public long DepotId { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }

        public long? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }

        public long? FreightZoneId { get; set; }
        public string FreightZoneName { get; set; }

        public long? FreightRouteId { get; set; }
        public string FreightRouteName { get; set; }

        public decimal Capacity { get; set; }

        //public int CityId { get; set; }
        //public string CityName { get; set; }
        //public int DistrictId { get; set; }
        //public string DistrictName { get; set; }

        public long TransportModeId { get; set; }
        public string TransportMode { get; set; }

        public long VerticalId { get; set; }
        public string Vertical { get; set; }

        public decimal ActualFreight { get; set; }
        public decimal SalesFreight { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
        public long RoleId { get; set; }
    }
}
