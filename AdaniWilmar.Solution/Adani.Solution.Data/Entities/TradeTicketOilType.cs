using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class TradeTicketOilType : Auditable
    {
        public string OilTypeName { get; set; }      
        public bool IsActive { get; set; }
        public string SAPId { get; set; }
        public long DivisionId { get; set; }
    }
}
