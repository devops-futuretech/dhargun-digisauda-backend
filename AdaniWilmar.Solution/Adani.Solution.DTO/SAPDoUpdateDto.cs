using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SAPDoUpdateDto
    {
        public string DONumber { get; set; }
        public string SoldToParty { get; set; }
        public string ShipToParty { get; set; }
        public string Payer { get; set; }
        public string BillToParty { get; set; }
        public string Vertical { get; set; }
        public decimal OrderQuantity { get; set; }
        public string MaterialNumber { get; set; }        
        public string SaudaNumber { get; set; }
        public string Uom { get; set; }
        public string Enquiry { get; set; }
        public string Reason { get; set; }
    }
}
