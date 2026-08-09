using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaOrderInputDto
    {
        public long PricingId { get; set; }
        public long SkuId { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidPrice { get; set; }
        public decimal QuotedPrice { get; set; }
        public long OilTypeId { get; set; }
        public int StatusId { get; set; }
        public long DiscountTypeId { get; set; }
        public decimal DiscountAmount { get; set; }
        public long BiddingwindowId { get; set; }
        public decimal DiscountAmountPerCase { get; set; }
        public long IncotermsId { get; set; }
        public long PlantId { get; set; }
        public long DealerLocationId { get; set; }
        public DateTime? SaudaValidFromDate { get; set; }
        public DateTime? SaudaValidToDate { get; set; }
        public long PricingRecordId { get; set; }
        public decimal QPSDiscount { get; set; }
        public bool IsMandatorySku { get; set; } = false;
        public long DiscountId { get; set; }
    }

    public class SaudaOrderViewDto
    {
        public long Id { get; set; }
        public long SaudhaId { get; set; }
        public string SaudhaNumber { get; set; }
        public long SaudaOrderId { get; set; }
        public long OilTypeId { get; set; }
        public string Oiltype { get; set; }
        public long SkuId { get; set; }
        public string Sku { get; set; }
        public DateTime BiddingDate { get; set; }
        public string TradeTicketNumber { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidPricePerSku { get; set; }
        public string PlantName { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal BidQuantityCase { get; set; }
        public string DealerName { get; set; }
        public string StateName { get; set; }
    }

    public class TradeTicketMaptoSaudaOrderDto : IAPIInputDTO
    {
        public string TradeTicketNumber { get; set; }
        public List<long> SaudaOrders { get; set; }
        public long LoginUserId { get; set; }
        public decimal OpenQuantity { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SaudaDto
    {
        public int StatusId { get; set; }
    }


    public class SaudaCreateNotificationDto
    {
        public int StatusId { get; set; }
        public long SaudaOrderId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public int SaudaOrderStatusId { get; set; }
        public int SaudaOrderCreatedBy { get; set; }
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public string SkuName { get; set; }
        public long BiddingWindowId { get; set; }
        public string WindowName { get; set; }
        public long OilTypeId { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidQuantityInCase { get; set; }
    }


    public class TradeTicketDto
    {
        public string TradeticketNumber { get; set; }
    }
}