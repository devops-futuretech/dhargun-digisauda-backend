
namespace Adani.Solution.DTO
{
    public class NewIndentSkuListDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public decimal MaxAllowableCasesSingleSku { get; set; }
        public decimal MaxAllowableCasesMultipleSku { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal MaximumVehicleCapacityInPercent { get; set; }
        public decimal MaximumVolumeCapacityInPercent { get; set; }
        public long OilTypeId { get; set; }
        public string SkuCode { get; set; }
    }
}
