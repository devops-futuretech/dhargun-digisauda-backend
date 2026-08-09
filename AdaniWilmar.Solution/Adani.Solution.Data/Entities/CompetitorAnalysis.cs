using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class CompetitorAnalysis : Auditable
    {
        public long SkuId { get; set; }
        public long? OilTypeId { get; set; }
        public long? StatusId { get; set; }
        public decimal Margin { get; set; }
        public decimal EmamiPrice { get; set; }
        [MaxLength(4000)]
        public string Remarks { get; set; }
        public long WorkableQuantity { get; set; }
        public decimal WorkablePrice { get; set; }

        public virtual Sku Sku { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Status Status { get; set; }
    }
}
