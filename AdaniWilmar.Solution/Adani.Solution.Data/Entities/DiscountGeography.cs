using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class DiscountGeography : Auditable
    {
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        [Required]
        public long OilTypeId { get; set; }
        [Required]
        public long PackGroupId { get; set; }
        [Required]
        public long PackTypeId { get; set; }
        [Required]
        public long SkuId { get; set; }
        public decimal ActualDiscount { get; set; }
        public string DiscountReason { get; set; }
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
        public bool IsActive { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }        
        public virtual DistributionChannel DistributionChannel { get; set; }        
        public virtual Division Division { get; set; }        
        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
