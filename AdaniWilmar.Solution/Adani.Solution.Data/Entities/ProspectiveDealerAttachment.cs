using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class ProspectiveDealerAttachment : Auditable
    {
        public long ProspectiveDealerId { get; set; }
        public string Filename { get; set; }
        public string Attachment { get; set; }
    }
}
