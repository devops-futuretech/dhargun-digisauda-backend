using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HBCLooseDiscountDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long HBCLooseDisocuntId { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public long PlantId { get; set; }
        public string Plant { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long OilTypeId { get; set; }
        public decimal Quantity { get; set; }
        public string OilType { get; set; }
        public long PackGroupId { get; set; }
        public string PackGroup { get; set; }
        public decimal Discount { get; set; }

        public long StatusId { get; set; }
        public string Status { get; set; }
        public long? RequestedById { get; set; }
        public string RequestedByUserName { get; set; }
        public DateTime RequestedOn { get; set; }
        public long? RequestedToId { get; set; }
        public string RequestedToUserName { get; set; }
        public long? ApprovedById { get; set; }
        public string ApprovedByUserName { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Remarks { get; set; }
        public long LoginUserId { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
