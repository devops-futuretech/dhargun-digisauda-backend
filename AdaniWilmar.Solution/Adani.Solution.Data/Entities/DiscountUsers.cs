using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class DiscountUsers : Auditable
    {
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        [Required]
        public long OilTypeId { get; set; }
        [Required]
        public long SkuId { get; set; }
        [Required]
        public long UserId { get; set; }


        public decimal ActualDiscount { get; set; }
        public decimal RequestedDiscount { get; set; }
        public string DiscountReason { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public bool Status { get; set; }
        public long ApprovedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public long ParentId { get; set; }
        public long ParentDiscountId { get; set; }
        public long? StateId { get; set; }

        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual User User { get; set; }
    }
}
