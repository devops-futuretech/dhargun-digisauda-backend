
namespace Adani.Solution.DTO
{
    public class CityUploadDto : CommonResultDto
    {
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string IsActive { get; set; }
    }
}
