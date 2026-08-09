using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConversionOrder : Auditable
    {
        public long SaudaConversionId { get; set; }
        public long SaudaId { get; set; }
        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public decimal QuotedPrice { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal BidQuantity { get; set; }

        public decimal BidQuantityCase { get; set; }
        public decimal BidPrice { get; set; }
        public string TradeTicketNumber { get; set; }

        public virtual SaudaConversion SaudaConversion { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
