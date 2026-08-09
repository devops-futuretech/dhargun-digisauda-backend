using Adani.Solution.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
   public class SpecalityFatDiscountUser : Auditable
    {
        [Required]
        public long SkuId { get; set; }

        [Required]
        public long UserId { get; set; }
        
        [Required]
        public long OilTypeId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal ActualDiscount { get; set; }

        public decimal RequestedDiscount { get; set; }
                
        public long ApprovedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public long ParentId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal RemainingQuantity { get; set; }

        public long ParentQuantityId { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public DateTime? RequestedDiscountDate { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual User User { get; set; }        
        
    }
}
