using System;
namespace Adani.Solution.DTO
{
    public class FreightZoneUploadDto : CommonResultDto
    {
        public string Name { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
