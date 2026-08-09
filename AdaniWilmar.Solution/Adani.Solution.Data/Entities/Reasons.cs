using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Reasons : Auditable
    {
        [Required]
        public string Reason { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
