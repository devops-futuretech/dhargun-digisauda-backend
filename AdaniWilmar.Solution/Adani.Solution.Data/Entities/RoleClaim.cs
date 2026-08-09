using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class RoleClaim : Auditable
    {
        [Required]
        public long RoleId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        public virtual Claim Claim { get; set; }
        public virtual Role Role { get; set; }
    }
}
