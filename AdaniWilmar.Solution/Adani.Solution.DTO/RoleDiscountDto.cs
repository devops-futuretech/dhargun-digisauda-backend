using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class RoleDiscountDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long VerticleId { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public decimal Discount { get; set; }
        public decimal HbcDiscout { get; set; }
        public decimal SpecialityFatDiscount { get; set; }

        public bool IsHbc { get; set; }
        public bool IsSpecialityFat { get; set; }

        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SkuDepotDiscountDto : LoginUserIdDto
    {
        public SkuDepotDiscountDto()
        {
            SkuDetails = new List<DropDownDto>();
        }

        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string DiscountReason { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }
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

        public bool IsCustomer { get; set; }
        public bool IsProduct { get; set; }

        public long ZoneId { get; set; }
        public string ZoneName { get; set; }

        public long StateId { get; set; }
        public string StateName { get; set; }

        public long TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public long DistrictId { get; set; }
        public string DistrictName { get; set; }

        public long CityId { get; set; }
        public string CityName { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public List<DropDownDto> SkuDetails { get; set; }
    }

    public class DiscountExportDto
    {
        public long Id { get; set; }
        public string SalesOrganization { get; set; }
        public long SalesOrganizationId { get; set; }
        public string DistributionChannel { get; set; }
        public long DistributionChannelId { get; set; }
        public string Division { get; set; }
        public long DivisionId { get; set; }
        public string OilTypeName { get; set; }
        public string DiscountReason { get; set; }
        public string OilTypeCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<DiscountExportInnerData> DiscountSkuDataList { get; set; }
        public DiscountExportDto()
        {
            DiscountSkuDataList = new List<DiscountExportInnerData>();
        }
    }
    public class DiscountExportInnerData
    {
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string State { get; set; }
        public decimal Discount { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }

    public class CustomerDiscountinputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public int DiscountType { get; set; }
    }

    public class DiscountDto
    {
        public long Id { get; set; }
        public long VerticleId { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal ActualDiscount { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public int DiscountType { get; set; }

        public long ZoneId { get; set; }
        public string ZoneName { get; set; }

        public long StateId { get; set; }
        public string StateName { get; set; }

        public long TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public long DistrictId { get; set; }
        public string DistrictName { get; set; }

        public long CityId { get; set; }
        public string CityName { get; set; }

        public List<long> ZoneIds { get; set; }



        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }

    public class DiscountSkuCityMappingDto
    {
        public long ZoneId { get; set; }
        public long StateId { get; set; }
        public long TerritoryId { get; set; }
        public long DistrictId { get; set; }
        public long CityId { get; set; }
    }

    public class DiscountInputDto : IAPIInputDTO
    {
        public DiscountInputDto()
        {
            Cities = new List<DiscountSkuCityMappingDto>();
        }

        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public string DiscountReason { get; set; }
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public decimal ActualDiscount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long? SubCategoryId { get; set; }
        //BPOrCPWise
        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        public List<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
        public List<long> TerritoryId { get; set; }
        public List<long> DistrictId { get; set; }
        public List<long> CityId { get; set; }
        public long RoleId  { get; set; }
        public List<DiscountSkuCityMappingDto> Cities { get; set; }
        public long PackTypeId { get; set; }
    }

    public class CityDetails
    {
        public long Id { get; set; }

        public long ZoneId { get; set; }
        public string ZoneName { get; set; }

        public long StateId { get; set; }
        public string StateName { get; set; }

        public long TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public long DistrictId { get; set; }
        public string DistrictName { get; set; }

        public long CityId { get; set; }
        public string CityName { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public decimal Discount { get; set; }

        public bool IsChecked { get; set; }

        public long TotalRows { get; set; }
    }

    public class CityMobileDetails
    {
        public long Id { get; set; }

        public long ZoneId { get; set; }
        public string ZoneName { get; set; }

        public long StateId { get; set; }
        public string StateName { get; set; }

        public long TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public long DistrictId { get; set; }
        public string DistrictName { get; set; }

        public long CityId { get; set; }
        public string CityName { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public decimal Discount { get; set; }

        public long ParentId { get; set; }

        public bool IsChecked { get; set; }

        public long TotalRows { get; set; }
    }

    public class TerritoryId
    {
        public List<long> TerritoryIds { get; set; }
        public List<long> CityIds { get; set; }
    }

    public class DiscountOutputDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }
        public long DivisionId { get; set; }
        public string Division { get; set; }
        public string DiscountReason { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }
        public string PackGroupName { get; set; }
        public string PackGroupId { get; set; }
        public long PackTypeId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal ActualDiscount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long ParentId { get; set; }
        public string SkuIdsString { get; set; }
        public List<long> SkuIds { get; set; }
        public List<long> StateIds { get; set; }
        public List<long> ZoneIds { get; set; }
        public List<DiscountSkuCityMappingDto> Cities { get; set; }
        public int TotalRecords { get; set; }
    }

    public class GeographyCityListParam
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public long VerticalId { get; set; }
        public long PageNumber { get; set; }
        public long PageSize { get; set; }
        public bool IsRequestFromWeb { get; set; }
        public string ZoneIds {  get; set; }
        public string StateIds {  get; set; }
        public string DistrictIds {  get; set; }
        public string CityIds {  get; set; }
    }

    public class GeographyDiscountCityListParam
    {
        public long ParentId { get; set; }
        public long PageNumber { get; set; }
        public long PageSize { get; set; }
    }

    public class DiscountUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }
        public long DivisionId { get; set; }
        public string Division { get; set; }
        public string DiscountReason { get; set; }

        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }

        public long SkuId { get; set; }
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public List<long> SkuIds { get; set; }

        public long? SubCategoryId { get; set; }
        //BPOrCPWise
        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public List<long> CustomerId { get; set; }
        public List<long?> StateIds { get; set; }
        public string CustomerName { get; set; }

        public decimal ActualDiscount { get; set; }
        public long LoginUserId { get; set; }
        public long RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string ParentValidFrom { get; set; }
        public string ParentValidTo { get; set; }
        public decimal ParentDiscountAmount { get; set; }
        public bool IsProcessed { get; set; }
        public List<SkuOutputDto> SkuDetails { get; set; }
        public List<DropDownDto> SkuList { get; set; }
        public List<DropDownDto> UserList { get; set; }

    }

    public class EmployeeUserDiscountDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }

        public long VerticleId { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }
        public string DistributionChannelName { get; set; }
        public string DivisionName { get; set; }
        public long DistributionChannelId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long? StateId { get; set; }
        public List<long> SkuIds { get; set; }
        public string OilTypeName { get; set; }
        public string SkuName { get; set; }
        public string StateName { get; set; }

        public List<long> CustomerId { get; set; }
        public string CustomerName { get; set; }

        public decimal ActualDiscount { get; set; }
        public decimal EmpActualDiscount { get; set; }
        public decimal RemainingQuantity { get; set; }
        public string DiscountReason { get; set; }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public long RoleId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime EmpValidFrom { get; set; }
        public DateTime EmpValidTo { get; set; }

    }

    public class SpecialityFatDiscountOutputDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticleId { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal ActualDiscount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long ParentId { get; set; }

        public List<DiscountSkuCityMappingDto> Cities { get; set; }
    }

    public class SpecialityFatDiscountInputDto : IAPIInputDTO
    {
        public SpecialityFatDiscountInputDto()
        {
            Cities = new List<DiscountSkuCityMappingDto>();
        }
        public long Id { get; set; }
        public long VerticleId { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public decimal ActualDiscount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
        public List<long> TerritoryId { get; set; }
        public List<long> DistrictId { get; set; }
        public List<long> CityId { get; set; }

        public List<DiscountSkuCityMappingDto> Cities { get; set; }
    }
    public class SpecialityFatDiscountUserExportDto
    {
        public long Id { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string EmployeeName { get; set; }
        public List<SpecialityFatDiscountInnerListExportDto> SpecialityFatDiscountDetails { get; set; }
        public SpecialityFatDiscountUserExportDto()
        {
            SpecialityFatDiscountDetails = new List<SpecialityFatDiscountInnerListExportDto>();
        }

    }

    public class SpecialityFatDiscountInnerListExportDto
    {
        public long Id { get; set; }
        public decimal QuantityLimit { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
    }
    public class SpecialityFatDiscountUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long VerticleId { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }
        public string DivisionName { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }

        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public List<long> CustomerId { get; set; }
        public string CustomerName { get; set; }

        public decimal QuantityLimit { get; set; }
        public decimal RequestedQuantityLimit { get; set; }
        public long LoginUserId { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public int DiscountType { get; set; }
        public decimal RemainingQuantity { get; set; }
        public long ParentQuantityId { get; set; }
        public long ParentId { get; set; }
        public List<long> SkuIds { get; set; }

        public long? SubCategoryId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public List<DropDownDto> SkuDetails { get; set; }
    }

    public class SpecialityFatEmployeeDiscountDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }

        public long VerticleId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }

        public long HiddenVerticleId { get; set; }
        public string OilTypeName { get; set; }
        public string SkuName { get; set; }

        public List<long> CustomerId { get; set; }
        public List<long> SkuIds { get; set; }
        public string CustomerName { get; set; }

        public decimal ActualDiscount { get; set; }
        public decimal EmpActualDiscount { get; set; }
        public decimal RemainingQuantity { get; set; }
        public decimal RemainingQuantityHidden { get; set; }

        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long RoleId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime EmpValidFrom { get; set; }
        public DateTime EmpValidTo { get; set; }

    }
    public class SpecialityFatEmployeeExportDto 
    {
        public long Id { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilTypeName { get; set; }
        public decimal QuantityLimit { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<SpecialityFatEmployeeDto> InnerList { get; set; }
        public SpecialityFatEmployeeExportDto()
        {
            InnerList = new List<SpecialityFatEmployeeDto>();
        }
    }



    public class SpecialityFatEmployeeDto
    {
        public long Id { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Designation { get; set; }
        public decimal Quantity { get; set; }
        public decimal RemainingQuantity { get; set; }
    }
    public class DiscountParentDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public List<DiscountUserQuantityOutput> DiscountList { get; set; }

        public DiscountParentDto()
        {

            DiscountList = new List<DiscountUserQuantityOutput>();
        }
    }
    public class DiscountUserQuantityOutput
    {
        public long Id { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }
        public long DivisionId { get; set; }
        public string Division { get; set; }
        public string DiscountReason { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Designation { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        //public bool Status { get; set; }
    }

    public class DiscountUserParentChildDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal ActualDiscount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsProcessed { get; set; }
        public decimal ChildActualDiscount { get; set; }
        public DateTime ChildValidFrom { get; set; }
        public DateTime ChildValidTo { get; set; }
        public IList<DiscountUserQuantityOutput> AssignedUserDiscountList { get; set; }
        public IList<SkuOutputDto> ParentSkuList { get; set; }
        public DiscountUserParentChildDto()
        {
            AssignedUserDiscountList = new List<DiscountUserQuantityOutput>();
            ParentSkuList = new List<SkuOutputDto>();
        }
    }

    public class SpecialityFatQuantityLimitParentChildDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsProcessed { get; set; }
        public decimal ChildActualQuantity { get; set; }
        public DateTime ChildValidFrom { get; set; }
        public DateTime ChildValidTo { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public IList<DiscountUserQuantityOutput> AssignedUserQuantityList { get; set; }
        public SpecialityFatQuantityLimitParentChildDto()
        {
            AssignedUserQuantityList = new List<DiscountUserQuantityOutput>();
        }
    }

    public class SpecialityFatDiscountUpdateInputDto
    {
        public long SpecialityFatDiscountId { get; set; }
        public decimal ActualDiscount { get; set; }
    }

    public class DashboardDetailsDto : IAPIInputDTO
    {
        public decimal RegisteredUserCount;
        public decimal ActiveUserCount;
        public decimal TotalUserCount;
        public decimal RecentUsersCount;
        public decimal ActiveMobileUserCount;
        public decimal TotalMobileUserCount;
        public decimal RecentMobileUsersCount;
        public decimal TodayContract { get; set; }
        public decimal TodaySalesOrder { get; set; }
        public decimal TodayInvoice { get; set; }
        public decimal OverDue { get; set; }
        public decimal TomorrowDue { get; set; }
        public bool PostStatus { get ; set ; }
        public string PostMessage { get; set ; }
        public GoogleAnalyticsDataDto GoogleAnalyticsData { get; set; }
    }
}
