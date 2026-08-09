using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class PremiumUser : Auditable
    {
        [Required]
        public long SkuId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long OilTypeId { get; set; }

        public decimal ActualPremium { get; set; }

        public decimal RequestedPremium { get; set; }

        public long ParentPremiumId { get; set; }

        public long ParentId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public virtual OilType OilType { get; set; }

        public virtual Sku Sku { get; set; }

        public virtual User User { get; set; }
    }
}
