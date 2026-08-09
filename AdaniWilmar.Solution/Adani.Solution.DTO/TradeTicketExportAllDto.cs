using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
  public  class TradeTicketExportAllDto
    {
        public long TradeTicketId { get; set; }
        public string ContractType { get; set; }
        public string MaterialType { get; set; }
        public string BookingType { get; set; }
        public string UnitOfMeasure { get; set; }
        public string PlantOrVendor { get; set; }
        public string TradeTicketNumber { get; set; }
        public DateTime ContractDate { get; set; }
        public decimal ContractQuantity { get; set; }
        public decimal SaudaBookedQuantity { get; set; }
        public decimal OpenQty { get; set; }
        public string PlantName { get; set; }
        public string TradeTicketOilTypes { get; set; }
        public DateTime SAPCreationDate { get; set; }
        public decimal RatePerMT { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public decimal OtherElement { get; set; }
    }
}
