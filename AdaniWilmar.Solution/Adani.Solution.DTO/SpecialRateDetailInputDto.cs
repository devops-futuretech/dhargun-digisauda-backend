using System;

namespace Adani.Solution.DTO
{
    public class SpecialRateDetailInputDto:LoginUserIdDto
    {
        public long DealerId { get; set; }
        public DateTime RequestDate { get; set; }
        public int StatusId { get; set; }
        public long SpecialRateId { get; set; }
    }
}
