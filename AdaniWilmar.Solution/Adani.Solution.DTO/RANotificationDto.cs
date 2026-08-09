using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RANotificationDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public int LoginUserId { get; set; }
        public List<long> CustomerGroupIds { get; set; }
        public List<long> NotificationActionIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public string WindowVolumeCapacity { get; set; }
        public string NotificationActions { get; set; }
        public string CautionNotificationTimes { get; set; }
        public bool SMS { get; set; }
        public bool IsEmail { get; set; }
        public bool InAppNotification { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string SelecteDealerIdsString { get; set; }
        public List<long> SelectedDealerIds { get; set; }
        public List<long> SelectedCustomerIdsToRemove { get; set; }
        public string SelectedCustomerIdsToRemoveString { get; set; }
        public List<RaNotificationDetailDto> RaNotificationDetailDtoList { get; set; }
        public RANotificationDto()
        {
            RaNotificationDetailDtoList = new List<RaNotificationDetailDto>();
        }
    }
    public class RaNotificationDetailDto
    {
        public long RaNotificationId { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroup{ get; set; }
        public long NotificationActionId { get; set; }
        public string NotificationAction { get; set; }
        public string WindowVolumeCapacity { get; set; }
        public long CustomerGroupDetailId { get; set; }
        public long CustomerId { get; set; }
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

    public class RANotificationInputDto : KendoGridResult
    {
        public long CustomerGroupId { get; set; }
        public List<long> CustomerGroupIds { get; set; }
        public bool IsRemoveSelectedDealerIdsFromSession { get; set; }
    }

    public class RANotificationGridInputDto : KendoGridResult
    {
        public long RaNotificationId { get; set; }
        public long BDOId { get; set; }
        public bool IsRemoveSelectedDealerIdsFromSession { get; set; }
        public long ZoneId { get; set; }
        public List<long> ZoneIds { get; set; }
        public int DistrictId { get; set; }
        public List<int> DistrictIds { get; set; }
        public int CityId { get; set; }
        public List<int> CityIds { get; set; }
        public int StateId { get; set; }
        public List<int> StateIds { get; set; }
        public int TerritoryId { get; set; }
        public List<int> TerritoryIds { get; set; }
    }

    public class RaNotificationDetailsForExpiryNotificationDto
    {
        public long DayAfterTommorrowExpiringCount { get; set; }
        public long TommorrowExpiringCount { get; set; }
        public bool SMS { get; set; }
        public bool Email { get; set; }
        public bool InAppNotification { get; set; }
        public string CustomerGroup { get; set; }
        public string Customer { get; set; }
        public string NotificationAction { get; set; }
        public string WindowVolumeCapacity { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public DateTime ValidFrom { get; set; } 
        public DateTime ValidTo { get; set; } 
    }


}
