using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingSaudaUploadDto : CommonResultDto
    {
        public string PlantCode { get; set; }
        public string IncoTerms { get; set; }
        //SaudaNumber
        public string ContractNo { get; set; }
        //BiddingDate
        public DateTime SaudaDate { get; set; }
        //ValidFrom
        public DateTime ValidFrom { get; set; }
        //ValidTo
        public DateTime ValidTo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerVerticalCode { get; set; }
        //public string BrokerCode { get; set; }
        //public string BrokerVerticalCode { get; set; }
        //public string ContractQuantity { get; set; }
        //public string DispatchQuantity { get; set; }
        //BidQuantityCase
        public string PendingQuantity { get; set; }
        //BidQuantityMT
        public string PendingQuantityMT { get; set; }
        public decimal BasicRate { get; set; }
        public string PONumber { get; set; }
        public string TradeTicketNumber { get; set; }
        public string SaudaBookingType { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string SkuVerticalCode { get; set; }
        //public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    }
}
