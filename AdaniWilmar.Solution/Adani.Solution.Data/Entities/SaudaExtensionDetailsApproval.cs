using System;
using Adani.Solution.Data.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaExtensionDetailsApproval : Auditable
    {
        public long SaudaOrderId { get; set; }
        public long PendingContractId { get; set; }
        public string SaudaNumber { get; set; }
        public string RequestDate { get; set; }
        public string ExtentionDateCount { get; set; }
        public bool IsApproval { get; set; }
        public DateTime SaudaValidFrom { get; set; }
        public DateTime SaudaValidTo { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal BasicRate { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal SaudaQuantityMT { get; set; }
        public decimal PendingQuantityCase { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal PendingQuantityMT { get; set; }
        public decimal SaudaQuantityCase { get; set; }
        public string SkuCode { get; set; }
        public string UserCode { get; set; }
        public string SAPRemarks { get; set; }
        public string Remarks { get; set; }
        public bool IsSAPDataSync { get; set; }
        public DateTime SaudaRequestDate { get; set; }
        public bool SaudaExtensionUpdateFromSap { get; set; }
    }
}
