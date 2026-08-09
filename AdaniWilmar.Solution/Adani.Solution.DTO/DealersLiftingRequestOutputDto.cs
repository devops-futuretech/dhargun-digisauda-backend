using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealersLiftingRequestOutputDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long LiftingRequestId { get; set; }
        public string LiftingRequestNumber { get; set; }
        public DateTime LiftingRequestdate { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string CreatedUser { get; set; }
        public bool IsApproved { get; set; }
        public string Remarks { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public bool HasChildren { get; set; }
        public long? ShipToPartyId { get; set; }
        public string ShipToParty { get; set; }
        public bool IsCreatedBy { get; set; }
    }
}
