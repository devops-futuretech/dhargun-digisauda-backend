using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class QuestionMaster : Auditable
    {
        public QuestionMaster()
        {
            this.FormQuestions = new HashSet<FormQuestion>();
            this.AnswerOptions = new HashSet<AnswerOption>();
        }
        [Required, MaxLength(4000)]
        public string Query { get; set; }
        public string Textlength { get; set; }
        public string QueryIdentifer { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
        //[Required]
        //public long QuestionSectionId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public bool IsMandatory { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public int? OrderId { get; set; }
        public virtual QuestionType QuestionType { get; set; }

        public virtual ICollection<FormQuestion> FormQuestions { get; set; }
        public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
    }
}
