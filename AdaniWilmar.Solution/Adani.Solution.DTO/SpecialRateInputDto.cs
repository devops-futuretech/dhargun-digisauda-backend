using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SpecialRateInputDto
    {
        public long LoginUserId { get; set; }
        public long BDOId { get; set; }
        public long? DealerId { get; set; }
        public long? OilTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long ZHId { get; set; }
        public long StatusId { get; set; }
    }
}
