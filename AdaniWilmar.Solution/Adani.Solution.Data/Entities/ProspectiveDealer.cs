using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class ProspectiveDealer : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }

         [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string MobileNumber { get; set; }

        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }

        [MaxLength(10)]
        public string Pincode { get; set; }

        [MaxLength(4000)]
        public string Address { get; set; }

        public bool IsActive { get; set; }
        public decimal ProspectiveSales { get; set; }
        public decimal ProspectiveInterestLevel { get; set; }

        public decimal BusinessPotentialPeryear { get; set; }
        public long DealerId { get; set; }
    }
}
