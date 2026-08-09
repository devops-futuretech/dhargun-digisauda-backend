using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SkuDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        //Product
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        //Branch
        public long? DepotId { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }

        //Brand
        public long? OilTypeId { get; set; }
        public string OilType { get; set; }
        public string OilTypeCode { get; set; }

        public long? OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }
        public long? OilPackingBPCPTypeId { get; set; }
        public string OilPackingBPCPType { get; set; }

        public long? OilPackGroupTypeId { get; set; }
        public string OilPackGroupType { get; set; }

        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public string VerticalCode { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }

        public long? UOM1_No { get; set; }

        public long? SubCategoryId { get; set; }
        public string SubCategory { get; set; }

        //public string BaseUOM { get; set; }
        //public string UOM1 { get; set; }
        //public string UOM2 { get; set; }
        //public string UOM3 { get; set; }
        //public string Conversion1 { get; set; }
        //public string Conversion2 { get; set; }

        public long PackTypeId { get; set; }
        public string TDAndPacktype { get; set; }

        public long? QuantityTypeUomId { get; set; }
        public long? QuantityTypeUom { get; set; }
        public decimal Quantity { get; set; }
        //public decimal ProcessCost { get; set; }

        public long UomMappingId1 { get; set; }
        public long Conversion1_UomId { get; set; }
        public string Conversion1_Uom { get; set; }
        public long Conversion1_RelationUomId { get; set; }
        public string Conversion1_RelationUom { get; set; }

        public long UomMappingId2 { get; set; }
        public long Conversion2_UomId { get; set; }
        public string Conversion2_Uom { get; set; }
        public long Conversion2_RelationUomId { get; set; }
        public string Conversion2_RelationUom { get; set; }

        public long UomMappingId3 { get; set; }
        public long Conversion3_UomId { get; set; }
        public string Conversion3_Uom { get; set; }
        public long Conversion3_RelationUomId { get; set; }
        public string Conversion3_RelationUom { get; set; }

        public decimal ConversionFactor1 { get; set; }
        public decimal ConversionFactor2 { get; set; }
        public decimal ConversionFactor3 { get; set; }

        public bool IsActive { get; set; }
        //public bool IsRequiredToAttachTT { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        public bool IsChecked { get; set; }
        public string NewlyAdded { get; set; }

        public int? MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; }

        //public bool IsBaseSku { get; set; }
        //public string DocumentType { get; set; }
        public decimal GrossWeight { get; set; }
        //public decimal? PremiumAmount { get; set; }
        public string StorageLocation { get; set; }
        public string BusinessLine { get; set; }
        public string ParentMaterialCode { get; set; }
        //public long SalesDocumentTypeId { get; set; }
        //public string SalesDocumentType { get; set; }
        public string UOMName { get; set; }
        public List<long> LineId { get; set; }
        public string LineName { get; set; }

        public long? DiscountAutomationConversion_UomId { get; set; }
        public decimal? DiscountAutomationConversionFactor1 { get; set; }
        public decimal? DiscountAutomationConversionFactor2 { get; set; }
        public long? DiscountAutomationConversion_RelationalUomId { get; set; }
    }

    public class SkuDropDownInputDto : LoginUserIdDto
    {
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public long SubCategoryId { get; set; }
        public string SearchText { get; set; }
    }

    public class OilTypeNameDto : IAPIInputDTO
    {
        public bool IsRasoi { get; set; }

        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }

    public class ConvertCasetoMetricTonSku
    {
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public long? UomId { get; set; }
        public decimal LitreConversion { get; set; }
    }

    public class ConvertCasetoMetricTonSkuUom
    {
        public long Id { get; set; }
        public decimal ConversionFactor { get; set; }
        public long SkuId { get; set; }
        public long UomId { get; set; }
        public long RelationUomId { get; set; }
    }

    public class SkuOutputListDto
    {
        public long Id { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
    }

    public class ConvertCasetoMetricTon
    {
        public decimal Quantity { get; set; }
        public long SkuId { get; set; }
    }

    public class SaudaMofificationFromSkuInfoDto
    {
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public bool IsToReturnInactiveData { get; set; }
    }

    public class SaudaMofificationFromSkuDetailsDto
    {
        public long OilTypeId { get; set; }
        public long OilPackGroupTypeId { get; set; }
        public string SaudaNumber { get; set; }
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
    }
}
