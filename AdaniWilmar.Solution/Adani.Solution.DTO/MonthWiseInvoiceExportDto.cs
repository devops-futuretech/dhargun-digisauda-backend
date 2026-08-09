using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthWiseInvoiceExportDto
    {
        public int Id { get; set; }
        public string DealerName { get; set; }
        public string UserCode { get; set; }
        public string PlantName { get; set; }
        public string PlantCode { get; set; }
        public string OilType { get; set; }
        public string SkuName { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal ActualBilledQuantity { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceDueDate { get; set; }
        public string CreatedDate { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal PendingInvoice { get; set; }
        public string BillingDocument { get; set; }
        public decimal NetValue { get; set; }
        public string Discount { get; set; }
        public decimal SKUInvoiceTax { get; set; }
        public long UomId { get; set; }
        public string FromWarehouseId { get; set; }
        public string Mode { get; set; }
        public string BillDiscount { get; set; }
        public string BillDiscountType { get; set; }
        public string Status { get; set; }
        public string SalesDocumentType { get; set; }
        public string UnitPrice { get; set; }
        public string VechicleId { get; set; }
        public string DriverNumber { get; set; }
        public string DriverName { get; set; }
        public string GstAmount { get; set; }
        public string PdfUrl { get; set; }
    }
}
