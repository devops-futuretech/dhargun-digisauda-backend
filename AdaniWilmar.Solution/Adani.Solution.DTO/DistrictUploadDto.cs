namespace Adani.Solution.DTO
{
    public class DistrictUploadDto : CommonResultDto
    {
        public string DistrictName { get; set; }
        public string TerritoryName { get; set; }
        public string StateName { get; set; }
        public string IsActive { get; set; }
    }
}