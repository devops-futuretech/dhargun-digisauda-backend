using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddDistrictDto
    {
        public string DistrictName { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
    }


    public class SmsInputDto : IAPIInputDTO
    {
        public string SmsContent { get; set; }
        public long RoleId { get; set; }
        public string MobileNumber { get; set; }
        public string Role { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public int NotificationType { get; set; }
        public string Email { get; set; }
        public string PushTokenKey { get; set; }
        public int? RegistrationTypeId { get; set; }
        public string Subject { get; set; }
        public long VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public string TestEmail { get; set; }
        public string TestMobileNumber { get; set; }
        public int LiveOrTesting { get; set; }
    }

    public class SaudaBookingConfigurationDto : IAPIInputDTO
    {
        public List<long> RoleIds { get; set; }
        public bool IsActive { get; set; }
        public string EncryptedId { get; set; }
        public long Id { get; set; }
        public bool DealerIsActive { get; set; }
        public bool StateIsActive { get; set; }
        public bool ZonalIsActive { get; set; }
        public DateTime StartDateForDistributor { get; set; }
        public DateTime StartDateForST { get; set; }
        public DateTime StartDateForZT { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long LoginUserId { get; set; }
        public long RoleId { get; set; }
        public long RoleIdForST { get; set; }
        public long RoleIdForZT { get; set; }
        public List<long> OilTypeIdsForDistributor { get; set; }
        public List<long> OilTypeIdsForStateTrader { get; set; }
        public List<long> OilTypeIdsForZonalTrader { get; set; }

        public List<long> UserIdsForDistributor { get; set; }
        public List<long> UserIdsForStateTrader { get; set; }
        public List<long> UserIdsForZonalTrader { get; set; }

        public List<long> OilTypeIds { get; set; }
        public DateTime StartDate { get; set; }
    }
}
