using Adani.Solution.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConversionSkuDetail : Auditable
    {
        public long SaudaConversionSkuId { get; set; }
        public long SaudaConversionUnitAndDifferenceRateDetailsId { get; set; }
        public long ToSkuId { get; set; }
        public decimal ToQuantityInSku { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal ToQuantityInMt { get; set; }
        public long? ToSaudaOrderId { get; set; }
        public string ToSaudaNumber { get; set; }
        public decimal? ToBaseRate { get; set; }
        public string TradeTicketNumber { get; set; }
        public string Remarks { get; set; }

        public virtual SaudaConversionSku SaudaConversionSku { get; set; }        
    }
}