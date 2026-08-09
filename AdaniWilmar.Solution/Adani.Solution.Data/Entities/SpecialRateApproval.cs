using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class SpecialRateApproval : Auditable
    {
        public long SpecialRateId { get; set; }
        public long RequestedBy { get; set; }
        public long RequestedTo { get; set; }
        public long ApprovedBy { get; set; }
        public long? StatusId { get; set; }
        public string Remarks { get; set; }

        public virtual Status Status { get; set; }
        public virtual SpecialRate SpecialRate { get; set; }
    }
}
