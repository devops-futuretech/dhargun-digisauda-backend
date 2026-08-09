using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class OverduePayment : Auditable
    {
        public string Reference { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime DueDate { get; set; }
        public string DocumentType { get; set; }
        public decimal Balance { get; set; }
        public long UserId { get; set; }
        public string UserCode { get; set; }
        public string CompanyCode { get; set; }
        public string Currency { get; set; }      
    }
}
