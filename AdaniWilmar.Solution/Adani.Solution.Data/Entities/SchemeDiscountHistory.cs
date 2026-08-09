using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace Adani.Solution.Data.Entities
{
   public class SchemeDiscountHistory : Auditable
    {
        public string Name { get; set; }
        public long DiscountId { get; set; }
        public long DiscountType { get; set; }
        public decimal Discount { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
    }
}
