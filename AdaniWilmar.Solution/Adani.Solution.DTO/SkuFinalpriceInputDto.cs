using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SkuFinalpriceListInputDto : IAPIInputDTO
    {
        public long LoginUserId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long OilTypeId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public long[] TransportModeId { get; set; }
        public long OilPackingTypeId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long FreightZoneId { get; set; }
        public long FreightRouteId { get; set; }


        public decimal CounterBidLimit { get; set; }
        public decimal BpCpJump { get; set; }
        public decimal XMargin { get; set; }
        public long VerticalId { get; set; }

        public long SkuId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<long> OilTypeIds { get; set; }
        public List<long> DepotIds { get; set; }
        public List<long> CityIds { get; set; }
        public List<long> StateIds { get; set; }
        public List<long> DistrictIds { get; set; }
        public List<long> TerritoryIds { get; set; }
        public List<long> FreightZoneIds { get; set; }
        public List<long> FreightRouteIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }
        //public List<long> OilTypeIds { get; set; }

        public long BiddingWindowId { get; set; }
        public DateTime BiddingDate { get; set; }
        public string MobileNoList { get; set; }
        public long CustomerGroupId { get; set; }
    }

    public class SaveFinalPricngInputDto : IAPIInputDTO
    {
        public SkuFinalpriceListInputDto inputDto { get; set; }
        public List<SkuFinalpriceListOutputDto> outputDto { get; set; }
        public long BiddingWindowId { get; set; }
        public DateTime BiddingDate { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class PricePublishesDto
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public bool IsPublish { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public string OilType { get; set; }
        public string Plant { get; set; }
        public long? FinalPriceRecordCount { get; set; }
        public long BiddingWindowId { get; set; }
        public string BiddingWindowTiming { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PricePublishInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public DateTime SearchDate { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long RoleId { get; set; }
    }
    public class PricePublistInputDataDto
    {
        public DataSourceRequest DataSourceRequest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long bookingTypeId { get; set; }
        public long OilTypeId { get; set; }
        public long PlantId { get; set; }
        public long RoleId { get; set; }
        public long LoginUserId { get; set; }
    }

    public class PriceErrorMessageDto
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public bool IsPublish { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PublishDate { get; set; }

        public long? FinalPriceRecordCount { get; set; }

        public long BiddingWindowId { get; set; }
        public string BiddingWindowTiming { get; set; }
        public string ErrorMessage { get; set; }
    }


    public class FinalPriceGenerateInputDto : IAPIInputDTO
    {
        public long LoginUserId { get; set; }
        public long DivisionId { get; set; }
        public long DistributionChannelId { get; set; }
        public long SalesOrganizationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long RoleId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }
        public long PlantId { get; set; }
        public long OilTypeId { get; set; }

        public List<long> ZoneIds { get; set; }
        public List<int> StateIds { get; set; }
  

        public decimal CounterBidLimit { get; set; }
        public decimal BpCpJump { get; set; }
        public decimal XMargin { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class FinalPriceGenerateOutputDto
    {
        public long Id { get; set; }
        public DateTime PricingDate { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public string Vertical { get; set; }
        public int TotalState { get; set; }
        public bool IsPublish { get; set; }
        public int PublishButtonStatus { get; set; }
        public int StatusId { get; set; }
    }

    public class FinalPriceGenerateListDto
    {
        public long Id { get; set; }
        public string SAPPricingCode { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilTypeName { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeCode { get; set; }
        public string OilPackingType { get; set; }
        public long PlantId { get; set; }
        public string PlantName { get; set; }
        public string PlantCode { get; set; }
        public string SalesOrganizationName { get; set; }
        public long SalesOrganizationId { get; set; }
        public string DivisionName { get; set; }
        public long DivisionId { get; set; }
        public string DistributionChannelName { get; set; }
        public long DistributionChannelId { get; set; }
        public decimal Price { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidTo { get; set; }
                              
    }

    public class FinalPriceGenerateExportDto
    {
        [DisplayName("Created Date")]
        public String CreatedDate { get; set; }
        [DisplayName("SAP Pricing Code")]
        public string SAPPricingCode { get; set; }
        [DisplayName("Material Name")]
        public string SkuName { get; set; }
        [DisplayName("Material Code")]
        public string SkuCode { get; set; }
        [DisplayName("OilType Name")]
        public string OilTypeName { get; set; }
        [DisplayName("Pack Group")]
        public string OilPackingType { get; set; }
        [DisplayName("Plant Code")]
        public string PlantCode { get; set; }
        [DisplayName("Plant Name")]
        public string PlantName { get; set; }
        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Sales Organization")]
        public string SalesOrganizationName { get; set; }
        [DisplayName("Distribution Channel")]
        public string DistributionChannelName { get; set; }
        [DisplayName("Division")]
        public string DivisionName { get; set; }
        
      
        [DisplayName("Valid From")]
        public String ValidFrom { get; set; }
        [DisplayName("Valid To")]
        public String ValidTo { get; set; }

    }
    public class FinalPriceGenerateDetailOutputDto
    {
        public long Id { get; set; }
        public string OilType { get; set; }
        public string PackGroup { get; set; }
        public string PlantName { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public string Status { get; set; }
        public bool IsPublish { get; set; }
        public int StatusId { get; set; }
        public int TaskStatusId { get; set; }
        public string TaskStatus { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int TotalPriceCount { get; set; }
        public int ErrorMessageCount { get; set; }
    }

    public class FinalPricePublishInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long SaudaBookingTypeId { get; set; }
    }

    public class TaskResultDto
    {
        public Guid Guid { get; set; }
        public long PriceGenerateDetailId { get; set; }
        public long CustomerGroupId { get; set; }
        public long BaseSkuPriceId { get; set; }
    }

    #region RA2.0 Final Price
    public class RaFinalPriceGenerateInputDto : IAPIInputDTO
    {
        public long LoginUserId { get; set; }
        public long VerticalId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }
        public long PlantId { get; set; }
        public List<long> ZoneIds { get; set; }
        public List<int> StateIds { get; set; }
        public DateTime PricingDate { get; set; }
        public long BiddingWindowId { get; set; }
        public DateTime BiddingDate { get; set; }
        public string MobileNumber { get; set; }
        public List<long> CustomerGroupIds { get; set; }

        public decimal CounterBidLimit { get; set; }
        public decimal BpCpJump { get; set; }
        public decimal XMargin { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class RaFinalPriceGenerateOutputDto
    {
        public long Id { get; set; }
        public DateTime PricingDate { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public string Vertical { get; set; }
        public int TotalCustomerGroup { get; set; }
        public bool IsPublish { get; set; }
        public int PublishButtonStatus { get; set; }
        public int StatusId { get; set; }
        public string BiddingWindowName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string WindowStatus { get; set; }
    }

    public class RaFinalPriceGenerateDetailOutputDto
    {
        public long Id { get; set; }
        public string OilType { get; set; }
        public string PackGroup { get; set; }
        public string PlantName { get; set; }
        public string CustomerGroupName { get; set; }        
        public string Status { get; set; }
        public bool IsPublish { get; set; }
        public int StatusId { get; set; }
        public int TaskStatusId { get; set; }
        public string TaskStatus { get; set; }
        public long CustomerGroupId { get; set; }
        public long BiddingWindowId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int TotalPriceCount { get; set; }
        public int ErrorMessageCount { get; set; }
    }

    public class RaPricePublishInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public DateTime SearchDate { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long RoleId { get; set; }
    }
    #endregion

    public class PriceGenerateStatusUpdateDto
    {
        public long PriceGenerateId { get; set; }
        public int StatusId { get; set; }
        public int TaskStatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorMessageCount { get; set; }
    }

    public class ProcessStartDto
    {
        public long PriceGenerateDetailId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public int VerticalId { get; set; }
        public long PriceGenerateId { get; set; }
        public string OilTypeId { get; set; }
        public string PackGroupId { get; set; }
        public long PlantId { get; set; }
        public int StateId { get; set; }
        public long BiddingWindowId { get; set; }
        public long StatusId { get; set; }
        public long CreatedBy { get; set; }
        public long CustomerGroupId { get; set; }
    }
}
