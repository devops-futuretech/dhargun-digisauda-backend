using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class Sku : Auditable
    {
        [Required, MaxLength(150)]
        public string SkuName { get; set; }
        [MaxLength(150)]
        public string SkuCode { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long? OilTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequiredToAttachTT { get; set; }
        public decimal ProcessCost { get; set; }
        public decimal Quantity { get; set; }
        public long PackTypeId { get; set; }
        public long? PackGroupId { get; set; }
        public long? OilPackGroupTypeId { get; set; }
        public long DivisionGroupId { get; set; }
        public long? UomId { get; set; }
        public long? SubCategoryId { get; set; }
        public int SapStatusId { get; set; }
        public decimal LitreConversion { get; set; }
        //public int? MaterialTypeId { get; set; }
        public bool IsSAPData { get; set; }
        public bool IsSAPDataSyncOrNot { get; set; }
        public bool IsBaseSku { get; set; }
        [DecimalPrecision(18, 8)]
        public decimal GrossWeight { get; set; }
        public decimal? PremiumAmount { get; set; }
        public string StorageLocation { get; set; }
        public string BusinessLine { get; set; }
        public string ParentMaterialCode { get; set; }
        public long? QuantityTypeUom { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
        public virtual PackType PackType { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual PackGroup PackGroup { get; set; }
        public virtual Uom Uom { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        //public virtual MaterialType MaterialType { get; set; }
        public string LineId { get; set; }

        public long? DiscountAutomationConversionUomId { get; set; }
        public long? DiscountAutomationConversionRelationUomId { get; set; }
        public decimal? DiscountAutomationConversionFactor1 { get; set; }
        public decimal? DiscountAutomationConversionFactor2 { get; set; }
    }
}
