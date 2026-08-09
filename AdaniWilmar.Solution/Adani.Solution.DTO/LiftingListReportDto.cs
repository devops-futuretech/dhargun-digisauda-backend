using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingListReportDto
    {
        public long LiftingRequestId { get; set; }
        //LiftingRequestNumber/IndentNo
        public string IndentNo { get; set; }
        //LiftingRequestdate
        public DateTime IndentReceivedDate { get; set; }
        public string IndentReceivedTime { get; set; }
        public string BDOName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public long? ShipToPartyId { get; set; }
        public string ShipToPartyName { get; set; }
        public string ShipToPartyCode { get; set; }
        public string Destination { get; set; }
        public string State { get; set; }
        public decimal LiftingQuantityInMT { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public decimal TotalQuantityInMT { get; set; }
        public decimal TotalQuantityInCase { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Status { get; set; }
        public string InquiryNumber { get; set; }
        public string ContractNumber { get; set; }
        public string DOStatus { get; set; }
        public string PlantOrDepotName { get; set; }
        public decimal VehicleSize { get; set; }
        public decimal GrossWeight { get; set; }
        public bool IsSAPSalesOrder { get; set; }
        public string CreatedByName { get; set; }
    }
}
