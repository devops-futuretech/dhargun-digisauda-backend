using Adani.Solution.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class TradeTicket: Auditable
    {
        [Required]
        public int ContractTypeId { get; set; }
        public int MaterialTypeId { get; set; }
        public int BookingTypeId { get; set; }
        public long DepotId { get; set; }
        public long UomId { get; set; }
        public long DivisionId { get; set; }
        public string TradeTicketNumber { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal ContractQuantity { get; set; }

        public string UnitOfMeasure { get; set; }        
        public decimal OtherElement { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ContractDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidTo { get; set; }

        public bool IsSAPDataSync { get; set; }

        public string ContractType { get; set; }
        public string BookingType { get; set; }
        public string MaterialType { get; set; }
        public decimal TotalOilCost { get; set; }
        public decimal TotalProcessCost { get; set; }
        public decimal TotalCost { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal OpenQuantityFromSap { get; set; }
        public string TTStatus { get; set; }
    }
}
