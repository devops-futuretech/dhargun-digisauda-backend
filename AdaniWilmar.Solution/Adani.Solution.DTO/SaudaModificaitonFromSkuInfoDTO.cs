using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaModificaitonFromSkuInfoDTO
    {
        public long Id { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long? OilTypeId { get; set; }

        public decimal PendingQuantityInCase { get; set; }
        public decimal SaudaQuantity { get; set; }
        public decimal BasicRate { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public long? OilPackGroupTypeId { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
