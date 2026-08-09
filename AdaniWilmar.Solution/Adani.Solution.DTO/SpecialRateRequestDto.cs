using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SpecialRateRequestDto
    {
        public IList<long> SpecialRateRequest { get; set; }
        public string Remark { get; set; }
        public long LoginUserId { get; set; }
        public int Status { get; set; }
        public SpecialRateRequestDto()
        {
            SpecialRateRequest = new List<long>();
        }
    }
}
