using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class City : Entity
    {
        [Required, MaxLength(150)]
        public string CityName { get; set; }

        [Required]
        public int DistrictId { get; set; }

        

        public int? SortOrder { get; set; }

        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
    }
}
