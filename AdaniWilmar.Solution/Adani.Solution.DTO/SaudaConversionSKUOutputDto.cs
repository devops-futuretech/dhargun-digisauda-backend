using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionSKUOutputDto
    {
        public long SkuId { get; set; }
        public long SaudaConversionUnitAndDifferenceRateDetailsId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public decimal Unit { get; set; }
        public decimal BasicRateDifference { get; set; }
        public decimal SaudaConversionMin { get; set; }
        public decimal SaudaConversionMax { get; set; }


    }
}
