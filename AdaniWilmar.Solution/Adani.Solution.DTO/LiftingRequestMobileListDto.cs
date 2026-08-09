using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestMobileListDto
    {
        public long LiftingRequestId { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public string LiftingRequestNumber { get; set; }
        public DateTime LiftingRequestdate { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal RequestedQuantityInCase { get; set; }
        public string CreatedUser { get; set; }
        public string Remarks { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public string CustomerRemarks { get; set; }
        public long? ShipToPartyId { get; set; }
        public string ShipToParty { get; set; }
        public bool ReprocessStatusId { get; set; }
        public string EnquiryRemarks { get; set; }
        public bool EnquiryNumberSyncFromSap { get; set; }
    }

    public class LiftingRequestDetDTO
    {
        public long LiftingRequestId { get; set; }
        public string EnquiryRemarks { get; set; }
        public bool EnquiryNumberSyncFromSap { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal RequestedQuantityInCase { get; set; }
    }

    public class LiftingRequestDetailMobileListDto
    {
        public long LiftingRequestDetailId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilType { get; set; }
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public string LDDeliveryOrderNumber { get; set; }
        public string LDStatusName { get; set; }
        public string LDEnquiryNumber { get; set; }

        public long SaudaOrderLRId { get; set; }
        public string SaudaNumber { get; set; }
        public decimal SaudaOrderLRLiftingQuantity { get; set; }
        public decimal SaudaOrderLRLiftingQuantityCase { get; set; }
        public string SaudaOrderLRDeliveryOrderNumber { get; set; }
        public string SaudaOrderLRStatus { get; set; }
        public int SaudaOrderLRStatusId { get; set; }
    }
}
