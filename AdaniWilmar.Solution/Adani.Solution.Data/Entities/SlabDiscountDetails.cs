using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SlabDiscountDetails : Auditable
    {
        [Required]
        public long QPSId { get; set; }

        public string SlabName { get; set; }

        public int FromRange { get; set; }

        public int ToRange { get; set; }

        public decimal DiscountAmount { get; set; }
    }
}
