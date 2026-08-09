using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaSkuDetailsDto
    {
        public long SkuId { get; set; }
        //Product
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long SkuUomId { get; set; }
        public string SkuUomName { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public decimal MaxAllowableCasesSingleSku { get; set; }
        public decimal MaxAllowableCasesMultipleSku { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal MaximumVehicleCapacityInPercent { get; set; }
        public decimal MaximumVolumeCapacityInPercent { get; set; }
        public long SaudaOrderId { get; set; }
    }
}
