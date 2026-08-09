using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class PremiumGeography : Auditable
    {
        [Required]
        public long SkuId { get; set; }
        [Required]
        public long OilTypeId { get; set; }

        public decimal ActualPremium { get; set; }
        public long ZoneId { get; set; }
        public long StateId { get; set; }
        public long TerritoryId { get; set; }
        public long DistrictId { get; set; }
        public long CityId { get; set; }       
        
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public long ParentId { get; set; }

        public virtual Sku Sku { get; set; }
        public virtual OilType OilType { get; set; }
        
    }
}
