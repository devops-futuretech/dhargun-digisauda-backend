using System;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class State : Entity
    {
        [Required, MaxLength(150)]
        public string StateName { get; set; }

        [Required]
        public int CountryId { get; set; }

        public int? SortOrder { get; set; }

        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public virtual Country Country { get; set; }
    }
}
