namespace Adani.Solution.DTO
{
    public class TerritoryUploadDto : CommonResultDto
    {
        public string TerritoryName { get; set; }
        public string StateName { get; set; }
        public string IsActive { get; set; }
        public string Message { get; set; }
    }
}