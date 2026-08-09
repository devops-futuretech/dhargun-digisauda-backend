using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ClaimDto
    {
        public int ClaimId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
    }

    public class RoleTypeDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPrime { get; set; }
        public int LevelNo { get; set; }
        public string Description { get; set; }
    }

    public class RoleTypeClaimDto : LoginUserIdDto
    {
        public RoleTypeDto RoleType { get; set; }
        public IList<int> ClaimIds { get; set; }

        public RoleTypeClaimDto()
        {
            ClaimIds = new List<int>();
            RoleType = new RoleTypeDto();
        }
    }

    public class RoleClaimDto : LoginUserIdDto
    {
        public RoleDto Role { get; set; }
        public IList<int> ClaimIds { get; set; }

        public RoleClaimDto()
        {
            ClaimIds = new List<int>();
        }
    }

    public class RoleTypeUsersDto
    {
        public int RoleTypeId { get; set; }
        public bool IsActiveUsersonly { get; set; } = false;
    }

    public class RoleClaimUpdateDto : LoginUserIdDto
    {
        //Item1-RoleId, Item2-RoleName,Item3-RoleIsDeleted, Item4-ClaimIds
        public IList<Tuple<long, string, bool, List<int>>> RoleClaimIds { get; set; }

        public RoleClaimUpdateDto()
        {
            RoleClaimIds = new List<Tuple<long, string, bool, List<int>>>();
        }
    }

    public class RoleTypeClaimUpdateDto : LoginUserIdDto
    {
        //Item1-RoleTypeId, Item2-RoleTypeName,Item3-RoleTypeIsDeleted, Item4-ClaimIds
        public IList<Tuple<long, string, bool, List<int>>> RoleTypeClaimIds { get; set; }

        public RoleTypeClaimUpdateDto()
        {
            RoleTypeClaimIds = new List<Tuple<long, string, bool, List<int>>>();
        }
    }

    public class RoleTypeHierarchyDto
    {
        public ICollection<KeyValuePair<int, int>> RoleTpyeHierarchyNo { get; set; }

        public RoleTypeHierarchyDto()
        {
            RoleTpyeHierarchyNo = new List<KeyValuePair<int, int>>();
        }

    }

    public class RoleTypeIdDto : LoginUserIdDto
    {
        public long RoleTypeId { get; set; }
    }        
}
