using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class RoleClaimUpdateViewModel
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<RoleTypeUpdateViewModel> RoleTypeUpdate { get; set; }
        public RoleClaimViewDto RoleClaimViewDto { get; set; }
        public bool AllChk { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string SearchText { get; set; }
        public List<int> ClaimIds  { get; set; }
        public bool IsSearch { get; set; }

        public RoleClaimUpdateViewModel()
        {
            RoleTypeUpdate = new List<RoleTypeUpdateViewModel>();
        }
    }
}