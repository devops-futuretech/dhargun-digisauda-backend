using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IndentReportDto
    {
        public String IndentReceivedDate { get; set; }
        public string IndentReceivedTime { get; set; }
        [DisplayName("Sales Order Request Number")]
        public string IndentNo { get; set; }
        public string BDOName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string ShipToPartyCode { get; set; }
        public string ShipToPartyName { get; set; }
        public string State { get; set; }
        [DisplayName("Quantity (MT)")]
        public decimal LiftingQuantityInMT { get; set; }
        [DisplayName("Quantity")]
        public decimal LiftingQuantityCase { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string InquiryNumber { get; set; }
        public string PlantOrDepotName { get; set; }
        public string ContractNumber { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string Status { get; set; }
        public bool IsSapSalesOrder { get; set; }
        public string CreatedBy { get; set; }
    }
}
