using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASaudaAmendmentDtoList
    {
        public List<SAPSaudaAmendmentDto> SAPSaudaAmendmentList { get; set; }
        public HANASaudaAmendmentDtoList()
        {
            SAPSaudaAmendmentList = new List<SAPSaudaAmendmentDto>();
        }
    }

    public class SAPSaudaAmendmentDto
    {
        public string SaudaNumber { get; set; }
        public long SaudaOrderId { get; set; }
        public string SkuCode { get; set; }
        public decimal Quantity { get; set; }
        public string DepotCode { get; set; }
        public string INCO1 { get; set; }
        public string INCO2 { get; set; }
        public DateTime ToDate { get; set; }
        public string SoldToParty { get; set; }
        public string ShipToParty { get; set; }
        public string Payer { get; set; }
        public string BillToParty { get; set; }
        public string Broker { get; set; }       
        public string Uom { get; set; }
        public string Vertical { get; set; }
        public decimal BidAmount { get; set; }
        public decimal Rate1 { get; set; }
        
    }
}
