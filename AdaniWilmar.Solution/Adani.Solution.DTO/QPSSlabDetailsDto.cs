using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class QPSSlabDetailsDto 
    {
        public long SlabId { get; set; }
        public string SlabName { get; set; }
        public int FromRange { get; set; }
        public int ToRange { get; set; }
        public decimal Discount { get; set; }
    }
}
