using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class ScheduleDemoUser : Auditable
    {
        [Required]
        public long DemoUserId { get; set; }
        [Required]
        public long SubmittedFormId { get; set; }
        public long? DependentMasterFormId { get; set; }
        [Required]
        public DateTime DemoDate { get; set; }
        public bool IsActive { get; set; }
        public long DemoInchargeId { get; set; }
        public virtual User DemoUser { get; set; }
        public virtual SubmittedForm SubmittedForm { get; set; }
    }
}
