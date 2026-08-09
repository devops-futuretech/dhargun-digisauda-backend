using System;

namespace Adani.Solution.DTO
{
    public class PremiumDisocuntRequestDto: LoginUserIdDto, IAPIInputDTO
    {
        public long Id { get; set; }

        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long VerticleId { get; set; }
        public string VerticleName { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public string SaudaBookingTypeName { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public decimal ActualDiscount { get; set; }
        public decimal RequestedDiscount { get; set; }
        public int Status { get; set; }
        public long ApprovedBy { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class PremiumDisocuntRequestInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long SkuId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class ApprovePremiunDiscountRequestDto : LoginUserIdDto, IAPIInputDTO
    {
        public long Id { get; set; }
        public string Reason { get; set; }
        public int ReasonType { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    //public class PremiumDisocuntRequestUpdateDto : LoginUserIdDto, IAPIInputDTO
    //{
    //    public long Id { get; set; }
    //    public decimal RequestedDiscount { get; set; }
    //    public DateTime ValidFrom { get; set; }
    //    public DateTime ValidTo { get; set; }
    //    public bool PostStatus { get; set; }
    //    public string PostMessage { get; set; }
    //}
}
