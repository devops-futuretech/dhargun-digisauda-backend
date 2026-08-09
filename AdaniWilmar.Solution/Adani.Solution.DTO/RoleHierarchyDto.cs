using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RoleHierarchyDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPrime { get; set; }
        public int LevelNo { get; set; }
        public string Description { get; set; }
        

        public ICollection<KeyValuePair<int, int>> RoleHierarchyNo { get; set; }

        public RoleHierarchyDto()
        {
            RoleHierarchyNo = new List<KeyValuePair<int, int>>();
        }
    }

    public class RoleHierarchyParamDto
    {
        public long VerticalId { get; set; }
        public long ProcessId { get; set; }
    }
}
