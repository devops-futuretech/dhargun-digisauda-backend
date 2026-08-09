using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestOutputDto
    {
        public string EncryptedId { get; set; }
        public string PlantName { get; set; }
        public string PlantCode { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long LiftingRequestId { get; set; }
        public string LiftingRequestNumber { get; set; }
        public DateTime LiftingRequestdate { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal RequestedQuantityInCase { get; set; }
        public string CreatedUser { get; set; }
        public bool IsApproved { get; set; }
        public string Remarks { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public bool HasChildren { get; set; }
        public string CustomerRemarks { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StateId { get; set; }
        public List<long> StateIds { get; set; }

        public long? ShipToPartyId { get; set; }
        public string ShipToParty { get; set; }
        public long VerticalId { get; set; }
        public long RoleId { get; set; }
        public bool ReprocessStatusId { get; set; }
        public bool EnquiryNumberSyncFromSap { get; set; }
        public string EnquiryRemarks { get; set; }
        public string EnquiryNumber { get; set; }
        public string DeliveryOrderNumber { get; set; }

        public List<LiftingRequestDetailsOutputDto> LiftingRequestDetails { get; set; }
        public LiftingRequestOutputDto()
        {
            LiftingRequestDetails = new List<LiftingRequestDetailsOutputDto>();
        }
    }

    public class LiftingRequestListOutputDto
    {
        public int ListCount { get; set; }
        public List<LiftingRequestOutputDto> LiftingRequestOutputs { get; set; }

    }
}
