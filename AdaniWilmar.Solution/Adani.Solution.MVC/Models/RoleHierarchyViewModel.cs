using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class RoleHierarchyViewModel
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public int ProcessId { get; set; }
        public long VerticalId { get; set; }
        public List<RoleHierarchyModel> RoleTypeDto { get; set; }
    }

    public class RoleHierarchyModel : RoleHierarchyDto
    {
        public int OrderId { get; set; }
    }   
}