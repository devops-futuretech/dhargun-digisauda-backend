using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class DistributorStockEntryDetail : Auditable
    {
        public long DistributorStockEntryId { get; set; }
        public long SkuId { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal QuantityInCase { get; set; }
        [DecimalPrecision(18, 8)]
        public decimal QuantityInMT { get; set; }
        public virtual DistributorStockEntry DistributorStockEntry { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
