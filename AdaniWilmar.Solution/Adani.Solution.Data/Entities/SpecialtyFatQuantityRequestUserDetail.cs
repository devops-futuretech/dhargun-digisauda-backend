using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class SpecialtyFatQuantityRequestUserDetail : Auditable
    {
        [Required]
        public long UserId { get; set; }
        public long StatusId { get; set; }
        public long SpecialtyFatQuantityRequestId { get; set; }
        public virtual User User { get; set; }
    }
}
