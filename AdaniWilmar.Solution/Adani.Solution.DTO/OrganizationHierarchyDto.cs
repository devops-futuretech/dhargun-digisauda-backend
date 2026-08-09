using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class OrganizationHierarchyDto
    {
        //Item1-RoleName, Item2-HierarchyId, Item3-ReportingId,Item4-UserName,Item5-ImageUrl
        public IList<Tuple<string, long, long?, string, string>> OrganizationHierarchy { get; set; }
        public int MaxLevelid { get; set; }
        public OrganizationHierarchyDto()
        {
            OrganizationHierarchy = new List<Tuple<string, long, long?, string, string>>();
        }
    }
}
