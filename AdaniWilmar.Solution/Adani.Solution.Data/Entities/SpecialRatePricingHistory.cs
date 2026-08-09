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
    public class SpecialRatePricingHistory : Auditable
    {
        public string SAPPricingCode { get; set; }
        public long SkuId { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public long OilPackingTypeId { get; set; }
        public long PlantId { get; set; }
        public string PlantCode { get; set; }
        //public long DepotId { get; set; }
        public string DepotCode { get; set; }
        public decimal Price { get; set; }
        public string SalesOrganization { get; set; }
        public long SalesOrganizationId { get; set; }
        public string DistributionChannel { get; set; }
        public long DistributionChannelId { get; set; }
        public string Division { get; set; }
        public long DivisionId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long PricingReferneceId { get; set; }

        public decimal PerUnit { get; set; }
    }
}
