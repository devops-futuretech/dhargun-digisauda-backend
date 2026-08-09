using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestDto : IAPIInputDTO
    {
        public long SaudaId { get; set; }
        public string SaudaNumber { get; set; }
        public long LiftingId { get; set; }
        public string LiftingNumber { get; set; }
        public DateTime LiftingDate { get; set; }
        public int StatusId { get; set; }
        public string LiftingStatus { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public string TradeTicketNumber { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedUser { get; set; }
        public int LiftingFlag { get; set; }
        public decimal TotalQuantity { get; set; }
        public string Status { get; set; }
        public decimal VehicleSize { get; set; }

        public long? ShipToPartyId { get; set; }
        public string ShipToParty { get; set; }

        public string Remarks { get; set; }
        public string CustomerRemarks { get; set; }
        public string EnquiryRemarks { get; set; }
        public string EnquiryNumber { get; set; }
        public string SAPDeliveryNo { get; set; }
        public string SAPDocumentNo { get; set; }
        

        public List<LiftingRequestDetailDto> LiftingRequestDetailList { get; set; }
        public List<LiftingDetailGroupingDto> LiftingDetailGroupingList { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string PlantName { get; set; }
        public string DepotName { get; set; }
        public LiftingRequestDto()
        {
            LiftingRequestDetailList = new List<LiftingRequestDetailDto>();
        }
    }
}
