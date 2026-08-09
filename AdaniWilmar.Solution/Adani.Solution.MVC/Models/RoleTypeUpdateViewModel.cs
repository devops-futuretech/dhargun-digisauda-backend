using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class RoleTypeUpdateViewModel
    {
        public bool IsDelete { get; set; }
        public long RoleTypeId { get; set; }
        public long RoleId { get; set; }
        public string RoleTypeName { get; set; }
        public List<RoleTypeViewModel> ClaimDto { get; set; }

        public RoleTypeUpdateViewModel()
        {
            ClaimDto = new List<RoleTypeViewModel>();
        }
    }
}