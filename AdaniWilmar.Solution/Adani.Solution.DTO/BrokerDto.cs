using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BrokerDto
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string BrokerCode { get; set; }
        public string BrokerName { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumber2 { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public long? ZoneId { get; set; }
        public string Zone { get; set; }

        public string District { get; set; }
        public string State { get; set; }
        public string Territory { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string FreightZoneName { get; set; }
        public string FreightRouteName { get; set; }
        public long? SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public bool IsSelf { get; set; }
        public bool IsBroker { get; set; }
        public decimal SaudaLimit { get; set; }
        public string GSTN { get; set; }
        public string VisitDay { get; set; }
        public int SaudaValidityPeriod { get; set; }
        public string WeeklyClosingDay { get; set; }
        public string MonthlyPotential { get; set; }
        public string Incoterms { get; set; }
        public decimal Loadability { get; set; }
        public long? TransportModeId { get; set; }
        public string TransportMode { get; set; }
        public string SaudaType { get; set; }
        public string Pincode { get; set; }
        //public string PlantName { get; set; }
        //public string DepotName { get; set; }
        public string VerticalCode { get; set; }
        public string VerticalName { get; set; }
        public decimal DepotTruckCapacity { get; set; }
        public decimal PlantTruckCapacity { get; set; }

        public string PlantTruckCapacities { get; set; }
        public string DepotTruckCapacities { get; set; }
        public string Depots { get; set; }
        public string Plants { get; set; }
        public string FSSAINumber { get; set; }
        public string StateTrader { get; set; }
        public string BDOCode { get; set; }
        public string DealerCodeList { get; set; }
        public string Password { get; set; }

        public string NewlyAdded { get; set; }
        public string AdditionalMobileNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string CompanyCode { get; set; }
        public bool IsActiveForCall { get; set; }

    }
}
