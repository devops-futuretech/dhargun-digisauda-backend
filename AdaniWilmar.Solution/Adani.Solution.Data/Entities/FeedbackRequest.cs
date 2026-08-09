using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class FeedbackRequest:Auditable
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long FeedbackTypeId { get; set; }
        [Required]
        public string Details { get; set; }

        public virtual FeedbackType FeedbackType { get; set; }
        public virtual User User { get; set; }
    }
}
