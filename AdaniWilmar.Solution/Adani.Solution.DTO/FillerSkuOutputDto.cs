using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FillerSkuOutputDto
    {
        public long SkuId { get; set; }
        public long PackTypeId { get; set; }
        public long UserId { get; set; }
        public decimal BidedCases { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public decimal SuggestedQuantity { get; set; }
        public decimal MaxAllowableSingleSku { get; set; }
        public decimal MaxAllowableMultipleSku { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal  CaseToMetricTon { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string PackType { get; set; }
        public long OilTypeId { get; set; }
    }
}
