using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingContractReportInputDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public List<long> PackGroupId { get; set; }
        public List<long> SkuId { get; set; }
        public List<long> StateIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> NationalHeadIds { get; set; }
        public long LoginUserId { get; set; }
        public bool isGroupByBdo { get; set; }
    }

    public class PendingContractExportDto
    {
        [DisplayName("Sauda Number")]
        public string SaudaNumber { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Customer Code")]
        public string CustomerCode { get; set; }
        [DisplayName("Material Code")]
        public string MaterialCode { get; set; }
        [DisplayName("Basic Price")]
        public decimal BasicPrice { get; set; }
        [DisplayName("Total Price")]
        public decimal TotalPrice { get; set; }
        [DisplayName("Contract ValidTo")]
        public String ContractValidTo { get; set; }
        [DisplayName("Sales Organization")]
        public string SalesOrganization { get; set; }
        [DisplayName("Distribution Channel")]
        public string DistributionChannel { get; set; }
        [DisplayName("Division")]
        public string Division { get; set; }
        [DisplayName("Pending Quantity In Case")]
        public decimal PendingQuantityInCase { get; set; }
        [DisplayName("Pending Quantity In MT")]
        public decimal PendingQuantityInMT { get; set; }
        [DisplayName("Open SalesOrder Quantity")]
        public decimal OpenSalesOrderQuantity { get; set; }
        [DisplayName("Creaated Date")]
        public string CreatedDate { get; set; }
    }
    public class PendingContractReportSaudaOrderContextDto
    {
        public long UserId { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal BidQuantity { get; set; }
        public long Id { get; set; }
        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public decimal BidQuantityCase { get; set; }
        public string SkuName { get; set; }
        public long BdoId { get; set; }
        public string BdoName { get; set; }
        public decimal Rate { get; set; }
    }

    public class PendingContractstDto
    {
        public long Id { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        public  DateTime  RecordCreatedDate { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string SalesOrgDescription { get; set; }
        public string ContractValidFrom { get; set; }
        public DateTime ContractValidTo { get; set; }
        public string PONumber { get; set; }
        public string SaudaDate { get; set; }
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
        public string TermsOfPaymentKey { get; set; }
        public string UsegeIndicator { get; set; }
        public DateTime  CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string PackGroup { get; set; }
        public string Validity { get; set; }
        public long AgingByDays { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal OpenSalesOrderQuantity { get; set; }

}

}
