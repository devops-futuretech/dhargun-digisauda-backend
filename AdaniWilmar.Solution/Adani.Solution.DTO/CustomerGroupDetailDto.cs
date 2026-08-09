using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CustomerGroupDetailDto
    {
        public long CustomerGroupDetailId { get; set; }
        public long CustomerGroupId { get; set; }
        public long CustomerId { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerName { get; set; }
        public string RoleName { get; set; }
        public string Code { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsChecked { get; set; }

        public long VerticalId { get; set; }
        public string Vertical { get; set; }

        public long? SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }

        public long ZoneId { get; set; }
        public string Zone { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int TerritoryId { get; set; }
        public string Territory { get; set; }
    }
}
