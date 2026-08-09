
using System;

namespace Adani.Solution.DTO
{
    public class MaterialCostUploadDto : CommonResultDto
    {
        public string PlantCode { get; set; }
        public string VerticalCode { get; set; }
        //OilWise
        public string OilType { get; set; }
        public decimal RateOrMT { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
