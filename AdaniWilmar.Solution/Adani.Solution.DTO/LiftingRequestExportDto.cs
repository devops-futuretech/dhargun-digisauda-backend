using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestExportDto
    {
        public LiftingRequestExportDto()
        {
            SaudaOrderLiftingRequest = new List<SaudaOrderLiftingRequestDto>();
        }

        public string DealerName { get; set; }
        public long LiftingRequestId { get; set; }
        public string LiftingRequestNumber { get; set; }
        public DateTime LiftingRequestdate { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantityInCase { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SaudaOrderId { get; set; }
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public string DeliveryOrderNumber { get; set; }

        public string CreatedUser { get; set; }
        public bool IsApproved { get; set; }
        public string ApproverRemarks { get; set; }
        public string CustomerRemarks { get; set; }
        public string StatusName { get; set; }

        public long? ShipToPartyId { get; set; }
        public string ShipToPartyName { get; set; }
        public string ShipToPartyCode { get; set; }
        public string EnquiryNumber { get; set; }
        public string EnquiryRemarks { get; set; }

        public List<SaudaOrderLiftingRequestDto> SaudaOrderLiftingRequest { get; set; }
    }

    public class LiftingRequestExportLists
    {
        public long? ShipToPartyId { get; set; }
        public string ShipToPartyName { get; set; }
        public string ShipToPartyCode { get; set; }
        public string LiftingRequestNumber { get; set; }
        public DateTime LiftingRequestdate { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OilType { get; set; }
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public string ApproverRemarks { get; set; }
        public string CustomerRemarks { get; set; }
        public string EnquiryNumber { get; set; }
        public string EnquiryRemarks { get; set; }
        public string StatusName { get; set; }
        public string DealerName { get; set; }
        public List<LiftingRequestDetailsOutputDto> SkuDetails { get; set; }
        
    }
}
