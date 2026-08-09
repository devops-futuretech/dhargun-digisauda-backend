using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CompetitorAnalysisApprovalDto
    {
        public long CompetitorAnalysisId { get; set; }
        public long RequestedBy { get; set; }
        public long RequestedTo { get; set; }
        public long ApprovedBy { get; set; }
        public long? StatusId { get; set; }
        public int LoginUserId { get; set; }
        public decimal Margin { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
