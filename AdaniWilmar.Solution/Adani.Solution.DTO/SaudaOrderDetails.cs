using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaOrderDetails : CommonResultDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCases { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal CounterBidOffer { get; set; }
        public DateTime? CounterBidOfferDate { get; set; }

        public decimal Discount { get; set; }
        public decimal QuotedPrice { get; set; }
        public DateTime LiftedDate { get; set; }

        public string IncoTerms { get; set; }
        public string PlantDepot { get; set; }
        //public string FrieghtRoute { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }

        public DateTime ValidToDate { get; set; }
        public DateTime BookedDate { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public long DiscountTypeId { get; set; }
        public long SaudaConversionId { get; set; }
        public long CreatedBy { get; set; }

        public string CityName { get; set; }
        public string PlantName { get; set; }
        public string IncoTerm { get; set; }
        public DateTime? ValidFrom { get; set; }

        public decimal PendingQuantity { get; set; }
        public decimal PendingQuantityCases { get; set; }

        public string Remarks { get; set; }

        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long IncoTermId { get; set; }
        public decimal BidPricePerCaseWithoutTax { get; set; }
        public SKUDetail SKUDetail { get; set; }
    }
}
