using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthwiseSaudaExportDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public string Vertical { get; set; }
        public string OilName { get; set; }
        public string SkuName { get; set; }
        public decimal QuotedPrice { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPrice { get; set; }
        public long SpecialRateRequestId { get; set; }
        public string SaudaNumber { get; set; }
        public string TradeTicketNumber { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Status { get; set; }
        public string SaudaStatus { get; set; }
        public long BiddingwindowId { get; set; }
        public string CustomerPONumber { get; set; }
        public long ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string Incoterms1 { get; set; }
        public long Incoterms2 { get; set; }
        public long DealerTypeId { get; set; }
        public string PlantName { get; set; }
        public long DealerLocationId { get; set; }
        public long BrokerId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public decimal CounterBidOffer { get; set; }
        public string CounterBidOfferDate { get; set; }
        public string ValidFromDate { get; set; }
        public string ValidToDate { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public long UomId { get; set; }
        public decimal Proo { get; set; }
        public decimal Frc1 { get; set; }
        public bool IsSAPDataSyncApproval { get; set; }
        public bool IsSAPDataSync { get; set; }
        public long DepotIdForRake { get; set; }
    }
}
