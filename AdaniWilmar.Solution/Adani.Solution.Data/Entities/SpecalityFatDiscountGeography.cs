using Adani.Solution.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class SpecalityFatDiscountGeography : Auditable
    {
        [Required]
        public long SkuId { get; set; }
        [Required]
        public long OilTypeId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal ActualDiscount { get; set; }

        public long ZoneId { get; set; }
        public long StateId { get; set; }
        public long TerritoryId { get; set; }
        public long DistrictId { get; set; }
        public long CityId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public bool Status { get; set; }
        public long ApprovedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public long ParentId { get; set; }

        public virtual Sku Sku { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual SaudaBookingType SaudaBookingType { get; set; }
    }
}
