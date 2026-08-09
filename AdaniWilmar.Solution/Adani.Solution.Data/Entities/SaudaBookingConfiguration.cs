using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class SaudaBookingConfiguration : Auditable
    {
        public long RoleId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool IsActive { get; set; }
        public string OilTypeIds { get; set; }
        public string UserIds { get; set; }
    }
}
