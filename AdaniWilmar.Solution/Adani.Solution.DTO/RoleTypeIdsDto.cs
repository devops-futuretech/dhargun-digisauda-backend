using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RoleTypeIdsDto
    {
        public IList<long> RoleTypeIds { get; set; }
        public RoleTypeIdsDto()
        {
            RoleTypeIds = new List<long>();
        }
    }
}
