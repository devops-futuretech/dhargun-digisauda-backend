using System;

namespace Adani.Solution.DTO
{
    public class PrimaryDiscountUserDto : LoginUserIdDto, IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticleId { get; set; }
        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long DepotId { get; set; }
        public string DepotName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal ActualDiscount { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public int DiscountType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsProduct { get; set; }
    }

    public class PrimaryDiscountUserInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public int DiscountType { get; set; }
    }
}
