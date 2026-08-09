using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class CustomerTruckCapacityMapping : Auditable
    {
        public long UserId { get; set; }
        public decimal TruckCapacity  { get; set; }
        public long StorageTypeId { get; set; }
    }
}
