using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaListOutputDto
    {
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public DateTime BiddingDate { get; set; }
        public string DealerName { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalAmt { get; set; }
        public string DeliveryLocation { get; set; }
        public string PlantOrDepot { get; set; }
        public string IncoTerms { get;set;}
        public string SaudaNo { get; set; }
        public string SaudaNumber { get; set; }
        public long DealerId { get; set; }
    }

    public class SaudaListGroupedOutputDto
    {
        public DateTime BiddingDate { get; set; }
        public List<SaudaListOutputDto> saudaListOutputs { get; set; }
        public SaudaListGroupedOutputDto()
        {
            saudaListOutputs = new List<SaudaListOutputDto>();
        }
    }
        public class SaudaDetailsListDto
    {
        public int Id { get; set; }
        public string OilType { get; set; }
        public string Sku { get; set; }
        public string Qty { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
    }
     
    public class LiftingListOutputDto
    {
        public int SaudaNumber { get; set; }
        public DateTime Date { get; set; }
        public string DealerName { get; set; }
        public int TotalQty { get; set; }
        public int RequestedQty { get; set; }
        public int PendingQty { get; set; }
    }

    public class LiftingDetailDto
    {
        public string DealerName { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }

    public class LiftingDetailsListto
    {
        public string OilType { get; set; }
        public string Sku { get; set; }
        public string Total { get; set; }
        public string Request { get; set; }
        public string Pending { get; set; }
        public string Status { get; set; }

    }

    public class TradTicketStatusListDto 
    {
        public int TradeTicketNo { get; set; }
        public DateTime Date { get; set; }
        public int RequestedQty { get; set; }
        public int SaudaBookedQty { get; set; }
        public int OpenQty { get; set; }

    }

    public class AddorUpdateTradeTicketInputDto
    {
        public int ContractTypeId { get; set; }
        public int BookingTypeId { get; set; }
        public int MeterialTypeId { get; set; }
        public int OilTypeId { get; set; }
        public decimal OilCost { get; set; }
        public decimal ProcessCost { get; set; }
        public string Proportion { get; set; }
        public string ContractQuantity { get; set; }
        public string UOM { get; set; }
        public int PlantOrVendor { get; set; }
        public DateTime ContractDate { get; set; } 
        public DateTime ValidityFrom { get; set; }
        public DateTime ValidaityTo { get; set; }
        public decimal OtherElements { get; set; }
    }


    public class CompetitiveAnalysisOutputDto
    {
        public string SkuName { get; set; }
        public string ADANIMOP { get; set; } 
        public string ADANISR { get; set; }
        public string PatanjaliMOP { get; set; }
        public string PatanjaliSR { get; set; } 
        public string EmamiPrice { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public int Margin { get; set; }
        public string Decision { get; set; }
    }
}


