
using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class CustomerLedgerDto
    {
        public decimal CurrentBalance  { get; set; }
        public int TransactionType  { get; set; }
        public List<CustomerLedgerlist> customerLedger { get; set; }
        public CustomerLedgerDto()
        {
            customerLedger = new List<CustomerLedgerlist>();
        }
    }

    public class CustomerLedgerlist
    {
        public decimal TransactionAmount { get; set; }
        public int TransactionType { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Reference { get; set; }
    }

    public class CustomerLedgerUsersList
    {
        public string CustomerLedgerUserName { get; set; }
        public long CustomerLedgerUserId { get; set; }
        public string  CustomerLedgerUserCode { get; set; }
        public decimal UserOutStandingBalance { get; set; }
        public int TransactionType { get; set; }
    }

    public class CustomerLedgerRolewiseDto
    {
        public decimal TotalOutStandingBalance { get; set; }
        public int TransactionType { get; set; }
        public List<CustomerLedgerUsersList> customerLedger { get; set; }
        public CustomerLedgerRolewiseDto()
        {
            customerLedger = new List<CustomerLedgerUsersList>();
        }
    }
    
}
