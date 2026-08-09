using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class MaterialCost : Auditable
    {
        public long PlantId { get; set; }
        public long? SalesOrganizationId { get; set; }
        public long? DistributionChannelId { get; set; }
        public long? DivisionId { get; set; }
        public long OilTypeId { get; set; }
        public decimal RatePerMt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }

        public virtual Depot Plant { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
    }
}
