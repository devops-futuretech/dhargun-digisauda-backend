using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class RoleTypeClaimUpdateViewModel
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<RoleTypeUpdateViewModel> RoleTypeUpdate { get; set; }
        public SystemRoleTypeClaimsDto SystemRoleTypeClaimsDto { get; set; }
        public bool AllChk { get; set; }

        public long RoleTypeId { get; set; }
        public string RoleTypeName  { get; set; }
        public string SearchText { get; set; }
        public List<int> ClaimIds { get; set; }
        public bool IsActive { get; set; }
        public bool IsSearch { get; set; }
        public RoleTypeClaimUpdateViewModel()
        {
            RoleTypeUpdate = new List<RoleTypeUpdateViewModel>();
        }
    }
}