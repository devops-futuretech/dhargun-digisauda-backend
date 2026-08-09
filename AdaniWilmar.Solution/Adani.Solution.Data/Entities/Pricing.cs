using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Pricing : Auditable
    {
        public string SAPPricingCode { get; set; }
        [Required]
        public long SkuId { get; set; }

        [Required]
        public long OilTypeId { get; set; }
        public long OilPackingTypeId { get; set; }
        public long PlantId { get; set; }
        public decimal Price { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DivisionId { get; set; }
        public long DistributionChannelId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
