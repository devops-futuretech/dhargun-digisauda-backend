using System;

namespace Adani.Solution.DTO
{
    public class SaudaReportFilterDto:LoginUserIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long DealerId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long IncotermsId { get; set; }
        public int BookingTypeId { get; set; }
        public int StatusId { get; set; }
    }
}
