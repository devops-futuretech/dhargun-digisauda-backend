using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Attachment : Auditable
    {
        public long RecordId { get; set; }
        public int PageId { get; set; }
        public string Url { get; set; }
    }
}
