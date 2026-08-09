using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class LoadCapacityConversion : Auditable
    {
        public long? SalesOrganizationId { get; set; }
        public long? DistributionChannelId { get; set; }
        public long? DivisionId { get; set; }
        public long? OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long TransportModeId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal LoadCapacity { get; set; }  
        
        public decimal LoadQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
                
        public decimal ActualLoadQuantity { get; set; }

        public virtual TransportMode TransportMode { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
