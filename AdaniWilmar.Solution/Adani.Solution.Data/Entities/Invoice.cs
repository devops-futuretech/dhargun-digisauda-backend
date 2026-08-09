using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Invoice : Auditable
    {
        [Required]
        public long UserId { get; set; }
        public long LiftingRequestId { get; set; }
        //public long DepotId { get; set; }
        //public long SaudaOrderId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime InvoiceDate { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime? InvoiceDueDate { get; set; }
        public decimal TotalInvoice { get; set; }
        //public decimal PendingInvoice { get; set; }
        //public string DeliveryOrderNumber { get; set; }
        public string BillingDocument { get; set; }
        public string SAPDocumentNo { get; set; }
        public string UserCode { get; set; }      
        public string SalesOrganization { get; set; }        
        //public decimal NetValue { get; set; }
        //public string Plant { get; set; }     
        public bool IsSAPDataSync { get; set; }
        //public string FromWarehouseId { get; set; }
        //public string Mode { get; set; }
        //public string BillDiscount { get; set; }
        //public string BillDiscountType { get; set; }
         public string Status { get; set; }
        //public string SalesDocumentType { get; set; }
        //public string UnitPrice { get; set; }
        //public string VechicleId { get; set; }
        //public string DriverNumber { get; set; }
        //public string DriverName { get; set; }
        //public string GstAmount { get; set; }
        //public string PdfUrl { get; set; }
        //public bool PaymentStatus { get; set; }
        public virtual User User { get; set; }       
    }
}
