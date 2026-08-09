namespace Adani.Solution.DTO
{
    public class SkuShortViewOutputDto
    {
        public long SpecialRateId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityCase { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal SpecialPrice { get; set; }
        public decimal LiftedDate { get; set; }
        public decimal ImpactOnMarginMT { get; set; }
        public decimal ImpactOnMarginCase { get; set; }
        public string IncotermsName { get; set; }
        public bool IsRake { get; set; }
        public string DealerLocationName { get; set; }
        public string PlantName { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public bool IsLTD { get; set; }
    }
}
