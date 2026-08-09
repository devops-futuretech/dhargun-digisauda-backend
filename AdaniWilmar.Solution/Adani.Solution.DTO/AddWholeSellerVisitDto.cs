using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddWholeSellerVisitDto
    {
        public long WholeSellerId { get; set; }
        public string WholeSellerName { get; set; }
        public long DealerId { get; set; }
        public long CreatedBy { get; set; }
        public List<BdoCompetitorAddDto> BdoCompetitorAddDto { get; set; }
        public List<WholeSellerSalesDetailDto> WholeSellerSalesDetailDto { get; set; }
        public AddWholeSellerVisitDto()
        {
            BdoCompetitorAddDto = new List<BdoCompetitorAddDto>();
            WholeSellerSalesDetailDto = new List<WholeSellerSalesDetailDto>();
        }
    }
    public class WholeSellerSalesDetailDto
    {
        public long WholesellerBdoId { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public decimal QuantityPerMt { get; set; }
        public decimal Price { get; set; }
    }
}
