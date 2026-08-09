using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    [Table("TodayPricingBackups")]
    public class TodayPricingBackup : Auditable
    {
        public string SAPPricingCode { get; set; }
        public long SkuId { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public long OilPackingTypeId { get; set; }
        public long PlantId { get; set; }
        public string PlantCode { get; set; }
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

