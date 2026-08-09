using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class SpecialtyFatQuantityRequest : Auditable
    {
       
        [Required]
        public long SkuId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal Quantity { get; set; }

        public long StatusId { get; set; }
        public long OilTypeId { get; set; }
        public long SpecialtyFatQuantityLimitId { get; set; }
        public string Remarks { get; set; }
        public long DivisionId { get; set; }

        public virtual Sku Sku { get; set; }        
        public virtual Status Status { get; set; }
        public virtual OilType OilType { get; set; }
    }
}
