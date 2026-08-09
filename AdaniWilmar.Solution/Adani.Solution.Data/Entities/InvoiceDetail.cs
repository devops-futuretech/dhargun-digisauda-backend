using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class InvoiceDetail : Auditable
    {
        public string ItemNo { get; set; }
        public long InvoiceId { get; set; }
        public string MaterialNumber { get; set; }      
        public decimal QuantityInCase { get; set; }
        public long  SkuId { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal ActualBilledQuantity { get; set; }
        //public string Discount { get; set; }
        //public string DiscountType { get; set; }
        //public decimal SKUInvoiceTax { get; set; }
        public long OilTypeId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        //public long UomId { get; set; }
        //public long SaudaOrderId { get; set; }
        public virtual Invoice Invoice { get; set; }
        //public long LiftingRequestDetailsId { get; set; }
        //public string DeliveryOrderNumber { get; set; }
        //public string BatchNumber { get; set; }
        //public string InvoiceReturnBatchNumber { get; set; }
        //public string EnquiryNumber { get; set; }
    }
}
