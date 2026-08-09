using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class UserInputDto : LoginUserIdDto
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public List<long> SelectedDealerIds { get; set; }
        public int SelectedDealerIdsCount { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long DealerId { get; set; }
        public long StateTraderId { get; set; }
        public long SkuId { get; set; }
    }

    public class UserNotificationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PushTokenKey { get; set; }
        public int RegistrationTypeId { get; set; }
    }

    public class SaudaBoookingConfig
    {
        public bool IsActive { get; set; }
        public long Id { get; set; } = 0;
        public string Message { get; set; }
        public DateTime StartTime { get; set; }
    }
    
}
