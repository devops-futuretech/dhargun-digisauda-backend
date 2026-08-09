using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class QPSDiscountSkuMappingDto
    {
        public long QPsDisId { get; set; }
        public List<long> SkuIds { get; set; }
        public List<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
    }
}
