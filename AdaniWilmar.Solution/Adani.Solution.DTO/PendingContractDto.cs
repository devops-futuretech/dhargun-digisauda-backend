using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingContractListDto
    {
        public List<HANAPendingContractDto> PendingContracts { get; set; }
        public PendingContractListDto()
        {
            PendingContracts = new List<HANAPendingContractDto>();
        }
    }
    public class HANAPendingContractDto
    {
        public string SaudaNumber { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        public DateTime? RecordCreatedDate { get; set; }
        public string SalesOrganization { get; set; }
        public string SalesOrgDescription { get; set; }
        public DateTime? ContractValidFrom { get; set; }
        public DateTime? ContractValidTo { get; set; }
        public string PONumber { get; set; }
        public DateTime? SaudaDate { get; set; }
        public string IncoTerms1 { get; set; }
        public string Tax { get; set; }
        public string BrokerCode { get; set; }
        public string Place { get; set; }
        public string BrokerName { get; set; }
        public string BrokerCity { get; set; }
        public string BrokerRegionDescription { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerRegionDescription { get; set; }
        public string CustomerRegionalMarket { get; set; }
        public string MaterialCode { get; set; }
        public string CustomerMaterialCode { get; set; }
        public string MaterialGroup { get; set; }
        public string MaterialDescription1 { get; set; }
        public string MaterialDescription2 { get; set; }
        public string Location { get; set; }
        public decimal BasicRate { get; set; }
        public decimal Discount { get; set; }
        public decimal BasicRateAfterDiscount { get; set; }
        public string PR00 { get; set; }
        public string ConditionType { get; set; }
        public string ZDC1 { get; set; }
        public string ZDC2 { get; set; }
        public string FRC1 { get; set; }
        public string FRC2 { get; set; }
        public string JINSVALUE { get; set; }
        public decimal DespatchQty { get; set; }
        public decimal PendingQuantityInMT { get; set; }
        public decimal PendingQuantityInCase { get; set; }
        public decimal SaudaQuantity { get; set; }
        public string UOM { get; set; }
        public string ContractType { get; set; }
        public string PartnerFunction { get; set; }
        public string Description { get; set; }
        public string ReleaseStatus { get; set; }
        public string UsageDescription { get; set; }
        public string CreatedPerson { get; set; }
        public string UsegeIndicator { get; set; }
        public string TermsOfPaymentKey { get; set; }
        public int BatchNo { get; set; }
        public string Validity { get; set; }
        public long AgingByDays { get; set; }
    }

    public class PendingContractDto
    {
        public string SaudaNumber { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        public DateTime? RecordCreatedDate { get; set; }
        public string SalesOrganization { get; set; }
        public string SalesOrgDescription { get; set; }
        public DateTime? ContractValidFrom { get; set; }
        public DateTime? ContractValidTo { get; set; }
        public string PONumber { get; set; }
        public DateTime? SaudaDate { get; set; }
        public string IncoTerms1 { get; set; }
        public string Tax { get; set; }
        public string BrokerCode { get; set; }
        public string Place { get; set; }
        public string BrokerName { get; set; }
        public string BrokerCity { get; set; }
        public string BrokerRegionDescription { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerRegionDescription { get; set; }
        public string CustomerRegionalMarket { get; set; }
        public string MaterialCode { get; set; }
        public string CustomerMaterialCode { get; set; }
        public string MaterialGroup { get; set; }
        public string MaterialDescription1 { get; set; }
        public string MaterialDescription2 { get; set; }
        public string Location { get; set; }
        public decimal BasicRate { get; set; }
        public decimal Discount { get; set; }
        public decimal BasicRateAfterDiscount { get; set; }
        public string PR00 { get; set; }
        public string ConditionType { get; set; }
        public string ZDC1 { get; set; }
        public string ZDC2 { get; set; }
        public string ZPU1 { get; set; }
        public string ZPU2 { get; set; }
        public string FRC1 { get; set; }
        public string FRC2 { get; set; }
        public string JINSVALUE { get; set; }
        public decimal DespatchQty { get; set; }
        public decimal PendingQuantityInMT { get; set; }
        public decimal PendingQuantityInCase { get; set; }
        public decimal SaudaQuantity { get; set; }
        public string UOM { get; set; }
        public string ContractType { get; set; }
        public string PartnerFunction { get; set; }
        public string Description { get; set; }
        public string ReleaseStatus { get; set; }
        public string MaterialGroup1 { get; set; }
        public string MaterialGroup2 { get; set; }
        public string MaterialGroup3 { get; set; }
        public string MaterialGroup4 { get; set; }
        public string MaterialGroup5 { get; set; }
        public string MaterialGroupDescription1 { get; set; }
        public string MaterialGroupDescription2 { get; set; }
        public string MaterialGroupDescription3 { get; set; }
        public string MaterialGroupDescription4 { get; set; }
        public string MaterialGroupDescription5 { get; set; }
        public string UsageDescription { get; set; }
        public string CreatedPerson { get; set; }
        public string UsegeIndicator { get; set; }
        public string TermsOfPaymentKey { get; set; }
    }

    public class PendingContractComparisionOutputDto
    {
        public string SAPDealerCode { get; set; }
        public string SAPDealerName { get; set; }
        public string SAPBrokerCode { get; set; }
        public string SAPContractNumber { get; set; }
        public string SAPContractDate { get; set; }
        public string SAPMaterialCode { get; set; }
        public string SAPMaterialDescription { get; set; }
        public string SAPOilType { get; set; }
        public string SAPContractQuantity { get; set; }
        public string SAPDespatchQuantity { get; set; }
        public string SAPPendingQuantity { get; set; }
        public string SAPPendingQuantityMT { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string BrokerCode { get; set; }
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string OilType { get; set; }
        public string ContractQuantity { get; set; }
        public string DespatchQuantity { get; set; }
        public string PendingQuantity { get; set; }
        public string PendingQuantityMT { get; set; }
        public string Status { get; set; }
        public string ActionToTaken { get; set; }

    }

    public class PendingContractMaterialInfoDTO
    {
        public string MaterialCode { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
        public decimal BasicRate { get; set; }
        public decimal PendingQuantityInCase { get; set; }
        public decimal SaudaQuantity { get; set; }

    }

    public class PendingContractDetails
    {
        public List<PendingContractOilTypeDetails> OilTypes { get; set; }
    }

    public class PendingContractOilTypeDetails
    {
        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public List<PendingContractPackTypeDetails> PackTypes { get; set; }
    }

    public class PendingContractPackTypeDetails
    {
        public long? PackTypeId { get; set; }
        public string PackTypeName { get; set; }

        public decimal OriginalMT { get; set; }
        public decimal ModifiedMT { get; set; }
        public decimal DifferenceMT { get; set; }

        public List<PendingContractSkuDetails> Skus { get; set; }
    }

    public class PendingContractSkuDetails
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal BasicRate { get; set; }
        public decimal PendingQuantityInCase { get; set; }
        public decimal SaudaQuantity { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
    }

}
