using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.DTO
{
    public class RoleDisocuntDto : LoginUserIdDto
    {
        public RoleDisocuntDto()
        {
            SkuDiscounts = new List<SkuDiscounts>();
        }
        public long Id { get; set; }

        public long RoleId { get; set; }
        public string RoleName { get; set; }

        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }

        public long VerticleId { get; set; }
        public string VerticleName { get; set; }

        public long SaudaBookingTypeId { get; set; }
        public string SaudaBookingTypeName { get; set; }

        public decimal ActualDiscount { get; set; }
        public decimal RequestedDiscount { get; set; }
        public int Status { get; set; }
        public long ApprovedBy { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        //public List<RoleDisocuntStatusDto> RoleDisocunts { get; set; }
        public List<SkuDiscounts> SkuDiscounts { get; set; }
    }

    public class RoleDisocuntStatusDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public long SkuName { get; set; }
        public decimal ActualDiscount { get; set; }
        public decimal RequestedDiscount { get; set; }
        public bool Status { get; set; }
    }

    public class RoleDisocuntInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long OilTypeId { get; set; }
    }

    //public class RequestDisocuntInputDto : LoginUserIdDto
    //{
    //    public long RoleId { get; set; }
    //    public long SkuId { get; set; }
    //}

    //Sku Discount
    public class SkuDiscounts : LoginUserIdDto
    {
        public SkuDiscounts()
        {
            SkuDropDown = new SkuDropDown();
        }
        public long Id { get; set; }
        public decimal ActualDiscount { get; set; }

        [UIHint("SkuDropDownPartial")]
        public SkuDropDown SkuDropDown { get; set; }
    }

    public class SkuDropDown
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string Code { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public decimal Unit { get; set; }
    }
}
