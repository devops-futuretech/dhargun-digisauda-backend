using System;


namespace Adani.Solution.DTO
{
    public class SaudaLimitInputDto : LoginUserIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StatusId { get; set; }
    }
}
