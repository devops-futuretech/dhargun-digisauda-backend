using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Answers : Auditable
    {
        [Required]
        public long QuestionId { get; set; }
        public string Answer { get; set; }
        public virtual Questions Question { get; set; }
    }
}
