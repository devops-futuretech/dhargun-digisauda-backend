using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class PendingContract : Auditable
    {
        public long UserId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public string MaterialCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }       
        public DateTime? ContractValidTo { get; set; }
        public DateTime? ContractValidFrom { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal BasicRate { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal PendingQuantityInCase { get; set; }
        [DecimalPrecision(18, 3)]
        public decimal SaudaQuantity { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsSaudaExtended { get; set; }

        [DecimalPrecision(18, 3)]
        public decimal OpenSalesOrderQuantity { get; set; }
        

        //public string PlantCode { get; set; }
        //public string PlantName { get; set; }        
        //public DateTime? RecordCreatedDate { get; set; }
        //public string SalesOrganization { get; set; }
        //public string SalesOrgDescription { get; set; }
        //public DateTime? ContractValidFrom { get; set; }       
        //public string PONumber { get; set; }
        //public DateTime? SaudaDate { get; set; }
        //public string IncoTerms1 { get; set; }
        //public string Tax { get; set; }
        //public string BrokerCode { get; set; }
        //public string Place { get; set; }
        //public string BrokerName { get; set; }
        //public string BrokerCity { get; set; }
        //public string BrokerRegionDescription { get; set; }

        //public string CustomerCity { get; set; }
        //public string CustomerRegionDescription { get; set; }
        //public string CustomerRegionalMarket { get; set; }

        //public string CustomerMaterialCode { get; set; }
        //public string MaterialGroup { get; set; }
        //public string MaterialDescription1 { get; set; }
        //public string MaterialDescription2 { get; set; }
        //public string Location { get; set; }

        //[DecimalPrecision(18, 3)]
        //public decimal Discount { get; set; }
        //[DecimalPrecision(18, 3)]
        //public decimal BasicRateAfterDiscount { get; set; }
        //public string PR00 { get; set; }
        //public string ConditionType { get; set; }
        //public string ZDC1 { get; set; }
        //public string ZDC2 { get; set; }
        //public string ZPU1 { get; set; }
        //public string ZPU2 { get; set; }
        //public string FRC1 { get; set; }
        //public string FRC2 { get; set; }
        //public string JINSVALUE { get; set; }
        //[DecimalPrecision(18, 3)]
        //public decimal DespatchQty { get; set; }
        //[DecimalPrecision(18, 3)]
        //public decimal PendingQuantityInMT { get; set; }

        //public string UOM { get; set; }
        //public string ContractType { get; set; }
        //public string PartnerFunction { get; set; }
        //public string Description { get; set; }       
        //public string ReleaseStatus { get; set; }
        //public string MaterialGroup1 { get; set; }
        //public string MaterialGroup2 { get; set; }
        //public string MaterialGroup3 { get; set; }
        //public string MaterialGroup4 { get; set; }
        //public string MaterialGroup5 { get; set; }
        //public string MaterialGroupDescription1 { get; set; }
        //public string MaterialGroupDescription2 { get; set; }
        //public string MaterialGroupDescription3 { get; set; }
        //public string MaterialGroupDescription4 { get; set; }
        //public string MaterialGroupDescription5 { get; set; }
        //public string UsageDescription { get; set; }        
        //public string CreatedPerson { get; set; }
        //public string UsegeIndicator { get; set; }       
        //public string TermsOfPaymentKey { get; set; }

        //public string Validity { get; set; }
        //public long AgingByDays { get; set; }
    }
}
