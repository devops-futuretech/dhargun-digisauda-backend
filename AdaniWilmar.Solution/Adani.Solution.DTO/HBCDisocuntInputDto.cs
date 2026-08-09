using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HBCDisocuntInputDto : IAPIInputDTO
    {
        public long HBCLooseDisocuntId { get; set; }
        public long PlantId { get; set; }
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public List<long> SkuIds { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public long LoginUserId { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class HBCDisocuntListInputDto
    {
        public long HBCLooseDisocuntId { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> ZHIds { get; set; }
        public long LoginUserId { get; set; }
        public int StatusId { get; set; }
        public bool IsFromWeb { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }


    public class HBCDisocuntApprovalDto : IAPIInputDTO
    {
        public List<long> HBCLooseDisocuntIds { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
