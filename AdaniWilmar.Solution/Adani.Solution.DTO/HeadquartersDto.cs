using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HeadquartersDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public long ZoneId { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }

        public string Zone { get; set; }
        public string State { get; set; }
        public string Territory { get; set; }
        public string District { get; set; }
        public string City { get; set; }
    }
}
