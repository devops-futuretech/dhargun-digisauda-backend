using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SpecialRateApprovalOutputDto
    {
        public long Id { get; set; }
        public long LoginUserId { get; set; }
        public long UserId { get; set; }
        public string DealerName { get; set; }
        public string OilTypeName { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityCase { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal SpecialPrice { get; set; }
        public string FreightRoute { get; set; }
        public string IncoTerms { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long CreatedById { get; set; }
        public bool HasAccessToProceed { get; set; }
        public int ApprovalsCount { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedTo { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsLTD { get; set; }
        public string LTD_SR { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CounterBidNotificationSku
    {
        public long UserId { get; set; }
        public long SkuId { get; set; }
        public string Sku { get; set; }
        public decimal counterBidOffer { get; set; }
        public string MobileNumber { get; set; }
    }
}
