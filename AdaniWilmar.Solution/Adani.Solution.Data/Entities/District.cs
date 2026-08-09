using System;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class District : Entity
    {
        [Required, MaxLength(150)]
        public string DistrictName { get; set; }

        [Required]
        public int StateId { get; set; }
       
        public int? SortOrder { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual State State { get; set; }        
    }
}
