using System;

namespace Adani.Solution.DTO
{
    public class AccountStatementDto
    {
        public DateTime StatementDate { get; set; }
        public DateTime DurationDate { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal BankGuarantee { get; set; }
        public bool IsActive { get; set; }
    }
}
