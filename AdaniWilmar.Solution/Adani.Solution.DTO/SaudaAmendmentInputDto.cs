using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaAmendmentInputDto
    {
        public long SaudaId { get; set; }
        public long DealerId { get; set; }
        public long DepotId { get; set; }
        public long IncotermId { get; set; }
        public DateTime ToDate { get; set; }
        public SaudaAmedmantOrdersInputDto saudaAmedmantOrdersInputDto { get; set; }
        public SaudaAmendmentInputDto()
        {
            saudaAmedmantOrdersInputDto = new SaudaAmedmantOrdersInputDto();
        }
    }

    public class SaudaAmedmantOrdersInputDto
    {
        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public decimal QuotedPrice { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidQuantity { get; set; }
    }
}
