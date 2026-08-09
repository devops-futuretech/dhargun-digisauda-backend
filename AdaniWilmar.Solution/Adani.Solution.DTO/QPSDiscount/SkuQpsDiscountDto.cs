using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.QPSDiscount
{
    public class SkuQpsInputDto
    {
        public long DealerId { get; set; }
        public List<SkuQpsDiscountDto> SkuDetails { get; set; }
    }

    public class SkuQpsDiscountDto
    {
        public long SkuId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class SkuQpsDiscountResultDto
    {
        public long SkuId { get; set; }
        public string SlabName { get; set; }
        public int FromRange { get; set; }
        public int ToRange { get; set; }
        public decimal Discount { get; set; }
        public long SkuType { get; set; }
        public long QpsDiscountId { get; set; }
    }

    public class MultipleSkuQpsDiscountResultDto
    {
        public long SkuId { get; set; }
        public decimal Discount { get; set; }
        public string QpsId { get; set; }
        public string IndividualQPSDiscount { get; set; }
    }
}
