using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.DTO
{



    public class SalesRegisterExportDto
    {
        [DisplayName("Material Code")]
        public string MaterialCode { get; set; }
        [DisplayName("Material Name")]
        public string MaterialName { get; set; }
        [DisplayName("Distributor Code")]
        public string DistributorCode { get; set; }
        [DisplayName("Distributor Name")]
        public string DistributorName { get; set; }
        [DisplayName("Quantity (MT)")]
        public decimal QuantityMT { get; set; }
        [DisplayName("Invoice Type")]
        public string InvoiceType { get; set; }
        [DisplayName("Invoice Number")]
        public string InvoiceNumber { get; set; }
        [DisplayName("Delivery Number")]
        public string DeliveryNumber { get; set; }
        [DisplayName("Invoice Date")]
        public string InvoiceDate { get; set; }
        [DisplayName("Created Date")]
        public string CreatedDate { get; set; }
        [DisplayName("Total GST")]
        public string TotalGST { get; set; }
        [DisplayName("Total Amount")]
        public decimal TotalAmount { get; set; }
        [DisplayName("Sales Organization")]
        public string SalesOrganization { get; set; }
        [DisplayName("Distribution Channel")]
        public string DistributionChannel { get; set; }
        [DisplayName("Division")]
        public string Division { get; set; }
        [DisplayName("Order Number")]
        public string OrderNumber { get; set; }
        [DisplayName("Contract Number")]
        public string ContractNumber { get; set; }
        [DisplayName("ShipToParty")]
        public string ShiptToParty { get; set; }
        [DisplayName("BrokerName")]
        public string BrokerName { get; set; }
        [DisplayName("Vehicle Number")]
        public string VehicleNumber { get; set; }
        [DisplayName("LRNo")]
        public string LRNo { get; set; }
    }
    public class SalesReportOutputDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class SalesBDOWiseReportDto
    {
        public string BDOCode { get; set; }
        public string BDOName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string OilTypeName { get; set; }
        //public decimal? BPInMT { get; set; }
        //public decimal? BPInCase { get; set; }
        //public decimal? CPInMT { get; set; }
        //public decimal? CPInCase { get; set; }
        public decimal? TotalSalesInMT { get; set; }
        public decimal? TotalSalesInCase { get; set; }
    }
    public class HANASalesRegisterOutputDto
    {
        public List<HANASalesRegister> SalesRegisterList { get; set; }

        public HANASalesRegisterOutputDto()
        {
            SalesRegisterList = new List<HANASalesRegister>();
        }
    }

    public class AWLSalesRegisterOutputDto
    {
        public List<AWLSalesRegister> Records { get; set; }

        public AWLSalesRegisterOutputDto()
        {
            Records = new List<AWLSalesRegister>();
        }
    }

    public class AWLSalesRegister
    {
        public string Material { get; set; }
        public string Customer { get; set; }
        public string Qty_TON { get; set; }
        public string Invoice_Type { get; set; }
        public string Contract_Number { get; set; }
        public string Order_Number { get; set; }
        public string Delivery_Number { get; set; }
        public string Invoice_Number { get; set; }
        public string Invoice_Date { get; set; }
        public string Total_GST { get; set; }
        public string Total_Amount { get; set; }
        public string Sales_Org { get; set; }
        public string Distribution_Channel { get; set; }
        public string Division { get; set; }
        public string Broker_Name { get; set; }
        public string LR_No { get; set; }
        public string Vehicle_No { get; set; }
        public string ShiptoParty { get; set; }

    }

    public class HANASalesRegister
    {
        //public long Id { get; set; }
        public string Payer { get; set; }
        public string PlantCode { get; set; }
        public string PlantNameOne { get; set; }
        public string CityPincode { get; set; }
        public string PlantSAPStateCode { get; set; }
        public string PlantGSTStateCode { get; set; }
        public string PlantSAPStateDescription { get; set; }
        public string DeliveryNo { get; set; }
        public string BatchItemNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialPricingGrp { get; set; }
        public string MaterialPricingTxt { get; set; }        
        public string PacktypeDesc { get; set; }
        public string ItemName { get; set; }
        public string OilTypeTwo { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Vertical { get; set; }       
        public string OrderType { get; set; }
        public string BillNumber { get; set; }
        public string CustGrp { get; set; }
        public string PriceZone { get; set; }
        public string SourceLocation { get; set; }
        public string PriceList { get; set; }
        public string BillingType { get; set; }
        public string StorageLocation { get; set; }
        public string PlantNameTwo { get; set; }
        public string BatchNumber { get; set; }
        public string MfgDate { get; set; }
        public string QuantityinSKU { get; set; }
        public string UOM { get; set; }
        public string SalesUOM { get; set; }
        public decimal QuantityCase { get; set; }
        public decimal QuantityMT { get; set; }
        public decimal QuantityKG { get; set; }
        public string RateperSKU { get; set; }
        public string ValueinPR00 { get; set; }
        public string MaterialReturnValue { get; set; }
        public string DocCurrency { get; set; }
        public string TradeDiscount { get; set; }
        public string QuantityDiscount { get; set; }
        public string SpecialDiscount { get; set; }
        public string OtherDiscount { get; set; }
        public string CashDiscount { get; set; }
        public string FrieghtDiscount { get; set; }
        public string TaxableAmount { get; set; }
        public string TotalvalueBeforeGST { get; set; }
        public string NetReturnValue { get; set; }
        public string NetTaxAmount { get; set; }
        public DateTime? BillingDate { get; set; }
        public string BillingTime { get; set; }
        public string SoldtoPartyCountry { get; set; }
        public string ShipTo { get; set; }
        public string ShipToPartyDescription { get; set; }
        public string ShipToPartySAPStateCode { get; set; }
        public string StateofShipparty { get; set; }
        public string ShipToPartyGSTStateCode { get; set; }
        public string ShipToPartyGSTNO { get; set; }
        public string SoldToParty { get; set; }
        public string SoldToPartyDescription { get; set; }
        public string StateofSoldparty { get; set; }
        public string BillToParty { get; set; }
        public string BillToPartyDescription { get; set; }
        public string BillToPartySAPStateCode { get; set; }
        public string StateofBillparty { get; set; }
        public string BillToPartyGSTStateCode { get; set; }
        public string BillToPartyGSTNO { get; set; }
        public string SalesOrderNo { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string FreightTerms { get; set; }
        public string Contractnumber { get; set; }
        public string ContractDate { get; set; }        
        public string INS { get; set; }      
        public string freight { get; set; }
        public string freightDiff { get; set; }       
        public string RoundOff { get; set; }
        public string Discount { get; set; }       
        public string FirstBrokerageRate { get; set; }
        public string FirstBrokerage { get; set; }
        public string SecondBrokerName { get; set; }
        public string SecondBrokerageRate { get; set; }
        public string SecondBrokerage { get; set; }        
        public string SGST { get; set; }
        public string CGST { get; set; }
        public string IGST { get; set; }
        public string UGST { get; set; }
        public string SGSTPercentage { get; set; }
        public string CGSTPercentage { get; set; }
        public string IGSTPercentage { get; set; }
        public string UGSTPercentage { get; set; }
        public string CompCess { get; set; }
        public string TotalGST { get; set; }
        public string TotalValueWithGST { get; set; }
        public string WaybillNo { get; set; }
        public string Vehicleno { get; set; }
        public string Transporter { get; set; }
        public string TransporterName { get; set; }
        public string FrieghtFRC3 { get; set; }
        public long Insuranceandpackingcharges { get; set; }
        public string PrimaryDiscount { get; set; }
        public string ContractDescriptionColumn { get; set; }
        public string Transportationzone { get; set; }
        public string TzoneDescription { get; set; }
        public string ShipTOPartyZone { get; set; }
        public string BillTOPartyZone { get; set; }
        public DateTime? ContractValidfrom { get; set; }
        public DateTime? ContractValidto { get; set; }
        public string LiquidationDisc { get; set; }
        public string TTNumber { get; set; }
        public string MRPValue { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        //public string CreatedBy { get; set; }
        //public string ModifiedBy { get; set; }
        //public string PackgroupName { get; set; }
        //public string DoNumber { get; set; }
        //public string InvBillNumber { get; set; }
        //public decimal InvQuantityInCase { get; set; }
        //public string Status { get; set; }
        //public string ActionToTaken { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime ModifiedDate { get; set; }
        //public string SalesUnit { get; set; }

        public int BatchNo  { get; set; }

    }

    public class SalesRegisterOutputDto
    {
        public long Id { get; set; }
        public string Payer { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceType { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string PlantCode { get; set; }
        public string PlantNameOne { get; set; }
        public string CityPincode { get; set; }
        public string PlantSAPStateCode { get; set; }
        public string PlantGSTStateCode { get; set; }
        public string PlantSAPStateDescription { get; set; }
        public string DeliveryNo { get; set; }
        public string BatchItemNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialPricingGrp { get; set; }
        public string MaterialPricingTxt { get; set; }
        public string OilTypeOne { get; set; }
        public string OilTypeDesc { get; set; }
        public string Vertical { get; set; }
        public string VerticalDesc { get; set; }
        public string Brand { get; set; }
        public string BrandDesc { get; set; }
        public string Packtype { get; set; }
        public string PacktypeDesc { get; set; }
        public string ItemName { get; set; }
        public string OilTypeTwo { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string OrderType { get; set; }
        public string OrderNumber { get; set; }
        public string BillNumber { get; set; }
        public string CustGrp { get; set; }
        public string PriceZone { get; set; }
        public string SourceLocation { get; set; }
        public string PriceList { get; set; }
        public string BillingType { get; set; }
        public string StorageLocation { get; set; }
        public string PlantNameTwo { get; set; }
        public string BatchNumber { get; set; }
        public string MfgDate { get; set; }
        public string QuantityinSKU { get; set; }
        public string UOM { get; set; }
        public string SalesUOM { get; set; }
        public decimal QuantityCase { get; set; }
        public decimal QuantityMT { get; set; }
        public decimal QuantityKG { get; set; }
        public string RateperSKU { get; set; }
        public string ValueinPR00 { get; set; }
        public string MaterialReturnValue { get; set; }
        public string DocCurrency { get; set; }
        public string TradeDiscount { get; set; }
        public string QuantityDiscount { get; set; }
        public string SpecialDiscount { get; set; }
        public string OtherDiscount { get; set; }
        public string CashDiscount { get; set; }
        public string FrieghtDiscount { get; set; }
        public string TaxableAmount { get; set; }
        public string TotalvalueBeforeGST { get; set; }
        public string NetReturnValue { get; set; }
        public string NetTaxAmount { get; set; }
        public DateTime? BillingDate { get; set; }
        public string BillingTime { get; set; }
        public string SoldtoPartyCountry { get; set; }
        public string ShipTo { get; set; }
        public string ShipToPartyDescription { get; set; }
        public string ShipToPartySAPStateCode { get; set; }
        public string StateofShipparty { get; set; }
        public string ShipToPartyGSTStateCode { get; set; }
        public string ShipToPartyGSTNO { get; set; }
        public string SoldToParty { get; set; }
        public string SoldToPartyDescription { get; set; }
        public string StateofSoldparty { get; set; }
        public string BillToParty { get; set; }
        public string BillToPartyDescription { get; set; }
        public string BillToPartySAPStateCode { get; set; }
        public string StateofBillparty { get; set; }
        public string BillToPartyGSTStateCode { get; set; }
        public string BillToPartyGSTNO { get; set; }
        public string SalesOrderNo { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string FreightTerms { get; set; }
        public string Contractnumber { get; set; }
        public string ContractDate { get; set; }
        public string VAT { get; set; }
        public string CST { get; set; }
        public string INS { get; set; }
        public string AgriTax { get; set; }
        public string freight { get; set; }
        public string freightDiff { get; set; }
        public string Entrytax { get; set; }
        public string ExiseDuty { get; set; }
        public string RoundOff { get; set; }
        public string Discount { get; set; }
        public string SAT { get; set; }
        public string FirstBrokerageRate { get; set; }
        public string FirstBrokerage { get; set; }
        public string SecondBrokerName { get; set; }
        public string SecondBrokerageRate { get; set; }
        public string SecondBrokerage { get; set; }
        public string VATSurcharge { get; set; }
        public string SGST { get; set; }
        public string CGST { get; set; }
        public string IGST { get; set; }
        public string UGST { get; set; }
        public string SGSTPercentage { get; set; }
        public string CGSTPercentage { get; set; }
        public string IGSTPercentage { get; set; }
        public string UGSTPercentage { get; set; }
        public string CompCess { get; set; }
        public string TotalGST { get; set; }
        public string TotalValueWithGST { get; set; }
        public string WaybillNo { get; set; }
        public string LRNo { get; set; }
        public string Vehicleno { get; set; }
        public string Transporter { get; set; }
        public string TransporterName { get; set; }
        public string FrieghtFRC3 { get; set; }
        public long Insuranceandpackingcharges { get; set; }
        public string PrimaryDiscount { get; set; }
        public string ContractDescriptionColumn { get; set; }
        public string Transportationzone { get; set; }
        public string TzoneDescription { get; set; }
        public string ShipTOPartyZone { get; set; }
        public string BillTOPartyZone { get; set; }
        public DateTime? ContractValidfrom { get; set; }
        public DateTime? ContractValidto { get; set; }
        public string LiquidationDisc { get; set; }
        public string TTNumber { get; set; }
        public string MRPValue { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string PackgroupName { get; set; }
        public string DoNumber { get; set; }
        public string InvBillNumber { get; set; }
        public decimal InvQuantityInCase { get; set; }
        public string Status { get; set; }
        public string ActionToTaken { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SalesUnit { get; set; }
        public string ShiptoParty { get; set; }

    }


    public class CallRecordingDto : IAPIInputDTO
    {
        public string FileDownloadName { get; set; }
        public DateTime CallRecordedDate { get; set; }
        public List<CallRecordingListOutputDto> CallRecordingListOutput { get; set; }
        public  CallRecordingDto()
        {
            CallRecordingListOutput = new List<CallRecordingListOutputDto>();
        }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class CallRecordingListOutputDto 
    {
        public long AudioFileDetailId { get; set; }
        public string EncryptedId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ZonalHeadName { get; set; }
        public string BdoName { get; set; }
        public long SaudaId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime CallRecordedDate { get; set; }
        public DateTime CallRecordedTime { get; set; }
        public string CallRecordedPath { get; set; }
        public string CallRecordedFileName { get; set; }
        public string CalledBy { get; set; }
        public string CalledTo { get; set; }
        public string ImagePaths { get; set; }
        public int MediaTypeId { get; set; }
        public DateTime SaudaBookedDate { get; set; }
        public string AudioFiles { get; set; }
        public string FileDownloadName { get; set; }
        
    }

    public class CallRecordMapDto
    {
        [DisplayName("Customer Code")]
        public string CustomerCode { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("ZonalTrader Name")]
        public string ZonaltradeName { get; set; }
        [DisplayName("StateTrader Name")]
        public string StateTraderName { get; set; }
        [DisplayName("Sauda BookingId")]
        public string SaudaBookinId { get; set; }
        [DisplayName("Sauda Number")]
        public string SaudaNumber { get; set; }
        [DisplayName("Sauda Booked Date")]
        public string SaudaBookedDate { get; set; }
        [DisplayName("Images")]
        public string Images { get; set; }
        [DisplayName("Audio Files")]
        public string AudioFiles { get; set; }
    }

    public class CallRecordingExportDto
    {
        [DisplayName("Called By")]
        public string CalledBy { get; set; }
        [DisplayName("Called To")]
        public string CalledTo { get; set; }
        [DisplayName("Call Recorded Date")]
        public string CallRecordingDate { get; set; }
        [DisplayName("Call Recorded Time")]
        public string CallRecordedTime { get; set; }
        [DisplayName("Audio File")]
        public string Audiofile { get; set; }
    }

    }
