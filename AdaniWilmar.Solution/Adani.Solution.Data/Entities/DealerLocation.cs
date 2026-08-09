using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class DealerLocation : Auditable
    {
        [Required]
        public long UserId { get; set; }
        public int? StateId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public bool IsSAPData { get; set; }
        
        public virtual User User { get; set; }
    }
}
