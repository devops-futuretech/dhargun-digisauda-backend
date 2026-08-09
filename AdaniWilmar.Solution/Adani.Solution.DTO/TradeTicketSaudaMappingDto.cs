using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketSaudaMappingDto: IAPIInputDTO
    {
        public long TradeTicketId { get; set; }
       
        public List<long> StateId { get; set; }
        public List<long> DealerIds { get; set; }
        public decimal OpenQuantity { get; set; }
        public decimal SaudaQuantity { get; set; }
        public decimal SaudaTotalQuantity { get; set; }
        public string PlantName { get; set; }
        public decimal RatePerMT { get; set; }
        public string TradeTicketOilTypes { get; set; }
        public string MaterialType { get; set; }
        public long DepotId { get; set; }
        public DateTime SAPCreationDate { get; set; }

        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        public List<SaudaOrderViewDto> SaudaOrderViewDtoList { get; set; }
    }
}
