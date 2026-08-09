using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionInputDto
    {
        public long LoginUserId { get; set; }
        public long VerticalId { get; set; }

        public List<long> BdoIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> NationalHeadIds { get; set; }
        //public int PageNo { get; set; }
    }
}
