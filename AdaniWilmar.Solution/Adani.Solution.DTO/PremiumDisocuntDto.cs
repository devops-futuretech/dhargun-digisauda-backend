using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PremiumDisocuntDto : LoginUserIdDto
    {
        public PremiumDisocuntDto()
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

        public DateTime ValidFrom { get; set; }        
        public DateTime ValidTo { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
                
        public List<SkuDiscounts> SkuDiscounts { get; set; }
    }

    public class PremiumDisocuntInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long OilTypeId { get; set; }
    }


    public class PremiumUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }

        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }

        public long? SubCategoryId { get; set; }

        public long SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public List<long> CustomerId { get; set; }
        public string CustomerName { get; set; }

        public decimal ActualPremium { get; set; }
        public long LoginUserId { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public string ParentValidFrom { get; set; }
        public string ParentValidTo { get; set; }
        public bool IsProcessed { get; set; }

        //BPOrCPWise
        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        public List<SkuOutputDto> SkuDetails { get; set; }
    }

    public class PremiumUserListParam
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
    }

    public class EmployeeUserPremiumDto : IAPIInputDTO
    {
        public long Id { get; set; }

        public long VerticleId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }

        public string OilTypeName { get; set; }
        public string SkuName { get; set; }

        public List<long> CustomerId { get; set; }
        public string CustomerName { get; set; }

        public decimal ActualPremium { get; set; }
        public decimal EmpActualPremium { get; set; }

        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime EmpValidFrom { get; set; }
        public DateTime EmpValidTo { get; set; }

    }


    public class PremiumInputDto : IAPIInputDTO
    {

        public PremiumInputDto()
        {
            Cities = new List<DiscountSkuCityMappingDto>();
        }
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public decimal ActualPremium { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long? SubCategoryId { get; set; }

        public List<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
        public List<long> TerritoryId { get; set; }
        public List<long> DistrictId { get; set; }
        public List<long> CityId { get; set; }

        //BPOrCPWise
        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        public List<DiscountSkuCityMappingDto> Cities { get; set; }
    }

    public class PremiumOutputDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticleId { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal ActualPremium { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long ParentId { get; set; }

        public List<DiscountSkuCityMappingDto> Cities { get; set; }
    }

    public class PremiumUserQuantityOutput
    {
        public long Id { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Designation { get; set; }
        public decimal Premium { get; set; }
    }

    public class PremiumUserParentChildDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal ActualPremium { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsProcessed { get; set; }
        public decimal ChildActualPremium { get; set; }
        public DateTime ChildValidFrom { get; set; }
        public DateTime ChildValidTo { get; set; }
        public IList<PremiumUserQuantityOutput> AssignedUserPremiumList { get; set; }
        public PremiumUserParentChildDto()
        {
            AssignedUserPremiumList = new List<PremiumUserQuantityOutput>();
        }
    }
}
