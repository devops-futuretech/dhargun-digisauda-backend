using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SubmittedFormAnswerOption : Auditable
    {
        [Required]
        public long SubmittedFormQuestionId { get; set; }
        [Required]
        public long QuestionId { get; set; }
        public long? AnswerOptionId { get; set; }
        [MaxLength(1000)]
        public string Option { get; set; }
        public string TextAnswer { get; set; }
        public bool? IsYes { get; set; }
        public bool? IsSelected { get; set; }
        public string AttachmentFileName { get; set; }
        public int? MediaTypeId { get; set; }

        public virtual SubmittedFormQuestion SubmittedFormQuestion { get; set; }
        public virtual QuestionMaster Question { get; set; }
        public virtual AnswerOption AnswerOption { get; set; }
        public virtual MediaType MediaType { get; set; }
    }
}
