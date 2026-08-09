using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SpecialtyFatQuantityRequestDto : IAPIInputDTO
    {
        public IList<long> QuantityRequestIds { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }       
        public long SkuId { get; set; }
        public long OiltypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal updateQuantity { get; set; }
        public long StatusId { get; set; }
        public long SpecialtyFatQuantityLimitId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string UserName { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }
        public string Status { get; set; }
        public long LoginUserId { get; set; }
        public decimal RemainingQuantity { get; set; }
        public long ParentQuantityId { get; set; }
        public bool IsChecked { get; set; }
        public bool IsRequestedUser { get; set; }
        public long SpecialtyFatQuantityRequestId { get; set; }
        public long RoleId { get; set; }
        public bool IsApprove { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public long VerticleId { get; set; }
    }
}
