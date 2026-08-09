using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SpecialRateAddInputDto
    {
        public long LoginUserId { get; set; }
        public long UserId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long PricingId { get; set; }
        public decimal Quantity { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal SpecialPrice { get; set; }
        public long IncotermsId { get; set; }
        public long PlantId { get; set; }
        public long DealerLocationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StatusId { get; set; }
        public long VerticalId { get; set; }
        public bool IsLTD { get; set; }
        public long BrokerId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
    }
}
