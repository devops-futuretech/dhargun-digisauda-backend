using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealerListAllFilterDto :LoginUserIdDto
    {
        public DealerListAllFilterDto()
        {
            BdoIds = new List<long>();
        }
        public long ZHId { get; set; }
        public List<long> BdoIds { get; set; }
    }
}
