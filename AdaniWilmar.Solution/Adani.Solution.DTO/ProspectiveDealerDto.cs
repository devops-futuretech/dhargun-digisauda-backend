using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ProspectiveDealerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int? StateId { get; set; }
        public string State { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public decimal ProspectiveSales { get; set; }
        public decimal ProspectiveInterestLevel { get; set; }
        public decimal BusinessPotentialPeryear { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Attachments { get; set; }
    }
}
