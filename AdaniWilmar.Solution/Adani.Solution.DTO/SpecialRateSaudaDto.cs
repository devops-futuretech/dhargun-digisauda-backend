using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SpecialRateSaudaDto
    {
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public DateTime BiddingDate { get; set; }
        public long StatusId { get; set; }
        public string Remarks { get; set; }
        public long DealerTypeId { get; set; }
        public long BrokerId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public List<SpecialRateIdInfoDto> SpecialRateIdInfo { get; set; }

        public SpecialRateSaudaDto()
        {
            SpecialRateIdInfo = new List<SpecialRateIdInfoDto>();
        }
    }
    

    public class SpecialRateIdInfoDto
    {
        public long SpecialRateIds { get; set; }
        public long QuantityInCases { get; set; }
        public DateTime? SaudaValidFromDate { get; set; }
        public long DealerId { get; set; }
    }
}
