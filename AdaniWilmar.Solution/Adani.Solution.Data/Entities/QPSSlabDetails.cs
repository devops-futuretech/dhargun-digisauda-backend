using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class QPSSlabDetails : Auditable
    {
        [Required]
        public long QpsDiscountId { get; set; }
      
        public int FromRange { get; set; }
        
        public int ToRange { get; set; }
        
        public decimal Discount { get; set; }
        
        public virtual QpsDiscount QpsDiscount { get; set; }
    }
}
