using System;

namespace Adani.Solution.DTO
{
    public class HoneyCombCostUploadDto : CommonResultDto
    {
        public string PlantCode { get; set; }
        public string OilType { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string TransportMode { get; set; }
        public decimal CostPerMT { get; set; }
        public string VerticalCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long CreatedBy { get; set; }

        //public string IsActive { get; set; }
    }
}
