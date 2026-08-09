using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SAPTradeTicketDetailsDto
    {
        public long TradeTicketId { get; set; }
        public long TradeTicketDetailsId { get; set; }
        //public long OilTypeId { get; set; }
        //public decimal ProcessCost { get; set; }
        //public string Proportion { get; set; }
        //public decimal ProportionValues { get; set; }
        //public decimal OilCost { get; set; }
        //public string OilType { get; set; }
        //public string OilName { get; set; }
        public string TradeTicketNumber { get; set; }
        public string MATERIAL_TYPE { get; set; }
        public decimal PRICE { get; set; }
        public decimal PRCOST { get; set; }
        public decimal PROPORTION { get; set; }
    }

    public class HANATradeTicketDetailsDto
    {    
        public string TradeTicketNumber { get; set; }
        public string MATERIAL_TYPE { get; set; }
        public decimal PRICE { get; set; }
        public decimal PRCOST { get; set; }
        public decimal PROPORTION { get; set; }
    }
}
