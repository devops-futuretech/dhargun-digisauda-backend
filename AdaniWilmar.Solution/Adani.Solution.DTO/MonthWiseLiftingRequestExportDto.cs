using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthWiseLiftingRequestExportDto
    {
        public string LiftingRequestNumber { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string OilType { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public string LiftingDate { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string EnquiryNumber { get; set; }
        public string EnquiryRemarks { get; set; }
        public string LiftingRequestStatus { get; set; }
        public string Status { get; set; }
        public string TradeTicketNumber { get; set; }
        public string ApproverRemarks { get; set; }
        public string CustomerRemarks { get; set; }
    }
}
