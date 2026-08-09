using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaOrder : Auditable
    {
        public long SaudaId { get; set; }
        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public decimal QuotedPrice { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPriceBeforeDiscount { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidPricePerCase { get; set; }
        public long SpecialRateRequestId { get; set; }
        public string SaudaNumber { get; set; }
        public long DiscountTypeId { get; set; }
        public decimal DiscountAmount { get; set; }
        public int StatusId { get; set; }
        public long PricingId { get; set; }
        public string Remarks { get; set; }
        public string Incoterms1 { get; set; }
        public long Incoterms2 { get; set; }
        public long PlantId { get; set; }
        public long DealerLocationId { get; set; }
        public long BrokerId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public bool IsSAPDataSyncApproval { get; set; }
        public bool IsReportingtoAllocation { get; set; }
        public bool IsSAPDataSync { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFromDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidToDate { get; set; }
        public long UomId { get; set; }
        public DateTime? SaudaReleaseDate { get; set; }  
        public decimal BaseRate { get; set; }
        public bool IsSapSauda { get; set; }
        public bool IsBaseSauda { get; set; }
        public long BaseSaudaOrderId { get; set; }
        public bool IsLooseVerticalForAcceptedStatus { get; set; }
        public bool IsQuantityLimitForBookingSauda { get; set; }
        public decimal BaseSkuBidPrice { get; set; }
        public long EmployeeSkuDiscountId { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual Sauda Sauda { get; set; }
        public bool IsSaudaApprovalSyncConfirmation { get; set; }
        public bool IsSaudaApprovalStatusFromSap { get; set; }
        public bool IsSapSaudaNumberUpdateSync { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public decimal SalesOrderQuantityCase { get; set; }
        public decimal InvoiceQuantityCase { get; set; }
        public decimal SalesOrderQuantity { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public decimal QPSDiscount { get; set; }
        public string QpsId { get; set; }
        public string IndividualQPSDiscount { get; set; }
        public decimal PRGST { get; set; }
        public decimal PRAmount { get; set; }
        public bool IsMandatorySku { get; set; }
        public decimal QuotedPriceBeforeSAPDiscount { get; set; }
        public virtual Division Division { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
    }
}
