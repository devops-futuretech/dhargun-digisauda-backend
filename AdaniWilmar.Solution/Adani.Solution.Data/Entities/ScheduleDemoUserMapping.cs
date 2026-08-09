using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class ScheduleDemoUserMapping : Auditable
    {
        [Required]
        public long DemoId { get; set; }
        public long EALUserId { get; set; }
        public virtual ScheduleDemoUser Demo { get; set; }
    }
}
