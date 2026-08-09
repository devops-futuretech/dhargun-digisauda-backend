using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class LiftingRequest : Auditable
    {
        public string LiftingRequestNumber { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime LiftingDate { get; set; }
        [Required]
        public long UserId { get; set; }
        public int LiftingStatusId { get; set; }
        public int StatusId { get; set; }
        //public string TradeTicketNumber { get; set; }
        public long ApprovedBy { get; set; }
        public string ApproverRemarks { get; set; }
        public string CustomerRemarks { get; set; }
        //public long VehicleSizeId { get; set; }
        public bool IsSAPDataSync { get; set; }
        public long? ShipToPartyId { get; set; }
        public long PlantId { get; set; }
        //public long DepotId { get; set; }
        public string SAPDocumentNo { get; set; }
        public string SAPDeliveryNo { get; set; }
        public string SAPInvoiceNo { get; set; }
        public virtual User User { get; set; }
        public virtual User ShipToParty { get; set; }
        //public virtual VehicleLodability VehicleSize { get; set; }
        public decimal QantityInCase { get; set; }
        public DateTime ApproveDate { get; set; }
        public bool IsSAPSalesOrder { get; set; }
        public bool IsCompleted { get; set; }
    }
}
