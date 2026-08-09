using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class PrimaryDiscountSku : Auditable
    {
        [Required]
        public long SkuId { get; set; }

        public decimal ActualDiscount { get; set; }
        public decimal RequestedDiscount { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public bool Status { get; set; }
        public long ApprovedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }


        public virtual Sku Sku { get; set; }
        public virtual SaudaBookingType SaudaBookingType { get; set; }
    }
}
