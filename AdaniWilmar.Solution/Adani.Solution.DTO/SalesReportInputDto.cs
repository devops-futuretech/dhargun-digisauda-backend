using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SalesReportInputDto : LoginUserIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long DealerId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long IncotermsId { get; set; }
        public List<long> BDOIds { get; set; }
        public int PackTypeId { get; set; }
    }
}
