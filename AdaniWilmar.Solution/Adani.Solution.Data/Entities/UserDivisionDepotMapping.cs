using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserDivisionDepotMapping : Auditable
    {
        [Required]
        public long UserDivisionId { get; set; }

        public long DepotId { get; set; }

        public virtual UserDivisionMapping UserDivision { get; set; }
        public virtual Depot Depot { get; set; }
    }
}
