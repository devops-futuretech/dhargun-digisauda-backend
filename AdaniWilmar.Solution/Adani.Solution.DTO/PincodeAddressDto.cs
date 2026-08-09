using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PincodeAddressDto : EntityDto
    {
        public string Pincode { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public List<AreaDto> Areas { get; set; }

        public PincodeAddressDto()
        {
            Areas = new List<AreaDto>();
        }
    }

    public class AreaDto
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
    }
}
