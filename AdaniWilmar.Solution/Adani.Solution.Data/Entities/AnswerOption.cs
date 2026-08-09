using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class AnswerOption : Auditable
    {
        [Required]
        public long QuestionId { get; set; }
        [Required, MaxLength(1000)]
        public string Option { get; set; }
        public bool IsDeleted { get; set; }

        public virtual QuestionMaster Question { get; set; }
    }
}
