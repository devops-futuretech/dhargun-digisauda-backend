using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestDetailDto
    {
        public long Id { get; set; }
        public long LiftingRequestId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SaudaOrderId { get; set; }
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityInMT { get; set; }
        public long ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string EnquiryNo { get; set; }
        public decimal FinalRate { get; set; }
    }

    public class LiftingRequestNotificationDto
    {
        public LiftingRequestNotificationDto()
        {
            LiftingRequestSkuDetails = new List<LiftingRequestSkuDto>();
        }
        
        public string APPIndentNo { get; set; }        
        public DateTime APPIndentNoCreatedDateTime { get; set; }

        public string BillToPartyName { get; set; }
        public string BillToPartyPlace { get; set; }
        public string ShipToPartyName { get; set; }
        public string ShipToPartyPlace { get; set; }
        public string RemarksFromApp { get; set; }
        public long UserId { get; set; }
        public long CreatedBy { get; set; }
        public string LiftingRequestNumber { get; set; }

        public List<LiftingRequestSkuDto> LiftingRequestSkuDetails { get; set; }
    }

    public class LiftingRequestSkuDto
    {
        public long ItemLine { get; set; }
        public string Sku { get; set; }
        public decimal QtyInCase { get; set; }
        public string UOM { get; set; }
    }
}
