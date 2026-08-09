using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class RoleType: Auditable
    {
        public RoleType()
        {
            this.RoleTypeClaims = new HashSet<RoleTypeClaim>();
            this.Roles = new HashSet<Role>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public int HierarchyId { get; set; }

        [Required]
        public bool IsPrime { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<RoleTypeClaim> RoleTypeClaims { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
