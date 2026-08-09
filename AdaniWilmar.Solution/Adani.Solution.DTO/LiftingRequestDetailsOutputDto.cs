using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestDetailsOutputDto
    {
        public long Id { get; set; }
        public long LiftingRequestId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SaudaOrderId { get; set; }
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public long ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string EnquiryNumber { get; set; }
        public string EnquiryRemarks { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserCode { get; set; }
        public string SaudaNumber { get; set; }
    }

    public class SaudaOrderLiftingRequestDto
    {
        public long Id { get; set; }
        public string SaudaNumber { get; set; }
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public string DeliveryOrderNumber { get; set; }        
        public string StatusName { get; set; }
        public int StatusId { get; set; }
    }
    public class InvoiceLiftingRequestDto
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserCode { get; set; }
        //public string StatusName { get; set; }
        //public int StatusId { get; set; }
    }
}
