using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class OilTransferCost : Auditable
    {
        public long OilTypeId { get; set; }
        public long SourceId { get; set; }
        public long DestinationId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long? SkuId { get; set; }
        public decimal RatePerMt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }        


        public virtual Depot Source { get; set; }
        public virtual Depot Destination { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
