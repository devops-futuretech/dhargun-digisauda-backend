using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RoleClaimViewDto
    {
        public IList<ClaimDto> SystemClaims { get; set; }
        public IList<RoleClaimRoleTypeClaimViewDto> RoleClaimsAndRoleTypeClaims { get; set; }

        public RoleClaimViewDto()
        {
            SystemClaims = new List<ClaimDto>();
            RoleClaimsAndRoleTypeClaims = new List<RoleClaimRoleTypeClaimViewDto>();
        }
    }

    public class RoleClaimRoleTypeClaimViewDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<ClaimDto> RoleClaims { get; set; }

        public long RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public IList<ClaimDto> RoleTypeClaims { get; set; }

        public RoleClaimRoleTypeClaimViewDto()
        {
            RoleClaims = new List<ClaimDto>();
            RoleTypeClaims = new List<ClaimDto>();
        }
    }
}
