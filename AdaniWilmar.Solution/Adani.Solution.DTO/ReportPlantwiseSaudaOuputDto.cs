using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ReportPlantwiseSaudaOuputDto
    {
        public long PlantId { get; set; }
        public string PlantName { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public decimal MaterialQtyInMT { get; set; }
        public decimal MaterialQtyInCase { get; set; }
        public decimal RealizationPMT { get; set; }
        public decimal PurchasePMT { get; set; }
        public decimal MarginPMT { get; set; }
        public string ProductGroup { get; set; }
        public string PackSize { get; set; }
    }
}
