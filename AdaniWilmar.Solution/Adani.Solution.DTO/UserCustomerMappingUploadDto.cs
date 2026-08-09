namespace Adani.Solution.DTO
{
    public class UserCustomerMappingUploadDto : CommonResultDto
    {
        public long CreatedBy { get; set; }
        public string UserCode { get; set; }
        public string CustomerCode { get; set; }
        public string DivisionCode { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string IsDealer { get; set; }
        public string IsDeleteOldMapping { get; set; }
        public string IsUnassign { get; set; }
    }
}
