using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class RoleHierarchy : Auditable
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public int HierarchyId { get; set; }
        //public int ProcessId { get; set; }
        public long RoleId { get; set; }
        [Required]
        public bool IsPrime { get; set; } = false;
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        public bool IsDeleted { get; set; } = false;

        //public long DivisionId { get; set; }

        public virtual ICollection<RoleTypeClaim> RoleTypeClaims { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        public virtual Role Role { get; set; }
        //public virtual Division Division { get; set; }

        public RoleHierarchy()
        {
            this.RoleTypeClaims = new HashSet<RoleTypeClaim>();
            this.Roles = new HashSet<Role>();
        }
    }
}
