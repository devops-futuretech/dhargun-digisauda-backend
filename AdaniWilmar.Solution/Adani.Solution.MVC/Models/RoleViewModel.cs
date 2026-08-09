using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class RoleViewModel : RoleDto
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<RoleTypeViewModel> ClaimDto { get; set; }
        public bool AllChk { get; set; }
        public string RoleType { get; set; }
    }
}