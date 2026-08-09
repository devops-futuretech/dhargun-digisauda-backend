using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseDiscountDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SkuId { get; set; }
        public string Sku { get; set; }
        public long CustomerId { get; set; }
        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; } = DateTime.Now;
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
