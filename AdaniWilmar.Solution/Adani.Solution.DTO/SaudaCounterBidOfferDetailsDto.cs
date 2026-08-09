using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaCounterBidOfferDetailsDto
    {
        public long Id { get; set; }
        public string SkuName { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string OilTypeName { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCases { get; set; }
        public string FrieghtRoute { get; set; }
        public string PlantDepot { get; set; }
        public string IncoTerms { get; set; }
        public decimal CounterBidOffer { get; set; }
        public decimal BidPricePerCase { get; set; }
        public long SaudaId { get; set; }
        public long StatusId { get; set; }
        public long SaudaOrderId { get; set; }
        public long BiddingWindowId { get; set; }
        public long BiddingWindowStatusId { get; set; }
        public string BiddingWindowStatus { get; set; }
        public string SaudaAllocationTime { get; set; }
    }

    public class SaudaCounterBidOfferStatusUpdate : LoginUserIdDto
    {
        public long Id { get; set; }
        public long StatusId { get; set; }
    }

    public class SaudaCounterBidOfferDetailsInputDto
    {
        public long Id { get; set; }
    }
}
