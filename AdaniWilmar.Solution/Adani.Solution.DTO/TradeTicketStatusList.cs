using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketStatusListDto
    {
        public long TradeTicketId { get; set; }
        public string TradeTicketNumber { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal SaudaQuantity { get; set; }
        public decimal OpenQty { get; set; }
        public string PlantName { get; set; }
        public decimal RatePerMT { get; set; }
        public string TradeTicketOilTypes { get; set; }
        public long DepotId { get; set; }
        public DateTime SAPCreationDate { get; set; }
        public string MaterialType { get; set; }
        public string TTStatus { get; set; }
    }
}
