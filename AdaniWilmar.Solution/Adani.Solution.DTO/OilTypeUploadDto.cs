namespace Adani.Solution.DTO
{
    public class OilTypeUploadDto: CommonResultDto
    {
        public string Name { get; set; }        
        public string Code { get; set; }        
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public decimal LitreConversion { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string IsRasoi { get; set; }
    }
}
