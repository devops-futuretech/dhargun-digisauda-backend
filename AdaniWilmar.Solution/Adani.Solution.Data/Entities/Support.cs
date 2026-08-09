using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Support : Auditable
    {
        [Required]
        public string Description { get; set; }
        public int IssueTypeId { get; set; }
        public int SeverityTypeId { get; set; }
        public int ModuleId { get; set; }
        public string Feature { get; set; }

        public int StatusId { get; set; }
        public int DeviceId { get; set; }
        public long StateId { get; set; }

        public virtual ICollection<SupportAttachment> SupportAttachments { get; set; }

        public Support()
        {
            SupportAttachments = new HashSet<SupportAttachment>();
        }
    }
}
