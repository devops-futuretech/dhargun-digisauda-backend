using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class CreditNote:Auditable
    {
        [Required]
        public long UserId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreditNoteDate { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
    }
}
