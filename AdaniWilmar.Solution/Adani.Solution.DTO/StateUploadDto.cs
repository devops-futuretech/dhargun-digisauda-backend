namespace Adani.Solution.DTO
{
    public class StateUploadDto: CommonResultDto
    {
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string IsActive { get; set; }
    }
}