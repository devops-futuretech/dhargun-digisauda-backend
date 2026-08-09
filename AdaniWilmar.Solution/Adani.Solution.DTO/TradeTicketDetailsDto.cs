using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class TradeTicketDetailsDto
    {       
        public long TradeTicketId { get; set; }
        public long TradeTicketDetailsId { get; set; }
        public long OilTypeId { get; set; }       
        public decimal ProcessCost { get; set; }
        public decimal ProcessCostProportion { get; set; }
        public decimal Proportion { get; set; }       
        public decimal OilCost { get; set; }
        public decimal OilCostCalculated { get; set; }
        public string OilType { get; set; }
        public string OilName { get; set; }
        public string TradeTicketNumber { get; set; }
        public long VerticalId { get; set; }
    }
}
