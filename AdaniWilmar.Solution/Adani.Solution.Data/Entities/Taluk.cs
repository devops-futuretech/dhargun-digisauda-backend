using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Taluk : Entity
    {
        [Required, MaxLength(150)]
        public string TalukName { get; set; }

        [MaxLength(150)]
        public string TamilName { get; set; }

        public int? SortOrder { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
