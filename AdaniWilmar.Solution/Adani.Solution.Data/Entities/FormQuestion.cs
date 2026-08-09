using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class FormQuestion : Auditable
    {
        [Required]
        public long FormId { get; set; }
        [Required]
        public long QuestionId { get; set; }
        public long QuestionSectionId { get; set; }
        public int OrderNo { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public virtual Form Form { get; set; }
        public virtual QuestionMaster Question { get; set; }
    }
}
