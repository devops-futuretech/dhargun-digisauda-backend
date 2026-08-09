using Adani.Solution.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConversionSku : Auditable
    {        
        public long SkuId { get; set; }
        public decimal QuantityInSku { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal QuantityInMt { get; set; }
        public long OilTypeId { get; set; }
        public long DealerId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long? SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public long? SaudaConversionSkuHeaderId { get; set; }
        public string TradeTicketNumber { get; set; }
        

        [MaxLength(1000)]
        public string Remarks { get; set; }
        public decimal? BaseRate { get; set; }
        public bool IsSAPDataSync { get; set; }
        public bool IsApproved { get; set; }
        public bool IsNotSyncToSAP { get; set; }
        public bool SaudaConversionUpdateFromSap { get; set; }
        public long StatusId { get; set; }
    }
}