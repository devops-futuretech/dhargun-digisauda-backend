using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class QPSDiscountSkuMapping : Auditable
    {
        [Required]
        public long QpsDiscountId { get; set; }

        //public long ZoneId { get; set; }

        public long StateId { get; set; }

        public long SkuId { get; set; }

        public long OilTypeId { get; set; }

        public bool IsActive { get; set; }

        public virtual QpsDiscount QpsDiscount { get; set; }
    }
}
