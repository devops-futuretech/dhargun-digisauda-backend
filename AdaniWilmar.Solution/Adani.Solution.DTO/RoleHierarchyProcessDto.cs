using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class RoleHierarchyProcessDto
    {
        public ICollection<KeyValuePair<int, int>> RoleHierarchyNo { get; set; }

        public RoleHierarchyProcessDto()
        {
            RoleHierarchyNo = new List<KeyValuePair<int, int>>();
        }

    }
}
