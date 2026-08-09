using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaLimitRequestDto
    {
        public IList<SaudaLimitRequestDetailDto> LimitRequest { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public SaudaLimitRequestDto()
        {
            LimitRequest = new List<SaudaLimitRequestDetailDto>();
        }
    }
    public class SaudaLimitRequestDetailDto
    {
        public long Id { get; set; }
        public decimal RequestedLimitRequest { get; set; }
    }

    public class SaudaLimitRequestInputDto:LoginUserIdDto
    {
        public long Id { get; set; }
        public decimal RequestedLimitRequest { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }
    }
}
