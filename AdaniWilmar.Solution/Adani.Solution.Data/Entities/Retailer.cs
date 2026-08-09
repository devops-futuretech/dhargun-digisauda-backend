using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Retailer : Auditable
    {
        [Required, MaxLength(150)]
        public string AccountName { get; set; }

        [MaxLength(150)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string MobileNumber { get; set; }

        public long? SPFZoneId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public int? TerritoryId { get; set; }

        [MaxLength(10)]
        public string Pincode { get; set; }

        [MaxLength(4000)]
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public long? FreightZoneId { get; set; }
        public long? FreightRouteId { get; set; }

        public string VisitDay { get; set; }
        public string DistributorSalesMan { get; set; }
        public string DistributorSalesManCode { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
        public string ASOASEname { get; set; }
        public string ASOASECode { get; set; }
        public string AccountManager { get; set; }
        public string AccountType { get; set; }
        public string AreaName { get; set; }
        public string OwnersName { get; set; }
        public string DecisionMakerName { get; set; }
        public string DecisionMakerNumber { get; set; }
        public string ChefName { get; set; }
        public string ChefNumber { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public long? VerticalId { get; set; }
        public long? DealerId { get; set; }


        public virtual Zone SPFZone { get; set; }
        public virtual State State { get; set; }
        public virtual District District { get; set; }
        public virtual City City { get; set; }
        public virtual Territory Territory { get; set; }

        public virtual FreightZone FreightZone { get; set; }
        public virtual FreightRoute FreightRoute { get; set; }
        public virtual Vertical Vertical { get; set; }
    }
}
