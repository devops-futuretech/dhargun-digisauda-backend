using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SubmittedFormQuestion : Auditable
    {
        public SubmittedFormQuestion()
        {
            this.Answers = new HashSet<SubmittedFormAnswerOption>();
        }
        [Required]
        public long SubmittedFormId { get; set; }
        [Required]
        public long QuestionId { get; set; }
        [Required, MaxLength(4000)]
        public string Query { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public string Answer { get; set; }

        public virtual SubmittedForm SubmittedForm { get; set; }
        public virtual ICollection<SubmittedFormAnswerOption> Answers { get; set; }
    }
}
