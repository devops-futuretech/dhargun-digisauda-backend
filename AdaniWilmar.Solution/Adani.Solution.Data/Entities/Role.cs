using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Role : Auditable
    {
        public Role()
        {
            this.RoleClaims = new HashSet<RoleClaim>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        public bool IsPrime { get; set; } = false;

        public bool IsActive { get; set; } = true;

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public long RoleTypeId { get; set; }

        public virtual RoleType RoleType { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }

    }
}
