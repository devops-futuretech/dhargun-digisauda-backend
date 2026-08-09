using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserDepotMapping : Auditable
    {
        [Required]
        public long UserId { get; set; }
        public long DepotId { get; set; }
        public bool IsSAPData { get; set; }
        public virtual User User { get; set; }
        public virtual Depot Depot { get; set; }
    }
}
