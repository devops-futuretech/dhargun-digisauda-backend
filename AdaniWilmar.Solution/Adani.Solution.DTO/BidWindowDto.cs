using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BiddingWindowTimingDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public bool IsLastWindowPerDay { get; set; }
        public bool IsActive { get; set; }

        public string FromTimeString { get; set; }
        public string ToTimeString { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public long LoginUserId { get; set; }
    }

    public interface IAPIInputDTO
    {
        bool PostStatus { get; set; }
        string PostMessage { get; set; }
    }

    public class BidWindowInputDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long CustomerGroupId { get; set; }
        public DateTime BiddingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NoOfAttemptsForBidding { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long LoginUserId { get; set; }
    }

    public class BidWindowDto : IAPIInputDTO
    {
        public BidWindowDto()
        {
            BidWindowVolumeCapacity = new List<BidWindowVolumeCapacityDto>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public List<long> CustomerGroupIds { get; set; }
        public DateTime BiddingDate { get; set; }
        public DateTime StartTimeWithDate { get; set; }
        public DateTime EndTimeWithDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int NoOfAttemptsForBidding { get; set; }
        public bool IsActive { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long LoginUserId { get; set; }

        public DateTime SkuAllocationTimeLimit { get; set; }
        public TimeSpan SaudaAllocationStartTime { get; set; }
        public TimeSpan SaudaAllocationEndTime { get; set; }

        public DateTime SaudaStartTimeWithDate { get; set; }
        public DateTime SaudaEndTimeWithDate { get; set; }

        public int SaudaAllocationStatusId { get; set; }
        public int WindowTimeInterval { get; set; }

        public List<BidWindowVolumeCapacityDto> BidWindowVolumeCapacity { get; set; }
    }

    public class BidWindowVolumeCapacityDto
    {
        public long Id { get; set; }
        public long BiddWindowId { get; set; }
        public long VerticalId { get; set; }
        public long OilTypeId { get; set; }
        public decimal VolumeCapacity { get; set; }
    }

    public class BidWindowListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CustomerGroupName { get; set; }
        public DateTime BiddingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NoOfAttemptsForBidding { get; set; }
        public long LoginUserId { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public bool IsEdit { get; set; }
        public DateTime SaudaAllocationStartTime { get; set; }
        public DateTime SaudaAllocationEndTime { get; set; }
        public string SaudaAllocationStatus { get; set; }
    }

    public class BidWindowVolumeCapacityListDto
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public string VerticalName { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public decimal VolumeCapacity { get; set; }
    }

    public class BidWindowListSearchDto : LoginUserIdDto
    {
        public int StatusId { get; set; }
        public DateTime SearchDate { get; set; }
    }


    public class BiddingWindowDashboardDto
    {
        public string CustomerGroups { get; set; }
        public string WindowName { get; set; }
        public long BiddingWindowId { get; set; }
        public string Oiltypes { get; set; }
        public string OilTypeName { get; set; }
        public string WindowStartAndEndTime { get; set; }
        public string SaudaAllocationStartAndEndTime { get; set; }
        public long StatusId { get; set; }
        public long StateId { get; set; }
        public long OilTypeId { get; set; }
        public long ApprovedCount { get; set; }
        public long PendingCount { get; set; }
        public long RejectedCount { get; set; }
        public string SaudaBooked { get; set; }
        public decimal BidQuantityAccepted { get; set; }
        public decimal BidQuantityRejected { get; set; }
        public decimal BidQuantityPending { get; set; }
        public string WindowStatusName { get; set; }
        public decimal BookedVolumeCapacity { get; set; }
        public decimal TotalVolumeCapacity { get; set; }
        public string PlantName { get; set; }
        public DateTime WindowStartTime  { get; set; }
        public DateTime WindowEndTime { get; set; }

    }

    public class BiddingWindowDashboardChartDto
    {
        public string Status { get; set; }
        public long StatusCount { get; set; }

    }


    public class BiddingWindowDashboardChartVolumeCapacityDto
    {
        public long OilTypeId { get; set; }
        public string OilName { get; set; }
        public decimal RemainingVolumeCapacity { get; set; }
        public decimal TotalVolumeCapacity { get; set; }
        public int RemainingVolumeCapacityInLong { get; set; }
        public int TotalVolumeCapacityInLong { get; set; }
        public decimal BookedVolumeCapacity { get; set; }
    }

    public class IdDiscountAndBenefitInputDto : IAPIInputDTO
    {
        public long LoginUserId { get; set; }
        public long DiscountType { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public bool IsActive { get; set; }
        public long Id { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class BidWindowExportDto
    {
        public string Name { get; set; }
        public string CustomerGroupNames { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int NoOfAttemptsForBidding { get; set; }
        public string WindowStatus { get; set; }
        public string Verticals { get; set; }
        public string OilName { get; set; }
        public decimal VolumeCapacity { get; set; }
    }

    public class BiddingWindowVolumeCapacityDto
    {
        public string Oiltypes { get; set; }
        public long ApprovedCount { get; set; }
        public long PendingCount { get; set; }
        public long RejectedCount { get; set; }
        public decimal BidQuantityAccepted { get; set; }
        public decimal BidQuantityRejected { get; set; }
        public decimal BidQuantityPending { get; set; }
    }

    public class BiddingWindowStatusWiseCountDto
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string OilType { get; set; }
        public int TotalBidding { get; set; }
        public decimal TotalVolume { get; set; }
        public long ApprovedCount { get; set; }
        public long PendingCount { get; set; }
        public long RejectedCount { get; set; }
        public decimal BidQuantityAccepted { get; set; }
        public decimal BidQuantityRejected { get; set; }
        public decimal BidQuantityPending { get; set; }
    }

    public class BiddingWindowStatusWiseDetailsDto
    {
        public string StatusName { get; set; }
        public long? TotalBidding { get; set; }
        public decimal? TotalVolume { get; set; }
    }

    public class SkuCustomerDto
    {
        public string Name { get; set; }
        public long SkuId { get; set; }
        public long CustomerId { get; set; }
        public long CityId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal Discount { get; set; }
        public long CustomerGroupId { get; set; }
        public long BenefitTypesId { get; set; }
        public long BenefitOrCategoryId { get; set; } 
        public decimal DiscountOrDays { get; set; }
    }
}

