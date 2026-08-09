using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class ZoneStateMapping : Auditable
    {
        [Required]
        public long ZoneId { get; set; }

        [Required]
        public int StateId { get; set; }

        public virtual Zone Zone { get; set; }
        public virtual State State { get; set; }
    }
}
