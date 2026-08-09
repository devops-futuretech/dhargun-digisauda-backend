using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Competitor : Auditable
    {
        [Required]
        public string Name { get; set; }
        public long ZoneId { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public int TerritoryId { get; set; }
        [MaxLength(4000)]
        public string Address { get; set; }
        [MaxLength(10)]
        public string Pincode { get; set; }
        public bool IsActive { get; set; }

        public virtual Zone Zone { get; set; }
        public virtual State State { get; set; }
        //public virtual District District { get; set; }
        //public virtual City City { get; set; }
        //public virtual Territory Territory { get; set; }
    }
}
