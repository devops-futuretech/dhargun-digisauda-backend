using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class CompetitorAnalysisApproval : Auditable
    {
        public long CompetitorAnalysisId { get; set; }        
        public long RequestedBy { get; set; }
        public long RequestedTo { get; set; }
        public long ApprovedBy { get; set; }
        public long? StatusId { get; set; }

        public virtual Status Status { get; set; }
        public virtual CompetitorAnalysis CompetitorAnalysis { get; set; }        
    }
}
