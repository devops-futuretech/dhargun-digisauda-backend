using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class RoleTypeHierarchyViewModel
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<RoleTypeHierarchyModel> RoleTypeDto { get; set; }
    }

    public class RoleTypeHierarchyModel : RoleTypeDto
    {
        public int OrderId { get; set; }
    }
}