using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Form : Auditable
    {
        public Form()
        {
            this.FormQuestions = new HashSet<FormQuestion>();
            this.ScheduleDemoUsers = new HashSet<ScheduleDemoUser>();
        }

        [Required, MaxLength(2000)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsFormStatus { get; set; }
        public string RoleIds { get; set; }
        public long? ParentFormId { get; set; }

        public virtual ICollection<FormQuestion> FormQuestions { get; set; }
        public virtual ICollection<ScheduleDemoUser> ScheduleDemoUsers { get; set; }
    }
}
