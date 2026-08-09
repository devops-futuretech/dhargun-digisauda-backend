using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ShipToPartyUploadDto : CommonResultDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string CompanyCode { get; set; }
        public string Email { get; set; }
        public int SaudaValidityPeriod { get; set; }
        public decimal SaudaLimit { get; set; }
        public string GSTN { get; set; }
        public string PlantTruckCapacity { get; set; }
        public string DepotTruckCapacity { get; set; }
        public string IncoTerms { get; set; }
        public string TransportMode { get; set; }
        public string SaudaBookingType { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public string TerritoryName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string Pincode { get; set; }
        //public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2{ get; set; }
        //public string FreightZoneName { get; set; }
        //public string FreightRouteName { get; set; }
        //public string IsSelf { get; set; }
        //public string IsBroker { get; set; }
        public string BrokerCode { get; set; }
        public string IsActive { get; set; }
        public long RoleId { get; set; }
        public long CreatedBy { get; set; }
        public string PlantCode { get; set; }
        public string DepotCode { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
        public string EncryptedPassword { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        //public string CustomerGroupOneName { get; set; }
        //public string CustomerGroupTwoName { get; set; }
        public string CustomerGroupFiveName { get; set; }

    }
}
