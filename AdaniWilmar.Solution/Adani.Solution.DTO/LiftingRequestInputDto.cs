using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestInputDto
    {
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public long ShipToPartyId { get; set; }
        public string LiftingRequestNumber { get; set; }
        public string LiftingDate { get; set; }
        public string TradeTicketNumber { get; set; }
        public int StatusId { get; set; }
        public string CustomerRemarks { get; set; }
        public long VehicleSizeId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public List<LiftingRequestDetailInputDto> LiftingRequestDetails { get; set; }
        public decimal QantityInCase { get; set; }

        public LiftingRequestInputDto()
        {
            LiftingRequestDetails = new List<LiftingRequestDetailInputDto>();
        }
    }

}
