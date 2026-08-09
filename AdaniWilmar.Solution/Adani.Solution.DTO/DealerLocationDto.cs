using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealerLocationDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int? StateId { get; set; }
        public long DistrictId { get; set; }
        public long CityId { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
