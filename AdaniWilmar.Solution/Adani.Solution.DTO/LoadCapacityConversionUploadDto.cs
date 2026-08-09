using System;

namespace Adani.Solution.DTO
{
    public class LoadCapacityConversionUploadDto :CommonResultDto
    {
        public string TransportMode { get; set; }
        public decimal LoadCapacity { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string LoadQuantity { get; set; }
        public string VerticalCode { get; set; }
        public string OilType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string ActualLoadQuantity { get; set; }
    }
}
