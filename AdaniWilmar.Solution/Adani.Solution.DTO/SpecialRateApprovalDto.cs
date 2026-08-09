using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SpecialRateApprovalDto
    {
        public IList<long> SpecialRateIds { get; set; }
        public long Id { get; set; }
        public string Remarks { get; set; }
        public long LoginUserId { get; set; }
        public int StatusId { get; set; }
        public long RequestedBy { get; set; }
        public long RequestedTo { get; set; }
        public long ApprovedBy { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        public SpecialRateApprovalDto()
        {
            SpecialRateIds = new List<long>();
        }
    }
}
