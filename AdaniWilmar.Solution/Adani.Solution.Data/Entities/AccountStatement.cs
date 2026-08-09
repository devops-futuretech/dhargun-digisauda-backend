using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class AccountStatement:Auditable
    {
        [Required]
        public long UserId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime StatementDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DurationDate { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal BankGuarantee { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
    }
}
