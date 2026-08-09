using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class UserViewDto
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public string UserCode { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        public List<long> SelectedDealerIds { get; set; }
        public int SelectedDealerIdsCount { get; set; }
        public string BusinessTypes { get; set; }
        public string CustomerCodes { get; set; }
        public long DealerId { get; set; }
        public bool HasChildren { get; set; }
        public string ADRNR { get; set; }
        public string TaxNumber { get; set; }
        public string DeliveringPlant { get; set; }
        public string Address { get; set; }
        public string CentralDeletionFlag { get; set; }
        
    }
}
