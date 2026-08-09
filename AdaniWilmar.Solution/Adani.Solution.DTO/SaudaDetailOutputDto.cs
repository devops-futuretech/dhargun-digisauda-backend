using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaDetailOutputDto :IAPIInputDTO
    {
        public SaudaDetailOutputDto()
        {

            SaudaLists = new List<SaudaDetailOutputDto>();
            SaudaOrders = new List<SaudaOrderDetails>();
            LiftingDetailGrouping = new List<LiftingDetailGroupingDto>();
            LiftingDetails = new LiftingDetailViewDto();
        }
        public List<SaudaDetailOutputDto> SaudaLists { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public long DiscountTypeId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime SaudaDate { get; set; }
        public int SaudaExpireDays { get; set; }
        public int? SaudaValidityDays { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantityInMT { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ImpactMargin { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string BDOName { get; set; }
        public string BDOCode { get; set; }
        public string DealerCode { get; set; }
        public string DealerLoaction { get; set; }
        public string PlantOrDepot { get; set; }
        public string Incoterm { get; set; }
        public string ApproverRemarks { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal TotalPendingQuantity { get; set; }
        public List<SaudaOrderDetails> SaudaOrders { get; set; }
        public List<LiftingDetailGroupingDto> LiftingDetailGrouping { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public LiftingDetailViewDto LiftingDetails { get; set; }
        public int StatusId { get; set; }

        public int SaudaStatusId { get; set; }
        public string SaudaStatus { get; set; }
        
        public string Status { get; set; }

        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OiltypeName { get; set; }        
        public string StateName { get; set; }        

        public decimal QuotedPrice { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPrice { get; set; }

        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }

        public string Incoterms1 { get; set; }
        public string Incoterms2 { get; set; }
        public string PlantName { get; set; }
        public string DealerLocation { get; set; }
        public string SaudaBookingType { get; set; }
        public string Broker { get; set; }
        public string TradeTicketNumber { get; set; }

        public DateTime BiddingDate { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public string CreatedBy { get; set; }

        public long BrokerId { get; set; }
        public string BrokerName { get; set; }

        public decimal CounterBidOffer { get; set; }
        public decimal BasePricePerCase { get; set; }
        public decimal BidPricePerSku { get; set; }
        public decimal BidPricePerCase { get; set; }
        public long SaudaBookedNumber { get; set; }

        public long SkuId { get; set; }
        public long LoginUserId { get; set; }
        public bool IsFromSAPData { get; set; }
        public List<long> AudiofileDetailIds { get; set; }
        public List<string> ImagePaths { get; set; }
        public bool CanSubmitAudioMapping { get; set; }
        public string Remarks { get; set; }
        public string SaudaListString { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
    }
}
