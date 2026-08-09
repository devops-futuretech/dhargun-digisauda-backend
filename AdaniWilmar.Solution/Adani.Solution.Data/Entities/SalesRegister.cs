using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SalesRegister : Auditable
    {
        public long InvoiceId { get; set; }
        public long SkuId { get; set; }
        public long UserId { get; set; }
        public string MaterialCode { get; set; }
        public string CustomerCode { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal QuantityCase { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal QuantityMT { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNumber { get; set; }
        public string DeliveryNumber { get; set; }
        public string OrderNumber { get; set; }
        public string ContractNumber { get; set; }
        public string BrokerName { get; set; }
        public string LRNo { get; set; }
        public string VehicleNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string TotalGST { get; set; }
        public string TotalAmount { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }

        public string ShiptoParty { get; set; }


        //public string Payer { get; set; }
        //public string PlantCode { get; set; }
        //public string PlantNameOne { get; set; }
        //public string CityPincode { get; set; }
        //public string PlantSAPStateCode { get; set; }
        //public string PlantGSTStateCode { get; set; }
        //public string PlantSAPStateDescription { get; set; }
        //public string DeliveryNo { get; set; }
        //public string BatchItemNo { get; set; }    
        //public string MaterialPricingGrp { get; set; }
        //public string MaterialPricingTxt { get; set; }
        //public string OilTypeOne { get; set; }
        //public string OilTypeDesc { get; set; }      
        //public string DivisionDesc { get; set; }
        //public string Brand { get; set; }
        //public string BrandDesc { get; set; }
        //public string Packtype { get; set; }
        //public string PacktypeDesc { get; set; }
        //public string ItemName { get; set; }
        //public string OilTypeTwo { get; set; }       
        //public string OrderType { get; set; }
        //public string BillNumber { get; set; }
        //public string PriceZone { get; set; }
        //public string PlantNameTwo { get; set; }
        //public string SourceLocation { get; set; }
        //public string CustGrp { get; set; }
        //public string PriceList { get; set; }
        //public string BillingType { get; set; }
        //public string StorageLocation { get; set; }
        //public string BatchNumber { get; set; }
        //public string MfgDate { get; set; }
        //public string QuantityinSKU { get; set; }
        //public string UOM { get; set; }
        //public string SalesUOM { get; set; }       
        //public decimal QuantityKG { get; set; }
        //public string RateperSKU { get; set; }
        //public string ValueinPR00 { get; set; }
        //public string MaterialReturnValue { get; set; }
        //public string DocCurrency { get; set; }
        //public string TradeDiscount { get; set; }
        //public string QuantityDiscount { get; set; }
        //public string SpecialDiscount { get; set; }
        //public string OtherDiscount { get; set; }
        //public string CashDiscount { get; set; }
        //public string FrieghtDiscount { get; set; }
        //public string TaxableAmount { get; set; }
        //public string TotalvalueBeforeGST { get; set; }
        //public string NetReturnValue { get; set; }
        //public string NetTaxAmount { get; set; }
        //public DateTime? BillingDate { get; set; }
        //public string BillingTime { get; set; }
        //public string SoldtoPartyCountry { get; set; }
        //public string ShipTo { get; set; }
        //public string ShipToPartyDescription { get; set; }
        //public string ShipToPartySAPStateCode { get; set; }
        //public string StateofShipparty { get; set; }
        //public string ShipToPartyGSTStateCode { get; set; }
        //public string ShipToPartyGSTNO { get; set; }
        //public string SoldToParty { get; set; }
        //public string SoldToPartyDescription { get; set; }
        //public string StateofSoldparty { get; set; }
        //public string BillToParty { get; set; }
        //public string BillToPartyDescription { get; set; }
        //public string BillToPartySAPStateCode { get; set; }
        //public string StateofBillparty { get; set; }
        //public string BillToPartyGSTStateCode { get; set; }
        //public string BillToPartyGSTNO { get; set; }
        //public string SalesOrderNo { get; set; }
        //public string SalesUnit { get; set; }
        //public string PONo { get; set; }
        //public string PODate { get; set; }
        //public string FreightTerms { get; set; }
        //public string Contractnumber { get; set; }
        //public string ContractDate { get; set; }
        //public string VAT { get; set; }
        //public string CST { get; set; }
        //public string AgriTax { get; set; }
        //public string freight { get; set; }
        //public string freightDiff { get; set; }
        //public string Entrytax { get; set; }
        //public string ExiseDuty { get; set; }
        //public string RoundOff { get; set; }
        //public string Discount { get; set; }
        //public string SAT { get; set; }
        //public string FirstBrokerageRate { get; set; }
        //public string FirstBrokerage { get; set; }
        //public string SecondBrokerName { get; set; }
        //public string SecondBrokerageRate { get; set; }
        //public string SecondBrokerage { get; set; }
        //public string VATSurcharge { get; set; }
        //public string SGST { get; set; }
        //public string CGST { get; set; }
        //public string IGST { get; set; }
        //public string UGST { get; set; }
        //public string SGSTPercentage { get; set; }
        //public string CGSTPercentage { get; set; }
        //public string IGSTPercentage { get; set; }
        //public string UGSTPercentage { get; set; }
        //public string CompCess { get; set; }       
        //public string TotalValueWithGST { get; set; }
        //public string WaybillNo { get; set; }
        //public string Vehicleno { get; set; }
        //public string Transporter { get; set; }
        //public string TransporterName { get; set; }
        //public string FrieghtFRC3 { get; set; }
        //public long Insuranceandpackingcharges { get; set; }
        //public string ContractDescriptionColumn { get; set; }
        //public string Transportationzone { get; set; }
        //public string TzoneDescription { get; set; }
        //public string ShipTOPartyZone { get; set; }
        //public string BillTOPartyZone { get; set; }
        //public DateTime? ContractValidfrom { get; set; }
        //public DateTime? ContractValidto { get; set; }
        //public string LiquidationDisc { get; set; }
        //public string TTNumber { get; set; }
        //public string MRPValue { get; set; }
        //public string PrimaryDiscount { get; set; }
        //public string INS { get; set; }

    }
}



