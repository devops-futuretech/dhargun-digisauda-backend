using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SystemRoleTypeClaimsDto
    {
        public IList<ClaimDto> SystemClaims { get; set; }

        public IList<RoleTypeClaimOutputDto> SystemRoleTypes { get; set; }

        public SystemRoleTypeClaimsDto()
        {
            SystemClaims = new List<ClaimDto>();
            SystemRoleTypes = new List<RoleTypeClaimOutputDto>();
        }
    }

    public class RoleTypeClaimOutputDto
    {
        public long RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public bool IsPrime { get; set; }
        public IList<ClaimDto> Claims { get; set; }

        public RoleTypeClaimOutputDto()
        {
            Claims = new List<ClaimDto>();
        }
    }
}
