using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConditionalBookingSkuMappingDto
    {
        public long Id { get; set; }
        public long EssentialSkuId { get; set; }
        public string EssentialSkuName { get; set; }
        public long MandatorySkuId { get; set; }
        public string MandatorySkuName { get; set; }
        public decimal MandatoryBookingQuantityPercentage { get; set; }
        public bool IsActive { get; set; }
    }

    public class SaudaConditionalBookingSkuMappingListDto
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long EssentialSkuId { get; set; }
        public string EssentialSkuName { get; set; }
        public string EssentialSkuCode { get; set; }
        public long MandatorySkuId { get; set; }
        public string MandatorySkuName { get; set; }
        public string MandatorySkuCode { get; set; }
        public decimal MandatoryBookingQuantityPercentage { get; set; }
        public bool IsActive { get; set; }
        public string OilType { get; set; }
    }

    public class SaudaConditionalBookingSkuDto
    {
        public long Id { get; set; }
        public List<long> EssentialSkuId { get; set; }
        public string EssentialSkuName { get; set; }      
        public long EssentialPackGroupId { get; set; }      
        public List<long> EssentialOilTypeId { get; set; }      
        public List<SaudaConditionalBookingEssentialSkuMappingDto> EssentialSkuData { get; set; }      
        public bool IsActive { get; set; }
        public List<SaudaConditionalBookingMandatorySkuMappingDto> MandatorySkuMappingList { get; set; }
    }

    public class SaudaConditionalBookingSkuOutputDto
    {
        public long Id { get; set; }
        public List<long> EssentialSkuId { get; set; }
        public List<string> EssentialSkuName { get; set; }
        public List<string> EssentialSkuCode { get; set; }
        public bool IsActive { get; set; }
        public List<SaudaConditionalBookingMandatorySkuPricingDto> MandatorySkuMappingList { get; set; }
    }

    public class SaudaConditionalBookingMandatorySkuMappingDto
    {
        public long ParentId { get; set; }
        public long MandatorySkuId { get; set; }
        public long MandatoryOilTypeId { get; set; }
        public long MandatoryPackGroupId { get; set; }
        public string MandatorySkuName { get; set; }
        public string MandatorySkuCode { get; set; }
        public decimal MandatoryBookingQuantityPercentage { get; set; }
        public decimal EmployeeSkuDiscount { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal BasicRate { get; set; }
    }

    public class SaudaConditionalBookingEssentialSkuMappingDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
    }

    public class SaudaConditionalBookingMandatorySkuPricingDto
    {
        public long MandatorySkuId { get; set; }
        public string MandatorySkuName { get; set; }
        public string MandatorySkuCode { get; set; }
        public decimal MandatoryBookingQuantityPercentage { get; set; }
        public decimal MandatorySkuQuantity { get; set; }
        public long PricingId { get; set; }
        public long OilTypeId { get; set; }
        public decimal MandatorySkuPrice { get; set; }
        public decimal EmployeeSkuPremium { get; set; }
        public decimal EmployeeSkuPremiumId { get; set; }
        public decimal EmployeeSkuDiscount { get; set; }
        public decimal EmployeeSkuDiscountId { get; set; }
        public long PlantId { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public long UOMId { get; set; }
        public string UOM { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
    }
}
