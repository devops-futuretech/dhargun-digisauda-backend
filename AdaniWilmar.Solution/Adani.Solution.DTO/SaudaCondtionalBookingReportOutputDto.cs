using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaCondtionalBookingReportOutputDto
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
        [DisplayName("Material Category")]
        public string MaterialCategory { get; set; }
        [DisplayName("Discount Id")]
        public string DiscountId { get; set; }
        [DisplayName("Discount Type")]
        public string DiscountType { get; set; }
    }
}
