using System;
using System.Collections.Generic;
namespace Adani.Solution.DTO
{
    public class SpecialRateDetailsDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public long StatusId { get; set; }
        public string Remarks { get; set; }
        public string SaudaLimitExceedRemarks { get; set; }
        public bool IsAccessToApprove { get; set; }
        public IList<SkuShortViewOutputDto> SkuList { get; set; }
        public SpecialRateDetailsDto()
        {
            SkuList = new List<SkuShortViewOutputDto>();
        }
    }
}
