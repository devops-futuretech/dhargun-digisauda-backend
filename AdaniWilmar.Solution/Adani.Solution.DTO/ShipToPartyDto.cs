using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ShipToPartyDto : UserIdDto, IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int? SaudaValidityPeriod { get; set; }
        public decimal SaudaLimit { get; set; }

        public long? ZoneId { get; set; }
        public string Zone { get; set; }

        public string District { get; set; }
        public string State { get; set; }
        public string Territory { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        
        public string GSTN { get; set; }
        public string FreightZoneName { get; set; }
        public string FreightRouteName { get; set; }
        public long? SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public string CustClass { get; set; }
        public string VisitDay { get; set; }
        public bool IsSelf { get; set; }
        public bool IsBroker { get; set; }
        public string WeeklyClosingDay { get; set; }
        public string MonthlyPotential { get; set; }
        public string Incoterms { get; set; }
        public decimal PlantTruckCapacity { get; set; }
        //Depot Loadability
        public decimal DepotTruckCapacity { get; set; }
        public string PlantTruckCapacities { get; set; }
        public string DepotTruckCapacities { get; set; }
        public string TransportMode { get; set; }
        public string SaudaType { get; set; }
        public string Pincode { get; set; }
        public long? SaudaTypeId { get; set; }
        public long? IncoTermsId { get; set; }
        public long? TransportModeId { get; set; }
        public string PlantName { get; set; }
        public string DepotName { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string BrokerCode { get; set; }
        public string VerticalCode { get; set; }
        public string VerticalName { get; set; }
        public string Depots { get; set; }
        public string Plants { get; set; }
        public string FSSAINumber { get; set; }
        public string StateTrader { get; set; }
        public string BDOCode { get; set; }
        public string Password { get; set; }

        public string NewlyAdded { get; set; }

        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }

        public bool IsChecked { get; set; }
        public string ShipToParty { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CompanyCode { get; set; }
        public long CustomerGroupFiveId { get; set; }
    }

    public class ShipToPartyExportDto
    {
        [DisplayName("ShipToPartyName")]
        public String Name { get; set; }
        [DisplayName("ShipToPartyCode")]
        public string Code { get; set; }
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        [DisplayName("Division")]
        public string VerticalName { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string GSTN { get; set; }
        public string Incoterms { get; set; }
        [DisplayName("Broker Code")]
        public string BrokerCode { get; set; }
        public string Plants { get; set; }
        public string IsActive { get; set; }

    }
}
