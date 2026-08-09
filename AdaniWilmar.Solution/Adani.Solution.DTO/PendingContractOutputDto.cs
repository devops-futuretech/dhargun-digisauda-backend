using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingContractOutputDto
    {
        public long Id { get; set; }
        public DateTime CurrentDate { get; set; }
        public List<PendingContractDealerOutputDto> PendingContractDealerOutput { get; set; }
        public decimal TotalQuantityInMT { get; set; }
        public decimal TotalQuantityInCase { get; set; }
        public DateTime PendingDate { get; set; }
        public PendingContractOutputDto()
        {
            PendingContractDealerOutput = new List<PendingContractDealerOutputDto>();
        }
    }

    public class PendingContractDealerOutputDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public List<PendingContractSkuOutputDto> PendingContractSkuOutput { get; set; }
    }
    public class PendingContractSkuDetailsDto
    {
        public decimal QuantityInCase { get; set; }
        public decimal QuantityInMT { get; set; }
        public long BdoIsd { get; set; }
        public string BdoName { get; set; }
        public decimal Rate { get; set; }
        public long UserId { get; set; }
        public DateTime BiddingDate { get; set; }
        public string Dealer { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractValidFrom { get; set; }
        public DateTime ContractValidTo { get; set; }
    }
    public class PendingContractSkuOutputDto
    {
        public long SkuId { get; set; }
        public long StateId { get; set; }
        public string Sku { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal QuantityInMT { get; set; }
        public long BdoId { get; set; }
        public string BdoName { get; set; }
        public decimal Rate { get; set; }
        public long UserId { get; set; }
        public DateTime BiddingDate { get; set; }
        public string Dealer { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractValidFrom { get; set; }
        public DateTime ContractValidTo { get; set; }
        public List<PendingContractSkuDetailsDto> PendingContractSkuDetails { get; set; }
    }

    public class PendingContractOutputDtoDealer
    {
        public long Id { get; set; }
        public long SaudaOrderId { get; set; }
        public long UserId { get; set; }
        public string User { get; set; }
        public string City { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal TotalBidPrice { get; set; }
        public decimal TotalBidQuantity { get; set; }
        public string OiltypeName { get; set; }
        public long OilTypeId { get; set; }
        public string SaudaNumber { get; set; }

    }
}
