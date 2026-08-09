using System;


namespace Adani.Solution.DTO
{
    public class CreditNoteDto
    {
        public DateTime CreditNoteDate { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
