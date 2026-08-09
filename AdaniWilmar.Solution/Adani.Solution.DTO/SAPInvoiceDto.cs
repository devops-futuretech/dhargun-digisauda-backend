using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASAPInvoiceDtoList
    {
        public List<HANASAPInvoiceDto> SAPInvoiceDto { get; set; }

        public HANASAPInvoiceDtoList()
        {
            SAPInvoiceDto = new List<HANASAPInvoiceDto>();
        }
    }
    public class HANASAPInvoiceDto
    {
        public string BillingDocument { get; set; }
        public string UserCode { get; set; }
        public string Plant { get; set; }
        public string FromWarehouseId { get; set; }
        public string Mode { get; set; }
        public decimal NetValue { get; set; }
        public string BillDiscount { get; set; }
        public string BillDiscountType { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public string MaterialNumber { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal ActualBilledQuantity { get; set; }
        public string Discount { get; set; }
        public string DiscountType { get; set; }
        public string Status { get; set; }
        public decimal SKUInvoiceTax { get; set; }
        public string SalesDocumentType { get; set; }
        public string UnitPrice { get; set; }
        public string VechicleId { get; set; }
        public string DriverNumber { get; set; }
        public string DriverName { get; set; }
        public string GstAmount { get; set; }
        public string VerticalCode { get; set; }
        public string UOM { get; set; }
        public string SaudaNumber { get; set; }
        public string BatchNo { get; set; }
        public string DoNumber { get; set; }
        public string InquiryNumber { get; set; }
        public bool InvoiceCancelFlag { get; set; }
        public bool ReturnFlag { get; set; }
    }
    public class SAPInvoiceDto
    {
        public string BillingDocument { get; set; }
        public string UserCode { get; set; }
        public string Plant { get; set; }
        public string FromWarehouseId { get; set; }
        public string Mode { get; set; }
        public decimal NetValue { get; set; }
        public string BillDiscount { get; set; }
        public string BillDiscountType { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public string MaterialNumber { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal ActualBilledQuantity { get; set; }
        public string Discount { get; set; }
        public string DiscountType { get; set; }
        public string Status { get; set; }
        public decimal SKUInvoiceTax { get; set; }   
        public string SalesDocumentType { get; set; }
        public string UnitPrice { get; set; }
        public string VechicleId { get; set; }
        public string DriverNumber { get; set; }
        public string DriverName { get; set; }
        public string GstAmount { get; set; }
        public string VerticalCode { get; set; }
        public string UOM { get; set; }
        public string SaudaNumber { get; set; }
        public string BatchNo { get; set; }
        public string DoNumber { get; set; }
        public string PdfUrl { get; set; }
        public string PdfFileName { get; set; }  
        public bool InvoiceCancelFlag { get; set; }
        public bool ReturnFlag { get; set; }
    }
    public class SAPInvoiceStatusDto
    {
        public string InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class InvoiceDto
    {
        public string SAPDocNumber { get; set; }
        public string Sap_Document_Number { get; set; }
        public string InvoiceNumber { get; set; }
        public string SAPRefDoc { get; set; }
        public string ImpigerReqNo { get; set; }        
        public string Invoice_Date { get; set; } 
        public string Invoice_Amount { get; set; }
        public string Message { get; set; }
        public List<InvoiceDetailsDto> ItemData { get; set; }

        public InvoiceDto()
        {
            ItemData = new List<InvoiceDetailsDto>();
        }
    }
    public class InvoiceDetailsDto
    {
        public string ItemNo { get; set; }
        public string Material { get; set; }
        public string Bill_Qty { get; set; }   
    }

    public class InvoiceSalesOrder
    {
        public long InvoiceId { get; set; }
        public string SalesOrderNumber { get; set; }
        public string BillingDocument { get; set; }
        public long LiftingRequestId { get; set; }
    }

    public class InvoiceSalesOrderDataDto
    {
        public long SkuId { get; set; }
        public decimal InvoiceQuantityCase { get; set; }
        public decimal InvoiceQuantity { get; set; }

    }
}
