using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class RoleTypeClaim : Auditable
    {
        [Required]
        public long RoleTypeId { get; set; }

        [Required]
        public int ClaimId { get; set; }

        public virtual RoleType RoleType { get; set; }
        public virtual Claim Claim { get; set; }
    }
}
