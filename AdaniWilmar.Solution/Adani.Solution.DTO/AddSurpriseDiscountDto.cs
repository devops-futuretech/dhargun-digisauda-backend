using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddSurpriseDiscountDto
    {
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long CustomerId { get; set; }
        public long OilPackingTypeId { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long CreatedBy { get; set; }
    }
}
