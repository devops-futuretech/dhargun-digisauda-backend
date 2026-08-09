using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ReportDto { }

    public class SaudaOrderListOutputDto
    {
        public int ListCount { get; set; }
        public List<SaudaOrderReportOutputDto> SaudaOrderReports { get; set; }

    }
    public class SaudaOrderReportInputputDto
    {
        public List<long> StateIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string StateId { get; set; }
        public long VerticalId { get; set; }
        public long LoginUserId { get; set; }
        public long RoleId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public int SaudaBookingTypeId { get; set; }
        public List<long> BDOIds { get; set; }
        public int PackTypeId { get; set; }
        public int StatusId { get; set; }
        public List<long> StatusIds { get; set; }
        public int PageNo { get; set; }
    }

    public class SaudaOrderReportOutputDto
    {
        //String
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string FreightRoute { get; set; }
        public string BrokerCode { get; set; }
        public string BrokerName { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string Incoterms { get; set; }
        public string SaudaNumber { get; set; }
        public string PackGroup { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public string State { get; set; }
        public string PlantName { get; set; }
        public string PlantCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Vertical { get; set; }
        public string SaudaBookingType { get; set; }
        public string UOM { get; set; }

        //Decimal
        public decimal BidQuantity { get; set; }
        public decimal BidPricePer1MT { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal PrimaryFreight { get; set; }
        public decimal SecondaryFreight { get; set; }
        public decimal PackingCost { get; set; }
        public decimal HoneycombCost { get; set; }
        public decimal BrokerageCost { get; set; }
        public decimal DetentionCharges { get; set; }
        public decimal DepotCost { get; set; }
        public decimal TD { get; set; }
        public decimal LTD { get; set; }
        public decimal Premium { get; set; }
        public decimal TotalValue { get; set; }
        public decimal RealizationPerCase { get; set; }
        public decimal RealizationPerMt { get; set; }
        public decimal ActualPackingCost { get; set; }
        public decimal MarginCostTP { get; set; }
        public decimal MarginCostRA { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPrice { get; set; }
        public decimal PR00 { get; set; }
        public decimal FRC1 { get; set; }
        public decimal SaleRate { get; set; }
        public string Status { get; set; }
        public decimal LTDValue { get; set; }
        public decimal SpecialRate { get; set; }
        public decimal SchemeCost { get; set; }

        //Long        
        public string PackSize { get; set; }

        //DateTime
        public DateTime BiddingDate { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }

        public string Remarks { get; set; }
        public decimal CushionMargin { get; set; }
        public TimeSpan BiddingTime { get; set; }
        public string OilType { get; set; }
        public decimal TaxPaid { get; set; }
        public decimal Brokerage { get; set; }
        public decimal Purchase { get; set; }
        public string Area { get; set; }
        public decimal RealizationPerCasePostBrokerage { get; set; }
        public decimal SkuWiseWeight { get; set; }
        public decimal RealizationPerMTPostBrokerage { get; set; }
        public decimal FinalRealization { get; set; }
        public decimal RealizationTotal { get; set; }
        public decimal PurchaseTotal { get; set; }
        public decimal MarginPMTLineItem { get; set; }

        public string MaterialType { get; set; }
        public decimal CustomerGroupMargin { get; set; }
        public decimal RaTotalDiscount { get; set; }

        public long SaudaBookingTypeId { get; set; }
        public decimal RAPremiumWithTax { get; set; }
        public decimal RAPremiumWithoutTax { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal OilTransferCost { get; set; }

        public bool IsBaseSauda { get; set; }
        public decimal SkuAllocationPremiumWithTax { get; set; }
        public decimal SkuAllocationPremiumWithoutTax { get; set; }

        public string CustomerGroupOne { get; set; }
        public string CustomerGroupTwo { get; set; }
        public string CustomerGroupFive { get; set; }
        public long SaudaOrderId { get; set; }
        public string AppBookingNo { get; set; }
    }

    public class SaudaModificationReportOutputDto
    {
        public string SaudaNumber { get; set; }
        public long SaudaBookedNumber { get; set; }
        public long SaudaModificationNumber { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string DealerName { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string OilTypeName { get; set; }
        public string OilPackGroupTypeName { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }

    public class SaudaBDOWiseReportDto
    {
        public string BDOCode { get; set; }
        public string BDOName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string OilTypeName { get; set; }
        public decimal? BPInMT { get; set; }
        public decimal? BPInCase { get; set; }
        public decimal? CPInMT { get; set; }
        public decimal? CPInCase { get; set; }
        public decimal? TotalSalesInMT { get; set; }
        public decimal? TotalSalesInCase { get; set; }
    }

    public class ActualSaudaOrderReportOutputDto
    {
        [DisplayName("OilType")]
        public string OilType { get; set; }
        [DisplayName("Material Description")]
        public string SkuName { get; set; }
        [DisplayName("Material Code")]
        public string SkuCode { get; set; }
        [DisplayName("Material Qty")]
        public decimal BidQuantityCase { get; set; }
        [DisplayName("UOM")]
        public string UOM { get; set; }
        [DisplayName("Material Qty(MT)")]
        public decimal BidQuantity { get; set; }
        [DisplayName("Pack Group")]
        public string PackGroup { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("Customer Code")]
        public string CustomerCode { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("Plant Name")]
        public string PlantName { get; set; }
        [DisplayName("Incoterms")]
        public string Incoterms { get; set; }
        [DisplayName("Broker Code")]
        public string BrokerCode { get; set; }
        [DisplayName("Broker Name")]
        public string BrokerName { get; set; }
        [DisplayName("App Contract Time")]
        public String BiddingTime { get; set; }
        [DisplayName("App Contract Date")]
        public String BiddingDate { get; set; }
        [DisplayName("Contract Valid From")]
        public String ValidFromDate { get; set; }
        [DisplayName("Contract Valid To")]
        public String ValidToDate { get; set; }
        [DisplayName("Premium")]
        public decimal Premium { get; set; }
        [DisplayName("TD")]
        public decimal TD { get; set; }
        [DisplayName("LTD")]
        public decimal LTDValue { get; set; }
        [DisplayName("Basic Rate")]
        public decimal SaleRate { get; set; }
        [DisplayName("Discount")]
        public decimal Discount { get; set; }
        [DisplayName("QPS Discount")]
        public decimal QPSDiscount { get; set; }
        [DisplayName("QPS Scheme Id")]
        public string QPSId { get; set; }
        [DisplayName("Individual QPS Discount")]
        public string IndividualQPSDiscount { get; set; }
        [DisplayName("Total Value")]
        public decimal TotalValue { get; set; }
        [DisplayName("SalesOrganization")]
        public string SalesOrganization { get; set; }
        [DisplayName("DistributionChannel")]
        public string DistributionChannel { get; set; }
        [DisplayName("Division")]
        public string Vertical { get; set; }
        [DisplayName("SaudaType")]
        public string SaudaType { get; set; }
        [DisplayName("Employee Code")]
        public string EmployeeCode { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        
        [DisplayName("Status")]
        public string Status { get; set; }
        [DisplayName("Special Rate")]
        public decimal SpecialRate { get; set; }
        [DisplayName("Customer Group Five")]
        public string CustomerGroupFive { get; set; }
        [DisplayName("Sauda Number")]
        public string SaudaNumber { get; set; }
        [DisplayName("App Booking No")]
        public string AppBookingNo { get; set; }
        [DisplayName("App Id")]
        public long SaudaOrderId { get; set; }
        [DisplayName("Sauda Remarks")]
        public string SaudaRemarks { get; set; }
        [DisplayName("Sauda From")]
        public string DirectSauda { get; set; }
        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }
        [DisplayName("Zonal Head")]
        public string ZonalHeadName { get; set; }
        [DisplayName("ApprovalUser")]
        public string ApprovalUser { get; set; }
        [DisplayName("Basic Rate without Packing Charges (PRAmount)")]
        public decimal PRAmount { get; set; }
        [DisplayName("Basic Rate without Packing Charges with GST (PRGST)")]
        public decimal PRGST { get; set; }

        [DisplayName("Is Cross & Upselling Contract")]
        public string IsCrossAndUpsellContract { get; set; }
        [DisplayName("Material Category")]
        public string MaterialCategory { get; set; }
        [DisplayName("Discount Id")]
        public string DiscountId { get; set; }
        [DisplayName("Discount Type")]
        public string DiscountType { get; set; }
    }
}
