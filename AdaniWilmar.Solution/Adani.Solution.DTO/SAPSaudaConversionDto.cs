using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASaudaConversionDtoList
    {
        public List<SAPSaudaConversionDto> SaudaConversionList { get; set; }

        public HANASaudaConversionDtoList()
        {
            SaudaConversionList = new List<SAPSaudaConversionDto>();
        }
    }
    public class SAPSaudaConversionDto
    {
        public long SaudaConversionSkusId { get; set; }
        public string SaudaNumber { get; set; }
        public string SkuCode { get; set; }       
        public decimal Quantity { get; set; }
        public decimal BaseRate { get; set; }
        public bool SaudaType { get; set; }
        public bool Status { get; set; }
        public string Remarks { get; set; }
        public string TradeTicketNumber { get; set; }
    }
}
