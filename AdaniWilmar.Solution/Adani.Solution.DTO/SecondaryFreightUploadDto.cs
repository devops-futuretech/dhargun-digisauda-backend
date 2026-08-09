using System;

namespace Adani.Solution.DTO
{
    public class SecondaryFreightUploadDto : CommonResultDto
    {
        public string PlantOrDepotCode { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public string FreightZone { get; set; }
        public string FreightRoute { get; set; }
        public decimal LoadCapacity { get; set; }
        public string TransportMode { get; set; }
        public string VerticalCode { get; set; }
        public decimal ActualFreight { get; set; }
        public decimal SalesFreight { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
