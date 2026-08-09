using System;
namespace Adani.Solution.DTO
{
    public class SaudaOrderListDto
    {
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public string SkuName { get; set; }
        public string OilTypeName { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public DateTime BookedDate { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
