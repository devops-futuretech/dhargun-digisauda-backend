using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaFilterDto : UserIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long? DealerId { get; set; }
        public long? OilTypeId { get; set; }
        public int? SaudaBookingTypeId { get; set; }
        public bool IsExpired { get; set; }
        public int StatusId { get; set; }
        public bool IsConversion { get; set; }
        public long VerticalId { get; set; }
        public long BDOId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
    }

    public class LoginUserIdCoversionDto
    {
        public long LoginUserId { get; set; }
        public bool IsConversion { get; set; }
    }

    public class ChartSaudaSalesByOilTypeInputDto
    {
        public long LoginUserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long VerticalId { get; set; }
    }
    public class ChartSaudaSalesByOilTypeOutputDto
    {
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public decimal SaudaCount { get; set; }
        public decimal SalesCount { get; set; }
    }
    public class ChartApprovalsByOilTypeOutputDto
    {
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public decimal PendingCount { get; set; }
        public decimal ApprovedCount { get; set; }
        public decimal RejectedCount { get; set; }
        public decimal HoldCount { get; set; }
    }

    public class SaudaListFilterDto : KendoGridResult
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long? DealerId { get; set; }
        public long? OilTypeId { get; set; }
        public long? SkuId { get; set; }
        public int? SaudaBookingTypeId { get; set; }
        public bool IsExpired { get; set; }
        public int StatusId { get; set; }
        public bool IsConversion { get; set; }
        public long SalesOrganizationIds { get; set; }
        public long DistributionChannelIds { get; set; }
        public long DivisionIds { get; set; }
        public long PageNo { get; set; }
        public long ZoneId { get; set; }
        public long StateId { get; set; }
        public long DistrictId { get; set; }
        public long CityId { get; set; }
        public List<long> StateIds { get; set; }

        public int? DataFilter { get; set; }
    }

    public class SaudaListAdminAppFilterDto : LoginDealerIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long? DealerId { get; set; }
        public long? VerticalId { get; set; }
        public long? OilTypeId { get; set; }
        public int? SaudaBookingTypeId { get; set; }
        public bool IsExpired { get; set; }
        public int StatusId { get; set; }
        public bool IsConversion { get; set; }
        public long PageNo { get; set; }
        public int? DataFilter { get; set; }
        public List<long> DealerIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> NationalHeadIds { get; set; }
    }
}
