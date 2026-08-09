using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Country : Entity
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(3)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string CurrencyName { get; set; }
        public int? SortOrder { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
