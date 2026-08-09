using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Claim : Entity
    {
        public Claim()
        {
            this.RoleClaims = new HashSet<RoleClaim>();
            this.RoleTypeClaims = new HashSet<RoleTypeClaim>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        public virtual ICollection<RoleTypeClaim> RoleTypeClaims { get; set; }
    }
}
