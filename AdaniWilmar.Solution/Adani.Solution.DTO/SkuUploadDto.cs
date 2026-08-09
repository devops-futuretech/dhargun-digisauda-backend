using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SkuUploadDto: CommonResultDto
    {
        //SkuName and Code
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string OilTypeName { get; set; }

        //PackType - Tin, Jar
        public string PackType { get; set; }

        //Uom - Ltr,Kg
        public string PackSize { get; set; }
        public decimal PackSizeQuantity { get; set; }

        //OilPackingType - BP/CP
        public string PackGroup { get; set; }
        public string OilPackGroupType { get; set; }
        public decimal ProcessCost { get; set; }
        public string IsActive { get; set; }

        //Conversion1_UomId
        public decimal UOM1_No { get; set; }
        //Conversion2_UomId
        public decimal Uom2_CaseToNumberConversion { get; set; }
        //Conversion3_UomId
        public decimal Uom3_MetricTonToNumberConversion { get; set; }
        //Gold..
        public decimal ConversionFactor1 { get; set; }
        public decimal ConversionFactor2 { get; set; }
        public string UOM { get; set; }
        public string OilTypeCode { get; set; }
        public string RelationalUOM { get; set; }
        public string BusinessLine { get; set; }
        public string ParentMaterialCode { get; set; }
        public string SubCategory { get; set; }
        public int SapStatusId { get; set; }
        public long CreatedBy { get; set; }
        public string MaterialTypeName { get; set; }
        public string SalesDocumentType { get; set; }
        public string IsRequiredToAttachTradeTicket { get; set; }
        public string DocumentType { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal PremiumAmount { get; set; }
        public string StorageLocation { get; set; }

        public string DiscountAutomationConversionUom { get; set; }
        public decimal DiscountAutomationConversionFactor1 { get; set; }
        public decimal DiscountAutomationConversionFactor2 { get; set; }
        public string DiscountAutomationConversionRelationalUom { get; set; }

    }

    public class MaterialTypeUploadDto : CommonResultDto
    {
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string IsActive { get; set; }
        public string MaterialType { get; set; }
        public long CreatedBy { get; set; }
    }

    public class PackGroupTypeMapping : CommonResultDto
    {
        public long Id { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string SkuCode { get; set; }
        public long? PackGroupTypeId { get; set; }
        public string PackGroupType { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
