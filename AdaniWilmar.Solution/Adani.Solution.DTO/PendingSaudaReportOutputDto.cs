using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingSaudaReportOutputDto
    {
        public long SaudaOrderId { get; set; }
        public string BDOName { get; set; }
        public string PlantName { get; set; }
        public string DepotName { get; set; }
        public string DealerName { get; set; }
        public string SaudaNumber { get; set; }
        public string OilType { get; set; }
        public string SkuName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal ContractQtyInCase { get; set; }
        public decimal ContractQtyInMT { get; set; }
        public decimal PendingQtyInCase { get; set; }
        public decimal PendingQtyInMT { get; set; }
        public decimal SaudaBidPrice { get; set; }
    }
}
