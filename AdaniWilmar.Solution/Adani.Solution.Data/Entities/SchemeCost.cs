using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class SchemeCost : Auditable
    {
        public long? DivisionId { get; set; }
        public long OilTypeId { get; set; }        

        public long ZoneId { get; set; }
        public int StateId { get; set; }
        public int? TerritoryId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public long PackGroupId { get; set; }
        public long SkuId { get; set; }

        public decimal RatePerMt { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
               
        public virtual Division Division { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual State State { get; set; }
        public virtual Territory Territory { get; set; }
        public virtual District District { get; set; }
        public virtual City City { get; set; }
        public virtual PackGroup PackGroup { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
