using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Headquarters : Auditable
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public long ZoneId { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }

        public virtual Zone Zone { get; set; }
        public virtual State State { get; set; }
        public virtual Territory Territory { get; set; }
        public virtual District District { get; set; }
        public virtual City City { get; set; }
    }
}
