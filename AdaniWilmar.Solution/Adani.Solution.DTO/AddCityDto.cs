using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddCityDto
    {
        public string CityName { get; set; }
        public int DistrictId { get; set; }
        public int TerritoryId { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
    public class VolumeCapacityDto
    {
        public string WindowName { get; set; }
        public string OilName { get; set; }
        public decimal TotalVolumeCapacity { get; set; }        
        public decimal UsedPercentage { get; set; }
        public decimal RemainingVolumeCapacity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long BiddingWindowId { get; set; }
        public long OilTypeId { get; set; }
    }

    public class ExpiryDateNotificationDto
    {
        public string ScreenName { get; set; }
        public long TommorrowExpiringCount { get; set; }
        public long DayAfterTommorrowExpiringCount { get; set; }
    }
    public class GetCountBasedOnCurrentDateFromPricingsDto
    {
        public long CountOfRecords { get; set; }
    }

    public class ExcelExportInputDto : VerticalIdDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long VerticalId { get; set; }
        public long IsActiveStatus { get; set; }
    }

    public class NotificationsDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public int LoginUserId { get; set; }
        
        public List<long> NotificationActionIds { get; set; }
       
        public string NotificationActions { get; set; }
        public bool SMS { get; set; }
        public bool IsEmail { get; set; }
        public bool InAppNotification { get; set; }
       
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string SelecteDealerIdsString { get; set; }
        public List<long> SelectedDealerIds { get; set; }
        public string SelecteDealerIdsStringToRemove { get; set; }
        public List<long> SelectedDealerIdsToremove { get; set; }
        public List<NotificationDetailDto> NotificationDetailDtoList { get; set; }
        public NotificationsDto()
        {
            NotificationDetailDtoList = new List<NotificationDetailDto>();
        }
    }

    public class NotificationDetailDto
    {
        public long NotificationId { get; set; }
      
        public long NotificationActionId { get; set; }
        public string NotificationAction { get; set; }
       
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

    public class NotificationInputDto : KendoGridResult
    {
        public long CustomerId { get; set; }
        public List<long> BdoIds { get; set; }
        public bool IsRemoveSelectedDealerIdsFromSession { get; set; }
    }
    public class NotificationGridInputDto : KendoGridResult
    {
        public long NotificationId { get; set; }
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
}
