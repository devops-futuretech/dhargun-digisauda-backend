using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class RaMargin : Auditable
    {
        public long? DivisionId { get; set; }
        public long OilTypeId { get; set; }
        public long? SkuId { get; set; }
        public long OilPackingTypeId { get; set; }
        public int StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public long ZoneId { get; set; }
        public int? TerritoryId { get; set; }
        public string CustomerCategoryWise { get; set; }       
        public decimal RatePerMt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }

        public virtual Division Division { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual PackGroup OilPackingType { get; set; }
        public virtual State State { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
        public virtual City City { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual Territory Territory { get; set; }
    }
}
