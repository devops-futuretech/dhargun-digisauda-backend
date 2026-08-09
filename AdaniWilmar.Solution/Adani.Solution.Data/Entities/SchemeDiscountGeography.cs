using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SchemeDiscountGeography : Auditable
    {
        public string Name { get; set; }
        public decimal Discount { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public bool IsEdited { get; set; }

        public decimal TargetQuantity { get; set; }
        public string DiscountReason { get; set; }
    }
}
