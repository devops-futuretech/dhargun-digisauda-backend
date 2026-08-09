using System;

namespace Adani.Solution.DTO
{
    public class PackingCostUploadDto : CommonResultDto
    {
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string VerticalCode { get; set; }
        public string OilType { get; set; }
        public string PlantCode { get; set; }
        public decimal ActualPackingCost { get; set; }
        public decimal SalesPackingCost { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public long CreatedBy { get; set; }
    }
}
