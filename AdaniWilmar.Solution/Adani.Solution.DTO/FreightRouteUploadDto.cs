using System;
namespace Adani.Solution.DTO
{
    public class FreightRouteUploadDto :CommonResultDto
    {
        public string FreightZoneName { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
