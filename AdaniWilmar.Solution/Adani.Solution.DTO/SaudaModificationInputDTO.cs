using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaModificationInputDTO
    {
        public long DealerId { get; set; }
        public long LoginUserId { get; set; }
        public string SaudaNumber { get; set; }
        public List<SaudaModificationOilTypeDetails> OilTypes { get; set; }
    }

    public class SaudaModificationOilTypeDetails
    {
        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public List<SaudaModificaitonPackTypeDetails> PackTypes { get; set; }
    }

    public class SaudaModificaitonPackTypeDetails
    {
        public long? PackTypeId { get; set; }
        public string PackTypeName { get; set; }
        public decimal OriginalMT { get; set; }
        public decimal ModifiedMT { get; set; }
        public decimal DifferenceMT { get; set; }
        public List<SaudaModificationSkuDetails> Skus { get; set; }
    }

    public class SaudaModificationSkuDetails
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal BasicRate { get; set; }
        public decimal PendingQuantityInCase { get; set; }
        public decimal PendingQuantityInCaseCopy { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal SaudaQuantity { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public Boolean IsDelete { get; set; }
        public decimal Price { get; set; }
        public decimal EmployeeSkuDiscount { get; set; }
    }

    public class SaudaModificationUpdateDto : IAPIInputDTO
    {
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long ModifiedBy { get; set; }
        public List<long> SaudaModificationIds { get; set; }
        public long LoginUserId { get; set; }
        public SaudaModificationUpdateDto()
        {
            SaudaModificationIds = new List<long>();
        }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
