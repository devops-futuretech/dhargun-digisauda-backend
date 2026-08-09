using System;

namespace Adani.Solution.DTO
{
    public class PrimaryFreightUploadDto : CommonResultDto
    {
        public string PlantCode { get; set; }
        public string DepotCode { get; set; }
        public string TransportMode { get; set; }
        public decimal LoadCapacity { get; set; }
        //public string HireCost { get; set; }
        public string VerticalCode { get; set; }
        public decimal ActualFreight { get; set; }
        public decimal SalesFreight { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
