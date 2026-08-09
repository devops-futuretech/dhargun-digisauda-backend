using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class EmployeeDto : IAPIInputDTO
    {
        public EmployeeDto()
        {
            this.PickupLocation = new List<PickUpLoationsDto>();
            this.FormUsers = new List<FormDto>();
            Attachments = new List<SupportAttachmentDto>();
            ShipToPartyList = new List<ShipToPartyMappingDto>();
            DivisionList = new List<DivisionDetailsDto>();
        }
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string AdditionalMobileNumber { get; set; }
        public string ContactPersonName { get; set; }
        public string Password { get; set; }
        public string CompanyCode { get; set; }
        public string OtpNumber { get; set; }
        public long RoleId { get; set; }
        //public string UserCode { get; set; }
        public string PushTokenKey { get; set; }
        public long? ReportingToId { get; set; }
        public long? SpecialityFatReportingToId { get; set; }
        //public long? CMSReportingToId { get; set; }
        //public long? FreightZoneId { get; set; }
        //public long? FreightRouteId { get; set; }
        //public string FreightZone { get; set; }
        //public string FreightRoute { get; set; }

        public string Remarks { get; set; }
        public DateTime? LastLoggedInDate { get; set; }
        public DateTime? PreviousLoggedInDate { get; set; }
        public bool IsApproved { get; set; } = false;
        public long? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveForCall { get; set; }
        public bool IsBlacklisted { get; set; }
        public string ImageUrl { get; set; }
        public long? ParentUserId { get; set; }
        public int? RegistrationTypeId { get; set; }

        public string Region { get; set; }
        public string Pincode { get; set; }
        public string Street { get; set; }

        public long? ZoneId { get; set; }
        public string Zone { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int TerritoryId { get; set; }
        public string Territory { get; set; }

        public string ExecutivePassword { get; set; }

        //public long CustomerGroupOneId { get; set; }
        //public string CustomerGroupOneName { get; set; }

        public long CustomerGroupFiveId { get; set; }
        public string CustomerGroupFiveName { get; set; }
       

        //public long CustomerGroupTwoId { get; set; }
        //public string CustomerGroupTwoName { get; set; }

        public string McsNo { get; set; }
        public string Code { get; set; }
        // public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        //public string AddressLine1 { get; set; }
        //public string AddressLine2 { get; set; }
        //public string AddressLine3 { get; set; }
        public string GSTN { get; set; }
        public string TANNumber { get; set; }
        public string VisitDay { get; set; }
        public int SaudaValidityPeriod { get; set; }
        public decimal SaudaLimit { get; set; }
        public string WeeklyClosingDay { get; set; }
        public string MonthlyPotential { get; set; }
        //Loadability
        public decimal PlantTruckCapacity { get; set; }
        //public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CustClass { get; set; }
        //Depot Loadability
        //public decimal DepotTruckCapacity { get; set; }
        public string PlantTruckCapacities { get; set; }
        public string DepotTruckCapacities { get; set; }

        public List<decimal> PlantTruckCapacityList { get; set; }
        public List<decimal> DepotTruckCapacityList { get; set; }
        //Employee
        public string Branch { get; set; }
        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        public long ReportingTo { get; set; }
        public string SalesAccess { get; set; }
        public string Designation { get; set; }
        public long? HeadquartersId { get; set; }
        public string Headquarters { get; set; }
        public string Acedns { get; set; }
        public long? SaudaBookingTypeId { get; set; }
        //public long? IncoTermsId { get; set; }
        public List<long> IncoTermsId { get; set; }
        public string IncoTerms { get; set; }
        public long? TransportModeId { get; set; }

        public int AreaId { get; set; }
        public string Area { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        //SAP data
        public string ADRNR { get; set; }
        public string TaxNumber { get; set; }
        public string DeliveringPlant { get; set; }
        public string CentralDeletionFlag { get; set; }
        public string ErrorMessage { get; set; }

        public int SelectedDealerOrBrokerId { get; set; }
        public decimal AvailableSaudaLimit { get; set; }

        //Dealer Popup
        public List<long> SelectedDealerBrokerIds { get; set; }
        public int SelectedDealerBrokerIdsCount { get; set; }
        public string SelecteDealerBrokerIdsString { get; set; }
        public string RemovedDealerBrokerIdsString { get; set; }
        public List<long> RemovedDealerBrokerIds { get; set; }

        public string SelectedDepotIdsString { get; set; }
        public List<long> SelectedDepotIds { get; set; }
        public string DepotNames { get; set; }
        public List<long> SelectedPlantIds { get; set; }
        public List<long> SelectedReportingToIds { get; set; }
        //public List<string> SelectedReportingToEncryptedIds { get; set; } // NEW PROPERTY

        public string PlantNames { get; set; }

        public string SelecteDealerIdsString { get; set; }
        public List<long> SelectedDealerIds { get; set; } // ShipToPartyIds
        public List<ShipToPartyMappingDto> ShipToPartyList { get; set; }

        public int SelectedDealerIdsCount { get; set; }
        public string RemovedDealerIdsString { get; set; }
        public List<long> RemovedDealerIds { get; set; }

        public List<long> SelectedBrokerIds { get; set; }

        public bool IsSelf { get; set; }
        public bool IsBroker { get; set; }

        //public long PlantId { get; set; }
        public string PlantName { get; set; }

        //public long DepotId { get; set; }
        public string DepotName { get; set; }

        public long BrokerId { get; set; }
        public string FSSAINumber { get; set; }
        public bool IsFromMobile { get; set; }

        public List<PickUpLoationsDto> PickupLocation { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string InActiveRemarks { get; set; }
        public long? InActiveRemarkId { get; set; }
        public List<FormDto> FormUsers { get; set; }
        public string Role { get; set; }
        public bool IsCustomer { get; set; }
        public List<long> BrokerIds { get; set; }
        public string BrokerNames { get; set; }
        public List<SupportAttachmentDto> Attachments { get; set; }
        public List<DivisionDetailsDto> DivisionList { get; set; }
        public List<long> LineId { get; set; }
        public string LineNames { get; set; }
    }

    public class PickUpLoationsDto
    {

        public long PickupLocationId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string Address { get; set; }
    }

    public class ShipToPartyMappingDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string District { get; set; }
    }
}
