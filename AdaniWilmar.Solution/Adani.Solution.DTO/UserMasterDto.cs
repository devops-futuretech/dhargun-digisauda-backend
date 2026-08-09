using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserMasterDto
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Branch { get; set; }
        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        public long ReportingTo { get; set; }
        public long? OrganizationReportingToId { get; set; }
        public long? SalesReportingToId { get; set; }
        public long? SpecialityFatReportingToId { get; set; }
        public long? CMSReportingToId { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string AdditionalMobileNumber { get; set; }
        public string SalesAccess { get; set; }
        public string Designation { get; set; }
        public long? HeadquartersId { get; set; }
        public string Headquarters { get; set; }
        public long StateId { get; set; }
        public string State { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string Acedns { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Pincode { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string FrieghtZone { get; set; }
        public string FrieghtRoute { get; set; }
        public IList<DealerLocationDto> DealerLocation { get; set; }
        public bool IsBroker { get; set; }
        public long BdoCount { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public decimal Loadability { get; set; }
        public decimal DepotLoadability { get; set; }
        public List<decimal> PlantTruckCapacities { get; set; }
        public List<decimal> DepotTruckCapacities { get; set; }
        public string SaudaBookingType { get; set; }
        public string OrganizationReportingToName { get; set; }
        public string SalesReportingToName { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyCode { get; set; }

        public string RoleName { get; set; }
        public SaudaAndBiddingChancesDto SaudaAndBiddingChances { get; set; }

        public UserMasterDto()
        {
            DealerLocation = new List<DealerLocationDto>();
            SaudaAndBiddingChances = new SaudaAndBiddingChancesDto();
        }
    }

    public class DealerBrokerDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string RoleName { get; set; }
        public bool IsChecked { get; set; }
    }

    public class UserCustomerMappingDto
    {
        public long UserId { get; set; }
        public long CustomerId { get; set; }
        public long LoginUserId { get; set; }
    }

    public class UserDetailsViewModel : EmployeeDto
    {
        public string SalesOrg { get; set; }
        public string DistChannel { get; set; }
        public string VerticalIdString { get; set; }
        public string UserDetailsId { get; set; }
        public bool IsRemoveSelectedDealerIdsFromSession { get; set; }
    }

    public class DealerBrokerParamDto : LoginUserIdDto
    {
        public DealerBrokerParamDto()
        {
            DivisionList = new List<DivisionDetailsDto>();
        }
        public long VerticalId { get; set; }        
        public long SaudaBookingTypeId { get; set; }
        public long StateId { get; set; }
        public List<DivisionDetailsDto> DivisionList { get; set; }
    }

    public class SaudaAndBiddingChancesDto 
    {
        public decimal TotalSaudaLimit { get; set; }
        public decimal AvailableSaudaLimit { get; set; }
        public long TotalChances { get; set; }
        public long ChancesLeft{ get; set; }
    }

    public class UserMasterDetailDto
    {
        public long Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Loadability { get; set; }
        public decimal DepotLoadability { get; set; }
        public long SaudaBookingTypeId { get; set; }

    }

    public class UserProfileDto : IAPIInputDTO
    {
        public long LoginUserId { get; set; }
        public string ProfilePath { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
