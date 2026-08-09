using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class KeyPerformanceIndicator : Auditable
    {
        public long RoleId { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
