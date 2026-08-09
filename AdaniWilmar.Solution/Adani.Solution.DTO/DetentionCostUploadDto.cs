using System;
namespace Adani.Solution.DTO
{
    public class DetentionCostUploadDto : CommonResultDto
    {
        public string DepotCode { get; set; }
        public decimal CostPerMT { get; set; }
        public string VerticalCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
